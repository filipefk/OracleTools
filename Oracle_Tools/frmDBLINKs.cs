using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.IO;
using Oracle.DataAccess.Client;

namespace Oracle_Tools
{
    public partial class frmDBLINKs : Form
    {
        #region Campos privados

        private string _Username = "";
        private string _Password = "";
        private string _Database = "";
        private csOracle _csOracle = new csOracle();
        private bool _PreenchendoLvw = false;
        private csListViewColumnSorter lvwColumnSorter;
        private string _TagMenuContextoClicado = "";

        #endregion

        public frmDBLINKs()
        {
            InitializeComponent();
            _Username = "";
            _Password = "";
            _Database = "";
            this.Text = "Sessões - NÃO CONECTADO";
            lvwColumnSorter = new csListViewColumnSorter();
        }

        public frmDBLINKs(string p_Username, string p_Password, string p_Database)
        {
            InitializeComponent();
            _Username = p_Username;
            _Password = p_Password;
            _Database = p_Database;
            lvwDBLINKs.ListViewItemSorter = lvwColumnSorter;
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

        private void AtualizarListaDBLINKs()
        {
            if (!this.EstaConectado)
            {
                if (!this.ConectouNoBanco())
                {
                    return;
                }
            }

            _PreenchendoLvw = true;

            string _Query = @"SELECT CREATED,
                                   OWNER,
                                   DB_LINK,
                                   ' ' AS TESTE,
                                   USERNAME,
                                   HOST
                            FROM ALL_DB_LINKS";

            string _Mensagem = "";
            lvwColumnSorter.SortColumn = 0;
            lvwDBLINKs.ListViewItemSorter = null;
            lvwDBLINKs.Items.Clear();

            _csOracle.PreencheLvw(_Username, _Password, _Database, _Query, true, ref _Mensagem, ref lvwDBLINKs);

            if (_Mensagem.Trim().Length > 0)
            {
                MessageBox.Show("Problemas ao buscar DBLINKs\n" + _Mensagem, "Atualizar Lista de DBLINKs", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lvwDBLINKs.ListViewItemSorter = lvwColumnSorter;
            lvwColumnSorter.SortColumn = 0;
            _PreenchendoLvw = false;
        }
        
        private void btAtualizar_Click(object sender, EventArgs e)
        {
            this.AtualizarListaDBLINKs();
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
                case "TESTAR_DBLINKS":
                    if (lvwDBLINKs.SelectedItems.Count > 0)
                    {
                        this.TestarDBLINKs(true);
                    }
                    break;
            }
        }

        private void TestarDBLINKs(bool p_So_Selecionados)
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
                //OracleCommand _CommandCLOSE_DATABASE_LINK = new OracleCommand();
                //OracleDataAdapter _OracleDataAdapter;
                OracleCommand _Command = new OracleCommand();
                OracleConnection _Connection = new OracleConnection();
                OracleDataReader _DataReader;
                ListViewItem lvwItem;
                bool _TestarEste = false;
                string _Query = "";

                _Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
                _Connection.Open();
                _Command.Connection = _Connection;
                _Command.CommandType = CommandType.Text;

                for (int i = 0; i < lvwDBLINKs.Items.Count; i++)
                {
                    lvwItem = lvwDBLINKs.Items[i];
                    _TestarEste = false;

                    if (p_So_Selecionados)
                    {
                        if (lvwItem.Selected)
                        {
                            _TestarEste = true;
                        }
                        else
                        {
                            _TestarEste = false;
                        }
                    }
                    else
                    {
                        _TestarEste = true;
                    }

                    if (_TestarEste)
                    {
                        _Query = "Select * from dual@" + lvwItem.SubItems[2].Text;
                        try
                        {
                            //_Connection.Close();
                            //_Connection = new OracleConnection();
                            //_Connection.ConnectionString = "User Id=" + _Username + ";Password=" + _Password + ";Data Source=" + _Database;
                            //_Connection.Open();
                            //_Command.Connection = _Connection;
                            //_Command.CommandType = CommandType.Text;

                            _Command.CommandText = _Query;
                            _DataReader = _Command.ExecuteReader();
                            if (_DataReader.Read())
                            {
                                _DataReader.Close();
                                lvwItem.SubItems[3].Text = "TESTE OK";
                                lvwItem.ForeColor = Color.DarkGreen;
                                for (int c = 1; c < lvwDBLINKs.Columns.Count; c++)
                                {
                                    lvwItem.SubItems[c].ForeColor = Color.DarkGreen;
                                }
                                //_CommandCLOSE_DATABASE_LINK = new OracleCommand("Begin DBMS_SESSION.CLOSE_DATABASE_LINK('" + lvwItem.SubItems[2].Text + "'); End;", _Connection);
                                //_CommandCLOSE_DATABASE_LINK.CommandType = CommandType.Text;
                                //_OracleDataAdapter = new OracleDataAdapter(_CommandCLOSE_DATABASE_LINK);
                                //_Command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception _Exception)
                        {
                            //if (_Exception.Message.Trim().ToUpper().Substring(0, 9) == "ORA-02020") //TOO MANY DATABASE LINKS IN USE
                            //{
                            //    lvwItem.SubItems[3].Text = "TESTE OK";
                            //    lvwItem.ForeColor = Color.DarkGreen;
                            //    for (int c = 1; c < lvwDBLINKs.Columns.Count; c++)
                            //    {
                            //        lvwItem.SubItems[c].ForeColor = Color.DarkGreen;
                            //    }
                            //}
                            //else
                            //{
                                lvwItem.SubItems[3].Text = "ERRO: " + _Exception.Message;
                                lvwItem.ForeColor = Color.Red;
                                for (int c = 1; c < lvwDBLINKs.Columns.Count; c++)
                                {
                                    lvwItem.SubItems[c].ForeColor = Color.Red;
                                }
                            //}
                            
                        }
                        for (int c = 0; c < lvwDBLINKs.Columns.Count; c++)
                        {
                            lvwDBLINKs.Columns[c].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                        }
                        Application.DoEvents();
                    }
                }
                _Connection.Close();
            }
            catch (Exception _Exception)
            {
                MessageBox.Show("Problemas para conectar no banco de dados\n" + _Exception.Message, "Testar DBLINKs", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void lvwDBLINKs_ColumnClick(object sender, ColumnClickEventArgs e)
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
            this.lvwDBLINKs.Sort();
        }

        private void lvwDBLINKs_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (lvwDBLINKs.SelectedItems.Count > 0))
            {
                ToolStripItem _Menu = null;
                
                Point mousePos = new Point(Cursor.Position.X + 10, Cursor.Position.Y - 10);
                mnuContextoLvw.Items.Clear();

                _Menu = mnuContextoLvw.Items.Add("Testar DBLINKs selecionados");
                _Menu.Tag = "TESTAR_DBLINKS:";

                mnuContextoLvw.Show(mousePos);
            }
        }

        private void btTestarTodos_Click(object sender, EventArgs e)
        {
            this.TestarDBLINKs(false);
        }
        
    }
}
