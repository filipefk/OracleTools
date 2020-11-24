using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;
using System.DirectoryServices;
using SharpSvn;
using System.Net;

namespace Oracle_Tools
{
    public partial class frmDetalhesUser : Form
    {
    
    #region Campos privados

        private string _Username = "";
        private string _Password = "";
        private string _Database = "";
        private csOracle _csOracle = new csOracle();

        private string _Usuario_AD = "";
        private string _Senha_AD = "";
        private string _Dominio_AD = "";
        private string _Porta_AD = "";

        private string _Usuario_SVN = "";
        private string _Senha_SVN = "";
        private string _URL_SVN = "";

        private string[] _ListaUsuariosSVN = null;

    #endregion Campos privados

    #region Propriedades

        /// <summary>
        /// Retorna se tem os parâmetros de conexão definidos
        /// </summary>
        public bool EstaConectadoSVN
        {
            get
            {
                return (_Usuario_SVN.Length > 0 && _Senha_SVN.Length > 0 && _URL_SVN.Length > 0);
            }
        }

        /// <summary>
        /// Retorna se tem os parâmetros de conexão definidos
        /// </summary>
        public bool EstaConectadoDB
        {
            get
            {
                return (_Username.Length > 0 && _Password.Length > 0 && _Database.Length > 0);
            }
        }

        /// <summary>
        /// Retorna se tem os parâmetros de conexão do AD definidos
        /// </summary>
        public bool EstaConectadoAD
        {
            get
            {
                return (_Usuario_AD.Length > 0 && _Senha_AD.Length > 0 && _Dominio_AD.Length > 0 && _Porta_AD.Length > 0);
            }
        }

    #endregion Propriedades

    #region Construtores

        public frmDetalhesUser()
        {
            InitializeComponent();
            _Username = "";
            _Password = "";
            _Database = "";
            this.Text = "Detalhes Usuário - NÃO CONECTADO";
        }

