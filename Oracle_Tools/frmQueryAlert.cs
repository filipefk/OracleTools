using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using System.Text;

namespace Oracle_Tools
{
    public partial class frmQueryAlert : Form
    {

    #region Campos privados

        private bool _Executar = false;
        private string _Username = "";
        private string _Password = "";
        private string _Database = "";
        private csOracle _csOracle = new csOracle();

    #endregion

    #region Propriedades

        /// <summary>
        /// Retorna se tem os parâmetros de conexão definidos
        /// </summary>
        public bool EstaConectado
        {
            get
            {
                return (_Username.Length > 0 && _Password.Length > 0 && _Database.Length > 0);
            }
        }

    #endregion

    #region Construtores

        public frmQueryAlert()
        {
            InitializeComponent();
            lblTimerInterval.Text = tmrExecuta.Interval.ToString("#,###");
            _Username = "";
            _Password = "";
            _Database = "";
            this.Text = "Query Alert - NÃO CONECTADO";
        }

        public frmQueryAlert(string p_Username, string p_Password, string p_Database)
        {
            InitializeComponent();
            lblTimerInterval.Text = tmrExecuta.Interval.ToString("#,###");
            _Username = p_Username;
            _Password = p_Password;
            _Database = p_Database;

            string _Mensagem = "";
            string _InfoBanco = "";
            _InfoBanco = _csOracle.InfoBanco(_Username, _Password, _Database, ref _Mensagem);
        }

    #endregion

    #region Eventos dos controles

        private void btExecutar_Click(object sender, EventArgs e)
        {
            this.IniciarExecucao();
        }

        private void btParar_Click(object sender, EventArgs e)
        {
            this.PararExecucao();
        }

        private void tmrExecuta_Tick(object sender, EventArgs e)
        {
            tmrExecuta.Enabled = false;
            lblExecutando.Text = "Desativado";
            this.ExecutaQuery();
            tmrExecuta.Enabled = _Executar;
            this.LayoutAguardando();
        }

        private void txtTempo_TextChanged(object sender, EventArgs e)
        {
            lblTimerInterval.ForeColor = Color.Black;
            try
            {
                tmrExecuta.Interval = Int32.Parse(txtTempo.Text);
                lblTimerInterval.Text = tmrExecuta.Interval.ToString("#,###");
            }
            catch (Exception)
            {
                lblTimerInterval.Text = tmrExecuta.Interval.ToString("#,###");
                lblTimerInterval.ForeColor = Color.Red;
                return;
            }
        }

        private void txtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    txtQuery.SelectAll();
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void mnuInfoBanco_Click(object sender, EventArgs e)
        {
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }
            frmInfoBanco _frmInfoBanco = new frmInfoBanco(_Username, _Password, _Database);
            _frmInfoBanco.Close();
            _frmInfoBanco.Dispose();
            GC.Collect();
        }

    #endregion

    #region Métodos Privados

