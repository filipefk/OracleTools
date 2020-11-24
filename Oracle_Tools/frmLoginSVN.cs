using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using SharpSvn;
using System.Net;

namespace Oracle_Tools
{
    public partial class frmLoginSVN : Form
    {

        public class csDadosConexaoSVN
        {
            public string Apelido = "";
            public string Usuario = "";
            public string Senha = "";
            public string URL_SVN = "";
        }

        #region Variáveis privadas

        csDadosConexaoSVN _ExcluirDadosConexao = null;

        private string _Usuario = "";
        private string _Senha = "";
        private string _URL_SVN = "";
        private bool _TestarConexao = true;
        private string _StringFake = "[" + Environment.ProcessorCount.ToString() + Environment.MachineName + Environment.ProcessorCount.ToString() + "]";
        private ArrayList _ConexoesSalvas = new ArrayList();

        #endregion Variáveis privadas

        #region Construtores

        public frmLoginSVN(bool p_TestarConexao, ref string p_Usuario, ref string p_Senha, ref string p_URL_SVN)
        {
            InitializeComponent();

            _TestarConexao = p_TestarConexao;

            if (this.UltimoLoginUsuario != null)
            {
                txtUsername.Text = this.UltimoLoginUsuario;
            }
            if (this.UltimoLoginUrlSvn != null)
            {
                txtURL_SVN.Text = this.UltimoLoginUrlSvn;
            }

            this.ShowDialog();

            p_Usuario = _Usuario;
            p_Senha = _Senha;
            p_URL_SVN = _URL_SVN;
        }

        #endregion Construtores

        #region Propriedades Privadas

        /// <summary>
        /// Retorna ou salva a URL SVN do último login, que fica salvo no registro no caminho HKEY_CURRENT_USER\Software\SVN_Tools\Parametros\UltimoLoginUsuario
        /// </summary>
        private string UltimoLoginUrlSvn
        {
            get
            {
                string _UltimoLoginUsuario;
                _UltimoLoginUsuario = (string)csUtil.CarregarPreferencia("UltimoLoginUrlSvn");
                return _UltimoLoginUsuario;
            }
            set
            {
                string _UltimoLoginUrlSvn;
                _UltimoLoginUrlSvn = value;
                csUtil.SalvarPreferencia("UltimoLoginUrlSvn", _UltimoLoginUrlSvn);
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
                _UltimoLoginUsuario = (string)csUtil.CarregarPreferencia("UltimoLoginUsuarioSVN");
                return _UltimoLoginUsuario;
            }
            set
            {
                string _UltimoLoginUsuario;
                _UltimoLoginUsuario = value;
                csUtil.SalvarPreferencia("UltimoLoginUsuarioSVN", _UltimoLoginUsuario);
            }
        }

        #endregion

        #region Métodos Privados

        private bool SalvouDadosConexao()
        {
            csDadosConexaoSVN l_csDadosConexaoSVN;
            string _DadosAlinhados = "";
            string _DadosDesencriptados = "";
            string _DadosEncriptados = "";
            int _Pos = -1;
            int _Loops = -1;

            for (int i = _ConexoesSalvas.Count - 1; i >= 0; i--)
            {
                l_csDadosConexaoSVN = (csDadosConexaoSVN)_ConexoesSalvas[i];
                _DadosAlinhados = l_csDadosConexaoSVN.Apelido + "\n" + l_csDadosConexaoSVN.URL_SVN + "\n" + l_csDadosConexaoSVN.Usuario + "\n" + l_csDadosConexaoSVN.Senha + "\n";
                _Loops = csUtil.GetRandomNumber(5, 11);
                for (int l = 0; l < _Loops; l++)
                {
                    _Pos = csUtil.GetRandomNumber(1, _DadosAlinhados.Length);
                    _DadosAlinhados = _DadosAlinhados.Substring(0, _Pos) + _StringFake + _DadosAlinhados.Substring(_Pos);
                }
                _DadosDesencriptados = _DadosDesencriptados + _DadosAlinhados;
            }

            _DadosEncriptados = csUtil.Encriptar(_DadosDesencriptados);
            csUtil.SalvarPreferencia("ConexoesSVN", _DadosEncriptados);
            return true;
        }

