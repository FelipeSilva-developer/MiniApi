using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tela
{
    public partial class FrmInputBox : Form
    {
        public FrmInputBox()
        {
            InitializeComponent();

        }

        public string texto = "digite o ID";
        public string valor = string.Empty;
        public bool ok = false;

        private void FrmInputBox_Load(object sender, EventArgs e)
        {
            lblTitulo.Text = texto;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtInput.Text))
            {
                valor = txtInput.Text;
                ok = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Insira o ID");
                txtInput.Focus();
            }
        }
    }
}
