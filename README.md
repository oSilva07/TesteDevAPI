# 📌 API RESTful e Cliente WinForms - Teste Técnico

Este projeto consiste na criação de uma API RESTful em C# com ASP.NET Core e SQLite, além de um aplicativo WinForms que consome essa API. O objetivo é aplicar boas práticas de desenvolvimento de software para garantir eficiência, segurança, escalabilidade e manutenibilidade.

## 🔧 Tecnologias Utilizadas

* **ASP.NET Core:** Desenvolvimento da API Web.
* **Entity Framework Core:** ORM para persistência de dados.
* **SQLite:** Banco de dados leve para armazenamento.
* **JWT (JSON Web Token):** Autenticação e segurança de endpoints.
* **WinForms:** Interface gráfica (.NET 8) para consumo da API.
* **HttpClient:** Consumo assíncrono da API no cliente desktop.
* **ILogger:** Monitoramento e logs de execução.
* **xUnit & Moq:** Testes unitários com isolamento de dependências.

## 📂 Estrutura do Projeto
A solução foi organizada para separar as responsabilidades de forma clara, utilizando o padrão de injeção de dependências e arquitetura em camadas (Controllers, Services, Repositories e DTOs).

```
📁 TesteDevAPI/
├── 📁 MinhaApiComSQLite (Projeto da API)
│   │── 📁 .postman
│   │── 📁 .vscode
│   │── 📁 bin \ Debug \ net8.0
│   │── 📁 Controllers
│   │── 📁 Data
│   │── 📁 DTOs
│   │── 📁 Migrations
│   │── 📁 Models
│   │── 📁 obj
│   │── 📁 postman
│   │── 📁 Properties
│   │── 📁 Repositories
│   │── 📁 Services
│   │── appsettings.Development.json
│   │── appsettings.json
│   │── MinhaApiComSQLite.csproj
│   │── MinhaApiComSQLite.http
│   │── produtos.db
│   │── produtos.db-shm
│   │── produtos.db-wal
│   │── Program.cs
│   │── Startup.cs
│
├── 📁 MinhaApiComSQLite.Tests
│   │── UnitTest1.cs
│   │── MinhaApiComSQLite.Tests.csproj
│
└── 📁 MinhaAppWinForms (Projeto Cliente)
│   │── Form1.cs
│   │── Form1.Designer.cs
│   │── MinhaAppWinForms.csproj
│   │── MinhaAppWinForms.csproj.user
    │── Program.cs
```

## 🚀 Como Executar o Projeto
### 1️⃣ Clonando o Repositório
```
git clone <URL_DO_REPOSITORIO>
cd TesteDevAPI
```

### 2️⃣ Rodando a API (Backend)
O banco de dados SQLite é gerado e atualizado automaticamente via Entity Framework.
Abra um terminal na pasta da API e execute:

```
cd MinhaApiComSQLite
dotnet run
```
A API estará disponível por padrão em https://localhost:5001 ou http://localhost:5000.

### 3️⃣ Rodando o Cliente WinForms (Frontend)
Abra um novo terminal na raiz do repositório e execute:

```
dotnet run --project MinhaAppWinForms\MinhaAppWinForms.csproj
```

### 4️⃣ Rodando os Testes Unitários
Para validar as regras de negócio via xUnit, execute na raiz do repositório:
```
dotnet test
```

## 📌 Funcionalidades Implementadas

### API
```
✅ CRUD de Produtos e Categorias: Isolado em Services e Repositories.
✅ Autenticação via JWT: Endpoints de alteração protegidos (Diferencial concluído).
✅ Paginação de produtos: Utilizando Skip e Take direto no banco.
✅ Registro de logs: Utilizando ILogger nas Controllers (Diferencial concluído).
✅ Histórico de preços: Rastreamento de alterações de valor (Diferencial concluído).
✅ Relatórios e Estatísticas: Endpoint dedicado para métricas do estoque (Diferencial concluído).
✅ Regras de Negócio Avançadas: Validação de preço positivo e primeira letra maiúscula garantidos por testes unitários.
```

### Aplicação WinForms
```
✅ Interface gráfica com DataGridView.
✅ Formulário completo integrado com os botões para Criar, Atualizar e Excluir produtos.
✅ Consumo da API com HttpClient.
✅ Autenticação JWT injetada nativamente nas requisições.
✅ Uso de DTOs/Models para manipulação de dados no Grid.
```

## 📜 Exemplos de Requisição
Como a API é protegida, os endpoints de escrita exigem autenticação.

1. Gerar Token (POST)
```
POST /api/auth/login
Content-Type: application/json

{
  "usuario": "admin",
  "senha": "admin123"
}
```

2. Criar Produto (POST)
```
POST /api/produto
Authorization: Bearer {SEU_TOKEN_AQUI}
Content-Type: application/json

{
  "nome": "Monitor Ultrawide",
  "preco": 1250.00,
  "estoque": 10,
  "categoriaId": 1
}
```

3. Listar Produtos Paginados (GET - Público)
```
GET /api/produto?pageNumber=1&pageSize=10
```
