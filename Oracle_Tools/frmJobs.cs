using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Oracle_Tools
{
    public partial class frmJobs : Form
    {
        #region Campos privados

        private string _Username = "";
        private string _Password = "";
        private string _Database = "";
        private csOracle _csOracle = new csOracle();
        private bool _PreenchendoLvw = false;
        private csListViewColumnSorter lvwColumnSorter;
        private string _TagMenuContextoClicado = "";

        #endregion Campos privados

        #region Construtores

        public frmJobs()
        {
            InitializeComponent();
            this._PropriedadesPadrao();
            _Username = "";
            _Password = "";
            _Database = "";
            lvwColumnSorter = new csListViewColumnSorter();
            this.lvwJobs.ListViewItemSorter = lvwColumnSorter;
            this.Text = "Jobs de banco - NÃO CONECTADO";
        }

        public frmJobs(string p_Username, string p_Password, string p_Database)
        {
            InitializeComponent();
            this._PropriedadesPadrao();
            _Username = p_Username;
            _Password = p_Password;
            _Database = p_Database;
            lvwColumnSorter = new csListViewColumnSorter();
            this.lvwJobs.ListViewItemSorter = lvwColumnSorter;
            string _Mensagem = "";
            string _InfoBanco = "";
            _InfoBanco = _csOracle.InfoBanco(_Username, _Password, _Database, ref _Mensagem);
            if (_Mensagem.Length == 0)
            {
                this.Text = "Jobs de banco - " + _InfoBanco;
            }
        }

        #endregion Construtores

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
                    this.Text = "Jobs de banco - " + _InfoBanco;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void _PropriedadesPadrao()
        {
            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwColumnSorter = new csListViewColumnSorter();
            this.lvwJobs.ListViewItemSorter = lvwColumnSorter;

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

        }

        private void AtualizarListaJobs()
        {
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }

            _PreenchendoLvw = true;

            string _Mensagem = "";
            bool _PrimeiraVez = true;

            Cursor.Current = Cursors.WaitCursor;
            lvwJobs.Items.Clear();
            chkSelecionarTodos.Checked = false;
            lblStatusLista.Text = lvwJobs.Items.Count.ToString() + " itens listados - " + lvwJobs.CheckedItems.Count.ToString() + " itens selecionados";
            lblStatusLista.Refresh();
            Application.DoEvents();
            lvwColumnSorter.SortColumn = 0;

            string _Query = @"SELECT ALL_OBJECTS.CREATED AS CRIACAO,
	                            ALL_OBJECTS.LAST_DDL_TIME AS ULTIMA_ALTERACAO,
	                            ALL_SCHEDULER_JOBS.OWNER,
	                            ALL_SCHEDULER_JOBS.JOB_NAME,
	                            ALL_SCHEDULER_JOBS.JOB_ACTION,
	                            ALL_SCHEDULER_JOBS.ENABLED,
	                            ALL_SCHEDULER_JOBS.JOB_TYPE,
	                            ALL_SCHEDULER_JOBS.PROGRAM_OWNER,
	                            ALL_SCHEDULER_JOBS.PROGRAM_NAME,
	                            ALL_SCHEDULER_JOBS.SCHEDULE_OWNER,
	                            ALL_SCHEDULER_JOBS.SCHEDULE_NAME,
	                            ALL_SCHEDULER_JOBS.SCHEDULE_TYPE,
	                            ALL_SCHEDULER_JOBS.REPEAT_INTERVAL,
	                            (ALL_SCHEDULER_JOBS.LAST_START_DATE AT TIME ZONE SESSIONTIMEZONE) AS LAST_START_DATE,
	                            ALL_SCHEDULER_JOBS.LAST_RUN_DURATION,
                                (ALL_SCHEDULER_JOBS.NEXT_RUN_DATE AT TIME ZONE SESSIONTIMEZONE) AS NEXT_RUN_DATE,
                                ALL_SCHEDULER_JOBS.RUN_COUNT,
                                ALL_SCHEDULER_JOBS.FAILURE_COUNT,
                                ALL_SCHEDULER_JOBS.COMMENTS,
                                ALL_SCHEDULER_JOBS.STATE,
                                SYSDATE
                            FROM ALL_SCHEDULER_JOBS
							    INNER JOIN ALL_OBJECTS
									ON ALL_SCHEDULER_JOBS.OWNER = ALL_OBJECTS.OWNER
									AND ALL_SCHEDULER_JOBS.JOB_NAME = ALL_OBJECTS.OBJECT_NAME
                            ORDER BY ALL_SCHEDULER_JOBS.ENABLED DESC, (ALL_SCHEDULER_JOBS.LAST_START_DATE AT TIME ZONE SESSIONTIMEZONE) DESC";

            if (lvwJobs.Columns.Count > 0)
            {
                _PrimeiraVez = false;
            }

            _csOracle.PreencheLvw(_Username, _Password, _Database, _Query, true, ref _Mensagem, ref lvwJobs);

            if (lvwJobs.Columns.Count > 0 && _PrimeiraVez)
            {
                lvwJobs.Columns[0].Text = "     " + lvwJobs.Columns[0].Text;
            }

            this.ClassificaJobsNaLista();

            _PreenchendoLvw = false;
        }

        private string RetornaItemDaLista(int p_NumeroLinha, string p_NomeColuna)
        {
            string _Retorno = "";
            ListViewItem lvwItem;

            if (lvwJobs.Items.Count >= (p_NumeroLinha + 1))
            {
                lvwItem = lvwJobs.Items[p_NumeroLinha];
                for (int i = 0; i < lvwJobs.Columns.Count; i++)
                {
                    if (lvwJobs.Columns[i].Text.Trim().ToUpper() == p_NomeColuna.Trim().ToUpper())
                    {
                        _Retorno = lvwItem.SubItems[i].Text;
                        break;
                    }
                }
            }

            return _Retorno;
        }

        private void ClassificaJobsNaLista()
        {
            ListViewItem lvwItem;
            DateTime _DB_SYSDATE = DateTime.MinValue;
            string _Valor1 = "";
            string _Valor2 = "";
            string _Valor3 = "";

            for (int i = 0; i < lvwJobs.Items.Count; i++)
            {
                lvwItem = lvwJobs.Items[i];
                _Valor1 = RetornaItemDaLista(i, "ENABLED");
                if (_Valor1.Trim().ToUpper() == "FALSE")
                {
                    // Job desabilitado
                    lvwItem.ForeColor = Color.LightGray;
                    for (int c = 1; c < lvwJobs.Columns.Count; c++)
                    {
                        lvwItem.SubItems[c].ForeColor = Color.LightGray;
                    }
                }
                else
                {
                    _DB_SYSDATE = DateTime.Parse(RetornaItemDaLista(i, "SYSDATE"));
                    _Valor1 = RetornaItemDaLista(i, "STATE");
                    _Valor2 = RetornaItemDaLista(i, "SCHEDULE_TYPE");
                    if (_Valor1.Trim().ToUpper() != "RUNNING")
                    {
                        _Valor3 = RetornaItemDaLista(i, "NEXT_RUN_DATE");
                        switch (_Valor2.Trim().ToUpper())
                        {
                            case "ONCE":
                            case "NAMED":
                            case "IMMEDIATE":
                                break;
                            default:
                                DateTime _STZ_NEXT_RUN_DATE = DateTime.Parse(_Valor3);
                                if (_STZ_NEXT_RUN_DATE < _DB_SYSDATE.Add(new TimeSpan(0, -1, 0)))
                                {
                                    // Job travado
                                    lvwItem.ForeColor = Color.Red;
                                    for (int c = 1; c < lvwJobs.Columns.Count; c++)
                                    {
                                        lvwItem.SubItems[c].ForeColor = Color.Red;
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        _Valor3 = RetornaItemDaLista(i, "LAST_START_DATE");
                        switch (_Valor2.Trim().ToUpper())
                        {
                            case "ONCE":
                            case "NAMED":
                            case "IMMEDIATE":
                                break;
                            default:
                                DateTime _STZ_LAST_START_DATE = DateTime.Parse(_Valor3);
                                if (_STZ_LAST_START_DATE < _DB_SYSDATE.Add(new TimeSpan(-1, 0, 0)))
                                {
                                    // Job rodando a mais de 1 hora
                                    lvwItem.ForeColor = Color.Orange;
                                    for (int c = 1; c < lvwJobs.Columns.Count; c++)
                                    {
                                        lvwItem.SubItems[c].ForeColor = Color.Orange;
                                    }
                                }
                                break;
                        }
                    }
                }
            }

        }

        private void CopiarListaSelecionada()
        {
            if (lvwJobs.SelectedItems.Count > 0)
            {
                StringBuilder _ClipBoard = new StringBuilder();
                ListViewItem lvwItem = null;
                for (int i = 0; i < lvwJobs.Columns.Count; i++)
                {
                    _ClipBoard.Append(lvwJobs.Columns[i].Text + "\t");
                }
                _ClipBoard.Append("\r\n");

                for (int i = 0; i < lvwJobs.SelectedItems.Count; i++)
                {
                    lvwItem = lvwJobs.Items[lvwJobs.SelectedIndices[i]];
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
            if (lvwJobs.Items.Count > 0)
            {
                StringBuilder _ClipBoard = new StringBuilder();
                ListViewItem lvwItem = null;
                for (int i = 0; i < lvwJobs.Columns.Count; i++)
                {
                    _ClipBoard.Append(lvwJobs.Columns[i].Text + "\t");
                }
                _ClipBoard.Append("\r\n");

                for (int i = 0; i < lvwJobs.Items.Count; i++)
                {
                    lvwItem = lvwJobs.Items[i];
                    for (int j = 0; j < lvwItem.SubItems.Count; j++)
                    {
                        _ClipBoard.Append(lvwItem.SubItems[j].Text + "\t");
                    }
                    _ClipBoard.Append("\r\n");
                }
                Clipboard.SetText(_ClipBoard.ToString());
            }
        }

        #endregion Métodos Privados

        #region Eventos dos controles

        private void btAtualizar_Click(object sender, EventArgs e)
        {
            this.AtualizarListaJobs();
        }

        private void lvwJobs_ColumnClick(object sender, ColumnClickEventArgs e)
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
            this.lvwJobs.Sort();
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

        private void lvwJobs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && lvwJobs.SelectedItems.Count > 0)
            {
                ToolStripItem _Menu = null;
                Point mousePos = new Point(Cursor.Position.X + 10, Cursor.Position.Y - 10);
                mnuContextoLvw.Items.Clear();

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

        private void chkSelecionarTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (!_PreenchendoLvw && chkSelecionarTodos.CheckState != CheckState.Indeterminate)
            {
                _PreenchendoLvw = true;
                foreach (ListViewItem lvwItem in lvwJobs.Items)
                {
                    lvwItem.Checked = chkSelecionarTodos.Checked;
                }
                _PreenchendoLvw = false;
                lblStatusLista.Text = lvwJobs.Items.Count.ToString() + " itens listados - " + lvwJobs.CheckedItems.Count.ToString() + " itens selecionados";
                lblStatusLista.Refresh();
                Application.DoEvents();
            }
        }

        private void lvwJobs_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!_PreenchendoLvw)
            {
                lblStatusLista.Text = lvwJobs.Items.Count.ToString() + " itens listados - " + lvwJobs.CheckedItems.Count.ToString() + " itens selecionados";
                lblStatusLista.Refresh();

                _PreenchendoLvw = true;
                if (lvwJobs.CheckedItems.Count == 0)
                {
                    chkSelecionarTodos.CheckState = CheckState.Unchecked;
                }
                else
                {
                    if (lvwJobs.CheckedItems.Count == lvwJobs.Items.Count)
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

        #endregion

        

    }
}
