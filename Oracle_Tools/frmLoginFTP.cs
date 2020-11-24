using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using WinSCP;

namespace Oracle_Tools
{
    public partial class frmLoginFTP : Form
    {

        public class csDadosConexaoFTP
        {
            public string Apelido = "";
            public string Protocolo = "";
            public string Servidor = "";
            public string Usuario = "";
            public string Senha = "";
            public string SshHostKeyFingerprint = "";
        }

        #region Variáveis privadas

        csDadosConexaoFTP _ExcluirDadosConexao = null;

        private string _Protocolo = "";
        private string _Servidor = "";
        private string _Usuario = "";
        private string _Senha = "";
        private string _SshHostKeyFingerprint = "";
        private bool _TestarConexao = true;
        private string _StringFake = "[" + Environment.ProcessorCount.ToString() + Environment.MachineName + Environment.ProcessorCount.ToString() + "]";
        private ArrayList _ConexoesSalvas = new ArrayList();

        #endregion Variáveis privadas

        #region Construtores

        public frmLoginFTP(bool p_TestarConexao, ref string p_Protocolo, ref string p_HostName, ref string p_Usuario, ref string p_Senha, ref string p_SshHostKeyFingerprint)
        {
            InitializeComponent();
            this.PreencheComboProtocolos();
            _TestarConexao = p_TestarConexao;

            if (this.UltimoLoginUsuario != null)
            {
                txtUsername.Text = this.UltimoLoginUsuario;
            }
            if (this.UltimoLoginHostFtp != null)
            {
                txtHostName.Text = this.UltimoLoginHostFtp;
            }

            this.ShowDialog();

            p_Protocolo = _Protocolo;
            p_HostName = _Servidor;
            p_Usuario = _Usuario;
            p_Senha = _Senha;
            p_SshHostKeyFingerprint = _SshHostKeyFingerprint;
        }

        #endregion Construtores

        #region Propriedades Privadas

        /// <summary>
        /// Retorna ou salva a URL SVN do último login, que fica salvo no registro no caminho HKEY_CURRENT_USER\Software\SVN_Tools\Parametros\UltimoLoginUsuario
        /// </summary>
        private string UltimoLoginHostFtp
        {
            get
            {
                string _UltimoLoginUsuario;
                _UltimoLoginUsuario = (string)csUtil.CarregarPreferencia("UltimoLoginHostFtp");
                return _UltimoLoginUsuario;
            }
            set
            {
                string _UltimoLoginHostFtp;
                _UltimoLoginHostFtp = value;
                csUtil.SalvarPreferencia("UltimoLoginHostFtp", _UltimoLoginHostFtp);
            }
        }

        /// <summary>
        /// Retorna ou salva o nome do usuário do último login, que fica salvo no registro no caminho HKEY_CURRENT_USER\Software\SVN_Tools\Parametros\UltimoLoginUsuario
        /// </summary>
        private string UltimoLoginUsuario
        {
            get
            {
                string _UltimoLoginUsuario;
                _UltimoLoginUsuario = (string)csUtil.CarregarPreferencia("UltimoLoginUsuarioFTP");
                return _UltimoLoginUsuario;
            }
            set
            {
                string _UltimoLoginUsuario;
                _UltimoLoginUsuario = value;
                csUtil.SalvarPreferencia("UltimoLoginUsuarioFTP", _UltimoLoginUsuario);
            }
        }

    #endregion

        #region Métodos Privados

        private void PreencheComboProtocolos()
        {
            cboProtocolo.Items.Clear();
            cboProtocolo.Items.Add("FTP");
            cboProtocolo.Items.Add("SCP");
            cboProtocolo.Items.Add("SFTP");
            cboProtocolo.Items.Add("WEBDAV");
        }

        private bool SalvouDadosConexao()
        {
            csDadosConexaoFTP l_csDadosConexaoFTP;
            string _DadosAlinhados = "";
            string _DadosDesencriptados = "";
            string _DadosEncriptados = "";
            int _Pos = -1;
            int _Loops = -1;

            for (int i = _ConexoesSalvas.Count - 1; i >= 0; i--)
            {
                l_csDadosConexaoFTP = (csDadosConexaoFTP)_ConexoesSalvas[i];
                _DadosAlinhados = l_csDadosConexaoFTP.Apelido + "\n" + l_csDadosConexaoFTP.Protocolo + "\n" + l_csDadosConexaoFTP.Servidor + "\n" + l_csDadosConexaoFTP.Usuario + "\n" + l_csDadosConexaoFTP.Senha + "\n" + l_csDadosConexaoFTP.SshHostKeyFingerprint + "\n";
                _Loops = csUtil.GetRandomNumber(5, 11);
                for (int l = 0; l < _Loops; l++)
                {
                    _Pos = csUtil.GetRandomNumber(1, _DadosAlinhados.Length);
                    _DadosAlinhados = _DadosAlinhados.Substring(0, _Pos) + _StringFake + _DadosAlinhados.Substring(_Pos);
                }
                _DadosDesencriptados = _DadosDesencriptados + _DadosAlinhados;
            }

            _DadosEncriptados = csUtil.Encriptar(_DadosDesencriptados);
            csUtil.SalvarPreferencia("ConexoesFTP", _DadosEncriptados);
            return true;
        }

