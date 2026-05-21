using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinhaAppWinForms
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient;
        private string _tokenJwt = string.Empty;

        // Componentes da Tela
        private DataGridView _gridProdutos;
        private TextBox _txtId;
        private TextBox _txtNome;
        private TextBox _txtPreco;
        private TextBox _txtEstoque;
        private Button _btnSalvar;
        private Button _btnAtualizar;
        private Button _btnExcluir;
        private Button _btnLimpar;

        public Form1()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            
            ConfigurarJanela();
            CriarComponentes();

            // Evento executado ao abrir a tela
            this.Load += async (s, e) => 
            {
                await RealizarLoginAutomatico();
                await AtualizarGrid();
            };
        }

        private void ConfigurarJanela()
        {
            this.Text = "Gerenciamento de Produtos - Cadastro Desktop";
            this.Width = 950;
            this.Height = 550;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void CriarComponentes()
        {
            // --- GRID DE PRODUTOS (LADO ESQUERDO) ---
            _gridProdutos = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(550, 460),
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            // Evento: Quando clicar em uma linha do grid, joga os dados nos campos de texto
            _gridProdutos.SelectionChanged += GridProdutos_SelectionChanged;
            this.Controls.Add(_gridProdutos);

            // --- PAINEL DE FORMULÁRIO (LADO DIREITO) ---
            GroupBox boxForm = new GroupBox { Text = "Detalhes do Produto", Location = new Point(590, 15), Size = new Size(320, 230) };
            this.Controls.Add(boxForm);

            CriarLabel(boxForm, "ID (Automático):", 25);
            _txtId = new TextBox { Location = new Point(130, 22), Size = new Size(160, 23), ReadOnly = true, BackColor = Color.LightGray };
            boxForm.Controls.Add(_txtId);

            CriarLabel(boxForm, "Nome:", 65);
            _txtNome = new TextBox { Location = new Point(130, 62), Size = new Size(160, 23) };
            boxForm.Controls.Add(_txtNome);

            CriarLabel(boxForm, "Preço (R$):", 105);
            _txtPreco = new TextBox { Location = new Point(130, 102), Size = new Size(160, 23) };
            boxForm.Controls.Add(_txtPreco);

            CriarLabel(boxForm, "Estoque:", 145);
            _txtEstoque = new TextBox { Location = new Point(130, 142), Size = new Size(160, 23) };
            boxForm.Controls.Add(_txtEstoque);

            // --- PAINEL DE BOTÕES (ABAIXO DO FORMULÁRIO) ---
            _btnSalvar = new Button { Text = "Incluir Novo", Location = new Point(590, 260), Size = new Size(150, 40), BackColor = Color.LightGreen };
            _btnSalvar.Click += async (s, e) => await CriarProduto();
            this.Controls.Add(_btnSalvar);

            _btnAtualizar = new Button { Text = "Salvar Alteração", Location = new Point(760, 260), Size = new Size(150, 40), BackColor = Color.LightBlue };
            _btnAtualizar.Click += async (s, e) => await AtualizarProduto();
            this.Controls.Add(_btnAtualizar);

            _btnExcluir = new Button { Text = "Excluir Produto", Location = new Point(590, 315), Size = new Size(150, 40), BackColor = Color.LightCoral };
            _btnExcluir.Click += async (s, e) => await ExcluirProduto();
            this.Controls.Add(_btnExcluir);

            _btnLimpar = new Button { Text = "Limpar Campos", Location = new Point(760, 315), Size = new Size(150, 40) };
            _btnLimpar.Click += (s, e) => LimparCampos();
            this.Controls.Add(_btnLimpar);
        }

        private void CriarLabel(GroupBox container, string texto, int posY)
        {
            Label lbl = new Label { Text = texto, Location = new Point(15, posY), Size = new Size(110, 20), Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            container.Controls.Add(lbl);
        }

        // --- LÓGICA DE INTEGRAÇÃO COM A API ---

        private async Task RealizarLoginAutomatico()
        {
            try
            {
                var dadosLogin = new { usuario = "admin", senha = "admin123" };
                var json = JsonSerializer.Serialize(dadosLogin);
                var conteudo = new StringContent(json, Encoding.UTF8, "application/json");

                var resposta = await _httpClient.PostAsync("Auth/login", conteudo);
                if (resposta.IsSuccessStatusCode)
                {
                    var respostaJson = await resposta.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(respostaJson))
                    {
                        _tokenJwt = doc.RootElement.GetProperty("token").GetString();
                        // Injeta o Token JWT no cabeçalho padrão de todas as requisições seguintes
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenJwt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha na autenticação em segundo plano: {ex.Message}", "Aviso de Segurança", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task AspNetCoreGet() // Método auxiliar para atualizar a Grid
        {
            var resposta = await _httpClient.GetAsync("Produto?pageNumber=1&pageSize=50");
            if (resposta.IsSuccessStatusCode)
            {
                var stream = await resposta.Content.ReadAsStreamAsync();
                var opcoes = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var produtos = await JsonSerializer.DeserializeAsync<List<ProdutoExibicao>>(stream, opcoes);
                
                _gridProdutos.DataSource = null;
                _gridProdutos.DataSource = produtos;
            }
        }

        private async Task AtualizarGrid() => await AspNetCoreGet();

        private async Task CriarProduto()
        {
            try
            {
                var dto = ObterObjetoDoForm();
                var json = JsonSerializer.Serialize(dto);
                var conteudo = new StringContent(json, Encoding.UTF8, "application/json");

                var resposta = await _httpClient.PostAsync("Produto", conteudo);
                if (resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show("Produto criado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    await AtualizarGrid();
                }
                else
                {
                    var erro = await resposta.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro da API: {erro}", "Erro ao Criar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro interno: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task AtualizarProduto()
        {
            if (string.IsNullOrEmpty(_txtId.Text)) return;

            try
            {
                var id = int.Parse(_txtId.Text);
                var dto = ObterObjetoDoForm();
                var json = JsonSerializer.Serialize(dto);
                var conteudo = new StringContent(json, Encoding.UTF8, "application/json");

                var resposta = await _httpClient.PutAsync($"Produto/{id}", conteudo);
                if (resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show("Produto atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await AtualizarGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ExcluirProduto()
        {
            if (string.IsNullOrEmpty(_txtId.Text)) return;

            var confirmacao = MessageBox.Show("Tem certeza que deseja excluir este produto?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacao == DialogResult.No) return;

            try
            {
                var id = int.Parse(_txtId.Text);
                var resposta = await _httpClient.DeleteAsync($"Produto/{id}");
                if (resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show("Produto excluído!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    await AtualizarGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- MÉTODOS AUXILIARES DE INTERAÇÃO DA TELA ---

        private void GridProdutos_SelectionChanged(object sender, EventArgs e)
        {
            if (_gridProdutos.SelectedRows.Count > 0)
            {
                var linha = _gridProdutos.SelectedRows[0];
                _txtId.Text = linha.Cells["Id"].Value?.ToString();
                _txtNome.Text = linha.Cells["Nome"].Value?.ToString();
                _txtPreco.Text = linha.Cells["Preco"].Value?.ToString();
                _txtEstoque.Text = linha.Cells["Estoque"].Value?.ToString();
            }
        }

        private object ObterObjetoDoForm()
        {
            return new
            {
                nome = _txtNome.Text,
                preco = double.TryParse(_txtPreco.Text, out double p) ? p : 0,
                estoque = int.TryParse(_txtEstoque.Text, out int e) ? e : 0,
                categoriaId = 1 // Passando categoria fixa ID 1 para simplificar o escopo do teste
            };
        }

        private void LimparCampos()
        {
            _txtId.Clear();
            _txtNome.Clear();
            _txtPreco.Clear();
            _txtEstoque.Clear();
            if (_gridProdutos.SelectedRows.Count > 0) _gridProdutos.ClearSelection();
        }
    }
    public class ProdutoExibicao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }
        public int Estoque { get; set; }
    }
}