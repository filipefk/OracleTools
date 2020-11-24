using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.IO;

namespace Oracle_Tools
{
    public partial class frmSessoes : Form
    {
        #region Campos privados

        private string _Username = "";
        private string _Password = "";
        private string _Database = "";
        private csOracle _csOracle = new csOracle();
        private bool _PreenchendoLvw = false;
        private csListViewColumnSorter lvwColumnSorter;
        private string _TagMenuContextoClicado = "";
        private string _NomeArquivoListaUsuarios = csUtil.PastaLocalExecutavel() + "Lista_Usuarios2.txt";
        private string _ConteudoArquivoUsuarios = "";
        Dictionary<string, string> _ListaNomes = new Dictionary<string, string>();
        private StringBuilder _ConteudoArquivoListaNomes = new StringBuilder();

        #endregion

        public frmSessoes()
        {
            InitializeComponent();
            _Username = "";
            _Password = "";
            _Database = "";
            this.Text = "Sessões - NÃO CONECTADO";
            lvwColumnSorter = new csListViewColumnSorter();
        }

        public frmSessoes(string p_Username, string p_Password, string p_Database)
        {
            InitializeComponent();
            _Username = p_Username;
            _Password = p_Password;
            _Database = p_Database;
            lvwSessoes.ListViewItemSorter = lvwColumnSorter;
            string _Mensagem = "";
            string _InfoBanco = "";
            _InfoBanco = _csOracle.InfoBanco(_Username, _Password, _Database, ref _Mensagem);
            if (_Mensagem.Length == 0)
            {
                this.Text = "Sessões - " + _InfoBanco;
            }
            lvwColumnSorter = new csListViewColumnSorter();
        }

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
                    this.Text = "Sessões - " + _InfoBanco;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void mnuArquivoConectar_Click(object sender, EventArgs e)
        {
            this.ConectouNoBanco();
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

        private void AtualizarListaSessoes()
        {
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }

            _PreenchendoLvw = true;
            chkSelecionarTodos.Checked = false;

            string _Query = @"SELECT SID, 
                                   SERIAL#,
                                   USERNAME,
                                   ' ' As NOME_USERNAME,
                                   OSUSER,
                                   ' ' As NOME_OSUSER,
                                   MACHINE,
                                   STATUS,
                                   PROGRAM,
                                   TYPE,
                                   MODULE,
                                   LOGON_TIME,
                                   STATE
                            FROM GV$SESSION";
            string _Mensagem = "";
            lvwColumnSorter.SortColumn = 0;
            lvwSessoes.ListViewItemSorter = null;
            lvwSessoes.Items.Clear();

            _csOracle.PreencheLvw(_Username, _Password, _Database, _Query, true, ref _Mensagem, ref lvwSessoes);

            if (_Mensagem.Trim().Length > 0)
            {
                MessageBox.Show("Problemas ao buscar sessões ativas\n" + _Mensagem, "Atualizar Lista de Sessões", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

            lvwSessoes.Columns[0].Text = "     " + lvwSessoes.Columns[0].Text;

            lvwSessoes.ListViewItemSorter = lvwColumnSorter;
            lvwColumnSorter.SortColumn = 0;
            _PreenchendoLvw = false;
        }

        private string RemoverAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }

        private bool NomesIguais(string p_Nome1, string p_Nome2)
        {
            bool _Resp = false;

            p_Nome1 = p_Nome1.Trim().ToUpper();
            while (p_Nome1.IndexOf("  ") > 0)
            {
                p_Nome1 = p_Nome1.Replace("  ", " ");
            }
            p_Nome1 = this.RemoverAcentos(p_Nome1);

            p_Nome2 = p_Nome2.Trim().ToUpper();
            while (p_Nome2.IndexOf("  ") > 0)
            {
                p_Nome2 = p_Nome2.Replace("  ", " ");
            }
            p_Nome2 = this.RemoverAcentos(p_Nome2);

            _Resp = (p_Nome1 == p_Nome2);

            return _Resp;
        }

        private void PreencheNomeRealNoItemDaLista(ListViewItem p_lvwItem)
        {
            string _NomeChave = "";
            string _NomeReal = "";
            bool _AchouNome = false;

            _NomeChave = p_lvwItem.SubItems[2].Text;
            _NomeChave = _NomeChave.ToUpper();
            if (_NomeChave != "NULL")
            {
                _AchouNome = false;
                try
                {
                    _NomeReal = _ListaNomes[_NomeChave];
                    _AchouNome = true;
                }
                catch (Exception)
                {
                    // faz nada
                }
                if (!_AchouNome)
                {
                    _NomeReal = this.NomeUsuarioNoAD(_NomeChave);
                    if (_NomeReal.Trim().Length == 0)
                    {
                        _NomeReal = this.NomeUsuarioListaUsuarios(_NomeChave);
                        if (_NomeReal.Trim().Length > 0)
                        {
                            _AchouNome = true;
                        }
                    }
                    else
                    {
                        _AchouNome = true;
                    }
                }
                if (_AchouNome)
                {
                    try
                    {
                        _ListaNomes.Add(_NomeChave, _NomeReal);
                        _ConteudoArquivoListaNomes.Append(_NomeChave + "\t" + _NomeReal + "\n");
                    }
                    catch (Exception)
                    {
                        // faz nada
                    }
                    p_lvwItem.SubItems[3].Text = _NomeReal;
                }
                else
                {
                    _ListaNomes.Add(_NomeChave, " ");
                }

                _NomeChave = p_lvwItem.SubItems[4].Text;
                _NomeChave = _NomeChave.ToUpper();
                _AchouNome = false;
                try
                {
                    _NomeReal = _ListaNomes[_NomeChave];
                    _AchouNome = true;
                }
                catch (Exception)
                {
                    // faz nada
                }
                if (!_AchouNome)
                {
                    _NomeReal = this.NomeUsuarioNoAD(_NomeChave);
                    if (_NomeReal.Trim().Length == 0)
                    {
                        _NomeReal = this.NomeUsuarioListaUsuarios(_NomeChave);
                        if (_NomeReal.Trim().Length > 0)
                        {
                            _AchouNome = true;
                        }
                    }
                    else
                    {
                        _AchouNome = true;
                    }
                }
                if (_AchouNome)
                {
                    try
                    {
                        _ListaNomes.Add(_NomeChave, _NomeReal);
                    }
                    catch (Exception)
                    {
                        // faz nada
                    }
                    p_lvwItem.SubItems[5].Text = _NomeReal;
                }
                else
                {
                    _ListaNomes.Add(_NomeChave, " ");
                }
            }

            if (p_lvwItem.SubItems[3].Text.Trim().Length > 0 && p_lvwItem.SubItems[5].Text.Trim().Length > 0 && !this.NomesIguais(p_lvwItem.SubItems[3].Text, p_lvwItem.SubItems[5].Text))
            {
                p_lvwItem.SubItems[0].BackColor = Color.LightGray;
                //for (int i = 0; i < lvwItem.SubItems.Count; i++)
                //{
                //    lvwItem.SubItems[i].BackColor = Color.LightGray;
                //}
            }

            switch (p_lvwItem.SubItems[2].Text.Trim().ToUpper())
            {
                case "AESPROD":
                case "GACESSGX":
                    p_lvwItem.SubItems[0].BackColor = Color.LightYellow;
                    break;
            }
        }

        private void CarregaArquivoListaNomes()
        {
            if (_ListaNomes.Count == 0)
            {
                if (File.Exists(csUtil.PastaLocalExecutavel() + "ListaNomesAd.txt"))
                {
                    string _Temp = File.ReadAllText(csUtil.PastaLocalExecutavel() + "ListaNomesAd.txt");
                    _ConteudoArquivoListaNomes.Append(_Temp);
                    _Temp = _Temp.Replace("\r", "");
                    string[] _SplitLinhas = _Temp.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] _SplitColunas;
                    for (int i = 0; i < _SplitLinhas.Length; i++)
                    {
                        _SplitColunas = _SplitLinhas[i].Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (_SplitColunas[0].Trim().Length > 0 && _SplitColunas[1].Trim().Length > 0)
                        {
                            try
                            {
                                _ListaNomes.Add(_SplitColunas[0], _SplitColunas[1]);
                            }
                            catch (Exception)
                            {
                                // Faz nada
                            }
                        }
                    }
                }
            }
        }