        private bool SalvouDadosConexao(string p_Protocolo, string p_Servidor, string p_Usuario, string p_Senha, string p_SshHostKeyFingerprint)
        {
            csDadosConexaoFTP l_csDadosConexaoFTP;
            string _Apelido = "";
            bool _JaTinha = false;
            

            this.BuscaDadosConexao();

            foreach (csDadosConexaoFTP _csDadosConexao in _ConexoesSalvas)
            {
                if (_csDadosConexao.Protocolo == p_Protocolo && _csDadosConexao.Servidor == p_Servidor && _csDadosConexao.Usuario == p_Usuario && _csDadosConexao.SshHostKeyFingerprint == p_SshHostKeyFingerprint)
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

            l_csDadosConexaoFTP = new csDadosConexaoFTP();

            foreach (csDadosConexaoFTP _csDadosConexao in _ConexoesSalvas)
            {
                if (_csDadosConexao.Apelido == _Apelido)
                {
                    _csDadosConexao.Protocolo = p_Protocolo;
                    _csDadosConexao.Servidor = p_Servidor;
                    _csDadosConexao.Usuario = p_Usuario;
                    _csDadosConexao.Senha = p_Senha;
                    _csDadosConexao.SshHostKeyFingerprint = p_SshHostKeyFingerprint;
                    l_csDadosConexaoFTP = _csDadosConexao;
                    _JaTinha = true;
                    break;
                }
            }

            if (_JaTinha)
            {
                _ConexoesSalvas.Remove(l_csDadosConexaoFTP);
                _ConexoesSalvas.Add(l_csDadosConexaoFTP);
            }
            else
            {
                l_csDadosConexaoFTP.Apelido = _Apelido;
                l_csDadosConexaoFTP.Protocolo = p_Protocolo;
                l_csDadosConexaoFTP.Servidor = p_Servidor;
                l_csDadosConexaoFTP.Usuario = p_Usuario;
                l_csDadosConexaoFTP.Senha = p_Senha;
                l_csDadosConexaoFTP.SshHostKeyFingerprint = p_SshHostKeyFingerprint;
                _ConexoesSalvas.Add(l_csDadosConexaoFTP);
            }

            return this.SalvouDadosConexao();
        }

        private void BuscaDadosConexao()
        {
            string _DadosAlinhados = "";
            string _DadosEncriptados = "";
            csDadosConexaoFTP _csDadosConexaoFTP = new csDadosConexaoFTP();
            int _Cont = 0;
            string[] _Split;

            _ConexoesSalvas = new ArrayList();

            _DadosEncriptados = (string)csUtil.CarregarPreferencia("ConexoesFTP");

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
                    _csDadosConexaoFTP = new csDadosConexaoFTP();
                    _csDadosConexaoFTP.Apelido = _Split[i];
                    _Cont++;
                }
                else if (_Cont == 1)
                {
                    _csDadosConexaoFTP.Protocolo = _Split[i];
                    _Cont++;
                }
                else if (_Cont == 2)
                {
                    _csDadosConexaoFTP.Servidor = _Split[i];
                    _Cont++;
                }
                else if (_Cont == 3)
                {
                    _csDadosConexaoFTP.Usuario = _Split[i];
                    _Cont++;
                }
                else if (_Cont == 4)
                {
                    _csDadosConexaoFTP.Senha = _Split[i];
                    if (_csDadosConexaoFTP.Senha.Trim() == "")
                    {
                        _csDadosConexaoFTP.Senha = "";
                    }
                    _Cont++;
                }
                else if (_Cont == 5)
                {
                    _csDadosConexaoFTP.SshHostKeyFingerprint = _Split[i];
                    _ConexoesSalvas.Add(_csDadosConexaoFTP);
                    _Cont = 0;
                }
            }
        }