        private bool SalvouDadosConexao(string p_URL_SVN, string p_Usuario, string p_Senha)
        {
            csDadosConexaoSVN l_csDadosConexaoSVN;
            string _Apelido = "";
            bool _JaTinha = false;

            this.BuscaDadosConexao();

            foreach (csDadosConexaoSVN _csDadosConexao in _ConexoesSalvas)
            {
                if (_csDadosConexao.URL_SVN == p_URL_SVN && _csDadosConexao.Usuario == p_Usuario)
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

            l_csDadosConexaoSVN = new csDadosConexaoSVN();

            foreach (csDadosConexaoSVN _csDadosConexao in _ConexoesSalvas)
            {
                if (_csDadosConexao.Apelido == _Apelido)
                {
                    _csDadosConexao.URL_SVN = p_URL_SVN;
                    _csDadosConexao.Usuario = p_Usuario;
                    _csDadosConexao.Senha = p_Senha;
                    l_csDadosConexaoSVN = _csDadosConexao;
                    _JaTinha = true;
                    break;
                }
            }

            if (_JaTinha)
            {
                _ConexoesSalvas.Remove(l_csDadosConexaoSVN);
                _ConexoesSalvas.Add(l_csDadosConexaoSVN);
            }
            else
            {
                l_csDadosConexaoSVN.Apelido = _Apelido;
                l_csDadosConexaoSVN.URL_SVN = p_URL_SVN;
                l_csDadosConexaoSVN.Usuario = p_Usuario;
                l_csDadosConexaoSVN.Senha = p_Senha;
                _ConexoesSalvas.Add(l_csDadosConexaoSVN);
            }

            return this.SalvouDadosConexao();
        }

        private void BuscaDadosConexao()
        {
            string _DadosAlinhados = "";
            string _DadosEncriptados = "";
            csDadosConexaoSVN _csDadosConexaoSVN = new csDadosConexaoSVN();
            int _Cont = 0;
            string[] _Split;

            _ConexoesSalvas = new ArrayList();

            _DadosEncriptados = (string)csUtil.CarregarPreferencia("ConexoesSVN");

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
                    _csDadosConexaoSVN = new csDadosConexaoSVN();
                    _csDadosConexaoSVN.Apelido = _Split[i];
                    _Cont++;
                }
                else if (_Cont == 1)
                {
                    _csDadosConexaoSVN.URL_SVN = _Split[i];
                    _Cont++;
                }
                else if (_Cont == 2)
                {
                    _csDadosConexaoSVN.Usuario = _Split[i];
                    _Cont++;
                }
                else if (_Cont == 3)
                {
                    _csDadosConexaoSVN.Senha = _Split[i];
                    if (_csDadosConexaoSVN.Senha.Trim() == "")
                    {
                        _csDadosConexaoSVN.Senha = "";
                    }
                    _ConexoesSalvas.Add(_csDadosConexaoSVN);
                    _Cont = 0;
                }
            }
        }

        private bool ConexaoOK(string p_URL_SVN, string p_Usuario_SVN, string p_Senha_SVN)
        {
            bool Resp = false;

            SvnClient _SvnClient = new SvnClient();
            SvnTarget _SvnTarget = SvnTarget.FromString(p_URL_SVN);
            _SvnClient.Authentication.DefaultCredentials = new NetworkCredential(p_Usuario_SVN, p_Senha_SVN);
            System.Collections.ObjectModel.Collection<SvnListEventArgs> _Lista;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    if (_SvnClient.GetList(_SvnTarget, out _Lista))
                    {
                        Resp = true;
                    }
                }
                catch (Exception)
                {

                }
            }
            return Resp;
        }

    #endregion

    #region Eventos dos controles
        
        private void btOK_Click(object sender, EventArgs e)
        {
            if (_TestarConexao)
            {

                if (this.ConexaoOK(txtURL_SVN.Text, txtUsername.Text, txtPassword.Text))
                {
                    _Usuario = txtUsername.Text;
                    _Senha = txtPassword.Text;
                    _URL_SVN = txtURL_SVN.Text;

                    if (chkSalvarConexao.Checked)
                    {
                        if (chkSalvarSenha.Checked)
                        {
                            if (!this.SalvouDadosConexao(_URL_SVN, _Usuario, _Senha))
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (!this.SalvouDadosConexao(_URL_SVN, _Usuario, " "))
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
                _URL_SVN = txtURL_SVN.Text;
                if (chkSalvarConexao.Checked)
                {
                    if (chkSalvarSenha.Checked)
                    {
                        if (!this.SalvouDadosConexao(_URL_SVN, _Usuario, _Senha))
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (!this.SalvouDadosConexao(_URL_SVN, _Usuario, " "))
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
            _URL_SVN = "";
            this.Hide();
        }

        private void cboConexoesSalvas_Click(object sender, EventArgs e)
        {
            csDadosConexaoSVN l_csDadosConexaoSVN = null;
            ToolStripItem _SubMenu = null;
            Point _Point = new Point(Cursor.Position.X + 12, Cursor.Position.Y);
            mnuMenu.Items.Clear();
            this.BuscaDadosConexao();

            if (_ConexoesSalvas.Count > 0)
            {
                for (int i = 0; i < _ConexoesSalvas.Count; i++)
                {
                    l_csDadosConexaoSVN = (csDadosConexaoSVN)_ConexoesSalvas[i];
                    _SubMenu = mnuMenu.Items.Add(l_csDadosConexaoSVN.Apelido);
                    _SubMenu.Tag = l_csDadosConexaoSVN;
                }
                mnuMenu.Show(_Point);
            }
        }

        private void mnuMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            csDadosConexaoSVN l_csDadosConexaoSVN = (csDadosConexaoSVN)e.ClickedItem.Tag;

            if (e.ClickedItem.Text.Length >= 9 && e.ClickedItem.Text.Substring(0, 9) == "EXCLUIR: ")
            {
                _ExcluirDadosConexao = l_csDadosConexaoSVN;
                tmrMenu.Enabled = true;
            }
            else
            {
                txtURL_SVN.Text = l_csDadosConexaoSVN.URL_SVN;
                txtUsername.Text = l_csDadosConexaoSVN.Usuario;
                txtPassword.Text = l_csDadosConexaoSVN.Senha;
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
                csDadosConexaoSVN l_csDadosConexaoSVN = null;
                ToolStripItem _SubMenu = null;
                Point _Point = new Point(Cursor.Position.X + 12, Cursor.Position.Y);
                mnuMenu.Items.Clear();
                this.BuscaDadosConexao();

                if (_ConexoesSalvas.Count > 0)
                {
                    for (int i = 0; i < _ConexoesSalvas.Count; i++)
                    {
                        l_csDadosConexaoSVN = (csDadosConexaoSVN)_ConexoesSalvas[i];
                        _SubMenu = mnuMenu.Items.Add("EXCLUIR: " + l_csDadosConexaoSVN.Apelido);
                        _SubMenu.Tag = l_csDadosConexaoSVN;
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

    #endregion

    }
}
