using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using System.Diagnostics;
using System.Data.OleDb;

namespace Oracle_Tools
{
    class csOracle
    {
    #region Variáveis privadas

        private int _TemTabContrConc = -1; // -1 = Indefinido, zero = Não tem, Diferente de zero = Tem
        string _UsuarioDB = "";
        string _SenhaDB = "";
        string _AliasDatabase = "";
        string _QuebraLinha = "\r\n";

    #endregion Variáveis privadas
        
    #region Propriedades

        /// <summary>
        /// Retorna se tem a tabela de controle de concorrência
        /// </summary>
        public bool TemTabContrConc
        {
            get
            {
                if (_TemTabContrConc == -1) // -1 = Indefinido
                {
                    if (_UsuarioDB.Trim().Length > 0 && _SenhaDB.Trim().Length > 0 && _AliasDatabase.Trim().Length > 0)
                    {
                        OracleCommand _Command = new OracleCommand();
                        OracleConnection _Connection = new OracleConnection();
                        OracleDataReader _DataReader = null;

                        _Connection.ConnectionString = "User Id=" + _UsuarioDB + ";Password=" + _SenhaDB + ";Data Source=" + _AliasDatabase;
                        _Connection.Open();
                        _Command.Connection = _Connection;

                        string _Consulta = "";
                        _Consulta = _Consulta + "SELECT COUNT(*) ";
                        _Consulta = _Consulta + "FROM ALL_OBJECTS ";
                        _Consulta = _Consulta + "WHERE OWNER = 'AESPROD' ";
                        _Consulta = _Consulta + "    AND OBJECT_NAME = 'AUDITORIA_OBJETOS' ";

                        _Command.CommandText = _Consulta;
                        _DataReader = _Command.ExecuteReader();
                        if (_DataReader.Read())
                        {
                            if (_DataReader.GetDecimal(0) > 0)
                            {
                                _TemTabContrConc = 1; // Diferente de zero = Tem
                            }
                            else
                            {
                                _TemTabContrConc = 0; // zero = Não tem
                            }
                        }
                        else
                        {
                            _TemTabContrConc = 0; // zero = Não tem
                        }
                        _DataReader.Close();
                        _DataReader.Dispose();
                        _Connection.Close();
                        _Connection.Dispose();
                        GC.Collect();
                    }
                }
                return (_TemTabContrConc != 0);
            }
        }

        /// <summary>
        /// Retorna ou salva o indicador se deve ou não colocar cabecalho nos arquivos ao extrair a DDL, que fica salvo no registro no caminho HKEY_CURRENT_USER\Software\ORACLE_Tools\Parametros\ColocarCabecalhoAoExtrairDDL
        /// </summary>
        public bool ColocarCabecalhoAoExtrairDDL
        {
            get
            {
                object _ObjColocarCabecalhoAoExtrairDDL = csUtil.CarregarPreferencia("ColocarCabecalhoAoExtrairDDL");
                string _ColocarCabecalhoAoExtrairDDL = null;
                bool _ColocarCabecalho = true;
                if (_ObjColocarCabecalhoAoExtrairDDL == null)
                {
                    csUtil.SalvarPreferencia("ColocarCabecalhoAoExtrairDDL", "1");
                    _ColocarCabecalho = true;
                }
                else
                {
                    _ColocarCabecalhoAoExtrairDDL = _ObjColocarCabecalhoAoExtrairDDL.ToString().Trim().ToUpper();
                    if (_ColocarCabecalhoAoExtrairDDL != "1" && _ColocarCabecalhoAoExtrairDDL != "0" && _ColocarCabecalhoAoExtrairDDL != "TRUE" && _ColocarCabecalhoAoExtrairDDL != "FALSE" && _ColocarCabecalhoAoExtrairDDL != "SIM" && _ColocarCabecalhoAoExtrairDDL != "NÃO")
                    {
                        csUtil.SalvarPreferencia("ColocarCabecalhoAoExtrairDDL", "1");
                        _ColocarCabecalho = true;
                    }
                    else if (_ColocarCabecalhoAoExtrairDDL == "1" || _ColocarCabecalhoAoExtrairDDL == "TRUE" || _ColocarCabecalhoAoExtrairDDL == "SIM")
                    {
                        _ColocarCabecalho = true;
                    }
                    else
                    {
                        _ColocarCabecalho = false;
                    }
                }
                return _ColocarCabecalho;
            }
            set
            {
                bool _ColocarCabecalho = value;
                string _ColocarCabecalhoAoExtrairDDL = null;
                if (_ColocarCabecalho)
                {
                    _ColocarCabecalhoAoExtrairDDL = "1";
                }
                else
                {
                    _ColocarCabecalhoAoExtrairDDL = "0";
                }
                csUtil.SalvarPreferencia("ColocarCabecalhoAoExtrairDDL", _ColocarCabecalhoAoExtrairDDL);
            }
        }

        /// <summary>
        /// Retorna ou salva o indicador se deve ou não colocar a mensagem de erro na linha com erro quando objeto está inválido, que fica salvo no registro no caminho HKEY_CURRENT_USER\Software\ORACLE_Tools\Parametros\ColocarMensagemErroNaLinhaAoExtrairDDL
        /// </summary>
        public bool ColocarMensagemErroNaLinhaAoExtrairDDL
        {
            get
            {
                object _ObjColocarMensagemErroNaLinhaAoExtrairDDL = csUtil.CarregarPreferencia("ColocarMensagemErroNaLinhaAoExtrairDDL");
                string _ColocarMensagemErroNaLinhaAoExtrairDDL = null;
                bool _ColocarMensagem = true;
                if (_ObjColocarMensagemErroNaLinhaAoExtrairDDL == null)
                {
                    csUtil.SalvarPreferencia("ColocarMensagemErroNaLinhaAoExtrairDDL", "1");
                    _ColocarMensagem = true;
                }
                else
                {
                    _ColocarMensagemErroNaLinhaAoExtrairDDL = _ObjColocarMensagemErroNaLinhaAoExtrairDDL.ToString().Trim().ToUpper();
                    if (_ColocarMensagemErroNaLinhaAoExtrairDDL != "1" && _ColocarMensagemErroNaLinhaAoExtrairDDL != "0" && _ColocarMensagemErroNaLinhaAoExtrairDDL != "TRUE" && _ColocarMensagemErroNaLinhaAoExtrairDDL != "FALSE" && _ColocarMensagemErroNaLinhaAoExtrairDDL != "SIM" && _ColocarMensagemErroNaLinhaAoExtrairDDL != "NÃO")
                    {
                        csUtil.SalvarPreferencia("ColocarMensagemErroNaLinhaAoExtrairDDL", "1");
                        _ColocarMensagem = true;
                    }
                    else if (_ColocarMensagemErroNaLinhaAoExtrairDDL == "1" || _ColocarMensagemErroNaLinhaAoExtrairDDL == "TRUE" || _ColocarMensagemErroNaLinhaAoExtrairDDL == "SIM")
                    {
                        _ColocarMensagem = true;
                    }
                    else
                    {
                        _ColocarMensagem = false;
                    }
                }
                return _ColocarMensagem;
            }
            set
            {
                bool _ColocarMensagem = value;
                string _ColocarMensagemErroNaLinhaAoExtrairDDL = null;
                if (_ColocarMensagem)
                {
                    _ColocarMensagemErroNaLinhaAoExtrairDDL = "1";
                }
                else
                {
                    _ColocarMensagemErroNaLinhaAoExtrairDDL = "0";
                }
                csUtil.SalvarPreferencia("ColocarMensagemErroNaLinhaAoExtrairDDL", _ColocarMensagemErroNaLinhaAoExtrairDDL);
            }
        }
        

        /// <summary>
        /// Retorna ou salva o caminho do TNSNAMES.ora, que fica salvo no registro no caminho HKEY_CURRENT_USER\Software\ORACLE_Tools\Parametros\CaminhoTnsNames
        /// </summary>
        public string CaminhoTnsNames
        {
            get
            {
                string _CaminhoTnsNames;
                _CaminhoTnsNames = (string)csUtil.CarregarPreferencia("CaminhoTnsNames");
                return _CaminhoTnsNames;
            }
            set
            {
                string _CaminhoTnsNames;
                _CaminhoTnsNames = value;
                csUtil.SalvarPreferencia("CaminhoTnsNames", _CaminhoTnsNames);
            }
        }

        /// <summary>
        /// Retorna ou salva o nome do usuário do último login, que fica salvo no registro no caminho HKEY_CURRENT_USER\Software\ORACLE_Tools\Parametros\UltimoLoginUsuario
        /// </summary>
        public string UltimoLoginUsuario
        {
            get
            {
                string _UltimoLoginUsuario;
                _UltimoLoginUsuario = (string)csUtil.CarregarPreferencia("UltimoLoginUsuario");
                return _UltimoLoginUsuario;
            }
            set
            {
                string _UltimoLoginUsuario;
                _UltimoLoginUsuario = value;
                csUtil.SalvarPreferencia("UltimoLoginUsuario", _UltimoLoginUsuario);
            }
        }

        /// <summary>
        /// Retorna ou salva o nome do usuário do último login, que fica salvo no registro no caminho HKEY_CURRENT_USER\Software\ORACLE_Tools\Parametros\UltimoLoginUsuario
        /// </summary>
        public string UltimoLoginDatabase
        {
            get
            {
                string _UltimoLoginDatabase;
                _UltimoLoginDatabase = (string)csUtil.CarregarPreferencia("UltimoLoginDatabase");
                return _UltimoLoginDatabase;
            }
            set
            {
                string _UltimoLoginDatabase;
                _UltimoLoginDatabase = value;
                csUtil.SalvarPreferencia("UltimoLoginDatabase", _UltimoLoginDatabase);
            }
        }

    #endregion Propriedades

    #region Métodos públicos

        #region Info Banco

        public bool ConectouNoBanco(string p_Usuario, string p_Senha, string p_Database, bool p_SalvarPreferencia = true)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            OracleConnection con = new OracleConnection();
            try
            {
                con.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                con.Open();
                if (p_SalvarPreferencia)
                {
                    this.UltimoLoginUsuario = p_Usuario;
                    this.UltimoLoginDatabase = p_Database;
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
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ConectouNoBanco(p_Usuario, p_Senha, p_Database, p_SalvarPreferencia);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaUltimasCompilacoes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

        }

        public string InfoBanco(string p_Usuario, string p_Senha, string p_Database, ref string p_Mensagem)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            string _Retorno = "";
            string _Query = "";
            p_Mensagem = "";

            try
            {
                OracleCommand _Command = new OracleCommand();
                OracleConnection _Connection = new OracleConnection();
                OracleDataReader _DataReader;

                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Procurando se existem fontes do objeto
                _Query = "Select TO_CHAR(SYSDATE, 'DD/MM/YY HH24:MI:SS'), UPPER(SYS_CONTEXT('USERENV', 'SERVER_HOST')), UPPER(SYS_CONTEXT('USERENV', 'INSTANCE_NAME')), UPPER(SYS_CONTEXT('USERENV', 'CURRENT_USER')) FROM DUAL";
                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();

                if (_DataReader.Read())
                {
                    _Retorno = _DataReader.GetString(0) + " - Host: " + _DataReader.GetString(1) + " - Instance: " + _DataReader.GetString(2) + " - User: " + _DataReader.GetString(3) + " - Alias: " + p_Database.ToUpper();
                }

                // Fechando conexão e liberando objetos
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.InfoBanco(p_Usuario, p_Senha, p_Database, ref p_Mensagem);
                }
                else
                {
                    p_Mensagem = _Exception.ToString();
                    return "";
                }
            }

        }
        
        #endregion Info Banco

        #region TNSNAMES.ora

