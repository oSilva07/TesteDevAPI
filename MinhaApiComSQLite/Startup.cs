#region assembly Microsoft.AspNetCore.Mvc.NewtonsoftJson, Version=5.0.14.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// C:\Users\isaias.gomes\.nuget\packages\microsoft.aspnetcore.mvc.newtonsoftjson\5.0.14\lib\net5.0\Microsoft.AspNetCore.Mvc.NewtonsoftJson.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text;
using System;
using Microsoft.AspNetCore.Mvc;
using MinhaApiComSQLite.Data;
using Microsoft.EntityFrameworkCore;
using MinhaApiComSQLite.Repositories;
using MinhaApiComSQLite.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MinhaApiComSQLite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Método chamado pela ASP.NET Core para adicionar serviços ao container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<MinhaApiComSQLite.Services.TokenService>();
            // Configurar banco de dados SQLite
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            // Configurar o Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Minha API com SQLite",
                    Version = "v1",
                    Description = "API para gerenciar produtos com SQLite",
                    Contact = new OpenApiContact
                    {
                        Name = "Seu Nome",
                        Email = "seu.email@exemplo.com",
                        Url = new Uri("https://seuwebsite.com")
                    }
                });
            });
            var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Secret"]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Configurar controladores e endpoints
            services.AddControllers()
                .AddNewtonsoftJson(options => 
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // Registrando Repositories (Interface + Classe)
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IHistoricoPrecoRepository, HistoricoPrecoRepository>();

            // Registrando Services (Interface + Classe)
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProdutoService, ProdutoService>();
        }

        // Método chamado pela ASP.NET Core para configurar o pipeline HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Configurar Swagger apenas em desenvolvimento
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API com SQLite v1");
                    options.RoutePrefix = ""; // Swagger abre na URL raiz
                });
            }

            app.UseHttpsRedirection(); // Força HTTPS
            app.UseRouting();          // Habilita o roteamento
            app.UseAuthentication();   // Habilita a autenticação
            app.UseAuthorization();    // Habilita a autorização

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Mapeia controladores
            });
        }
    }
}
