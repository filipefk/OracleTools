using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Oracle_Tools
{
    public partial class frmExtractUser : Form
    {
    #region Campos privados
        
        private string _Username = "";
        private string _Password = "";
        private string _Database = "";
        private csOracle _csOracle = new csOracle();
        private bool _PreenchendoLvw = false;
        private csListViewColumnSorter lvwColumnSorter;

    #endregion

    #region Construtores

        public frmExtractUser()
        {
            InitializeComponent();
            _Username = "";
            _Password = "";
            _Database = "";
            this.PreencheComboOwners();
            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwColumnSorter = new csListViewColumnSorter();
            this.lvwUsuarios.ListViewItemSorter = lvwColumnSorter;
            this.Text = "Extract User Oracle - NÃO CONECTADO";
        }

        public frmExtractUser(string p_Username, string p_Password, string p_Database)
        {
            InitializeComponent();
            _Username = p_Username;
            _Password = p_Password;
            _Database = p_Database;
            this.PreencheComboOwners();
            this.PreencheComboProfiles();
            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwColumnSorter = new csListViewColumnSorter();
            this.lvwUsuarios.ListViewItemSorter = lvwColumnSorter;

            string _Mensagem = "";
            string _InfoBanco = "";
            _InfoBanco = _csOracle.InfoBanco(_Username, _Password, _Database, ref _Mensagem);
            if (_Mensagem.Length == 0)
            {
                this.Text = "Extract User Oracle - " + _InfoBanco;
            }

        }

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

    #region Métodos Privados

        private void PreencheComboProfiles()
        {
            ArrayList _Lista = _csOracle.ListaProfiles(_Username, _Password, _Database);
            if (_Lista != null)
            {
                if (_Lista.Count > 0)
                {
                    cboProfile.Items.Clear();
                    cboProfile.Items.Add("Todos");
                    foreach (string _String in _Lista)
                    {
                        cboProfile.Items.Add(_String);
                    }
                }
            }
        }

        private void PreencheComboOwners()
        {
            cboOwners.Items.Clear();
            cboOwners.Items.Add("Todos");
            cboOwners.Items.Add("Que seja Owner");
            cboOwners.Items.Add("Que não seja Owner");
        }

        private string SelecionarPasta()
        {
            string _PastaEscolhida = "";
            FolderBrowserDialog dlgCaminho = new FolderBrowserDialog();
            DialogResult Resp;

            string _PastaPadraoSalvar = (string)csUtil.CarregarPreferencia("PastaPadraoSalvarArquivos");
            //string _PastaPadraoSalvar = ;
            if (_PastaPadraoSalvar != null)
            {
                if (Directory.Exists(_PastaPadraoSalvar))
                {
                    dlgCaminho.SelectedPath = _PastaPadraoSalvar;
                }
                else
                {
                    dlgCaminho.SelectedPath = Directory.GetCurrentDirectory();
                }
            }
            else
            {
                dlgCaminho.SelectedPath = Directory.GetCurrentDirectory();
            }
            Resp = dlgCaminho.ShowDialog();
            if (Resp == System.Windows.Forms.DialogResult.OK)
            {
                _PastaEscolhida = dlgCaminho.SelectedPath;
                csUtil.SalvarPreferencia("PastaPadraoSalvarArquivos", _PastaEscolhida);
            }
            else
            {
                return "";
            }

            return _PastaEscolhida;
        }

        //private void CriarEstruturaPastas(string p_Caminho, ArrayList p_ListaOwners)
        //{
        //    p_Caminho = p_Caminho.Trim();
        //    if (Directory.Exists(p_Caminho))
        //    {
        //        if (p_Caminho.Substring(p_Caminho.Length - 1) != "\\")
        //        {
        //            p_Caminho = p_Caminho + "\\";
        //        }
        //        foreach (string _Owner in p_ListaOwners)
        //        {
        //            if (!Directory.Exists(p_Caminho + _Owner))
        //            {
        //                Directory.CreateDirectory(p_Caminho + _Owner);
        //                this.CriarEstruturaPastasTipos(p_Caminho + _Owner);
        //            }
        //        }
        //    }
        //}

        //private void CriarEstruturaPastasTipos(string p_Caminho)
        //{
        //    p_Caminho = p_Caminho.Trim();
        //    if (Directory.Exists(p_Caminho))
        //    {
        //        if (p_Caminho.Substring(p_Caminho.Length - 1) != "\\")
        //        {
        //            p_Caminho = p_Caminho + "\\";
        //        }
        //        foreach (string _Tipo in _TiposPermitidos)
        //        {
        //            if (!Directory.Exists(p_Caminho + _Tipo))
        //            {
        //                Directory.CreateDirectory(p_Caminho + _Tipo);
        //            }
        //        }
        //    }
        //}

        private bool SalvouScriptUser(string p_Caminho, csUsuarioBanco p_csUsuarioBanco)
        {
            bool _Retorno = false;

            p_Caminho = p_Caminho.Trim();
            if (p_Caminho.Substring(p_Caminho.Length - 1) != "\\")
            {
                p_Caminho = p_Caminho + "\\";
            }
            p_Caminho = p_Caminho + p_csUsuarioBanco.Nome + ".sql";
            try
            {
                File.WriteAllText(p_Caminho, p_csUsuarioBanco.ScriptCriacao, Encoding.Default);
                _Retorno = true;
            }
            catch (Exception _Exception)
            {
                MessageBox.Show("Problemas para salvar o arquivo\n" + p_Caminho + "\n" + _Exception.ToString(), "Salvou Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _Retorno = false;
            }

            return _Retorno;
        }

        private void MostraScriptUser(string p_NomeUser)
        {
            Cursor.Current = Cursors.WaitCursor;
            string _Mensagem = "";

            pbrStatus.Minimum = 0;
            pbrStatus.Maximum = 1;
            pbrStatus.Value = 0;
            pbrStatus.Value++;
            lblStatus.Text = "1 de 1 - Extraindo Script do usuário " + p_NomeUser;
            stStatusStrip.Refresh();
            //this.Refresh();
            Application.DoEvents();

            
            string _Fonte = _csOracle.ExtractDDLUser(_Username, _Password, _Database, p_NomeUser, ref _Mensagem);

            Cursor.Current = Cursors.Default;
            if (_Mensagem.Trim().Length > 0)
            {
                lblStatus.Text = "Parado";
                MessageBox.Show(_Mensagem, "Busca Script do Usuário", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                csUtil.SalvarEAbrir(_Fonte, p_NomeUser + ".sql");
                lblStatus.Text = "Parado";
            }
            pbrStatus.Value = 0;

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
                this.PreencheComboOwners();
                this.PreencheComboProfiles();

                string _Mensagem = "";
                string _InfoBanco = "";
                _InfoBanco = _csOracle.InfoBanco(_Username, _Password, _Database, ref _Mensagem);
                if (_Mensagem.Length == 0)
                {
                    this.Text = "Extract User Oracle - " + _InfoBanco;
                }

                return true;
            }
            else
            {
                return false;
            }
        }


    #endregion

    #region Eventos dos controles

        private void frmExtractUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btListarUsuarios_Click(object sender, EventArgs e)
        {
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }
            this.Show();
            string _Where = "";
            if (cboOwners.Text.Trim().Length == 0 && cboProfile.Text.Trim().Length == 0 && txtNomeUsuario.Text.Trim().Length == 0)
            {
                DialogResult _Resp;
                _Where = (string)csUtil.CarregarPreferencia("FiltroPesquisaUsuarios");
                _Resp = csUtil.InputBox("Deseja informar um filtro?", "Não colocar WHERE nem o primeiro AND", ref _Where);
                if (_Resp != DialogResult.OK)
                {
                    return;
                }
                else
                {
                    csUtil.SalvarPreferencia("FiltroPesquisaUsuarios", _Where);
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            lvwUsuarios.Items.Clear();
            chkSelecionarTodos.Checked = false;
            lblStatusLista.Text = lvwUsuarios.Items.Count.ToString() + " usuários listados - " + lvwUsuarios.CheckedItems.Count.ToString() + " usuários selecionados";
            lblStatusLista.Refresh();
            Application.DoEvents();
            lvwColumnSorter.SortColumn = 0;
            ArrayList _Lista = _csOracle.ListaUsuarios(_Username, _Password, _Database, cboOwners.Text, cboProfile.Text, txtNomeUsuario.Text, _Where);
            if (_Lista != null)
            {
                if (_Lista.Count > 0)
                {
                    csUsuarioBanco _csUsuarioBanco = null;
                    ListViewItem lvwItem = null;
                    ListViewItem.ListViewSubItem lvwSubItem = null;
                    _PreenchendoLvw = true;
                    for (int i = 0; i < _Lista.Count; i++)
                    {
                        _csUsuarioBanco = (csUsuarioBanco)_Lista[i];
                        lvwItem = lvwUsuarios.Items.Add(_csUsuarioBanco.Nome);
                        lvwItem.UseItemStyleForSubItems = false;
                        lvwItem.SubItems.Add(_csUsuarioBanco.DataCriacao.ToString("dd/MM/yy HH:mm:ss"));
                        lvwItem.SubItems.Add(_csUsuarioBanco.Profile);
                        lvwItem.SubItems.Add(_csUsuarioBanco.Status);

                        if (_csUsuarioBanco.DataLock > DateTime.MinValue)
                        {
                            lvwItem.SubItems.Add(_csUsuarioBanco.DataLock.ToString("dd/MM/yy HH:mm:ss"));
                        }
                        else
                        {
                            lvwSubItem = lvwItem.SubItems.Add(_csUsuarioBanco.DataLock.ToString("dd/MM/yy HH:mm:ss"));
                            lvwSubItem.ForeColor = Color.Transparent;
                            lvwSubItem.ForeColor = Color.Transparent;
                        }

                        if (_csUsuarioBanco.DataExpiracao > DateTime.MinValue)
                        {
                            lvwItem.SubItems.Add(_csUsuarioBanco.DataExpiracao.ToString("dd/MM/yy HH:mm:ss"));
                        }
                        else
                        {
                            lvwSubItem = lvwItem.SubItems.Add(_csUsuarioBanco.DataExpiracao.ToString("dd/MM/yy HH:mm:ss"));
                            lvwSubItem.ForeColor = Color.Transparent;
                            lvwSubItem.ForeColor = Color.Transparent;
                        }

                        if (_csUsuarioBanco.UltimoLogin > DateTime.MinValue)
                        {
                            lvwItem.SubItems.Add(_csUsuarioBanco.UltimoLogin.ToString("dd/MM/yy HH:mm:ss"));
                        }
                        else
                        {
                            lvwSubItem = lvwItem.SubItems.Add(_csUsuarioBanco.UltimoLogin.ToString("dd/MM/yy HH:mm:ss"));
                            lvwSubItem.ForeColor = Color.Transparent;
                            lvwSubItem.ForeColor = Color.Transparent;
                        }
                    }
                    _PreenchendoLvw = false;
                }
            }
            Cursor.Current = Cursors.Default;
            lblStatusLista.Text = lvwUsuarios.Items.Count.ToString() + " usuários listados - " + lvwUsuarios.CheckedItems.Count.ToString() + " usuários selecionados";
            lblStatusLista.Refresh();
            Application.DoEvents();
        }


        private void btEscolherPastaLvw_Click(object sender, EventArgs e)
        {
            string _PastaEscolhida = "";
            _PastaEscolhida = this.SelecionarPasta();
            if (_PastaEscolhida.Trim().Length > 0)
            {
                this.txtCaminhoLvw.Text = _PastaEscolhida;
            }
        }

        private void btEscolherPastaTxt_Click(object sender, EventArgs e)
        {
            string _PastaEscolhida = "";
            _PastaEscolhida = this.SelecionarPasta();
            if (_PastaEscolhida.Trim().Length > 0)
            {
                this.txtCaminhoTxt.Text = _PastaEscolhida;
            }
        }

        private void btExtrairLvw_Click(object sender, EventArgs e)
        {
            
            int _QuantUsuariosExtraidos = 0;
            string _Mensagens = "";
            DialogResult _Resposta;

            lblStatus.Text = "Validando lista de usuários";
            stStatusStrip.Refresh();
            //this.Refresh();
            Application.DoEvents();
            if (lvwUsuarios.CheckedItems.Count == 0)
            {
                MessageBox.Show("Nenhum usuário selecionado", "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Parado";
                return;
            }
            if (txtCaminhoLvw.Text.Trim().Length == 0)
            {
                MessageBox.Show("Informe o caminho", "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Parado";
                return;
            }

            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    lblStatus.Text = "Parado";
                    return;
                }
            }

            lblStatus.Text = "Verificando se existe a pasta informada";
            stStatusStrip.Refresh();
            Application.DoEvents();
            if (Directory.Exists(txtCaminhoLvw.Text))
            {
                csUtil.SalvarPreferencia("PastaPadraoSalvarArquivos", txtCaminhoLvw.Text);
                Cursor.Current = Cursors.WaitCursor;
                lblStatus.Text = "Criando estrutura de pastas";
                stStatusStrip.Refresh();
                Application.DoEvents();
                if (!Directory.Exists(txtCaminhoLvw.Text + "\\USUARIOS"))
                {
                    Directory.CreateDirectory(txtCaminhoLvw.Text + "\\USUARIOS");
                }
                
                string _Mensagem = "";
                string _ScriptCriacao = "";
                csUsuarioBanco _csUsuarioBanco = new csUsuarioBanco();

                pbrStatus.Minimum = 0;
                pbrStatus.Maximum = lvwUsuarios.CheckedItems.Count;
                pbrStatus.Value = 0;
                foreach (ListViewItem lvwItem in lvwUsuarios.CheckedItems)
                {
                    pbrStatus.Value++;
                    stStatusStrip.Refresh();
                    Application.DoEvents();
                    _csUsuarioBanco.Nome = lvwItem.Text;
                    _ScriptCriacao = "";

                    lblStatus.Text = pbrStatus.Value.ToString() + " de " + pbrStatus.Maximum.ToString() + " - Extraindo script do usuário " + _csUsuarioBanco.Nome;
                    stStatusStrip.Refresh();
                    Application.DoEvents();
                    
                    _ScriptCriacao = _csOracle.ExtractDDLUser(_Username, _Password, _Database, _csUsuarioBanco.Nome, ref _Mensagem);
                    

                    if (_Mensagem.Trim().Length > 0)
                    {
                        _Mensagens = _Mensagens + _Mensagem + "\n";
                        //MessageBox.Show("Problemas ao extrair DDL do objeto " + _ObjetoDeBanco.Owner + "." + _ObjetoDeBanco.Nome + "\n" + _Mensagem, "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //return;
                    }
                    else
                    {
                        _csUsuarioBanco.ScriptCriacao = _ScriptCriacao;
                        lblStatus.Text = (_QuantUsuariosExtraidos + 1).ToString() + " de " + pbrStatus.Maximum.ToString() + " - Salvando arquivo para o usuário " + _csUsuarioBanco.Nome;
                        stStatusStrip.Refresh();
                        Application.DoEvents();
                        if (this.SalvouScriptUser(txtCaminhoLvw.Text + "\\USUARIOS\\", _csUsuarioBanco))
                        {
                            _QuantUsuariosExtraidos++;
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                lblStatus.Text = "Parado";
                if (_Mensagens.Trim().Length > 0)
                {
                    _Resposta = MessageBox.Show("Extraídos " + _QuantUsuariosExtraidos + " usuários!\nDeseja ver as mensagens de erro?\n", "Extract Script Usuário", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (_Resposta == DialogResult.Yes)
                    {
                        MessageBox.Show(_Mensagens, "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Extraídos " + _QuantUsuariosExtraidos + " usuários!", "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                Process.Start("explorer.exe", txtCaminhoLvw.Text);
                pbrStatus.Value = 0;
            }
            else
            {
                MessageBox.Show("Caminho inválido\n" + txtCaminhoLvw.Text, "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btExtrairTxt_Click(object sender, EventArgs e)
        {

            int _QuantUsuariosExtraidos = 0;

            lblStatus.Text = "Validando lista de usuários";
            stStatusStrip.Refresh();
            Application.DoEvents();
            if (txtUsuarios.Text.Trim().Length == 0)
            {
                MessageBox.Show("Nenhum usuário informado", "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Parado";
                return;
            }
            
            string _TestaListaInvalida = txtUsuarios.Text.Trim().ToUpper();
            _TestaListaInvalida = _TestaListaInvalida.Replace("\n", "");
            _TestaListaInvalida = _TestaListaInvalida.Replace("\r", "");
            foreach (char _Char in Path.GetInvalidFileNameChars())
            {
                if (_TestaListaInvalida.IndexOf(_Char) > -1)
                {
                    MessageBox.Show("Lista de usuários inválida!", "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Parado";
                    return;
                }
            }

            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    lblStatus.Text = "Parado";
                    return;
                }
            }

            // Se tem só um nome de usuário no txtUsuarios.Text, extrai o script e joga na clipboard
            if ((txtUsuarios.Text.IndexOf("\n") < 0) && (txtUsuarios.Text.IndexOf("\r") < 0) && (txtUsuarios.Text.Trim().Length > 0))
            {
                this.MostraScriptUser(txtUsuarios.Text.Trim().ToUpper());
                return;
            }

            if (txtCaminhoTxt.Text.Trim().Length == 0)
            {
                MessageBox.Show("Informe o caminho", "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Parado";
                return;
            }

            lblStatus.Text = "Verificando se existe a pasta informada";
            stStatusStrip.Refresh();
            Application.DoEvents();
            if (Directory.Exists(txtCaminhoTxt.Text))
            {
                csUtil.SalvarPreferencia("PastaPadraoSalvarArquivos", txtCaminhoTxt.Text);
                Cursor.Current = Cursors.WaitCursor;
                string _TextoUsuarios = txtUsuarios.Text.Trim().ToUpper();
                string[] _SplitUsuarios;
                _TextoUsuarios = _TextoUsuarios.Replace("\r", "");
                _SplitUsuarios = _TextoUsuarios.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                lblStatus.Text = "Criando estrutura de pastas";
                stStatusStrip.Refresh();
                Application.DoEvents();
                if (!Directory.Exists(txtCaminhoTxt.Text + "\\USUARIOS"))
                {
                    Directory.CreateDirectory(txtCaminhoTxt.Text + "\\USUARIOS");
                }

                string _Mensagem = "";
                string _ScriptCriacao = "";
                csUsuarioBanco _csUsuarioBanco = new csUsuarioBanco();

                lblStatus.Text = "Montando lista de usuários";
                stStatusStrip.Refresh();
                Application.DoEvents();
                // Eliminando usuários duplicados
                ArrayList _ListaUsuarios = new ArrayList();
                foreach (string _strUsuario in _SplitUsuarios)
                {
                    if (_ListaUsuarios.IndexOf(_strUsuario) == -1)
                    {
                        _ListaUsuarios.Add(_strUsuario.Trim());
                    }
                }
                pbrStatus.Minimum = 0;
                pbrStatus.Maximum = _ListaUsuarios.Count;
                pbrStatus.Value = 0;
                foreach (string _strUsuario in _ListaUsuarios)
                {
                    pbrStatus.Value++;
                    stStatusStrip.Refresh();
                    Application.DoEvents();
                    _csUsuarioBanco.Nome = _strUsuario;
                    _ScriptCriacao = "";

                    lblStatus.Text = (_QuantUsuariosExtraidos + 1).ToString() + " de " + pbrStatus.Maximum.ToString() + " - Extraindo Script do usuário " + _csUsuarioBanco.Nome;
                    stStatusStrip.Refresh();
                    Application.DoEvents();
                    
                    _ScriptCriacao = _csOracle.ExtractDDLUser(_Username, _Password, _Database, _csUsuarioBanco.Nome, ref _Mensagem);
                    

                    if (_Mensagem.Trim().Length > 0)
                    {
                        MessageBox.Show("Problemas ao extrair script do usuário " + _csUsuarioBanco.Nome + "\n" + _Mensagem, "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        _csUsuarioBanco.ScriptCriacao = _ScriptCriacao;
                        lblStatus.Text = (_QuantUsuariosExtraidos + 1).ToString() + " de " + pbrStatus.Maximum.ToString() + " - Salvando arquivo para o usuário " + _csUsuarioBanco.Nome;
                        stStatusStrip.Refresh();
                        Application.DoEvents();
                        if (this.SalvouScriptUser(txtCaminhoTxt.Text, _csUsuarioBanco))
                        {
                            _QuantUsuariosExtraidos++;
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                lblStatus.Text = "Parado";
                MessageBox.Show("Extraídos " + _QuantUsuariosExtraidos + " usuários!", "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start("explorer.exe", txtCaminhoTxt.Text);
                pbrStatus.Value = 0;
            }
            else
            {
                MessageBox.Show("Caminho inválido\n" + txtCaminhoTxt.Text, "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Parado";
            }
        }

        private void lvwUsuarios_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!_PreenchendoLvw)
            {
                //bool _Achei = false;
                //foreach (string _Tipo in _TiposPermitidos)
                //{
                //    if (e.Item.SubItems[2].Text == _Tipo)
                //    {
                //        _Achei = true;
                //    }
                //}
                //if (!_Achei)
                //{
                //    _PreenchendoLvw = true;
                //    e.Item.Checked = false;
                //    MessageBox.Show("Tipo " + e.Item.SubItems[2].Text + " não suportado para extrair DDL", "Extract Script Usuário", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    _PreenchendoLvw = false;
                //}
                lblStatusLista.Text = lvwUsuarios.Items.Count.ToString() + " usuários listados - " + lvwUsuarios.CheckedItems.Count.ToString() + " usuários selecionados";
                lblStatusLista.Refresh();

                _PreenchendoLvw = true;
                if (lvwUsuarios.CheckedItems.Count == 0)
                {
                    chkSelecionarTodos.CheckState = CheckState.Unchecked;
                }
                else
                {
                    if (lvwUsuarios.CheckedItems.Count == lvwUsuarios.Items.Count)
                    {
                        chkSelecionarTodos.CheckState = CheckState.Checked;
                    }
                    else
                    {
                        chkSelecionarTodos.CheckState = CheckState.Indeterminate;
                    }
                }
                Application.DoEvents();
                _PreenchendoLvw = false;
            }
        }

        private void chkSelecionarTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (!_PreenchendoLvw && chkSelecionarTodos.CheckState != CheckState.Indeterminate)
            {
                _PreenchendoLvw = true;
                foreach (ListViewItem lvwItem in lvwUsuarios.Items)
                {
                    lvwItem.Checked = chkSelecionarTodos.Checked;
                }
                _PreenchendoLvw = false;
                lblStatusLista.Text = lvwUsuarios.Items.Count.ToString() + " usuários listados - " + lvwUsuarios.CheckedItems.Count.ToString() + " usuários selecionados";
                lblStatusLista.Refresh();
                Application.DoEvents();
            }
        }

        private void lvwUsuarios_DoubleClick(object sender, EventArgs e)
        {
            if (lvwUsuarios.SelectedItems.Count > 0)
            {
                string _NomeUser = lvwUsuarios.SelectedItems[0].Text;
                this.MostraScriptUser(_NomeUser);
            }
        }

        private void lvwUsuarios_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvwUsuarios.Sort();
        }

        private void lvwUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblStatusLista.Text = lvwUsuarios.Items.Count.ToString() + " usuários listados - " + lvwUsuarios.CheckedItems.Count.ToString() + " usuários selecionados";
            lblStatusLista.Refresh();
            Application.DoEvents();
        }

        private void txtUsuarios_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    txtUsuarios.SelectAll();
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void mnuArquivoConectar_Click(object sender, EventArgs e)
        {
            this.ConectouNoBanco();
        }

        private void mnuArquivoDetalhesUsuarios_Click(object sender, EventArgs e)
        {
            if (!this.EstaConectado)
            {
                if (this.ConectouNoBanco())
                {
                    frmDetalhesUser _frmDetalhesUser = new frmDetalhesUser(_Username, _Password, _Database, "");
                    _frmDetalhesUser.Show();
                }
            }
            else
            {
                frmDetalhesUser _frmDetalhesUser = new frmDetalhesUser(_Username, _Password, _Database, "");
                _frmDetalhesUser.Show();
            }
        }

        private void lvwUsuarios_KeyDown(object sender, KeyEventArgs e)
        {
            // Se pressionou CTLR + C, copia as informações dos itens selecionados da ListView para a área de transferência
            if (e.KeyCode == Keys.C)
            {
                if (e.Control)
                {
                    if (lvwUsuarios.SelectedItems.Count > 0)
                    {
                        string _ClipBoard = "";
                        ListViewItem lvwItem = null;
                        for (int i = 0; i < lvwUsuarios.Columns.Count; i++)
                        {
                            _ClipBoard = _ClipBoard + lvwUsuarios.Columns[i].Text + "\t";
                        }
                        _ClipBoard = _ClipBoard + "\r\n";

                        for (int i = 0; i < lvwUsuarios.SelectedItems.Count; i++)
                        {
                            lvwItem = lvwUsuarios.Items[lvwUsuarios.SelectedIndices[i]];
                            for (int j = 0; j < lvwItem.SubItems.Count; j++)
                            {
                                _ClipBoard = _ClipBoard + lvwItem.SubItems[j].Text + "\t";
                            }
                            _ClipBoard = _ClipBoard + "\r\n";
                        }
                        Clipboard.SetText(_ClipBoard);
                        //MessageBox.Show("Itens selecionados copiados para a área de transferência", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void infoBancoToolStripMenuItem_Click(object sender, EventArgs e)
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
        private void mnuContextoLvw_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string[] _Split;
            
            _Split = e.ClickedItem.Tag.ToString().Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string _NomeUser = _Split[1];

            switch (_Split[0])
            {
                case "SCRIPT":
                    
                    this.MostraScriptUser(_NomeUser);
                    break;

                case "PERMISSOES":

                    frmDetalhesUser _frmDetalhesUser = new frmDetalhesUser(_Username, _Password, _Database, _NomeUser);
                    _frmDetalhesUser.Show();
                    break;
            }

        }
        
    #endregion

        private void lvwUsuarios_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lvwUsuarios.SelectedItems.Count > 0)
                {
                    ToolStripItem _Menu = null;
                    Point mousePos = new Point(Cursor.Position.X, Cursor.Position.Y);

                    mnuContextoLvw.Items.Clear();
                    _Menu = mnuContextoLvw.Items.Add("Ver script de criação do usuário " + lvwUsuarios.SelectedItems[0].Text);
                    _Menu.Tag = "SCRIPT:" + lvwUsuarios.SelectedItems[0].Text;
                    _Menu = mnuContextoLvw.Items.Add("Ver permissões do usuário " + lvwUsuarios.SelectedItems[0].Text);
                    _Menu.Tag = "PERMISSOES:" + lvwUsuarios.SelectedItems[0].Text;
                    mnuContextoLvw.Show(mousePos);
                }
            }
        }

        

    }
}
