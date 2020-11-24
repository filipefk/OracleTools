using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Media;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace Oracle_Tools
{
    public partial class frmProcuraLock : Form
    {
    #region Campos privados

        private string _Username = "";
        private string _Password = "";
        private string _Database = "";
        private csOracle _csOracle = new csOracle();
        private int _QualScript = 0;
        private ArrayList _Scripts = new ArrayList();

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

        public frmProcuraLock()
        {
            InitializeComponent();
            _Username = "";
            _Password = "";
            _Database = "";
            this.Text = "Procura Lock no banco - NÃO CONECTADO";
            if (!this.PreencheuListaScripts())
            {
                MessageBox.Show("Nenhum script de lock encontrado. Procurando por:\n" + csUtil.PastaLocalExecutavel() + "ScriptLock01.sql", "Salvou DDL Objeto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btScript1.Enabled = false;
                btScript2.Enabled = false;
            }
            else
            {
                if (_Scripts.Count == 1)
                {
                    btScript2.Enabled = false;
                }
            }
        }

        public frmProcuraLock(string p_Username, string p_Password, string p_Database)
        {
            InitializeComponent();
            _Username = p_Username;
            _Password = p_Password;
            _Database = p_Database;

            string _Mensagem = "";
            string _InfoBanco = "";
            _InfoBanco = _csOracle.InfoBanco(_Username, _Password, _Database, ref _Mensagem);
            if (_Mensagem.Length == 0)
            {
                this.Text = "Procura Lock no banco - " + _InfoBanco;
                if (!this.PreencheuListaScripts())
                {
                    MessageBox.Show("Nenhum script de lock encontrado. Procurando por:\n" + csUtil.PastaLocalExecutavel() + "ScriptLock01.sql", "Salvou DDL Objeto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btScript1.Enabled = false;
                    btScript2.Enabled = false;
                }
                else
                {
                    if (_Scripts.Count == 1)
                    {
                        btScript2.Enabled = false;
                    }
                }
            }

        }

    #endregion

    #region Métodos Privados

        private bool PreencheuListaScripts()
        {
            bool _AchouAlgumScript = false;
            string _NomeScript = "";
            string _Script = "";

            _NomeScript = csUtil.PastaLocalExecutavel() + "ScriptLock01.sql";
            if (File.Exists(_NomeScript))
            {
                _Script = File.ReadAllText(_NomeScript, Encoding.Default);
                _Scripts.Add(_Script);
                _AchouAlgumScript = true;
            }

            _NomeScript = csUtil.PastaLocalExecutavel() + "ScriptLock02.sql";
            if (File.Exists(_NomeScript))
            {
                _Script = File.ReadAllText(_NomeScript, Encoding.Default);
                _Scripts.Add(_Script);
                _AchouAlgumScript = true;
            }

            return _AchouAlgumScript;
        }

        private bool ConectouNoBanco()
        {
            string l_Username = "";
            string l_Password = "";
            string l_Database = "";
            frmLoginORACLE FormLogin = new frmLoginORACLE(true, ref l_Username, ref l_Password, ref l_Database);

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
                    this.Text = "Procura Lock no banco - " + _InfoBanco;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ProcuraLock()
        {
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }

            lblMensagem.Text = "Procurando...";
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            try
            {
                OracleCommand _Command = new OracleCommand();
                OracleConnection _Connection = new OracleConnection();
                OracleDataReader _DataReader;
                ListViewItem _lvwItem = null;
                ListViewItem.ListViewSubItem _lvwSubItem = null;
                ColumnHeader _lvwColuna = null;

                _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
                _Connection.Open();

                _Command.Connection = _Connection;
                _Command.CommandText = _Scripts[_QualScript - 1].ToString();
                _Command.CommandType = CommandType.Text;
                _DataReader = _Command.ExecuteReader();

                lvwLocks.Visible = false;
                lvwLocks.Items.Clear();
                lvwLocks.Columns.Clear();
                for (int i = 0; i < _DataReader.FieldCount; i++)
                {
                    _lvwColuna = lvwLocks.Columns.Add(_DataReader.GetName(i));
                }
                
                _lvwItem = lvwLocks.Items.Add(lvwLocks.Columns[0].Text);
                lvwLocks.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                _lvwItem.Text = "";

                for (int i = 1; i < _DataReader.FieldCount; i++)
                {
                    _lvwSubItem = _lvwItem.SubItems.Add(lvwLocks.Columns[i].Text);
                    lvwLocks.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    _lvwSubItem.Text = "";
                }
                //for (int i = 0; i < _DataReader.FieldCount; i++)
                //{
                //    lvwLocks.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                //}
                lvwLocks.Items.Clear();
                lvwLocks.Visible = true;
                while (_DataReader.Read())
                {
                    if (_DataReader.IsDBNull(0))
                    {
                        _lvwItem = lvwLocks.Items.Add("NULL");
                    }
                    else
                    {
                        switch (_DataReader.GetFieldType(0).Name)
                        {
                            case "DateTime":
                                _lvwItem = lvwLocks.Items.Add(_DataReader.GetDateTime(0).ToString("dd/MM/yy HH:mm:ss"));
                                break;
                            case "Decimal":
                                _lvwItem = lvwLocks.Items.Add(_DataReader.GetDecimal(0).ToString());
                                break;
                            case "String":
                                _lvwItem = lvwLocks.Items.Add(_DataReader.GetString(0));
                                break;
                            default:
                                _lvwItem = lvwLocks.Items.Add(_DataReader.GetString(0));
                                break;
                        }
                    }
                    

                    for (int i = 1; i < _DataReader.FieldCount; i++)
                    {
                        if (_DataReader.IsDBNull(i))
                        {
                            _lvwItem.SubItems.Add("NULL");
                        }
                        else
                        {
                            switch (_DataReader.GetFieldType(i).Name)
                            {
                                case "DateTime":
                                    _lvwItem.SubItems.Add(_DataReader.GetDateTime(i).ToString("dd/MM/yy HH:mm:ss"));
                                    break;
                                case "Decimal":
                                    _lvwItem.SubItems.Add(_DataReader.GetDecimal(i).ToString());
                                    break;
                                case "String":
                                    _lvwItem.SubItems.Add(_DataReader.GetString(i));
                                    break;
                                default:
                                    _lvwItem.SubItems.Add(_DataReader.GetString(i));
                                    break;
                            }
                        }
                    }
                }
                
                _DataReader.Close();
                _DataReader.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                _Command.Dispose();
                GC.Collect();
                Cursor.Current = Cursors.Default;
                Application.DoEvents();
                if (chkAvisar.Checked && lvwLocks.Items.Count > 0)
                {
                    if (this.WindowState == FormWindowState.Minimized)
                    {
                        this.WindowState = FormWindowState.Normal;
                    }
                    
                    this.TopMost = true;
                    this.TopMost = false;
                    SystemSounds.Beep.Play();
                }
                if (tmrProcura.Enabled)
                {
                    lblMensagem.Text = "Ativado";
                }
                else
                {
                    lblMensagem.Text = "Parado";
                }
            }
            catch (Exception _Exception)
            {
                Cursor.Current = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Erro ao busca Lock de Banco\n\n" + _Exception.ToString(), "Procura Lock de banco", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GC.Collect();
                if (tmrProcura.Enabled)
                {
                    lblMensagem.Text = "Ativado";
                }
                else
                {
                    lblMensagem.Text = "Parado";
                }
            }
        }


    #endregion

    #region Eventos dos controles

        private void btScript1_Click(object sender, EventArgs e)
        {
            tmrProcura.Enabled = false;
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }
            _QualScript = 1;
            this.ProcuraLock();

            tmrProcura.Enabled = true;
            if (tmrProcura.Enabled)
            {
                lblMensagem.Text = "Ativado";
            }
            else
            {
                lblMensagem.Text = "Parado";
            }
        }

        private void btScript2_Click(object sender, EventArgs e)
        {
            tmrProcura.Enabled = false;
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }

            _QualScript = 2;
            this.ProcuraLock();

            tmrProcura.Enabled = true;
            if (tmrProcura.Enabled)
            {
                lblMensagem.Text = "Ativado";
            }
            else
            {
                lblMensagem.Text = "Parado";
            }
            
        }

        private void btParar_Click(object sender, EventArgs e)
        {
            _QualScript = 0;
            tmrProcura.Enabled = false;
            if (tmrProcura.Enabled)
            {
                lblMensagem.Text = "Ativado";
            }
            else
            {
                lblMensagem.Text = "Parado";
            }
        }

        private void btScript1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Clipboard.SetData(DataFormats.Text, _Scripts[0].ToString());
                MessageBox.Show("Script na área de transferência", "Procura Lock no banco", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btScript2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Clipboard.SetData(DataFormats.Text, _Scripts[1].ToString());
                MessageBox.Show("Script na área de transferência", "Procura Lock no banco", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tmrProcura_Tick(object sender, EventArgs e)
        {
            tmrProcura.Enabled = false;
            if (tmrProcura.Enabled)
            {
                lblMensagem.Text = "Ativado";
            }
            else
            {
                lblMensagem.Text = "Parado";
            }
            this.ProcuraLock();
            tmrProcura.Enabled = true;
            if (tmrProcura.Enabled)
            {
                lblMensagem.Text = "Ativado";
            }
            else
            {
                lblMensagem.Text = "Parado";
            }
        }

        private void lvwLocks_MouseDown(object sender, MouseEventArgs e)
        {


            if (e.Button == System.Windows.Forms.MouseButtons.Right && lvwLocks.Items.Count > 0)
            {
                bool _EstavaAtivo = tmrProcura.Enabled;
                tmrProcura.Enabled = false;
                if (tmrProcura.Enabled)
                {
                    lblMensagem.Text = "Ativado";
                }
                else
                {
                    lblMensagem.Text = "Parado";
                }
                string _Texto = "";

                for (int i = 0; i < lvwLocks.Columns.Count; i++)
                {
                    if (i > 0)
                    {
                        _Texto = _Texto + "\t";
                    }
                    _Texto = _Texto + lvwLocks.Columns[i].Text;
                    
                }
                _Texto = _Texto + "\n";

                for (int i = 0; i < lvwLocks.Items.Count; i++)
                {
                    for (int z = 0; z < lvwLocks.Columns.Count; z++)
                    {
                        if (z > 0)
                        {
                            _Texto = _Texto + "\t";
                        }
                        _Texto = _Texto + lvwLocks.Items[i].SubItems[z].Text;
                    }
                    _Texto = _Texto + "\n";
                }

                Clipboard.SetData(DataFormats.Text, _Texto);
                MessageBox.Show("Resultado na área de transferência", "Procura Lock no banco", MessageBoxButtons.OK, MessageBoxIcon.Information);

                tmrProcura.Enabled = _EstavaAtivo;
                if (tmrProcura.Enabled)
                {
                    lblMensagem.Text = "Ativado";
                }
                else
                {
                    lblMensagem.Text = "Parado";
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

    }
}
