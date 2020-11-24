using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Oracle.DataAccess.Client;

namespace Oracle_Tools
{
    public partial class frmLoginORACLE : Form
    {
        public class csDadosConexaoORACLE
        {
            public string Apelido = "";
            public string Usuario = "";
            public string Senha = "";
            public string Database = "";
        }

        #region Variáveis privadas

        csDadosConexaoORACLE _ExcluirDadosConexao = null;

        private string _Usuario = "";
        private string _Senha = "";
        private string _Database = "";
        private bool _TestarConexao = true;
        private string _StringFake = "[" + Environment.ProcessorCount.ToString() + Environment.MachineName + Environment.ProcessorCount.ToString() + "]";
        private ArrayList _ConexoesSalvas = new ArrayList();

        #endregion Variáveis privadas

        #region Construtores

        public frmLoginORACLE(bool p_TestarConexao, ref string p_Usuario, ref string p_Senha, ref string p_Datadabse)
        {
            InitializeComponent();

            _TestarConexao = p_TestarConexao;

            if (p_Usuario.Trim().Length > 0)
            {
                txtUsername.Text = p_Usuario;
            }
            else 
            {
                string _UltimoLoginUsuario = (string)csUtil.CarregarPreferencia("UltimoLoginUsuario");

                if (_UltimoLoginUsuario != null)
                {
                    txtUsername.Text = _UltimoLoginUsuario;
                }
            }

            if (p_Datadabse.Trim().Length > 0)
            {
                cboDatabase.Text = p_Datadabse;
            }
            else 
            {
                string _UltimoLoginDatabase = (string)csUtil.CarregarPreferencia("UltimoLoginDatabase");

                if (_UltimoLoginDatabase != null)
                {
                    cboDatabase.Text = _UltimoLoginDatabase;
                }
            }

            if (p_Senha.Trim().Length > 0)
            {
                txtPassword.Text = p_Senha;
                chkSalvarConexao.Checked = true;
                chkSalvarSenha.Checked = true;
            }

            this.ShowDialog();

            p_Usuario = _Usuario;
            p_Senha = _Senha;
            p_Datadabse = _Database;
        }

        #endregion Construtores

        #region Métodos Privados

