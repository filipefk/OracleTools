using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.DirectoryServices;

namespace Oracle_Tools
{
    public partial class frmLoginAD : Form
    {
        public class csDadosConexaoAD
        {
            public string Apelido = "";
            public string Usuario = "";
            public string Senha = "";
            public string Dominio = "";
            public string Porta = "";
        }

        #region Variáveis privadas

        csDadosConexaoAD _ExcluirDadosConexao = null;

        private string _Usuario = "";
        private string _Senha = "";
        private string _Dominio = "";
        private string _Porta = "";
        private bool _TestarConexao = true;
        private string _StringFake = "[" + Environment.ProcessorCount.ToString() + Environment.MachineName + Environment.ProcessorCount.ToString() + "]";
        private ArrayList _ConexoesSalvas = new ArrayList();

        #endregion Variáveis privadas

        #region Construtores

        public frmLoginAD(bool p_TestarConexao, ref string p_Usuario, ref string p_Senha, ref string p_Dominio, ref string p_Porta)
        {
            InitializeComponent();

            _TestarConexao = p_TestarConexao;

            this.ShowDialog();

            p_Usuario = _Usuario;
            p_Senha = _Senha;
            p_Dominio = _Dominio;
            p_Porta = _Porta;
        }

        #endregion Construtores

        #region Métodos Privados

        private bool SalvouDadosConexao()
        {
            csDadosConexaoAD l_csDadosConexaoAD;
            string _DadosAlinhados = "";
            string _DadosDesencriptados = "";
            string _DadosEncriptados = "";
            int _Pos = -1;
            int _Loops = -1;

            for (int i = _ConexoesSalvas.Count - 1; i >= 0; i--)
            {
                l_csDadosConexaoAD = (csDadosConexaoAD)_ConexoesSalvas[i];
                _DadosAlinhados = l_csDadosConexaoAD.Apelido + "\n" + l_csDadosConexaoAD.Usuario + "\n" + l_csDadosConexaoAD.Senha + "\n" + l_csDadosConexaoAD.Dominio + "\n" + l_csDadosConexaoAD.Porta + "\n";
                _Loops = csUtil.GetRandomNumber(5, 11);
                for (int l = 0; l < _Loops; l++)
                {
                    _Pos = csUtil.GetRandomNumber(1, _DadosAlinhados.Length);
                    _DadosAlinhados = _DadosAlinhados.Substring(0, _Pos) + _StringFake + _DadosAlinhados.Substring(_Pos);
                }
                _DadosDesencriptados = _DadosDesencriptados + _DadosAlinhados;
            }

            _DadosEncriptados = csUtil.Encriptar(_DadosDesencriptados);
            csUtil.SalvarPreferencia("ConexoesAD", _DadosEncriptados);
            return true;
        }

        private bool SalvouDadosConexao(string p_Dominio, string p_Porta, string p_Usuario, string p_Senha)
        {
            csDadosConexaoAD l_csDadosConexaoAD;
            string _Apelido = "";
            bool _JaTinha = false;
            this.BuscaDadosConexao();

            foreach (csDadosConexaoAD _csDadosConexao in _ConexoesSalvas)
            {
                if (_csDadosConexao.Dominio == p_Dominio && _csDadosConexao.Porta == p_Porta && _csDadosConexao.Usuario == p_Usuario)
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

            l_csDadosConexaoAD = new csDadosConexaoAD();

            foreach (csDadosConexaoAD _csDadosConexao in _ConexoesSalvas)
            {
                if (_csDadosConexao.Apelido == _Apelido)
                {
                    _csDadosConexao.Dominio = p_Dominio;
                    _csDadosConexao.Porta = p_Porta;
                    _csDadosConexao.Usuario = p_Usuario;
                    _csDadosConexao.Senha = p_Senha;
                    l_csDadosConexaoAD = _csDadosConexao;
                    _JaTinha = true;
                    break;
                }
            }

            if (_JaTinha)
            {
                _ConexoesSalvas.Remove(l_csDadosConexaoAD);
                _ConexoesSalvas.Add(l_csDadosConexaoAD);
            }
            else
            {
                l_csDadosConexaoAD.Apelido = _Apelido;
                l_csDadosConexaoAD.Dominio = p_Dominio;
                l_csDadosConexaoAD.Porta = p_Porta;
                l_csDadosConexaoAD.Usuario = p_Usuario;
                l_csDadosConexaoAD.Senha = p_Senha;
                _ConexoesSalvas.Add(l_csDadosConexaoAD);
            }

            return this.SalvouDadosConexao();
        }

        private void BuscaDadosConexao()
        {
            string _DadosAlinhados = "";
            string _DadosEncriptados = "";
            csDadosConexaoAD _csDadosConexaoAD = new csDadosConexaoAD();
            int _Cont = 0;
            string[] _Split;

            _ConexoesSalvas = new ArrayList();

            _DadosEncriptados = (string)csUtil.CarregarPreferencia("ConexoesAD");

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
                    _csDadosConexaoAD = new csDadosConexaoAD();
                    _csDadosConexaoAD.Apelido = _Split[i];
                    _Cont++;
                }
                else if (_Cont == 1)
                {
                    _csDadosConexaoAD.Usuario = _Split[i];
                    _Cont++;
                }
                else if (_Cont == 2)
                {
                    _csDadosConexaoAD.Senha = _Split[i];
                    if (_csDadosConexaoAD.Senha.Trim() == "")
                    {
                        _csDadosConexaoAD.Senha = "";
                    }
                    _Cont++;
                }
                else if (_Cont == 3)
                {
                    _csDadosConexaoAD.Dominio = _Split[i];
                    _Cont++;
                }
                else if (_Cont == 4)
                {
                    _csDadosConexaoAD.Porta = _Split[i];
                    _ConexoesSalvas.Add(_csDadosConexaoAD);
                    _Cont = 0;
                }
            }
        }