        public frmDetalhesUser(string p_Username, string p_Password, string p_Database, string p_NomeUser)
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
                this.Text = "Detalhes Usuário - " + _InfoBanco;
            }
            if (p_NomeUser.Trim().Length > 0)
            {
                txtPesquisaUsuarioBanco.Text = p_NomeUser;
                this.PesquisarUsuario();
            }
        }

    #endregion Construtores

    #region Métodos Privados

        private string RelatorioTree(TreeNode p_Node)
        {
            string _Relatorio = "";

            for (int i = 0; i < p_Node.Nodes.Count; i++)
            {
                _Relatorio = _Relatorio + p_Node.Nodes[i].FullPath + "\n";
                if ((p_Node.Nodes[i].Nodes.Count > 0) && (p_Node.Nodes[i].IsExpanded))
                {
                    _Relatorio = _Relatorio + this.RelatorioTree(p_Node.Nodes[i]);
                }
            }
            return _Relatorio;
        }

        private void PesquisarUsuario()
        {
            if (!this.EstaConectadoDB)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }

            string _Mensagem = "";
            string _NomeUser = txtPesquisaUsuarioBanco.Text.Trim().ToUpper();

            trvUsuario.Nodes.Clear();
            TreeNode _NodePai = trvUsuario.Nodes.Add(_NomeUser);
            TreeNode _Node = null;
            ArrayList _Retorno = null;

            _Retorno = _csOracle.ListaPropUser(_Username, _Password, _Database, _NomeUser, ref _Mensagem);
            if (_Mensagem.Trim().Length > 0)
            {
                MessageBox.Show("Problemas ao listar propriedades do usuário " + _NomeUser + "\n" + _Mensagem, "Pesquisar propriedades do Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                for (int i = 0; i < _Retorno.Count; i++)
                {
                    _Node = _NodePai.Nodes.Add(_Retorno[i].ToString());
                    if (_Node.Text.Trim().ToUpper().IndexOf("PROFILE") > -1)
                    {
                        _Node = _Node.Nodes.Add("PROFILE");
                        _Node.Tag = "FAKE";
                    }
                }
            }

            if (_csOracle.ExisteOwner(_Username, _Password, _Database, "HORACIUS"))
            {
                string _NomeScript = "";
                _NomeScript = csUtil.PastaLocalExecutavel() + "Consulta_Horacius.sql";
                if (File.Exists(_NomeScript))
                {
                    _Node = _NodePai.Nodes.Add("Perfil SGX");
                    _Node = _Node.Nodes.Add("HORACIUS");
                    _Node.Tag = "FAKE";
                    _Node.Parent.Expand();
                }
            }

            _Node = _NodePai.Nodes.Add("Tablespaces");
            _Node = _Node.Nodes.Add("PROP");
            _Node.Tag = "FAKE";
            //_Node.Parent.Expand();

            _Node = _NodePai.Nodes.Add("Privilégios");
            _Node = _Node.Nodes.Add("PROP");
            _Node.Tag = "FAKE";
            //_Node.Parent.Expand();

            _Node = _NodePai.Nodes.Add("Roles");
            _Node = _Node.Nodes.Add("PROP");
            _Node.Tag = "FAKE";
            _Node.Parent.Expand();

            _Node = _NodePai.Nodes.Add("Grants");
            _Node = _Node.Nodes.Add("PROP");
            _Node.Tag = "FAKE";
            _Node.Parent.Expand();

            _NodePai.Expand();

            txtPesquisaUsuarioBanco.SelectAll();
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
                    this.Text = "Detalhes Usuário - " + _InfoBanco;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ConectouNoAD()
        {
            string l_Usuario_AD = "";
            string l_Senha_AD = "";
            string l_Dominio_AD = "";
            string l_Porta_AD = "";
            frmLoginAD FormLoginAD = new frmLoginAD(true, ref l_Usuario_AD, ref l_Senha_AD, ref l_Dominio_AD, ref l_Porta_AD);

            if (l_Usuario_AD.Length > 0 && l_Senha_AD.Length > 0 && l_Dominio_AD.Length > 0 && l_Porta_AD.Length > 0)
            {
                _Usuario_AD = l_Usuario_AD;
                _Senha_AD = l_Senha_AD;
                _Dominio_AD = l_Dominio_AD;
                _Porta_AD = l_Porta_AD;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ConectouNoSVN()
        {
            string l_Usuario_SVN = "";
            string l_Senha_SVN = "";
            string l_URL_SVN = "";
            frmLoginSVN FormLoginSVN = new frmLoginSVN(true, ref l_Usuario_SVN, ref l_Senha_SVN, ref l_URL_SVN);

            if (l_Usuario_SVN.Length > 0 && l_Senha_SVN.Length > 0 && l_URL_SVN.Length > 0)
            {
                _Usuario_SVN = l_Usuario_SVN;
                _Senha_SVN = l_Senha_SVN;
                _URL_SVN = l_URL_SVN;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CarregaListaUsuariosSVN()
        {
            if (_ListaUsuariosSVN != null)
            {
                return;
            }

            string _CaminhoArquivo = "";
            string _ConteudoArquivoUsuarios = "";

            string _CaminhoPasta = csUtil.PastaLocalExecutavel() + "Access_fontes";

            SvnClient _SvnClient = new SvnClient();
            SvnTarget _SvnTarget = SvnTarget.FromString(_URL_SVN);
            _SvnClient.Authentication.DefaultCredentials = new NetworkCredential(_Usuario_SVN, _Senha_SVN);

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    _SvnClient.CheckOut(new Uri(_URL_SVN + "/branches/_Apoio/Access_fontes"), _CaminhoPasta);
                }
                catch (Exception)
                {

                }
            }
            
            _CaminhoArquivo = _CaminhoPasta + "\\Lista_Usuarios.txt";
            _ConteudoArquivoUsuarios = File.ReadAllText(_CaminhoArquivo, Encoding.Default);

            Directory.Delete(_CaminhoPasta, true);

            _ConteudoArquivoUsuarios = _ConteudoArquivoUsuarios.Replace("\r", "");
            _ListaUsuariosSVN = _ConteudoArquivoUsuarios.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        }

        private string LocalizaUsuarioAD(string p_Nome)
        {
            bool _FazerPesquiza = false;

            string _Retorno = "";

            if (!this.EstaConectadoAD)
            {
                if (this.ConectouNoAD())
                {
                    _FazerPesquiza = true;
                }
            }
            else
            {
                _FazerPesquiza = true;
            }

            if (_FazerPesquiza)
            {
                try
                {
                    DirectoryEntry _DirectoryEntry = new DirectoryEntry("LDAP://" + _Dominio_AD + ":" + _Porta_AD, _Usuario_AD, _Senha_AD);
                    DirectorySearcher _DirectorySearcher = new DirectorySearcher(_DirectoryEntry);
                    int userAccountControl = 0;
                    bool isAccountDisabled = false;
                    bool isAccountLockedOut = false;
                    bool isPasswordNeverExpire = false;
                    bool UserMustChangePasswordAtNextLogon = false;
                    string _NomeUser = "";
                    ArrayList _Grupos = new ArrayList();

                    _DirectorySearcher.PropertiesToLoad.Add("displayname");
                    _DirectorySearcher.PropertiesToLoad.Add("description");
                    _DirectorySearcher.PropertiesToLoad.Add("samaccountname");
                    _DirectorySearcher.PropertiesToLoad.Add("userAccountControl");
                    _DirectorySearcher.PropertiesToLoad.Add("pwdLastSet");

                    string _Filtro = p_Nome.Replace("%", "*");

                    _DirectorySearcher.Filter = "(|(displayname=" + _Filtro + ") (samaccountname=" + _Filtro + "))";

                    SearchResultCollection Results = _DirectorySearcher.FindAll();

                    if (Results.Count > 0)
                    {
                        ResultPropertyValueCollection _ResultPropertyValueCollection = null;

                        foreach (SearchResult searchResult in Results)
                        {
                            _ResultPropertyValueCollection = searchResult.Properties["samaccountname"];
                            _Retorno = _Retorno + _ResultPropertyValueCollection[0].ToString().ToUpper();
                            _NomeUser = _ResultPropertyValueCollection[0].ToString().ToUpper();

                            _ResultPropertyValueCollection = searchResult.Properties["displayname"];
                            if (_ResultPropertyValueCollection.Count > 0)
                            {
                                _Retorno = _Retorno + " - " + _ResultPropertyValueCollection[0].ToString().ToUpper() + "\r\n";
                            }
                            else
                            {
                                _ResultPropertyValueCollection = searchResult.Properties["description"];
                                if (_ResultPropertyValueCollection.Count > 0)
                                {
                                    _Retorno = _Retorno + " - " + _ResultPropertyValueCollection[0].ToString().ToUpper() + "\r\n";
                                }
                                else
                                {
                                    _Retorno = _Retorno + " - PROBLEMAS PAGA BUSCAR O NOME NO AD\r\n";
                                }
                            }

                            _ResultPropertyValueCollection = searchResult.Properties["userAccountControl"];
                            userAccountControl = int.Parse(_ResultPropertyValueCollection[0].ToString());

                            isAccountDisabled = ((userAccountControl & 2) == 2);
                            if (isAccountDisabled)
                            {
                                _Retorno = _Retorno + "Conta Desabilitada: TRUE\r\n";
                            }
                            else
                            {
                                _Retorno = _Retorno + "Conta Desabilitada: FALSE\r\n";
                            }

                            isAccountLockedOut = ((userAccountControl & 16) == 16);
                            if (isAccountLockedOut)
                            {
                                _Retorno = _Retorno + "Conta Bloqueada: TRUE\r\n";
                            }
                            else
                            {
                                _Retorno = _Retorno + "Conta Bloqueada: FALSE\r\n";
                            }

                            isPasswordNeverExpire = ((userAccountControl & 65536) == 65536);
                            if (isPasswordNeverExpire)
                            {
                                _Retorno = _Retorno + "Senha nunca expira: TRUE\r\n";
                            }
                            else
                            {
                                _Retorno = _Retorno + "Senha nunca expira: FALSE\r\n";
                            }

                            _ResultPropertyValueCollection = searchResult.Properties["pwdLastSet"];
                            UserMustChangePasswordAtNextLogon = ((_ResultPropertyValueCollection[0].ToString().ToUpper() == "0") || (_ResultPropertyValueCollection[0].ToString().ToUpper() == "-1"));
                            if (UserMustChangePasswordAtNextLogon)
                            {
                                _Retorno = _Retorno + "Trocar senha no próximo logon: TRUE\r\n";
                            }
                            else
                            {
                                _Retorno = _Retorno + "Trocar senha no próximo logon: FALSE\r\n";
                            }
                            //_Retorno = _Retorno + "\r\n";

                            _Grupos = new ArrayList();
                            _Retorno = _Retorno + "Membro dos grupos:\r\n";
                            _DirectorySearcher = new DirectorySearcher(_DirectoryEntry, "(&(objectClass=user)(samaccountname=" + _NomeUser.Trim() + "))");
                            _DirectorySearcher.PropertiesToLoad.Add("memberOf");
                            SearchResult _SearchResult = _DirectorySearcher.FindOne();
                            if (_SearchResult != null && !string.IsNullOrEmpty(_SearchResult.Path))
                            {
                                DirectoryEntry user = _SearchResult.GetDirectoryEntry();
                                PropertyValueCollection groups = user.Properties["memberOf"];
                                foreach (string path in groups)
                                {
                                    string[] parts = path.Split(',');
                                    if (parts.Length > 0)
                                    {
                                        foreach (string part in parts)
                                        {
                                            string[] p = part.Split('=');
                                            if (p[0].Equals("cn", StringComparison.OrdinalIgnoreCase))
                                            {
                                                _Grupos.Add(p[1]);
                                                //_Retorno = _Retorno + p[1] + "\r\n";
                                                //allRoles.Add(p[1]);
                                            }
                                        }
                                    }
                                }
                            }
                            if (_Grupos.Count > 0)
                            {
                                _Grupos.Sort();
                                for (int i = 0; i < _Grupos.Count; i++)
                                {
                                    _Retorno = _Retorno + "- " + _Grupos[i] + "\r\n";
                                }
                            }
                            _Retorno = _Retorno + "\r\n";

                        }
                    }
                    else
                    {
                        _Retorno = "Nenhum resultado encontrado no AD para " + p_Nome;
                    }

                    

                }
                catch (Exception _Exception)
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "LocalizaUsuarioAD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return _Retorno;
        }

        private string LocalizaUsuarioDB(string p_Nome)
        {
            bool _FazerPesquiza = false;

            string _Retorno = "";

            if (!this.EstaConectadoDB)
            {
                if (this.ConectouNoBanco())
                {
                    _FazerPesquiza = true;
                }
            }
            else
            {
                _FazerPesquiza = true;
            }

            if (_FazerPesquiza)
            {
                _Retorno = _Retorno + _csOracle.LocalizaUsuario(_Username, _Password, _Database, p_Nome);
            }

            return _Retorno;
        }

        private string LocalizaUsuarioSVN(string p_Nome)
        {
            bool _FazerPesquiza = false;

            string _Retorno = "";

            if (!this.EstaConectadoSVN)
            {
                if (this.ConectouNoSVN())
                {
                    _FazerPesquiza = true;
                }
            }
            else
            {
                _FazerPesquiza = true;
            }

            if (_FazerPesquiza)
            {
                this.CarregaListaUsuariosSVN();
                if (_ListaUsuariosSVN != null)
                {
                    string _Pesquisa = p_Nome;
                    _Pesquisa = _Pesquisa.Replace("%", "");
                    _Pesquisa = _Pesquisa.Replace("*", "");
                    _Pesquisa = _Pesquisa.Trim().ToUpper();
                    for (int i = 0; i < _ListaUsuariosSVN.Length; i++)
                    {
                        if (_ListaUsuariosSVN[i].Trim().ToUpper().IndexOf(_Pesquisa, 0) > -1)
                        {
                            _Retorno = _Retorno + _ListaUsuariosSVN[i].Replace("\t", " - ") + "\r\n";
                        }
                    }
                }
            }

            if (_Retorno.Trim().Length == 0)
            {
                _Retorno = "Usuário " + p_Nome + " não encontrado\r\n";
            }

            return _Retorno;
        }

        private void LocalizarUsuario()
        {
            txtRelatorio.Text = "";

            Application.DoEvents();

            txtRelatorio.Text = txtRelatorio.Text + "######## Procurando no Active Directory: ########\r\n\r\n";
            txtRelatorio.Text = txtRelatorio.Text + this.LocalizaUsuarioAD(txtLocalizaUsuario.Text) + "\r\n\r\n";

            Application.DoEvents();

            txtRelatorio.Text = txtRelatorio.Text + "######## Procurando no banco de dados: ########\r\n\r\n";
            txtRelatorio.Text = txtRelatorio.Text + this.LocalizaUsuarioDB(txtLocalizaUsuario.Text) + "\r\n\r\n";

            Application.DoEvents();

            txtRelatorio.Text = txtRelatorio.Text + "######## Procurando no SVN: ########\r\n\r\n";
            txtRelatorio.Text = txtRelatorio.Text + this.LocalizaUsuarioSVN(txtLocalizaUsuario.Text) + "\r\n\r\n";

        }

    #endregion Métodos Privados

    #region Eventos dos controles

        private void btPesquisar_Click(object sender, EventArgs e)
        {
            this.PesquisarUsuario();
        }

        private void trvUsuario_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Tag != null)
            {
                if (e.Node.Nodes[0].Tag.ToString() == "FAKE")
                {
                    string _NomeUser = e.Node.Parent.Text;
                    TreeNode _Node = null;
                    ArrayList _Retorno = null;
                    string _NomeRole = "";
                    string _TipoObjRole = "";
                    int _Pos = -1;
                    string _NomeProfile = "";
                    string _TipoProfile = "";

                    Cursor.Current = Cursors.WaitCursor;

                    switch (e.Node.Nodes[0].Text)
                    {
                        #region case PROP
                        case "PROP":
                            switch (e.Node.Text)
                            {
                                case "Tablespaces":
                                    e.Node.Nodes.Clear();
                                    _Retorno = _csOracle.ListaCotasTablespaceUser(_Username, _Password, _Database, _NomeUser);
                                    for (int i = 0; i < _Retorno.Count; i++)
                                    {
                                        _Node = e.Node.Nodes.Add(_Retorno[i].ToString());
                                    }
                                    e.Node.Expand();
                                    break;

                                case "Privilégios":
                                    e.Node.Nodes.Clear();
                                    _Retorno = _csOracle.ListaPrivUser(_Username, _Password, _Database, _NomeUser);
                                    for (int i = 0; i < _Retorno.Count; i++)
                                    {
                                        _Node = e.Node.Nodes.Add(_Retorno[i].ToString());
                                    }
                                    e.Node.Expand();
                                    break;

                                case "Roles":
                                    e.Node.Nodes.Clear();
                                    _Retorno = _csOracle.ListaRolesUser(_Username, _Password, _Database, _NomeUser);
                                    for (int i = 0; i < _Retorno.Count; i++)
                                    {
                                        _Node = e.Node.Nodes.Add(_Retorno[i].ToString());
                                        _Node = _Node.Nodes.Add("ROLE");
                                        _Node.Tag = "FAKE";
                                    }
                                    e.Node.Expand();
                                    break;

                                case "Grants":
                                    e.Node.Nodes.Clear();
                                    _Retorno = _csOracle.ListaNoRolesGrantsUser(_Username, _Password, _Database, _NomeUser);
                                    for (int i = 0; i < _Retorno.Count; i++)
                                    {
                                        _Node = e.Node.Nodes.Add(_Retorno[i].ToString());
                                    }
                                    e.Node.Expand();
                                    break;
                            }

                            break;
                        #endregion
                        #region case ROLE
                        case "ROLE":

                            _NomeRole = e.Node.Text;
                            _Pos = _NomeRole.IndexOf(":");
                            if (_Pos <= -1)
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Problemas para identificar o nome da role " + _NomeRole, "Pesquisar propriedades do Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                _NomeRole = _NomeRole.Substring(0, _Pos);
                            }
                            e.Node.Nodes.Clear();

                            _Retorno = _csOracle.ListaPrivSysRole(_Username, _Password, _Database, _NomeRole);
                            for (int i = 0; i < _Retorno.Count; i++)
                            {
                                _Node = e.Node.Nodes.Add(_Retorno[i].ToString());
                            }

                            _Retorno = _csOracle.ListaTiposObjRole(_Username, _Password, _Database, _NomeRole);
                            for (int i = 0; i < _Retorno.Count; i++)
                            {
                                _Node = e.Node.Nodes.Add(_Retorno[i].ToString());
                                _Node = _Node.Nodes.Add("OBJ ROLE");
                                _Node.Tag = "FAKE";
                            }
                            e.Node.Expand();
                            break;
                        #endregion
                        #region case OBJ ROLE
                        case "OBJ ROLE":

                            _NomeRole = e.Node.Parent.Text;
                            _TipoObjRole = e.Node.Text;
                            _Pos = _NomeRole.IndexOf(":");
                            if (_Pos <= -1)
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Problemas para identificar o nome da role " + _NomeRole, "Pesquisar propriedades do Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                _NomeRole = _NomeRole.Substring(0, _Pos);
                            }
                            e.Node.Nodes.Clear();
                            
                            _Retorno = _csOracle.ListaPrivRoleObj(_Username, _Password, _Database, _NomeRole, _TipoObjRole);
                            for (int i = 0; i < _Retorno.Count; i++)
                            {
                                _Node = e.Node.Nodes.Add(_Retorno[i].ToString());
                            }
                            e.Node.Expand();
                            break;
                        #endregion
                        #region case PROFILE
                        case "PROFILE":

                            _NomeProfile = e.Node.Text;
                            _Pos = _NomeProfile.IndexOf(":");
                            if (_Pos <= -1)
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Problemas para identificar o nome do PROFILE " + _NomeProfile, "Pesquisar propriedades do Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                _NomeProfile = _NomeProfile.Substring(_Pos + 1);
                                _NomeProfile = _NomeProfile.Trim().ToUpper();
                            }
                            e.Node.Nodes.Clear();

                            TreeNode _NodeKernel = null;
                            TreeNode _NodePassword = null;

                            _NodeKernel = e.Node.Nodes.Add("KERNEL");
                            _NodePassword = e.Node.Nodes.Add("PASSWORD");

                            _Retorno = _csOracle.ListaDetalheProfile(_Username, _Password, _Database, _NomeProfile);
                            for (int i = 0; i < _Retorno.Count; i++)
                            {
                                _Pos = _Retorno[i].ToString().IndexOf(".");
                                _TipoProfile = _Retorno[i].ToString().Substring(0, _Pos);

                                if (_TipoProfile == "KERNEL")
                                {
                                    _Node = _NodeKernel.Nodes.Add(_Retorno[i].ToString().Substring(_Pos + 1));
                                }
                                else
                                {
                                    _Node = _NodePassword.Nodes.Add(_Retorno[i].ToString().Substring(_Pos + 1));
                                }
                            }
                            e.Node.Expand();
                            _NodeKernel.Expand();
                            _NodePassword.Expand();
                            break;
                        #endregion
                        #region case HORACIUS
                        case "HORACIUS":

                            e.Node.Nodes.Clear();

                            _Retorno = _csOracle.ListaPerfilHoracius(_Username, _Password, _Database, _NomeUser);
                            for (int i = 0; i < _Retorno.Count; i++)
                            {
                                _Node = e.Node.Nodes.Add(_Retorno[i].ToString());
                            }
                            e.Node.Expand();
                            break;
                        #endregion
                    }

                    Cursor.Current = Cursors.Default;
                }
            }
        }
        
        private void btExtrairDDLUsuario_Click(object sender, EventArgs e)
        {
            if (!this.EstaConectadoDB)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }

            string _Mensagem = "";
            string _NomeUser = txtPesquisaUsuarioBanco.Text.Trim().ToUpper();

            Cursor.Current = Cursors.WaitCursor;
            string _ScriptCriacao = _csOracle.ExtractDDLUser(_Username, _Password, _Database, _NomeUser, ref _Mensagem);
            Cursor.Current = Cursors.Default;

            if (_Mensagem.Trim().Length > 0)
            {
                MessageBox.Show("Problemas ao extrair script do usuário " + _NomeUser + "\n" + _Mensagem, "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                csUtil.SalvarEAbrir(_ScriptCriacao, _NomeUser + ".sql");
            }

        }

        private void btExpandirTudo_Click(object sender, EventArgs e)
        {
            if (trvUsuario.Nodes.Count > 0)
            {
                trvUsuario.Nodes[0].ExpandAll();
            }
        }

        private void btRelatorio_Click(object sender, EventArgs e)
        {
            string _Relatorio = "";
            string[] _Split;
            int _ContBarra = 0;
            int _PosUltimaBarra = -1;
            string _StringAuxiliar = "";
            string _Mensagem = "";

            if (trvUsuario.Nodes.Count > 0)
	        {
                _Relatorio = _csOracle.InfoBanco(_Username, _Password, _Database, ref _Mensagem) + "\n";
                _Relatorio = _Relatorio + trvUsuario.Nodes[0].FullPath + "\n";
                if (trvUsuario.Nodes[0].IsExpanded)
                {
                    _Relatorio = _Relatorio + this.RelatorioTree(trvUsuario.Nodes[0]);
                }
	        }
            if (_Relatorio.Length > 0)
            {
                _Split = _Relatorio.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                _Relatorio = "";
                for (int i = 0; i < _Split.Length; i++)
                {
                    _ContBarra = _Split[i].Split('\\').Length - 1;
                    if (_ContBarra > 0)
                    {
                        _PosUltimaBarra = _Split[i].LastIndexOf("\\");
                        _StringAuxiliar = _Split[i].Substring(_PosUltimaBarra + 1);
                        for (int z = 0; z < _ContBarra; z++)
                        {
                            _StringAuxiliar = "\t" + _StringAuxiliar;
                        }
                        _Relatorio = _Relatorio + _StringAuxiliar + "\n";
                    }
                    else
                    {
                        _Relatorio = _Relatorio + _Split[i] + "\n";
                    }
                }
            }
            _Relatorio = _Relatorio.Replace("\n", "\r\n");
            csUtil.SalvarEAbrir(_Relatorio, "Relatorio_Usuario_" + trvUsuario.Nodes[0].FullPath + ".txt");
        }

        private void mnuInfoBanco_Click(object sender, EventArgs e)
        {
            if (!this.EstaConectadoDB)
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

        private void trvUsuario_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode _NodeClicked = trvUsuario.GetNodeAt(e.X, e.Y);
                try
                {
                    int _Pos = -1;

                    switch (_NodeClicked.Parent.Text)
                    {
                        case "PACKAGE":
                        case "FUNCTION":
                        case "SEQUENCE":
                        case "VIEW":
                        case "PROCEDURE":

                            string _OwnerNomeObj = _NodeClicked.Text;
                            string _TipoObjeto = _NodeClicked.Parent.Text;

                            _Pos = _OwnerNomeObj.IndexOf("ON");
                            _OwnerNomeObj = _OwnerNomeObj.Substring(_Pos + 3);

                            Cursor.Current = Cursors.WaitCursor;
                            _csOracle.ExtractDDLTextEditor(_Username, _Password, _Database, _OwnerNomeObj);
                            Cursor.Current = Cursors.Default;
                            break;

                        case "Roles":

                            string _NomeRole = _NodeClicked.Text;
                            _Pos = _NomeRole.IndexOf(":");
                            _NomeRole = _NomeRole.Substring(0, _Pos);
                            frmDetalhesRole _frmDetalhesRole = new frmDetalhesRole(_Username, _Password, _Database, _NomeRole);
                            _frmDetalhesRole.Show(this);
                            break;
                    }
                }
                catch (Exception)
                {
                    // SE DER ERRO NÃO FAZ NADA
                }
            }
        }

        private void btLocalizarUsuario_Click(object sender, EventArgs e)
        {
            this.LocalizarUsuario();
        }

        private void txtPesquisaUsuarioBanco_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.PesquisarUsuario();
            }
        }

        private void txtLocalizaUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.LocalizarUsuario();
            }
        }

        private void txtRelatorio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    ((TextBox)sender).SelectAll();
                    e.SuppressKeyPress = true;
                }
            }
        }

    #endregion Eventos dos controles

    }
}