        private bool ConectouNoBanco(string p_Usuario, string p_Senha, string p_Database, bool p_SalvarPreferencia = true)
        {
            OracleConnection con = new OracleConnection();
            try
            {
                con.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                con.Open();
                if (p_SalvarPreferencia)
                {
                    csUtil.SalvarPreferencia("UltimoLoginUsuario", p_Usuario);
                    csUtil.SalvarPreferencia("UltimoLoginDatabase", p_Database);
                }
                con.Close();
                con.Dispose();
                GC.Collect();
                return true;
            }
            catch (Exception _Exception)
            {
                con.Close();
                con.Dispose();
                GC.Collect();
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    return this.ConectouNoBanco(p_Usuario, p_Senha, p_Database, p_SalvarPreferencia);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.Message, "ConectouNoBanco", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

        }

        /// <summary>
        /// Abre dialog para selecionar o caminho do TNSNAMES.ora e salva no registro
        /// </summary>
        private string SelecionaESalvaCaminhoTnsNames()
        {
            OpenFileDialog dlgAbrir = new OpenFileDialog();
            DialogResult _Resp;
            dlgAbrir.Title = "Informe o caminho do arquivo tnsnames.ora";
            dlgAbrir.Filter = "TNS Names|tnsnames.ora";
            dlgAbrir.Multiselect = false;
            _Resp = dlgAbrir.ShowDialog();
            if (_Resp == DialogResult.OK)
            {
                return dlgAbrir.FileName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Abre arquivo TNSNAMES.ora e retorna a lista de databases contida no arquivo
        /// </summary>
        private ArrayList ListaDatabasesDoTnsNames(string p_CaminhoCompleto)
        {
            if (p_CaminhoCompleto == null)
            {
                return null;
            }
            else
            {
                if (File.Exists(p_CaminhoCompleto))
                {
                    ArrayList _Retorno = new ArrayList();
                    string _ArquivoTexto = null;
                    int _Pos = -1;
                    string _TextoAux = "";
                    string[] _Split;

                    char[] _ArquivoChar;
                    StreamReader _StreamReader = File.OpenText(p_CaminhoCompleto);
                    _ArquivoChar = new char[_StreamReader.BaseStream.Length];
                    _StreamReader.Read(_ArquivoChar, 0, (int)_StreamReader.BaseStream.Length);
                    _StreamReader.Close();
                    foreach (char _char in _ArquivoChar)
                    {
                        if (_char.ToString() != "\n" && _char.ToString() != "\r")
                        {
                            _ArquivoTexto = _ArquivoTexto + _char.ToString().ToUpper();
                        }
                        else
                        {
                            _ArquivoTexto = _ArquivoTexto + " ";
                        }
                    }

                    while (_ArquivoTexto.IndexOf("  ") >= 0)
                    {
                        _ArquivoTexto = _ArquivoTexto.Replace("  ", " ");
                    }

                    while (_ArquivoTexto.IndexOf("DESCRIPTION") >= 0)
                    {
                        _Pos = _ArquivoTexto.IndexOf("DESCRIPTION");
                        _TextoAux = _ArquivoTexto.Substring(0, _Pos);
                        _Pos = _TextoAux.LastIndexOf("=");
                        _TextoAux = _ArquivoTexto.Substring(0, _Pos);
                        _Split = _TextoAux.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        _Retorno.Add(_Split[_Split.Length - 1]);
                        _Pos = _ArquivoTexto.IndexOf("DESCRIPTION");
                        _ArquivoTexto = _ArquivoTexto.Substring(_Pos + "DESCRIPTION".Length);
                    }
                    return _Retorno;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Preenche a combo com a lista de databases obtida do arquivo TNSNAMES.ora
        /// </summary>
        private void PreencheComboDatabases()
        {
            string _CaminhoTnsNames = (string)csUtil.CarregarPreferencia("CaminhoTnsNames");
            if (_CaminhoTnsNames == null)
            {
                _CaminhoTnsNames = this.SelecionaESalvaCaminhoTnsNames();
            }
            if (_CaminhoTnsNames != null)
            {
                ArrayList _Lista = this.ListaDatabasesDoTnsNames(_CaminhoTnsNames);
                if (_Lista != null)
                {
                    if (_Lista.Count > 0)
                    {
                        cboDatabase.Items.Clear();
                        foreach (string _String in _Lista)
                        {
                            cboDatabase.Items.Add(_String);
                        }
                    }
                }
            }
        }
        
        private bool SalvouDadosConexao()
        {
            string _DadosAlinhados = "";
            string _DadosDesencriptados = "";
            string _DadosEncriptados = "";
            int _Pos = -1;
            int _Loops = -1;
            csDadosConexaoORACLE l_csDadosConexaoORACLE;

            for (int i = _ConexoesSalvas.Count - 1; i >= 0; i--)
            {
                l_csDadosConexaoORACLE = (csDadosConexaoORACLE)_ConexoesSalvas[i];
                _DadosAlinhados = l_csDadosConexaoORACLE.Apelido + "\n" + l_csDadosConexaoORACLE.Usuario + "\n" + l_csDadosConexaoORACLE.Senha + "\n" + l_csDadosConexaoORACLE.Database + "\n";
                _Loops = csUtil.GetRandomNumber(5, 11);
                for (int l = 0; l < _Loops; l++)
                {
                    _Pos = csUtil.GetRandomNumber(1, _DadosAlinhados.Length);
                    _DadosAlinhados = _DadosAlinhados.Substring(0, _Pos) + _StringFake + _DadosAlinhados.Substring(_Pos);
                }
                _DadosDesencriptados = _DadosDesencriptados + _DadosAlinhados;
            }

            _DadosEncriptados = csUtil.Encriptar(_DadosDesencriptados);
            csUtil.SalvarPreferencia("ConexoesORACLE", _DadosEncriptados);
            return true;
        }

        private bool SalvouDadosConexao(string p_Usuario, string p_Senha, string p_Database)
        {
            csDadosConexaoORACLE l_csDadosConexaoORACLE;
            string _Apelido = "";
            bool _JaTinha = false;
            

            this.BuscaDadosConexao();

            foreach (csDadosConexaoORACLE _csDadosConexao in _ConexoesSalvas)
            {
                if (_csDadosConexao.Usuario == p_Usuario && _csDadosConexao.Database == p_Database)
                {
                    _Apelido = _csDadosConexao.Apelido;
                    break;
                }
            }

            if (_Apelido == "")
            {
                DialogResult _Resp = csUtil.InputBox("Apelido da Conexão", "Informe o apelido da Conexão", ref _Apelido);
                if (_Resp == DialogResult.Cancel)
                {
                    return false;
                }
            }

            l_csDadosConexaoORACLE = new csDadosConexaoORACLE();

            foreach (csDadosConexaoORACLE _csDadosConexao in _ConexoesSalvas)
            {
                if (_csDadosConexao.Apelido == _Apelido)
                {
                    _csDadosConexao.Usuario = p_Usuario;
                    _csDadosConexao.Senha = p_Senha;
                    _csDadosConexao.Database = p_Database;
                    l_csDadosConexaoORACLE = _csDadosConexao;
                    _JaTinha = true;
                    break;
                }
            }

            if (_JaTinha)
            {
                _ConexoesSalvas.Remove(l_csDadosConexaoORACLE);
                _ConexoesSalvas.Add(l_csDadosConexaoORACLE);
            }
            else
            {
                l_csDadosConexaoORACLE.Apelido = _Apelido;
                l_csDadosConexaoORACLE.Usuario = p_Usuario;
                l_csDadosConexaoORACLE.Senha = p_Senha;
                l_csDadosConexaoORACLE.Database = p_Database;
                _ConexoesSalvas.Add(l_csDadosConexaoORACLE);
            }
            return this.SalvouDadosConexao();
        }

        private void BuscaDadosConexao()
        {
            string _DadosAlinhados = "";
            string _DadosEncriptados = "";
            csDadosConexaoORACLE _csDadosConexaoORACLE = new csDadosConexaoORACLE();
            int _Cont = 0;
            string[] _Split;

            _ConexoesSalvas = new ArrayList();

            _DadosEncriptados = (string)csUtil.CarregarPreferencia("ConexoesORACLE");

            if (_DadosEncriptados == null)
            {
                return;
            }

            _DadosAlinhados = csUtil.Desencriptar(_DadosEncriptados);

            while (_DadosAlinhados.IndexOf(_StringFake) > -1)
            {
                _DadosAlinhados = _DadosAlinhados.Replace(_StringFake, "");
            }

            _Split = _DadosAlinhados.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            _Cont = 0;
            for (int i = 0; i < _Split.Length; i++)
            {
                if (_Cont == 0)
                {
                    _csDadosConexaoORACLE = new csDadosConexaoORACLE();
                    _csDadosConexaoORACLE.Apelido = _Split[i];
                    _Cont++;
                }
                else if (_Cont == 1)
                {
                    _csDadosConexaoORACLE.Usuario = _Split[i];
                    _Cont++;
                }
                else if (_Cont == 2)
                {
                    _csDadosConexaoORACLE.Senha = _Split[i];
                    if (_csDadosConexaoORACLE.Senha.Trim() == "")
                    {
                        _csDadosConexaoORACLE.Senha = "";
                    }
                    _Cont++;
                }
                else if (_Cont == 3)
                {
                    _csDadosConexaoORACLE.Database = _Split[i];
                    _ConexoesSalvas.Add(_csDadosConexaoORACLE);
                    _Cont = 0;
                }
            }
        }

        #endregion Métodos Privados

        #region Eventos dos controles

        private void cboDatabase_DropDown(object sender, EventArgs e)
        {
            this.PreencheComboDatabases();
        }

        private void btLocalizarTnsNames_Click(object sender, EventArgs e)
        {
            string _CaminhoTnsNames;
            _CaminhoTnsNames = this.SelecionaESalvaCaminhoTnsNames();
            if (_CaminhoTnsNames != null)
            {
                this.PreencheComboDatabases();
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (_TestarConexao)
            {
                if (this.ConectouNoBanco(txtUsername.Text, txtPassword.Text, cboDatabase.Text))
                {
                    _Usuario = txtUsername.Text;
                    _Senha = txtPassword.Text;
                    _Database = cboDatabase.Text;
                    if (chkSalvarConexao.Checked)
                    {
                        if (chkSalvarSenha.Checked)
                        {
                            if (!this.SalvouDadosConexao(_Usuario, _Senha, _Database))
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (!this.SalvouDadosConexao(_Usuario, " ", _Database))
                            {
                                return;
                            }
                        }
                    }
                    this.Hide();
                }
            }
            else
            {
                _Usuario = txtUsername.Text;
                _Senha = txtPassword.Text;
                _Database = cboDatabase.Text;
                if (chkSalvarConexao.Checked)
                {
                    if (chkSalvarSenha.Checked)
                    {
                        if (!this.SalvouDadosConexao(_Usuario, _Senha, _Database))
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (!this.SalvouDadosConexao(_Usuario, " ", _Database))
                        {
                            return;
                        }
                    }
                }
                this.Hide();
            }
            
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            _Usuario = "";
            _Senha = "";
            _Database = "";
            this.Hide();
        }

        private void cboConexoesSalvas_Click(object sender, EventArgs e)
        {
            csDadosConexaoORACLE l_csDadosConexaoORACLE = null;
            ToolStripItem _SubMenu = null;
            Point _Point = new Point(Cursor.Position.X + 12, Cursor.Position.Y);
            mnuMenu.Items.Clear();
            this.BuscaDadosConexao();

            if (_ConexoesSalvas.Count > 0)
            {
                for (int i = 0; i < _ConexoesSalvas.Count; i++)
                {
                    l_csDadosConexaoORACLE = (csDadosConexaoORACLE)_ConexoesSalvas[i];
                    _SubMenu = mnuMenu.Items.Add(l_csDadosConexaoORACLE.Apelido);
                    _SubMenu.Tag = l_csDadosConexaoORACLE;
                }
                mnuMenu.Show(_Point);
            }
        }

        private void mnuMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            csDadosConexaoORACLE l_csDadosConexaoORACLE = (csDadosConexaoORACLE)e.ClickedItem.Tag;

            if (e.ClickedItem.Text.Length >= 9 && e.ClickedItem.Text.Substring(0, 9) == "EXCLUIR: ")
            {
                _ExcluirDadosConexao = l_csDadosConexaoORACLE;
                tmrMenu.Enabled = true;
            }
            else
            {
                txtUsername.Text = l_csDadosConexaoORACLE.Usuario;
                txtPassword.Text = l_csDadosConexaoORACLE.Senha;
                cboDatabase.Text = l_csDadosConexaoORACLE.Database;
                chkSalvarConexao.Checked = true;
                chkSalvarSenha.Checked = (txtPassword.Text.Length > 0);
            }
        }

        private void chkSalvarConexao_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSalvarConexao.Checked)
            {
                chkSalvarSenha.Enabled = true;
            }
            else
            {
                chkSalvarSenha.Checked = false;
                chkSalvarSenha.Enabled = false;
            }
        }

        private void cboConexoesSalvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                csDadosConexaoORACLE l_csDadosConexaoORACLE = null;
                ToolStripItem _SubMenu = null;
                Point _Point = new Point(Cursor.Position.X + 12, Cursor.Position.Y);
                mnuMenu.Items.Clear();
                this.BuscaDadosConexao();

                if (_ConexoesSalvas.Count > 0)
                {
                    for (int i = 0; i < _ConexoesSalvas.Count; i++)
                    {
                        l_csDadosConexaoORACLE = (csDadosConexaoORACLE)_ConexoesSalvas[i];
                        _SubMenu = mnuMenu.Items.Add("EXCLUIR: " + l_csDadosConexaoORACLE.Apelido);
                        _SubMenu.Tag = l_csDadosConexaoORACLE;
                    }
                    mnuMenu.Show(_Point);
                }
            }
        }

        private void tmrMenu_Tick(object sender, EventArgs e)
        {
            tmrMenu.Enabled = false;
            DialogResult _Resp = MessageBox.Show("Tem certeza que deseja apagar os dados de conexão de apelido \"" + _ExcluirDadosConexao.Apelido + "\"?", "Apagar Dados de Conexão Salvo", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (_Resp == DialogResult.Yes)
            {
                _ConexoesSalvas.Remove(_ExcluirDadosConexao);
                if (this.SalvouDadosConexao())
                {
                    MessageBox.Show("Dados de conexão excluído!", "Apagar Dados de Conexão Salvo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Problemas para excluir os dados de conexão", "Apagar Dados de Conexão Salvo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion Eventos dos controles

    }
}