        private bool ConexaoOK(string p_Protocolo, string p_HostName, string p_Usuario, string p_Senha, string p_SshHostKeyFingerprint)
        {
            bool _Resp = false;

            SessionOptions _SessionOptions = new SessionOptions();
            p_Protocolo = p_Protocolo.Trim().ToUpper();
            switch (p_Protocolo)
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

            _SessionOptions.HostName = p_HostName;
            _SessionOptions.UserName = p_Usuario;
            _SessionOptions.Password = p_Senha;
            _SessionOptions.SshHostKeyFingerprint = p_SshHostKeyFingerprint;

            Session _Session = new Session();

            try
            {
                _Session.Open(_SessionOptions);
                _Resp = true;
            }
            catch (Exception e)
            {
                _Resp = false;
                MessageBox.Show("Erro ao tentar conectar em " + p_HostName + "\n" + e.Message, "Testar Conexão FTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (_Session.Opened)
            {
                _Session.Close();
            }
            _Session.Dispose();
            return _Resp;
        }

        #endregion Métodos Privados

        #region Eventos dos controles

        private void btOK_Click(object sender, EventArgs e)
        {
            if (_TestarConexao)
            {

                if (this.ConexaoOK(cboProtocolo.Text, txtHostName.Text, txtUsername.Text, txtPassword.Text, txtFingerprint.Text))
                {

                    _Protocolo = cboProtocolo.Text;
                    _Servidor = txtHostName.Text;
                    _Usuario = txtUsername.Text;
                    _Senha = txtPassword.Text;
                    _SshHostKeyFingerprint = txtFingerprint.Text;

                    if (chkSalvarConexao.Checked)
                    {
                        if (chkSalvarSenha.Checked)
                        {
                            if (!this.SalvouDadosConexao(_Protocolo, _Servidor, _Usuario, _Senha, _SshHostKeyFingerprint))
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (!this.SalvouDadosConexao(_Protocolo, _Servidor, _Usuario, " ", _SshHostKeyFingerprint))
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
                _Protocolo = cboProtocolo.Text;
                _Servidor = txtHostName.Text;
                _Usuario = txtUsername.Text;
                _Senha = txtPassword.Text;
                _SshHostKeyFingerprint = txtFingerprint.Text;

                if (chkSalvarConexao.Checked)
                {
                    if (chkSalvarSenha.Checked)
                    {
                        if (!this.SalvouDadosConexao(_Protocolo, _Servidor, _Usuario, _Senha, _SshHostKeyFingerprint))
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (!this.SalvouDadosConexao(_Protocolo, _Servidor, _Usuario, " ", _SshHostKeyFingerprint))
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
            _Protocolo = "";
            _Servidor = "";
            _Usuario = "";
            _Senha = "";
            _SshHostKeyFingerprint = "";
            this.Hide();
        }

        private void cboConexoesSalvas_Click(object sender, EventArgs e)
        {
            csDadosConexaoFTP l_csDadosConexaoFTP = null;
            ToolStripItem _SubMenu = null;
            Point _Point = new Point(Cursor.Position.X + 12, Cursor.Position.Y);
            mnuMenu.Items.Clear();
            this.BuscaDadosConexao();

            if (_ConexoesSalvas.Count > 0)
            {
                for (int i = 0; i < _ConexoesSalvas.Count; i++)
                {
                    l_csDadosConexaoFTP = (csDadosConexaoFTP)_ConexoesSalvas[i];
                    _SubMenu = mnuMenu.Items.Add(l_csDadosConexaoFTP.Apelido);
                    _SubMenu.Tag = l_csDadosConexaoFTP;
                }
                mnuMenu.Show(_Point);
            }
        }

        private void mnuMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            csDadosConexaoFTP l_csDadosConexaoFTP = (csDadosConexaoFTP)e.ClickedItem.Tag;

            if (e.ClickedItem.Text.Length >= 9 && e.ClickedItem.Text.Substring(0, 9) == "EXCLUIR: ")
            {
                _ExcluirDadosConexao = l_csDadosConexaoFTP;
                tmrMenu.Enabled = true;
            }
            else
            {
                cboProtocolo.Text = l_csDadosConexaoFTP.Protocolo;
                txtHostName.Text = l_csDadosConexaoFTP.Servidor;
                txtUsername.Text = l_csDadosConexaoFTP.Usuario;
                txtPassword.Text = l_csDadosConexaoFTP.Senha;
                txtFingerprint.Text = l_csDadosConexaoFTP.SshHostKeyFingerprint;
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
                csDadosConexaoFTP l_csDadosConexaoFTP = null;
                ToolStripItem _SubMenu = null;
                Point _Point = new Point(Cursor.Position.X + 12, Cursor.Position.Y);
                mnuMenu.Items.Clear();
                this.BuscaDadosConexao();

                if (_ConexoesSalvas.Count > 0)
                {
                    for (int i = 0; i < _ConexoesSalvas.Count; i++)
                    {
                        l_csDadosConexaoFTP = (csDadosConexaoFTP)_ConexoesSalvas[i];
                        _SubMenu = mnuMenu.Items.Add("EXCLUIR: " + l_csDadosConexaoFTP.Apelido);
                        _SubMenu.Tag = l_csDadosConexaoFTP;
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
