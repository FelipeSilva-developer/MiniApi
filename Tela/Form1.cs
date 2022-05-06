using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Tela.models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tela
{
    public partial class Form1 : Form
    {
        string URI = "https://localhost:7031/Clientes";
        int codigoCliente = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            obter();
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            inserir();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            InputBox();
            if (codigoCliente != 0)
            {
                editar(codigoCliente);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            InputBox();
            if (codigoCliente != 0)
            {
                deletar(codigoCliente);
            }
        }

        private async void obter()
        {
            
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(URI))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var ClienteJsonString = await response.Content.ReadAsStringAsync();
                         var cli = JsonConvert.DeserializeObject<List<Cliente>>(ClienteJsonString);
                        dgvDados.DataSource = cli;
                    }
                    else
                    {
                        MessageBox.Show("Não foi possível obter o produto : " + response.StatusCode);
                    }
                }
            }
        }
        private async void inserir()
        {
            Random random = new Random();
            Cliente cli = new Cliente();
            cli.id = random.Next(1, 100);
            
            using (FrmInputBox frmInputBox = new FrmInputBox())
            {
                frmInputBox.lblTitulo.Text = "Insira o Nome";
                frmInputBox.ShowDialog();

                if (frmInputBox.ok)
                {
                    cli.Nome = Convert.ToString(frmInputBox.valor);
                }
            }
            
            using (var client = new HttpClient())
            {
                var serializedCliente = JsonConvert.SerializeObject(cli);
                var content = new StringContent(serializedCliente, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(URI, content);
                if (result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cliente inserido");
                }
                else
                {
                    MessageBox.Show("Falha ao inserir : " + result.StatusCode);
                }
            }
            obter();
        }
        private async void editar(int id)
        {

            Cliente cli = new Cliente();
            cli.id = id;

            using (FrmInputBox frmInputBox = new FrmInputBox())
            {
                frmInputBox.lblTitulo.Text = "Insira o Nome";
                frmInputBox.ShowDialog();

                if (frmInputBox.ok)
                {
                    cli.Nome = Convert.ToString(frmInputBox.valor);
                }
            }

            using (var client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.PutAsJsonAsync(URI + "/" + cli.id, cli);
                if (responseMessage.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cliente atualizado");
                }
                else
                {
                    MessageBox.Show("Falha ao atualizar : " + responseMessage.StatusCode);
                }
            }
            obter();
        }
        private async void deletar(int id)
        {
            int codID = id;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URI);
                HttpResponseMessage responseMessage = await client.DeleteAsync(String.Format("{0}/{1}", URI, codID));
                if (responseMessage.IsSuccessStatusCode)
                {
                    MessageBox.Show("Produto excluído com sucesso");
                }
                else
                {
                    MessageBox.Show("Falha ao excluir o produto  : " + responseMessage.StatusCode);
                }
            }
            obter();
        }
        private void InputBox()
        {

            using(FrmInputBox frmInputBox = new FrmInputBox())
            {
                frmInputBox.lblTitulo.Text = "Insira o ID";
                frmInputBox.ShowDialog();

                if (frmInputBox.ok)
                {
                    codigoCliente = Convert.ToInt32(frmInputBox.valor);
                }
            }
        }

    }   
}       