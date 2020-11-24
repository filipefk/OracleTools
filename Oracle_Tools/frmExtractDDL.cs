using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Oracle_Tools
{
    public partial class frmExtractDDL : Form
    {
    #region Campos privados
        
        private string _Username = "";
        private string _Password = "";
        private string _Database = "";
        private csOracle _csOracle = new csOracle();
        private bool _PreenchendoLvw = false;
        private ArrayList _TiposPermitidos = new ArrayList();
        private csListViewColumnSorter lvwColumnSorter;
        private string _TagMenuContextoClicado = "";

    #endregion

    #region Construtores

        public frmExtractDDL()
        {
            InitializeComponent();
            this._PropriedadesPadrao();
            _Username = "";
            _Password = "";
            _Database = "";
            this.Text = "Extract DDL Oracle - NÃO CONECTADO";
        }

        public frmExtractDDL(string p_Username, string p_Password, string p_Database)
        {
            InitializeComponent();
            this._PropriedadesPadrao();
            _Username = p_Username;
            _Password = p_Password;
            _Database = p_Database;
            this.PreencheComboOwners();
            this.lvwObjetos.ListViewItemSorter = lvwColumnSorter;
            string _Mensagem = "";
            string _InfoBanco = "";
            _InfoBanco = _csOracle.InfoBanco(_Username, _Password, _Database, ref _Mensagem);
            if (_Mensagem.Length == 0)
            {
                this.Text = "Extract DDL Oracle - " + _InfoBanco;
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

        private void _PropriedadesPadrao()
        {
            this.PreencheListaTiposPermitidos();
            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwColumnSorter = new csListViewColumnSorter();
            this.lvwObjetos.ListViewItemSorter = lvwColumnSorter;

            object _ObjColocarCabecalhoAoExtrairDDL = csUtil.CarregarPreferencia("ColocarCabecalhoAoExtrairDDL");
            string _ColocarCabecalhoAoExtrairDDL = null;
            if (_ObjColocarCabecalhoAoExtrairDDL == null)
            {
                csUtil.SalvarPreferencia("ColocarCabecalhoAoExtrairDDL", "1");
            }
            else 
            {
                _ColocarCabecalhoAoExtrairDDL = _ObjColocarCabecalhoAoExtrairDDL.ToString().Trim().ToUpper();
                if (_ColocarCabecalhoAoExtrairDDL != "1" && _ColocarCabecalhoAoExtrairDDL != "0" && _ColocarCabecalhoAoExtrairDDL != "TRUE" && _ColocarCabecalhoAoExtrairDDL != "FALSE" && _ColocarCabecalhoAoExtrairDDL != "SIM" && _ColocarCabecalhoAoExtrairDDL != "NÃO")
                {
                    csUtil.SalvarPreferencia("ColocarCabecalhoAoExtrairDDL", "1");
                }
            }

            object _ObjColocarMensagemErroNaLinhaAoExtrairDDL = csUtil.CarregarPreferencia("ColocarMensagemErroNaLinhaAoExtrairDDL");
            string _ColocarMensagemErroNaLinhaAoExtrairDDL = null;
            if (_ObjColocarMensagemErroNaLinhaAoExtrairDDL == null)
            {
                csUtil.SalvarPreferencia("ColocarMensagemErroNaLinhaAoExtrairDDL", "1");
            }
            else
            {
                _ColocarMensagemErroNaLinhaAoExtrairDDL = _ObjColocarMensagemErroNaLinhaAoExtrairDDL.ToString().Trim().ToUpper();
                if (_ColocarMensagemErroNaLinhaAoExtrairDDL != "1" && _ColocarMensagemErroNaLinhaAoExtrairDDL != "0" && _ColocarMensagemErroNaLinhaAoExtrairDDL != "TRUE" && _ColocarMensagemErroNaLinhaAoExtrairDDL != "FALSE" && _ColocarMensagemErroNaLinhaAoExtrairDDL != "SIM" && _ColocarMensagemErroNaLinhaAoExtrairDDL != "NÃO")
                {
                    csUtil.SalvarPreferencia("ColocarMensagemErroNaLinhaAoExtrairDDL", "1");
                }
            }

        }

        private void PreencheListaTiposPermitidos()
        {
            _TiposPermitidos.Add("FUNCTION");
            _TiposPermitidos.Add("JOB");
            _TiposPermitidos.Add("PACKAGE");
            _TiposPermitidos.Add("PROCEDURE");
            _TiposPermitidos.Add("SEQUENCE");
            _TiposPermitidos.Add("TRIGGER");
            _TiposPermitidos.Add("VIEW");
            _TiposPermitidos.Add("JAVA SOURCE");

            cboTipoObjeto.Items.Clear();
            cboTipoObjeto.Items.Add("Todos");
            foreach (string _Tipo in _TiposPermitidos)
            {
                cboTipoObjeto.Items.Add(_Tipo);
            }
        }

        private void PreencheComboOwners()
        {
            ArrayList _Lista = _csOracle.ListaOwners(_Username, _Password, _Database);
            if (_Lista != null)
            {
                if (_Lista.Count > 0)
                {
                    cboOwners.Items.Clear();
                    cboOwners.Items.Add("Todos");
                    foreach (string _String in _Lista)
                    {
                        cboOwners.Items.Add(_String);
                    }
                }
            }
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

        private bool SalvouFonteObjeto(string p_Caminho, csObjetoDeBanco p_ObjetoDeBanco)
        {
            bool _Retorno = false;

            p_Caminho = p_Caminho.Trim();
            if (p_Caminho.Substring(p_Caminho.Length - 1) != "\\")
            {
                p_Caminho = p_Caminho + "\\";
            }
            p_Caminho = p_Caminho + p_ObjetoDeBanco.Owner + "\\" + p_ObjetoDeBanco.Tipo.Replace(" ", "_"); //+ "\\" + p_ObjetoDeBanco.NomeArquivo;
            if (!csUtil.CriouPasta(p_Caminho))
            {
                return false;
            }
            p_Caminho = p_Caminho + "\\" + p_ObjetoDeBanco.NomeArquivo;
            try
            {
                File.WriteAllText(p_Caminho, p_ObjetoDeBanco.Fonte, Encoding.Default);
                _Retorno = true;
            }
            catch (Exception _Exception)
            {
                MessageBox.Show("Problemas para salvar o arquivo\n" + p_Caminho + "\n" + _Exception.ToString(), "Salvou DDL Objeto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _Retorno = false;
            }

            return _Retorno;
        }

        private void BuscaObjClipboard(string p_OwnerNomeObj)
        {
            pbrStatus.Minimum = 0;
            pbrStatus.Maximum = 1;
            pbrStatus.Value = 0;
            pbrStatus.Value++;
            lblStatus.Text = "1 de 1 - Extraindo DDL do objeto " + p_OwnerNomeObj;
            stStatusStrip.Refresh();
            Application.DoEvents();

            _csOracle.ExtractDDLClipboard(_Username, _Password, _Database, p_OwnerNomeObj);

            lblStatus.Text = "Parado";
            pbrStatus.Value = 0;

        }

        private void BuscaObjEditorTXT(string p_OwnerNomeObj)
        {
            pbrStatus.Minimum = 0;
            pbrStatus.Maximum = 1;
            pbrStatus.Value = 0;
            pbrStatus.Value++;
            lblStatus.Text = "1 de 1 - Extraindo DDL do objeto " + p_OwnerNomeObj;
            stStatusStrip.Refresh();
            Application.DoEvents();

            _csOracle.ExtractDDLTextEditor(_Username, _Password, _Database, p_OwnerNomeObj);

            lblStatus.Text = "Parado";
            pbrStatus.Value = 0;

        }

        private void BuscaUsrClipboard(string p_NomeUsuario)
        {
            string _Mensagem = "";

            pbrStatus.Minimum = 0;
            pbrStatus.Maximum = 1;
            pbrStatus.Value = 0;
            pbrStatus.Value++;
            lblStatus.Text = "1 de 1 - Extraindo DDL do usuário " + p_NomeUsuario;
            stStatusStrip.Refresh();
            Application.DoEvents();

            Cursor.Current = Cursors.WaitCursor;
            string _Fonte = _csOracle.ExtractDDLUser(_Username, _Password, _Database, p_NomeUsuario, ref _Mensagem);
            Cursor.Current = Cursors.Default;

            if (_Mensagem.Trim().Length > 0)
            {
                lblStatus.Text = "Parado";
                MessageBox.Show(_Mensagem, "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Clipboard.SetData(DataFormats.Text, (object)_Fonte);
                lblStatus.Text = "Parado";
                MessageBox.Show("DDL do usuário " + p_NomeUsuario + " na área de transferência", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                this.PreencheListaTiposPermitidos();

                string _Mensagem = "";
                string _InfoBanco = "";
                _InfoBanco = _csOracle.InfoBanco(_Username, _Password, _Database, ref _Mensagem);
                if (_Mensagem.Length == 0)
                {
                    this.Text = "Extract DDL Oracle - " + _InfoBanco;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void ListarObjetosInvalidos()
        {
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            lvwObjetos.Items.Clear();
            chkSelecionarTodos.Checked = false;
            lblStatusLista.Text = lvwObjetos.Items.Count.ToString() + " itens listados - " + lvwObjetos.CheckedItems.Count.ToString() + " itens selecionados";
            lblStatusLista.Refresh();
            Application.DoEvents();
            lvwColumnSorter.SortColumn = 0;

            ArrayList _Lista = _csOracle.ListaObjetosInvalidos(_Username, _Password, _Database);
            if (_Lista != null)
            {
                if (_Lista.Count > 0)
                {
                    csObjetoDeBanco _csObjetoDeBanco = null;
                    ListViewItem lvwItem = null;
                    _PreenchendoLvw = true;
                    for (int i = 0; i < _Lista.Count; i++)
                    {
                        _csObjetoDeBanco = (csObjetoDeBanco)_Lista[i];
                        lvwItem = lvwObjetos.Items.Add(_csObjetoDeBanco.Owner);
                        lvwItem.SubItems.Add(_csObjetoDeBanco.Nome);
                        lvwItem.SubItems.Add(_csObjetoDeBanco.Tipo);
                        lvwItem.SubItems.Add(_csObjetoDeBanco.DataCriacao.ToString("dd/MM/yy HH:mm:ss"));
                        lvwItem.SubItems.Add(_csObjetoDeBanco.DataAlteracao.ToString("dd/MM/yy HH:mm:ss"));
                        lvwItem.SubItems.Add(_csObjetoDeBanco.Status);
                        lvwItem.SubItems.Add(_csObjetoDeBanco.MensagemErro);
                        lvwItem.SubItems.Add(_csObjetoDeBanco.UsuarioUltimaAlteracao);
                    }
                    _PreenchendoLvw = false;
                }
            }
            lblStatusLista.Text = lvwObjetos.Items.Count.ToString() + " itens listados - " + lvwObjetos.CheckedItems.Count.ToString() + " itens selecionados";
            Cursor.Current = Cursors.Default;
            lblStatusLista.Refresh();
            Application.DoEvents();
        }

        private void ListarObjetos()
        {
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }
            lvwObjetos.Items.Clear();
            chkSelecionarTodos.Checked = false;
            lblStatusLista.Text = lvwObjetos.Items.Count.ToString() + " itens listados - " + lvwObjetos.CheckedItems.Count.ToString() + " itens selecionados";
            lblStatusLista.Refresh();
            Application.DoEvents();
            lvwColumnSorter.SortColumn = 0;
            ArrayList _Lista = _csOracle.ListaObjetos(_Username, _Password, _Database, cboOwners.Text, cboTipoObjeto.Text, txtNomeObjeto.Text, _TiposPermitidos);
            if (_Lista != null)
            {
                if (_Lista.Count > 0)
                {
                    csObjetoDeBanco _csObjetoDeBanco = null;
                    ListViewItem lvwItem = null;
                    _PreenchendoLvw = true;
                    for (int i = 0; i < _Lista.Count; i++)
                    {
                        _csObjetoDeBanco = (csObjetoDeBanco)_Lista[i];
                        lvwItem = lvwObjetos.Items.Add(_csObjetoDeBanco.Owner);
                        lvwItem.SubItems.Add(_csObjetoDeBanco.Nome);
                        lvwItem.SubItems.Add(_csObjetoDeBanco.Tipo);
                        lvwItem.SubItems.Add(_csObjetoDeBanco.DataCriacao.ToString("dd/MM/yy HH:mm:ss"));
                        lvwItem.SubItems.Add(_csObjetoDeBanco.DataAlteracao.ToString("dd/MM/yy HH:mm:ss"));
                        lvwItem.SubItems.Add(_csObjetoDeBanco.Status);
                        lvwItem.SubItems.Add(""); // Mensagem de erro
                        lvwItem.SubItems.Add(_csObjetoDeBanco.UsuarioUltimaAlteracao);
                        //lblStatusLista.Text = lvwObjetos.Items.Count.ToString() + " itens listados - " + lvwObjetos.CheckedItems.Count.ToString() + " itens selecionados";
                        //lblStatusLista.Refresh();
                        //Application.DoEvents();
                    }
                    _PreenchendoLvw = false;
                }
                else
                {
                    MessageBox.Show("Nenum objeto encontrado!", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Nenum objeto encontrado!", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            lblStatusLista.Text = lvwObjetos.Items.Count.ToString() + " itens listados - " + lvwObjetos.CheckedItems.Count.ToString() + " itens selecionados";
            lblStatusLista.Refresh();
            Application.DoEvents();
        }

    #endregion Métodos Privados

    #region Eventos dos controles

        private void frmExtractDDL_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btListarObjetos_Click(object sender, EventArgs e)
        {
            this.ListarObjetos();
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
            
            int _QuantObjExtraidos = 0;
            string _Mensagens = "";
            DialogResult _Resposta;

            lblStatus.Text = "Validando lista de objetos";
            stStatusStrip.Refresh();
            //this.Refresh();
            Application.DoEvents();
            if (lvwObjetos.CheckedItems.Count == 0)
            {
                MessageBox.Show("Nenhum objeto selecionado", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Parado";
                return;
            }
            if (txtCaminhoLvw.Text.Trim().Length == 0)
            {
                MessageBox.Show("Informe o caminho", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            //this.Refresh();
            Application.DoEvents();
            if (Directory.Exists(txtCaminhoLvw.Text))
            {
                csUtil.SalvarPreferencia("PastaPadraoSalvarArquivos", txtCaminhoLvw.Text);
                ArrayList _ListaOwners = new ArrayList();

                lblStatus.Text = "Montando lista de Owners";
                stStatusStrip.Refresh();
                //this.Refresh();
                Application.DoEvents();
                foreach (ListViewItem lvwItem in lvwObjetos.CheckedItems)
                {
                    if (_ListaOwners.IndexOf(lvwItem.Text) == -1)
	                {
                        _ListaOwners.Add(lvwItem.Text);
	                }
                }

                //lblStatus.Text = "Criando estrutura de pastas";
                //stStatusStrip.Refresh();
                ////this.Refresh();
                //Application.DoEvents();
                //this.CriarEstruturaPastas(txtCaminhoLvw.Text, _ListaOwners);
                
                string _TipoObjeto = "";
                string _Mensagem = "";
                string _Fonte = "";
                csObjetoDeBanco _ObjetoDeBanco = new csObjetoDeBanco();

                pbrStatus.Minimum = 0;
                pbrStatus.Maximum = lvwObjetos.CheckedItems.Count;
                pbrStatus.Value = 0;
                foreach (ListViewItem lvwItem in lvwObjetos.CheckedItems)
                {
                    pbrStatus.Value++;
                    stStatusStrip.Refresh();
                    //this.Refresh();
                    Application.DoEvents();
                    _ObjetoDeBanco.Owner = lvwItem.Text;
                    _ObjetoDeBanco.Nome = lvwItem.SubItems[1].Text;
                    _ObjetoDeBanco.Tipo = lvwItem.SubItems[2].Text;
                    _Fonte = "";

                    lblStatus.Text = pbrStatus.Value.ToString() + " de " + pbrStatus.Maximum.ToString() + " - Extraindo DDL do objeto " + _ObjetoDeBanco.Owner + "." + _ObjetoDeBanco.Nome;
                    stStatusStrip.Refresh();
                    //this.Refresh();
                    Application.DoEvents();

                    if (_ObjetoDeBanco.Tipo == "JAVA CLASS" || _ObjetoDeBanco.Tipo == "TYPE")
                    {
                        _Mensagem = "Tipo de objeto " + _ObjetoDeBanco.Tipo + " não suportado";
                    }
                    else
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        _Fonte = _csOracle.ExtractDDL(_Username, _Password, _Database, _ObjetoDeBanco.Owner + "." + _ObjetoDeBanco.Nome, ref _TipoObjeto, ref _Mensagem);
                        Cursor.Current = Cursors.Default;
                    }

                    if (_Mensagem.Trim().Length > 0)
                    {
                        _Mensagens = _Mensagens + _Mensagem + "\n";
                        //MessageBox.Show("Problemas ao extrair DDL do objeto " + _ObjetoDeBanco.Owner + "." + _ObjetoDeBanco.Nome + "\n" + _Mensagem, "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //return;
                    }
                    else if (_Fonte.Trim().Length > 0)
                    {
                        _ObjetoDeBanco.Tipo = _TipoObjeto;
                        _ObjetoDeBanco.Fonte = _Fonte;
                        lblStatus.Text = (_QuantObjExtraidos + 1).ToString() + " de " + pbrStatus.Maximum.ToString() + " - Salvando arquivo para o objeto " + _ObjetoDeBanco.Owner + "." + _ObjetoDeBanco.Nome;
                        stStatusStrip.Refresh();
                        //this.Refresh();
                        Application.DoEvents();
                        if (this.SalvouFonteObjeto(txtCaminhoLvw.Text, _ObjetoDeBanco))
                        {
                            _QuantObjExtraidos++;
                        }
                    }
                }
                lblStatus.Text = "Parado";
                if (_Mensagens.Trim().Length > 0)
                {
                    _Resposta = MessageBox.Show("Extraídos " + _QuantObjExtraidos + " objetos!\nDeseja ver as mensagens de erro?\n", "Extract DDL", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (_Resposta == DialogResult.Yes)
                    {
                        MessageBox.Show(_Mensagens, "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Extraídos " + _QuantObjExtraidos + " objetos!", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                Process.Start("explorer.exe", txtCaminhoLvw.Text);
                pbrStatus.Value = 0;
            }
            else
            {
                MessageBox.Show("Caminho inválido\n" + txtCaminhoLvw.Text, "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btExtrairTxt_Click(object sender, EventArgs e)
        {
            
            int _QuantObjExtraidos = 0;

            lblStatus.Text = "Validando lista de objetos";
            stStatusStrip.Refresh();
            //this.Refresh();
            Application.DoEvents();
            if (txtObjetos.Text.Trim().Length == 0)
            {
                MessageBox.Show("Nenhum objeto informado", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Parado";
                return;
            }
            
            string _TestaListaInvalida = txtObjetos.Text.Trim().ToUpper();
            _TestaListaInvalida = _TestaListaInvalida.Replace("\n", "");
            _TestaListaInvalida = _TestaListaInvalida.Replace("\r", "");
            if (_TestaListaInvalida.IndexOf(".") == -1)
            {
                MessageBox.Show("Lista de objetos inválida!", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Parado";
                return;
            }
            foreach (char _Char in Path.GetInvalidFileNameChars())
            {
                if (_TestaListaInvalida.IndexOf(_Char) > -1)
                {
                    MessageBox.Show("Lista de objetos inválida!", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // Se tem só um nome de objeto no txtObjetos.Text, extrai o fonte e joga na clipboard
            if ((txtObjetos.Text.IndexOf("\n") < 0) && (txtObjetos.Text.IndexOf("\r") < 0) && (txtObjetos.Text.Trim().Length > 0) && (txtObjetos.Text.IndexOf(".") >= 0))
            {
                string[] _SplitUmObjeto;
                _SplitUmObjeto = txtObjetos.Text.Trim().Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                this.BuscaObjEditorTXT(_SplitUmObjeto[0] + "." + _SplitUmObjeto[1]);
                return;
            }

            if (txtCaminhoTxt.Text.Trim().Length == 0)
            {
                MessageBox.Show("Informe o caminho", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Parado";
                return;
            }

            lblStatus.Text = "Verificando se existe a pasta informada";
            stStatusStrip.Refresh();
            //this.Refresh();
            Application.DoEvents();
            if (Directory.Exists(txtCaminhoTxt.Text))
            {
                csUtil.SalvarPreferencia("PastaPadraoSalvarArquivos", txtCaminhoTxt.Text);
                string _TextoObjetos = txtObjetos.Text.Trim().ToUpper();
                string[] _SplitObjetos;
                string[] _SplitOwnerNome;
                _TextoObjetos = _TextoObjetos.Replace("\r", "");
                _SplitObjetos = _TextoObjetos.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                lblStatus.Text = "Montando lista de Owners";
                stStatusStrip.Refresh();
                //this.Refresh();
                Application.DoEvents();
                ArrayList _ListaOwners = new ArrayList();
                foreach (string _strObjeto in _SplitObjetos)
                {
                    _SplitOwnerNome = _strObjeto.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (_ListaOwners.IndexOf(_SplitOwnerNome[0]) == -1)
                    {
                        _ListaOwners.Add(_SplitOwnerNome[0]);
                    }
                }

                //lblStatus.Text = "Criando estrutura de pastas";
                //stStatusStrip.Refresh();
                ////this.Refresh();
                //Application.DoEvents();
                //this.CriarEstruturaPastas(txtCaminhoTxt.Text, _ListaOwners);

                string _TipoObjeto = "";
                string _Mensagem = "";
                string _Fonte = "";
                csObjetoDeBanco _ObjetoDeBanco = new csObjetoDeBanco();

                lblStatus.Text = "Montando lista de objetos";
                stStatusStrip.Refresh();
                //this.Refresh();
                Application.DoEvents();
                // Eliminando objetos duplicados
                ArrayList _ListaObjetos = new ArrayList();
                foreach (string _strObjeto in _SplitObjetos)
                {
                    if (_ListaObjetos.IndexOf(_strObjeto) == -1)
                    {
                        _ListaObjetos.Add(_strObjeto.Trim());
                    }
                }
                pbrStatus.Minimum = 0;
                pbrStatus.Maximum = _ListaObjetos.Count;
                pbrStatus.Value = 0;
                foreach (string _strObjeto in _ListaObjetos)
                {
                    pbrStatus.Value++;
                    stStatusStrip.Refresh();
                    //this.Refresh();
                    Application.DoEvents();
                    _SplitOwnerNome = _strObjeto.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    _ObjetoDeBanco.Owner = _SplitOwnerNome[0];
                    _ObjetoDeBanco.Nome = _SplitOwnerNome[1];
                    _Fonte = "";

                    lblStatus.Text = (_QuantObjExtraidos + 1).ToString() + " de " + pbrStatus.Maximum.ToString() + " - Extraindo DDL do objeto " + _ObjetoDeBanco.Owner + "." + _ObjetoDeBanco.Nome;
                    stStatusStrip.Refresh();
                    //this.Refresh();
                    Application.DoEvents();
                    Cursor.Current = Cursors.WaitCursor;
                    _Fonte = _csOracle.ExtractDDL(_Username, _Password, _Database, _ObjetoDeBanco.Owner + "." + _ObjetoDeBanco.Nome, ref _TipoObjeto, ref _Mensagem);
                    Cursor.Current = Cursors.Default;

                    if (_Mensagem.Trim().Length > 0)
                    {
                        MessageBox.Show("Problemas ao extrair DDL do objeto " + _ObjetoDeBanco.Owner + "." + _ObjetoDeBanco.Nome + "\n" + _Mensagem, "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (_Fonte.Trim().Length > 0)
                    {
                        _ObjetoDeBanco.Tipo = _TipoObjeto;
                        _ObjetoDeBanco.Fonte = _Fonte;
                        lblStatus.Text = (_QuantObjExtraidos + 1).ToString() + " de " + pbrStatus.Maximum.ToString() + " - Salvando arquivo para o objeto " + _ObjetoDeBanco.Owner + "." + _ObjetoDeBanco.Nome;
                        stStatusStrip.Refresh();
                        //this.Refresh();
                        Application.DoEvents();
                        if (this.SalvouFonteObjeto(txtCaminhoTxt.Text, _ObjetoDeBanco))
                        {
                            _QuantObjExtraidos++;
                        }
                    }
                }
                lblStatus.Text = "Parado";
                MessageBox.Show("Extraídos " + _QuantObjExtraidos + " objetos!", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start("explorer.exe", txtCaminhoTxt.Text);
                pbrStatus.Value = 0;
            }
            else
            {
                MessageBox.Show("Caminho inválido\n" + txtCaminhoTxt.Text, "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Parado";
            }
        }

        private void lvwObjetos_ItemChecked(object sender, ItemCheckedEventArgs e)
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
                //    MessageBox.Show("Tipo " + e.Item.SubItems[2].Text + " não suportado para extrair DDL", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    _PreenchendoLvw = false;
                //}
                lblStatusLista.Text = lvwObjetos.Items.Count.ToString() + " itens listados - " + lvwObjetos.CheckedItems.Count.ToString() + " itens selecionados";
                lblStatusLista.Refresh();

                _PreenchendoLvw = true;
                if (lvwObjetos.CheckedItems.Count == 0)
                {
                    chkSelecionarTodos.CheckState = CheckState.Unchecked;
                }
                else
                {
                    if (lvwObjetos.CheckedItems.Count == lvwObjetos.Items.Count)
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
                foreach (ListViewItem lvwItem in lvwObjetos.Items)
                {
                    lvwItem.Checked = chkSelecionarTodos.Checked;
                }
                _PreenchendoLvw = false;
                lblStatusLista.Text = lvwObjetos.Items.Count.ToString() + " itens listados - " + lvwObjetos.CheckedItems.Count.ToString() + " itens selecionados";
                lblStatusLista.Refresh();
                Application.DoEvents();
            }
        }

        private void lvwObjetos_DoubleClick(object sender, EventArgs e)
        {
            if (lvwObjetos.SelectedItems.Count > 0)
            {
                string _OwnerObj = lvwObjetos.SelectedItems[0].Text;
                string _NomeObj = lvwObjetos.SelectedItems[0].SubItems[1].Text;
                string _TipoObj = lvwObjetos.SelectedItems[0].SubItems[2].Text;
                if (_TipoObj == "JAVA CLASS" || _TipoObj == "TYPE")
                {
                    MessageBox.Show("Tipo de objeto " + _TipoObj + " não suportado", "Extract DDL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //_csOracle.ExtractDDLTextEditor(_Username, _Password, _Database, _OwnerObj + "." + _NomeObj);
                this.BuscaObjEditorTXT(_OwnerObj + "." + _NomeObj);
            }
        }

        private void lvwObjetos_ColumnClick(object sender, ColumnClickEventArgs e)
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
            this.lvwObjetos.Sort();
        }

        private void lvwObjetos_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblStatusLista.Text = lvwObjetos.Items.Count.ToString() + " itens listados - " + lvwObjetos.CheckedItems.Count.ToString() + " itens selecionados";
            lblStatusLista.Refresh();
            Application.DoEvents();
        }

        private void txtObjetos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    txtObjetos.SelectAll();
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void btUltimasCompilacoes_Click(object sender, EventArgs e)
        {
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            lvwObjetos.Items.Clear();
            cboOwners.Text = "";
            cboTipoObjeto.Text = "";
            txtNomeObjeto.Text = "";

            chkSelecionarTodos.Checked = false;
            lblStatusLista.Text = lvwObjetos.Items.Count.ToString() + " itens listados - " + lvwObjetos.CheckedItems.Count.ToString() + " itens selecionados";
            lblStatusLista.Refresh();
            Application.DoEvents();
            lvwColumnSorter.SortColumn = 0;
            int _QuantDias = 15;
            try
            {
                _QuantDias = int.Parse(txtDias.Text);
            }
            catch (Exception)
            {
                txtDias.Text = _QuantDias.ToString();
            }
            ArrayList _Lista = _csOracle.ListaUltimasCompilacoes(_Username, _Password, _Database, _QuantDias);
            if (_Lista != null)
            {
                if (_Lista.Count > 0)
                {
                    csObjetoDeBanco _csObjetoDeBanco = null;
                    ListViewItem lvwItem = null;
                    _PreenchendoLvw = true;
                    for (int i = 0; i < _Lista.Count; i++)
                    {
                        _csObjetoDeBanco = (csObjetoDeBanco)_Lista[i];
                        lvwItem = lvwObjetos.Items.Add(_csObjetoDeBanco.Owner);
                        lvwItem.SubItems.Add(_csObjetoDeBanco.Nome);
                        lvwItem.SubItems.Add(_csObjetoDeBanco.Tipo);
                        lvwItem.SubItems.Add(_csObjetoDeBanco.DataCriacao.ToString("dd/MM/yy HH:mm:ss"));
                        lvwItem.SubItems.Add(_csObjetoDeBanco.DataAlteracao.ToString("dd/MM/yy HH:mm:ss"));
                        lvwItem.SubItems.Add(_csObjetoDeBanco.Status);
                        lvwItem.SubItems.Add(""); // Mensagem de erro
                        lvwItem.SubItems.Add(_csObjetoDeBanco.UsuarioUltimaAlteracao);
                    }
                    _PreenchendoLvw = false;
                }
            }
            lblStatusLista.Text = lvwObjetos.Items.Count.ToString() + " itens listados - " + lvwObjetos.CheckedItems.Count.ToString() + " itens selecionados";
            Cursor.Current = Cursors.Default;
            lblStatusLista.Refresh();
            Application.DoEvents();
        }
        
        private void mnuArquivoConectar_Click(object sender, EventArgs e)
        {
            this.ConectouNoBanco();
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

        private void CopiarListaSelecionada()
        {
            if (lvwObjetos.SelectedItems.Count > 0)
            {
                StringBuilder _ClipBoard = new StringBuilder();
                ListViewItem lvwItem = null;
                for (int i = 0; i < lvwObjetos.Columns.Count; i++)
                {
                    _ClipBoard.Append(lvwObjetos.Columns[i].Text + "\t");
                }
                _ClipBoard.Append("\r\n");

                for (int i = 0; i < lvwObjetos.SelectedItems.Count; i++)
                {
                    lvwItem = lvwObjetos.Items[lvwObjetos.SelectedIndices[i]];
                    for (int j = 0; j < lvwItem.SubItems.Count; j++)
                    {
                        _ClipBoard.Append(lvwItem.SubItems[j].Text + "\t");
                    }
                    _ClipBoard.Append("\r\n");
                }
                Clipboard.SetText(_ClipBoard.ToString());
            }
        }

        private void CopiarTodaLista()
        {
            if (lvwObjetos.Items.Count > 0)
            {
                StringBuilder _ClipBoard = new StringBuilder();
                ListViewItem lvwItem = null;
                for (int i = 0; i < lvwObjetos.Columns.Count; i++)
                {
                    _ClipBoard.Append(lvwObjetos.Columns[i].Text + "\t");
                }
                _ClipBoard.Append("\r\n");

                for (int i = 0; i < lvwObjetos.Items.Count; i++)
                {
                    lvwItem = lvwObjetos.Items[i];
                    for (int j = 0; j < lvwItem.SubItems.Count; j++)
                    {
                        _ClipBoard.Append(lvwItem.SubItems[j].Text + "\t");
                    }
                    _ClipBoard.Append("\r\n");
                }
                Clipboard.SetText(_ClipBoard.ToString());
            }
        }

        private void btListarInvalidos_Click(object sender, EventArgs e)
        {
            this.ListarObjetosInvalidos();
            if (lvwObjetos.Items.Count == 0)
            {
                MessageBox.Show("Nenhum objeto inválido", "Listar Objetos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btRecompilarInvalidos_Click(object sender, EventArgs e)
        {
            bool _CompilouAlgo = false;
            string _Relatorio = "";
            DialogResult _Resposta;

            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }

            _csOracle.RecompilarObjetosInvalidos(_Username, _Password, _Database, ref _CompilouAlgo, ref _Relatorio);

            if (_CompilouAlgo)
            {
                _Resposta = MessageBox.Show("A quantidade de objetos inválidos mudou. Deseja atualizar a lista de objetos inválidos?", "Recompilar Objetos Inválidos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (_Resposta == DialogResult.Yes)
                {
                     this.ListarObjetosInvalidos();
                }
            }

            _Resposta = MessageBox.Show("Deseja visualizar o relatório de compilações?", "Recompilar Objetos Inválidos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (_Resposta == DialogResult.Yes)
            {
                csUtil.SalvarEAbrir(_Relatorio, "RelatorioRecompilacao.txt");
            }
        }


        private void lvwObjetos_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && lvwObjetos.SelectedItems.Count > 0)
            {
                ToolStripItem _Menu = null;

                Point mousePos = lvwObjetos.PointToClient(Control.MousePosition);
                ListViewHitTestInfo hitTest = lvwObjetos.HitTest(mousePos);
                string _OwnerNomeObj = hitTest.Item.Text + "." + hitTest.Item.SubItems[1].Text;
                mousePos = new Point(Cursor.Position.X + 10, Cursor.Position.Y - 10);

                //Point mousePos = new Point(Cursor.Position.X + 10, Cursor.Position.Y - 10);
                //string _OwnerNomeObj = lvwObjetos.SelectedItems[0].Text + "." + lvwObjetos.SelectedItems[0].SubItems[1].Text;
                mnuContextoLvw.Items.Clear();

                if (lvwObjetos.SelectedItems.Count == 1)
                {
                    _Menu = mnuContextoLvw.Items.Add("Extrair DDL do objeto " + _OwnerNomeObj);
                    _Menu.Tag = "EXTRACT_DDL:" + _OwnerNomeObj;

                    if (_csOracle.TemTabContrConc)
                    {
                        _Menu = mnuContextoLvw.Items.Add("Ver controle de concorrência do objeto " + _OwnerNomeObj);
                        _Menu.Tag = "CONTROLE_CONCORRENCIA:" + _OwnerNomeObj;
                    }

                    if (hitTest.Item.SubItems[6].Text.Trim().Length > 0)
                    {
                        _Menu = mnuContextoLvw.Items.Add("Mostrar Mensagem de erro");
                        _Menu.Tag = "MENSAGEM_DE_ERRO:" + hitTest.Item.SubItems[6].Text.Trim();
                    }
                    
                }

                _Menu = mnuContextoLvw.Items.Add("Copiar lista selecionada");
                _Menu.Tag = "COPIAR_SELECIONADA";

                _Menu = mnuContextoLvw.Items.Add("Copiar toda lista");
                _Menu.Tag = "COPIAR_TODA_LISTA";

                _Menu = mnuContextoLvw.Items.Add("-");
                _Menu.Tag = "SEPARADOR_01";

                _Menu = mnuContextoLvw.Items.Add("Informações do banco de dados");
                _Menu.Tag = "INFO_BANCO";

                mnuContextoLvw.Show(mousePos);
            }
        }

        private void mnuContextoLvw_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _TagMenuContextoClicado = e.ClickedItem.Tag.ToString();
            tmrTimerMenuContexto.Enabled = true;
        }

        private void tmrTimerMenuContexto_Tick(object sender, EventArgs e)
        {
            tmrTimerMenuContexto.Enabled = false;
            string[] _Split;

            _Split = _TagMenuContextoClicado.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            switch (_Split[0])
            {
                case "EXTRACT_DDL":
                    this.BuscaObjEditorTXT(_Split[1]);
                    break;

                case "CONTROLE_CONCORRENCIA":
                    frmControleConcorrencia _frmControleConcorrencia = new frmControleConcorrencia(_Username, _Password, _Database, "OBJETO:" + _Split[1]);
                    _frmControleConcorrencia.Show();
                    break;

                case "MENSAGEM_DE_ERRO":
                    string _Mensagem = _TagMenuContextoClicado.Substring("MENSAGEM_DE_ERRO".Length + 1);
                    MessageBox.Show(_Mensagem, "Mensagem de erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "COPIAR_SELECIONADA":
                    this.CopiarListaSelecionada();
                    break;

                case "COPIAR_TODA_LISTA":
                    this.CopiarTodaLista();
                    break;

                case "INFO_BANCO":
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
                    break;

            }
        }

        private void txtNomeObjeto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.ListarObjetos();
            }
        }

        private void mnuArquivoParametros_Click(object sender, EventArgs e)
        {
            DialogResult _Resp = DialogResult.Cancel;
            ArrayList _Parametros = new ArrayList();
            csUtil.stParametro _Parametro;
            string[] _ListaPreferencias = csUtil.ListaPreferencias();
            for (int i = 0; i < _ListaPreferencias.Length; i++)
            {
                _Parametro = new csUtil.stParametro();
                _Parametro.NomeParametro = _ListaPreferencias[i];
                _Parametro.ValoParametro = csUtil.CarregarPreferencia(_Parametro.NomeParametro).ToString();

                switch (_Parametro.NomeParametro)
                {
                    case "CaminhoExeTortoiseSVN":
                        _Parametro.NomeAmigavel = "Caminho do Executável do Tortoise SVN";
                        break;

                    case "CaminhoTnsNames":
                        _Parametro.NomeAmigavel = "Caminho do tnsnames.ora";
                        break;

                    case "ColocarCabecalhoAoExtrairDDL":
                        _Parametro.NomeAmigavel = "Colocar cabeçalho ao extrair DDL";
                        break;

                    case "ColocarMensagemErroNaLinhaAoExtrairDDL":
                        _Parametro.NomeAmigavel = "Colocar mensagem de erro na linha ao extrair DDL";
                        break;

                    case "ConexoesFTPSalvas":
                        _Parametro.NomeAmigavel = "Conexões FTP salvas encriptadas";
                        break;

                    case "ConexoesORACLE":
                        _Parametro.NomeAmigavel = "Conexões de banco ORACLE salvas encriptadas";
                        break;

                    case "ConexoesSVNSalvas":
                        _Parametro.NomeAmigavel = "Conexões SVN salvas encriptadas";
                        break;

                    case "FiltroPesquisaUsuarios":
                        _Parametro.NomeAmigavel = "Filtro padrão para pesquisa de usuário sem filtro";
                        break;

                    case "PastaPadraoSalvarArquivos":
                        _Parametro.NomeAmigavel = "Pasta padrão para salvar arquivos ao extrair DDL";
                        break;

                    case "UltimoLoginDatabase":
                        _Parametro.NomeAmigavel = "Último banco logado";
                        break;

                    case "UltimoLoginUsuario":
                        _Parametro.NomeAmigavel = "Último usuário logado";
                        break;

                    case "PastaImagensRelatoriosHtml":
                        _Parametro.NomeAmigavel = "Pasta de imagens para relatórios HTML";
                        break;

                    case "CaminhoListagemTrunk":
                        _Parametro.NomeAmigavel = "Caminho do arquivo de listagem do Trunk";
                        break;

                    case "CaminhoListagemBranches":
                        _Parametro.NomeAmigavel = "Caminho do arquivo de listagem dos branches";
                        break;
                }

                _Parametros.Add(_Parametro);
            }

            _Resp = csUtil.EditaParametros("Parâmetros do ORACLE Tools", "Lista de parâmetros do ORACLE Tools", ref _Parametros);
            if (_Resp == DialogResult.OK)
            {
                for (int i = 0; i < _Parametros.Count; i++)
                {
                    _Parametro = (csUtil.stParametro)_Parametros[i];
                    csUtil.SalvarPreferencia(_Parametro.NomeParametro, _Parametro.ValoParametro);
                }
            }
        }

        private void mnuArquivoInfoBanco_Click(object sender, EventArgs e)
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



    #endregion Eventos dos controles

        
    }
}