        private void SalvaArquivoListaNomes()
        {
            if (_ConteudoArquivoListaNomes.Length > 0)
            {
                string _Temp = _ConteudoArquivoListaNomes.ToString();
                File.WriteAllText(csUtil.PastaLocalExecutavel() + "ListaNomesAd.txt", _Temp);
            }
        }

        private void PreencheNomesReaisNaLista()
        {
            
            //_ListaNomes = new Dictionary<string, string>();
            this.CarregaArquivoListaNomes();
            foreach (ListViewItem lvwItem in lvwSessoes.Items)
            {
                this.PreencheNomeRealNoItemDaLista(lvwItem);
            }
            for (int i = 0; i < lvwSessoes.Columns.Count; i++)
            {
                lvwSessoes.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            this.SalvaArquivoListaNomes();
        }

        private void btAtualizar_Click(object sender, EventArgs e)
        {
            this.AtualizarListaSessoes();
        }

        private void lvwSessoes_ColumnClick(object sender, ColumnClickEventArgs e)
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
            this.lvwSessoes.Sort();
        }

        private void chkSelecionarTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (!_PreenchendoLvw && chkSelecionarTodos.CheckState != CheckState.Indeterminate)
            {
                _PreenchendoLvw = true;
                foreach (ListViewItem lvwItem in lvwSessoes.Items)
                {
                    lvwItem.Checked = chkSelecionarTodos.Checked;
                }
                _PreenchendoLvw = false;
                Application.DoEvents();
            }
        }

