using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using System.Data.OleDb;
using System.Collections;
using SharpSvn;
using WinSCP;
using System.Diagnostics;

namespace Oracle_Tools
{
    public partial class frmControleConcorrencia : Form
    {
    #region Campos privados

        private string _Username = "";
        private string _Password = "";
        private string _Database = "";

        private string _Usuario_SVN = "";
        private string _Senha_SVN = "";
        private string _URL_SVN = "";

        private string _Protocolo_FTP = "";
        private string _Host_FTP = "";
        private string _Usuario_FTP = "";
        private string _Senha_FTP = "";
        private string _Fingerprint_FTP = "";
        private bool _JaAtualizouArquivoListaBranches = false;
        private bool _JaAtualizouArquivoListaTrunk = false;

        private string _ConteudoArquivoUsuarios = "";
        private string _UltimaConsultaSecundaria = "";
        private csOracle _csOracle = new csOracle();
        private csListViewColumnSorter lvwColumnSorterPrincipal;
        private csListViewColumnSorter lvwColumnSorterSecundaria;
        private const int _colDATA_HORA = 0;
        private const int _colTIPO_OBJ = 1;
        private const int _colOWNER_OBJ = 2;
        private const int _colNOME_OBJ = 3;
        private const int _colID_ANALISTA = 4;
        private const int _colNOME_ANALISTA = 5;
        private const int _colEMPRESA = 6;
        private const int _colOS_USER = 7;
        private const int _colDB_USER = 8;
        private const int _colHOST = 9;
        private ToolTip _ToolTipText = new ToolTip();
        private bool _PararPesquisa = false;
        private DateTime _UltimaConsulta = DateTime.MinValue;
        private string _mnuContextoBotoesTag = "";
        private string _mnuContextoLvwTag = "";
        private string _ItemClicado = "";

    #endregion Campos privados

    #region Construtores

        public frmControleConcorrencia()
        {
            InitializeComponent();
            lvwColumnSorterPrincipal = new csListViewColumnSorter();
            lvwColumnSorterSecundaria = new csListViewColumnSorter();
            this.lvwObjetos.ListViewItemSorter = lvwColumnSorterPrincipal;
            this.lvwPesquisaSecundaria.ListViewItemSorter = lvwColumnSorterSecundaria;
            this.Text = "Controle de Concorrência - NÃO CONECTADO";
            lblStatusRelatorio.Text = "";
            btPesquisar.Tag = "PESQUISAR_ULTIMAS_24_HORAS";
        }

