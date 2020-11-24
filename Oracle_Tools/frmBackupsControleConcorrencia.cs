using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace Oracle_Tools
{
    public partial class frmBackupsControleConcorrencia : Form
    {
    #region Campos privados

        private string _Username = "";
        private string _Password = "";
        private string _Database = "";
        private string _OwnerNomeObj = "";
        private csOracle _csOracle = new csOracle();
        private csListViewColumnSorter lvwColumnSorterBackups;
        private string _TagMenuContextoClicado = "";

    #endregion Campos privados

    #region Propriedades privadas

        /// <summary>
        /// Retorna ou salva O caminho do executável do Tortoise na máquina
        /// </summary>
        private string CaminhoExeTortoiseSVN
        {
            get
            {
                string _CaminhoExeTortoiseSVN = "";
                _CaminhoExeTortoiseSVN = (string)csUtil.CarregarPreferencia("CaminhoExeTortoiseSVN");
                return _CaminhoExeTortoiseSVN;
            }
            set
            {
                string _CaminhoExeTortoiseSVN;
                _CaminhoExeTortoiseSVN = value;
                csUtil.SalvarPreferencia("CaminhoExeTortoiseSVN", _CaminhoExeTortoiseSVN);
            }
        }

    #endregion Propriedades privadas

    #region Construtores

        public frmBackupsControleConcorrencia(string p_Usuario, string p_Senha, string p_Database, string p_OwnerNomeObj)
        {
            InitializeComponent();
            _Username = p_Usuario;
            _Password = p_Senha;
            _Database = p_Database;
            _OwnerNomeObj = p_OwnerNomeObj;
            this.Text = "Backups do objeto " + p_OwnerNomeObj;

            lvwColumnSorterBackups = new csListViewColumnSorter();
            this.lvwBackups.ListViewItemSorter = lvwColumnSorterBackups;

            if (_csOracle.ConectouNoBanco(_Username, _Password, _Database))
            {
                this.Show();
                Application.DoEvents();
                this.PreencheListaBackups();
            }
            else
            {
                MessageBox.Show("Problemas para conectar no banco de dados\nUsuario: " + _Username + "\nDatabase: " + _Database, "Lista Backups", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Hide();
            }
        }

        

    #endregion Construtores

    #region Métodos privados

        private void PreencheListaBackups()
        {
            Cursor.Current = Cursors.WaitCursor;

            ArrayList _ListaBackupsObjeto = null;
            string _Mensagem = "";
            csBackupObjetoDeBanco _csBackupObjetoDeBanco = null;
            ListViewItem _ListViewItem = null;
            ListViewItem.ListViewSubItem _ListViewSubItem = null;

            pbrStatus.Minimum = 0;
            pbrStatus.Maximum = 0;
            pbrStatus.Value = 0;
            lblStatus.Text = "";
            stStatusStrip.Refresh();
            Application.DoEvents();

            lvwBackups.Items.Clear();
            _ListaBackupsObjeto = _csOracle.ListaBackupsObjeto(_OwnerNomeObj, ref _Mensagem);

            

            if (_ListaBackupsObjeto.Count == 0)
            {
                MessageBox.Show("Nenhum Backup encontrado para " + _OwnerNomeObj, "Lista Backups", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                pbrStatus.Minimum = 0;
                pbrStatus.Maximum = _ListaBackupsObjeto.Count;
                pbrStatus.Value = 0;
                lblStatus.Text = pbrStatus.Value + " de " + pbrStatus.Maximum + " - Listando backups para " + _OwnerNomeObj;
                stStatusStrip.Refresh();
                Application.DoEvents();

                for (int i = 0; i < _ListaBackupsObjeto.Count; i++)
                {
                    _csBackupObjetoDeBanco = (csBackupObjetoDeBanco)_ListaBackupsObjeto[i];
                    _ListViewItem = lvwBackups.Items.Add(_csBackupObjetoDeBanco.Id_Alteracao.ToString());
                    _ListViewSubItem = _ListViewItem.SubItems.Add(_csBackupObjetoDeBanco.DataBackup.ToString("dd/MM/yy HH:mm:ss"));
                    _ListViewSubItem = _ListViewItem.SubItems.Add(_csBackupObjetoDeBanco.Tipo);
                    _ListViewSubItem = _ListViewItem.SubItems.Add(_csBackupObjetoDeBanco.NomeUsuarioQueGerouBackup);

                    pbrStatus.Value++;
                    lblStatus.Text = pbrStatus.Value + " de " + pbrStatus.Maximum + " - Listando backups para " + _OwnerNomeObj;
                    stStatusStrip.Refresh();
                    Application.DoEvents();

                }

                //for (int i = 0; i < lvwBackups.Columns.Count; i++)
                //{
                //    lvwBackups.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                //}
            }

            pbrStatus.Minimum = 0;
            pbrStatus.Maximum = 0;
            pbrStatus.Value = 0;
            lblStatus.Text = lvwBackups.Items.Count + " backups listados e " + lvwBackups.SelectedItems.Count + " backups selecionados";
            stStatusStrip.Refresh();
            Application.DoEvents();

            Cursor.Current = Cursors.Default;

        }

    #endregion Métodos privados

        

    #region Eventos de controles

        private void lvwBackups_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorterBackups.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorterBackups.Order == SortOrder.Ascending)
                {
                    lvwColumnSorterBackups.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorterBackups.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorterBackups.SortColumn = e.Column;
                lvwColumnSorterBackups.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvwBackups.Sort();
        }

        private void lvwBackups_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ToolStripItem _Menu = null;
                Point mousePos = new Point(Cursor.Position.X, Cursor.Position.Y);
                
                mnuContextoLvw.Items.Clear();

                if (lvwBackups.SelectedItems.Count > 0)
                {
                    if (lvwBackups.SelectedItems.Count == 1)
                    {
                        _Menu = mnuContextoLvw.Items.Add("Extrair fonte do backup selecionado");
                        _Menu.Tag = "ExtractBackup:" + lvwBackups.SelectedItems[0].Text;

                        if (this.CaminhoExeTortoiseSVN != null)
                        {
                            _Menu = mnuContextoLvw.Items.Add("Comparar versão atual com backup selecionado");
                            _Menu.Tag = "CompararBackupComVersaoAtual:" + lvwBackups.SelectedItems[0].Text;
                        }

                    }
                    else if (lvwBackups.SelectedItems.Count == 2)
                    {
                        _Menu = mnuContextoLvw.Items.Add("Extrair fonte dos backups selecionados");
                        _Menu.Tag = "ExtractBackups:" + lvwBackups.SelectedItems[0].Text + "," + lvwBackups.SelectedItems[1].Text;

                        if ((this.CaminhoExeTortoiseSVN != null) && (lvwBackups.SelectedItems[0].SubItems[2].Text == lvwBackups.SelectedItems[1].SubItems[2].Text))
                        {
                            _Menu = mnuContextoLvw.Items.Add("Comparar Backups Selecionados");
                            _Menu.Tag = "CompararBackups:" + lvwBackups.SelectedItems[0].Text + "," + lvwBackups.SelectedItems[1].Text;
                        }
                    }
                    else
                    {
                        _Menu = mnuContextoLvw.Items.Add("Extrair fonte dos backups selecionados");
                        _Menu.Tag = "ExtractBackups:";
                        for (int i = 0; i < lvwBackups.SelectedItems.Count; i++)
                        {
                            if (i > 0)
                            {
                                _Menu.Tag = _Menu.Tag + ",";
                            }
                            _Menu.Tag = _Menu.Tag + lvwBackups.SelectedItems[i].Text;
                        }

                    }
                    mnuContextoLvw.Show(mousePos);
                }
            }
        }

        private void mnuContextoLvw_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _TagMenuContextoClicado = e.ClickedItem.Tag.ToString();
            tmrMnuContextoLvw.Enabled = true;
        }

        private void tmrMnuContextoLvw_Tick(object sender, EventArgs e)
        {
            tmrMnuContextoLvw.Enabled = false;
            string[] _Split;
            string _Comando = "";
            string _Mensagem = "";
            string _TipoObjeto = "";
            string _Fonte = "";
            string _OwnerNomeObjeto = "";
            decimal _IdBackup1 = 0;
            decimal _IdBackup2 = 0;
            string _NomeArquivoBackup1 = "";
            string _NomeArquivoBackup2 = "";
            string _NomeArquivoAtual = "";
            string _Linha_de_Comando = "";

            _Split = _TagMenuContextoClicado.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            _Comando = _Split[0];

            switch (_Comando)
            {
                case "ExtractBackup":
                    Cursor.Current = Cursors.WaitCursor;
                    _IdBackup1 = decimal.Parse(_Split[1]);
                    _Fonte = _csOracle.ExtractBackupObjeto(_IdBackup1, ref _OwnerNomeObjeto, ref _Mensagem);
                    if (_Fonte.Trim().Length == 0)
                    {
                        if (_Mensagem.Trim().Length > 0)
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Problemas para extrair backup\n" + _Mensagem, "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Problemas para extrair backup", "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        _NomeArquivoBackup1 = "Backup_" + _IdBackup1 + "_" + _OwnerNomeObjeto + ".sql";
                        csUtil.SalvarEAbrir(_Fonte, _NomeArquivoBackup1);
                    }
                    Cursor.Current = Cursors.Default;
                    break;

                case "CompararBackupComVersaoAtual":
                    Cursor.Current = Cursors.WaitCursor;
                    _IdBackup1 = decimal.Parse(_Split[1]);
                    _Fonte = _csOracle.ExtractBackupObjeto(_IdBackup1, ref _OwnerNomeObjeto, ref _Mensagem);
                    if (_Fonte.Trim().Length == 0)
                    {
                        if (_Mensagem.Trim().Length > 0)
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Problemas para extrair backup\n" + _Mensagem, "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Problemas para extrair backup", "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        _NomeArquivoBackup1 = Path.GetTempPath() + "Backup_" + _IdBackup1 + "_" + _OwnerNomeObjeto + ".sql";
                        File.WriteAllText(_NomeArquivoBackup1, _Fonte, Encoding.Default);
                        _Fonte = _csOracle.ExtractDDL(_Username, _Password, _Database, _OwnerNomeObjeto, ref _TipoObjeto, ref _Mensagem);
                        _NomeArquivoAtual = Path.GetTempPath() + _OwnerNomeObjeto + ".sql";
                        File.WriteAllText(_NomeArquivoAtual, _Fonte, Encoding.Default);

                        _Linha_de_Comando = "/command:diff /path:\"" + _NomeArquivoAtual + "\" /path2:\"" + _NomeArquivoBackup1 + "\"";
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                        process.StartInfo.FileName = this.CaminhoExeTortoiseSVN;
                        process.StartInfo.Arguments = _Linha_de_Comando;
                        process.Start();
                    }
                    Cursor.Current = Cursors.Default;
                    break;

                case "ExtractBackups":
                    Cursor.Current = Cursors.WaitCursor;

                    _Split = _Split[1].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < _Split.Length; i++)
                    {
                        _IdBackup1 = decimal.Parse(_Split[i]);
                        _Fonte = _csOracle.ExtractBackupObjeto(_IdBackup1, ref _OwnerNomeObjeto, ref _Mensagem);
                        if (_Fonte.Trim().Length == 0)
                        {
                            if (_Mensagem.Trim().Length > 0)
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Problemas para extrair backup\n" + _Mensagem, "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Problemas para extrair backup", "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            _NomeArquivoBackup1 = "Backup_" + _IdBackup1 + "_" + _OwnerNomeObjeto + ".sql";
                            csUtil.SalvarEAbrir(_Fonte, _NomeArquivoBackup1);
                        }
                    }
                    Cursor.Current = Cursors.Default;
                    break;

                case "CompararBackups":
                    
                    Cursor.Current = Cursors.WaitCursor;

                    _Split = _Split[1].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    _IdBackup1 = decimal.Parse(_Split[0]);
                    _Fonte = _csOracle.ExtractBackupObjeto(_IdBackup1, ref _OwnerNomeObjeto, ref _Mensagem);
                    if (_Fonte.Trim().Length == 0)
                    {
                        if (_Mensagem.Trim().Length > 0)
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Problemas para extrair backup\n" + _Mensagem, "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Problemas para extrair backup", "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        _NomeArquivoBackup1 = Path.GetTempPath() + "Backup_" + _IdBackup1 + "_" + _OwnerNomeObjeto + ".sql";
                        File.WriteAllText(_NomeArquivoBackup1, _Fonte, Encoding.Default);

                        _IdBackup2 = decimal.Parse(_Split[1]);
                        _Fonte = _csOracle.ExtractBackupObjeto(_IdBackup2, ref _OwnerNomeObjeto, ref _Mensagem);

                        if (_Fonte.Trim().Length == 0)
                        {
                            if (_Mensagem.Trim().Length > 0)
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Problemas para extrair backup\n" + _Mensagem, "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Problemas para extrair backup", "Extrair Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            _NomeArquivoBackup2 = Path.GetTempPath() + "Backup_" + _IdBackup2 + "_" + _OwnerNomeObjeto + ".sql";
                            File.WriteAllText(_NomeArquivoBackup2, _Fonte, Encoding.Default);

                            if (_IdBackup1 > _IdBackup2)
                            {
                                _Linha_de_Comando = "/command:diff /path:\"" + _NomeArquivoBackup1 + "\" /path2:\"" + _NomeArquivoBackup2 + "\"";
                            }
                            else
                            {
                                _Linha_de_Comando = "/command:diff /path:\"" + _NomeArquivoBackup2 + "\" /path2:\"" + _NomeArquivoBackup1 + "\"";
                            }
                            
                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                            process.StartInfo.FileName = this.CaminhoExeTortoiseSVN;
                            process.StartInfo.Arguments = _Linha_de_Comando;
                            process.Start();
                        }
                    }
                    Cursor.Current = Cursors.Default;
                    break;
            }
        }

        private void lvwBackups_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            lblStatus.Text = lvwBackups.Items.Count + " backups listados e " + lvwBackups.SelectedItems.Count + " backups selecionados";
            stStatusStrip.Refresh();
            Application.DoEvents();
        }

    #endregion Eventos de controles
        
    }
}
