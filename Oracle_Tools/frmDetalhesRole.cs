using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;

namespace Oracle_Tools
{
    public partial class frmDetalhesRole : Form
    {
    
    #region Campos privados

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

        public frmDetalhesRole()
        {
            InitializeComponent();
            _Username = "";
            _Password = "";
            _Database = "";
            this.Text = "Detalhes Role - NÃO CONECTADO";
        }

        public frmDetalhesRole(string p_Username, string p_Password, string p_Database, string p_NomeRole)
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
                this.Text = "Detalhes Role - " + _InfoBanco;
            }
            if (p_NomeRole.Trim().Length > 0)
            {
                txtNomeRole.Text = p_NomeRole;
                this.PesquisarRole();
            }
        }

    #endregion

    #region Eventos dos controles

        private void btPesquisar_Click(object sender, EventArgs e)
        {
            this.PesquisarRole();
        }

        private void trvRole_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Tag != null)
            {
                if (e.Node.Nodes[0].Tag.ToString() == "FAKE")
                {
                    TreeNode _Node = null;
                    ArrayList _Retorno = null;
                    string _NomeRole = "";
                    string _TipoObjRole = "";

                    Cursor.Current = Cursors.WaitCursor;

                    switch (e.Node.Nodes[0].Text)
                    {
                        #region case USUARIOS
                        case "USUARIOS":
                            _NomeRole = trvRole.Nodes[0].Text;
                            e.Node.Nodes.Clear();
                            _Retorno = _csOracle.ListaUsuariosRole(_Username, _Password, _Database, _NomeRole);
                            for (int i = 0; i < _Retorno.Count; i++)
                            {
                                _Node = e.Node.Nodes.Add(_Retorno[i].ToString());
                            }
                            e.Node.Expand();
                            break;
                        #endregion
                        #region case ROLE
                        case "ROLE":

                            _NomeRole = trvRole.Nodes[0].Text;
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

                            _NomeRole = trvRole.Nodes[0].Text;
                            _TipoObjRole = e.Node.Text;
                            e.Node.Nodes.Clear();
                            
                            _Retorno = _csOracle.ListaPrivRoleObj(_Username, _Password, _Database, _NomeRole, _TipoObjRole);
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
        
        private void btExpandirTudo_Click(object sender, EventArgs e)
        {
            if (trvRole.Nodes.Count > 0)
            {
                trvRole.Nodes[0].ExpandAll();
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

            if (trvRole.Nodes.Count > 0)
	        {
                _Relatorio = _csOracle.InfoBanco(_Username, _Password, _Database, ref _Mensagem) + "\n";
                _Relatorio = _Relatorio + trvRole.Nodes[0].FullPath + "\n";
                if (trvRole.Nodes[0].IsExpanded)
                {
                    _Relatorio = _Relatorio + this.RelatorioTree(trvRole.Nodes[0]);
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

            csUtil.SalvarEAbrir(_Relatorio, "Relatorio_Role_" + trvRole.Nodes[0].FullPath + ".txt");
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

        private void trvRole_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode _NodeClicked = trvRole.GetNodeAt(e.X, e.Y);
                try
                {
                    switch (_NodeClicked.Parent.Text)
                    {
                        case "PACKAGE":
                        case "FUNCTION":
                        case "SEQUENCE":
                        case "VIEW":
                        case "PROCEDURE":

                            int _Pos = -1;
                            string _OwnerNomeObj = _NodeClicked.Text;
                            string _TipoObjeto = _NodeClicked.Parent.Text;
                            string _Mensagem = "";

                            _Pos = _OwnerNomeObj.IndexOf("ON");
                            _OwnerNomeObj = _OwnerNomeObj.Substring(_Pos + 3);

                            Cursor.Current = Cursors.WaitCursor;

                            _csOracle.ExtractDDLTextEditor(_Username, _Password, _Database, _OwnerNomeObj);

                            //string _Fonte = _csOracle.ExtractDDL(_Username, _Password, _Database, _OwnerNomeObj, ref _TipoObjeto, ref _Mensagem);
                            Cursor.Current = Cursors.Default;


                            break;

                        case "Usuários":

                            string _NomeUsuario = _NodeClicked.Text;
                            _Pos = _NomeUsuario.IndexOf(":");
                            _NomeUsuario = _NomeUsuario.Substring(0, _Pos);

                            frmDetalhesUser _frmDetalhesUser = new frmDetalhesUser(_Username, _Password, _Database, _NomeUsuario);
                            _frmDetalhesUser.Show(this);

                            break;
                    }
                }
                catch (Exception)
                {
                    // SE DER ERRO NÃO FAZ NADA
                }
            }
        }

    #endregion Eventos dos controles

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

        private void PesquisarRole()
        {
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }

            string _Mensagem = "";
            string _NomeRole = txtNomeRole.Text.Trim().ToUpper();

            trvRole.Nodes.Clear();
            TreeNode _NodePai = trvRole.Nodes.Add(_NomeRole);
            TreeNode _Node = null;
            DateTime _DataCriacaoRole = _csOracle.DataCriacaoRole(_Username, _Password, _Database, _NomeRole, ref _Mensagem);

            if (_Mensagem.Trim().Length > 0)
            {
                MessageBox.Show("Problemas ao listar propriedades da role " + _NomeRole + "\n" + _Mensagem, "Pesquisar propriedades de uma Role", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                _Node = _NodePai.Nodes.Add("Criada em: " + _DataCriacaoRole.ToString("dd/MM/yy HH:mm:ss"));
            }

            _Node = _NodePai.Nodes.Add("Usuários");
            _Node = _Node.Nodes.Add("USUARIOS");
            _Node.Tag = "FAKE";

            _Node = _NodePai.Nodes.Add("Objetos");
            _Node = _Node.Nodes.Add("ROLE");
            _Node.Tag = "FAKE";
            
            _NodePai.Expand();

            txtNomeRole.SelectAll();
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
                    this.Text = "Detalhes Role - " + _InfoBanco;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

    #endregion Métodos Privados

    }
}