        public frmControleConcorrencia(string p_Usuario, string p_Senha, string p_Database, string p_Filtro)
        {
            InitializeComponent();
            lvwColumnSorterPrincipal = new csListViewColumnSorter();
            lvwColumnSorterSecundaria = new csListViewColumnSorter();
            this.lvwObjetos.ListViewItemSorter = lvwColumnSorterPrincipal;
            this.lvwPesquisaSecundaria.ListViewItemSorter = lvwColumnSorterSecundaria;
            this.Text = "Controle de Concorrência - NÃO CONECTADO";
            lblStatusRelatorio.Text = "";
            btPesquisar.Tag = "PESQUISAR_ULTIMAS_24_HORAS";

            if (this.ConectouNoBanco(p_Usuario, p_Senha, p_Database))
	        {
                string[] _Split;
                _Split = p_Filtro.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                switch (_Split[0])
                {
                    case "OBJETO":
                        btPesquisar.Tag = "PESQUISAR_TUDO";
                        txtPesquisa.Text = _Split[1];
                        tmrDigitacao.Enabled = false;
                        this.BotaoPesquisar(btPesquisar.Tag.ToString());
                        break;

                }
	        }
            else
            {
                MessageBox.Show("Problemas para conectar no banco de dados\nUsuario: " + p_Usuario + "\nDatabase: " + p_Database, "Controle de concorrência", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    #endregion Construtores

    #region Propriedades

        /// <summary>
        /// Retorna se tem os parâmetros de conexão do FTP definidos
        /// </summary>
        public bool EstaConectadoFTP
        {
            get
            {
                return (_Usuario_FTP.Length > 0 && _Senha_FTP.Length > 0 && _Host_FTP.Length > 0);
            }
        }

        /// <summary>
        /// Retorna se tem os parâmetros de conexão do SVN definidos
        /// </summary>
        public bool EstaConectadoSVN
        {
            get
            {
                return (_Usuario_SVN.Length > 0 && _Senha_SVN.Length > 0 && _URL_SVN.Length > 0);
            }
        }
        

        /// <summary>
        /// Retorna se tem os parâmetros de conexão de banco definidos
        /// </summary>
        public bool EstaConectado
        {
            get
            {
                return (_Username.Length > 0 && _Password.Length > 0 && _Database.Length > 0);
            }
        }

        /// <summary>
        /// Retorna ou salva O caminho do executável do Tortoise na máquina
        /// </summary>
        private string CaminhoExeTortoiseSVN
        {
            get
            {
                string _CaminhoExeTortoiseSVN = "";
                _CaminhoExeTortoiseSVN = (string)csUtil.CarregarPreferencia("CaminhoExeTortoiseSVN");
                return _CaminhoExeTortoiseSVN;
            }
            set
            {
                string _CaminhoExeTortoiseSVN;
                _CaminhoExeTortoiseSVN = value;
                csUtil.SalvarPreferencia("CaminhoExeTortoiseSVN", _CaminhoExeTortoiseSVN);
            }
        }

    #endregion Propriedades

    #region Métodos Privados
        
        private void BotaoPesquisar(string p_QualPesquisa)
        {
            switch (p_QualPesquisa)
            {
                case "PESQUISAR_TUDO":
                    btPesquisar.Tag = p_QualPesquisa;
                    this.Pesquisar(0);
                    //this.PreencheRelatorio("Geral");
                    break;

                case "PESQUISAR_ULTIMA_SEMANA":
                    btPesquisar.Tag = p_QualPesquisa;
                    this.Pesquisar(7);
                    //this.PreencheRelatorio("Geral");
                    break;

                case "PESQUISAR_ULTIMAS_24_HORAS":
                    btPesquisar.Tag = p_QualPesquisa;
                    this.Pesquisar(1);
                    //this.PreencheRelatorio("Geral");
                    break;

                case "PESQUISAR_ULTIMAS_72_HORAS":
                    btPesquisar.Tag = p_QualPesquisa;
                    this.Pesquisar(3);
                    //this.PreencheRelatorio("Geral");
                    break;

                case "LISTAR_CONFLITOS":
                    btPesquisar.Tag = p_QualPesquisa;
                    this.ListaConflitos();
                    break;

                case "CHAMAR_PREENCHE_USUARIOS":
                    this.ChamaProcedure("AESPROD.P_PREENCHE_USUARIO_AUDIT");
                    break;

                case "CHAMAR_PROCURA_CONFLITOS":
                    this.ChamaProcedure("AESPROD.P_PROCURA_CONFLITOS");
                    break;
            }
        }

        private bool ConectouNoFTP()
        {
            string l_Protocolo_FTP = "";
            string l_Host_FTP = "";
            string l_Usuario_FTP = "";
            string l_Senha_FTP = "";
            string l_Fingerprint_FTP = "";
            frmLoginFTP FormLoginFTP = new frmLoginFTP(true, ref l_Protocolo_FTP, ref l_Host_FTP, ref l_Usuario_FTP, ref l_Senha_FTP, ref l_Fingerprint_FTP);

            if (l_Usuario_FTP.Length > 0 && l_Senha_FTP.Length > 0 && l_Host_FTP.Length > 0)
            {
                _Protocolo_FTP = l_Protocolo_FTP;
                _Host_FTP = l_Host_FTP;
                _Usuario_FTP = l_Usuario_FTP;
                _Senha_FTP = l_Senha_FTP;
                _Fingerprint_FTP = l_Fingerprint_FTP;
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

        private bool ConectouNoBanco()
        {
            string l_Username = "";
            string l_Password = "";
            string l_Database = "";

            if (Environment.MachineName == "CI01081278")
            {
                l_Username = "TECBMFFK";
                l_Password = "ffktecbm";
                l_Database = "SGITST";

            }

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
                    this.Text = "Controle de Concorrência - " + _InfoBanco;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ConectouNoBanco(string p_Usuario, string p_Senha, string p_Database)
        {
            _Username = p_Usuario;
            _Password = p_Senha;
            _Database = p_Database;

            if (_Username.Length > 0 && _Password.Length > 0 && _Database.Length > 0)
            {
                string _Mensagem = "";
                string _InfoBanco = "";
                _InfoBanco = _csOracle.InfoBanco(_Username, _Password, _Database, ref _Mensagem);
                if (_Mensagem.Length == 0)
                {
                    this.Text = "Controle de Concorrência - " + _InfoBanco;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void PesquisaSecundaria(string p_Query)
        {
            if (p_Query == _UltimaConsultaSecundaria)
            {
                return;
            }
            else
            {
                _UltimaConsultaSecundaria = p_Query;
            }
            try
            {
                
                lblStatusListaSecundaria.Text = "Pesquisando...";
                lblStatusListaSecundaria.Refresh();
                Application.DoEvents();

                OracleCommand _Command = new OracleCommand();
                OracleConnection _Connection = new OracleConnection();
                OracleDataReader _DataReader;
                ListViewItem _ListViewItem = null;
                ListViewItem.ListViewSubItem _ListViewSubItem = null;

                _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                _Command.CommandText = p_Query;
                _DataReader = _Command.ExecuteReader();

                lvwPesquisaSecundaria.Items.Clear();
                _UltimaConsultaSecundaria = "";

                while (_DataReader.Read())
                {
                    //int colDATA_HORA = 0;
                    //int colTIPO_OBJ = 1;
                    //int colOWNER_OBJ = 2;
                    //int colNOME_OBJ = 3;
                    //int colID_ANALISTA = 4;
                    //int colNOME_ANALISTA = 5;
                    //int colEMAIL = 6;

                    _ListViewItem = lvwPesquisaSecundaria.Items.Add(_DataReader.GetDateTime(_colDATA_HORA).ToString("dd/MM/yy HH:mm:ss"));
                    _ListViewItem.SubItems.Add(_DataReader.GetString(_colTIPO_OBJ));
                    if (_DataReader.IsDBNull(_colOWNER_OBJ))
                    {
                        _ListViewItem.SubItems.Add("");
                    }
                    else
                    {
                        _ListViewItem.SubItems.Add(_DataReader.GetString(_colOWNER_OBJ));
                    }
                    _ListViewItem.SubItems.Add(_DataReader.GetString(_colNOME_OBJ));
                    if (_DataReader.IsDBNull(_colID_ANALISTA))
                    {
                        _ListViewSubItem = _ListViewItem.SubItems.Add(""); // colNOME_ANALISTA
                        _ListViewSubItem.Tag = -1; // colID_ANALISTA
                    }
                    else
                    {
                        _ListViewSubItem = _ListViewItem.SubItems.Add(_DataReader.GetString(_colNOME_ANALISTA));
                        _ListViewSubItem.Tag = _DataReader.GetDecimal(_colID_ANALISTA);
                    }

                }

                if (lvwPesquisaSecundaria.Items.Count > 0)
                {
                    for (int i = 0; i < lvwPesquisaSecundaria.Columns.Count; i++)
                    {
                        lvwPesquisaSecundaria.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    }
                }

                _DataReader.Close();
                _DataReader.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                _Command.Dispose();
                GC.Collect();
                lblStatusListaSecundaria.Text = lvwPesquisaSecundaria.Items.Count.ToString() + " itens listados";
                lblStatusListaSecundaria.Refresh();
                Application.DoEvents();
            }
            catch (Exception _Exception)
            {
                MessageBox.Show("Erro ao tentar pesquisar: \n" + _Exception.ToString(), "Pesquisar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Pesquisar(int p_PrazoDias)
        {
            string _Pesquisa = "";
            string _Query = "";

            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            lblStatusListaPrincipal.Text = "Pesquisando...";
            lblStatusListaPrincipal.Refresh();
            Application.DoEvents();

            _Query = "";
            _Query = _Query + "Select AU.DATA_HORA, "; // _colDATA_HORA = 0;
            _Query = _Query + "    AU.TIPO AS TIPO_OBJ, "; // _colTIPO_OBJ = 1;
            _Query = _Query + "    AU.OWNER AS OWNER_OBJ, "; // _colOWNER_OBJ = 2;
            _Query = _Query + "    AU.NOME AS NOME_OBJ, "; // _colNOME_OBJ = 3;
            _Query = _Query + "    CO.ID AS ID_ANALISTA, "; // _colID_ANALISTA = 4;
            _Query = _Query + "    CO.NOME_COMPLETO AS NOME_ANALISTA, "; // _colNOME_ANALISTA = 5;
            _Query = _Query + "    CO.EMPRESA, "; // _colEMPRESA = 6;
            _Query = _Query + "    AU.OSUSER, "; // _colOS_USER = 7;
            _Query = _Query + "    AU.CURRENT_USER, "; // _colDB_USER = 8;
            _Query = _Query + "    AU.HOST "; // _colHOST = 9;
            _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
            _Query = _Query + "    Left Join AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
            _Query = _Query + "    On AU.ID_USUARIO = CO.ID ";

            _Pesquisa = txtPesquisa.Text;
            string[] _Split;
            _Split = _Pesquisa.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (_Pesquisa.IndexOf(".") > -1 && _Pesquisa.LastIndexOf(".") == _Pesquisa.IndexOf(".") && _Pesquisa.IndexOf(" ") == -1 && _Split.Length == 2)
            {
                // Se só tem um ponto no filtro e não tem espaços em branco, vou considerar que é um objeto OWNER.NOME...
                _Query = _Query + "WHERE AU.OWNER = '" + _Split[0] + "' ";
                _Query = _Query + "    AND AU.NOME = '" + _Split[1] + "' ";
            }
            else
            {
                if (_Pesquisa.Trim().Length > 0)
                {
                    _Pesquisa = _Pesquisa.ToUpper();
                    _Pesquisa = _Pesquisa.Replace("Á", "A");
                    _Pesquisa = _Pesquisa.Replace("É", "E");
                    _Pesquisa = _Pesquisa.Replace("Í", "I");
                    _Pesquisa = _Pesquisa.Replace("Ó", "O");
                    _Pesquisa = _Pesquisa.Replace("Ú", "U");
                    _Pesquisa = _Pesquisa.Replace("Ç", "C");
                    _Pesquisa = _Pesquisa.Replace("Â", "A");
                    _Pesquisa = _Pesquisa.Replace("Ê", "E");
                    _Pesquisa = _Pesquisa.Replace("Î", "I");
                    _Pesquisa = _Pesquisa.Replace("Ô", "O");
                    _Pesquisa = _Pesquisa.Replace("Û", "U");
                    _Pesquisa = _Pesquisa.Replace("À", "A");
                    _Pesquisa = _Pesquisa.Replace("Ü", "U");
                    _Query = _Query + "WHERE (UPPER(AU.NOME) LIKE '%" + _Pesquisa + "%' ";
                    _Query = _Query + "    OR UPPER(AU.TIPO) LIKE '%" + _Pesquisa + "%' ";
                    _Query = _Query + "    OR UPPER(AU.OWNER) LIKE '%" + _Pesquisa + "%' ";
                    _Query = _Query + "    OR UPPER(CO.NOME_COMPLETO) LIKE '%" + _Pesquisa + "%') ";

                    if (p_PrazoDias > 0)
                    {
                        _Query = _Query + "    AND (AU.DATA_HORA >= SYSDATE - " + p_PrazoDias + ") ";
                    }
                }
                else
                {
                    if (p_PrazoDias > 0)
                    {
                        _Query = _Query + "WHERE (AU.DATA_HORA >= SYSDATE - " + p_PrazoDias + ") ";
                    }
                }
            }

            _Query = _Query + "ORDER BY AU.DATA_HORA DESC";

            try
            {
                //OracleCommand _Command = new OracleCommand();
                //OracleConnection _Connection = new OracleConnection();
                //OracleDataReader _DataReader;

                OleDbCommand _Command = new OleDbCommand();
                OleDbConnection _Connection = new OleDbConnection();
                OleDbDataReader _DataReader;

                ListViewItem _ListViewItem = null;
                ListViewItem.ListViewSubItem _ListViewSubItem = null;

                //_Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
                //_Connection.Open();

                _Connection.ConnectionString = "Provider=OraOLEDB.Oracle; User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database + ";ChunkSize=65535;"; // 65535 é o máximo. se colocar um parâmetro maior que isso vão vir menos caracteres.
                _Connection.Open();
                
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();

                lvwObjetos.Items.Clear();
                lvwPesquisaSecundaria.Items.Clear();
                lvwRelatorio.Items.Clear();
                lblPesquisaSecundaria.Text = "";
                _UltimaConsultaSecundaria = "";

                while (_DataReader.Read())
                {
                    // _colDATA_HORA = 0;
                    // _colTIPO_OBJ = 1;
                    // _colOWNER_OBJ = 2;
                    // _colNOME_OBJ = 3;
                    // _colID_ANALISTA = 4;
                    // _colNOME_ANALISTA = 5;
                    // _colEMPRESA = 6;
                    // _colOS_USER = 7;
                    // _colDB_USER = 8;
                    // _colHOST = 9;

                    _ListViewItem = lvwObjetos.Items.Add(_DataReader.GetDateTime(_colDATA_HORA).ToString("dd/MM/yy HH:mm:ss"));
                    _ListViewItem.SubItems.Add(_DataReader.GetString(_colTIPO_OBJ));
                    if (_DataReader.IsDBNull(_colOWNER_OBJ))
                    {
                        _ListViewItem.SubItems.Add("");
                    }
                    else
                    {
                        _ListViewItem.SubItems.Add(_DataReader.GetString(_colOWNER_OBJ));
                    }
                    _ListViewItem.SubItems.Add(_DataReader.GetString(_colNOME_OBJ));

                    if (_DataReader.IsDBNull(_colID_ANALISTA))
                    {
                        _ListViewSubItem = _ListViewItem.SubItems.Add(""); // colNOME_ANALISTA
                        _ListViewSubItem.Tag = -1; // colID_ANALISTA
                    }
                    else
                    {
                        _ListViewSubItem = _ListViewItem.SubItems.Add(_DataReader.GetString(_colNOME_ANALISTA));
                        _ListViewSubItem.Tag = _DataReader.GetDecimal(_colID_ANALISTA);
                    }

                    if (!_DataReader.IsDBNull(_colEMPRESA))
                    {
                        _ListViewItem.SubItems.Add(_DataReader.GetString(_colEMPRESA));
                    }
                    else
                    {
                        _ListViewItem.SubItems.Add("");
                    }
                    
                    _ListViewItem.SubItems.Add(_DataReader.GetString(_colOS_USER));

                    if (!_DataReader.IsDBNull(_colDB_USER))
                    {
                        _ListViewItem.SubItems.Add(_DataReader.GetString(_colDB_USER));
                    }
                    else
                    {
                        _ListViewItem.SubItems.Add("");
                    }
                    
                    _ListViewItem.SubItems.Add(_DataReader.GetString(_colHOST));
                }

                if (lvwObjetos.Items.Count > 0)
                {
                    for (int i = 0; i < lvwObjetos.Columns.Count; i++)
                    {
                        lvwObjetos.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    }
                }

                lvwObjetos.Visible = true;
                _DataReader.Close();
                _DataReader.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                _Command.Dispose();
                GC.Collect();
                lblStatusListaPrincipal.Text = lvwObjetos.Items.Count.ToString() + " itens listados";
                switch (btPesquisar.Tag.ToString())
                {
                    case "PESQUISAR_TUDO":
                        lblStatusListaPrincipal.Text = lblStatusListaPrincipal.Text + " - Pesquisar Tudo";
                        break;

                    case "PESQUISAR_ULTIMA_SEMANA":
                        lblStatusListaPrincipal.Text = lblStatusListaPrincipal.Text + " - Última semana";
                        break;

                    case "PESQUISAR_ULTIMAS_24_HORAS":
                        lblStatusListaPrincipal.Text = lblStatusListaPrincipal.Text + " - Últimas 24 horas";
                        break;

                    case "PESQUISAR_ULTIMAS_72_HORAS":
                        lblStatusListaPrincipal.Text = lblStatusListaPrincipal.Text + " - Últimas 72 horas";
                        break;
                }
                if (_Pesquisa.Trim().Length > 0)
                {
                    lblStatusListaPrincipal.Text = lblStatusListaPrincipal.Text + " - Filtro: " + _Pesquisa;
                }
                lblStatusListaPrincipal.Refresh();
                Application.DoEvents();
            }
            catch (Exception _Exception)
            {
                Cursor.Current = Cursors.Default;
                lvwObjetos.Visible = true;
                lblStatusListaPrincipal.Text = lvwObjetos.Items.Count.ToString() + " itens listados";
                switch (btPesquisar.Tag.ToString())
                {
                    case "PESQUISAR_TUDO":
                        lblStatusListaPrincipal.Text = lblStatusListaPrincipal.Text + " - Pesquisar Tudo";
                        break;

                    case "PESQUISAR_ULTIMA_SEMANA":
                        lblStatusListaPrincipal.Text = lblStatusListaPrincipal.Text + " - Última semana";
                        break;

                    case "PESQUISAR_ULTIMAS_24_HORAS":
                        lblStatusListaPrincipal.Text = lblStatusListaPrincipal.Text + " - Últimas 24 horas";
                        break;

                    case "PESQUISAR_ULTIMAS_72_HORAS":
                        lblStatusListaPrincipal.Text = lblStatusListaPrincipal.Text + " - Últimas 72 horas";
                        break;
                }
                lblStatusListaPrincipal.Refresh();
                Application.DoEvents();
                MessageBox.Show("Erro ao tentar pesquisar: \n" + _Exception.ToString(), "Pesquisar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        private void ListaConflitos()
        {
            string _Query = "";
            string _OwnerNome = "";
            string[] _Split;

            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }

            lblStatusListaPrincipal.Text = "Pesquisando...";
            lblStatusListaPrincipal.Refresh();
            Application.DoEvents();

            _Query = "";
            _Query = _Query + "Select LC.ULTIMA_DATA, ";
            _Query = _Query + "    LC.OBJETO, ";
            _Query = _Query + "    CO.ID AS ID_ANALISTA, ";
            _Query = _Query + "    CO.NOME_COMPLETO AS NOME_ANALISTA, ";
            _Query = _Query + "    CO.EMPRESA, ";
            _Query = _Query + "    LC.OSUSER, "; 
            _Query = _Query + "    LC.CURRENT_USER, "; 
            _Query = _Query + "    LC.HOST ";
            _Query = _Query + "From AESPROD.LISTA_CONFLITOS LC ";
            _Query = _Query + "    Left Join AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
            _Query = _Query + "        On LC.ID_USUARIO = CO.ID ";
            _Query = _Query + "Order By LC.OBJETO, LC.ULTIMA_DATA Desc";

            try
            {
                OracleCommand _Command = new OracleCommand();
                OracleConnection _Connection = new OracleConnection();
                OracleDataReader _DataReader;
                OracleDataReader _DataReaderTipo;
                ListViewItem _ListViewItem = null;
                ListViewItem.ListViewSubItem _ListViewSubItem = null;

                _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();

                lvwObjetos.Items.Clear();
                lvwPesquisaSecundaria.Items.Clear();
                lvwRelatorio.Items.Clear();
                lblPesquisaSecundaria.Text = "";
                _UltimaConsultaSecundaria = "";

                while (_DataReader.Read())
                {
                    _OwnerNome = _DataReader.GetString(1);
                    _Split = _OwnerNome.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    _Split[0] = _Split[0].Trim().ToUpper();
                    _Split[1] = _Split[1].Trim().ToUpper();

                    // Data Hora
                    _ListViewItem = lvwObjetos.Items.Add(_DataReader.GetDateTime(0).ToString("dd/MM/yy HH:mm:ss"));

                    // Tipo
                    _Query = "";
                    _Query = _Query + "Select OBJECT_TYPE ";
                    _Query = _Query + "From ALL_OBJECTS ";
                    _Query = _Query + "Where OWNER = '" + _Split[0] + "' ";
                    _Query = _Query + "    And OBJECT_NAME = '" + _Split[1] + "' ";

                    _Command.CommandText = _Query;
                    _DataReaderTipo = _Command.ExecuteReader();
                    if (_DataReaderTipo.Read())
                    {
                        _ListViewItem.SubItems.Add(_DataReaderTipo.GetString(0));
                    }
                    else
                    {
                        _ListViewItem.SubItems.Add("");
                    }
                    _DataReaderTipo.Close();

                    // Owner
                    _ListViewItem.SubItems.Add(_Split[0]);

                    // Nome
                    _ListViewItem.SubItems.Add(_Split[1]);

                    if (_DataReader.IsDBNull(2))
                    {
                        _ListViewSubItem = _ListViewItem.SubItems.Add(""); // NOME_ANALISTA
                        _ListViewSubItem.Tag = -1; // ID_ANALISTA
                    }
                    else
                    {
                        _ListViewSubItem = _ListViewItem.SubItems.Add(_DataReader.GetString(3)); // NOME_ANALISTA
                        _ListViewSubItem.Tag = _DataReader.GetDecimal(2); // ID_ANALISTA
                    }
                    if (!_DataReader.IsDBNull(4))
                    {
                        _ListViewSubItem = _ListViewItem.SubItems.Add(_DataReader.GetString(4)); // EMPRESA
                    }

                    if (!_DataReader.IsDBNull(5))
                    {
                        _ListViewSubItem = _ListViewItem.SubItems.Add(_DataReader.GetString(5)); // OSUSER
                    }

                    if (!_DataReader.IsDBNull(6))
                    {
                        _ListViewSubItem = _ListViewItem.SubItems.Add(_DataReader.GetString(6)); // CURRENT_USER
                    }

                    if (!_DataReader.IsDBNull(7))
                    {
                        _ListViewSubItem = _ListViewItem.SubItems.Add(_DataReader.GetString(7)); // HOST
                    }
                }

                if (lvwObjetos.Items.Count > 0)
                {
                    for (int i = 0; i < lvwObjetos.Columns.Count; i++)
                    {
                        lvwObjetos.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    }
                }

                _DataReader.Close();
                _DataReader.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                _Command.Dispose();
                GC.Collect();
                lblStatusListaPrincipal.Text = lvwObjetos.Items.Count.ToString() + " itens listados - Conflitos";
                lblStatusListaPrincipal.Refresh();
                Application.DoEvents();
            }
            catch (Exception _Exception)
            {
                lblStatusListaPrincipal.Text = lvwObjetos.Items.Count.ToString() + " itens listados - Conflitos";
                lblStatusListaPrincipal.Refresh();
                Application.DoEvents();
                MessageBox.Show("Erro ao tentar listar os conflitos: \n" + _Exception.ToString(), "Lista Conflitos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ChamaProcedure(string p_NomeProc)
        {
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }

            try
            {
                OracleConnection _Connection = new OracleConnection();
                OracleCommand _Command = new OracleCommand(p_NomeProc, _Connection);
                _Command.CommandType = CommandType.StoredProcedure;

                _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
                _Connection.Open();

                OracleDataAdapter _OracleDataAdapter = new OracleDataAdapter(_Command);
                _Command.ExecuteNonQuery();

                _Connection.Close();
                _Connection.Dispose();
                _Command.Dispose();
                GC.Collect();
            }
            catch (Exception _Exception)
            {
                MessageBox.Show("Erro ao tentar executar a procedure " + p_NomeProc + ":\n" + _Exception.ToString(), "Chama Procedure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PreencheRelatorio(string p_QualRelatorio)
        {
            //Tipo: TIPO
            //Total de objetos alterados:
            //Última compilação:
            //Quantidade de usuários alterando:
            //Quantidade de usuários concorrentes:

            DialogResult _Resp = DialogResult.Cancel;
            _PararPesquisa = false;

            _UltimaConsulta = DateTime.Now;

            lvwRelatorio.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            if (p_QualRelatorio.Trim().Length > 0)
            {
                string[] _Split;
                string _Query = "";
                OracleCommand _Command = new OracleCommand();
                OracleConnection _Connection = new OracleConnection();
                OracleDataReader _DataReader;
                ListViewItem _ListViewItem = null;
                bool _ProcurarNoLOG = true;

                p_QualRelatorio = p_QualRelatorio.Trim().ToUpper();

                if (p_QualRelatorio.IndexOf("ANALISTA:") > -1)
                {
                    _Split = p_QualRelatorio.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (_Split.Length != 2)
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    _Query = "";
                    _Query = _Query + "Select CO.NOME_COMPLETO As Usuario, ";
                    _Query = _Query + "    COUNT(Distinct AU.OWNER || '.' || AU.NOME) As Total_Objetos_Alterados, ";
                    _Query = _Query + "    Max(AU.DATA_HORA) As Ultima_Compilacao, ";
                    _Query = _Query + "    (Select Count(Distinct AU.ID_USUARIO) ";
                    _Query = _Query + "    From AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "    Where AU.OWNER || '.' || AU.NOME In ( ";
                    _Query = _Query + "    Select Distinct AU.OWNER || '.' || AU.NOME  ";
                    _Query = _Query + "    From AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "    Where AU.ID_USUARIO = " + _Split[1].Trim() + ")) -1 As Quant_Obj_Concorrentes ";
                    _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "    Left Join AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
                    _Query = _Query + "        On AU.ID_USUARIO = CO.ID ";
                    _Query = _Query + "Where CO.ID = " + _Split[1].Trim() + " ";
                    _Query = _Query + "Group By CO.NOME_COMPLETO ";

                    _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
                    _Connection.Open();
                    _Command.Connection = _Connection;
                    _Command.CommandType = CommandType.Text;

                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();

                    //Usuário: ID_USUARIO
                    //Total de objetos alterados:
                    //Última compilação:
                    //Quantidade de objetos concorrentes:
                    //Objeto Concorrente => Usuarios Concorrente

                    //decimal _QuantObjConcorrentes = 0;

                    if (_DataReader.Read())
                    {
                        _ListViewItem = lvwRelatorio.Items.Add("Usuário: " + _DataReader.GetString(0));
                        _ListViewItem = lvwRelatorio.Items.Add("Total de objetos alterados: " + _DataReader.GetDecimal(1).ToString());
                        _ListViewItem = lvwRelatorio.Items.Add("Última compilação: " + _DataReader.GetDateTime(2).ToString("dd/MM/yy HH:mm:ss"));
                        //_QuantObjConcorrentes = _DataReader.GetDecimal(3);
                        
                    }

                    _Query = "";
                    _Query = _Query + "Select AU.OWNER || '.' || AU.NOME As Objeto, ";
                    _Query = _Query + "    CO.NOME_COMPLETO ";
                    _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "    Left Join AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
                    _Query = _Query + "        On AU.ID_USUARIO = CO.ID ";
                    _Query = _Query + "Where AU.OWNER || '.' || AU.NOME In ( ";
                    _Query = _Query + "    Select Distinct AU.OWNER || '.' || AU.NOME  ";
                    _Query = _Query + "    From AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "    Where AU.ID_USUARIO = " + _Split[1].Trim() + ") ";
                    _Query = _Query + "    And AU.ID_USUARIO <> " + _Split[1].Trim() + " ";
                    _Query = _Query + "Group By AU.OWNER || '.' || AU.NOME, ";
                    _Query = _Query + "    CO.NOME_COMPLETO ";
                    _Query = _Query + "Order By AU.OWNER || '.' || AU.NOME ";

                    _DataReader.Close();
                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();

                    if (_DataReader.Read())
                    {
                        _ListViewItem = lvwRelatorio.Items.Add("Objetos concorrentes:");
                        _ListViewItem = lvwRelatorio.Items.Add(_DataReader.GetString(0) + " => " + _DataReader.GetString(1));
                    }
                    else
                    {
                        _ListViewItem = lvwRelatorio.Items.Add("Nenhum objeto concorrente");
                    }

                    while (_DataReader.Read())
                    {
                        _ListViewItem = lvwRelatorio.Items.Add(_DataReader.GetString(0) + " => " + _DataReader.GetString(1));
                    }

                    _DataReader.Close();
                    _DataReader.Dispose();
                    _Connection.Close();
                    _Connection.Dispose();
                    _Command.Dispose();
                    GC.Collect();

                    lblStatusRelatorio.Text = "";
                    Application.DoEvents();

                }
                else if (p_QualRelatorio.IndexOf("OBJETO:") > -1)
                {
                    string _InfoUltimoCommitNoTrunk = "";
                    string _CaminhoCompletoNoTrunk = "";

                    _Split = p_QualRelatorio.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (_Split.Length != 2)
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    lblStatusRelatorio.Text = "Buscando informações do objeto " + _Split[1];
                    Application.DoEvents();

                    _Split = _Split[1].Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (_Split.Length != 2)
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    _Query = "";
                    _Query = _Query + "Select AU.OWNER || '.' || AU.NOME As Objeto, ";
                    _Query = _Query + "    Count(*) As Quantidade_Compilacoes, ";
                    _Query = _Query + "    Max(AU.DATA_HORA) As Ultima_Compilacao, ";
                    _Query = _Query + "    (Select Count(Distinct AU.ID_USUARIO) ";
                    _Query = _Query + "        From AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "        Where AU.OWNER || '.' || AU.NOME In ( ";
                    _Query = _Query + "            Select Distinct AU.OWNER || '.' || AU.NOME ";
                    _Query = _Query + "            From AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "            Where AU.OWNER = '" + _Split[0].Trim() + "' ";
                    _Query = _Query + "            And AU.NOME = '" + _Split[1].Trim() + "')) As Quant_Usuarios_Alterando ";
                    _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "Where owner = '" + _Split[0].Trim() + "' ";
                    _Query = _Query + "    And nome = '" + _Split[1].Trim() + "' ";
                    _Query = _Query + "Group By AU.OWNER, AU.NOME ";

                    _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
                    _Connection.Open();
                    _Command.Connection = _Connection;
                    _Command.CommandType = CommandType.Text;

                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();

                    //Objeto: OWNER.NOME
                    //Total de compilações:
                    //Última compilação:
                    //Quantidade de usuários concorrentes:

                    Application.DoEvents();
                    if (_PararPesquisa)
                    {
                        _Resp = MessageBox.Show("Deseja abortar a pesquisa?", "Relatório de objeto", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (_Resp == DialogResult.Yes)
                        {
                            _PararPesquisa = false;
                            return;
                        }
                        else
                        {
                            _PararPesquisa = false;
                        }
                    }

                    if (_DataReader.Read())
                    {
                        _ListViewItem = lvwRelatorio.Items.Add("Objeto: " + _DataReader.GetString(0));

                        if (chkConsultarLogBranch.Checked)
                        {
                            if ((!this.EstaConectadoSVN) && (_ProcurarNoLOG))
                            {
                                if (!this.ConectouNoSVN())
                                {
                                    _ProcurarNoLOG = false;
                                }
                            }

                            if (_ProcurarNoLOG)
                            {
                                _InfoUltimoCommitNoTrunk = this.InfoUltimoCommitNoTrunk(_DataReader.GetString(0), ref _CaminhoCompletoNoTrunk);
                            }
                        }
                        if (_InfoUltimoCommitNoTrunk.Length > 0)
                        {
                            _ListViewItem = lvwRelatorio.Items.Add("Último commit no trunk: " + _InfoUltimoCommitNoTrunk);
                            _ListViewItem.Tag = _CaminhoCompletoNoTrunk;
                        }
                        else
                        {
                            lvwRelatorio.Items.Add("Último commit no trunk: Arquivo não encontrado no trunk");
                        }
                        _ListViewItem = lvwRelatorio.Items.Add("Total de compilações: " + _DataReader.GetDecimal(1).ToString());
                        _ListViewItem = lvwRelatorio.Items.Add("Última compilação: " + _DataReader.GetDateTime(2).ToString("dd/MM/yy HH:mm:ss"));
                        _ListViewItem = lvwRelatorio.Items.Add("Quantidade de usuários alterando: " + _DataReader.GetDecimal(3).ToString());
                    }

                    Application.DoEvents();
                    if (_PararPesquisa)
                    {
                        _Resp = MessageBox.Show("Deseja abortar a pesquisa?", "Relatório de objeto", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (_Resp == DialogResult.Yes)
                        {
                            _PararPesquisa = false;
                            return;
                        }
                        else
                        {
                            _PararPesquisa = false;
                        }
                    }

                    _Query = "";
                    _Query = _Query + "Select Max(AU.DATA_HORA) As Ultima_Compilacao, ";
                    _Query = _Query + "    CO.NOME_COMPLETO ";
                    _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "    Left Join AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
                    _Query = _Query + "        On AU.ID_USUARIO = CO.ID ";
                    _Query = _Query + "Where AU.OWNER = '" + _Split[0].Trim() + "' ";
                    _Query = _Query + "    And AU.NOME = '" + _Split[1].Trim() + "' ";
                    _Query = _Query + "Group By CO.NOME_COMPLETO ";
                    _Query = _Query + "Order By Max(AU.DATA_HORA) Desc ";

                    _DataReader.Close();
                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();

                    while (_DataReader.Read())
                    {
                        Application.DoEvents();
                        if (_PararPesquisa)
                        {
                            _Resp = MessageBox.Show("Deseja abortar a pesquisa?", "Relatório de objeto", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (_Resp == DialogResult.Yes)
                            {
                                _PararPesquisa = false;
                                return;
                            }
                            else
                            {
                                _PararPesquisa = false;
                            }
                        }
                        if (_DataReader.IsDBNull(1))
                        {
                            _ListViewItem = lvwRelatorio.Items.Add("NULL");
                        }
                        else
                        {
                            _ListViewItem = lvwRelatorio.Items.Add(_DataReader.GetDateTime(0).ToString("dd/MM/yy HH:mm:ss") + " - " + _DataReader.GetString(1));
                        }
                    }

                    _DataReader.Close();
                    _DataReader.Dispose();
                    _Connection.Close();
                    _Connection.Dispose();
                    _Command.Dispose();
                    GC.Collect();

                    Application.DoEvents();
                    if (_PararPesquisa)
                    {
                        _Resp = MessageBox.Show("Deseja abortar a pesquisa?", "Relatório de objeto", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (_Resp == DialogResult.Yes)
                        {
                            _PararPesquisa = false;
                            return;
                        }
                        else
                        {
                            _PararPesquisa = false;
                        }
                    }

                    _ListViewItem = lvwRelatorio.Items.Add("");

                    ArrayList _ListaBranches = new ArrayList();
                    string _Mensagem = "";

                    _ListaBranches = this.BranchesQueContemEsteObjeto(_Split[1].Trim(), ref _Mensagem);

                    if (_ListaBranches.Count > 0)
                    {
                        lblStatusRelatorio.Text = "Buscando informações no SVN do objeto " + _Split[0].Trim() + "." + _Split[1].Trim();
                        Application.DoEvents();

                        _ListViewItem = lvwRelatorio.Items.Add("Branches que contém o objeto " + _Split[1].Trim() + ":");
                        
                        string _AuxTexto = "";

                        for (int i = 0; i < _ListaBranches.Count; i++)
                        {
                            Application.DoEvents();
                            if (_PararPesquisa)
                            {
                                _Resp = MessageBox.Show("Deseja abortar a pesquisa?", "Relatório de objeto", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (_Resp == DialogResult.Yes)
                                {
                                    _PararPesquisa = false;
                                    return;
                                }
                                else
                                {
                                    _PararPesquisa = false;
                                }
                            }
                            if (chkConsultarLogBranch.Checked)
                            {
                                if ((!this.EstaConectadoSVN) && (_ProcurarNoLOG))
                                {
                                    if (!this.ConectouNoSVN())
                                    {
                                        _ProcurarNoLOG = false;
                                    }
                                }

                                if (_ProcurarNoLOG)
                                {
                                    string _UsuarioUltimoCommit = "";
                                    string _MensagemErro = "";
                                    lblStatusRelatorio.Text = "Buscando informações no último commit do branch " + _ListaBranches[i].ToString();
                                    Application.DoEvents();
                                    DateTime _UltimoCommit = this.DataUltimoCommit(_ListaBranches[i].ToString(), ref _UsuarioUltimoCommit, ref _MensagemErro);
                                    
                                    _AuxTexto = _ListaBranches[i].ToString().Replace("branches/", "");
                                    _AuxTexto = _AuxTexto.Replace("tags/", "");
                                    _AuxTexto = _AuxTexto.Substring(0, 8);
                                    _AuxTexto = _AuxTexto.Substring(6) + "/" + _AuxTexto.Substring(4, 2) + "/" + _AuxTexto.Substring(0, 4);
                                    try
                                    {
                                        DateTime _DataCriacaoBranch = DateTime.Parse(_AuxTexto);
                                        if (_UltimoCommit > _DataCriacaoBranch)
                                        {
                                            _UsuarioUltimoCommit = this.DadosUsuario(_UsuarioUltimoCommit);
                                            _ListViewItem = lvwRelatorio.Items.Add(_ListaBranches[i].ToString());
                                            _ListViewItem.Tag = _URL_SVN + "/branches/" + _ListaBranches[i].ToString();
                                            _ListViewItem = lvwRelatorio.Items.Add("Último commit: " + _UltimoCommit.ToString("dd/MM/yy HH:mm:ss") + " - " + _UsuarioUltimoCommit);
                                        }
                                        else
                                        {
                                            _ListViewItem = lvwRelatorio.Items.Add(_ListaBranches[i].ToString());
                                            _ListViewItem.Tag = _URL_SVN + "/branches/" + _ListaBranches[i].ToString();
                                            if (_MensagemErro.Trim().Length > 0)
                                            {
                                                _ListViewItem = lvwRelatorio.Items.Add(_MensagemErro);
                                            }
                                            else
                                            {
                                                _ListViewItem = lvwRelatorio.Items.Add("Nunca teve commit");
                                            }

                                        }
                                    }
                                    catch (Exception)
                                    {
                                        
                                    }
                                    
                                }
                                else
                                {
                                    _ListViewItem = lvwRelatorio.Items.Add(_ListaBranches[i].ToString());
                                    _ListViewItem.Tag = _URL_SVN + "/branches/" + _ListaBranches[i].ToString();
                                }
                            }
                            else
                            {
                                _ListViewItem = lvwRelatorio.Items.Add(_ListaBranches[i].ToString());
                                _ListViewItem.Tag = _URL_SVN + "/branches/" + _ListaBranches[i].ToString();
                            }

                            _AuxTexto = _ListaBranches[i].ToString();
                            _AuxTexto = _AuxTexto.Substring(0, _AuxTexto.IndexOf("/"));
                            _ListViewItem = lvwRelatorio.Items.Add("Usuários com permissão de escrita no branch:");

                            Application.DoEvents();
                            if (_PararPesquisa)
                            {
                                _Resp = MessageBox.Show("Deseja abortar a pesquisa?", "Relatório de objeto", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (_Resp == DialogResult.Yes)
                                {
                                    _PararPesquisa = false;
                                    return;
                                }
                                else
                                {
                                    _PararPesquisa = false;
                                }
                            }

                            ArrayList _ListaUsuariosPermissaoBranch = this.UsuariosPermissaoBranch(_AuxTexto);
                            if (_ListaUsuariosPermissaoBranch.Count > 0)
                            {
                                for (int j = 0; j < _ListaUsuariosPermissaoBranch.Count; j++)
                                {
                                    _ListViewItem = lvwRelatorio.Items.Add(_ListaUsuariosPermissaoBranch[j].ToString());
                                }
                            }
                            else
                            {
                                _ListViewItem = lvwRelatorio.Items.Add("Ninguém");
                            }

                            _ListViewItem = lvwRelatorio.Items.Add("");

                        }
                    }
                    else
                    {
                        if (_Mensagem.Trim().Length > 0)
                        {
                            _ListViewItem = lvwRelatorio.Items.Add(_Mensagem);
                        }
                        else
                        {
                            _ListViewItem = lvwRelatorio.Items.Add("Nenhum branch encontrado contendo o ojeto " + _Split[0].Trim() + "." + _Split[1].Trim());
                        }
                    }

                    lblStatusRelatorio.Text = "";
                    Application.DoEvents();

                }
                else if (p_QualRelatorio.IndexOf("OWNER:") > -1)
                {
                    _Split = p_QualRelatorio.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (_Split.Length != 2)
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    lblStatusRelatorio.Text = "Buscando informações no owner " + _Split[1].Trim();
                    Application.DoEvents();

                    _Query = "";
                    _Query = _Query + "Select Count(Distinct AU.NOME) As Total_Objetos_Alterados, ";
                    _Query = _Query + "    Max(AU.DATA_HORA) As Ultima_Compilacao, ";
                    _Query = _Query + "    Count(Distinct AU.ID_USUARIO) As Quantidade_Usuarios_Alterando ";
                    _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "Where AU.OWNER = '" + _Split[1].Trim() + "' ";

                    _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
                    _Connection.Open();
                    _Command.Connection = _Connection;
                    _Command.CommandType = CommandType.Text;

                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();

                    //Owner: OWNER
                    //Total de objetos alterados:
                    //Última compilação:
                    //Quantidade de usuários alterando:
                    //Quantidade de usuários concorrentes:

                    if (_DataReader.Read())
                    {
                        _ListViewItem = lvwRelatorio.Items.Add("Owner: " + _Split[1].Trim());
                        _ListViewItem = lvwRelatorio.Items.Add("Total de objetos alterados: " + _DataReader.GetDecimal(0).ToString());
                        _ListViewItem = lvwRelatorio.Items.Add("Última compilação: " + _DataReader.GetDateTime(1).ToString("dd/MM/yy HH:mm:ss"));
                        _ListViewItem = lvwRelatorio.Items.Add("Quantidade de usuários alterando: " + _DataReader.GetDecimal(2).ToString());
                    }

                    _Query = "";
                    _Query = _Query + "Select Max(AU.DATA_HORA) As Ultima_Compilacao, ";
                    _Query = _Query + "    CO.NOME_COMPLETO ";
                    _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "    Left Join AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
                    _Query = _Query + "        On AU.ID_USUARIO = CO.ID ";
                    _Query = _Query + "Where AU.OWNER = '" + _Split[1].Trim() + "' ";
                    _Query = _Query + "Group By CO.NOME_COMPLETO ";
                    _Query = _Query + "Order By Max(AU.DATA_HORA) Desc ";

                    _DataReader.Close();
                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();

                    while (_DataReader.Read())
                    {
                        if (_DataReader.IsDBNull(1))
                        {
                            _ListViewItem = lvwRelatorio.Items.Add("NULL");
                        }
                        else
                        {
                            _ListViewItem = lvwRelatorio.Items.Add(_DataReader.GetDateTime(0).ToString("dd/MM/yy HH:mm:ss") + " - " + _DataReader.GetString(1));
                        }
                    }

                    _DataReader.Close();
                    _DataReader.Dispose();
                    _Connection.Close();
                    _Connection.Dispose();
                    _Command.Dispose();
                    GC.Collect();

                    lblStatusRelatorio.Text = "";
                    Application.DoEvents();

                }
                else if (p_QualRelatorio == "GERAL")
                {
                    lblStatusRelatorio.Text = "Buscando informações gerais";
                    Application.DoEvents();

                    _Query = "";
                    _Query = _Query + "Select Count(Distinct AU.NOME) As Total_Objetos_Alterados, ";
                    _Query = _Query + "    Max(AU.DATA_HORA) As Ultima_Compilacao, ";
                    _Query = _Query + "    Count(Distinct AU.ID_USUARIO) As Quantidade_Usuarios_Alterando ";
                    _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";

                    _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
                    _Connection.Open();
                    _Command.Connection = _Connection;
                    _Command.CommandType = CommandType.Text;

                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();

                    //Geral
                    //Total de objetos alterados:
                    //Última compilação:
                    //Quantidade de usuários alterando:

                    if (_DataReader.Read())
                    {
                        _ListViewItem = lvwRelatorio.Items.Add("Geral");
                        _ListViewItem = lvwRelatorio.Items.Add("Total de objetos alterados: " + _DataReader.GetDecimal(0).ToString());
                        _ListViewItem = lvwRelatorio.Items.Add("Última compilação: " + _DataReader.GetDateTime(1).ToString("dd/MM/yy HH:mm:ss"));
                        _ListViewItem = lvwRelatorio.Items.Add("Quantidade de usuários alterando: " + _DataReader.GetDecimal(2).ToString());
                    }

                    _Query = "";
                    _Query = _Query + "Select Distinct CO.NOME_COMPLETO ";
                    _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "    Left Join AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
                    _Query = _Query + "        On AU.ID_USUARIO = CO.ID ";
                    _Query = _Query + "Order By CO.NOME_COMPLETO ";

                    _DataReader.Close();
                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();

                    while (_DataReader.Read())
                    {
                        if (_DataReader.IsDBNull(0))
                        {
                            _ListViewItem = lvwRelatorio.Items.Add("NULL");
                        }
                        else
                        {
                            _ListViewItem = lvwRelatorio.Items.Add("- " + _DataReader.GetString(0));
                        }
                    }

                    _DataReader.Close();
                    _DataReader.Dispose();
                    _Connection.Close();
                    _Connection.Dispose();
                    _Command.Dispose();
                    GC.Collect();

                    lblStatusRelatorio.Text = "";
                    Application.DoEvents();

                }
            }
            Cursor.Current = Cursors.Default;
        }

        private string InfoUltimoCommitNoTrunk(string p_OwnerNomeObjeto, ref string p_CaminhoCompletoNoTrunk)
        {
            string _Retorno = "";
            string _CaminhoArquivoListaTrunkLINUX = csUtil.CarregarPreferencia("CaminhoListagemTrunkLINUX").ToString(); // "/apl/software/Todos_arquivos_trunk.txt";
            string _NomeArquivoListaTrunk = csUtil.CarregarPreferencia("CaminhoListagemTrunkLOCAL").ToString();  // csUtil.PastaLocalExecutavel() + "Todos_arquivos_trunk.txt";
            string _CaminhoCompletoNoTrunk = "";
            p_CaminhoCompletoNoTrunk = "";

            if (!this.EstaConectadoFTP)
            {
                if (this.ConectouNoFTP())
                {
                    SessionOptions _SessionOptions = new SessionOptions();

                    lblStatusRelatorio.Text = "Conectando no FTP";
                    Application.DoEvents();

                    switch (_Protocolo_FTP)
                    {
                        case "FTP":
                            _SessionOptions.Protocol = Protocol.Ftp;
                            break;
                        case "SCP":
                            _SessionOptions.Protocol = Protocol.Scp;
                            break;
                        case "SFTP":
                            _SessionOptions.Protocol = Protocol.Sftp;
                            break;
                        case "WEBDAV":
                            _SessionOptions.Protocol = Protocol.Webdav;
                            break;
                        default:
                            _SessionOptions.Protocol = Protocol.Sftp;
                            break;
                    }

                    _SessionOptions.HostName = _Host_FTP;
                    _SessionOptions.UserName = _Usuario_FTP;
                    _SessionOptions.Password = _Senha_FTP;
                    _SessionOptions.SshHostKeyFingerprint = _Fingerprint_FTP;

                    Session _Session = new Session();
                    _Session.Open(_SessionOptions);

                    lblStatusRelatorio.Text = "Verificando datas dos arquivos";
                    Application.DoEvents();

                    RemoteFileInfo _RemoteFileInfo = _Session.GetFileInfo(_CaminhoArquivoListaTrunkLINUX);

                    Cursor.Current = Cursors.WaitCursor;

                    if (File.Exists(_NomeArquivoListaTrunk))
                    {
                        if (File.GetLastWriteTime(_NomeArquivoListaTrunk) < _RemoteFileInfo.LastWriteTime)
                        {
                            lblStatusRelatorio.Text = "Baixando arquivo de tamanho " + _RemoteFileInfo.Length.ToString("#,000") + " bytes";
                            Application.DoEvents();

                            TransferOptions transferOptions = new TransferOptions();
                            transferOptions.TransferMode = TransferMode.Binary;
                            TransferOperationResult transferResult;
                            transferResult = _Session.GetFiles(_CaminhoArquivoListaTrunkLINUX, _NomeArquivoListaTrunk, false, transferOptions);
                        }
                    }
                    else
                    {
                        lblStatusRelatorio.Text = "Baixando arquivo de tamanho " + _RemoteFileInfo.Length.ToString("#,000") + " bytes";
                        Application.DoEvents();

                        TransferOptions transferOptions = new TransferOptions();
                        transferOptions.TransferMode = TransferMode.Binary;
                        TransferOperationResult transferResult;
                        transferResult = _Session.GetFiles(_CaminhoArquivoListaTrunkLINUX, _NomeArquivoListaTrunk, false, transferOptions);
                    }

                    if (_Session.Opened)
                    {
                        _Session.Close();
                    }
                    _Session.Dispose();

                    Cursor.Current = Cursors.Default;

                    _JaAtualizouArquivoListaTrunk = true;
                    lblStatusRelatorio.Text = "";
                    Application.DoEvents();
                }
                else
                {
                    _JaAtualizouArquivoListaTrunk = true;
                }
            }
            else
            {
                _JaAtualizouArquivoListaTrunk = true;
            }

            if (File.Exists(_NomeArquivoListaTrunk))
            {
                string _ConteudoArquivoListaTrunk = File.ReadAllText(_NomeArquivoListaTrunk, Encoding.Default);
                string[] _Split;
                string[] _Split2;
                int _Pos = -1;
                _ConteudoArquivoListaTrunk = _ConteudoArquivoListaTrunk.Replace("\r", "");
                _Split = _ConteudoArquivoListaTrunk.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < _Split.Length; i++)
                {
                    _Pos = _Split[i].ToUpper().IndexOf(p_OwnerNomeObjeto.ToUpper());
                    if (_Pos > -1)
                    {
                        _CaminhoCompletoNoTrunk = _Split[i];
                        _CaminhoCompletoNoTrunk = _CaminhoCompletoNoTrunk.Substring(0, _CaminhoCompletoNoTrunk.Length - 4);
                        _Split2 = _CaminhoCompletoNoTrunk.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (p_OwnerNomeObjeto.ToUpper() == _Split2[_Split2.Length -1].ToUpper())
                        {
                            _CaminhoCompletoNoTrunk = _Split[i];
                            break;
                        }
                        else
                        {
                            _CaminhoCompletoNoTrunk = "";
                        }
                    }
                }
            }

            if (_CaminhoCompletoNoTrunk.Trim().Length > 0)
            {
                bool _ProcurarNoLOG = true;
                if (chkConsultarLogBranch.Checked)
                {
                    if ((!this.EstaConectadoSVN) && (_ProcurarNoLOG))
                    {
                        if (!this.ConectouNoSVN())
                        {
                            _ProcurarNoLOG = false;
                        }
                    }

                    if (_ProcurarNoLOG)
                    {
                        SvnClient _SvnClient = new SvnClient();
                        System.Collections.ObjectModel.Collection<SvnLogEventArgs> _ListaLog;
                        p_CaminhoCompletoNoTrunk = _URL_SVN + "/trunk/" + _CaminhoCompletoNoTrunk;
                        Uri _Uri = new Uri(p_CaminhoCompletoNoTrunk);
                        DateTime _DataUltimoCommit = DateTime.MinValue;
                        string _LogMessage = "";
                        string[] _Split = null;
                        try
                        {
                            if (_SvnClient.GetLog(_Uri, out _ListaLog))
                            {
                                foreach (SvnLogEventArgs _SvnLog in _ListaLog)
                                {
                                    _DataUltimoCommit = _SvnLog.Time;
                                    _DataUltimoCommit = _DataUltimoCommit.Add(new TimeSpan(-3, 0, 0));
                                    _Retorno = _DataUltimoCommit.ToString("dd/MM/yy HH:mm:ss");
                                    if (_SvnLog.Revision == 352)
                                    {
                                        _Retorno = _Retorno + " - Carga inicial no SVN";
                                    }
                                    else
                                    {
                                        _LogMessage = _SvnLog.LogMessage;
                                        _LogMessage = _LogMessage.Replace("\r", "");
                                        _Split = _LogMessage.Split("\n".ToCharArray(), StringSplitOptions.None);
                                        for (int i = 0; i < _Split.Length; i++)
                                        {
                                            if ((_Split[i].IndexOf("Mudança Nº") > -1) || (_Split[i].IndexOf("Change Nº") > -1))
                                            {
                                                _Retorno = _Retorno + " - " + _Split[i];
                                                break;
                                            }
                                        }
                                    }
                                    
                                    break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // Faz nada
                        }
                    }
                }
            }
            return _Retorno;
        }

        private ArrayList BranchesQueContemEsteObjeto(string p_NomeObjeto, ref string p_Mensagem)
        {
            string _CaminhoArquivoListaBranchesLINUX = csUtil.CarregarPreferencia("CaminhoListagemBranchesLINUX").ToString(); // "/apl/software/Todos_arquivos_branches.txt";

            p_Mensagem = "";
            ArrayList _Retorno = new ArrayList();
            string _NomeArquivoListaBranches = csUtil.CarregarPreferencia("CaminhoListagemBranchesLOCAL").ToString(); //csUtil.PastaLocalExecutavel() + "Todos_arquivos_branches.txt";
            //string _ArquivoSVNTools = @"D:\SVN_AES\_Apoio\Ferramentas\SVN_Tools\SVN_Tools\bin\Release\Todos_arquivos_branches.txt";

            //if (File.Exists(_NomeArquivoListaBranches))
            //{
            //    if (File.GetLastWriteTime(_NomeArquivoListaBranches) >= DateTime.Now.AddDays(-1))
            //    {
            //        _JaAtualizouArquivoListaBranches = true;
            //    }
            //}

            if (!_JaAtualizouArquivoListaBranches)
            {
                //if (File.Exists(_ArquivoSVNTools) && File.Exists(_NomeArquivoListaBranches))
                //{
                //    if (File.GetLastWriteTime(_ArquivoSVNTools) > File.GetLastWriteTime(_NomeArquivoListaBranches))
                //    {
                //        File.Copy(_ArquivoSVNTools, _NomeArquivoListaBranches, true);
                //    }
                //}

                if (!this.EstaConectadoFTP)
                {
                    if (this.ConectouNoFTP())
                    {
                        SessionOptions _SessionOptions = new SessionOptions();

                        lblStatusRelatorio.Text = "Conectando no FTP";
                        Application.DoEvents();

                        switch (_Protocolo_FTP)
                        {
                            case "FTP":
                                _SessionOptions.Protocol = Protocol.Ftp;
                                break;
                            case "SCP":
                                _SessionOptions.Protocol = Protocol.Scp;
                                break;
                            case "SFTP":
                                _SessionOptions.Protocol = Protocol.Sftp;
                                break;
                            case "WEBDAV":
                                _SessionOptions.Protocol = Protocol.Webdav;
                                break;
                            default:
                                _SessionOptions.Protocol = Protocol.Sftp;
                                break;
                        }

                        _SessionOptions.HostName = _Host_FTP;
                        _SessionOptions.UserName = _Usuario_FTP;
                        _SessionOptions.Password = _Senha_FTP;
                        _SessionOptions.SshHostKeyFingerprint = _Fingerprint_FTP;

                        Session _Session = new Session();
                        _Session.Open(_SessionOptions);

                        lblStatusRelatorio.Text = "Verificando datas dos arquivos";
                        Application.DoEvents();

                        RemoteFileInfo _RemoteFileInfo = _Session.GetFileInfo(_CaminhoArquivoListaBranchesLINUX);

                        Cursor.Current = Cursors.WaitCursor;

                        if (File.Exists(_NomeArquivoListaBranches))
                        {
                            if (File.GetLastWriteTime(_NomeArquivoListaBranches) < _RemoteFileInfo.LastWriteTime)
                            {
                                lblStatusRelatorio.Text = "Baixando arquivo de tamanho " + _RemoteFileInfo.Length.ToString("#,000") + " bytes";
                                Application.DoEvents();

                                TransferOptions transferOptions = new TransferOptions();
                                transferOptions.TransferMode = TransferMode.Binary;
                                TransferOperationResult transferResult;
                                transferResult = _Session.GetFiles(_CaminhoArquivoListaBranchesLINUX, _NomeArquivoListaBranches, false, transferOptions);
                            }
                        }
                        else
                        {
                            lblStatusRelatorio.Text = "Baixando arquivo de tamanho " + _RemoteFileInfo.Length.ToString("#,000") + " bytes";
                            Application.DoEvents();

                            TransferOptions transferOptions = new TransferOptions();
                            transferOptions.TransferMode = TransferMode.Binary;
                            TransferOperationResult transferResult;
                            transferResult = _Session.GetFiles(_CaminhoArquivoListaBranchesLINUX, _NomeArquivoListaBranches, false, transferOptions);
                        }

                        if (_Session.Opened)
                        {
                            _Session.Close();
                        }
                        _Session.Dispose();

                        Cursor.Current = Cursors.Default;

                        _JaAtualizouArquivoListaBranches = true;
                        lblStatusRelatorio.Text = "";
                        Application.DoEvents();
                    }
                    else
                    {
                        _JaAtualizouArquivoListaBranches = true;
                    }
                }
                else
                {
                    SessionOptions _SessionOptions = new SessionOptions();

                    lblStatusRelatorio.Text = "Conectando no FTP";
                    Application.DoEvents();

                    switch (_Protocolo_FTP)
                    {
                        case "FTP":
                            _SessionOptions.Protocol = Protocol.Ftp;
                            break;
                        case "SCP":
                            _SessionOptions.Protocol = Protocol.Scp;
                            break;
                        case "SFTP":
                            _SessionOptions.Protocol = Protocol.Sftp;
                            break;
                        case "WEBDAV":
                            _SessionOptions.Protocol = Protocol.Webdav;
                            break;
                        default:
                            _SessionOptions.Protocol = Protocol.Sftp;
                            break;
                    }

                    _SessionOptions.HostName = _Host_FTP;
                    _SessionOptions.UserName = _Usuario_FTP;
                    _SessionOptions.Password = _Senha_FTP;
                    _SessionOptions.SshHostKeyFingerprint = _Fingerprint_FTP;

                    Session _Session = new Session();
                    _Session.Open(_SessionOptions);

                    lblStatusRelatorio.Text = "Verificando datas dos arquivos";
                    Application.DoEvents();

                    RemoteFileInfo _RemoteFileInfo = _Session.GetFileInfo(_CaminhoArquivoListaBranchesLINUX);

                    Cursor.Current = Cursors.WaitCursor;

                    if (File.Exists(_NomeArquivoListaBranches))
                    {
                        if (File.GetLastWriteTime(_NomeArquivoListaBranches) < _RemoteFileInfo.LastWriteTime)
                        {
                            lblStatusRelatorio.Text = "Baixando arquivo de tamanho " + _RemoteFileInfo.Length.ToString("#,000") + " bytes";
                            Application.DoEvents();

                            TransferOptions transferOptions = new TransferOptions();
                            transferOptions.TransferMode = TransferMode.Binary;
                            TransferOperationResult transferResult;
                            transferResult = _Session.GetFiles(_CaminhoArquivoListaBranchesLINUX, _NomeArquivoListaBranches, false, transferOptions);
                        }
                    }
                    else
                    {
                        lblStatusRelatorio.Text = "Baixando arquivo de tamanho " + _RemoteFileInfo.Length.ToString("#,000") + " bytes";
                        Application.DoEvents();

                        TransferOptions transferOptions = new TransferOptions();
                        transferOptions.TransferMode = TransferMode.Binary;
                        TransferOperationResult transferResult;
                        transferResult = _Session.GetFiles(_CaminhoArquivoListaBranchesLINUX, _NomeArquivoListaBranches, false, transferOptions);
                    }

                    if (_Session.Opened)
                    {
                        _Session.Close();
                    }
                    _Session.Dispose();

                    Cursor.Current = Cursors.Default;

                    _JaAtualizouArquivoListaBranches = true;
                    lblStatusRelatorio.Text = "";
                    Application.DoEvents();
                }
            }
            
            if (File.Exists(_NomeArquivoListaBranches))
            {
                string _ConteudoArquivoListaBranches = File.ReadAllText(_NomeArquivoListaBranches, Encoding.Default);
                string[] _Split;
                string _StringTeste = "";
                int _Pos = -1;
                _ConteudoArquivoListaBranches = _ConteudoArquivoListaBranches.Replace("\r", "");
                _Split = _ConteudoArquivoListaBranches.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < _Split.Length; i++)
                {
                    _Pos = _Split[i].ToUpper().IndexOf(p_NomeObjeto.ToUpper());
                    if (_Pos > -1)
                    {
                        _StringTeste = _Split[i].ToUpper().Substring(_Pos);
                        if (_StringTeste.Length == (p_NomeObjeto.Length + 4))
                        {
                            _Retorno.Add(_Split[i]);
                        }
                    }
                }
            }
            else
            {
                p_Mensagem = "Arquivo de listagem de branches não encontrado! - " + _NomeArquivoListaBranches;
            }
            return _Retorno;
        }

        private string CabecalhoRelatorioHtml()
        {
            string _Retorno = "";

            object _ObjPastaImagensRelatoriosHtml = csUtil.CarregarPreferencia("PastaImagensRelatoriosHtml");
            string _PastaImagensRelatoriosHtml = "";
            if (_ObjPastaImagensRelatoriosHtml != null)
            {
                _PastaImagensRelatoriosHtml = _ObjPastaImagensRelatoriosHtml.ToString().Trim();
                if (Directory.Exists(_PastaImagensRelatoriosHtml))
                {
                    if (_PastaImagensRelatoriosHtml.Substring(_PastaImagensRelatoriosHtml.Length - 1, 1) != @"\")
                    {
                        _PastaImagensRelatoriosHtml = _PastaImagensRelatoriosHtml + @"\";
                    }
                }
                else
                {
                    _PastaImagensRelatoriosHtml = "";
                }
            }

            _Retorno = _Retorno + "<html>\r\n";
            _Retorno = _Retorno + "<head>\r\n";
            _Retorno = _Retorno + "<style type=\"text/css\">\r\n";
            _Retorno = _Retorno + "  body {font-family: sean-serif, Georgia, \"Times New Roman\", Times, serif;}\r\n";
            _Retorno = _Retorno + "  th {font-weight: normal; padding: 0px 6px 0px 6px;}\r\n";
            _Retorno = _Retorno + "  </style>\r\n";
            _Retorno = _Retorno + "</head>\r\n";
            if (_PastaImagensRelatoriosHtml.Length > 0)
            {
                _Retorno = _Retorno + "<LEFT><img src=\"" + _PastaImagensRelatoriosHtml + "cabecalho_relatorio_html.png\"></LEFT>\r\n";
            }
            _Retorno = _Retorno + "<br/>\r\n";
            _Retorno = _Retorno + "<br/>\r\n";
            _Retorno = _Retorno + "<b>Relatório de controle de concorrência</b><br/>\r\n";
            _Retorno = _Retorno + "<br/>\r\n";
            _Retorno = _Retorno + "<b>" + lblPesquisaSecundaria.Text + "</b><br/>\r\n";
            _Retorno = _Retorno + "<br/>\r\n";
            _Retorno = _Retorno + "<table frame=\"box\" rules=\"all\">\r\n";

            return _Retorno;
        }

        private string RodapeRelatorioHtml()
        {
            string _Retorno = "";
            string _Mensagem = "";
            string _InfoBanco = "";
            string[] _Split;

            _Retorno = _Retorno + "<b>Informações do banco de dados</b><br/>\r\n";
            _Retorno = _Retorno + "Data e hora da consulta: " + _UltimaConsulta.ToString("dd/MM/yy HH:mm:ss") + "<br/>\r\n";
            _InfoBanco = _csOracle.InfoBanco(_Username, _Password, _Database, ref _Mensagem);
            if (_Mensagem.Length == 0)
            {
                _Split = _InfoBanco.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 1; i < _Split.Length; i++)
                {
                    _Retorno = _Retorno + _Split[i].Trim() + "<br/>\r\n";
                }
            }
            return _Retorno;
        }

        private void MostraRelatorioHtml()
        {
            string _Relatorio = this.CabecalhoRelatorioHtml();

            _Relatorio = _Relatorio + "<tr>";
            for (int _IndiceColuna = 0; _IndiceColuna < lvwPesquisaSecundaria.Columns.Count; _IndiceColuna++)
            {
                _Relatorio = _Relatorio + "<th align=\"center\"><b>" + lvwPesquisaSecundaria.Columns[_IndiceColuna].Text + "</b></th>";
            }
            _Relatorio = _Relatorio + "</tr>\r\n";

            for (int _IndiceLinha = 0; _IndiceLinha < lvwPesquisaSecundaria.Items.Count; _IndiceLinha++)
            {
                _Relatorio = _Relatorio + "<tr>";
                for (int _IndiceColuna = 0; _IndiceColuna < lvwPesquisaSecundaria.Columns.Count; _IndiceColuna++)
                {
                    _Relatorio = _Relatorio + "<th align=\"left\">" + lvwPesquisaSecundaria.Items[_IndiceLinha].SubItems[_IndiceColuna].Text + "</th>";
                }
                _Relatorio = _Relatorio + "</tr>\r\n";
            }
            _Relatorio = _Relatorio + "</table>\r\n";
            _Relatorio = _Relatorio + "<br/><br/>\r\n";

            for (int _IndiceLinha = 0; _IndiceLinha < lvwRelatorio.Items.Count; _IndiceLinha++)
            {
                for (int _IndiceColuna = 0; _IndiceColuna < lvwRelatorio.Columns.Count; _IndiceColuna++)
                {
                    _Relatorio = _Relatorio + lvwRelatorio.Items[_IndiceLinha].SubItems[_IndiceColuna].Text + "<br/>\r\n";
                }
            }
            _Relatorio = _Relatorio + "<br/><br/>\r\n";
            _Relatorio = _Relatorio + this.RodapeRelatorioHtml();
            csUtil.SalvarEAbrir(_Relatorio, "RelatorioControleConcorrencia.htm");
            GC.Collect();
        }

        private void MostraRelatorioHtmlDbUser(string p_NomeDbUser)
        {

            string _Relatorio = "";
            //string _IdsAnalistas = "";

            _Relatorio = _Relatorio + "<html>\r\n";
            _Relatorio = _Relatorio + "<head>\r\n";
            _Relatorio = _Relatorio + "<style type=\"text/css\">\r\n";
            _Relatorio = _Relatorio + "  body {font-family: sean-serif, Georgia, \"Times New Roman\", Times, serif;}\r\n";
            _Relatorio = _Relatorio + "  th {font-weight: normal; padding: 0px 6px 0px 6px;}\r\n";
            _Relatorio = _Relatorio + "  </style>\r\n";
            _Relatorio = _Relatorio + "</head>\r\n";
            _Relatorio = _Relatorio + "<br/>\r\n";
            _Relatorio = _Relatorio + "<b>Analistas que estão utilizando o DB User " + p_NomeDbUser + "</b><br/>\r\n";
            _Relatorio = _Relatorio + "<br/>\r\n";
            _Relatorio = _Relatorio + "<table frame=\"box\" rules=\"all\">\r\n";

            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            _Query = "";
            _Query = _Query + "SELECT DISTINCT ";
            _Query = _Query + "    CO.NOME_COMPLETO, ";
            _Query = _Query + "    CO.EMAIL ";
            _Query = _Query + "FROM AESPROD.AUDITORIA_OBJETOS AU ";
            _Query = _Query + "    INNER JOIN AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
            _Query = _Query + "    ON AU.ID_USUARIO = CO.ID ";
            _Query = _Query + "WHERE AU.CURRENT_USER = '" + p_NomeDbUser + "' ";

            _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
            _Connection.Open();
            _Command.Connection = _Connection;
            _Command.CommandType = CommandType.Text;

            _Command.CommandText = _Query;
            _DataReader = _Command.ExecuteReader();

            while (_DataReader.Read())
            {
                _Relatorio = _Relatorio + _DataReader.GetString(0) + "&lt;" + _DataReader.GetString(1) + "&gt;<br/>\r\n";
            }

            _DataReader.Close();
            _DataReader.Dispose();
            _Connection.Close();
            _Connection.Dispose();
            _Command.Dispose();
            GC.Collect();
            
            _Relatorio = _Relatorio + "<br/><br/>\r\n";

            csUtil.SalvarEAbrir(_Relatorio, "RelatorioControleConcorrencia.htm");
            GC.Collect();
        }

        public DateTime DataUltimoCommit(string p_Caminho, ref string p_UsuarioUltimoCommit, ref string p_MensagemErro)
        {
            p_MensagemErro = "";
            DateTime _UltimoCommit = DateTime.MinValue;
            SvnClient _SvnClient = new SvnClient();
            p_UsuarioUltimoCommit = "";
            System.Collections.ObjectModel.Collection<SvnLogEventArgs> _ListaLog;
            Uri _Uri = new Uri(_URL_SVN + "/branches/" + p_Caminho);

            try
            {
                if (_SvnClient.GetLog(_Uri, out _ListaLog))
                {
                    foreach (SvnLogEventArgs _SvnLog in _ListaLog)
                    {
                        if (_SvnLog.Author.Trim().ToUpper() != "CON14103" && _SvnLog.Author.Trim().ToUpper() != "CON12434")
                        {
                            _UltimoCommit = _SvnLog.Time;
                            _UltimoCommit = _UltimoCommit.Add(new TimeSpan(-3, 0, 0));
                            p_UsuarioUltimoCommit = _SvnLog.Author.Trim().ToUpper();

                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Source == "SharpSvn" && e.Message.IndexOf("path not found") > -1)
                {
                    p_MensagemErro = "O arquivo foi excluído do SVN";
                }
                else
                {
                    p_MensagemErro = e.Message;
                }
            }
            return _UltimoCommit;
        }

        private ArrayList UsuariosPermissaoBranch(string p_NomeBranch)
        {
            ArrayList _Retorno = new ArrayList();
            string _NomeArquivoPermissoes = "";
            string _ConteudoArquivoPermissoes = "";

            _NomeArquivoPermissoes = @"D:\SVN_AES\_Apoio\Access_fontes\access.conf";

            if (File.Exists(_NomeArquivoPermissoes))
            {
                _ConteudoArquivoPermissoes = File.ReadAllText(_NomeArquivoPermissoes, Encoding.Default);
                _ConteudoArquivoPermissoes = _ConteudoArquivoPermissoes.Replace("\r", "");
                _ConteudoArquivoPermissoes = _ConteudoArquivoPermissoes.ToUpper();
                p_NomeBranch = p_NomeBranch.Trim().ToUpper();

                int _PosIni = _ConteudoArquivoPermissoes.IndexOf(p_NomeBranch);
                if (_PosIni > -1)
                {
                    int _PosFim = _ConteudoArquivoPermissoes.IndexOf("*=", _PosIni);
                    if (_PosFim > -1)
                    {
                        _ConteudoArquivoPermissoes = _ConteudoArquivoPermissoes.Substring(_PosIni, _PosFim - _PosIni);
                        string[] _Split;
                        string _Linha = "";
                        _Split = _ConteudoArquivoPermissoes.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (_Split.Length > 4)
                        {
                            for (int i = 1; i < _Split.Length - 3; i++)
                            {
                                _Linha = _Split[i];
                                _Linha = _Linha.Substring(0, 8);
                                _Linha = this.DadosUsuario(_Linha);
                                _Retorno.Add(_Linha);
                            }
                        }
                    }
                }

            }
            
            return _Retorno;
        }

        private string DadosUsuario(string p_CON)
        {
            string _Retorno = p_CON;
            string _NomeArquivoListaUsuarios = "";

            _NomeArquivoListaUsuarios = @"D:\SVN_AES\_Apoio\Access_fontes\Lista_Usuarios.txt";

            if (File.Exists(_NomeArquivoListaUsuarios))
            {
                _ConteudoArquivoUsuarios = File.ReadAllText(_NomeArquivoListaUsuarios, Encoding.Default);
                _ConteudoArquivoUsuarios = _ConteudoArquivoUsuarios.Replace("\r", "");
            }
            else
	        {
                _NomeArquivoListaUsuarios = csUtil.PastaLocalExecutavel() + "Lista_Usuarios.txt";
                if (File.Exists(_NomeArquivoListaUsuarios))
                {
                    _ConteudoArquivoUsuarios = File.ReadAllText(_NomeArquivoListaUsuarios, Encoding.Default);
                    _ConteudoArquivoUsuarios = _ConteudoArquivoUsuarios.Replace("\r", "");
                }
	        }
            
            if (_ConteudoArquivoUsuarios.Length > 0)
            {
                string[] _Split;
                string _Linha = "";
                _Split = _ConteudoArquivoUsuarios.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < _Split.Length; i++)
                {
                    if (_Split[i].ToUpper().IndexOf(p_CON.ToUpper()) > -1)
                    {
                        _Linha = _Split[i];
                        break;
                    }
                }
                if (_Linha.Length > 0)
                {
                    _Split = _Linha.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (_Split.Length > 1)
                    {
                        _Retorno = _Retorno + " - " + _Split[1];
                    }
                    if (_Split.Length > 3)
                    {
                        _Retorno = _Retorno + " - " + _Split[3];
                    }
                }
            }

            return _Retorno;
        }

        private ArrayList ListaUsuariosBancoUtilizadosAnalista(string p_IdAnalista)
        {
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            _Query = "";
            _Query = _Query + "SELECT DISTINCT ";
            _Query = _Query + "    CURRENT_USER ";
            _Query = _Query + "FROM AESPROD.AUDITORIA_OBJETOS AU ";
            _Query = _Query + "WHERE AU.ID_USUARIO = " + p_IdAnalista + " ";
            _Query = _Query + "ORDER BY CURRENT_USER ";

            _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
            _Connection.Open();
            _Command.Connection = _Connection;
            _Command.CommandType = CommandType.Text;

            _Command.CommandText = _Query;
            _DataReader = _Command.ExecuteReader();

            while (_DataReader.Read())
            {
                if (!_DataReader.IsDBNull(0))
                {
                    _Retorno.Add(_DataReader.GetString(0));
                }
            }

            _DataReader.Close();
            _DataReader.Dispose();
            _Connection.Close();
            _Connection.Dispose();
            _Command.Dispose();
            GC.Collect();

            return _Retorno;
        }

        private ArrayList ListaObjetosAlteradosAnalista(string p_IdAnalista)
        {
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            _Query = "";
            _Query = _Query + "SELECT DISTINCT ";
            _Query = _Query + "    AU.OWNER || '.' ||  AU.NOME ";
            _Query = _Query + "FROM AESPROD.AUDITORIA_OBJETOS AU ";
            _Query = _Query + "WHERE AU.ID_USUARIO = " + p_IdAnalista + " ";
            _Query = _Query + "ORDER BY AU.OWNER || '.' || AU.NOME ";

            _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
            _Connection.Open();
            _Command.Connection = _Connection;
            _Command.CommandType = CommandType.Text;

            _Command.CommandText = _Query;
            _DataReader = _Command.ExecuteReader();

            while (_DataReader.Read())
            {
                _Retorno.Add(_DataReader.GetString(0));
            }

            _DataReader.Close();
            _DataReader.Dispose();
            _Connection.Close();
            _Connection.Dispose();
            _Command.Dispose();
            GC.Collect();

            return _Retorno;
        }

        private ArrayList ListaAnalistasDaEmpresa(string p_NomeEmpresa)
        {
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            _Query = "";
            _Query = _Query + "Select ID || ':' || NOME_COMPLETO ";
            _Query = _Query + "FROM AESPROD.CONTATO_CONTROLE_CONCORRENCIA ";
            _Query = _Query + "WHERE UPPER(EMPRESA) = UPPER('" + p_NomeEmpresa + "') ";
            _Query = _Query + "ORDER BY NOME_COMPLETO ";

            _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
            _Connection.Open();
            _Command.Connection = _Connection;
            _Command.CommandType = CommandType.Text;

            _Command.CommandText = _Query;
            _DataReader = _Command.ExecuteReader();

            while (_DataReader.Read())
            {
                _Retorno.Add(_DataReader.GetString(0));
            }

            _DataReader.Close();
            _DataReader.Dispose();
            _Connection.Close();
            _Connection.Dispose();
            _Command.Dispose();
            GC.Collect();

            return _Retorno;
        }

        private ArrayList ListaAnalistasUtilizandoDbUser(string p_NomeDbUser)
        {
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            _Query = "";
            _Query = _Query + "SELECT DISTINCT ";
            _Query = _Query + "    CO.ID || ':' || CO.NOME_COMPLETO ";
            _Query = _Query + "FROM AESPROD.AUDITORIA_OBJETOS AU ";
            _Query = _Query + "    INNER JOIN AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
            _Query = _Query + "    ON AU.ID_USUARIO = CO.ID ";
            _Query = _Query + "WHERE AU.CURRENT_USER = '" + p_NomeDbUser + "' ";

            _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
            _Connection.Open();
            _Command.Connection = _Connection;
            _Command.CommandType = CommandType.Text;

            _Command.CommandText = _Query;
            _DataReader = _Command.ExecuteReader();

            while (_DataReader.Read())
            {
                _Retorno.Add(_DataReader.GetString(0));
            }

            _DataReader.Close();
            _DataReader.Dispose();
            _Connection.Close();
            _Connection.Dispose();
            _Command.Dispose();
            GC.Collect();

            return _Retorno;
        }

        private void CopiarListaSelecionada()
        {
            if (lvwObjetos.SelectedItems.Count > 0)
            {
                string _ClipBoard = "";
                ListViewItem lvwItem = null;
                for (int i = 0; i < lvwObjetos.Columns.Count; i++)
                {
                    _ClipBoard = _ClipBoard + lvwObjetos.Columns[i].Text + "\t";
                }
                _ClipBoard = _ClipBoard + "\r\n";

                for (int i = 0; i < lvwObjetos.SelectedItems.Count; i++)
                {
                    lvwItem = lvwObjetos.Items[lvwObjetos.SelectedIndices[i]];
                    for (int j = 0; j < lvwItem.SubItems.Count; j++)
                    {
                        _ClipBoard = _ClipBoard + lvwItem.SubItems[j].Text + "\t";
                    }
                    _ClipBoard = _ClipBoard + "\r\n";
                }
                Clipboard.SetText(_ClipBoard);
            }
        }

        private void ColorirLista()
        {
            ArrayList _ListaObjetos = new ArrayList();
            csObjetoDeBanco _csObjetoDeBanco = new csObjetoDeBanco();
            bool _JaTinhaNaLista = false;

            foreach (ListViewItem _ListViewItem in lvwObjetos.Items)
            {
                _csObjetoDeBanco = new csObjetoDeBanco();
                _csObjetoDeBanco.Owner = _ListViewItem.SubItems[2].Text;
                _csObjetoDeBanco.Nome = _ListViewItem.SubItems[3].Text;
                _JaTinhaNaLista = false;

                foreach (csObjetoDeBanco item in _ListaObjetos)
                {
                    if (item.Owner == _csObjetoDeBanco.Owner && item.Nome == _csObjetoDeBanco.Nome)
                    {
                        _JaTinhaNaLista = true;
                        break;
                    }
                }
                if (!_JaTinhaNaLista)
                {
                    _ListaObjetos.Add(_csObjetoDeBanco);
                }
                
            }

            _ListaObjetos = _csOracle.ClassificaObjetos(_ListaObjetos);

            if (_ListaObjetos != null)
            {
                foreach (ListViewItem _ListViewItem in lvwObjetos.Items)
                {
                    foreach (csObjetoDeBanco item in _ListaObjetos)
                    {
                        if (item.Owner.Trim().ToUpper() == _ListViewItem.SubItems[2].Text.Trim().ToUpper() && item.Nome.Trim().ToUpper() == _ListViewItem.SubItems[3].Text.Trim().ToUpper())
                        {
                            switch (item.ClassificacaoObjeto)
                            {
                                case csObjetoDeBanco.enuClassificacaoObjeto.Indefinida:
                                case csObjetoDeBanco.enuClassificacaoObjeto.Alterado:
                                    // Branco
                                    _ListViewItem.BackColor = Color.White;
                                    for (int i = 0; i < _ListViewItem.SubItems.Count; i++)
                                    {
                                        _ListViewItem.SubItems[i].BackColor = Color.White;
                                    }
                                    break;
                                case csObjetoDeBanco.enuClassificacaoObjeto.Novo:
                                    // Verde
                                    _ListViewItem.BackColor = Color.LightGreen;
                                    for (int i = 0; i < _ListViewItem.SubItems.Count; i++)
                                    {
                                        _ListViewItem.SubItems[i].BackColor = Color.LightGreen;
                                    }
                                    break;
                                case csObjetoDeBanco.enuClassificacaoObjeto.Excluido:
                                    // Vermelho
                                    _ListViewItem.BackColor = Color.LightPink;
                                    for (int i = 0; i < _ListViewItem.SubItems.Count; i++)
                                    {
                                        _ListViewItem.SubItems[i].BackColor = Color.LightPink;
                                    }
                                    break;
                            }
                        }
                    } // Fim "foreach (csObjetoDeBanco item in _ListaObjetos)"
                } // Fim "foreach (ListViewItem _ListViewItem in lvwObjetos.Items)"
            } // Fim "if (_ListaObjetos != null)"

            

        }

    #endregion Métodos Privados

    #region Eventos de controles

        private void btColorir_Click(object sender, EventArgs e)
        {
            this.ColorirLista();
        }

        private void tmrDigitacao_Tick(object sender, EventArgs e)
        {
            tmrDigitacao.Enabled = false;
            if (txtPesquisa.Text.Trim().Length >= 3)
            {
                this.BotaoPesquisar(btPesquisar.Tag.ToString());
            }
        }

        private void lvwObjetos_MouseClick(object sender, MouseEventArgs e)
        {

            Point mousePos = lvwObjetos.PointToClient(Control.MousePosition);
            ListViewHitTestInfo hitTest = lvwObjetos.HitTest(mousePos);
            int columnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
            string[] _Split;
            ArrayList _Listagem = null;

            // columnIndex = 0 => Data e hora
            // columnIndex = 1 => Tipo do Objeto
            // columnIndex = 2 => Owner do Objeto
            // columnIndex = 3 => Nome do Objeto
            // columnIndex = 4 => Analista
            // columnIndex = 5 => Empresa do Analista
            // columnIndex = 6 => OS User do Analista
            // columnIndex = 7 => DB User usado pelo analista
            // columnIndex = 8 => Nome da máquina do analista

            if (e.Button == MouseButtons.Left) // Botão da esquerda
            {
                string _Query = "";

                _Query = "";
                _Query = _Query + "Select AU.DATA_HORA, "; // colDATA_HORA = 0;
                _Query = _Query + "    AU.TIPO AS TIPO_OBJ, "; // colTIPO_OBJ = 1;
                _Query = _Query + "    AU.OWNER AS OWNER_OBJ, "; // colOWNER_OBJ = 2;
                _Query = _Query + "    AU.NOME AS NOME_OBJ, "; // colNOME_OBJ = 3;
                _Query = _Query + "    CO.ID AS ID_ANALISTA, "; // colID_ANALISTA = 4;
                _Query = _Query + "    CO.NOME_COMPLETO AS NOME_ANALISTA "; // colNOME_ANALISTA = 5;
                _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
                _Query = _Query + "    Left Join AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
                _Query = _Query + "    On AU.ID_USUARIO = CO.ID ";

                switch (columnIndex)
                {
                    case 0: // Data e hora
                        if (_ItemClicado == "DATA_E_HORA_" + lvwObjetos.SelectedItems[0].Text)
                        {
                            return;
                        }
                        lblPesquisaSecundaria.Text = "Objetos alterados em torno de " + lvwObjetos.SelectedItems[0].Text;
                        _Query = _Query + "WHERE AU.DATA_HORA >= (TO_DATE('" + lvwObjetos.SelectedItems[0].Text + "', 'DD/MM/YY HH24:MI:SS') - ((1/24/60) * 30)) ";
                        _Query = _Query + "    And AU.DATA_HORA <= (TO_DATE('" + lvwObjetos.SelectedItems[0].Text + "', 'DD/MM/YY HH24:MI:SS') + ((1/24/60) * 30)) ";
                        _Query = _Query + "ORDER BY AU.DATA_HORA DESC ";
                        _ItemClicado = "DATA_E_HORA_" + lvwObjetos.SelectedItems[0].Text;
                        this.PreencheRelatorio("");
                        break;

                    case 1: // Tipo do Objeto
                        if (_ItemClicado == "TIPO_DO_OBJETO_" + lvwObjetos.SelectedItems[0].SubItems[1].Text)
                        {
                            return;
                        }
                        lblPesquisaSecundaria.Text = "Objetos alterados que são do tipo " + lvwObjetos.SelectedItems[0].SubItems[1].Text;
                        _Query = _Query + "WHERE AU.TIPO = '" + lvwObjetos.SelectedItems[0].SubItems[1].Text + "' ";

                        _Query = _Query + "ORDER BY AU.OWNER, AU.NOME, AU.DATA_HORA DESC ";

                        lblStatusRelatorio.Text = "Buscando informações do tipo " + lvwObjetos.SelectedItems[0].SubItems[1].Text;
                        Application.DoEvents();
                        _ItemClicado = "TIPO_DO_OBJETO_" + lvwObjetos.SelectedItems[0].SubItems[1].Text;
                        this.PreencheRelatorio("");
                        break;

                    case 2: // Owner do Objeto
                        if (_ItemClicado == "OWNER_DO_OBJETO_" + lvwObjetos.SelectedItems[0].SubItems[2].Text)
                        {
                            return;
                        }
                        lblPesquisaSecundaria.Text = "Objetos alterados que são do Owner " + lvwObjetos.SelectedItems[0].SubItems[2].Text;

                        _Query = "";
                        _Query = _Query + "Select Max(AU.DATA_HORA) AS DATA_HORA, ";
                        _Query = _Query + "    AU.TIPO AS TIPO_OBJ, ";
                        _Query = _Query + "    AU.OWNER AS OWNER_OBJ, ";
                        _Query = _Query + "    AU.NOME AS NOME_OBJ, ";
                        _Query = _Query + "    CO.ID AS ID_ANALISTA, ";
                        _Query = _Query + "    CO.NOME_COMPLETO AS NOME_ANALISTA ";
                        _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
                        _Query = _Query + "    Left Join AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
                        _Query = _Query + "    On AU.ID_USUARIO = CO.ID ";
                        _Query = _Query + "WHERE AU.OWNER = '" + lvwObjetos.SelectedItems[0].SubItems[2].Text + "' ";
                        _Query = _Query + "GROUP BY AU.TIPO, AU.OWNER, AU.NOME, CO.ID, CO.NOME_COMPLETO ";
                        _Query = _Query + "ORDER BY MAX(AU.DATA_HORA) DESC ";

                        //Owner: OWNER
                        lblStatusRelatorio.Text = "Buscando informações do owner " + lvwObjetos.SelectedItems[0].SubItems[2].Text;
                        Application.DoEvents();
                        _ItemClicado = "OWNER_DO_OBJETO_" + lvwObjetos.SelectedItems[0].SubItems[2].Text;
                        this.PreencheRelatorio("Owner: " + lvwObjetos.SelectedItems[0].SubItems[2].Text);

                        break;

                    case 3: // Nome do Objeto
                        if (_ItemClicado == "NOME_DO_OBJETO_" + lvwObjetos.SelectedItems[0].SubItems[2].Text + "." + lvwObjetos.SelectedItems[0].SubItems[3].Text)
                        {
                            return;
                        }
                        lblPesquisaSecundaria.Text = "Alterações no objeto " + lvwObjetos.SelectedItems[0].SubItems[3].Text;

                        _Query = "";
                        _Query = _Query + "Select MAX(AU.DATA_HORA) AS DATA_HORA, ";
                        _Query = _Query + "    AU.TIPO AS TIPO_OBJ, ";
                        _Query = _Query + "    AU.OWNER AS OWNER_OBJ, ";
                        _Query = _Query + "    AU.NOME AS NOME_OBJ, ";
                        _Query = _Query + "    CO.ID AS ID_ANALISTA, ";
                        _Query = _Query + "    CO.NOME_COMPLETO AS NOME_ANALISTA ";
                        _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
                        _Query = _Query + "    Left Join AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
                        _Query = _Query + "    On AU.ID_USUARIO = CO.ID ";
                        _Query = _Query + "WHERE AU.OWNER = '" + lvwObjetos.SelectedItems[0].SubItems[2].Text + "' ";
                        _Query = _Query + "    And AU.NOME = '" + lvwObjetos.SelectedItems[0].SubItems[3].Text + "' ";
                        _Query = _Query + "GROUP BY AU.TIPO, AU.OWNER, AU.NOME, CO.ID, CO.NOME_COMPLETO ";
                        _Query = _Query + "ORDER BY MAX(AU.DATA_HORA) DESC ";

                        lblStatusRelatorio.Text = "Buscando informações do objeto " + lvwObjetos.SelectedItems[0].SubItems[2].Text + "." + lvwObjetos.SelectedItems[0].SubItems[3].Text;
                        Application.DoEvents();
                        _ItemClicado = "NOME_DO_OBJETO_" + lvwObjetos.SelectedItems[0].SubItems[2].Text + "." + lvwObjetos.SelectedItems[0].SubItems[3].Text;
                        this.PreencheRelatorio("Objeto: " + lvwObjetos.SelectedItems[0].SubItems[2].Text + "." + lvwObjetos.SelectedItems[0].SubItems[3].Text);

                        break;

                    case 4: // Analista
                        if (_ItemClicado == "ANALISTA_" + lvwObjetos.SelectedItems[0].SubItems[4].Tag.ToString())
                        {
                            return;
                        }
                        lblPesquisaSecundaria.Text = "Alterações do usuário " + lvwObjetos.SelectedItems[0].SubItems[4].Text;
                        _Query = "";
                        _Query = _Query + "Select MAX(AU.DATA_HORA) AS DATA_HORA, ";
                        _Query = _Query + "    AU.TIPO AS TIPO_OBJ, ";
                        _Query = _Query + "    AU.OWNER AS OWNER_OBJ, ";
                        _Query = _Query + "    AU.NOME AS NOME_OBJ, ";
                        _Query = _Query + "    CO.ID AS ID_ANALISTA, ";
                        _Query = _Query + "    CO.NOME_COMPLETO AS NOME_ANALISTA ";
                        _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
                        _Query = _Query + "    Left Join AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
                        _Query = _Query + "    On AU.ID_USUARIO = CO.ID ";
                        _Query = _Query + "WHERE CO.ID = '" + lvwObjetos.SelectedItems[0].SubItems[4].Tag.ToString() + "' ";
                        _Query = _Query + "GROUP BY AU.TIPO, AU.OWNER, AU.NOME, CO.ID, CO.NOME_COMPLETO ";
                        _Query = _Query + "ORDER BY MAX(AU.DATA_HORA) DESC ";

                        lblStatusRelatorio.Text = "Buscando informações do analista " + lvwObjetos.SelectedItems[0].SubItems[4].Text;
                        Application.DoEvents();
                        _ItemClicado = "ANALISTA_" + lvwObjetos.SelectedItems[0].SubItems[4].Tag.ToString();
                        this.PreencheRelatorio("Analista: " + lvwObjetos.SelectedItems[0].SubItems[4].Tag.ToString());
                        break;

                    case 5: // Empresa
                        if (_ItemClicado == "EMPRESA_" + lvwObjetos.SelectedItems[0].SubItems[5].Text.ToUpper())
                        {
                            return;
                        }
                        lblPesquisaSecundaria.Text = "Alterações da empresa " + lvwObjetos.SelectedItems[0].SubItems[5].Text;
                        _Query = "";
                        _Query = _Query + "Select MAX(AU.DATA_HORA) AS DATA_HORA, ";
                        _Query = _Query + "    AU.TIPO AS TIPO_OBJ, ";
                        _Query = _Query + "    AU.OWNER AS OWNER_OBJ, ";
                        _Query = _Query + "    AU.NOME AS NOME_OBJ, ";
                        _Query = _Query + "    CO.ID AS ID_ANALISTA, ";
                        _Query = _Query + "    CO.NOME_COMPLETO AS NOME_ANALISTA ";
                        _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
                        _Query = _Query + "    Left Join AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
                        _Query = _Query + "    On AU.ID_USUARIO = CO.ID ";
                        _Query = _Query + "WHERE UPPER(CO.EMPRESA) = '" + lvwObjetos.SelectedItems[0].SubItems[5].Text.ToUpper() + "' ";
                        _Query = _Query + "GROUP BY AU.TIPO, AU.OWNER, AU.NOME, CO.ID, CO.NOME_COMPLETO ";
                        _Query = _Query + "ORDER BY MAX(AU.DATA_HORA) DESC ";

                        lblStatusRelatorio.Text = "Buscando informações do analista " + lvwObjetos.SelectedItems[0].SubItems[4].Text;
                        Application.DoEvents();
                        _ItemClicado = "EMPRESA_" + lvwObjetos.SelectedItems[0].SubItems[5].Text.ToUpper();
                        this.PreencheRelatorio("Analista: " + lvwObjetos.SelectedItems[0].SubItems[4].Tag.ToString());
                        break;

                    default:
                        lblPesquisaSecundaria.Text = "";
                        _Query = "";
                        break;
                }

                this.PesquisaSecundaria(_Query);
            }
            else // Botão da direita
            {
                ToolStripItem _Menu = null;
                ToolStripItem _SubMenu = null;
                string _OwnerNomeObj = "";
                string _Mensagem = "";
                csBackupObjetoDeBanco _csBackupObjetoDeBanco = null;
                mousePos = new Point(Cursor.Position.X, Cursor.Position.Y);

                switch (columnIndex)
                {
                    case 0: // Data e hora
                        // Data Hora = lvwObjetos.SelectedItems[0].Text
                        // Nada para fazer
                        break;

                    case 1: // Tipo do Objeto
                        // Tipo do Objeto = lvwObjetos.SelectedItems[0].SubItems[1].Text
                        // Nada para fazer
                        break;

                    case 2: // Owner do Objeto
                        // Owner do Objeto = lvwObjetos.SelectedItems[0].SubItems[2].Text
                        // Nada para fazer
                        break;

                    case 3: // Nome do Objeto
                        // Nome do Objeto = lvwObjetos.SelectedItems[0].SubItems[2].Text + "." + lvwObjetos.SelectedItems[0].SubItems[3].Text
                        // Opção de extrair e mostrar o fonte do objeto selecionado
                        //_csOracle.ExtractDDLTextEditor(_Username, _Password, _Database, lvwObjetos.SelectedItems[0].SubItems[2].Text + "." + lvwObjetos.SelectedItems[0].SubItems[3].Text);

                        _OwnerNomeObj = lvwObjetos.SelectedItems[0].SubItems[2].Text + "." + lvwObjetos.SelectedItems[0].SubItems[3].Text;

                        mnuContextoLvw.Items.Clear();

                        // Extrair DDL do objeto
                        _Menu = mnuContextoLvw.Items.Add("Extrair fonte do objeto " + _OwnerNomeObj);
                        _Menu.Tag = "ExtractDDL:" + _OwnerNomeObj;

                        mnuContextoLvw.Show(mousePos);
                        _Menu = mnuContextoLvw.Items.Add("-");
                        _Menu = mnuContextoLvw.Items.Add("Procurando backups do objeto...");
                        Application.DoEvents();

                        decimal _QuantosBackups = _csOracle.QuantosBackups(_OwnerNomeObj, ref _Mensagem);
                        if (_QuantosBackups > 0)
                        {
                            _Menu.Text = "Mostrar todos " + _QuantosBackups + " backups de " + _OwnerNomeObj;
                            _Menu.Tag = "VerTodosBackups:" + _OwnerNomeObj;
                        }
                        else
                        {
                            _Menu.Text = "Nenhum Backup encontrado para " + _OwnerNomeObj;
                        }
                        Application.DoEvents();

                        //// Se tem versões salvas, mostrar as 5 últimas
                        //ArrayList _ListaBackupsObjeto = _csOracle.ListaBackupsObjeto(_OwnerNomeObj, ref _Mensagem);
                        //if (_ListaBackupsObjeto.Count == 0)
                        //{
                        //    _Menu.Text = "Nenhum Backup encontrado para " + _OwnerNomeObj;
                        //}
                        //else
                        //{
                        //    mnuContextoLvw.Items.Remove(_Menu);
                        //}
                        //for (int i = 0; i < _ListaBackupsObjeto.Count; i++)
                        //{
                        //    _csBackupObjetoDeBanco = (csBackupObjetoDeBanco)_ListaBackupsObjeto[i];
                        //    _Menu = mnuContextoLvw.Items.Add("Extrair backup de " + _csBackupObjetoDeBanco.DataBackup.ToString("dd/MM/yy HH:mm:ss") + " gerado pela alteração de " + _csBackupObjetoDeBanco.NomeUsuarioQueGerouBackup);
                        //    _Menu.Tag = "ExtractBackup:" + _csBackupObjetoDeBanco.Id_Alteracao;
                        //    if (i >= 5)
                        //    {
                        //        break;
                        //    }
                        //}

                        //// Verificar se tem o parametro com o caminho do Tortoise

                        //if (this.CaminhoExeTortoiseSVN != null)
                        //{
                        //    if (_ListaBackupsObjeto.Count > 0)
                        //    {
                        //        _Menu = mnuContextoLvw.Items.Add("-");
                        //    }
                        //    for (int i = 0; i < _ListaBackupsObjeto.Count; i++)
                        //    {
                        //        _csBackupObjetoDeBanco = (csBackupObjetoDeBanco)_ListaBackupsObjeto[i];
                        //        _Menu = mnuContextoLvw.Items.Add("Comparar versão atual com backup de " + _csBackupObjetoDeBanco.DataBackup.ToString("dd/MM/yy HH:mm:ss"));
                        //        _Menu.Tag = "CompararBackup:" + _csBackupObjetoDeBanco.Id_Alteracao;
                        //        if (i >= 5)
                        //        {
                        //            break;
                        //        }
                        //    }
                        //}

                        //if (_ListaBackupsObjeto.Count > 0)
                        //{
                        //    _Menu = mnuContextoLvw.Items.Add("-");
                        //    _Menu = mnuContextoLvw.Items.Add("Mostrar todos backups de " + _OwnerNomeObj);
                        //    _Menu.Tag = "VerTodosBackups:" + _OwnerNomeObj;
                        //}
                        

                        //MessageBox.Show("1 - Opção de extrair e mostrar o fonte do objeto selecionado\nNome do Objeto = " + lvwObjetos.SelectedItems[0].SubItems[2].Text + "." + lvwObjetos.SelectedItems[0].SubItems[3].Text);
                        break;

                    case 4: // Analista
                        // Nome Analista = lvwObjetos.SelectedItems[0].SubItems[4].Text
                        // ID Analista = lvwObjetos.SelectedItems[0].SubItems[4].Tag.ToString()
                        // 1 - Listar os objetos que estão sendo alterados pelo analista e permitir extrair o fonte de cada um
                        // 2 - Listar os usuários de banco já utilizados pelo analista

                        mnuContextoLvw.Items.Clear();

                        _Menu = mnuContextoLvw.Items.Add("Objetos alterados por " + lvwObjetos.SelectedItems[0].SubItems[4].Text);
                        _Listagem = this.ListaObjetosAlteradosAnalista(lvwObjetos.SelectedItems[0].SubItems[4].Tag.ToString());
                        for (int i = 0; i < _Listagem.Count; i++)
                        {
                            _SubMenu = (_Menu as ToolStripMenuItem).DropDownItems.Add("Extrair fonte do objeto " + _Listagem[i].ToString());
                            _SubMenu.Click += new System.EventHandler(this.mnuContextoLvw_ItemClicked);
                            _SubMenu.Tag = "ExtractDDL:" + _Listagem[i].ToString();
                        }

                        _Menu = mnuContextoLvw.Items.Add("Usuarios de banco utilizados por " + lvwObjetos.SelectedItems[0].SubItems[4].Text);
                        ArrayList ListaUsuariosBanco = this.ListaUsuariosBancoUtilizadosAnalista(lvwObjetos.SelectedItems[0].SubItems[4].Tag.ToString());
                        for (int i = 0; i < ListaUsuariosBanco.Count; i++)
                        {
                            _SubMenu = (_Menu as ToolStripMenuItem).DropDownItems.Add(ListaUsuariosBanco[i].ToString());
                            _SubMenu.Click += new System.EventHandler(this.mnuContextoLvw_ItemClicked);
                            _SubMenu.Tag = null;
                        }

                        mnuContextoLvw.Show(mousePos);

                        //MessageBox.Show("1 - Listar os objetos que estão sendo alterados pelo analista e permitir extrair o fonte de cada um\n2 - Listar os usuários de banco já utilizados pelo analista\nNome Analista = " + lvwObjetos.SelectedItems[0].SubItems[4].Text + "\nID Analista = " + lvwObjetos.SelectedItems[0].SubItems[4].Tag.ToString());
                        break;

                    case 5: // Empresa do Analista
                        // Empresa do Analista = lvwObjetos.SelectedItems[0].SubItems[5].Text

                        mnuContextoLvw.Items.Clear();

                        _Menu = mnuContextoLvw.Items.Add("Analistas da empresa " + lvwObjetos.SelectedItems[0].SubItems[5].Text);
                        _Listagem = this.ListaAnalistasDaEmpresa(lvwObjetos.SelectedItems[0].SubItems[5].Text);
                        for (int i = 0; i < _Listagem.Count; i++)
                        {
                            _Split = _Listagem[i].ToString().Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (_Split.Length == 2)
                            {
                                _SubMenu = (_Menu as ToolStripMenuItem).DropDownItems.Add(_Split[1]);
                                _SubMenu.Click += new System.EventHandler(this.mnuContextoLvw_ItemClicked);
                                _SubMenu.Tag = "Analista:" + _Split[0] + ":" + _Split[1];
                            }
                        }
                        mnuContextoLvw.Show(mousePos);

                        //MessageBox.Show("1 - Listar os analistas da empresa e permitir selecionar um deles para gerar o relatório de analista\nEmpresa do Analista = " + lvwObjetos.SelectedItems[0].SubItems[5].Text);
                        break;

                    case 6: // OS User do Analista
                        // OS User do Analista = lvwObjetos.SelectedItems[0].SubItems[6].Text
                        // Nada para fazer
                        break;

                    case 7: // DB User usado pelo analista
                        // DB User usado pelo analista = lvwObjetos.SelectedItems[0].SubItems[7].Text

                        mnuContextoLvw.Items.Clear();

                        _Menu = mnuContextoLvw.Items.Add("Analistas utilizando o DB User " + lvwObjetos.SelectedItems[0].SubItems[7].Text);
                        _Listagem = this.ListaAnalistasUtilizandoDbUser(lvwObjetos.SelectedItems[0].SubItems[7].Text);
                        for (int i = 0; i < _Listagem.Count; i++)
                        {
                            _Split = _Listagem[i].ToString().Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (_Split.Length == 2)
                            {
                                _SubMenu = (_Menu as ToolStripMenuItem).DropDownItems.Add(_Split[1]);
                                _SubMenu.Click += new System.EventHandler(this.mnuContextoLvw_ItemClicked);
                                _SubMenu.Tag = "Analista:" + _Split[0] + ":" + _Split[1];
                            }
                        }
                        _Menu = mnuContextoLvw.Items.Add("Gerar Relatório");
                        _Menu.Tag = "RelatórioDbUser:" + lvwObjetos.SelectedItems[0].SubItems[7].Text;
                        mnuContextoLvw.Show(mousePos);

                        //MessageBox.Show("1 - Quais analistas estão utilizando este usuário, permitir gerar relatório com e-mail\nDB User usado pelo analista = " + lvwObjetos.SelectedItems[0].SubItems[7].Text);
                        break;

                    case 8: // Nome da máquina do analista
                        // Nome da máquina do analista = lvwObjetos.SelectedItems[0].SubItems[8].Text
                        // Nada para fazer
                        break;

                    default:
                        // Nada para fazer
                        break;
                }
            }
        }
        
        private void lvwPesquisaSecundaria_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorterSecundaria.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorterSecundaria.Order == SortOrder.Ascending)
                {
                    lvwColumnSorterSecundaria.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorterSecundaria.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorterSecundaria.SortColumn = e.Column;
                lvwColumnSorterSecundaria.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvwPesquisaSecundaria.Sort();
        }

        private void lvwObjetos_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorterPrincipal.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorterPrincipal.Order == SortOrder.Ascending)
                {
                    lvwColumnSorterPrincipal.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorterPrincipal.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorterPrincipal.SortColumn = e.Column;
                lvwColumnSorterPrincipal.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvwObjetos.Sort();
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            tmrDigitacao.Enabled = false;
            tmrDigitacao.Enabled = true;
        }
        
        private void lvwPesquisaSecundaria_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lvwPesquisaSecundaria.Items.Count > 0)
                {
                    this.MostraRelatorioHtml();
                }
            }
        }

        private void lvwRelatorio_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lvwPesquisaSecundaria.Items.Count > 0)
                {
                    this.MostraRelatorioHtml();
                }
            }
            else
            {
                _PararPesquisa = true;
            }
        }

        private void mnuContextoLvw_ItemClicked(object sender, EventArgs e)
        {
            try
            {
                ToolStripItem _ToolStripItem = (ToolStripItem)sender;
                if (_ToolStripItem.Tag == null)
                {
                    return;
                }
                _mnuContextoLvwTag = _ToolStripItem.Tag.ToString();
            }
            catch (Exception)
            {
                ToolStripItemClickedEventArgs _ToolStripItemClickedEventArgs = (ToolStripItemClickedEventArgs)e;
                if (_ToolStripItemClickedEventArgs.ClickedItem.Tag == null)
                {
                    return;
                }
                _mnuContextoLvwTag = _ToolStripItemClickedEventArgs.ClickedItem.Tag.ToString();
            }

            tmrMnuContextoLvw.Enabled = true;
        }

        private void btMenuContextoBotoes_Click(object sender, EventArgs e)
        {
            Point _Point = new Point(Cursor.Position.X + 12, Cursor.Position.Y);
            mnuContextoBotoes.Show(_Point);
        }

        private void mnuContextoBotoes_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _mnuContextoBotoesTag = e.ClickedItem.Tag.ToString();
            tmrMnuContextoBotoes.Enabled = true;
        }

        private void btPesquisar_Click(object sender, EventArgs e)
        {
            this.BotaoPesquisar(btPesquisar.Tag.ToString());
        }

        private void btPesquisar_MouseEnter(object sender, EventArgs e)
        {
            string _TextoToolTip = "";

            _ToolTipText.IsBalloon = false;
            _ToolTipText.UseAnimation = true;
            _ToolTipText.UseFading = true;
            _ToolTipText.Show(string.Empty, this, 0, 0);

            switch (btPesquisar.Tag.ToString())
            {
                case "PESQUISAR_TUDO":
                    _TextoToolTip = "Pesquisar Tudo";
                    break;

                case "PESQUISAR_ULTIMA_SEMANA":
                    _TextoToolTip = "Pesquisar última semana";
                    break;

                case "PESQUISAR_ULTIMAS_24_HORAS":
                    _TextoToolTip = "Pesquisar últimas 24 horas";
                    break;

                case "PESQUISAR_ULTIMAS_72_HORAS":
                    _TextoToolTip = "Pesquisar últimas 72 horas";
                    break;

                case "LISTAR_CONFLITOS":
                    _TextoToolTip = "Listar conflitos";
                    break;
            }

            if (_TextoToolTip.Trim().Length > 0)
            {
                _ToolTipText.Show(_TextoToolTip, btPesquisar, 0, -20, 2000);
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

        private void lvwObjetos_KeyDown(object sender, KeyEventArgs e)
        {
            // Se pressionou CTLR + C, copia as informações dos itens selecionados da ListView para a área de transferência
            if (e.KeyCode == Keys.C)
            {
                if (e.Control)
                {
                    this.CopiarListaSelecionada();
                }
            }
        }

        private void lvwRelatorio_DoubleClick(object sender, EventArgs e)
        {
            if (lvwRelatorio.SelectedItems.Count > 0)
            {
                if (lvwRelatorio.SelectedItems[0].Tag != null)
                {
                    if (lvwRelatorio.SelectedItems[0].Tag.ToString().Trim().Length > 0 && lvwRelatorio.SelectedItems[0].Tag.ToString().IndexOf(_URL_SVN) > -1)
                    {
                        if (this.CaminhoExeTortoiseSVN == null)
                        {
                            string _PastaProgramFilesX86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + "\\";
                            string _PastaProgramFiles = Environment.ExpandEnvironmentVariables("%ProgramW6432%") + "\\";
                            if (File.Exists(_PastaProgramFilesX86 + @"TortoiseSVN\bin\TortoiseProc.exe"))
                            {
                                this.CaminhoExeTortoiseSVN = _PastaProgramFilesX86 + @"TortoiseSVN\bin\TortoiseProc.exe";
                            }
                            else if (File.Exists(_PastaProgramFiles + @"TortoiseSVN\bin\TortoiseProc.exe"))
                            {
                                this.CaminhoExeTortoiseSVN = _PastaProgramFiles + @"TortoiseSVN\bin\TortoiseProc.exe";
                            }
                            else
                            {
                                string _Filtro = "Executável do Tortoise SVN (TortoiseProc.exe)|TortoiseProc.exe";
                                DialogResult Resp;
                                OpenFileDialog dlgAbrir = new OpenFileDialog();
                                dlgAbrir.Title = "Informe o caminho do executável do Tortoise SVN";
                                dlgAbrir.Filter = _Filtro;
                                dlgAbrir.Multiselect = false;

                                Resp = dlgAbrir.ShowDialog();
                                if (Resp == System.Windows.Forms.DialogResult.OK)
                                {
                                    if (File.Exists(dlgAbrir.FileNames[0]) && csUtil.ParteNomeArquivo(dlgAbrir.FileNames[0], csUtil.enuParteNomeArquivo.NomeExtencao).ToUpper() == "TortoiseProc.exe".ToUpper())
                                    {
                                        this.CaminhoExeTortoiseSVN = dlgAbrir.FileNames[0];
                                    }
                                }
                            }
                        }
                        if (this.CaminhoExeTortoiseSVN == null)
                        {
                            MessageBox.Show("O Tortoise SVN precisa estar instalado na máquina para esta funcionalidade!", "Ver Log do Objeto no Trunk", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            if (lvwRelatorio.SelectedItems[0].Tag.ToString().IndexOf("/branches") > -1)
                            {
                                Process.Start(this.CaminhoExeTortoiseSVN, " /command:repobrowser /path:" + lvwRelatorio.SelectedItems[0].Tag.ToString());
                            }
                            else
                            {
                                Process.Start(this.CaminhoExeTortoiseSVN, " /command:log /path:" + lvwRelatorio.SelectedItems[0].Tag.ToString());
                            }
                        }
                    }
                }
            }
        }

        private void tmrMnuContextoBotoes_Tick(object sender, EventArgs e)
        {
            tmrMnuContextoBotoes.Enabled = false;
            switch (_mnuContextoBotoesTag)
            {
                case "PESQUISAR_TUDO":
                    btPesquisar.Tag = _mnuContextoBotoesTag;
                    this.BotaoPesquisar(btPesquisar.Tag.ToString());
                    break;

                case "PESQUISAR_ULTIMA_SEMANA":
                    btPesquisar.Tag = _mnuContextoBotoesTag;
                    this.BotaoPesquisar(btPesquisar.Tag.ToString());
                    break;

                case "PESQUISAR_ULTIMAS_24_HORAS":
                    btPesquisar.Tag = _mnuContextoBotoesTag;
                    this.BotaoPesquisar(btPesquisar.Tag.ToString());
                    break;

                case "PESQUISAR_ULTIMAS_72_HORAS":
                    btPesquisar.Tag = _mnuContextoBotoesTag;
                    this.BotaoPesquisar(btPesquisar.Tag.ToString());
                    break;

                case "LISTAR_CONFLITOS":
                    btPesquisar.Tag = _mnuContextoBotoesTag;
                    this.BotaoPesquisar(btPesquisar.Tag.ToString());
                    break;

                case "CHAMAR_PREENCHE_USUARIOS":
                    this.BotaoPesquisar("CHAMAR_PREENCHE_USUARIOS");
                    break;

                case "CHAMAR_PROCURA_CONFLITOS":
                    this.BotaoPesquisar("CHAMAR_PROCURA_CONFLITOS");
                    break;
            }
        }

        private void tmrMnuContextoLvw_Tick(object sender, EventArgs e)
        {
            tmrMnuContextoLvw.Enabled = false;

            string[] _Split;
            string _Query = "";
            string _Mensagem = "";
            string _OwnerNomeObjeto = "";
            string _Fonte = "";
            string _CaminhoCompletoArquivoBakup = "";
            string _CaminhoCompletoArquivoAtual = "";
            string _TipoObjeto = "";
            string _Linha_de_Comando = "";

            _Split = _mnuContextoLvwTag.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            switch (_Split[0])
            {
                case "ExtractDDL":
                    _csOracle.ExtractDDLTextEditor(_Username, _Password, _Database, _Split[1]);
                    break;

                case "ExtractBackup":
                    Cursor.Current = Cursors.WaitCursor;
                    _Fonte = _csOracle.ExtractBackupObjeto(decimal.Parse(_Split[1]), ref _OwnerNomeObjeto, ref _Mensagem);
                    if (_Fonte.Trim().Length == 0)
                    {
                        if (_Mensagem.Trim().Length > 0)
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Problemas para extrair backup\n" + _Mensagem, "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Problemas para extrair backup", "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        csUtil.SalvarEAbrir(_Fonte, "Backup_" + _OwnerNomeObjeto + ".sql");
                    }
                    Cursor.Current = Cursors.Default;
                    break;

                case "CompararBackup":
                    Cursor.Current = Cursors.WaitCursor;
                    _Fonte = _csOracle.ExtractBackupObjeto(decimal.Parse(_Split[1]), ref _OwnerNomeObjeto, ref _Mensagem);
                    if (_Fonte.Trim().Length == 0)
                    {
                        if (_Mensagem.Trim().Length > 0)
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Problemas para extrair backup\n" + _Mensagem, "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Problemas para extrair backup", "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        _CaminhoCompletoArquivoBakup = Path.GetTempPath() + "Backup_" + _OwnerNomeObjeto + ".sql";
                        File.WriteAllText(_CaminhoCompletoArquivoBakup, _Fonte, Encoding.Default);
                        _Fonte = _csOracle.ExtractDDL(_Username, _Password, _Database, _OwnerNomeObjeto, ref _TipoObjeto, ref _Mensagem);
                        _CaminhoCompletoArquivoAtual = Path.GetTempPath() + _OwnerNomeObjeto + ".sql";
                        File.WriteAllText(_CaminhoCompletoArquivoAtual, _Fonte, Encoding.Default);

                        _Linha_de_Comando = "/command:diff /path:\"" + _CaminhoCompletoArquivoAtual + "\" /path2:\"" + _CaminhoCompletoArquivoBakup + "\"";
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                        process.StartInfo.FileName = this.CaminhoExeTortoiseSVN;
                        process.StartInfo.Arguments = _Linha_de_Comando;
                        process.Start();
                    }
                    Cursor.Current = Cursors.Default;
                    break;

                case "VerTodosBackups":
                    frmBackupsControleConcorrencia _frmBackupsControleConcorrencia = new frmBackupsControleConcorrencia(_Username, _Password, _Database, _Split[1]);
                    break;

                case "Analista":
                    lblPesquisaSecundaria.Text = "Alterações do analista " + _Split[2];
                    _Query = "";
                    _Query = _Query + "Select MAX(AU.DATA_HORA) AS DATA_HORA, ";
                    _Query = _Query + "    AU.TIPO AS TIPO_OBJ, ";
                    _Query = _Query + "    AU.OWNER AS OWNER_OBJ, ";
                    _Query = _Query + "    AU.NOME AS NOME_OBJ, ";
                    _Query = _Query + "    CO.ID AS ID_ANALISTA, ";
                    _Query = _Query + "    CO.NOME_COMPLETO AS NOME_ANALISTA ";
                    _Query = _Query + "From AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "    Left Join AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
                    _Query = _Query + "    On AU.ID_USUARIO = CO.ID ";
                    _Query = _Query + "WHERE CO.ID = '" + _Split[1] + "' ";
                    _Query = _Query + "GROUP BY AU.TIPO, AU.OWNER, AU.NOME, CO.ID, CO.NOME_COMPLETO ";
                    _Query = _Query + "ORDER BY MAX(AU.DATA_HORA) DESC ";

                    lblStatusRelatorio.Text = "Buscando informações do analista " + lvwObjetos.SelectedItems[0].SubItems[4].Text;
                    Application.DoEvents();
                    this.PreencheRelatorio("Analista: " + _Split[1]);
                    this.PesquisaSecundaria(_Query);

                    break;

                case "RelatórioDbUser":

                    this.MostraRelatorioHtmlDbUser(_Split[1]);

                    break;

                default:
                    MessageBox.Show("Test");
                    break;
            }

        }

    #endregion Eventos de controles
                        
    }
}