        /// <summary>
        /// Abre dialog para selecionar o caminho do TNSNAMES.ora e salva no registro
        /// </summary>
        public string SelecionaESalvaCaminhoTnsNames()
        {
            OpenFileDialog dlgAbrir = new OpenFileDialog();
            DialogResult _Resp;
            dlgAbrir.Title = "Informe o caminho do arquivo tnsnames.ora";
            dlgAbrir.Filter = "TNS Names|tnsnames.ora";
            dlgAbrir.Multiselect = false;
            _Resp = dlgAbrir.ShowDialog();
            if (_Resp == DialogResult.OK)
            {
                this.CaminhoTnsNames = dlgAbrir.FileName;
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
        public ArrayList ListaDatabasesDoTnsNames()
        {
            string _CaminhoCompleto = this.CaminhoTnsNames;
            if (_CaminhoCompleto == null)
            {
                return null;
            }
            else
            {
                if (File.Exists(_CaminhoCompleto))
                {
                    ArrayList _Retorno = new ArrayList();
                    string _ArquivoTexto = null;
                    int _Pos = -1;
                    string _TextoAux = "";
                    string[] _Split;

                    char[] _ArquivoChar;
                    StreamReader _StreamReader = File.OpenText(_CaminhoCompleto);
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

        #endregion TNSNAMES.ora

        #region Sobre Owners de banco

        /// <summary>
        /// Conecta no banco e retorna se existe um owner específico
        /// </summary>
        public bool ExisteOwner(string p_Usuario, string p_Senha, string p_Database, string p_QualOwner)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            bool _Existe = false;
            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();

                _Command.Connection = _Connection;
                _Command.CommandText = "SELECT 1 FROM ALL_OBJECTS WHERE OWNER = '" + p_QualOwner.Trim().ToUpper() + "'";
                _Command.CommandType = CommandType.Text;
                OracleDataReader _DataReader = _Command.ExecuteReader();
                if (_DataReader.Read())
                {
                    _Existe = true;
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _Existe;
            }
            catch (Exception _Exception)
            {
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ExisteOwner(p_Usuario, p_Senha, p_Database, p_QualOwner);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ExisteOwner", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        /// <summary>
        /// Conecta no banco e retorna a lista de Owners
        /// </summary>
        public ArrayList ListaOwners(string p_Usuario, string p_Senha, string p_Database)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            ArrayList _ListaOwners = null;
            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();

                _Command.Connection = _Connection;
                _Command.CommandText = "Select distinct owner from ALL_OBJECTS order by owner";
                _Command.CommandType = CommandType.Text;
                OracleDataReader _DataReader = _Command.ExecuteReader();
                _ListaOwners = new ArrayList();
                while (_DataReader.Read())
                {
                    _ListaOwners.Add(_DataReader.GetString(0));
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _ListaOwners;
            }
            catch (Exception _Exception)
            {
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaOwners(p_Usuario, p_Senha, p_Database);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaOwners", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        #endregion Sobre Owners de banco

        #region HORACIUS

        /// <summary>
        /// Conecta no banco e retorna os prefis definidos para o usuário no esquema HORACIUS
        /// </summary>
        public ArrayList ListaPerfilHoracius(string p_Usuario, string p_Senha, string p_Database, string p_NomeUser)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            ArrayList _ListaOwners = null;
            try
            {

                string _NomeScript = "";
                string _Script = "";
                _NomeScript = csUtil.PastaLocalExecutavel() + "Consulta_Horacius.sql";
                if (File.Exists(_NomeScript))
                {
                    _Script = File.ReadAllText(_NomeScript, Encoding.Default);
                }
                else
                {
                    MessageBox.Show("Arquivo não encontrado\n" + _Script, "ListaPerfilHoracius", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return _ListaOwners;
                }

                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();

                _Command.Connection = _Connection;
                _Script = _Script.Replace("NOME_USUARIO", p_NomeUser);
                _Command.CommandText = _Script;
                _Command.CommandType = CommandType.Text;
                OracleDataReader _DataReader = _Command.ExecuteReader();
                _ListaOwners = new ArrayList();
                while (_DataReader.Read())
                {
                    _ListaOwners.Add("SISTEMA: " + _DataReader.GetString(0) + " | PERFIL: " + _DataReader.GetString(1) + " | SOMENTE CONSULTA: " + _DataReader.GetString(2));
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _ListaOwners;
            }
            catch (Exception _Exception)
            {
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaPerfilHoracius(p_Usuario, p_Senha, p_Database, p_NomeUser);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaOwners", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        #endregion HORACIUS

        #region Listagens Objetos

        /// <summary>
        /// Classifica os objetos recebidos baseado na enumeração ClassificacaoObjeto da classe csObjetoDeBanco
        /// </summary>
        public ArrayList ClassificaObjetos(ArrayList p_ListaObjetos)
        {
            ArrayList _Retorno = new ArrayList();
            csObjetoDeBanco NovocsObjetoDeBanco = new csObjetoDeBanco();
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader = null;
            string _Consulta = "";
            string _OWNER = "";
            string _OBJECT_NAME = "";
            string _OBJECT_TYPE = "";
            DateTime _CREATED = DateTime.MinValue;
            //DateTime _LAST_DDL_TIME = DateTime.MinValue;
            DateTime _DataCriacaoTabela = DateTime.MinValue;

            string _ListaObjetos = "(";

            foreach (csObjetoDeBanco _csObjetoDeBanco in p_ListaObjetos)
            {
                _ListaObjetos = _ListaObjetos + "'" + _csObjetoDeBanco.Owner + "." + _csObjetoDeBanco.Nome + "',";
                NovocsObjetoDeBanco = new csObjetoDeBanco();
                NovocsObjetoDeBanco.Owner = _csObjetoDeBanco.Owner;
                NovocsObjetoDeBanco.Nome = _csObjetoDeBanco.Nome;
                NovocsObjetoDeBanco.ClassificacaoObjeto = csObjetoDeBanco.enuClassificacaoObjeto.Excluido;
                _Retorno.Add(NovocsObjetoDeBanco);
            }

            _ListaObjetos = _ListaObjetos.Substring(0, _ListaObjetos.Length - 1) + ") ";

            try
            {
                _Connection.ConnectionString = "User Id=" + _UsuarioDB + ";Password=" + _SenhaDB + ";Data Source=" + _AliasDatabase;
                _Connection.Open();
                _Command.Connection = _Connection;

                if (_TemTabContrConc == -1) // -1 = Indefinido
                {
                    _Consulta = "";
                    _Consulta = _Consulta + "SELECT COUNT(*) ";
                    _Consulta = _Consulta + "FROM ALL_OBJECTS ";
                    _Consulta = _Consulta + "WHERE OWNER = 'AESPROD' ";
                    _Consulta = _Consulta + "    AND OBJECT_NAME = 'AUDITORIA_OBJETOS' ";

                    _Command.CommandText = _Consulta;
                    _DataReader = _Command.ExecuteReader();
                    if (_DataReader.Read())
                    {
                        if (_DataReader.GetDecimal(0) > 0)
                        {
                            _TemTabContrConc = 1; // Diferente de zero = Tem
                        }
                        else
                        {
                            _TemTabContrConc = 0; // zero = Não tem
                        }
                    }
                    else
                    {
                        _TemTabContrConc = 0; // zero = Não tem
                    }
                    _DataReader.Close();

                }
                else if (_TemTabContrConc == 0) // zero = Não tem
                {
                    _Connection.Close();
                    _Connection.Dispose();
                    GC.Collect();
                    return p_ListaObjetos;
                }

                _Consulta = "";
                _Consulta = _Consulta + "SELECT CREATED ";
                _Consulta = _Consulta + "FROM ALL_OBJECTS ";
                _Consulta = _Consulta + "WHERE OWNER = 'AESPROD' ";
                _Consulta = _Consulta + "    AND OBJECT_NAME = 'AUDITORIA_OBJETOS' ";

                _Command.CommandText = _Consulta;
                _DataReader = _Command.ExecuteReader();
                if (_DataReader.Read())
                {
                    _DataCriacaoTabela = _DataReader.GetDateTime(0);
                }
                else
                {
                    _DataReader.Close();
                    _Connection.Close();
                    _Connection.Dispose();
                    GC.Collect();
                    return p_ListaObjetos;
                }
                _DataReader.Close();

                _Consulta = "";
                _Consulta = _Consulta + "SELECT OWNER, ";
                _Consulta = _Consulta + "    OBJECT_NAME, ";
                _Consulta = _Consulta + "    OBJECT_TYPE, ";
                _Consulta = _Consulta + "    CREATED, ";
                _Consulta = _Consulta + "    LAST_DDL_TIME ";
                _Consulta = _Consulta + "FROM ALL_OBJECTS ";
                _Consulta = _Consulta + "WHERE OWNER || '.' || OBJECT_NAME IN " + _ListaObjetos;

                _Command.CommandText = _Consulta;
                _DataReader = _Command.ExecuteReader();

                while (_DataReader.Read())
                {
                    _OWNER = _DataReader.GetString(0);
                    _OBJECT_NAME = _DataReader.GetString(1);
                    _OBJECT_TYPE = _DataReader.GetString(2);
                    _CREATED = _DataReader.GetDateTime(3);
                    //_LAST_DDL_TIME = _DataReader.GetDateTime(4);
                    foreach (csObjetoDeBanco _csObjetoDeBanco in _Retorno)
                    {
                        if (_csObjetoDeBanco.Owner.Trim().ToUpper() == _OWNER.Trim().ToUpper() && _csObjetoDeBanco.Nome.Trim().ToUpper() == _OBJECT_NAME.Trim().ToUpper())
                        {
                            if (_CREATED > _DataCriacaoTabela)
                            {
                                _csObjetoDeBanco.ClassificacaoObjeto = csObjetoDeBanco.enuClassificacaoObjeto.Novo;
                            }
                            else
                            {
                                _csObjetoDeBanco.ClassificacaoObjeto = csObjetoDeBanco.enuClassificacaoObjeto.Alterado;
                            }
                            //if (_OBJECT_TYPE.ToUpper().IndexOf("PACKAGE", 0) == -1)
                            //{
                            //    break;
                            //}
                        }
                    }
                }

            }
            catch (Exception _Exception)
            {
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    return this.ClassificaObjetos(p_ListaObjetos);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaUltimasCompilacoes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            return _Retorno;
        }

        /// <summary>
        /// Conecta no banco e retorna a lista de últimas compilações
        /// </summary>
        public ArrayList ListaUltimasCompilacoes(string p_Usuario, string p_Senha, string p_Database, int p_QuantosDias)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader = null;
            ArrayList _ListaObjetos = null;
            csObjetoDeBanco _csObjetoDeBanco;
            string _Consulta = "";
            bool _AcheiRepetido = false;
            bool _ProcurarUsuarioUltimaAlteracao = false;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;

                if (_TemTabContrConc == -1) // -1 = Indefinido
                {
                    _Consulta = "";
                    _Consulta = _Consulta + "SELECT COUNT(*) ";
                    _Consulta = _Consulta + "FROM ALL_OBJECTS ";
                    _Consulta = _Consulta + "WHERE OWNER = 'AESPROD' ";
                    _Consulta = _Consulta + "    AND OBJECT_NAME = 'AUDITORIA_OBJETOS' ";

                    _Command.CommandText = _Consulta;
                    _DataReader = _Command.ExecuteReader();
                    if (_DataReader.Read())
                    {
                        if (_DataReader.GetDecimal(0) > 0)
                        {
                            _TemTabContrConc = 1; // Diferente de zero = Tem
                            _ProcurarUsuarioUltimaAlteracao = true;
                        }
                        else
                        {
                            _TemTabContrConc = 0; // zero = Não tem
                        }
                    }
                    else
                    {
                        _TemTabContrConc = 0; // zero = Não tem
                    }
                    _DataReader.Close();

                }
                else if (_TemTabContrConc != 0) // Diferente de zero = Tem
                {
                    _ProcurarUsuarioUltimaAlteracao = true;
                }

                if (_ProcurarUsuarioUltimaAlteracao)
                {
                    _Consulta = "";
                    _Consulta = _Consulta + "Select owner, object_name, object_type, created, last_ddl_time, status, AESPROD.FNC_USUARIO_LAST_DDL_OBJETO(OWNER, OBJECT_NAME) As ULTIMO_ALTERADOR ";
                    _Consulta = _Consulta + "From ALL_OBJECTS ";
                    _Consulta = _Consulta + "Where last_ddl_time > sysdate - " + p_QuantosDias.ToString() + " ";
                    _Consulta = _Consulta + "And object_type not in ('TABLE', 'TABLE PARTITION', 'TABLE SUBPARTITION', 'SYNONYM', 'INDEX', 'INDEX PARTITION', 'JOB', 'LOB PARTITION', 'DIRECTORY', 'QUEUE', 'TYPE', 'LOB') ";
                    _Consulta = _Consulta + "UNION ALL ";
                    _Consulta = _Consulta + "Select owner, object_name, object_type, created, last_ddl_time, status, AESPROD.FNC_USUARIO_LAST_DDL_OBJETO(OWNER, OBJECT_NAME) As ULTIMO_ALTERADOR ";
                    _Consulta = _Consulta + "From ALL_OBJECTS ";
                    _Consulta = _Consulta + "Where created > sysdate - " + p_QuantosDias.ToString() + " ";
                    _Consulta = _Consulta + "And object_type = 'JOB' ";
                    _Consulta = _Consulta + "Order By last_ddl_time desc, owner, object_name ";
                }
                else
                {
                    _Consulta = "";
                    _Consulta = _Consulta + "Select owner, object_name, object_type, created, last_ddl_time, status, NULL As ULTIMO_ALTERADOR ";
                    _Consulta = _Consulta + "From ALL_OBJECTS ";
                    _Consulta = _Consulta + "Where last_ddl_time > sysdate - " + p_QuantosDias.ToString() + " ";
                    _Consulta = _Consulta + "And object_type not in ('TABLE', 'TABLE PARTITION', 'TABLE SUBPARTITION', 'SYNONYM', 'INDEX', 'INDEX PARTITION', 'JOB', 'LOB PARTITION', 'DIRECTORY', 'QUEUE', 'TYPE', 'LOB') ";
                    _Consulta = _Consulta + "UNION ALL ";
                    _Consulta = _Consulta + "Select owner, object_name, object_type, created, last_ddl_time, status, NULL As ULTIMO_ALTERADOR ";
                    _Consulta = _Consulta + "From ALL_OBJECTS ";
                    _Consulta = _Consulta + "Where created > sysdate - " + p_QuantosDias.ToString() + " ";
                    _Consulta = _Consulta + "And object_type = 'JOB' ";
                    _Consulta = _Consulta + "Order By last_ddl_time desc, owner, object_name ";
                }

                _Command.CommandText = _Consulta;
                _Command.CommandType = CommandType.Text;
                _DataReader = _Command.ExecuteReader();
                _ListaObjetos = new ArrayList();

                while (_DataReader.Read())
                {
                    _csObjetoDeBanco = new csObjetoDeBanco();
                    _csObjetoDeBanco.Owner = _DataReader.GetString(0);
                    _csObjetoDeBanco.Nome = _DataReader.GetString(1);
                    _csObjetoDeBanco.Status = _DataReader.GetString(5);
                    if (!_DataReader.IsDBNull(6))
                    {
                        _csObjetoDeBanco.UsuarioUltimaAlteracao = _DataReader.GetString(6);
                    }

                    if (_DataReader.GetString(2).IndexOf("PACKAGE") > -1)
                    {
                        OracleDataReader _DataReaderPackage = null;

                        _csObjetoDeBanco.Tipo = "PACKAGE";
                        _Consulta = "Select Min(created), Max(last_ddl_time) From ALL_OBJECTS Where owner = '" + _csObjetoDeBanco.Owner + "' And object_name = '" + _csObjetoDeBanco.Nome + "'";
                        _Command.CommandText = _Consulta;
                        _Command.CommandType = CommandType.Text;
                        _DataReaderPackage = _Command.ExecuteReader();
                        if (_DataReaderPackage.Read())
                        {
                            _csObjetoDeBanco.DataCriacao = _DataReaderPackage.GetDateTime(0);
                            _csObjetoDeBanco.DataAlteracao = _DataReaderPackage.GetDateTime(1);
                        }
                        _DataReaderPackage.Close();

                        _Consulta = "Select status From ALL_OBJECTS Where owner = '" + _csObjetoDeBanco.Owner + "' And object_name = '" + _csObjetoDeBanco.Nome + "'";
                        _Command.CommandText = _Consulta;
                        _Command.CommandType = CommandType.Text;
                        _DataReaderPackage = _Command.ExecuteReader();
                        _csObjetoDeBanco.Status = "";
                        while (_DataReaderPackage.Read())
                        {
                            if (_csObjetoDeBanco.Status == "")
                            {
                                _csObjetoDeBanco.Status = _DataReaderPackage.GetString(0);
                            }
                            else
                            {
                                if (_csObjetoDeBanco.Status == "VALID")
                                {
                                    _csObjetoDeBanco.Status = _DataReaderPackage.GetString(0);
                                }
                            }
                        }
                        _DataReaderPackage.Close();

                        _DataReaderPackage.Dispose();
                        GC.Collect();
                    }
                    else
                    {
                        _csObjetoDeBanco.Tipo = _DataReader.GetString(2);
                        _csObjetoDeBanco.DataCriacao = _DataReader.GetDateTime(3);
                        _csObjetoDeBanco.DataAlteracao = _DataReader.GetDateTime(4);
                    }
                    if (_csObjetoDeBanco.Tipo == "PACKAGE")
                    {
                        _AcheiRepetido = false;
                        foreach (csObjetoDeBanco item in _ListaObjetos)
                        {
                            if (item.Owner == _csObjetoDeBanco.Owner && item.Nome == _csObjetoDeBanco.Nome)
                            {
                                _AcheiRepetido = true;
                                break;
                            }
                        }
                        if (!_AcheiRepetido)
                        {
                            _ListaObjetos.Add(_csObjetoDeBanco);
                        }
                    }
                    else
                    {
                        _ListaObjetos.Add(_csObjetoDeBanco);
                    }
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _ListaObjetos;
            }
            catch (Exception _Exception)
            {
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaUltimasCompilacoes(p_Usuario, p_Senha, p_Database, p_QuantosDias);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaUltimasCompilacoes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        /// <summary>
        /// Conecta no banco e retorna a lista de objetos
        /// </summary>
        public ArrayList ListaObjetos(string p_Usuario, string p_Senha, string p_Database, string p_Owner, string p_Tipo, string p_Nome, ArrayList p_TiposPermitidos)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader = null;
            ArrayList _ListaObjetos = null;
            csObjetoDeBanco _csObjetoDeBanco;
            string _Consulta = "";
            bool _ProcurarUsuarioUltimaAlteracao = false;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;

                if (_TemTabContrConc == -1) // -1 = Indefinido
                {
                    _Consulta = "";
                    _Consulta = _Consulta + "SELECT COUNT(*) ";
                    _Consulta = _Consulta + "FROM ALL_OBJECTS ";
                    _Consulta = _Consulta + "WHERE OWNER = 'AESPROD' ";
                    _Consulta = _Consulta + "    AND OBJECT_NAME = 'AUDITORIA_OBJETOS' ";

                    _Command.CommandText = _Consulta;
                    _DataReader = _Command.ExecuteReader();
                    if (_DataReader.Read())
                    {
                        if (_DataReader.GetDecimal(0) > 0)
                        {
                            _TemTabContrConc = 1; // Diferente de zero = Tem
                            _ProcurarUsuarioUltimaAlteracao = true;
                        }
                        else
                        {
                            _TemTabContrConc = 0; // zero = Não tem
                        }
                    }
                    else
                    {
                        _TemTabContrConc = 0; // zero = Não tem
                    }
                    _DataReader.Close();

                }
                else if (_TemTabContrConc != 0) // Diferente de zero = Tem
                {
                    _ProcurarUsuarioUltimaAlteracao = true;
                }

                if (_ProcurarUsuarioUltimaAlteracao)
                {
                    _Consulta = "";
                    _Consulta = _Consulta + "Select owner, "; // 0
                    _Consulta = _Consulta + "    object_name, "; // 1
                    _Consulta = _Consulta + "    object_type, "; // 2
                    _Consulta = _Consulta + "    created, "; // 3
                    _Consulta = _Consulta + "    last_ddl_time, "; // 4
                    _Consulta = _Consulta + "    status, "; // 5
                    _Consulta = _Consulta + "    AESPROD.FNC_USUARIO_LAST_DDL_OBJETO(OWNER, OBJECT_NAME) As ULTIMO_ALTERADOR "; // 6
                    _Consulta = _Consulta + "From ALL_OBJECTS ";
                    _Consulta = _Consulta + "Where 1 = 1 ";
                }
                else
                {
                    _Consulta = "";
                    _Consulta = _Consulta + "Select owner, "; // 0
                    _Consulta = _Consulta + "    object_name, "; // 1
                    _Consulta = _Consulta + "    object_type, "; // 2
                    _Consulta = _Consulta + "    created, "; // 3
                    _Consulta = _Consulta + "    last_ddl_time, "; // 4
                    _Consulta = _Consulta + "    status, "; // 5
                    _Consulta = _Consulta + "    NULL As ULTIMO_ALTERADOR "; // 6
                    _Consulta = _Consulta + "From ALL_OBJECTS ";
                    _Consulta = _Consulta + "Where 1 = 1 ";
                }
                
                if ((p_Owner.Trim().Length > 0) && (p_Owner.Trim().ToUpper() != "TODOS"))
                {
                    _Consulta = _Consulta + " And owner = '" + p_Owner.Trim().ToUpper() + "'";
                }

                if ((p_Tipo.Trim().Length > 0) && (p_Tipo.Trim().ToUpper() != "TODOS"))
                {
                    _Consulta = _Consulta + " And object_type = '" + p_Tipo.Trim().ToUpper() + "'";
                }

                if (p_TiposPermitidos != null)
                {
                    if (p_TiposPermitidos.Count > 0)
                    {
                        _Consulta = _Consulta + " And object_type in (";
                        foreach (string _Tipo in p_TiposPermitidos)
                        {
                            _Consulta = _Consulta + "'" + _Tipo + "', ";
                        }
                        _Consulta = _Consulta.Substring(0, _Consulta.Length - 2) + ")";
                    }
                }

                if (p_Nome.Trim().Length > 0)
                {
                    _Consulta = _Consulta + " And upper(object_name) like '%" + p_Nome.Trim().ToUpper() + "%'";
                }

                _Consulta = _Consulta + " Order By owner, object_type, object_name";

                _Command.CommandText = _Consulta;
                _Command.CommandType = CommandType.Text;
                _DataReader = _Command.ExecuteReader();
                _ListaObjetos = new ArrayList();
                while (_DataReader.Read())
                {
                    _csObjetoDeBanco = new csObjetoDeBanco();
                    _csObjetoDeBanco.Owner = _DataReader.GetString(0);
                    _csObjetoDeBanco.Nome = _DataReader.GetString(1);
                    _csObjetoDeBanco.Tipo = _DataReader.GetString(2);
                    _csObjetoDeBanco.Status = _DataReader.GetString(5);
                    if (!_DataReader.IsDBNull(6))
	                {
                        _csObjetoDeBanco.UsuarioUltimaAlteracao = _DataReader.GetString(6);
	                }

                    if (_csObjetoDeBanco.Tipo.Trim().ToUpper() == "PACKAGE")
                    {
                        OracleDataReader _DataReaderPackage = null;

                        _Consulta = "Select Min(created), Max(last_ddl_time) From ALL_OBJECTS Where owner = '" + _csObjetoDeBanco.Owner + "' And object_name = '" + _csObjetoDeBanco.Nome + "'";
                        _Command.CommandText = _Consulta;
                        _Command.CommandType = CommandType.Text;
                        _DataReaderPackage = _Command.ExecuteReader();
                        if (_DataReaderPackage.Read())
                        {
                            _csObjetoDeBanco.DataCriacao = _DataReaderPackage.GetDateTime(0);
                            _csObjetoDeBanco.DataAlteracao = _DataReaderPackage.GetDateTime(1);
                        }
                        else
                        {
                            _csObjetoDeBanco.DataCriacao = _DataReader.GetDateTime(3);
                            _csObjetoDeBanco.DataAlteracao = _DataReader.GetDateTime(4);
                        }
                        _DataReaderPackage.Close();

                        _Consulta = "Select status From ALL_OBJECTS Where owner = '" + _csObjetoDeBanco.Owner + "' And object_name = '" + _csObjetoDeBanco.Nome + "'";
                        _Command.CommandText = _Consulta;
                        _Command.CommandType = CommandType.Text;
                        _DataReaderPackage = _Command.ExecuteReader();
                        _csObjetoDeBanco.Status = "";
                        while (_DataReaderPackage.Read())
                        {
                            if (_csObjetoDeBanco.Status == "")
                            {
                                _csObjetoDeBanco.Status = _DataReaderPackage.GetString(0);
                            }
                            else
                            {
                                if (_csObjetoDeBanco.Status == "VALID")
                                {
                                    _csObjetoDeBanco.Status = _DataReaderPackage.GetString(0);
                                }
                            }
                        }
                        _DataReaderPackage.Close();

                        _DataReaderPackage.Dispose();
                        GC.Collect();
                    }
                    else
                    {
                        _csObjetoDeBanco.DataCriacao = _DataReader.GetDateTime(3);
                        _csObjetoDeBanco.DataAlteracao = _DataReader.GetDateTime(4);
                    }
                    _ListaObjetos.Add(_csObjetoDeBanco);
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _ListaObjetos;
            }
            catch (Exception _Exception)
            {
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaObjetos(p_Usuario, p_Senha, p_Database, p_Owner, p_Tipo, p_Nome, p_TiposPermitidos);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaObjetos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        /// <summary>
        /// Preenche uma list view qualquer a partir de uma query
        /// </summary>
        public void PreencheLvw(string p_Usuario, string p_Senha, string p_Database, string p_Query, bool p_SoCriarColunasSeNaoTiverNenhuma, ref string p_Mensagem, ref ListView p_Lvw)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            try
            {
                OleDbCommand _Command = new OleDbCommand();
                OleDbConnection _Connection = new OleDbConnection();
                OleDbDataReader _DataReader;
                ListViewItem _ListViewItem = null;
                ColumnHeader _lvwColuna = null;

                _Connection.ConnectionString = "Provider=OraOLEDB.Oracle; User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database + ";ChunkSize=65535;"; // 65535 é o máximo. se colocar um parâmetro maior que isso vão vir menos caracteres.
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                _Command.CommandText = p_Query;
                _DataReader = _Command.ExecuteReader();

                p_Lvw.Visible = false;

                if (p_SoCriarColunasSeNaoTiverNenhuma)
                {
                    if (p_Lvw.Columns.Count == 0)
                    {
                        p_Lvw.Columns.Clear();
                        for (int i = 0; i < _DataReader.FieldCount; i++)
                        {
                            _lvwColuna = p_Lvw.Columns.Add(_DataReader.GetName(i));
                        }
                        for (int i = 0; i < _DataReader.FieldCount; i++)
                        {
                            p_Lvw.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                        }
                    }
                }
                else
                {
                    p_Lvw.Columns.Clear();
                    for (int i = 0; i < _DataReader.FieldCount; i++)
                    {
                        _lvwColuna = p_Lvw.Columns.Add(_DataReader.GetName(i));
                    }
                    for (int i = 0; i < _DataReader.FieldCount; i++)
                    {
                        p_Lvw.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                    }
                }

                p_Lvw.Items.Clear();

                p_Lvw.Visible = true;



                while (_DataReader.Read())
                {
                    if (_DataReader.IsDBNull(0))
                    {
                        _ListViewItem = p_Lvw.Items.Add("NULL");
                    }
                    else
                    {
                        switch (_DataReader.GetFieldType(0).Name)
                        {
                            case "DateTime":
                                _ListViewItem = p_Lvw.Items.Add(_DataReader.GetDateTime(0).ToString("dd/MM/yy HH:mm:ss"));
                                break;
                            case "Decimal":
                                _ListViewItem = p_Lvw.Items.Add(_DataReader.GetDecimal(0).ToString());
                                break;
                            case "String":
                                _ListViewItem = p_Lvw.Items.Add(_DataReader.GetString(0));
                                break;
                            default:
                                _ListViewItem = p_Lvw.Items.Add(_DataReader.GetString(0));
                                break;
                        }
                    }


                    for (int i = 1; i < _DataReader.FieldCount; i++)
                    {
                        if (_DataReader.IsDBNull(i))
                        {
                            _ListViewItem.SubItems.Add("NULL");
                        }
                        else
                        {
                            switch (_DataReader.GetFieldType(i).Name)
                            {
                                case "DateTime":
                                    _ListViewItem.SubItems.Add(_DataReader.GetDateTime(i).ToString("dd/MM/yy HH:mm:ss"));
                                    break;
                                case "Decimal":
                                    _ListViewItem.SubItems.Add(_DataReader.GetDecimal(i).ToString());
                                    break;
                                case "String":
                                    _ListViewItem.SubItems.Add(_DataReader.GetString(i));
                                    break;
                                //case "Byte[]":
                                //    byte[] _Byte = new byte[1024];
                                //    _DataReader.GetBytes(i, 0, _Byte, 0, _Byte.Length);
                                //    string _Valor = "";
                                //    for (int j  = 0; j < _Byte.Length; j++)
                                //    {
                                //        //if (_Byte[j] == 0)
                                //        //{
                                //        //    break;
                                //        //}
                                //        _Valor = _Valor + _Byte[j].ToString();
                                //    }
                                //    _ListViewItem.SubItems.Add(_Valor.Trim());
                                //    break;
                                default:
                                    _ListViewItem.SubItems.Add(_DataReader.GetFieldType(i).Name);
                                    break;
                            }
                        }
                    }
                }

                if (p_Lvw.Items.Count > 0)
                {
                    for (int i = 0; i < _DataReader.FieldCount; i++)
                    {
                        p_Lvw.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
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
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    this.PreencheLvw(p_Usuario, p_Senha, p_Database, p_Query, p_SoCriarColunasSeNaoTiverNenhuma, ref p_Mensagem, ref p_Lvw);
                }
                else
                {
                    p_Mensagem = _Exception.Message;
                }
            }
        }

        /// <summary>
        /// Retorna a lista de objetos inválidos do banco
        /// </summary>
        public ArrayList ListaObjetosInvalidos(string p_Usuario, string p_Senha, string p_Database)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader = null;
            ArrayList _ListaObjetosInvalidos = null;
            csObjetoDeBanco _csObjetoDeBanco;
            string _Consulta = "";
            bool _AcheiRepetido = false;
            string _AuxTexto = "";
            bool _ProcurarUsuarioUltimaAlteracao = false;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;

                if (_TemTabContrConc == -1) // -1 = Indefinido
                {
                    _Consulta = "";
                    _Consulta = _Consulta + "SELECT COUNT(*) ";
                    _Consulta = _Consulta + "FROM ALL_OBJECTS ";
                    _Consulta = _Consulta + "WHERE OWNER = 'AESPROD' ";
                    _Consulta = _Consulta + "    AND OBJECT_NAME = 'AUDITORIA_OBJETOS' ";

                    _Command.CommandText = _Consulta;
                    _DataReader = _Command.ExecuteReader();
                    if (_DataReader.Read())
                    {
                        if (_DataReader.GetDecimal(0) > 0)
                        {
                            _TemTabContrConc = 1; // Diferente de zero = Tem
                            _ProcurarUsuarioUltimaAlteracao = true;
                        }
                        else
                        {
                            _TemTabContrConc = 0; // zero = Não tem
                        }
                    }
                    else
                    {
                        _TemTabContrConc = 0; // zero = Não tem
                    }
                    _DataReader.Close();

                }
                else if (_TemTabContrConc != 0) // Diferente de zero = Tem
                {
                    _ProcurarUsuarioUltimaAlteracao = true;
                }

                if (_ProcurarUsuarioUltimaAlteracao)
                {
                    _Consulta = "";
                    _Consulta = _Consulta + "SELECT OWNER, OBJECT_NAME, OBJECT_TYPE, CREATED, LAST_DDL_TIME, STATUS, ULTIMO_ALTERADOR ";
                    _Consulta = _Consulta + "FROM (SELECT OWNER, OBJECT_NAME, OBJECT_TYPE, CREATED, LAST_DDL_TIME, STATUS, AESPROD.FNC_USUARIO_LAST_DDL_OBJETO(OWNER, OBJECT_NAME) As ULTIMO_ALTERADOR ";
                    _Consulta = _Consulta + "    FROM ALL_OBJECTS ";
                    _Consulta = _Consulta + "    WHERE OBJECT_TYPE <> 'SYNONYM' ";
                    _Consulta = _Consulta + "        AND STATUS <> 'VALID' ";
                    _Consulta = _Consulta + "    UNION ";
                    _Consulta = _Consulta + "    SELECT OWNER, OBJECT_NAME, OBJECT_TYPE, CREATED, LAST_DDL_TIME, STATUS, AESPROD.FNC_USUARIO_LAST_DDL_OBJETO(OWNER, OBJECT_NAME) As ULTIMO_ALTERADOR ";
                    _Consulta = _Consulta + "    FROM ALL_OBJECTS ";
                    _Consulta = _Consulta + "    WHERE OBJECT_TYPE = 'SYNONYM' ";
                    _Consulta = _Consulta + "        AND OWNER NOT IN ('GIP981', 'RAJADAS') ";
                    _Consulta = _Consulta + "        AND STATUS <> 'VALID' ";
                    _Consulta = _Consulta + "    ) ";
                    _Consulta = _Consulta + "ORDER BY LAST_DDL_TIME DESC, OWNER, OBJECT_NAME ";
                }
                else
                {
                    _Consulta = "";
                    _Consulta = _Consulta + "SELECT OWNER, OBJECT_NAME, OBJECT_TYPE, CREATED, LAST_DDL_TIME, STATUS, ULTIMO_ALTERADOR ";
                    _Consulta = _Consulta + "FROM (SELECT OWNER, OBJECT_NAME, OBJECT_TYPE, CREATED, LAST_DDL_TIME, STATUS, NULL AS ULTIMO_ALTERADOR ";
                    _Consulta = _Consulta + "    FROM ALL_OBJECTS ";
                    _Consulta = _Consulta + "    WHERE OBJECT_TYPE <> 'SYNONYM' ";
                    _Consulta = _Consulta + "        AND STATUS <> 'VALID' ";
                    _Consulta = _Consulta + "    UNION ";
                    _Consulta = _Consulta + "    SELECT OWNER, OBJECT_NAME, OBJECT_TYPE, CREATED, LAST_DDL_TIME, STATUS, NULL AS ULTIMO_ALTERADOR ";
                    _Consulta = _Consulta + "    FROM ALL_OBJECTS ";
                    _Consulta = _Consulta + "    WHERE OBJECT_TYPE = 'SYNONYM' ";
                    _Consulta = _Consulta + "        AND OWNER NOT IN ('GIP981', 'RAJADAS') ";
                    _Consulta = _Consulta + "        AND STATUS <> 'VALID' ";
                    _Consulta = _Consulta + "    ) ";
                    _Consulta = _Consulta + "ORDER BY LAST_DDL_TIME DESC, OWNER, OBJECT_NAME ";
                }

                _Command.CommandText = _Consulta;
                _Command.CommandType = CommandType.Text;
                _DataReader = _Command.ExecuteReader();
                _ListaObjetosInvalidos = new ArrayList();

                while (_DataReader.Read())
                {
                    _csObjetoDeBanco = new csObjetoDeBanco();
                    _csObjetoDeBanco.Owner = _DataReader.GetString(0);
                    _csObjetoDeBanco.Nome = _DataReader.GetString(1);
                    _csObjetoDeBanco.Status = _DataReader.GetString(5);
                    if (!_DataReader.IsDBNull(6))
                    {
                        _csObjetoDeBanco.UsuarioUltimaAlteracao = _DataReader.GetString(6);
                    }

                    if (_DataReader.GetString(2).IndexOf("PACKAGE") > -1)
                    {
                        OracleDataReader _DataReaderPackage = null;

                        _csObjetoDeBanco.Tipo = "PACKAGE";
                        _Consulta = "Select Min(created), Max(last_ddl_time) From ALL_OBJECTS Where owner = '" + _csObjetoDeBanco.Owner + "' And object_name = '" + _csObjetoDeBanco.Nome + "'";
                        _Command.CommandText = _Consulta;
                        _Command.CommandType = CommandType.Text;
                        _DataReaderPackage = _Command.ExecuteReader();
                        if (_DataReaderPackage.Read())
                        {
                            _csObjetoDeBanco.DataCriacao = _DataReaderPackage.GetDateTime(0);
                            _csObjetoDeBanco.DataAlteracao = _DataReaderPackage.GetDateTime(1);
                        }
                        _DataReaderPackage.Close();

                        _Consulta = "Select status From ALL_OBJECTS Where owner = '" + _csObjetoDeBanco.Owner + "' And object_name = '" + _csObjetoDeBanco.Nome + "'";
                        _Command.CommandText = _Consulta;
                        _Command.CommandType = CommandType.Text;
                        _DataReaderPackage = _Command.ExecuteReader();
                        _csObjetoDeBanco.Status = "";
                        while (_DataReaderPackage.Read())
                        {
                            if (_csObjetoDeBanco.Status == "")
                            {
                                _csObjetoDeBanco.Status = _DataReaderPackage.GetString(0);
                            }
                            else
                            {
                                if (_csObjetoDeBanco.Status == "VALID")
                                {
                                    _csObjetoDeBanco.Status = _DataReaderPackage.GetString(0);
                                }
                            }
                        }
                        _DataReaderPackage.Close();

                        _DataReaderPackage.Dispose();
                        GC.Collect();
                    }
                    else
                    {
                        _csObjetoDeBanco.Tipo = _DataReader.GetString(2);
                        _csObjetoDeBanco.DataCriacao = _DataReader.GetDateTime(3);
                        _csObjetoDeBanco.DataAlteracao = _DataReader.GetDateTime(4);
                    }
                    if (_csObjetoDeBanco.Tipo == "PACKAGE")
                    {
                        _AcheiRepetido = false;
                        foreach (csObjetoDeBanco item in _ListaObjetosInvalidos)
                        {
                            if (item.Owner == _csObjetoDeBanco.Owner && item.Nome == _csObjetoDeBanco.Nome)
                            {
                                _AcheiRepetido = true;
                                break;
                            }
                        }
                        if (!_AcheiRepetido)
                        {
                            _ListaObjetosInvalidos.Add(_csObjetoDeBanco);
                        }
                    }
                    else
                    {
                        _ListaObjetosInvalidos.Add(_csObjetoDeBanco);
                    }
                }

                foreach (csObjetoDeBanco item in _ListaObjetosInvalidos)
                {

                    OracleDataReader _DataReaderErros = null;

                    _Consulta = "";
                    _Consulta = _Consulta + "SELECT ERR.LINE, ";
                    _Consulta = _Consulta + "    ERR.POSITION, ";
                    _Consulta = _Consulta + "    ERR.TEXT, ";
                    _Consulta = _Consulta + "    SOU.TEXT AS FONTE ";
                    _Consulta = _Consulta + "FROM ALL_ERRORS ERR ";
                    _Consulta = _Consulta + "    LEFT JOIN ALL_SOURCE SOU ";
                    _Consulta = _Consulta + "    ON ERR.OWNER = SOU.OWNER ";
                    _Consulta = _Consulta + "    AND ERR.NAME = SOU.NAME ";
                    _Consulta = _Consulta + "    AND ERR.TYPE = SOU.TYPE ";
                    _Consulta = _Consulta + "    AND ERR.LINE = SOU.LINE ";
                    _Consulta = _Consulta + "WHERE ERR.OWNER = '" + item.Owner + "' ";
                    _Consulta = _Consulta + "    AND ERR.NAME = '" + item.Nome + "' ";
                    _Consulta = _Consulta + "    AND ERR.TYPE = '" + item.Tipo + "' ";
                    _Consulta = _Consulta + "    AND UPPER(ERR.TEXT) NOT LIKE '%STATEMENT IGNORED%' ";
                    _Consulta = _Consulta + "ORDER BY ERR.OWNER, ERR.NAME, ERR.TYPE, ERR.SEQUENCE ";

                    _Command.CommandText = _Consulta;
                    _Command.CommandType = CommandType.Text;
                    _DataReaderErros = _Command.ExecuteReader();

                    while (_DataReaderErros.Read())
                    {
                        _AuxTexto = _DataReaderErros.GetString(2);
                        _AuxTexto = _AuxTexto.Replace("\n", " ");
                        _AuxTexto = _AuxTexto.Replace("\r", " ");

                        if (item.MensagemErro.Trim().Length > 0)
                        {
                            item.MensagemErro = item.MensagemErro + "\r\n";
                        }

                        if (_DataReaderErros.GetDecimal(0) > 0)
                        {
                            item.MensagemErro = item.MensagemErro + "Nº LINHA:" + _DataReaderErros.GetDecimal(0) + " - " + "Nº COLUNA:" + _DataReaderErros.GetDecimal(1) + " - ";
                        }

                        item.MensagemErro = item.MensagemErro + "MENSAGEM: " + _AuxTexto;
                        if (!_DataReaderErros.IsDBNull(3))
                        {
                            item.MensagemErro = item.MensagemErro + " - " + "TEXTO DA LINHA " + _DataReaderErros.GetDecimal(0) + ": " + _DataReaderErros.GetString(3).Trim();
                        }
                    }

                    _DataReaderErros.Close();
                    _DataReaderErros.Dispose();
                    GC.Collect();
                }

                _DataReader.Close();
                _DataReader.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _ListaObjetosInvalidos;
            }
            catch (Exception _Exception)
            {
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaObjetosInvalidos(p_Usuario, p_Senha, p_Database);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaObjetosInvalidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        #endregion Listagens Objetos

        #region Listagens Usuários

        /// <summary>
        /// Conecta no banco e retorna a lista de profiles de usuários que existem
        /// </summary>
        public ArrayList ListaProfiles(string p_Usuario, string p_Senha, string p_Database)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            ArrayList _ListaProfiles = null;
            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();

                _Command.Connection = _Connection;
                _Command.CommandText = "Select distinct profile from ALL_USERS order by profile";
                _Command.CommandType = CommandType.Text;
                OracleDataReader _DataReader = _Command.ExecuteReader();
                _ListaProfiles = new ArrayList();
                while (_DataReader.Read())
                {
                    _ListaProfiles.Add(_DataReader.GetString(0));
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _ListaProfiles;
            }
            catch (Exception _Exception)
            {
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaProfiles(p_Usuario, p_Senha, p_Database);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaProfiles", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        /// <summary>
        /// Conecta no banco e retorna a lista de usuários
        /// </summary>
        public ArrayList ListaUsuarios(string p_Usuario, string p_Senha, string p_Database, string p_Owner, string p_Profile, string p_NomeUsuario, string p_Where)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            
            ArrayList _ListaUsuarios = null;
            csUsuarioBanco _csUsuario;
            string _Consulta = "";
            try
            {
                OracleDataReader _DataReader;
                bool Existe_Tabela = false;

                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                
                _Command.Connection = _Connection;
                //Verificando se existe a tabela de ORADBA.SGC_USUARIOS_LOGADOS
                _Consulta = "";
                _Consulta = _Consulta + "SELECT 1 ";
                _Consulta = _Consulta + "FROM ALL_OBJECTS ";
                _Consulta = _Consulta + "WHERE OWNER = 'ORADBA' ";
                _Consulta = _Consulta + "    AND OBJECT_NAME = 'SGC_USUARIOS_LOGADOS' ";

                _Command.CommandText = _Consulta;
                _Command.CommandType = CommandType.Text;
                _DataReader = _Command.ExecuteReader();
                if (_DataReader.Read())
                {
                    Existe_Tabela = true;
                }
                else
                {
                    Existe_Tabela = false;
                }
                _DataReader.Close();

                switch (p_Owner.Trim().ToUpper())
                {
                    case "TODOS":
                        if (Existe_Tabela)
                        {
                            _Consulta = "";
                            _Consulta = _Consulta + "SELECT ALL_USERS.USERNAME, "; // 0
                            _Consulta = _Consulta + "    ALL_USERS.CREATED, "; // 1
                            _Consulta = _Consulta + "    ALL_USERS.PROFILE, "; // 2
                            _Consulta = _Consulta + "    ALL_USERS.ACCOUNT_STATUS, "; // 3
                            _Consulta = _Consulta + "    ALL_USERS.LOCK_DATE, "; // 4
                            _Consulta = _Consulta + "    ALL_USERS.EXPIRY_DATE, "; // 5
                            _Consulta = _Consulta + "    ORADBA.SGC_USUARIOS_LOGADOS.DATA AS ULTIMO_LOGIN "; // 6
                            _Consulta = _Consulta + "FROM ALL_USERS ";
                            _Consulta = _Consulta + "    LEFT JOIN ORADBA.SGC_USUARIOS_LOGADOS ";
                            _Consulta = _Consulta + "    ON ALL_USERS.USERNAME = ORADBA.SGC_USUARIOS_LOGADOS.USUARIO ";
                            _Consulta = _Consulta + "WHERE 1 = 1 ";
                        }
                        else
                        {
                            _Consulta = "";
                            _Consulta = _Consulta + "SELECT ALL_USERS.USERNAME, "; // 0
                            _Consulta = _Consulta + "    ALL_USERS.CREATED, "; // 1
                            _Consulta = _Consulta + "    ALL_USERS.PROFILE, "; // 2
                            _Consulta = _Consulta + "    ALL_USERS.ACCOUNT_STATUS, "; // 3
                            _Consulta = _Consulta + "    ALL_USERS.LOCK_DATE, "; // 4
                            _Consulta = _Consulta + "    ALL_USERS.EXPIRY_DATE, "; // 5
                            _Consulta = _Consulta + "    NULL AS ULTIMO_LOGIN "; // 6
                            _Consulta = _Consulta + "FROM ALL_USERS ";
                            _Consulta = _Consulta + "WHERE 1 = 1 ";
                        }
                        break;

                    case "QUE SEJA OWNER":
                        if (Existe_Tabela)
                        {
                            _Consulta = "";
                            _Consulta = _Consulta + "SELECT ALL_USERS.USERNAME, ";
                            _Consulta = _Consulta + "    ALL_USERS.CREATED, ";
                            _Consulta = _Consulta + "    ALL_USERS.PROFILE, ";
                            _Consulta = _Consulta + "    ALL_USERS.ACCOUNT_STATUS, ";
                            _Consulta = _Consulta + "    ALL_USERS.LOCK_DATE, ";
                            _Consulta = _Consulta + "    ALL_USERS.EXPIRY_DATE, ";
                            _Consulta = _Consulta + "    ORADBA.SGC_USUARIOS_LOGADOS.DATA AS ULTIMO_LOGIN ";
                            _Consulta = _Consulta + "FROM ALL_USERS ";
                            _Consulta = _Consulta + "    LEFT JOIN ORADBA.SGC_USUARIOS_LOGADOS ";
                            _Consulta = _Consulta + "    ON ALL_USERS.USERNAME = ORADBA.SGC_USUARIOS_LOGADOS.USUARIO ";
                            _Consulta = _Consulta + "WHERE ALL_USERS.USERNAME IN ( ";
                            _Consulta = _Consulta + "    SELECT DISTINCT OWNER ";
                            _Consulta = _Consulta + "    FROM ALL_OBJECTS) ";
                        }
                        else
                        {
                            _Consulta = "";
                            _Consulta = _Consulta + "SELECT ALL_USERS.USERNAME, ";
                            _Consulta = _Consulta + "    ALL_USERS.CREATED, ";
                            _Consulta = _Consulta + "    ALL_USERS.PROFILE, ";
                            _Consulta = _Consulta + "    ALL_USERS.ACCOUNT_STATUS, ";
                            _Consulta = _Consulta + "    ALL_USERS.LOCK_DATE, ";
                            _Consulta = _Consulta + "    ALL_USERS.EXPIRY_DATE, ";
                            _Consulta = _Consulta + "    NULL AS ULTIMO_LOGIN ";
                            _Consulta = _Consulta + "FROM ALL_USERS ";
                            _Consulta = _Consulta + "WHERE ALL_USERS.USERNAME IN ( ";
                            _Consulta = _Consulta + "    SELECT DISTINCT OWNER ";
                            _Consulta = _Consulta + "    FROM ALL_OBJECTS) ";
                        }
                        

                        break;

                    case "QUE NÃO SEJA OWNER":
                        if (Existe_Tabela)
                        {
                            _Consulta = "";
                            _Consulta = _Consulta + "SELECT ALL_USERS.USERNAME, ";
                            _Consulta = _Consulta + "    ALL_USERS.CREATED, ";
                            _Consulta = _Consulta + "    ALL_USERS.PROFILE, ";
                            _Consulta = _Consulta + "    ALL_USERS.ACCOUNT_STATUS, ";
                            _Consulta = _Consulta + "    ALL_USERS.LOCK_DATE, ";
                            _Consulta = _Consulta + "    ALL_USERS.EXPIRY_DATE, ";
                            _Consulta = _Consulta + "    ORADBA.SGC_USUARIOS_LOGADOS.DATA AS ULTIMO_LOGIN ";
                            _Consulta = _Consulta + "FROM ALL_USERS ";
                            _Consulta = _Consulta + "    LEFT JOIN ORADBA.SGC_USUARIOS_LOGADOS ";
                            _Consulta = _Consulta + "    ON ALL_USERS.USERNAME = ORADBA.SGC_USUARIOS_LOGADOS.USUARIO ";
                            _Consulta = _Consulta + "WHERE ALL_USERS.USERNAME NOT IN ( ";
                            _Consulta = _Consulta + "    SELECT DISTINCT OWNER ";
                            _Consulta = _Consulta + "    FROM ALL_OBJECTS) ";
                        }
                        else
                        {
                            _Consulta = "";
                            _Consulta = _Consulta + "SELECT ALL_USERS.USERNAME, ";
                            _Consulta = _Consulta + "    ALL_USERS.CREATED, ";
                            _Consulta = _Consulta + "    ALL_USERS.PROFILE, ";
                            _Consulta = _Consulta + "    ALL_USERS.ACCOUNT_STATUS, ";
                            _Consulta = _Consulta + "    ALL_USERS.LOCK_DATE, ";
                            _Consulta = _Consulta + "    ALL_USERS.EXPIRY_DATE, ";
                            _Consulta = _Consulta + "    NULL AS ULTIMO_LOGIN ";
                            _Consulta = _Consulta + "FROM ALL_USERS ";
                            _Consulta = _Consulta + "WHERE ALL_USERS.USERNAME NOT IN ( ";
                            _Consulta = _Consulta + "    SELECT DISTINCT OWNER ";
                            _Consulta = _Consulta + "    FROM ALL_OBJECTS) ";
                        }
                        break;

                    default:
                        if (Existe_Tabela)
                        {
                            _Consulta = "";
                            _Consulta = _Consulta + "SELECT ALL_USERS.USERNAME, ";
                            _Consulta = _Consulta + "    ALL_USERS.CREATED, ";
                            _Consulta = _Consulta + "    ALL_USERS.PROFILE, ";
                            _Consulta = _Consulta + "    ALL_USERS.ACCOUNT_STATUS, ";
                            _Consulta = _Consulta + "    ALL_USERS.LOCK_DATE, ";
                            _Consulta = _Consulta + "    ALL_USERS.EXPIRY_DATE, ";
                            _Consulta = _Consulta + "    ORADBA.SGC_USUARIOS_LOGADOS.DATA AS ULTIMO_LOGIN ";
                            _Consulta = _Consulta + "FROM ALL_USERS ";
                            _Consulta = _Consulta + "    LEFT JOIN ORADBA.SGC_USUARIOS_LOGADOS ";
                            _Consulta = _Consulta + "    ON ALL_USERS.USERNAME = ORADBA.SGC_USUARIOS_LOGADOS.USUARIO ";
                            _Consulta = _Consulta + "WHERE 1 = 1 ";
                        }
                        else
                        {
                            _Consulta = "";
                            _Consulta = _Consulta + "SELECT ALL_USERS.USERNAME, ";
                            _Consulta = _Consulta + "    ALL_USERS.CREATED, ";
                            _Consulta = _Consulta + "    ALL_USERS.PROFILE, ";
                            _Consulta = _Consulta + "    ALL_USERS.ACCOUNT_STATUS, ";
                            _Consulta = _Consulta + "    ALL_USERS.LOCK_DATE, ";
                            _Consulta = _Consulta + "    ALL_USERS.EXPIRY_DATE, ";
                            _Consulta = _Consulta + "    NULL AS ULTIMO_LOGIN ";
                            _Consulta = _Consulta + "FROM ALL_USERS ";
                            _Consulta = _Consulta + "WHERE 1 = 1 ";
                        }
                        break;
                }

                if (p_Where.Trim().Length > 0)
                {
                    _Consulta = _Consulta + " AND " + p_Where;
                }
                else
                {
                    if ((p_Profile.Trim().Length > 0) && (p_Profile.Trim().ToUpper() != "TODOS"))
                    {
                        _Consulta = _Consulta + " AND ALL_USERS.PROFILE = '" + p_Profile.Trim().ToUpper() + "' ";
                    }
                    if (p_NomeUsuario.Trim().Length > 0)
                    {
                        _Consulta = _Consulta + " AND ALL_USERS.USERNAME like '%" + p_NomeUsuario.Trim().ToUpper() + "%' ";
                    }
                }
                
                _Consulta = _Consulta + " ORDER BY ALL_USERS.USERNAME";

                _Command.CommandText = _Consulta;
                _Command.CommandType = CommandType.Text;
                _DataReader = _Command.ExecuteReader();
                _ListaUsuarios = new ArrayList();
                while (_DataReader.Read())
                {
                    _csUsuario = new csUsuarioBanco();
                    _csUsuario.Nome = _DataReader.GetString(0);
                    _csUsuario.DataCriacao = _DataReader.GetDateTime(1);
                    _csUsuario.Profile = _DataReader.GetString(2);
                    _csUsuario.Status = _DataReader.GetString(3);
                    if (!_DataReader.IsDBNull(4))
                    {
                        _csUsuario.DataLock = _DataReader.GetDateTime(4);
                    }
                    if (!_DataReader.IsDBNull(5))
                    {
                        _csUsuario.DataExpiracao = _DataReader.GetDateTime(5);
                    }
                    if (!_DataReader.IsDBNull(6))
                    {
                        _csUsuario.UltimoLogin = _DataReader.GetDateTime(6);
                    }
                    _ListaUsuarios.Add(_csUsuario);
                }

                _DataReader.Close();
                _DataReader.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _ListaUsuarios;
            }
            catch (Exception _Exception)
            {
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaUsuarios(p_Usuario, p_Senha, p_Database, p_Owner, p_Profile, p_NomeUsuario, p_Where);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaUsuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public string LocalizaUsuario(string p_Usuario, string p_Senha, string p_Database, string p_FiltroPesquisa)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            string _Retorno = "";
            string _Query = "";

            // Abrindo conexão com o banco
            OleDbCommand _Command = new OleDbCommand();
            OleDbConnection _Connection = new OleDbConnection();
            OleDbDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "Provider=OraOLEDB.Oracle; User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database + ";ChunkSize=65535;"; // 65535 é o máximo. se colocar um parâmetro maior que isso vão vir menos caracteres.
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                _Query = "";
                _Query = _Query + "SELECT USERNAME, ";
                _Query = _Query + "    ACCOUNT_STATUS, ";
                _Query = _Query + "    CREATED, ";
                _Query = _Query + "    PROFILE ";
                _Query = _Query + "FROM ALL_USERS ";
                _Query = _Query + "WHERE UPPER(USERNAME) LIKE UPPER('" + p_FiltroPesquisa + "') ";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();

                _Retorno = _Retorno + "Procurando em ALL_USERS:\r\n\r\n";

                if (_DataReader.Read())
                {
                    _Retorno = _Retorno + "Nome: " + _DataReader.GetString(0) + " - Status: " + _DataReader.GetString(1) + " - Criado em: " + _DataReader.GetDateTime(2).ToString("dd/MM/yy HH:mm:ss") + " - Profile: " + _DataReader.GetString(3) + "\r\n";
                    while (_DataReader.Read())
                    {
                        _Retorno = _Retorno + "Nome: " + _DataReader.GetString(0) + " - Status: " + _DataReader.GetString(1) + " - Criado em: " + _DataReader.GetDateTime(2).ToString("dd/MM/yy HH:mm:ss") + " - Profile: " + _DataReader.GetString(3) + "\r\n";
                    }
                }
                else
                {
                    _Retorno = _Retorno + "Nenhum usuário com nome " + p_FiltroPesquisa + " encontrado\r\n\r\n";
                }

                _DataReader.Close();

                _Query = "";
                _Query = _Query + "SELECT NOM_USR, ";
                _Query = _Query + "    DESC_USR ";
                _Query = _Query + "FROM AESPROD.USUARIOS ";
                _Query = _Query + "WHERE UPPER(NOM_USR) LIKE UPPER('" + p_FiltroPesquisa + "') ";
                _Query = _Query + "    OR UPPER(DESC_USR) LIKE UPPER('" + p_FiltroPesquisa + "') ";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();

                _Retorno = _Retorno + "\r\nProcurando em AESPROD.USUARIOS:\r\n\r\n";

                if (_DataReader.Read())
                {
                    _Retorno = _Retorno + _DataReader.GetString(0) + " - " + _DataReader.GetString(1) + "\r\n";
                    while (_DataReader.Read())
                    {
                        _Retorno = _Retorno + _DataReader.GetString(0) + " - " + _DataReader.GetString(1) + "\r\n";
                    }
                }
                else
                {
                    _Retorno = _Retorno + "Nenhum usuário com nome " + p_FiltroPesquisa + " encontrado\r\n\r\n";
                }

                _DataReader.Close();

            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.LocalizaUsuario(p_Usuario, p_Senha, p_Database, p_FiltroPesquisa);
                }
                else if (_Exception.Message.IndexOf("'OraOLEDB.Oracle'") > -1)
                {
                    MessageBox.Show("ERRO\nO provider OraOLEDB.Oracle não está registrado corretamente\nLocalize a DLL OraOLEDB11.dll ou versão equivalente na pasta de instalação do ORACLE CLIENT e registre como no exemplo abaixo\nregsvr32.exe C:\\oracle\\product\\11.2.0\\client_1\\bin\\OraOLEDB11.dll", "ListaPropUser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaPropUser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            if (!_DataReader.IsClosed)
            {
                _DataReader.Close();
            }
            _DataReader.Dispose();
            _Command.Dispose();
            _Connection.Close();
            _Connection.Dispose();
            GC.Collect();

            return _Retorno;
        }

        public ArrayList ListaPropUser(string p_Usuario, string p_Senha, string p_Database, string p_NomeUser, ref string p_Mensagem)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            // Abrindo conexão com o banco
            OleDbCommand _Command = new OleDbCommand();
            OleDbConnection _Connection = new OleDbConnection();
            OleDbDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "Provider=OraOLEDB.Oracle; User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database + ";ChunkSize=65535;"; // 65535 é o máximo. se colocar um parâmetro maior que isso vão vir menos caracteres.
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                //Verificando se existe a tabela de AESPROD.USUARIOS
                _Query = "";
                _Query = _Query + "SELECT 1 ";
                _Query = _Query + "FROM ALL_OBJECTS ";
                _Query = _Query + "WHERE OWNER = 'AESPROD' ";
                _Query = _Query + "    AND OBJECT_NAME = 'USUARIOS' ";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                if (_DataReader.Read())
                {
                    _DataReader.Close();

                    //Buscando nome do usuário da tabela AESPROD.USUARIOS
                    _Query = "";
                    _Query = _Query + "SELECT DESC_USR ";
                    _Query = _Query + "FROM AESPROD.USUARIOS ";
                    _Query = _Query + "WHERE NOM_USR = '" + p_NomeUser.ToUpper() + "'";
                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();
                    if (_DataReader.Read())
                    {
                        if (!_DataReader.IsDBNull(0))
                        {
                            _Retorno.Add("NOME: " + _DataReader.GetString(0).ToUpper());
                        }
                    }
                    _DataReader.Close();
                }

                //Verificando se existe a tabela de ORADBA.SGC_USUARIOS_LOGADOS
                _Query = "";
                _Query = _Query + "SELECT 1 ";
                _Query = _Query + "FROM ALL_OBJECTS ";
                _Query = _Query + "WHERE OWNER = 'ORADBA' ";
                _Query = _Query + "    AND OBJECT_NAME = 'SGC_USUARIOS_LOGADOS' ";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                if (_DataReader.Read())
                {
                    _DataReader.Close();
                    // Verificando o usuário existe e já buscando algumas informações
                    _Query = "";
                    _Query = _Query + "SELECT ALL_USERS.ACCOUNT_STATUS, "; // 0 
                    _Query = _Query + "    ALL_USERS.DEFAULT_TABLESPACE, "; // 1
                    _Query = _Query + "    ALL_USERS.TEMPORARY_TABLESPACE, "; // 2
                    _Query = _Query + "    ALL_USERS.PROFILE, "; // 3
                    _Query = _Query + "    ALL_USERS.CREATED, "; // 4
                    _Query = _Query + "    ORADBA.SGC_USUARIOS_LOGADOS.DATA AS ULTIMO_LOGIN "; // 5
                    _Query = _Query + "FROM ALL_USERS ";
                    _Query = _Query + "    LEFT JOIN ORADBA.SGC_USUARIOS_LOGADOS ";
                    _Query = _Query + "    ON ALL_USERS.USERNAME = ORADBA.SGC_USUARIOS_LOGADOS.USUARIO ";
                    _Query = _Query + "WHERE USERNAME = '" + p_NomeUser.ToUpper() + "'";
                }
                else
                {
                    _DataReader.Close();
                    // Verificando o usuário existe e já buscando algumas informações
                    _Query = "";
                    _Query = _Query + "SELECT ALL_USERS.ACCOUNT_STATUS, "; // 0 
                    _Query = _Query + "    ALL_USERS.DEFAULT_TABLESPACE, "; // 1
                    _Query = _Query + "    ALL_USERS.TEMPORARY_TABLESPACE, "; // 2
                    _Query = _Query + "    ALL_USERS.PROFILE, "; // 3
                    _Query = _Query + "    ALL_USERS.CREATED, "; // 4
                    _Query = _Query + "    NULL AS ULTIMO_LOGIN "; // 5
                    _Query = _Query + "FROM ALL_USERS ";
                    _Query = _Query + "WHERE USERNAME = '" + p_NomeUser.ToUpper() + "'";
                }

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                if (_DataReader.Read())
                {
                    _Retorno.Add("STATUS: " + _DataReader.GetString(0));
                    _Retorno.Add("DEFAULT TABLESPACE: " + _DataReader.GetString(1));
                    _Retorno.Add("TEMPORARY TABLESPACE " + _DataReader.GetString(2));
                    _Retorno.Add("PROFILE: " + _DataReader.GetString(3));
                    _Retorno.Add("CRIADO EM: " + _DataReader.GetDateTime(4).ToString("dd/MM/yy HH:mm:ss"));
                    if (_DataReader.IsDBNull(5))
                    {
                        _Retorno.Add("ÚLTIMO LOGIN: Sem registro");
                    }
                    else
                    {
                        _Retorno.Add("ÚLTIMO LOGIN: " + _DataReader.GetDateTime(5).ToString("dd/MM/yy HH:mm:ss"));
                    }

                }
                else
                {
                    p_Mensagem = "Usuário " + p_NomeUser + " não encontrado";
                    _DataReader.Close();
                    _DataReader.Dispose();
                    _Command.Dispose();
                    _Connection.Close();
                    _Connection.Dispose();
                    GC.Collect();
                    return null;
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaPropUser(p_Usuario, p_Senha, p_Database, p_NomeUser, ref p_Mensagem);
                }
                else if (_Exception.Message.IndexOf("'OraOLEDB.Oracle'") > -1)
                {
                    MessageBox.Show("ERRO\nO provider OraOLEDB.Oracle não está registrado corretamente\nLocalize a DLL OraOLEDB11.dll ou versão equivalente na pasta de instalação do ORACLE CLIENT e registre como no exemplo abaixo\nregsvr32.exe C:\\oracle\\product\\11.2.0\\client_1\\bin\\OraOLEDB11.dll", "ListaPropUser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaPropUser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public DateTime DataCriacaoRole(string p_Usuario, string p_Senha, string p_Database, string p_NomeRole, ref string p_Mensagem)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            DateTime _Retorno = DateTime.MinValue;
            string _Query = "";

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                //Verificando se existe a role e quando foi criada
                _Query = "";
                _Query = _Query + "SELECT DBAROLE.ROLE AS NOME_ROLE, ";
                _Query = _Query + "    SYSUSER.CTIME AS DATA_CRIACAO ";
                _Query = _Query + "FROM DBA_ROLES DBAROLE ";
                _Query = _Query + "    INNER JOIN SYS.USER$ SYSUSER ";
                _Query = _Query + "    ON DBAROLE.ROLE = SYSUSER.NAME ";
                _Query = _Query + "WHERE DBAROLE.ROLE = '" + p_NomeRole.Trim().ToUpper() + "' ";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                if (_DataReader.Read())
                {
                    _Retorno = _DataReader.GetDateTime(1);
                }
                else
                {
                    p_Mensagem = "Role " + p_NomeRole + " não encontrada";
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.DataCriacaoRole(p_Usuario, p_Senha, p_Database, p_NomeRole, ref p_Mensagem);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "DataCriacaoRole", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return DateTime.MinValue;
                }
            }
        }

        public ArrayList ListaCotasTablespaceUser(string p_Usuario, string p_Senha, string p_Database, string p_NomeUser)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Verificando as cotas nos tablespaces
                _Query = "";
                _Query = _Query + "SELECT TABLESPACE_NAME, "; // 0
                _Query = _Query + "    MAX_BYTES "; // 1
                _Query = _Query + "FROM DBA_TS_QUOTAS ";
                _Query = _Query + "WHERE DROPPED = 'NO'";
                _Query = _Query + "    AND USERNAME = '" + p_NomeUser.ToUpper() + "'";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                while (_DataReader.Read())
                {

                    if (_DataReader.GetDecimal(1) > -1)
                    {
                        _Retorno.Add(_DataReader.GetString(0) + ": " + _DataReader.GetDecimal(1).ToString());
                    }
                    else
                    {
                        _Retorno.Add(_DataReader.GetString(0) + ": UNLIMITED");
                    }
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaCotasTablespaceUser(p_Usuario, p_Senha, p_Database, p_NomeUser);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaCotasTablespaceUser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public ArrayList ListaPrivUser(string p_Usuario, string p_Senha, string p_Database, string p_NomeUser)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Verificando privilégios do sistema
                _Query = "";
                _Query = _Query + "SELECT PRIVILEGE, "; // 0
                _Query = _Query + "    ADMIN_OPTION "; // 1
                _Query = _Query + "FROM DBA_SYS_PRIVS ";
                _Query = _Query + "WHERE GRANTEE = '" + p_NomeUser.ToUpper() + "'";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                while (_DataReader.Read())
                {
                    if (_DataReader.GetString(1) == "YES")
                    {
                        _Retorno.Add(_DataReader.GetString(0) + " WITH ADMIN OPTION");
                    }
                    else
                    {
                        _Retorno.Add(_DataReader.GetString(0));
                    }

                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaPrivUser(p_Usuario, p_Senha, p_Database, p_NomeUser);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaPrivUser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            
        }

        public ArrayList ListaRolesUser(string p_Usuario, string p_Senha, string p_Database, string p_NomeUser)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Verificando roles
                _Query = "";
                _Query = _Query + "SELECT GRANTED_ROLE, "; // 0
                _Query = _Query + "    ADMIN_OPTION, "; // 1
                _Query = _Query + "    DEFAULT_ROLE "; // 2
                _Query = _Query + "FROM DBA_ROLE_PRIVS ";
                _Query = _Query + "WHERE GRANTEE = '" + p_NomeUser.ToUpper() + "' ";
                _Query = _Query + "ORDER BY GRANTED_ROLE ";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                while (_DataReader.Read())
                {
                    if (_DataReader.GetString(1) == "YES")
                    {
                        if (_DataReader.GetString(2) == "YES")
                        {
                            _Retorno.Add(_DataReader.GetString(0) + ": DEFAULT COM ADMIN OPTION");
                        }
                        else
                        {
                            _Retorno.Add(_DataReader.GetString(0) + ": NÃO DEFAULT COM ADMIN OPTION");
                        }
                    }
                    else
                    {
                        if (_DataReader.GetString(2) == "YES")
                        {
                            _Retorno.Add(_DataReader.GetString(0) + ": DEFAULT SEM ADMIN OPTION");
                        }
                        else
                        {
                            _Retorno.Add(_DataReader.GetString(0) + ": NÃO DEFAULT SEM ADMIN OPTION");
                        }
                    }
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaRolesUser(p_Usuario, p_Senha, p_Database, p_NomeUser);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaRolesUser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public ArrayList ListaNoRolesGrantsUser(string p_Usuario, string p_Senha, string p_Database, string p_NomeUser)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Verificando grants de tabelas e similares setados direto ao usuário
                _Query = "";
                _Query = _Query + "SELECT PRIVILEGE, "; // 0
                _Query = _Query + "    OWNER, "; // 1
                _Query = _Query + "    TABLE_NAME, "; // 2
                _Query = _Query + "    GRANTABLE "; // 3
                _Query = _Query + "FROM ALL_TAB_PRIVS ";
                _Query = _Query + "WHERE GRANTEE = '" + p_NomeUser.ToUpper() + "'";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();

                while (_DataReader.Read())
                {
                    // -- GRANT <privilege> ON <owner>.<table_name> TO <usuário>
                    // -- Se grantable = YES + WITH GRANT OPTION

                    if (_DataReader.GetString(0) == "READ" || _DataReader.GetString(0) == "WRITE")
                    {
                        if (_DataReader.GetString(3) == "YES")
                        {
                            _Retorno.Add(_DataReader.GetString(0) + " DIRECTORY " + _DataReader.GetString(1) + "." + _DataReader.GetString(2) + " COM ADMIN OPTION");
                        }
                        else
                        {
                            _Retorno.Add(_DataReader.GetString(0) + " DIRECTORY " + _DataReader.GetString(1) + "." + _DataReader.GetString(2));
                        }
                    }
                    else
                    {
                        if (_DataReader.GetString(3) == "YES")
                        {
                            _Retorno.Add(_DataReader.GetString(0) + " " + _DataReader.GetString(1) + "." + _DataReader.GetString(2) + " COM ADMIN OPTION");
                        }
                        else
                        {
                            _Retorno.Add(_DataReader.GetString(0) + " " + _DataReader.GetString(1) + "." + _DataReader.GetString(2));
                        }
                    }

                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaNoRolesGrantsUser(p_Usuario, p_Senha, p_Database, p_NomeUser);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaNoRolesGrantsUser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public ArrayList ListaPrivSysRole(string p_Usuario, string p_Senha, string p_Database, string p_NomeRole)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Verificando privilégios de sistema da role
                _Query = "";
                _Query = _Query + "SELECT PRIVILEGE, "; // 0
                _Query = _Query + "    ADMIN_OPTION "; // 1
                _Query = _Query + "FROM DBA_SYS_PRIVS ";
                _Query = _Query + "WHERE GRANTEE = '" + p_NomeRole + "' ";
                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                while (_DataReader.Read())
                {
                    _Retorno.Add(_DataReader.GetString(0));
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaPrivSysRole(p_Usuario, p_Senha, p_Database, p_NomeRole);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaPrivSysRole", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public ArrayList ListaTiposObjRole(string p_Usuario, string p_Senha, string p_Database, string p_NomeRole)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Verificando roles
                _Query = "";
                _Query = _Query + "SELECT DISTINCT OBJ.OBJECT_TYPE ";
                _Query = _Query + "FROM ALL_TAB_PRIVS TAB ";
                _Query = _Query + "    INNER JOIN ALL_OBJECTS OBJ ";
                _Query = _Query + "        ON TAB.OWNER = OBJ.OWNER ";
                _Query = _Query + "        AND TAB.TABLE_NAME = OBJ.OBJECT_NAME ";
                _Query = _Query + "WHERE TAB.GRANTEE = '" + p_NomeRole + "' ";
                _Query = _Query + "    AND OBJ.OBJECT_TYPE NOT IN ('TABLE PARTITION', 'TABLE SUBPARTITION', 'PACKAGE BODY', 'INDEX', 'TYPE BODY') ";
                _Query = _Query + "ORDER BY OBJ.OBJECT_TYPE ";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                while (_DataReader.Read())
                {
                    _Retorno.Add(_DataReader.GetString(0));
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaTiposObjRole(p_Usuario, p_Senha, p_Database, p_NomeRole);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaTiposObjRole", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public ArrayList ListaPrivRoleObj(string p_Usuario, string p_Senha, string p_Database, string p_NomeRole, string p_TipoObj)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";
            string _NomeObj = "";
            string _ListaPriv = "";

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Verificando roles
                _Query = "";
                _Query = _Query + "Select DISTINCT TAB.OWNER, "; // 0 
                _Query = _Query + "    TAB.TABLE_NAME, "; // 1
                _Query = _Query + "    TAB.PRIVILEGE "; // 2
                _Query = _Query + "From ALL_TAB_PRIVS TAB ";
                _Query = _Query + "    INNER JOIN ALL_OBJECTS OBJ ";
                _Query = _Query + "        ON TAB.OWNER = OBJ.OWNER ";
                _Query = _Query + "        AND TAB.TABLE_NAME = OBJ.OBJECT_NAME ";
                _Query = _Query + "Where TAB.GRANTEE = '" + p_NomeRole + "' ";
                _Query = _Query + "    AND OBJ.OBJECT_TYPE = '" + p_TipoObj + "' ";
                _Query = _Query + "ORDER BY TAB.OWNER, TAB.TABLE_NAME, TAB.PRIVILEGE ";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                while (_DataReader.Read())
                {
                    if (_NomeObj.Length == 0)
                    {
                        _NomeObj = _DataReader.GetString(0) + "." + _DataReader.GetString(1);
                        _ListaPriv = _DataReader.GetString(2);
                    }
                    else
                    {
                        if (_NomeObj == (_DataReader.GetString(0) + "." + _DataReader.GetString(1)))
                        {
                            _ListaPriv = _ListaPriv + ", " + _DataReader.GetString(2);
                        }
                        else
                        {
                            _Retorno.Add(_ListaPriv + " ON " + _NomeObj);
                            _NomeObj = _DataReader.GetString(0) + "." + _DataReader.GetString(1);
                            _ListaPriv = _DataReader.GetString(2);
                        }
                    }
                }
                if (_NomeObj.Length != 0)
                {
                    _Retorno.Add(_ListaPriv + " ON " + _NomeObj);
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaPrivRoleObj(p_Usuario, p_Senha, p_Database, p_NomeRole, p_TipoObj);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaPrivRoleObj", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public ArrayList ListaUsuariosRole(string p_Usuario, string p_Senha, string p_Database, string p_NomeRole)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            // Abrindo conexão com o banco
            OleDbCommand _Command = new OleDbCommand();
            OleDbConnection _Connection = new OleDbConnection();
            OleDbDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "Provider=OraOLEDB.Oracle; User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database + ";ChunkSize=65535;"; // 65535 é o máximo. se colocar um parâmetro maior que isso vão vir menos caracteres.
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                //Verificando se existe a tabela de AESPROD.USUARIOS
                _Query = "";
                _Query = _Query + "SELECT 1 ";
                _Query = _Query + "FROM ALL_OBJECTS ";
                _Query = _Query + "WHERE OWNER = 'AESPROD' ";
                _Query = _Query + "    AND OBJECT_NAME = 'USUARIOS' ";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                if (_DataReader.Read())
                {
                    _DataReader.Close();

                    // Existe a tabela AESPROD.USUARIOS
                    _Query = "";
                    _Query = _Query + "SELECT ROLEPRIVS.GRANTEE, "; // 0
                    _Query = _Query + "    ROLEPRIVS.DEFAULT_ROLE, "; // 1
                    _Query = _Query + "    AESUSUARIO.DESC_USR "; // 2
                    _Query = _Query + "FROM DBA_ROLE_PRIVS ROLEPRIVS ";
                    _Query = _Query + "    LEFT JOIN AESPROD.USUARIOS AESUSUARIO ";
                    _Query = _Query + "    ON ROLEPRIVS.GRANTEE = AESUSUARIO.NOM_USR ";
                    _Query = _Query + "WHERE ROLEPRIVS.GRANTED_ROLE = '" + p_NomeRole.Trim().ToUpper() + "' ";
                    _Query = _Query + "ORDER BY UPPER(AESUSUARIO.DESC_USR) ";

                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();
                    while (_DataReader.Read())
                    {
                        if (_DataReader.IsDBNull(2))
                        {
                            _Retorno.Add(_DataReader.GetString(0) + ": Default = " + _DataReader.GetString(1));
                        }
                        else
                        {
                            _Retorno.Add(_DataReader.GetString(0) + ": " + _DataReader.GetString(2).ToUpper() + " - Default = " + _DataReader.GetString(1));
                        }
                    }

                }
                else
                {
                    _DataReader.Close();

                    // Não existe a tabela AESPROD.USUARIOS

                    _Query = "";
                    _Query = _Query + "SELECT GRANTEE, ";
                    _Query = _Query + "    DEFAULT_ROLE ";
                    _Query = _Query + "FROM DBA_ROLE_PRIVS ";
                    _Query = _Query + "WHERE GRANTED_ROLE = '" + p_NomeRole.Trim().ToUpper() + "' ";
                    _Query = _Query + "ORDER BY GRANTEE ";

                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();
                    while (_DataReader.Read())
                    {
                        _Retorno.Add(_DataReader.GetString(0) + ": Default = " + _DataReader.GetString(1));
                    }
                }

                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaUsuariosRole(p_Usuario, p_Senha, p_Database, p_NomeRole);
                }
                else if (_Exception.Message.IndexOf("'OraOLEDB.Oracle'") > -1)
                {
                    MessageBox.Show("ERRO\nO provider OraOLEDB.Oracle não está registrado corretamente\nLocalize a DLL OraOLEDB11.dll ou versão equivalente na pasta de instalação do ORACLE CLIENT e registre como no exemplo abaixo\nregsvr32.exe C:\\oracle\\product\\11.2.0\\client_1\\bin\\OraOLEDB11.dll", "ListaUsuariosRole", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaUsuariosRole", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public ArrayList ListaDetalheProfile(string p_Usuario, string p_Senha, string p_Database, string p_NomeProfile)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            ArrayList _Retorno = new ArrayList();
            string _Query = "";

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Verificando detalhes do resource
                _Query = "";
                _Query = _Query + "SELECT RESOURCE_TYPE, "; // 0
                _Query = _Query + "    RESOURCE_NAME, "; // 1
                _Query = _Query + "    LIMIT "; // 2
                _Query = _Query + "FROM DBA_PROFILES ";
                _Query = _Query + "WHERE PROFILE = '" + p_NomeProfile + "' ";
                _Query = _Query + "ORDER BY RESOURCE_TYPE, RESOURCE_NAME ";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                while (_DataReader.Read())
                {
                    _Retorno.Add(_DataReader.GetString(0) + '.' + _DataReader.GetString(1) + " = " + _DataReader.GetString(2));
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ListaDetalheProfile(p_Usuario, p_Senha, p_Database, p_NomeProfile);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ListaDetalheProfile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        #endregion Listagens Usuários

        #region Extract DDL

        public string ExtractDDLUser(string p_Usuario, string p_Senha, string p_Database, string p_NomeUser, ref string p_Mensagem)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            string _Retorno = "";
            string _Query = "";
            bool _DefaultRole = true;
            string _ListaDefaultRole = "";
            bool _PrimeiraPassada = true;
            string _NomeUsuario = ""; // Caso tenha espaço no nome do usuário, coloco nesta variável entre aspas só uma vez

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Verificando o usuário existe e já buscando algumas informações
                _Query = "";
                _Query = _Query + "SELECT ACCOUNT_STATUS, "; // 0 -- IF <> 'OPEN' THEN ACCOUNT LOCK
                _Query = _Query + "    DEFAULT_TABLESPACE, "; // 1 -- DEFAULT TABLESPACE <DEFAULT_TABLESPACE>
                _Query = _Query + "    TEMPORARY_TABLESPACE, "; // 2 -- TEMPORARY TABLESPACE <TEMPORARY_TABLESPACE>
                _Query = _Query + "    PROFILE "; // 3 -- PROFILE <PROFILE>
                _Query = _Query + "FROM ALL_USERS ";
                _Query = _Query + "WHERE USERNAME = '" + p_NomeUser.ToUpper() + "'";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                if (_DataReader.Read())
                {
                    if (p_NomeUser.IndexOf(" ") > 0)
                    {
                        _NomeUsuario = "\"" + p_NomeUser.ToUpper() + "\"";
                    }
                    else
                    {
                        _NomeUsuario = p_NomeUser.ToUpper();
                    }
                    _Retorno = _Retorno + "CREATE USER " + _NomeUsuario + "\n";
                    _Retorno = _Retorno + "IDENTIFIED BY <SENHA_USUARIO>\n";
                    if (!_DataReader.IsDBNull(1))
                    {
                        _Retorno = _Retorno + "DEFAULT TABLESPACE " + _DataReader.GetString(1) + "\n";
                    }
                    if (!_DataReader.IsDBNull(2))
                    {
                        _Retorno = _Retorno + "TEMPORARY TABLESPACE " + _DataReader.GetString(2) + "\n";
                    }
                    _Retorno = _Retorno + "PROFILE " + _DataReader.GetString(3) + "\n";
                }
                else
                {
                    p_Mensagem = "Usuário " + p_NomeUser + " não encontrado";
                    _DataReader.Close();
                    _DataReader.Dispose();
                    _Command.Dispose();
                    _Connection.Close();
                    _Connection.Dispose();
                    GC.Collect();
                    return "";
                }
                _DataReader.Close();

                // Verificando as cotas nos tablespaces
                _Query = "";
                _Query = _Query + "SELECT TABLESPACE_NAME, "; // 0
                _Query = _Query + "    MAX_BYTES "; // 1
                _Query = _Query + "FROM DBA_TS_QUOTAS ";
                _Query = _Query + "WHERE DROPPED = 'NO'";
                _Query = _Query + "    AND USERNAME = '" + p_NomeUser.ToUpper() + "'";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                while (_DataReader.Read())
                {
                    if (_DataReader.GetDecimal(1) > -1)
                    {
                        _Retorno = _Retorno + "QUOTA " + _DataReader.GetDecimal(1).ToString() + " ON " + _DataReader.GetString(0) + "\n";
                    }
                    else
                    {
                        _Retorno = _Retorno + "QUOTA UNLIMITED ON " + _DataReader.GetString(0) + "\n";
                    }
                }
                _DataReader.Close();
                _Retorno = _Retorno + "/\n";

                // Verificando privilégios do sistema
                _Query = "";
                _Query = _Query + "SELECT PRIVILEGE, "; // 0
                _Query = _Query + "    ADMIN_OPTION "; // 1
                _Query = _Query + "FROM DBA_SYS_PRIVS ";
                _Query = _Query + "WHERE GRANTEE = '" + p_NomeUser.ToUpper() + "'";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                while (_DataReader.Read())
                {
                    _Retorno = _Retorno + "GRANT " + _DataReader.GetString(0) + " TO " + _NomeUsuario;
                    if (_DataReader.GetString(1) == "YES")
                    {
                        _Retorno = _Retorno + " WITH ADMIN OPTION\n/\n";
                    }
                    else
                    {
                        _Retorno = _Retorno + "\n/\n";
                    }

                }
                _DataReader.Close();

                // Verificando roles
                _Query = "";
                _Query = _Query + "SELECT GRANTED_ROLE, "; // 0
                _Query = _Query + "    ADMIN_OPTION, "; // 1
                _Query = _Query + "    DEFAULT_ROLE "; // 2
                _Query = _Query + "FROM DBA_ROLE_PRIVS ";
                _Query = _Query + "WHERE GRANTEE = '" + p_NomeUser.ToUpper() + "'";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                _DefaultRole = true;
                while (_DataReader.Read())
                {
                    _Retorno = _Retorno + "GRANT " + _DataReader.GetString(0) + " TO " + _NomeUsuario;
                    if (_DataReader.GetString(1) == "YES")
                    {
                        _Retorno = _Retorno + " WITH ADMIN OPTION\n/\n";
                    }
                    else
                    {
                        _Retorno = _Retorno + "\n/\n";
                    }
                    if (_DataReader.GetString(2) == "NO")
                    {
                        _DefaultRole = false;
                    }

                }
                _DataReader.Close();

                if (!_DefaultRole)
                {
                    // Tem pelo menos uma role que não é default... Tem que tirar o default de todas e setar default só para as que são default
                    _Retorno = _Retorno + "ALTER USER " + _NomeUsuario + " DEFAULT ROLE NONE\n/\n";

                    // Verificando roles que são DEFAULT_ROLE = YES para setar individualmente
                    _Query = "";
                    _Query = _Query + "SELECT GRANTED_ROLE "; // 0
                    _Query = _Query + "FROM DBA_ROLE_PRIVS ";
                    _Query = _Query + "WHERE DEFAULT_ROLE = 'YES' ";
                    _Query = _Query + "    AND GRANTEE = '" + p_NomeUser.ToUpper() + "'";

                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();
                    _ListaDefaultRole = "";
                    while (_DataReader.Read())
                    {
                        if (_ListaDefaultRole.Length > 0)
                        {
                            _ListaDefaultRole = _ListaDefaultRole + ", ";
                        }
                        _ListaDefaultRole = _ListaDefaultRole + _DataReader.GetString(0);
                    }
                    _DataReader.Close();

                    if (_ListaDefaultRole.Length > 0)
                    {
                        _Retorno = _Retorno + "ALTER USER " + _NomeUsuario + " DEFAULT ROLE " + _ListaDefaultRole + "\n/\n";
                    }
                }

                // Verificando grants de tabelas e similares setados direto ao usuário
                _Query = "";
                _Query = _Query + "SELECT PRIVILEGE, "; // 0
                _Query = _Query + "    OWNER, "; // 1
                _Query = _Query + "    TABLE_NAME, "; // 2
                _Query = _Query + "    GRANTABLE "; // 3
                _Query = _Query + "FROM ALL_TAB_PRIVS ";
                _Query = _Query + "WHERE GRANTEE = '" + p_NomeUser.ToUpper() + "'";

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();

                _PrimeiraPassada = true;
                while (_DataReader.Read())
                {
                    if (_PrimeiraPassada)
                    {
                        _Retorno = _Retorno + "\n--=======================================================================================================================\n";
                        _Retorno = _Retorno + "\n-- DAQUI PARA BAIXO SÃO GRANTS DE TABELAS OU SIMILARES DEFINIDOS DIRETO AO USUÁRIO AO INVÉS DE SEREM DEFINIDOS PARA ROLES\n";
                        _Retorno = _Retorno + "\n--=======================================================================================================================\n";
                        _PrimeiraPassada = false;
                    }

                    // -- GRANT <privilege> ON <owner>.<table_name> TO <usuário>
                    // -- Se grantable = YES + WITH GRANT OPTION
                    _Retorno = _Retorno + "GRANT " + _DataReader.GetString(0) + " ON ";

                    if (_DataReader.GetString(0) == "READ" || _DataReader.GetString(0) == "WRITE")
                    {
                        _Retorno = _Retorno + "DIRECTORY ";
                    }

                    _Retorno = _Retorno + _DataReader.GetString(1) + "." + _DataReader.GetString(2) + " TO " + _NomeUsuario;

                    if (_DataReader.GetString(3) == "YES")
                    {
                        _Retorno = _Retorno + " WITH ADMIN OPTION\n/\n";
                    }
                    else
                    {
                        _Retorno = _Retorno + "\n/\n";
                    }

                }
                _DataReader.Close();

                // Libera tudo
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ExtractDDLUser(p_Usuario, p_Senha, p_Database, p_NomeUser, ref p_Mensagem);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ExtractDDLUser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public void ExtractDDLClipboard(string p_Usuario, string p_Senha, string p_Database, string p_NomeObj)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            string _TipoObjeto = "";
            string _Mensagem = "";

            Cursor.Current = Cursors.WaitCursor;
            string _Fonte = this.ExtractDDL(p_Usuario, p_Senha, p_Database, p_NomeObj, ref _TipoObjeto, ref _Mensagem);
            Cursor.Current = Cursors.Default;

            if (_Mensagem.Trim().Length > 0)
            {
                MessageBox.Show(_Mensagem, "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (_Fonte.Trim().Length > 0)
            {
                Clipboard.SetData(DataFormats.Text, (object)_Fonte);
                MessageBox.Show("DDL do objeto " + p_NomeObj + " na área de transferência", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void ExtractDDLTextEditor(string p_Usuario, string p_Senha, string p_Database, string p_NomeObj)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            string _TipoObjeto = "";
            string _Mensagem = "";

            Cursor.Current = Cursors.WaitCursor;
            string _Fonte = this.ExtractDDL(p_Usuario, p_Senha, p_Database, p_NomeObj, ref _TipoObjeto, ref _Mensagem);
            Cursor.Current = Cursors.Default;

            if (_Mensagem.Trim().Length > 0)
            {
                MessageBox.Show(_Mensagem, "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (_Fonte.Trim().Length > 0)
            {
                csUtil.SalvarEAbrir(_Fonte, p_NomeObj + ".sql");
            }
        }

        public string ExtractDDL(string p_Usuario, string p_Senha, string p_Database, string p_NomeObj, ref string p_TipoObjeto, ref string p_Mensagem)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            string _Retorno = "";
            string _Query = "";
            string _OwnerObj = "";
            string _NomeObj = "";
            string[] _Split;
            DateTime _DataCriacaoObjeto = DateTime.MinValue;
            DateTime _DataUltimaAlteracaoObjeto = DateTime.MinValue;
            string _StatusObjeto = "";
            string _Cabecalho = "";
            bool _ProcurarUsuarioUltimaAlteracao = false;
            string _TextoUsuarioUltimaAlteracao = "";

            p_Mensagem = "";
            p_TipoObjeto = "";

            _Split = p_NomeObj.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (_Split.Length != 2)
            {
                p_Mensagem = "Nome de objeto inválido\n" + p_NomeObj;
                return "";
            }

            _OwnerObj = _Split[0].Trim().ToUpper();
            _NomeObj = _Split[1].Trim().ToUpper();

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                if (_TemTabContrConc == -1) // -1 = Indefinido
                {
                    _Query = "";
                    _Query = _Query + "SELECT COUNT(*) ";
                    _Query = _Query + "FROM ALL_OBJECTS ";
                    _Query = _Query + "WHERE OWNER = 'AESPROD' ";
                    _Query = _Query + "    AND OBJECT_NAME = 'AUDITORIA_OBJETOS' ";

                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();
                    if (_DataReader.Read())
                    {
                        if (_DataReader.GetDecimal(0) > 0)
                        {
                            _TemTabContrConc = 1; // Diferente de zero = Tem
                            _ProcurarUsuarioUltimaAlteracao = true;
                        }
                        else
                        {
                            _TemTabContrConc = 0; // zero = Não tem
                        }
                    }
                    else
                    {
                        _TemTabContrConc = 0; // zero = Não tem
                    }
                    _DataReader.Close();

                }
                else if (_TemTabContrConc != 0) // Diferente de zero = Tem
                {
                    _ProcurarUsuarioUltimaAlteracao = true;
                }

                // Verificando o tipo, data de criação e data da última alteração do objeto
                _Query = "";
                _Query = _Query + "SELECT MIN(OBJECT_TYPE), ";
                _Query = _Query + "    MIN(CREATED), ";
                _Query = _Query + "    MAX(LAST_DDL_TIME), ";
                _Query = _Query + "    MIN(STATUS) ";
                _Query = _Query + "FROM ALL_OBJECTS ";
                _Query = _Query + "WHERE OWNER = '" + _OwnerObj + "' ";
                _Query = _Query + "    AND OBJECT_NAME = '" + _NomeObj + "' ";
                _Query = _Query + "    AND OBJECT_TYPE NOT LIKE '%TABLE%' ";
                //_Query = _Query + "    AND ROWNUM = 1 ";
                //_Query = _Query + "GROUP BY OBJECT_TYPE ";
                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                if (_DataReader.Read())
                {
                    if (_DataReader.IsDBNull(0))
                    {
                        p_Mensagem = "Objeto " + p_NomeObj + " não encontrado";
                        _Retorno = "";
                        _StatusObjeto = "INDETERMINADO";
                    }
                    else
                    {
                        p_TipoObjeto = _DataReader.GetString(0);
                        p_TipoObjeto = p_TipoObjeto.Trim().ToUpper();

                        _DataCriacaoObjeto = _DataReader.GetDateTime(1);
                        _DataUltimaAlteracaoObjeto = _DataReader.GetDateTime(2);
                        _StatusObjeto = _DataReader.GetString(3);
                    }
                }
                else
                {
                    p_Mensagem = "Objeto " + p_NomeObj + " não encontrado";
                    _Retorno = "";
                    _StatusObjeto = "INDETERMINADO";
                }
                _DataReader.Close();

                if (_ProcurarUsuarioUltimaAlteracao)
                {
                    _Query = "";
                    _Query = _Query + "SELECT MAX(AU.DATA_HORA) AS DATA_HORA, "; // 0
                    _Query = _Query + "    AU.OWNER AS OWNER_OBJ, "; // 1
                    _Query = _Query + "    AU.NOME AS NOME_OBJ, "; // 2
                    _Query = _Query + "    CO.NOME_COMPLETO, "; // 3
                    _Query = _Query + "    CO.EMAIL, "; // 4
                    _Query = _Query + "    AU.OSUSER, "; // 5
                    _Query = _Query + "    AU.CURRENT_USER, "; // 6
                    _Query = _Query + "    AU.HOST "; // 7
                    _Query = _Query + "FROM AESPROD.AUDITORIA_OBJETOS AU ";
                    _Query = _Query + "    LEFT JOIN AESPROD.CONTATO_CONTROLE_CONCORRENCIA CO ";
                    _Query = _Query + "    ON AU.ID_USUARIO = CO.ID ";
                    _Query = _Query + "WHERE AU.OWNER = '" + _OwnerObj + "' ";
                    _Query = _Query + "    AND AU.NOME = '" + _NomeObj + "' ";
                    _Query = _Query + "GROUP BY AU.OWNER, AU.NOME, CO.ID, CO.NOME_COMPLETO, CO.EMAIL, AU.OSUSER, AU.CURRENT_USER, AU.HOST ";
                    _Query = _Query + "ORDER BY MAX(AU.DATA_HORA) DESC ";

                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();
                    if (_DataReader.Read())
                    {
                        _TextoUsuarioUltimaAlteracao = "";
                        _TextoUsuarioUltimaAlteracao = _TextoUsuarioUltimaAlteracao + "-- Último CREATE OR REPLACE ou DEBUG: " + _DataReader.GetDateTime(0).ToString("dd/MM/yy HH:mm:ss") + "\n";
                        if (_DataReader.IsDBNull(3))
                        {
                            _TextoUsuarioUltimaAlteracao = _TextoUsuarioUltimaAlteracao + "-- Analista: INDEFINIDO";
                        }
                        else
                        {
                            _TextoUsuarioUltimaAlteracao = _TextoUsuarioUltimaAlteracao + "-- Analista: " + _DataReader.GetString(3);
                        }
                        if (_DataReader.IsDBNull(4))
                        {
                            _TextoUsuarioUltimaAlteracao = _TextoUsuarioUltimaAlteracao + "\n";
                        }
                        else
                        {
                            if (_DataReader.GetString(4).Trim().Length == 0)
                            {
                                _TextoUsuarioUltimaAlteracao = _TextoUsuarioUltimaAlteracao + "\n";
                            }
                            else
                            {
                                _TextoUsuarioUltimaAlteracao = _TextoUsuarioUltimaAlteracao + " <" + _DataReader.GetString(4) + ">\n";
                            }
                        }
                        _TextoUsuarioUltimaAlteracao = _TextoUsuarioUltimaAlteracao + "-- Usuário de banco: " + _DataReader.GetString(6) + "\n";
                        _TextoUsuarioUltimaAlteracao = _TextoUsuarioUltimaAlteracao + "-- Nome da máquina: " + _DataReader.GetString(7) + "\n";
                        _TextoUsuarioUltimaAlteracao = _TextoUsuarioUltimaAlteracao + "-- Usuário da máquina: " + _DataReader.GetString(5) + "\n";
                    }

                    _DataReader.Close();
                }

                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                if (p_TipoObjeto.Trim().Length > 0)
                {
                    switch (p_TipoObjeto)
                    {
                        //FUNCTION
                        //LIBRARY
                        //PACKAGE
                        //PACKAGE BODY
                        //PROCEDURE
                        //TRIGGER
                        //TYPE
                        //TYPE BODY
                        case "FUNCTION":
                        case "PACKAGE":
                        case "PROCEDURE":
                        case "TRIGGER":
                        case "JAVA SOURCE":
                            _Retorno = this.ExtractDDL_ALL_SOURCE(p_Usuario, p_Senha, p_Database, _OwnerObj, _NomeObj, p_TipoObjeto, ref p_Mensagem);
                            if (_Retorno.Trim().Length == 0)
                            {
                                break;
                            }
                            _Retorno = _Retorno + this.ExtractSinonimo(p_Usuario, p_Senha, p_Database, _OwnerObj, _NomeObj);
                            if (p_TipoObjeto != "JAVA SOURCE")
                            {
                                _Retorno = _Retorno + this.ExtractGrants(p_Usuario, p_Senha, p_Database, _OwnerObj, _NomeObj);
                            }
                            break;

                        case "PACKAGE BODY":
                            _Retorno = this.ExtractDDL_ALL_SOURCE(p_Usuario, p_Senha, p_Database, _OwnerObj, _NomeObj, "PACKAGE", ref p_Mensagem);
                            if (_Retorno.Trim().Length == 0)
                            {
                                break;
                            }
                            _Retorno = _Retorno + this.ExtractSinonimo(p_Usuario, p_Senha, p_Database, _OwnerObj, _NomeObj);
                            _Retorno = _Retorno + this.ExtractGrants(p_Usuario, p_Senha, p_Database, _OwnerObj, _NomeObj);
                            break;

                        case "JOB":
                            _Retorno = this.ExtractDDL_JOB(p_Usuario, p_Senha, p_Database, _OwnerObj, _NomeObj, ref p_Mensagem);
                            break;

                        case "VIEW":
                            _Retorno = this.ExtractDDL_VIEW(p_Usuario, p_Senha, p_Database, _OwnerObj, _NomeObj, ref p_Mensagem);
                            if (_Retorno.Trim().Length == 0)
                            {
                                break;
                            }
                            _Retorno = _Retorno + this.ExtractSinonimo(p_Usuario, p_Senha, p_Database, _OwnerObj, _NomeObj);
                            _Retorno = _Retorno + this.ExtractGrants(p_Usuario, p_Senha, p_Database, _OwnerObj, _NomeObj);
                            break;

                        case "SEQUENCE":
                            _Retorno = this.ExtractDDL_SEQUENCE(p_Usuario, p_Senha, p_Database, _OwnerObj, _NomeObj, ref p_Mensagem);
                            _Retorno = _Retorno + this.ExtractSinonimo(p_Usuario, p_Senha, p_Database, _OwnerObj, _NomeObj);
                            _Retorno = _Retorno + this.ExtractGrants(p_Usuario, p_Senha, p_Database, _OwnerObj, _NomeObj);
                            break;

                        default:
                            p_Mensagem = "Tipo de objeto " + p_TipoObjeto + " não suportado";
                            _Retorno = "";
                            break;
                    }
                }

                if (_Retorno.Trim().Length > 0)
                {
                    while (_Retorno.Substring(_Retorno.Length - 1) == "\n" || _Retorno.Substring(_Retorno.Length - 1) == "\r")
                    {
                        _Retorno = _Retorno.Substring(0, _Retorno.Length - 1);
                    }
                }

                if (_Retorno.Trim().Length > 0)
         
                {
                    if (this.ColocarCabecalhoAoExtrairDDL)
                    {
                        _Cabecalho = "";
                        _Cabecalho = _Cabecalho + "-- Fonte extraído pelo Oracle Tools\n";
                        _Cabecalho = _Cabecalho + "-- Dados de conexão: " + this.InfoBanco(p_Usuario, p_Senha, p_Database, ref p_Mensagem) + "\n";
                        _Cabecalho = _Cabecalho + "-- Objeto criado em: " + _DataCriacaoObjeto.ToString("dd/MM/yy HH:mm:ss") + "\n";
                        _Cabecalho = _Cabecalho + "-- Última compilação deste objeto: " + _DataUltimaAlteracaoObjeto.ToString("dd/MM/yy HH:mm:ss") + "\n";
                        _Cabecalho = _Cabecalho + "-- Status deste objeto: " + _StatusObjeto + "\n";
                        _Retorno = _Cabecalho + _TextoUsuarioUltimaAlteracao + "\n" + _Retorno;
                    }
                    
                }

                return _Retorno.Replace("\n", _QuebraLinha);
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ExtractDDL(p_Usuario, p_Senha, p_Database, p_NomeObj, ref p_TipoObjeto, ref p_Mensagem);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ExtractDDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public decimal QuantosBackups(string p_NomeObj, ref string p_Mensagem)
        {
            decimal _Retorno = 0;
            string[] _Split;
            string _OwnerObj = "";
            string _NomeObj = "";
            string _Query = "";

            if (this.TemTabContrConc)
            {
                _Split = p_NomeObj.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (_Split.Length != 2)
                {
                    p_Mensagem = "Nome de objeto inválido\n" + p_NomeObj;
                    return _Retorno;
                }
                _OwnerObj = _Split[0].Trim().ToUpper();
                _NomeObj = _Split[1].Trim().ToUpper();

                // Abrindo conexão com o banco
                OleDbCommand _Command = new OleDbCommand();
                OleDbConnection _Connection = new OleDbConnection();
                OleDbDataReader _DataReader;

                try
                {
                    _Connection.ConnectionString = "Provider=OraOLEDB.Oracle; User Id=" + _UsuarioDB + ";Password=" + _SenhaDB + ";Data Source=" + _AliasDatabase + ";ChunkSize=65535;"; // 65535 é o máximo. se colocar um parâmetro maior que isso vão vir menos caracteres.
                    _Connection.Open();
                    _Command.Connection = _Connection;
                    _Command.CommandType = CommandType.Text;

                    _Query = "";
                    _Query = _Query + "SELECT COUNT(DISTINCT ID_ALTERACAO) ";
                    _Query = _Query + "FROM AESPROD.BKP_ALL_SOURCE ";
                    _Query = _Query + "WHERE OWNER = '" + _OwnerObj + "' ";
                    _Query = _Query + "    AND NOME = '" + _NomeObj + "' ";

                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();

                    if (_DataReader.Read())
                    {
                        _Retorno = _DataReader.GetDecimal(0);
                    }

                    _DataReader.Close();
                    _DataReader.Dispose();
                    _Connection.Close();
                    _Connection.Dispose();
                    GC.Collect();

                }
                catch (Exception _Exception)
                {
                    if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                    {
                        this.FazConsultaDUAL(_UsuarioDB, _SenhaDB, _AliasDatabase);
                        return this.QuantosBackups(p_NomeObj, ref p_Mensagem);
                    }
                    else
                    {
                        p_Mensagem = "ERRO\n" + _Exception.ToString();
                        return _Retorno;
                    }
                }

            }
            else
            {
                p_Mensagem = "Dados de conexão ou tabelas de controle não encontrado!";
            }

            return _Retorno;
        }

        public ArrayList ListaBackupsObjeto(string p_NomeObj, ref string p_Mensagem)
        {
            ArrayList _Retorno = new ArrayList();
            string[] _Split;
            string _OwnerObj = "";
            string _NomeObj = "";
            string _Query = "";

            if (this.TemTabContrConc)
            {
                _Split = p_NomeObj.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (_Split.Length != 2)
                {
                    p_Mensagem = "Nome de objeto inválido\n" + p_NomeObj;
                    return _Retorno;
                }

                _OwnerObj = _Split[0].Trim().ToUpper();
                _NomeObj = _Split[1].Trim().ToUpper();
                
                // Abrindo conexão com o banco
                OleDbCommand _Command = new OleDbCommand();
                OleDbConnection _Connection = new OleDbConnection();
                OleDbDataReader _DataReader;
                csBackupObjetoDeBanco _csBackupObjetoDeBanco = null;

                try
                {
                    _Connection.ConnectionString = "Provider=OraOLEDB.Oracle; User Id=" + _UsuarioDB + ";Password=" + _SenhaDB + ";Data Source=" + _AliasDatabase + ";ChunkSize=65535;"; // 65535 é o máximo. se colocar um parâmetro maior que isso vão vir menos caracteres.
                    _Connection.Open();
                    _Command.Connection = _Connection;
                    _Command.CommandType = CommandType.Text;

                    _Query = "";
                    _Query = _Query + "SELECT DISTINCT ";
                    _Query = _Query + "    BKP.ID_ALTERACAO, "; // 0
                    _Query = _Query + "    BKP.DATA_HORA, "; // 1
                    _Query = _Query + "    BKP.TIPO, "; // 2
                    _Query = _Query + "    CONTATO.ID AS ID_GERADOR_DO_BACKUP, "; // 3
                    _Query = _Query + "    CONTATO.NOME_COMPLETO AS NOME_GERADOR_DO_BACKUP, "; // 4
                    _Query = _Query + "    CONTATO.EMAIL AS EMAIL_GERADOR_DO_BACKUP "; // 5
                    _Query = _Query + "FROM AESPROD.BKP_ALL_SOURCE BKP ";
                    _Query = _Query + "    LEFT JOIN AESPROD.CONTATO_CONTROLE_CONCORRENCIA CONTATO ";
                    _Query = _Query + "    ON BKP.ID_USUARIO = CONTATO.ID ";
                    _Query = _Query + "WHERE OWNER = '" + _OwnerObj + "' ";
                    _Query = _Query + "    AND NOME = '" + _NomeObj + "' ";
                    _Query = _Query + "ORDER BY BKP.ID_ALTERACAO DESC ";
                    
                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();

                    while (_DataReader.Read())
                    {
                        _csBackupObjetoDeBanco = new csBackupObjetoDeBanco();
                        _csBackupObjetoDeBanco.Owner = _OwnerObj;
                        _csBackupObjetoDeBanco.Nome = _NomeObj;
                        _csBackupObjetoDeBanco.Id_Alteracao = _DataReader.GetDecimal(0);
                        _csBackupObjetoDeBanco.DataBackup = _DataReader.GetDateTime(1);
                        _csBackupObjetoDeBanco.Tipo = _DataReader.GetString(2);
                        if (!_DataReader.IsDBNull(3))
	                    {
		                    _csBackupObjetoDeBanco.IDUsuarioQueGerouBackup = _DataReader.GetDecimal(3);
	                    }
                        if (!_DataReader.IsDBNull(4))
                        {
                            _csBackupObjetoDeBanco.NomeUsuarioQueGerouBackup = _DataReader.GetString(4);
                        }
                        if (!_DataReader.IsDBNull(5))
                        {
                            _csBackupObjetoDeBanco.EmailUsuarioQueGerouBackup = _DataReader.GetString(5);
                        }
                        _Retorno.Add(_csBackupObjetoDeBanco);
                    }

                    _DataReader.Close();
                    _DataReader.Dispose();
                    _Connection.Close();
                    _Connection.Dispose();
                    GC.Collect();

                }
                catch (Exception _Exception)
                {
                    if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                    {
                        this.FazConsultaDUAL(_UsuarioDB, _SenhaDB, _AliasDatabase);
                        return this.ListaBackupsObjeto(p_NomeObj, ref p_Mensagem);
                    }
                    else
                    {
                        p_Mensagem = "ERRO\n" + _Exception.ToString();
                        return _Retorno;
                    }
                }
            }
            else
            {
                p_Mensagem = "Dados de conexão ou tabelas de controle não encontrado!";
            }

            return _Retorno;
        }

        public string ExtractBackupObjeto(decimal p_Id_Alteracao, ref string p_OwnerNomeObjeto, ref string p_Mensagem)
        {
            StringBuilder _Retorno = new StringBuilder();
            string _Query = "";
            string _Linha = "";
            string _Cabecalho = "";
            string _OwnerObj = "";
            string _NomeObj = "";

            if (this.TemTabContrConc)
            {
                // Abrindo conexão com o banco
                OleDbCommand _Command = new OleDbCommand();
                OleDbConnection _Connection = new OleDbConnection();
                OleDbDataReader _DataReader;

                try
                {
                    _Connection.ConnectionString = "Provider=OraOLEDB.Oracle; User Id=" + _UsuarioDB + ";Password=" + _SenhaDB + ";Data Source=" + _AliasDatabase + ";ChunkSize=65535;"; // 65535 é o máximo. se colocar um parâmetro maior que isso vão vir menos caracteres.
                    _Connection.Open();
                    _Command.Connection = _Connection;
                    _Command.CommandType = CommandType.Text;

                    _Query = "";
                    _Query = _Query + "SELECT BKP.DATA_HORA, "; // 0
                    _Query = _Query + "    BKP.OWNER, "; // 1
                    _Query = _Query + "    BKP.NOME, "; // 2
                    _Query = _Query + "    BKP.LINHA, "; // 3
                    _Query = _Query + "    BKP.TEXTO, "; // 4
                    _Query = _Query + "    CONTATO.NOME_COMPLETO AS GERADOR_DO_BACKUP, "; // 5
                    _Query = _Query + "    CONTATO.EMAIL AS EMAIL_GERADOR_DO_BACKUP "; // 6
                    _Query = _Query + "FROM AESPROD.BKP_ALL_SOURCE BKP ";
                    _Query = _Query + "    LEFT JOIN AESPROD.CONTATO_CONTROLE_CONCORRENCIA CONTATO ";
                    _Query = _Query + "    ON BKP.ID_USUARIO = CONTATO.ID ";
                    _Query = _Query + "WHERE BKP.ID_ALTERACAO = " + p_Id_Alteracao + " ";
                    _Query = _Query + "ORDER BY BKP.LINHA ";

                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();

                    if (_DataReader.Read())
                    {

                        _Cabecalho = "";
                        _Cabecalho = _Cabecalho + "-- Fonte extraído pelo Oracle Tools\n";
                        _Cabecalho = _Cabecalho + "-- Dados de conexão: " + this.InfoBanco(_UsuarioDB, _SenhaDB, _AliasDatabase, ref p_Mensagem) + "\n";
                        _Cabecalho = _Cabecalho + "-- Backup gerado em : " + _DataReader.GetDateTime(0).ToString("dd/MM/yy HH:mm:ss") + "\n";
                        if (!_DataReader.IsDBNull(5))
                        {
                            _Cabecalho = _Cabecalho + "-- Backup gerado pela alteração de: " + _DataReader.GetString(5);
                            if (!_DataReader.IsDBNull(6))
                            {
                                _Cabecalho = _Cabecalho + " - " + _DataReader.GetString(6) + "\n";
                            }
                            else
                            {
                                _Cabecalho = _Cabecalho + "\n";
                            }
                        }
                        else
                        {
                            _Cabecalho = _Cabecalho + "-- Backup gerado pela alteração de: Informação não disponível\n";
                        }
                        _Cabecalho = _Cabecalho + "\n";
                        _Retorno.Append(_Cabecalho);
                        _Retorno.Append("CREATE OR REPLACE ");

                        _OwnerObj = _DataReader.GetString(1);
                        _OwnerObj = _OwnerObj.Trim().ToUpper();
                        _NomeObj = _DataReader.GetString(2);
                        _NomeObj = _NomeObj.Trim().ToUpper();

                        p_OwnerNomeObjeto = _OwnerObj + "." + _NomeObj;

                        _Linha = _DataReader.GetString(4);
                        _Linha = _Linha.Trim().ToUpper();
                        _Linha = _Linha.Replace("\"", "");
                        if (_Linha.IndexOf(_OwnerObj + ".") < 0)
                        {
                            _Linha = _Linha.Replace(_NomeObj, _OwnerObj + "." + _NomeObj);
                        }
                        while (_Linha.IndexOf("  ") >= 0)
                        {
                            _Linha = _Linha.Replace("  ", " ");
                        }
                        _Retorno.Append(_Linha + "\n");
                    }

                    while (_DataReader.Read())
                    {
                        _Linha = _DataReader.GetString(4);
                        _Retorno.Append(_Linha);
                    }
                    if (_Retorno.ToString().Substring(_Retorno.Length - 1) != "\n" && _Retorno.ToString().Substring(_Retorno.Length - 1) != "\r")
                    {
                        _Retorno.Append("\n");
                    }
                    _Retorno.Append("/\n\n");

                    _DataReader.Close();
                    _DataReader.Dispose();
                    _Connection.Close();
                    _Connection.Dispose();
                    GC.Collect();

                }
                catch (Exception _Exception)
                {
                    if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                    {
                        this.FazConsultaDUAL(_UsuarioDB, _SenhaDB, _AliasDatabase);
                        return this.ExtractBackupObjeto(p_Id_Alteracao, ref p_OwnerNomeObjeto, ref p_Mensagem);
                    }
                    else
                    {
                        p_Mensagem = "ERRO\n" + _Exception.ToString();
                        return _Retorno.ToString();
                    }
                }
            }
            else
            {
                p_Mensagem = "Dados de conexão ou tabelas de controle não encontrado!";
            }

            return _Retorno.ToString();
        }

        #endregion Extract DDL

        public void MatarSessoes(string p_Usuario, string p_Senha, string p_Database, string p_ListaSessoes)
        {
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
            _Connection.Open();

            _Command.Connection = _Connection;
            _Command.CommandType = CommandType.Text;

            string[] _Split;
            p_ListaSessoes = p_ListaSessoes.Replace("\r", "");
            _Split = p_ListaSessoes.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < _Split.Length; i++)
            {
                this.MatarSessao(_Command, _Split[i]);
            }
        }

        private void MatarSessao(OracleCommand p_Command, string p_SID_SERIAL)
        {
            try
            {
                string _Comando = "ALTER SYSTEM DISCONNECT SESSION '" + p_SID_SERIAL + "' IMMEDIATE";
                p_Command.CommandText = _Comando;
                p_Command.ExecuteNonQuery();
            }
            catch (Exception _Exception)
            {
                MessageBox.Show("ERRO\n" + _Exception.Message, "MatarSessao", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RecompilarObjetosInvalidos(string p_Usuario, string p_Senha, string p_Database, ref bool p_CompilouAlgo, ref string p_Relatorio)
        {
            _UsuarioDB = p_Usuario;
            _SenhaDB = p_Senha;
            _AliasDatabase = p_Database;

            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;
            string _Consulta = "";
            p_Relatorio = "";
            bool _Parar = false;
            decimal _QuantInvalidosAntes = 0;
            decimal _QuantInvalidosDepois = 0;
            int _ContTentativas = 0;
            string _ComandoCompile = "";

            p_CompilouAlgo = false;

            try
            {

                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                while (!_Parar)
                {
                    _ContTentativas++;

                    // 1 - Buscar a quantidade de inválidos antes
                    p_Relatorio = p_Relatorio + "Tentativa: " + _ContTentativas.ToString() + " - Buscando quantidade de objetos inválidos antes\r\n";
                    _Consulta = "";
                    _Consulta = _Consulta + "SELECT SUM(TOTALINVALIDOSANTES) AS TOTALINVALIDOSANTES ";
                    _Consulta = _Consulta + "FROM ( ";
                    _Consulta = _Consulta + "    SELECT COUNT(*) AS TOTALINVALIDOSANTES ";
                    _Consulta = _Consulta + "    FROM ALL_OBJECTS ";
                    _Consulta = _Consulta + "    WHERE OBJECT_TYPE <> 'SYNONYM' ";
                    _Consulta = _Consulta + "        AND STATUS <> 'VALID' ";
                    _Consulta = _Consulta + "UNION ";
                    _Consulta = _Consulta + "    SELECT COUNT(*) AS TOTALINVALIDOSANTES ";
                    _Consulta = _Consulta + "    FROM ALL_OBJECTS ";
                    _Consulta = _Consulta + "    WHERE OBJECT_TYPE = 'SYNONYM' ";
                    _Consulta = _Consulta + "        AND OWNER NOT IN ('GIP981', 'RAJADAS') ";
                    _Consulta = _Consulta + "        AND STATUS <> 'VALID' ";
                    _Consulta = _Consulta + "    ) ";

                    _Command.CommandText = _Consulta;
                    _DataReader = _Command.ExecuteReader();

                    if (_DataReader.Read())
                    {
                        _QuantInvalidosAntes = _DataReader.GetDecimal(0);
                    }

                    if (_QuantInvalidosAntes == 0)
                    {
                        _Parar = true;
                        _DataReader.Close();
                        _DataReader.Dispose();
                        _Connection.Close();
                        _Connection.Dispose();
                        GC.Collect();
                        p_Relatorio = p_Relatorio + "Tentativa: " + _ContTentativas.ToString() + " - Nenhum objeto inválido encontrado, encerrando execução\r\n";
                    }
                    else
                    {

                        #region Recompilando PACKAGE E PACKAGE BODY

                        // 2 - Abrir um cursor com o nome dos inválidos - PACKAGE E PACKAGE BODY
                        _Consulta = "";
                        _Consulta = _Consulta + "SELECT OWNER, ";
                        _Consulta = _Consulta + "    OBJECT_NAME, ";
                        _Consulta = _Consulta + "    OBJECT_TYPE, ";
                        _Consulta = _Consulta + "    DECODE(OBJECT_TYPE, 'PACKAGE', 1, 'PACKAGE BODY', 2, 2) AS RECOMPILE_ORDER ";
                        _Consulta = _Consulta + "FROM ALL_OBJECTS ";
                        _Consulta = _Consulta + "WHERE OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY') ";
                        _Consulta = _Consulta + "    AND  OWNER NOT IN ('SYS') ";
                        _Consulta = _Consulta + "    AND  STATUS != 'VALID' ";
                        _Consulta = _Consulta + "ORDER BY OBJECT_NAME, RECOMPILE_ORDER ";

                        // 3 - Abrir o cursor e montar o comando de recompile
                        _Command.CommandText = _Consulta;
                        _DataReader = _Command.ExecuteReader();

                        while (_DataReader.Read())
                        {
                            if (_DataReader.GetString(2) == "PACKAGE")
                            {
                                p_Relatorio = p_Relatorio + "Tentativa: " + _ContTentativas.ToString() + " - Recompilando Package: " + _DataReader.GetString(0) + "." + _DataReader.GetString(1) + "\r\n";
                                _ComandoCompile = "ALTER PACKAGE " + _DataReader.GetString(0) + "." + _DataReader.GetString(1) + " COMPILE";
                            }
                            else
                            {
                                p_Relatorio = p_Relatorio + "Tentativa: " + _ContTentativas.ToString() + " - Recompilando Package Body: " + _DataReader.GetString(0) + "." + _DataReader.GetString(1) + "\r\n";
                                _ComandoCompile = "ALTER PACKAGE " + _DataReader.GetString(0) + "." + _DataReader.GetString(1) + " COMPILE BODY";
                            }

                            try
                            {
                                // 4 - Executar o comando de recompile
                                _Command.CommandText = _ComandoCompile;
                                _Command.ExecuteNonQuery();
                            }
                            catch (Exception _ExceptionCompile)
                            {
                                p_Relatorio = p_Relatorio + "ERRO: " + _ExceptionCompile.Message + "\r\n";
                            }
                        } // while (_DataReader.Read())

                        _DataReader.Close();

                        #endregion Recompilando PACKAGE E PACKAGE BODY

                        #region Recompilando PROCEDURE, FUNCTION, TRIGGER e VIEW

                        // 2 - Abrir um cursor com o nome dos inválidos - PROCEDURE, FUNCTION, TRIGGER e VIEW
                        _Consulta = "";
                        _Consulta = _Consulta + "SELECT OWNER, ";
                        _Consulta = _Consulta + "    OBJECT_NAME, ";
                        _Consulta = _Consulta + "    OBJECT_TYPE ";
                        _Consulta = _Consulta + "FROM ALL_OBJECTS ";
                        _Consulta = _Consulta + "WHERE OBJECT_TYPE IN ('PROCEDURE', 'FUNCTION', 'TRIGGER', 'VIEW') ";
                        _Consulta = _Consulta + "    AND STATUS != 'VALID' ";
                        _Consulta = _Consulta + "    AND OWNER NOT IN ('SYS') ";
                        _Consulta = _Consulta + "ORDER BY OBJECT_NAME ";

                        // 3 - Abrir o cursor e montar o comando de recompile
                        _Command.CommandText = _Consulta;
                        _DataReader = _Command.ExecuteReader();

                        while (_DataReader.Read())
                        {
                            p_Relatorio = p_Relatorio + "Tentativa: " + _ContTentativas.ToString() + " - Recompilando " + _DataReader.GetString(2) + ": " +_DataReader.GetString(0) + "." + _DataReader.GetString(1) + "\r\n";
                            _ComandoCompile = "ALTER " + _DataReader.GetString(2) + " " +_DataReader.GetString(0) + "." + _DataReader.GetString(1) + " COMPILE";

                            try
                            {
                                // 4 - Executar o comando de recompile
                                _Command.CommandText = _ComandoCompile;
                                _Command.ExecuteNonQuery();
                            }
                            catch (Exception _ExceptionCompile)
                            {
                                p_Relatorio = p_Relatorio + "ERRO: " + _ExceptionCompile.Message + "\r\n";
                            }
                        } // while (_DataReader.Read())

                        _DataReader.Close();

                        #endregion Recompilando PROCEDURE, FUNCTION, TRIGGER e VIEW

                        #region Recompilando SINÔNIMOS

                        // 2 - Abrir um cursor com o nome dos inválidos - SINÔNIMOS
                        _Consulta = "";
                        _Consulta = _Consulta + "SELECT DISTINCT ALL_OBJECTS.OWNER, ";
                        _Consulta = _Consulta + "    ALL_OBJECTS.OBJECT_NAME, ";
                        _Consulta = _Consulta + "    ALL_SYNONYMS.TABLE_OWNER, ";
                        _Consulta = _Consulta + "    ALL_SYNONYMS.TABLE_NAME ";
                        _Consulta = _Consulta + "FROM ALL_OBJECTS ";
                        _Consulta = _Consulta + "    INNER JOIN ALL_SYNONYMS ";
                        _Consulta = _Consulta + "    ON ALL_OBJECTS.OBJECT_NAME = ALL_SYNONYMS.SYNONYM_NAME ";
                        _Consulta = _Consulta + "WHERE ALL_OBJECTS.OBJECT_TYPE = 'SYNONYM' ";
                        _Consulta = _Consulta + "    AND ALL_OBJECTS.STATUS != 'VALID' ";
                        _Consulta = _Consulta + "ORDER BY ALL_OBJECTS.OBJECT_NAME ";

                        // 3 - Abrir o cursor e montar o comando de recompile
                        _Command.CommandText = _Consulta;
                        _DataReader = _Command.ExecuteReader();

                        while (_DataReader.Read())
                        {
                            if (_DataReader.GetString(0) == "PUBLIC")
                            {
                                p_Relatorio = p_Relatorio + "Tentativa: " + _ContTentativas.ToString() + " - Recompilando Sinônimo Público: " + _DataReader.GetString(1) + "\r\n";
                                _ComandoCompile = "CREATE OR REPLACE PUBLIC SYNONYM " + _DataReader.GetString(1) + " FOR " + _DataReader.GetString(2) + "." + _DataReader.GetString(3);
                            }
                            else
                            {
                                p_Relatorio = p_Relatorio + "Tentativa: " + _ContTentativas.ToString() + " - Recompilando Sinônimo: " + _DataReader.GetString(0) + "." + _DataReader.GetString(1) + "\r\n";
                                _ComandoCompile = "ALTER SYNONYM " + _DataReader.GetString(0) + "." + _DataReader.GetString(1) + " COMPILE";
                            }

                            try
                            {
                                // 4 - Executar o comando de recompile
                                _Command.CommandText = _ComandoCompile;
                                _Command.ExecuteNonQuery();
                            }
                            catch (Exception _ExceptionCompile)
                            {
                                p_Relatorio = p_Relatorio + "ERRO: " + _ExceptionCompile.Message + "\r\n";
                            }
                        } // while (_DataReader.Read())

                        _DataReader.Close();

                        #endregion Recompilando SINÔNIMOS

                        // 5 - Verificar a quantidade de inválidos depois
                        p_Relatorio = p_Relatorio + "Tentativa: " + _ContTentativas.ToString() + " - Buscando quantidade de objetos inválidos depois\r\n";
                        _Consulta = "";
                        _Consulta = _Consulta + "SELECT SUM(TOTALINVALIDOSANTES) AS TOTALINVALIDOSANTES ";
                        _Consulta = _Consulta + "FROM ( ";
                        _Consulta = _Consulta + "    SELECT COUNT(*) AS TOTALINVALIDOSANTES ";
                        _Consulta = _Consulta + "    FROM ALL_OBJECTS ";
                        _Consulta = _Consulta + "    WHERE OBJECT_TYPE <> 'SYNONYM' ";
                        _Consulta = _Consulta + "        AND STATUS <> 'VALID' ";
                        _Consulta = _Consulta + "UNION ";
                        _Consulta = _Consulta + "    SELECT COUNT(*) AS TOTALINVALIDOSANTES ";
                        _Consulta = _Consulta + "    FROM ALL_OBJECTS ";
                        _Consulta = _Consulta + "    WHERE OBJECT_TYPE = 'SYNONYM' ";
                        _Consulta = _Consulta + "        AND OWNER NOT IN ('GIP981', 'RAJADAS') ";
                        _Consulta = _Consulta + "        AND STATUS <> 'VALID' ";
                        _Consulta = _Consulta + "    ) ";

                        _Command.CommandText = _Consulta;
                        _DataReader = _Command.ExecuteReader();

                        if (_DataReader.Read())
                        {
                            _QuantInvalidosDepois = _DataReader.GetDecimal(0);
                        }

                        p_Relatorio = p_Relatorio + "Tentativa: " + _ContTentativas.ToString() + " - Objetos inválidos antes = " + _QuantInvalidosAntes.ToString() + " objetos inválidos depois = " + _QuantInvalidosDepois.ToString() + "\r\n";

                        if ((_QuantInvalidosAntes > _QuantInvalidosDepois) && (p_CompilouAlgo == false))
                        {
                            p_CompilouAlgo = true;
                        }

                        // 6 - Repetir o procedimento se a quantidade de antes e depois mudou, cuidar pra evitar um loop infiníto
                        if (_QuantInvalidosDepois == 0)
                        {
                            _Parar = true;
                            _DataReader.Close();
                            _DataReader.Dispose();
                            _Connection.Close();
                            _Connection.Dispose();
                            GC.Collect();
                            p_Relatorio = p_Relatorio + "Tentativa: " + _ContTentativas.ToString() + " - Nenhum objeto inválido encontrado, encerrando execução\r\n";
                        }
                        else if (_QuantInvalidosAntes == _QuantInvalidosDepois)
                        {
                            _Parar = true;
                            _DataReader.Close();
                            _DataReader.Dispose();
                            _Connection.Close();
                            _Connection.Dispose();
                            GC.Collect();
                            p_Relatorio = p_Relatorio + "Tentativa: " + _ContTentativas.ToString() + " - Objetos inválidos antes igual a quantidade de objetos inválidos depois, encerrando execução\r\n";
                        }
                        else
                        {
                            p_Relatorio = p_Relatorio + "Tentativa: " + _ContTentativas.ToString() + " - Objetos inválidos antes diferente da quantidade de objetos inválidos depois, executando mais uma vez\r\n";
                        }

                    } // else do if (_QuantInvalidosAntes == 0)

                } // while (!_Parar)
            }
            catch (Exception _ExceptionRotina)
            {
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();

                if (_ExceptionRotina.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    this.RecompilarObjetosInvalidos(p_Usuario, p_Senha, p_Database, ref p_CompilouAlgo, ref p_Relatorio);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _ExceptionRotina.ToString(), "RecompilarObjetosInvalidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        //public string DumpToAnsi(string p_String)
        //{
        //    string _Retorno = "";
        //    int _Pos = -1;
        //    string[] _Split;

        //    _Pos = p_String.IndexOf(":");
        //    p_String = p_String.Substring(_Pos + 1).Trim();
        //    _Split = p_String.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        //    for (int i = 0; i < _Split.Length; i++)
        //    {
        //        _Retorno = _Retorno + (char)(Convert.ToInt32(_Split[i]));
        //    }

        //    return _Retorno;
        //}

    #endregion Métodos públicos

    #region Métodos privados

        private void FazConsultaDUAL(string p_Usuario, string p_Senha, string p_Database)
        {
            //MessageBox.Show("Entro na FazConsultaDUAL");
            //try
            //{
            //    OracleCommand _Command = new OracleCommand();
            //    OracleConnection _Connection = new OracleConnection();
            //    OracleDataReader _DataReader = null;
            //    string _Consulta = "";

            //    _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
            //    _Connection.Open();

            //    _Consulta = "";
            //    _Consulta = _Consulta + "SELECT 1 FROM DUAL ";

            //    _Command.CommandText = _Consulta;
            //    _DataReader = _Command.ExecuteReader();
            //    _DataReader.Read();
            //    _DataReader.Close();
            //}
            //catch (Exception)
            //{
            //    // Faz nada
            //}
        }

        #region Extract DDL

        private string ExtractSinonimo(string p_Usuario, string p_Senha, string p_Database, string p_OwnerObj, string p_NomeObj)
        {
            string _Retorno = "";
            string _Query = "";
            string _Linha = "";

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Buscando se tem sinônimo
                _Query = "SELECT OWNER, SYNONYM_NAME FROM ALL_SYNONYMS WHERE TABLE_OWNER = '" + p_OwnerObj + "' AND TABLE_NAME = '" + p_NomeObj + "'";
                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                while (_DataReader.Read())
                {
                    if (_DataReader.GetString(0) == "PUBLIC")
                    {
                        _Linha = "CREATE OR REPLACE PUBLIC SYNONYM " + _DataReader.GetString(1) + " FOR " + p_OwnerObj + "." + p_NomeObj;
                    }
                    else
                    {
                        _Linha = "CREATE OR REPLACE SYNONYM " + _DataReader.GetString(0) + "." + _DataReader.GetString(1) + " FOR " + p_OwnerObj + "." + p_NomeObj;
                    }
                    _Retorno = _Retorno + _Linha + "\n/\n\n";
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ExtractSinonimo(p_Usuario, p_Senha, p_Database, p_OwnerObj, p_NomeObj);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ExtractSinonimo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            
        }

        private string ExtractGrants(string p_Usuario, string p_Senha, string p_Database, string p_OwnerObj, string p_NomeObj)
        {
            string _Retorno = "";
            string _Query = "";
            string _Linha = "";
            ArrayList _ListaGrantee = new ArrayList();
            string _Privilege = "";

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Pegando quem recebe os grants em ordem alfabética
                _Query = "SELECT DISTINCT GRANTEE FROM ALL_TAB_PRIVS WHERE TABLE_SCHEMA = '" + p_OwnerObj + "' AND TABLE_NAME = '" + p_NomeObj + "' ORDER BY GRANTEE";
                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                while (_DataReader.Read())
                {
                    _ListaGrantee.Add(_DataReader.GetString(0));
                }
                _DataReader.Close();

                // Para cada um que recebe grant
                for (int i = 0; i < _ListaGrantee.Count; i++)
                {
                    // Busvando os que GRANTABLE <> 'YES' e agrupando os privilégios
                    _Query = "SELECT PRIVILEGE, GRANTEE, GRANTABLE FROM ALL_TAB_PRIVS WHERE OWNER = '" + p_OwnerObj + "' AND TABLE_NAME = '" + p_NomeObj + "' AND GRANTEE = '" + _ListaGrantee[i] + "' AND GRANTABLE <> 'YES' ORDER BY GRANTEE";
                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();
                    _Privilege = "";
                    while (_DataReader.Read())
                    {
                        if (_Privilege.Length == 0)
                        {
                            _Privilege = _DataReader.GetString(0);
                        }
                        else
                        {
                            _Privilege = _Privilege + ", " + _DataReader.GetString(0);
                        }
                    }
                    if (_Privilege.Length != 0)
                    {
                        _Linha = "GRANT " + _Privilege + " ON " + p_OwnerObj + "." + p_NomeObj + " TO " + _ListaGrantee[i] + "\n/\n\n";
                        _Retorno = _Retorno + _Linha;
                    }
                    _DataReader.Close();

                    // Busvando os que GRANTABLE = 'YES' e agrupando os privilégios
                    _Query = "SELECT PRIVILEGE, GRANTEE, GRANTABLE FROM ALL_TAB_PRIVS WHERE OWNER = '" + p_OwnerObj + "' AND TABLE_NAME = '" + p_NomeObj + "' AND GRANTEE = '" + _ListaGrantee[i] + "' AND GRANTABLE = 'YES' ORDER BY GRANTEE";
                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();
                    _Privilege = "";
                    while (_DataReader.Read())
                    {
                        if (_Privilege.Length == 0)
                        {
                            _Privilege = _DataReader.GetString(0);
                        }
                        else
                        {
                            _Privilege = _Privilege + ", " + _DataReader.GetString(0);
                        }
                    }
                    if (_Privilege.Length != 0)
                    {
                        _Linha = "GRANT " + _Privilege + " ON " + p_OwnerObj + "." + p_NomeObj + " TO " + _ListaGrantee[i] + " WITH GRANT OPTION\n/\n\n";
                        _Retorno = _Retorno + _Linha;
                    }
                }

                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ExtractGrants(p_Usuario, p_Senha, p_Database, p_OwnerObj, p_NomeObj);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ExtractGrants", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        private string ExtractDDL_ALL_SOURCE(string p_Usuario, string p_Senha, string p_Database, string p_OwnerObj, string p_NomeObj, string p_TipoObj, ref string p_Mensagem)
        {
            StringBuilder _Retorno = new StringBuilder();
            string _Query = "";
            decimal _Quant = 0;
            bool _EncontrouFontes = false;
            string _Linha = "";
            bool _ColocarDISABLE = false;
            string _Status = "";
            string[] _Split;
            string _MsgErro = "";
            bool _ColocarMsgErro = this.ColocarMensagemErroNaLinhaAoExtrairDDL;
            decimal _UltimaLinhaInserida = -1;

            p_Mensagem = "";

            // Abrindo conexão com o banco
            OleDbCommand _Command = new OleDbCommand();
            OleDbConnection _Connection = new OleDbConnection();
            OleDbDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "Provider=OraOLEDB.Oracle; User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database + ";ChunkSize=65535;"; // 65535 é o máximo. se colocar um parâmetro maior que isso vão vir menos caracteres.
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Procurando se existem fontes do objeto
                _Query = "SELECT CAST(COUNT(*) AS INT) AS QUANT FROM ALL_SOURCE WHERE OWNER = '" + p_OwnerObj + "' AND NAME = '" + p_NomeObj + "'";
                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                _Quant = 0;
                if (_DataReader.Read())
                {
                    _Quant = _DataReader.GetDecimal(0);
                    if (_Quant > 0)
                    {
                        _EncontrouFontes = true;
                    }
                }
                _DataReader.Close();

                if (!_EncontrouFontes)
                {
                    p_Mensagem = "DDL não encontrado para o objeto\n" + p_OwnerObj + "." + p_NomeObj;
                    _DataReader.Dispose();
                    _Command.Dispose();
                    _Connection.Close();
                    _Connection.Dispose();
                    GC.Collect();
                    return "";
                }

                if (p_TipoObj == "TRIGGER")
                {
                    // Se é trigger, verificando se está DISABLE
                    _Query = "SELECT STATUS FROM ALL_TRIGGERS WHERE OWNER = '" + p_OwnerObj + "' AND TRIGGER_NAME = '" + p_NomeObj + "'";
                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();
                    if (_DataReader.Read())
                    {
                        _Status = _DataReader.GetString(0);
                        _Status = _Status.Trim().ToUpper();
                    }
                    _DataReader.Close();
                    if (_Status == "DISABLED")
                    {
                        _ColocarDISABLE = true;
                    }
                }

                _Retorno.Append("CREATE OR REPLACE ");

                if (p_TipoObj == "JAVA SOURCE")
                {
                    _Retorno.Append("AND COMPILE JAVA SOURCE NAMED " + p_OwnerObj + "." + p_NomeObj + " AS\n");
                }

                // Buscando o DDL ordenado por linha

                if (_ColocarMsgErro)
                {
                    _Query = "";
                    _Query = _Query + "SELECT SOU.TEXT, ";
                    _Query = _Query + "    SOU.LINE, ";
                    _Query = _Query + "    ERR.TEXT AS ERRO ";
                    _Query = _Query + "FROM ALL_SOURCE SOU ";
                    _Query = _Query + "    LEFT JOIN ALL_ERRORS ERR ";
                    _Query = _Query + "    ON SOU.OWNER = ERR.OWNER ";
                    _Query = _Query + "    AND SOU.NAME = ERR.NAME ";
                    _Query = _Query + "    AND SOU.TYPE = ERR.TYPE ";
                    _Query = _Query + "    AND SOU.LINE = ERR.LINE ";
                    _Query = _Query + "WHERE SOU.OWNER = '" + p_OwnerObj + "' ";
                    _Query = _Query + "    AND SOU.NAME = '" + p_NomeObj + "' ";
                    _Query = _Query + "    AND SOU.TYPE = '" + p_TipoObj + "' ";
                    _Query = _Query + "ORDER BY SOU.LINE, ERR.SEQUENCE ";
                }
                else
                {
                    _Query = "";
                    _Query = _Query + "SELECT SOU.TEXT, ";
                    _Query = _Query + "    SOU.LINE ";
                    _Query = _Query + "FROM ALL_SOURCE SOU ";
                    _Query = _Query + "WHERE SOU.OWNER = '" + p_OwnerObj + "' ";
                    _Query = _Query + "    AND SOU.NAME = '" + p_NomeObj + "' ";
                    _Query = _Query + "    AND SOU.TYPE = '" + p_TipoObj + "' ";
                    _Query = _Query + "ORDER BY SOU.LINE ";
                }
                _UltimaLinhaInserida = -1;

                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                if (_DataReader.Read())
                {
                    _Linha = _DataReader.GetString(0);
                    _Linha = _Linha.Trim().ToUpper();
                    _Linha = _Linha.Replace("\"", "");
                    if (_Linha.IndexOf(p_OwnerObj + ".") < 0)
                    {
                        _Linha = _Linha.Replace(p_NomeObj, p_OwnerObj + "." + p_NomeObj);
                    }
                    while (_Linha.IndexOf("  ") >= 0)
                    {
                        _Linha = _Linha.Replace("  ", " ");
                    }
                    if (_ColocarMsgErro)
                    {
                        if (!_DataReader.IsDBNull(2))
                        {
                            _MsgErro = _DataReader.GetString(2);
                            _MsgErro = _MsgErro.Replace("\r", "");
                            while (_MsgErro.IndexOf("  ") > -1)
                            {
                                _MsgErro = _MsgErro.Replace("  ", " ");
                            }
                            _MsgErro = _MsgErro.Replace("\n", "/");
                            _MsgErro = _MsgErro.Trim();
                            if (_MsgErro.Length > 0)
                            {
                                _Linha = _Linha + " -- ###ERRO### " + _MsgErro;
                            }
                        }
                    }
                    if (_UltimaLinhaInserida < _DataReader.GetDecimal(1))
                    {
                        _Retorno.Append(_Linha + "\n");
                        _UltimaLinhaInserida = _DataReader.GetDecimal(1);
                    }
                    
                }

                while (_DataReader.Read())
                {
                    _Linha = _DataReader.GetString(0);
                    if (_ColocarMsgErro)
                    {
                        if (!_DataReader.IsDBNull(2))
                        {
                            _MsgErro = _DataReader.GetString(2);
                            _MsgErro = _MsgErro.Replace("\r", "");
                            while (_MsgErro.IndexOf("  ") > -1)
                            {
                                _MsgErro = _MsgErro.Replace("  ", " ");
                            }
                            _MsgErro = _MsgErro.Replace("\n", "/");
                            _MsgErro = _MsgErro.Trim();
                            if (_MsgErro.Length > 0)
                            {
                                _Linha = _Linha.Replace("\r", "");
                                _Linha = _Linha.Replace("\n", "");
                                _Linha = _Linha + " -- ###ERRO### " + _MsgErro + "\n";
                            }
                        }
                    }
                    if (_UltimaLinhaInserida < _DataReader.GetDecimal(1))
                    {
                        _Retorno.Append(_Linha);
                        _UltimaLinhaInserida = _DataReader.GetDecimal(1);
                    }
                    
                }
                if (_Retorno.ToString().Substring(_Retorno.Length - 1) != "\n" && _Retorno.ToString().Substring(_Retorno.Length - 1) != "\r")
                {
                    _Retorno.Append("\n");
                }
                _Retorno.Append("/\n\n");
                _DataReader.Close();

                if (_ColocarDISABLE)
                {
                    _Split = _Retorno.ToString().Split("\n".ToCharArray(), StringSplitOptions.None);
                    for (int i = 0; i < _Split.Length; i++)
                    {
                        if (_Split[i].Trim().ToUpper().IndexOf("BEGIN") >= 0 || _Split[i].Trim().ToUpper().IndexOf("DECLARE") >= 0)
                        {
                            _Retorno = new StringBuilder();
                            for (int j = 0; j < i; j++)
                            {
                                _Retorno.Append(_Split[j] + "\n");
                            }
                            _Retorno.Append("DISABLE" + "\n");
                            for (int j = i; j < _Split.Length; j++)
                            {
                                _Retorno.Append(_Split[j] + "\n");
                            }
                            break;
                        }
                    }
                }

                if (p_TipoObj == "PACKAGE")
                {
                    // Buscando DDL do package body
                    _Retorno.Append("CREATE OR REPLACE ");

                    if (_ColocarMsgErro)
                    {
                        _Query = "";
                        _Query = _Query + "SELECT SOU.TEXT, ";
                        _Query = _Query + "    SOU.LINE, ";
                        _Query = _Query + "    ERR.TEXT AS ERRO ";
                        _Query = _Query + "FROM ALL_SOURCE SOU ";
                        _Query = _Query + "    LEFT JOIN ALL_ERRORS ERR ";
                        _Query = _Query + "    ON SOU.OWNER = ERR.OWNER ";
                        _Query = _Query + "    AND SOU.NAME = ERR.NAME ";
                        _Query = _Query + "    AND SOU.TYPE = ERR.TYPE ";
                        _Query = _Query + "    AND SOU.LINE = ERR.LINE ";
                        _Query = _Query + "WHERE SOU.OWNER = '" + p_OwnerObj + "' ";
                        _Query = _Query + "    AND SOU.NAME = '" + p_NomeObj + "' ";
                        _Query = _Query + "    AND SOU.TYPE = 'PACKAGE BODY' ";
                        _Query = _Query + "ORDER BY SOU.LINE, ERR.SEQUENCE ";
                    }
                    else
                    {
                        _Query = "";
                        _Query = _Query + "SELECT SOU.TEXT, ";
                        _Query = _Query + "    SOU.LINE ";
                        _Query = _Query + "FROM ALL_SOURCE SOU ";
                        _Query = _Query + "WHERE SOU.OWNER = '" + p_OwnerObj + "' ";
                        _Query = _Query + "    AND SOU.NAME = '" + p_NomeObj + "' ";
                        _Query = _Query + "    AND SOU.TYPE = 'PACKAGE BODY' ";
                        _Query = _Query + "ORDER BY SOU.LINE ";
                    }
                    _UltimaLinhaInserida = -1;

                    _Command.CommandText = _Query;
                    _DataReader = _Command.ExecuteReader();
                    if (_DataReader.Read())
                    {
                        _Linha = _DataReader.GetString(0);

                        _Linha = _Linha.Trim().ToUpper();
                        _Linha = _Linha.Replace("\"", "");
                        if (_Linha.IndexOf(p_OwnerObj + ".") < 0)
                        {
                            _Linha = _Linha.Replace(p_NomeObj, p_OwnerObj + "." + p_NomeObj);
                        }
                        while (_Linha.IndexOf("  ") >= 0)
                        {
                            _Linha = _Linha.Replace("  ", " ");
                        }
                        if (_ColocarMsgErro)
                        {
                            if (!_DataReader.IsDBNull(2))
                            {
                                _MsgErro = _DataReader.GetString(2);
                                _MsgErro = _MsgErro.Replace("\r", "");
                                while (_MsgErro.IndexOf("  ") > -1)
                                {
                                    _MsgErro = _MsgErro.Replace("  ", " ");
                                }
                                _MsgErro = _MsgErro.Replace("\n", "/");
                                _MsgErro = _MsgErro.Trim();
                                if (_MsgErro.Length > 0)
                                {
                                    _Linha = _Linha + " -- ###ERRO### " + _MsgErro;
                                }
                            }
                        }
                        if (_UltimaLinhaInserida < _DataReader.GetDecimal(1))
                        {
                            _Retorno.Append(_Linha + "\n");
                            _UltimaLinhaInserida = _DataReader.GetDecimal(1);
                        }
                        
                    }

                    while (_DataReader.Read())
                    {
                        _Linha = _DataReader.GetString(0);
                        if (_ColocarMsgErro)
                        {
                            if (!_DataReader.IsDBNull(2))
                            {
                                _MsgErro = _DataReader.GetString(2);
                                _MsgErro = _MsgErro.Replace("\r", "");
                                while (_MsgErro.IndexOf("  ") > -1)
                                {
                                    _MsgErro = _MsgErro.Replace("  ", " ");
                                }
                                _MsgErro = _MsgErro.Replace("\n", "/");
                                _MsgErro = _MsgErro.Trim();
                                if (_MsgErro.Length > 0)
                                {
                                    _Linha = _Linha.Replace("\r", "");
                                    _Linha = _Linha.Replace("\n", "");
                                    _Linha = _Linha + " -- ###ERRO### " + _MsgErro + "\n";
                                }
                            }
                        }
                        if (_UltimaLinhaInserida < _DataReader.GetDecimal(1))
                        {
                            _Retorno.Append(_Linha);
                            _UltimaLinhaInserida = _DataReader.GetDecimal(1);
                        }
                        
                    }
                    if (_Retorno.ToString().Substring(_Retorno.Length - 1) != "\n" && _Retorno.ToString().Substring(_Retorno.Length - 1) != "\r")
                    {
                        _Retorno.Append("\n");
                    }
                    _Retorno.Append("/\n\n");
                    _DataReader.Close();
                }

                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _Retorno.ToString() ;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ExtractDDL_ALL_SOURCE(p_Usuario, p_Senha, p_Database, p_OwnerObj, p_NomeObj, p_TipoObj, ref p_Mensagem);
                }
                else if (_Exception.Message.IndexOf("'OraOLEDB.Oracle'") > -1)
                {
                    MessageBox.Show("ERRO\nO provider OraOLEDB.Oracle não está registrado corretamente\nLocalize a DLL OraOLEDB11.dll ou versão equivalente na pasta de instalação do ORACLE CLIENT e registre como no exemplo abaixo\nregsvr32.exe C:\\oracle\\product\\11.2.0\\client_1\\bin\\OraOLEDB11.dll", "ExtractDDL_ALL_SOURCE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "";
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ExtractDDL_ALL_SOURCE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "";
                }
            }
        }

        private string ExtractDDL_JOB(string p_Usuario, string p_Senha, string p_Database, string p_OwnerObj, string p_NomeObj, ref string p_Mensagem)
        {
            string _Retorno = "";
            string _Query = "";
            p_Mensagem = "";

            // Abrindo conexão com o banco
            OleDbCommand _Command = new OleDbCommand();
            OleDbConnection _Connection = new OleDbConnection();
            OleDbDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "Provider=OraOLEDB.Oracle; User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database + ";ChunkSize=65535;"; // 65535 é o máximo. se colocar um parâmetro maior que isso vão vir menos caracteres.
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                // Procurando se existem fontes do objeto
                _Query = "";
                _Query = _Query + "SELECT ";
                _Query = _Query + "owner, "; // 0
                _Query = _Query + "job_name, "; // 1
                _Query = _Query + "program_owner, "; // 2
                _Query = _Query + "program_name, "; // 3
                _Query = _Query + "job_type, "; // 4
                _Query = _Query + "job_action, "; // 5
                _Query = _Query + "schedule_owner, "; // 6
                _Query = _Query + "schedule_name, "; // 7
                _Query = _Query + "repeat_interval, "; // 8
                _Query = _Query + "enabled, "; // 9
                _Query = _Query + "auto_drop, "; // 10
                _Query = _Query + "restartable, "; // 11
                _Query = _Query + "comments "; // 12
                _Query = _Query + "FROM ALL_SCHEDULER_JOBS ";
                _Query = _Query + "WHERE owner = '" + p_OwnerObj + "' ";
                _Query = _Query + "AND job_name = '" + p_NomeObj + "' ";
                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();
                _Retorno = "";
                if (_DataReader.Read())
                {
                    _Retorno = _Retorno + "BEGIN\n";
                    _Retorno = _Retorno + "   DBMS_SCHEDULER.CREATE_JOB (\n";
                    _Retorno = _Retorno + "   job_name           =>  '" + _DataReader.GetString(0) + "." + _DataReader.GetString(1) + "',\n";

                    if (_DataReader.IsDBNull(4)) // Se job_type é null, uso o program_owner.program_name
                    {
                        _Retorno = _Retorno + "   program_name       => '" + _DataReader.GetString(2) + "." + _DataReader.GetString(3) + "',\n";
                    }
                    else // Senão, uso o job_action
                    {
                        _Retorno = _Retorno + "   job_type           =>  '" + _DataReader.GetString(4) + "',\n";
                        _Retorno = _Retorno + "   job_action         =>  '" + _DataReader.GetString(5).Replace("'", "''") + "',\n";
                    }

                    if (_DataReader.IsDBNull(8)) // Se repeat_interval é null, uso o schedule_owner.schedule_name
                    {
                        if (!_DataReader.IsDBNull(6))
                        {
                            _Retorno = _Retorno + "   schedule_name      => '" + _DataReader.GetString(6) + "." + _DataReader.GetString(7) + "',\n";
                        }
                    }
                    else // Senão, uso repeat_interval
                    {
                        _Retorno = _Retorno + "   start_date         =>  SYSDATE,\n";
                        _Retorno = _Retorno + "   repeat_interval    =>  '" + _DataReader.GetString(8) + "',\n";
                        _Retorno = _Retorno + "   end_date           =>  null,\n";
                    }

                    _Retorno = _Retorno + "   enabled            =>  " + _DataReader.GetString(9) + ",\n";
                    //_Retorno = _Retorno + "   auto_drop          =>  " + _DataReader.GetString(10) + ",\n";
                    _Retorno = _Retorno + "   comments           =>  '" + _DataReader.GetString(12) + "');\n";
                    _Retorno = _Retorno + "END;\n";
                    _Retorno = _Retorno + "/\n\n";
                }
                else
                {
                    p_Mensagem = "Fonte do JOB " + p_OwnerObj + "." + p_NomeObj + " não encontrado";
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ExtractDDL_JOB(p_Usuario, p_Senha, p_Database, p_OwnerObj, p_NomeObj, ref p_Mensagem);
                }
                else if (_Exception.Message.IndexOf("'OraOLEDB.Oracle'") > -1)
                {
                    MessageBox.Show("ERRO\nO provider OraOLEDB.Oracle não está registrado corretamente\nLocalize a DLL OraOLEDB11.dll ou versão equivalente na pasta de instalação do ORACLE CLIENT e registre como no exemplo abaixo\nregsvr32.exe C:\\oracle\\product\\11.2.0\\client_1\\bin\\OraOLEDB11.dll", "ExtractDDL_JOB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "";
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ExtractDDL_JOB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "";
                }
            }
        }

        private string ExtractDDL_VIEW(string p_Usuario, string p_Senha, string p_Database, string p_OwnerObj, string p_NomeObj, ref string p_Mensagem)
        {
            string _Retorno = "";
            string _Query = "";
            p_Mensagem = "";

            // Abrindo conexão com o banco
            OleDbCommand _Command = new OleDbCommand();
            OleDbConnection _Connection = new OleDbConnection();
            OleDbDataReader _DataReader;
            string _lvwText = "";
            int _TamanhoEsperado = -1;

            try
            {
                _Connection.ConnectionString = "Provider=OraOLEDB.Oracle; User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database + ";ChunkSize=65535;"; // 65535 é o máximo. se colocar um parâmetro maior que isso vão vir menos caracteres.
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                _Retorno = _Retorno + "CREATE OR REPLACE VIEW " + p_OwnerObj + "." + p_NomeObj + " \n(\n";

                // Procurando as colunas da view
                _Query = "";
                _Query = _Query + "SELECT column_name ";
                _Query = _Query + "FROM ALL_TAB_COLUMNS ";
                _Query = _Query + "WHERE owner = '" + p_OwnerObj + "' ";
                _Query = _Query + "AND table_name = '" + p_NomeObj + "' ";
                _Query = _Query + "ORDER BY column_id ";
                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();

                while (_DataReader.Read())
                {
                    _Retorno = _Retorno + "   " + _DataReader.GetString(0) + ",\n";
                }
                _DataReader.Close();

                _Retorno = _Retorno.Substring(0, _Retorno.Length - 2) + "\n";

                _Retorno = _Retorno + ")\nAS\n";

                _Query = "";
                _Query = _Query + "SELECT owner, view_name, text, text_length ";
                _Query = _Query + "FROM ALL_VIEWS ";
                _Query = _Query + "WHERE owner = '" + p_OwnerObj + "' ";
                _Query = _Query + "AND view_name = '" + p_NomeObj + "' ";
                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();

                if (_DataReader.Read())
                {
                    _TamanhoEsperado = (int)_DataReader.GetDecimal(3);
                    _lvwText = _DataReader.GetString(2);
                    if (_lvwText.Length < _TamanhoEsperado)
                    {
                        OracleCommand _OCommand = new OracleCommand();
                        _OCommand.InitialLONGFetchSize = -1;
                        OracleConnection _OConnection = new OracleConnection();
                        OracleDataReader _ODataReader;

                        _OConnection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                        _OConnection.Open();
                        _OCommand.Connection = _OConnection;
                        _OCommand.CommandType = CommandType.Text;

                        _OCommand.CommandText = _Query;
                        _ODataReader = _OCommand.ExecuteReader();

                        if (_ODataReader.Read())
                        {
                            _TamanhoEsperado = (int)_ODataReader.GetDecimal(3);
                            _lvwText = _ODataReader.GetString(2);
                            if (_lvwText.Length < _TamanhoEsperado)
                            {
                                MessageBox.Show("Problemas para buscar o fonte da view " + p_OwnerObj + "." + p_NomeObj + "\n Eram esperados " + _TamanhoEsperado.ToString() + " caracteres e retornaram somente " + _lvwText.Length.ToString(), "ExtractDDL_VIEW", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        _ODataReader.Close();
                        _ODataReader.Dispose();
                        _OCommand.Dispose();
                        _OConnection.Close();
                        _OConnection.Dispose();
                        GC.Collect();
                    }
                    _Retorno = _Retorno + _lvwText + "\n";
                    _Retorno = _Retorno + "/\n\n";
                }
                else
                {
                    _Retorno = "";
                    p_Mensagem = "Fonte da View " + p_OwnerObj + "." + p_NomeObj + " não encontrado";
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ExtractDDL_VIEW(p_Usuario, p_Senha, p_Database, p_OwnerObj, p_NomeObj, ref p_Mensagem);
                }
                else if (_Exception.Message.IndexOf("'OraOLEDB.Oracle'") > -1)
                {
                    MessageBox.Show("ERRO\nO provider OraOLEDB.Oracle não está registrado corretamente\nLocalize a DLL OraOLEDB11.dll ou versão equivalente na pasta de instalação do ORACLE CLIENT e registre como no exemplo abaixo\nregsvr32.exe C:\\oracle\\product\\11.2.0\\client_1\\bin\\OraOLEDB11.dll", "ExtractDDL_VIEW", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "";
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ExtractDDL_VIEW", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "";
                }
            }
        }

        private string ExtractDDL_SEQUENCE(string p_Usuario, string p_Senha, string p_Database, string p_OwnerObj, string p_NomeObj, ref string p_Mensagem)
        {
            string _Retorno = "";
            string _Query = "";
            p_Mensagem = "";

            // Abrindo conexão com o banco
            OracleCommand _Command = new OracleCommand();
            _Command.InitialLONGFetchSize = -1;
            OracleConnection _Connection = new OracleConnection();
            OracleDataReader _DataReader;

            try
            {
                _Connection.ConnectionString = "User Id=" + p_Usuario + ";Password=" + p_Senha + ";Data Source=" + p_Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                _Retorno = _Retorno + "CREATE SEQUENCE " + p_OwnerObj + "." + p_NomeObj + " \n";

                // Procurando as sequence
                _Query = "";
                _Query = _Query + "Select last_number, "; // START WITH = 0
                _Query = _Query + "	   increment_by, "; // INCREMENT BY = 1
                _Query = _Query + "	   max_value, "; // MAXVALUE = 2
                _Query = _Query + "	   min_value, "; // MINVALUE = 3
                _Query = _Query + "	   cycle_flag, "; // CYCLE = 4
                _Query = _Query + "	   cache_size, "; // CACHE = 5
                _Query = _Query + "	   order_flag "; // ORDER = 6
                _Query = _Query + "From ALL_SEQUENCES ";
                _Query = _Query + "Where sequence_owner = '" + p_OwnerObj + "' ";
                _Query = _Query + "	  And sequence_name = '" + p_NomeObj + "' ";
                _Command.CommandText = _Query;
                _DataReader = _Command.ExecuteReader();

                if (_DataReader.Read())
                {
                    _Retorno = _Retorno + "  START WITH " + _DataReader.GetDecimal(0).ToString() + "\n";
                    _Retorno = _Retorno + "  INCREMENT BY " + _DataReader.GetDecimal(1).ToString() + "\n";
                    _Retorno = _Retorno + "  MAXVALUE " + _DataReader.GetDecimal(2).ToString() + "\n";
                    _Retorno = _Retorno + "  MINVALUE " + _DataReader.GetDecimal(3).ToString() + "\n";
                    if (_DataReader.GetString(4).Trim() == "N")
                    {
                        _Retorno = _Retorno + "  NOCYCLE\n";
                    }
                    else
                    {
                        _Retorno = _Retorno + "  CYCLE\n";
                    }

                    if (_DataReader.GetDecimal(5) == 0)
                    {
                        _Retorno = _Retorno + "  NOCACHE\n";
                    }
                    else
                    {
                        _Retorno = _Retorno + "  CACHE " + _DataReader.GetDecimal(5).ToString() + "\n";
                    }

                    if (_DataReader.GetString(6).Trim() == "N")
                    {
                        _Retorno = _Retorno + "  NOORDER\n";
                    }
                    else
                    {
                        _Retorno = _Retorno + "  ORDER\n";
                    }
                    _Retorno = _Retorno + "/\n\n";
                }
                else
                {
                    _Retorno = "";
                    p_Mensagem = "Fonte da SEQUENCE " + p_OwnerObj + "." + p_NomeObj + " não encontrado";
                }
                _DataReader.Close();
                _DataReader.Dispose();
                _Command.Dispose();
                _Connection.Close();
                _Connection.Dispose();
                GC.Collect();
                return _Retorno;
            }
            catch (Exception _Exception)
            {
                if (_Exception.Message.IndexOf("exceeded maximum idle time") > -1)
                {
                    this.FazConsultaDUAL(p_Usuario, p_Senha, p_Database);
                    return this.ExtractDDL_SEQUENCE(p_Usuario, p_Senha, p_Database, p_OwnerObj, p_NomeObj, ref p_Mensagem);
                }
                else
                {
                    MessageBox.Show("ERRO\n" + _Exception.ToString(), "ExtractDDL_SEQUENCE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        #endregion Extract DDL

    #endregion Métodos privados

    }

}