        private bool ConexaoOK(string p_Usuario, string p_Senha, string p_Dominio, string p_Porta)
        {
            bool Resp = false;

            try
            {
                DirectoryEntry _DirectoryEntry = new DirectoryEntry("LDAP://" + p_Dominio + ":" + p_Porta, p_Usuario, p_Senha);
                DirectorySearcher _DirectorySearcher = new DirectorySearcher(_DirectoryEntry);

                _DirectorySearcher.PropertiesToLoad.Add("displayName");
                _DirectorySearcher.PropertiesToLoad.Add("SAMAccountName");
                _DirectorySearcher.Filter = "(|(displayName=TESTEDECONEXAO) (SAMAccountName=TESTEDECONEXAO))";

                SearchResult _SearchResult = _DirectorySearcher.FindOne();
                Resp = true;
            }
            catch (Exception)
            {

            }

            return Resp;
        }

        #endregion Métodos Privados

        #region Eventos dos controles

        private void btOK_Click(object sender, EventArgs e)
        {
            if (_TestarConexao)
            {

                if (this.ConexaoOK(txtUsuario.Text, txtSenha.Text, txtDominio.Text, txtPorta.Text))
                {
                    _Usuario = txtUsuario.Text;
                    _Senha = txtSenha.Text;
                    _Dominio = txtDominio.Text;
                    _Porta = txtPorta.Text;
                    if (chkSalvarConexao.Checked)
                    {
                        if (chkSalvarSenha.Checked)
                        {
                            if (!this.SalvouDadosConexao(_Dominio, _Porta, _Usuario, _Senha))
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (!this.SalvouDadosConexao(_Dominio, _Porta, _Usuario, " "))
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
                _Usuario = txtUsuario.Text;
                _Senha = txtSenha.Text;
                _Dominio = txtDominio.Text;
                _Porta = txtPorta.Text;

                if (chkSalvarConexao.Checked)
                {
                    if (chkSalvarSenha.Checked)
                    {
                        if (!this.SalvouDadosConexao(_Dominio, _Porta, _Usuario, _Senha))
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (!this.SalvouDadosConexao(_Dominio, _Porta, _Usuario, " "))
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
            _Dominio = "";
            _Porta = "";
            this.Hide();
        }

        private void btConexoesSalvas_Click(object sender, EventArgs e)
        {
            csDadosConexaoAD l_csDadosConexaoAD = null;
            ToolStripItem _SubMenu = null;
            Point _Point = new Point(Cursor.Position.X + 12, Cursor.Position.Y);
            mnuMenu.Items.Clear();
            this.BuscaDadosConexao();

            if (_ConexoesSalvas.Count > 0)
            {
                for (int i = 0; i < _ConexoesSalvas.Count; i++)
                {
                    l_csDadosConexaoAD = (csDadosConexaoAD)_ConexoesSalvas[i];
                    _SubMenu = mnuMenu.Items.Add(l_csDadosConexaoAD.Apelido);
                    _SubMenu.Tag = l_csDadosConexaoAD;
                }
                mnuMenu.Show(_Point);
            }
        }

        private void mnuMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            csDadosConexaoAD l_csDadosConexaoAD = (csDadosConexaoAD)e.ClickedItem.Tag;

            if (e.ClickedItem.Text.Length >= 9 && e.ClickedItem.Text.Substring(0, 9) == "EXCLUIR: ")
            {
                _ExcluirDadosConexao = l_csDadosConexaoAD;
                tmrMenu.Enabled = true;
            }
            else
            {
                txtUsuario.Text = l_csDadosConexaoAD.Usuario;
                txtSenha.Text = l_csDadosConexaoAD.Senha;
                txtDominio.Text = l_csDadosConexaoAD.Dominio;
                txtPorta.Text = l_csDadosConexaoAD.Porta;
                chkSalvarConexao.Checked = true;
                chkSalvarSenha.Checked = (txtSenha.Text.Length > 0);
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

        private void btConexoesSalvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                csDadosConexaoAD l_csDadosConexaoAD = null;
                ToolStripItem _SubMenu = null;
                Point _Point = new Point(Cursor.Position.X + 12, Cursor.Position.Y);
                mnuMenu.Items.Clear();
                this.BuscaDadosConexao();

                if (_ConexoesSalvas.Count > 0)
                {
                    for (int i = 0; i < _ConexoesSalvas.Count; i++)
                    {
                        l_csDadosConexaoAD = (csDadosConexaoAD)_ConexoesSalvas[i];
                        _SubMenu = mnuMenu.Items.Add("EXCLUIR: " + l_csDadosConexaoAD.Apelido);
                        _SubMenu.Tag = l_csDadosConexaoAD;
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