        private void lvwSessoes_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!_PreenchendoLvw)
            {
                _PreenchendoLvw = true;
                if (lvwSessoes.CheckedItems.Count == 0)
                {
                    chkSelecionarTodos.CheckState = CheckState.Unchecked;
                }
                else
                {
                    if (lvwSessoes.CheckedItems.Count == lvwSessoes.Items.Count)
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
                case "MATAR_SESSOES":
                    _csOracle.MatarSessoes(_Username, _Password, _Database, _Split[1]);
                    DialogResult _Resp = MessageBox.Show("Comando executado.\nDeseja atualizar a lista de sessões?", "Matar Sessões", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (_Resp == DialogResult.Yes)
                    {
                        this.AtualizarListaSessoes();
                    }
                    break;

                case "NOME_REAL":
                    if (lvwSessoes.SelectedItems.Count > 0)
                    {
                        this.CarregaArquivoListaNomes();
                        this.PreencheNomeRealNoItemDaLista(lvwSessoes.SelectedItems[0]);
                        this.SalvaArquivoListaNomes();
                    }
                    break;

                case "PERMISSOES":
                    frmDetalhesUser _frmDetalhesUser = new frmDetalhesUser(_Username, _Password, _Database, _Split[1]);
                    _frmDetalhesUser.Show();
                    break;
            }
        }

        private string NomeUsuarioNoAD(string p_Consulta)
        {
            string _Resp = "";

            try
            {
                DirectoryEntry _DirectoryEntry = new DirectoryEntry("LDAP://10.249.1.242:389", "SVC00031", "Aes3e4r5t");
                DirectorySearcher _DirectorySearcher = new DirectorySearcher(_DirectoryEntry);
                _DirectorySearcher.PropertiesToLoad.Add("displayname");
                _DirectorySearcher.PropertiesToLoad.Add("description");
                string _Filtro = p_Consulta.Replace("%", "*");
                _DirectorySearcher.Filter = "(|(displayname=" + _Filtro + ") (samaccountname=" + _Filtro + "))";
                SearchResultCollection Results = _DirectorySearcher.FindAll();

                if (Results.Count > 0)
                {
                    ResultPropertyValueCollection _ResultPropertyValueCollection = null;
                    foreach (SearchResult searchResult in Results)
                    {
                        _ResultPropertyValueCollection = searchResult.Properties["displayname"];
                        if (_ResultPropertyValueCollection.Count > 0)
                        {
                            _Resp = _ResultPropertyValueCollection[0].ToString().ToUpper();
                        }
                        else
                        {
                            _ResultPropertyValueCollection = searchResult.Properties["description"];
                            if (_ResultPropertyValueCollection.Count > 0)
                            {
                                _Resp = _ResultPropertyValueCollection[0].ToString().ToUpper();
                            }
                            else
                            {
                                _Resp = "";
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception)
            {
                _Resp = "";
            }
            return _Resp;
        }

        private string NomeUsuarioListaUsuarios(string p_Consulta)
        {
            string _Resp = "";

            switch (p_Consulta.Trim().ToUpper())
            {
                case "AESPROD":
                case "GDP981":
                case "GIP981":
                case "PARALELO":
                    return _Resp;
            }

            if (_ConteudoArquivoUsuarios.Trim().Length == 0)
            {
                if (File.Exists(_NomeArquivoListaUsuarios))
                {
                    _ConteudoArquivoUsuarios = File.ReadAllText(_NomeArquivoListaUsuarios, Encoding.Default);
                    _ConteudoArquivoUsuarios = _ConteudoArquivoUsuarios.Replace("\r", "");
                }
            }

            if (_ConteudoArquivoUsuarios.Trim().Length > 0)
            {
                string[] _Split;
                string _Linha = "";
                _Split = _ConteudoArquivoUsuarios.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < _Split.Length; i++)
                {
                    if (_Split[i].ToUpper().IndexOf(p_Consulta.ToUpper()) > -1)
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
                        _Resp = _Split[1].Trim().ToUpper();
                    }
                }
            }

            return _Resp;
        }

        private void lvwSessoes_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (lvwSessoes.SelectedItems.Count > 0 || lvwSessoes.CheckedItems.Count > 0))
            {
                string _SID_SERIAL_ItensChecados = "";
                int _Cont = 0;

                foreach (ListViewItem lvwItem in lvwSessoes.SelectedItems)
                {
                    if (_SID_SERIAL_ItensChecados.Trim().Length > 0)
                    {
                        _SID_SERIAL_ItensChecados = _SID_SERIAL_ItensChecados + "\n";
                    }
                    _SID_SERIAL_ItensChecados = _SID_SERIAL_ItensChecados + lvwItem.Text + "," + lvwItem.SubItems[1].Text;
                    _Cont++;
                }

                foreach (ListViewItem lvwItem in lvwSessoes.CheckedItems)
                {
                    if (!lvwItem.Selected)
                    {
                        if (_SID_SERIAL_ItensChecados.Trim().Length > 0)
                        {
                            _SID_SERIAL_ItensChecados = _SID_SERIAL_ItensChecados + "\n";
                        }
                        _SID_SERIAL_ItensChecados = _SID_SERIAL_ItensChecados + lvwItem.Text + "," + lvwItem.SubItems[1].Text;
                        _Cont++;
                    }

                }

                ToolStripItem _Menu = null;

                //Point mousePos = lvwSessoes.PointToClient(Control.MousePosition);
                //ListViewHitTestInfo hitTest = lvwSessoes.HitTest(mousePos);
                //int columnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
                

                // columnIndex = 0 => SID
                // columnIndex = 1 => SERIAL#
                // columnIndex = 2 => USERNAME
                // columnIndex = 3 => NOME_OSUSER
                // columnIndex = 4 => OSUSER
                // columnIndex = 5 => NOME_OSUSER
                // columnIndex = 6 => MACHINE
                // columnIndex = 7 => STATUS
                // columnIndex = 8 => PROGRAM
                // columnIndex = 9 => TYPE
                // columnIndex = 10 => MODULE
                // columnIndex = 11 => LOGON_TIME
                // columnIndex = 12 => STATE

                Point mousePos = new Point(Cursor.Position.X + 10, Cursor.Position.Y - 10);
                mnuContextoLvw.Items.Clear();

                if (_Cont == 1)
                {
                    _Menu = mnuContextoLvw.Items.Add("Matar a sessão selecionada");
                }
                else
                {
                    _Menu = mnuContextoLvw.Items.Add("Matar as " + _Cont + " sessões selecionadas");
                }
                _Menu.Tag = "MATAR_SESSOES:" + _SID_SERIAL_ItensChecados;

                if (_Cont == 1)
                {
                    string _NomeUser = "";

                    _Menu = mnuContextoLvw.Items.Add("Preencher Nome Real");
                    _Menu.Tag = "NOME_REAL";

                    _NomeUser = lvwSessoes.SelectedItems[0].SubItems[2].Text;
                    _Menu = mnuContextoLvw.Items.Add("Mostrar Permissões do usuário " + _NomeUser);
                    _Menu.Tag = "PERMISSOES:" + _NomeUser;

                    if (lvwSessoes.SelectedItems[0].SubItems[2].Text.Trim().ToUpper() != lvwSessoes.SelectedItems[0].SubItems[4].Text.Trim().ToUpper())
                    {
                        _NomeUser = lvwSessoes.SelectedItems[0].SubItems[4].Text;
                        _Menu = mnuContextoLvw.Items.Add("Mostrar Permissões do usuário " + _NomeUser);
                        _Menu.Tag = "PERMISSOES:" + _NomeUser;
                    }
                }

                //if (lvwSessoes.SelectedItems.Count > 0)
                //{
                    //string _USERNAME = lvwSessoes.SelectedItems[0].SubItems[2].Text;
                    //string _OSUSER = lvwSessoes.SelectedItems[0].SubItems[3].Text;

                //    string _NomeUsuario = "";

                //    _NomeUsuario = this.NomeUsuarioNoAD(_USERNAME);
                //    if (_NomeUsuario.Trim().Length == 0)
                //    {
                //        _NomeUsuario = this.NomeUsuarioListaUsuarios(_USERNAME);
                //        if (_NomeUsuario.Trim().Length == 0)
                //        {
                //            _NomeUsuario = "Nome não encontrado";
                //        }
                //    }
                //    _Menu = mnuContextoLvw.Items.Add(_USERNAME + " => " + _NomeUsuario);
                //    _Menu.Tag = "FAZ_NADA:N";

                //    if (_USERNAME.Trim().ToUpper() != _OSUSER.Trim().ToUpper())
                //    {
                //        _NomeUsuario = this.NomeUsuarioNoAD(_OSUSER);
                //        if (_NomeUsuario.Trim().Length == 0)
                //        {
                //            _NomeUsuario = this.NomeUsuarioListaUsuarios(_OSUSER);
                //            if (_NomeUsuario.Trim().Length == 0)
                //            {
                //                _NomeUsuario = "Nome não encontrado";
                //            }
                //        }
                //        _Menu = mnuContextoLvw.Items.Add(_OSUSER + " => " + _NomeUsuario);
                //        _Menu.Tag = "FAZ_NADA:N";
                //    }

                //    _Menu = mnuContextoLvw.Items.Add("Procurar permissões do usuário " + _USERNAME + " no banco");
                //    _Menu.Tag = "PERMISSOES:" + _USERNAME;

                //    if (_USERNAME.Trim().ToUpper() != _OSUSER.Trim().ToUpper())
                //    {
                //        _Menu = mnuContextoLvw.Items.Add("Procurar permissões do usuário " + _OSUSER + " no banco");
                //        _Menu.Tag = "PERMISSOES:" + _OSUSER;
                //    }
                //}
                mnuContextoLvw.Show(mousePos);
            }
        }

        private void btPreencherNomeUsuarios_Click(object sender, EventArgs e)
        {
            this.PreencheNomesReaisNaLista();
        }

        private void lvwSessoes_KeyDown(object sender, KeyEventArgs e)
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
            if (lvwSessoes.SelectedItems.Count > 0)
            {
                StringBuilder _ClipBoard = new StringBuilder();
                ListViewItem lvwItem = null;
                for (int i = 0; i < lvwSessoes.Columns.Count; i++)
                {
                    _ClipBoard.Append(lvwSessoes.Columns[i].Text + "\t");
                }
                _ClipBoard.Append("\r\n");

                for (int i = 0; i < lvwSessoes.SelectedItems.Count; i++)
                {
                    lvwItem = lvwSessoes.Items[lvwSessoes.SelectedIndices[i]];
                    for (int j = 0; j < lvwItem.SubItems.Count; j++)
                    {
                        _ClipBoard.Append(lvwItem.SubItems[j].Text + "\t");
                    }
                    _ClipBoard.Append("\r\n");
                }
                Clipboard.SetText(_ClipBoard.ToString());
            }
        }
    }
}
