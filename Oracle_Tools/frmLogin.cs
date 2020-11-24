using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Oracle_Tools
{
    public partial class frmLogin : Form
    {
    #region Variáveis privadas

        private csOracle _csOracle = new csOracle();
        private string _Usuario = "";
        private string _Senha = "";
        private string _Database = "";
        private bool _TestarConexao = true;
        private string _StringFake = Environment.ProcessorCount.ToString() + Environment.MachineName + Environment.ProcessorCount.ToString();
        //private ToolTip _ToolTipText = new ToolTip();

    #endregion

    #region Construtores

        public frmLogin(bool p_TestarConexao, ref string p_Usuario, ref string p_Senha, ref string p_Datadabse)
        {
            InitializeComponent();

            _TestarConexao = p_TestarConexao;

            if (p_Usuario.Trim().Length > 0)
            {
                txtUsername.Text = p_Usuario;
            }
            else if (_csOracle.UltimoLoginUsuario != null)
            {
                txtUsername.Text = _csOracle.UltimoLoginUsuario;
            }

            if (p_Datadabse.Trim().Length > 0)
            {
                cboDatabase.Text = p_Datadabse;
            }
            else if (_csOracle.UltimoLoginDatabase != null)
            {
                cboDatabase.Text = _csOracle.UltimoLoginDatabase;
            }

            if (p_Senha.Trim().Length > 0)
            {
                txtPassword.Text = p_Senha;
                chkSalvar.Checked = true;
            }

            this.ShowDialog();

            p_Usuario = _Usuario;
            p_Senha = _Senha;
            p_Datadabse = _Database;
        }

    #endregion
    
    #region Métodos Privados

        /// <summary>
        /// Preenche a combo com a lista de databases obtida do arquivo TNSNAMES.ora
        /// </summary>
        private void PreencheComboDatabases()
        {
            string _CaminhoTnsNames;
            _CaminhoTnsNames = _csOracle.CaminhoTnsNames;
            if (_CaminhoTnsNames == null)
            {
                _CaminhoTnsNames = _csOracle.SelecionaESalvaCaminhoTnsNames();
            }
            if (_CaminhoTnsNames != null)
            {
                ArrayList _Lista = _csOracle.ListaDatabasesDoTnsNames();
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
        
        private void SalvaDadosConexao(string p_Usuario, string p_Senha, string p_Database)
        {
            bool _JaTinha = false;
            string _ConexoesSalvas = "";
            string _DadosEncriptados = "";
            csDadosConexao _csDadosConexao = null;
            ArrayList _DadosSalvos = this.BuscaDadosConexao();
            Random _Random = new Random();
            int _NumRandom = 0;

            if (_DadosSalvos.Count > 0)
            {
                for (int i = 0; i < _DadosSalvos.Count; i++)
                {
                    _csDadosConexao = (csDadosConexao)_DadosSalvos[i];
                    if ((_csDadosConexao.Usuario.ToUpper() == p_Usuario.ToUpper()) && (_csDadosConexao.Database.ToUpper() == p_Database.ToUpper()))
                    {
                        _JaTinha = true;
                        _csDadosConexao.Senha = p_Senha;
                        _DadosSalvos[i] = _csDadosConexao;
                        break;
                    }
                }
                if (_JaTinha)
                {
                    _ConexoesSalvas = "";
                    for (int i = 0; i < _DadosSalvos.Count; i++)
                    {
                        _csDadosConexao = (csDadosConexao)_DadosSalvos[i];

                        if (_ConexoesSalvas.Length > 0)
                        {
                            _ConexoesSalvas = _ConexoesSalvas + "\n";
                        }
                        _NumRandom = _Random.Next(100);
                        if ((_NumRandom % 2) == 0)
                        {
                            _ConexoesSalvas = _ConexoesSalvas + _StringFake;
                        }
                        _ConexoesSalvas = _ConexoesSalvas + _csDadosConexao.Usuario + "\n" + _csDadosConexao.Senha + "\n" + _csDadosConexao.Database;
                        _NumRandom = _Random.Next(100);
                        if ((_NumRandom % 2) == 0)
                        {
                            _ConexoesSalvas = _ConexoesSalvas + _StringFake;
                        }
                    }
                    _DadosEncriptados = csUtil.Encriptar(_ConexoesSalvas);
                    csUtil.SalvarPreferencia("ConexoesSalvas", _DadosEncriptados);
                }
                else
                {
                    _ConexoesSalvas = "";
                    for (int i = 0; i < _DadosSalvos.Count; i++)
                    {
                        _csDadosConexao = (csDadosConexao)_DadosSalvos[i];

                        if (_ConexoesSalvas.Length > 0)
                        {
                            _ConexoesSalvas = _ConexoesSalvas + "\n";
                        }
                        _NumRandom = _Random.Next(100);
                        if ((_NumRandom % 2) == 0)
                        {
                            _ConexoesSalvas = _ConexoesSalvas + _StringFake;
                        }
                        _ConexoesSalvas = _ConexoesSalvas + _csDadosConexao.Usuario + "\n" + _csDadosConexao.Senha + "\n" + _csDadosConexao.Database;
                        _NumRandom = _Random.Next(100);
                        if ((_NumRandom % 2) == 0)
                        {
                            _ConexoesSalvas = _ConexoesSalvas + _StringFake;
                        }
                    }
                    _ConexoesSalvas = _ConexoesSalvas + "\n" + p_Usuario + "\n" + p_Senha + "\n" + p_Database;
                    _DadosEncriptados = csUtil.Encriptar(_ConexoesSalvas);
                    csUtil.SalvarPreferencia("ConexoesSalvas", _DadosEncriptados);
                }
            }
            else
            {
                _NumRandom = _Random.Next(100);
                if ((_NumRandom % 2) == 0)
                {
                    _ConexoesSalvas = _ConexoesSalvas + _StringFake;
                }
                _ConexoesSalvas = p_Usuario + "\n" + p_Senha + "\n" + p_Database;
                _NumRandom = _Random.Next(100);
                if ((_NumRandom % 2) == 0)
                {
                    _ConexoesSalvas = _ConexoesSalvas + _StringFake;
                }
                _DadosEncriptados = csUtil.Encriptar(_ConexoesSalvas);
                csUtil.SalvarPreferencia("ConexoesSalvas", _DadosEncriptados);
            }
        }

        private ArrayList BuscaDadosConexao()
        {
            int _Pos = 0;
            csDadosConexao _csDadosConexao = null;
            ArrayList _Retorno = new ArrayList();
            string[] vetSplit;
            string _DadosEncriptados = (string)csUtil.CarregarPreferencia("ConexoesSalvas");
            if (_DadosEncriptados == null)
            {
                return _Retorno;
            }
            string _DadosDesencriptados = csUtil.Desencriptar(_DadosEncriptados);
            _DadosDesencriptados = _DadosDesencriptados.Replace(_StringFake, "");
            vetSplit = _DadosDesencriptados.Split(new Char[] { '\n' });
            for (int i = 0; i < vetSplit.Length; i++)
            {
                if (_Pos == 0)
                {
                    _csDadosConexao = new csDadosConexao();
                    _csDadosConexao.Usuario = vetSplit[i];
                    _Pos++;
                }
                else
                {
                    if (_Pos == 1)
                    {
                        _csDadosConexao.Senha = vetSplit[i];
                        _Pos++;
                    }
                    else
                    {
                        _csDadosConexao.Database = vetSplit[i];
                        _Retorno.Add(_csDadosConexao);
                        _Pos = 0;
                    }
                }
            }
            return _Retorno;
        }

    #endregion

    #region Eventos dos controles

        private void cboDatabase_DropDown(object sender, EventArgs e)
        {
            this.PreencheComboDatabases();
        }

        private void btLocalizarTnsNames_Click(object sender, EventArgs e)
        {
            string _CaminhoTnsNames;
            _CaminhoTnsNames = _csOracle.SelecionaESalvaCaminhoTnsNames();
            if (_CaminhoTnsNames != null)
            {
                this.PreencheComboDatabases();
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (_TestarConexao)
            {
                if (_csOracle.ConectouNoBanco(txtUsername.Text, txtPassword.Text, cboDatabase.Text))
                {
                    _Usuario = txtUsername.Text;
                    _Senha = txtPassword.Text;
                    _Database = cboDatabase.Text;
                    _csOracle = null;
                    if (chkSalvar.Checked)
                    {
                        this.SalvaDadosConexao(_Usuario, _Senha, _Database);
                    }
                    else
                    {
                        this.SalvaDadosConexao(_Usuario, "", _Database);
                    }
                    this.Hide();
                }
            }
            else
            {
                _Usuario = txtUsername.Text;
                _Senha = txtPassword.Text;
                _Database = cboDatabase.Text;
                _csOracle = null;
                if (chkSalvar.Checked)
                {
                    this.SalvaDadosConexao(_Usuario, _Senha, _Database);
                }
                else
                {
                    this.SalvaDadosConexao(_Usuario, "", _Database);
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
            csDadosConexao _csDadosConexao = null;
            ToolStripItem _SubMenu = null;
            Point _Point = new Point(Cursor.Position.X + 12, Cursor.Position.Y);
            mnuMenu.Items.Clear();
            ArrayList _DadosSalvos = this.BuscaDadosConexao();

            if (_DadosSalvos.Count > 0)
            {
                for (int i = 0; i < _DadosSalvos.Count; i++)
                {
                    _csDadosConexao = (csDadosConexao)_DadosSalvos[i];
                    _SubMenu = mnuMenu.Items.Add(_csDadosConexao.Usuario + "@" + _csDadosConexao.Database);
                    _SubMenu.Tag = _csDadosConexao.Usuario + "\n" + _csDadosConexao.Senha + "\n" + _csDadosConexao.Database;
                }
                mnuMenu.Show(_Point);
            }
            
        }

        private void mnuMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string[] vetSplit;
            vetSplit = e.ClickedItem.Tag.ToString().Split(new Char[] { '\n' });
            txtUsername.Text = vetSplit[0];
            txtPassword.Text = vetSplit[1];
            cboDatabase.Text = vetSplit[2];
            chkSalvar.Checked = (vetSplit[1].Length > 0);
        }

        private void chkSalvar_MouseEnter(object sender, EventArgs e)
        {
            //if (chkSalvar.Checked)
            //{
            //    _ToolTipText.IsBalloon = true;
            //    _ToolTipText.UseAnimation = true;
            //    _ToolTipText.UseFading = true;
            //    _ToolTipText.Show(string.Empty, this, 0, 0);
            //    _ToolTipText.Show("A informação será salva encriptada em:\nCURRENT_USER\\Software\\ORACLE_Tools\\Parametros\\ConexoesSalvas", chkSalvar, 0, -60, 10000);
            //}
        }

        private void chkSalvar_MouseLeave(object sender, EventArgs e)
        {
            //_ToolTipText.Hide(chkSalvar);
        }

        private void chkSalvar_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkSalvar.Checked)
            //{
            //    _ToolTipText.IsBalloon = true;
            //    _ToolTipText.UseAnimation = true;
            //    _ToolTipText.UseFading = true;
            //    _ToolTipText.Show(string.Empty, this, 0, 0);
            //    _ToolTipText.Show("A informação será salva encriptada em:\nCURRENT_USER\\Software\\ORACLE_Tools\\Parametros\\ConexoesSalvas", chkSalvar, 0, -60, 10000);
            //}
        }

    #endregion
        
    }
}
