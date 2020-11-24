using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Oracle_Tools
{
    public partial class frmScriptAgendado : Form
    {
        private csOracle _csOracle = new csOracle();
        private csListViewColumnSorter lvwColumnSorter;
        private csAgendaDeScripts _csAgendaDeScripts;
        private string _CaminhoArquivoAgenda = csUtil.PastaLocalExecutavel() + "AgendaDeScripts.txt";

        public frmScriptAgendado()
        {
            InitializeComponent();
            lvwColumnSorter = new csListViewColumnSorter();
            lvwAgendamentos.ListViewItemSorter = lvwColumnSorter;
            lvwColumnSorter.SortColumn = 0;
            _csAgendaDeScripts = new csAgendaDeScripts(_CaminhoArquivoAgenda);
            this.AtualizarListaAgendamentos();
        }

        private void btSelecionarBanco_Click(object sender, EventArgs e)
        {
            string _Username = "";
            string _Password = "";
            string _Database = "";

            frmLoginORACLE FormLogin = new frmLoginORACLE(false, ref _Username, ref _Password, ref _Database);

            if (_Username.Length > 0 && _Password.Length > 0 && _Database.Length > 0)
            {
                if (!_csOracle.ConectouNoBanco(_Username, _Password, _Database))
                {
                    DialogResult _Resp = MessageBox.Show("Não foi possível conectar no banco com os dados fornecidos. Deseja mesmo assim manter estes dados?", "Selecionar banco de dados", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (_Resp == DialogResult.Yes)
                    {
                        txtQualBanco.Text = _Username + "@" + _Database;
                        csDadosConexao DadosBanco = new csDadosConexao();
                        DadosBanco.Usuario = _Username;
                        DadosBanco.Senha = _Password;
                        DadosBanco.Database = _Database;
                        txtQualBanco.Tag = DadosBanco;
                    }
                    else
                    {
                        txtQualBanco.Text = "";
                        txtQualBanco.Tag = null;
                    }
                }
                else
                {
                    txtQualBanco.Text = _Username + "@" + _Database;
                    csDadosConexao DadosBanco = new csDadosConexao();
                    DadosBanco.Usuario = _Username;
                    DadosBanco.Senha = _Password;
                    DadosBanco.Database = _Database;
                    txtQualBanco.Tag = DadosBanco;
                }
            }
            else
            {
                txtQualBanco.Text = "";
                txtQualBanco.Tag = null;
            }

        }

        private void btSelecionarArquivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgAbrir = new OpenFileDialog();
            DialogResult _Resp = DialogResult.None;

            dlgAbrir.Title = "Selecione o arquivo de script";
            dlgAbrir.Filter = "Arquivos SQL (*.sql)|*.sql|Arquivos TXT (*.txt)|*.txt|Todos os arquivos (*.*)|*.*";
            dlgAbrir.Multiselect = false;
            _Resp = dlgAbrir.ShowDialog();
            if (_Resp == System.Windows.Forms.DialogResult.OK)
            {
                txtQualScript.Text = dlgAbrir.FileName;
            }

        }

        private void txtQualBanco_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                txtQualBanco.Text = "";
                txtQualBanco.Tag = null;
            }
            e.Handled = true;
        }

        private void btAgendar_Click(object sender, EventArgs e)
        {
            csScriptAgendado _csScriptAgendado = new csScriptAgendado();
            _csScriptAgendado.DataHora = dtDataHora.Value;
            _csScriptAgendado.DadosBanco = (csDadosConexao)txtQualBanco.Tag;
            _csScriptAgendado.CaminhoScript = txtQualScript.Text;
            _csScriptAgendado.HeUmaQuery = optQuery.Checked;
            _csAgendaDeScripts.AdicionarNaAgenda(_csScriptAgendado);
            if (_csAgendaDeScripts.SalvouAgenda())
            {
                this.AdicionarNaLista(_csScriptAgendado);
                txtQualBanco.Text = "";
                txtQualScript.Text = "";
            }
        }

        private void AtualizarListaAgendamentos()
        {
            csScriptAgendado _csScriptAgendado;

            lvwAgendamentos.Items.Clear();
            for (int i = 0; i < _csAgendaDeScripts.ListaScriptsAgendados.Count; i++)
            {
                _csScriptAgendado = (csScriptAgendado)_csAgendaDeScripts.ListaScriptsAgendados[i];
                this.AdicionarNaLista(_csScriptAgendado);
            }
        }

        private void AdicionarNaLista(csScriptAgendado p_csScriptAgendado)
        {
            ListViewItem lvwItem = null;

            lvwItem = lvwAgendamentos.Items.Add(p_csScriptAgendado.DataHora.ToString("dd/MM/yy HH:mm"));
            lvwItem.SubItems.Add(p_csScriptAgendado.DadosBanco.Usuario + "@" + p_csScriptAgendado.DadosBanco.Database);
            lvwItem.SubItems.Add(p_csScriptAgendado.CaminhoScript);
            if (p_csScriptAgendado.HeUmaQuery)
            {
                lvwItem.SubItems.Add("Query");
            }
            else
            {
                lvwItem.SubItems.Add("No Query");
            }
            lvwItem.Tag = p_csScriptAgendado;
        }

        private void lvwAgendamentos_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvwAgendamentos.Sort();
        }

        private void btIniciarParar_Click(object sender, EventArgs e)
        {
            tmrProcura.Enabled = true;
            TimeSpan UmMinuto = new TimeSpan(0, 0, 1, 0);
            lblStatus.Text = "Próxima procura em " + (DateTime.Now + UmMinuto).ToString("dd/MM/yy HH:mm:ss");
        }

        private void tmrProcura_Tick(object sender, EventArgs e)
        {
            TimeSpan UmMinuto = new TimeSpan(0, 0, 1, 0);
            lblStatus.Text = "Próxima procura em " + (DateTime.Now + UmMinuto).ToString("dd/MM/yy HH:mm:ss");
        }
    }
}