        private void IniciarExecucao()
        {
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }
            _Executar = true;
            tmrExecuta.Enabled = _Executar;
            this.LayoutAguardando();
        }

        private void PararExecucao()
        {
            _Executar = false;
            tmrExecuta.Enabled = _Executar;
            this.LayoutAguardando();
        }

        private void LimpaListView(ListView p_ListView)
        {
            p_ListView.Items.Clear();
            p_ListView.Columns.Clear();
        }

        private void CriaCabecalhosListView(ListView p_ListView, OracleDataReader _DataReader)
        {
            for (int i = 0; i < _DataReader.FieldCount; i++)
            {
                p_ListView.Columns.Add(_DataReader.GetName(i));
            }
        }

        private void MostraResultadoListView(ListView p_ListView, OracleDataReader _DataReader)
        {
            ListViewItem lvwItem = null;
            this.LimpaListView(p_ListView);
            this.CriaCabecalhosListView(p_ListView, _DataReader);

            for (int i = 0; i < _DataReader.FieldCount; i++)
            {
                if (_DataReader.IsDBNull(i))
                {
                    if (i == 0)
                    {
                        lvwItem = p_ListView.Items.Add("NULL");
                    }
                    else
                    {
                        lvwItem.SubItems.Add("NULL");
                    }
                }
                else
                {
                    switch (_DataReader.GetFieldType(i).Name.Trim())
                    {
                        case "DateTime":
                            if (i == 0)
                            {
                                lvwItem = p_ListView.Items.Add(_DataReader.GetDateTime(i).ToString("dd/MM/yy HH:mm:ss"));
                            }
                            else
                            {
                                lvwItem.SubItems.Add(_DataReader.GetDateTime(i).ToString("dd/MM/yy HH:mm:ss"));
                            }
                            break;
                        case "Decimal":
                            if (i == 0)
                            {
                                lvwItem = p_ListView.Items.Add(_DataReader.GetDecimal(i).ToString());
                            }
                            else
                            {
                                lvwItem.SubItems.Add(_DataReader.GetDecimal(i).ToString());
                            }
                            break;
                        case "String":
                            if (i == 0)
                            {
                                lvwItem = p_ListView.Items.Add(_DataReader.GetString(i));
                            }
                            else
                            {
                                lvwItem.SubItems.Add(_DataReader.GetString(i));
                            }
                            break;
                        default:
                            if (i == 0)
                            {
                                lvwItem = p_ListView.Items.Add(_DataReader.GetFieldType(i).Name.Trim());
                            }
                            else
                            {
                                lvwItem.SubItems.Add(_DataReader.GetFieldType(i).Name.Trim());
                            }
                            break;
                    }
                }
            }

            
            while (_DataReader.Read())
            {
                for (int i = 0; i < _DataReader.FieldCount; i++)
                {
                    if (_DataReader.IsDBNull(i))
                    {
                        if (i == 0)
                        {
                            lvwItem = p_ListView.Items.Add("NULL");
                        }
                        else
                        {
                            lvwItem.SubItems.Add("NULL");
                        }
                    }
                    else
                    {
                        switch (_DataReader.GetFieldType(i).Name.Trim())
                        {
                            case "DateTime":
                                if (i == 0)
                                {
                                    lvwItem = p_ListView.Items.Add(_DataReader.GetDateTime(i).ToString("dd/MM/yy HH:mm:ss"));
                                }
                                else
                                {
                                    lvwItem.SubItems.Add(_DataReader.GetDateTime(i).ToString("dd/MM/yy HH:mm:ss"));
                                }
                                break;
                            case "Decimal":
                                if (i == 0)
                                {
                                    lvwItem = p_ListView.Items.Add(_DataReader.GetDecimal(i).ToString());
                                }
                                else
                                {
                                    lvwItem.SubItems.Add(_DataReader.GetDecimal(i).ToString());
                                }
                                break;
                            case "String":
                                if (i == 0)
                                {
                                    lvwItem = p_ListView.Items.Add(_DataReader.GetString(i));
                                }
                                else
                                {
                                    lvwItem.SubItems.Add(_DataReader.GetString(i));
                                }
                                break;
                            default:
                                if (i == 0)
                                {
                                    lvwItem = p_ListView.Items.Add(_DataReader.GetFieldType(i).Name.Trim());
                                }
                                else
                                {
                                    lvwItem.SubItems.Add(_DataReader.GetFieldType(i).Name.Trim());
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void ExecutaQuery()
        {
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }

            OracleConnection con = new OracleConnection();
            bool _LeuAlgumaCoisa = false;

            this.LayoutExecutando();
            
            if (txtQuery.Text == "")
            {
                this.PararExecucao();
                return;
            }
            try
            {
                txtMensagens.Text = "Última execução: " + DateTime.Now.ToString() + "\n";
                string _UltimoCaractere = "";
                _UltimoCaractere = txtQuery.Text[txtQuery.Text.Length - 1].ToString();
                while ((_UltimoCaractere == " ") || (_UltimoCaractere == ";") || (_UltimoCaractere == "\n") || (_UltimoCaractere == "\r"))
	            {
	                txtQuery.Text = txtQuery.Text.Remove(txtQuery.Text.Length - 1);
                    _UltimoCaractere = txtQuery.Text[txtQuery.Text.Length - 1].ToString();
	            }
                OracleCommand cmd = new OracleCommand();
                con.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
                con.Open();

                cmd.Connection = con;
                cmd.CommandText = txtQuery.Text;
                cmd.CommandType = CommandType.Text;
                OracleDataReader DataReader = cmd.ExecuteReader();
                if (DataReader.Read())
                {
                    _LeuAlgumaCoisa = true;
                    if (chkMostrarResultado.Checked)
                    {
                        this.MostraResultadoListView(lvwResultado, DataReader);
                    }
                    else
                    {
                        this.LimpaListView(lvwResultado);
                    }
                }
                else
                {
                    _LeuAlgumaCoisa = false;
                    this.LimpaListView(lvwResultado);
                }
                DataReader.Close();
                if (optAvisarCasoRetorneAlgo.Checked)
                {
                    if (_LeuAlgumaCoisa)
                    {
                        DialogResult _Resposta;
                        this.WindowState = FormWindowState.Normal;
                        this.TopMost = true;
                        //SystemSounds.Beep.Play();
                        _Resposta = MessageBox.Show("A consulta retornou pelo menos um registro\n\n" + txtQuery.Text + "\n\n" + "Continuar Executando?", "Executa Query", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (_Resposta == System.Windows.Forms.DialogResult.No)
                        {
                            this.PararExecucao();
                        }
                        this.TopMost = false;
                    }
                }
                if (optAvisarCasoNaoRetorneNada.Checked)
                {
                    if (!_LeuAlgumaCoisa)
                    {
                        DialogResult _Resposta;
                        this.TopMost = true;
                        this.WindowState = FormWindowState.Normal;
                        //SystemSounds.Beep.Play();
                        _Resposta = MessageBox.Show("A consulta não retornou nenhum registro\n\n" + txtQuery.Text + "\n\n" + "Continuar Executando?", "Executa Query", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (_Resposta == System.Windows.Forms.DialogResult.No)
                        {
                            this.PararExecucao();
                        }
                        this.TopMost = false;
                    }
                }
                if (optAvisarCasoNaoRetorneErro.Checked)
                {
                    DialogResult _Resposta;
                    this.TopMost = true;
                    this.WindowState = FormWindowState.Normal;
                    _Resposta = MessageBox.Show("A consulta não retornou nenhum erro\n\n" + txtQuery.Text + "\n\n" + "Continuar Executando?", "Executa Query", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (_Resposta == System.Windows.Forms.DialogResult.No)
                    {
                        this.PararExecucao();
                    }
                    this.TopMost = false;
                }
                con.Close(); 
                con.Dispose();
                this.LayoutAguardando();
            }
            catch (Exception _Exception)
            {
                if (optAvisarCasoNaoRetorneErro.Checked)
                {
                    txtMensagens.Text = "Erro: " + DateTime.Now.ToString() + "\n" + _Exception.Message;
                }
                else
                {
                    if (optAvisarCasoRetorneErro.Checked)
                    {
                        DialogResult _Resposta;
                        this.TopMost = true;
                        this.WindowState = FormWindowState.Normal;
                        _Resposta = MessageBox.Show("A consulta retornou um erro\n\n" + _Exception.Message + "\n\n" + "Continuar Executando?", "Executa Query", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (_Resposta == System.Windows.Forms.DialogResult.No)
                        {
                            this.PararExecucao();
                        }
                        this.TopMost = false;
                    }
                    else
                    {
                        txtMensagens.Text = "Erro: " + DateTime.Now.ToString() + "\n" + _Exception.Message;
                        //this.PararExecucao();
                        //con.Close();
                        //con.Dispose();
                        //MessageBox.Show("ERRO\n" + _Exception.Message, "ExecutaQuery", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
        }

        private void LayoutExecutando()
        {
            lblExecutando.Text = "Executando";
            this.Text = "Executa Query (Executando)";
            this.Update();
            this.Refresh();
            Application.DoEvents();
        }

        private void LayoutAguardando()
        {
            if (_Executar)
            {
                lblExecutando.Text = "Ativado";
                this.Text = "Executa Query (Ativado)";
            }
            else
            {
                lblExecutando.Text = "Desativado";
                this.Text = "Executa Query (Desativado)";
            }
            this.Update();
            this.Refresh();
            Application.DoEvents();
        }

        private bool ConectouNoBanco()
        {
            string l_Username = "";
            string l_Password = "";
            string l_Database = "";
            frmLoginORACLE FormLogin = new frmLoginORACLE(false, ref l_Username, ref l_Password, ref l_Database);

            if (l_Username.Length > 0 && l_Password.Length > 0 && l_Database.Length > 0)
            {
                _Username = l_Username;
                _Password = l_Password;
                _Database = l_Database;
            }

            if (_Username.Length > 0 && _Password.Length > 0 && _Database.Length > 0)
            {

                string _Mensagem = "";
                string _InfoBanco = "";
                _InfoBanco = _csOracle.InfoBanco(_Username, _Password, _Database, ref _Mensagem);
                if (_Mensagem.Length == 0)
                {
                    _InfoBanco = _InfoBanco.Replace(" - ", "\n");
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void CopiarTodaLista()
        {
            if (lvwResultado.Items.Count > 0)
            {
                StringBuilder _ClipBoard = new StringBuilder();
                ListViewItem lvwItem = null;
                for (int i = 0; i < lvwResultado.Columns.Count; i++)
                {
                    _ClipBoard.Append(lvwResultado.Columns[i].Text + "\t");
                }
                _ClipBoard.Append("\r\n");

                for (int i = 0; i < lvwResultado.Items.Count; i++)
                {
                    lvwItem = lvwResultado.Items[i];
                    for (int j = 0; j < lvwItem.SubItems.Count; j++)
                    {
                        _ClipBoard.Append(lvwItem.SubItems[j].Text + "\t");
                    }
                    _ClipBoard.Append("\r\n");
                }
                Clipboard.SetText(_ClipBoard.ToString());
            }
        }

        private void lvwResultado_KeyDown(object sender, KeyEventArgs e)
        {
            // Se pressionou CTLR + C, copia as informações dos itens selecionados da ListView para a área de transferência
            if (e.KeyCode == Keys.C)
            {
                if (e.Control)
                {
                    this.CopiarTodaLista();
                }
            }
        }

    #endregion

        

    }
}
