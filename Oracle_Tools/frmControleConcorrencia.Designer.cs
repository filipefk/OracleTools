namespace Oracle_Tools
{
    partial class frmControleConcorrencia
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmControleConcorrencia));
            this.txtPesquisa = new System.Windows.Forms.TextBox();
            this.chkSelecionarTodos = new System.Windows.Forms.CheckBox();
            this.lvwObjetos = new System.Windows.Forms.ListView();
            this.colDataHora = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTipo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOwner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNome = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAnalista = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEmpresa = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOSUSER = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDBUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tmrDigitacao = new System.Windows.Forms.Timer(this.components);
            this.lblPesquisaSecundaria = new System.Windows.Forms.Label();
            this.lvwPesquisaSecundaria = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblStatusListaPrincipal = new System.Windows.Forms.Label();
            this.lblStatusListaSecundaria = new System.Windows.Forms.Label();
            this.lvwRelatorio = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chkConsultarLogBranch = new System.Windows.Forms.CheckBox();
            this.lblStatusRelatorio = new System.Windows.Forms.Label();
            this.mnuContextoLvw = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuContextoBotoes = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuPesquisarTodasAlteracoes = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPesquisarUltimaSemana = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPesquisarUltimas24Horas = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPesquisarUltimas72Horas = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuListarConflitos = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChamarPreecheUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChamarProcuraConflitos = new System.Windows.Forms.ToolStripMenuItem();
            this.btMenuContextoBotoes = new System.Windows.Forms.Button();
            this.btPesquisar = new System.Windows.Forms.Button();
            this.mnuMenu = new System.Windows.Forms.MenuStrip();
            this.mnuInfoBanco = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrMnuContextoBotoes = new System.Windows.Forms.Timer(this.components);
            this.tmrMnuContextoLvw = new System.Windows.Forms.Timer(this.components);
            this.btColorir = new System.Windows.Forms.Button();
            this.mnuContextoBotoes.SuspendLayout();
            this.mnuMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPesquisa
            // 
            this.txtPesquisa.Location = new System.Drawing.Point(12, 32);
            this.txtPesquisa.Name = "txtPesquisa";
            this.txtPesquisa.Size = new System.Drawing.Size(565, 20);
            this.txtPesquisa.TabIndex = 1;
            this.txtPesquisa.TextChanged += new System.EventHandler(this.txtPesquisa_TextChanged);
            // 
            // chkSelecionarTodos
            // 
            this.chkSelecionarTodos.AutoSize = true;
            this.chkSelecionarTodos.Location = new System.Drawing.Point(-141, 48);
            this.chkSelecionarTodos.Name = "chkSelecionarTodos";
            this.chkSelecionarTodos.Size = new System.Drawing.Size(15, 14);
            this.chkSelecionarTodos.TabIndex = 30;
            this.chkSelecionarTodos.UseVisualStyleBackColor = true;
            // 
            // lvwObjetos
            // 
            this.lvwObjetos.AllowDrop = true;
            this.lvwObjetos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvwObjetos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDataHora,
            this.colTipo,
            this.colOwner,
            this.colNome,
            this.colAnalista,
            this.colEmpresa,
            this.colOSUSER,
            this.colDBUser,
            this.colHost});
            this.lvwObjetos.FullRowSelect = true;
            this.lvwObjetos.GridLines = true;
            this.lvwObjetos.HideSelection = false;
            this.lvwObjetos.Location = new System.Drawing.Point(12, 58);
            this.lvwObjetos.Name = "lvwObjetos";
            this.lvwObjetos.Size = new System.Drawing.Size(693, 340);
            this.lvwObjetos.TabIndex = 29;
            this.lvwObjetos.UseCompatibleStateImageBehavior = false;
            this.lvwObjetos.View = System.Windows.Forms.View.Details;
            this.lvwObjetos.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwObjetos_ColumnClick);
            this.lvwObjetos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvwObjetos_KeyDown);
            this.lvwObjetos.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwObjetos_MouseClick);
            // 
            // colDataHora
            // 
            this.colDataHora.Text = "DATA HORA";
            this.colDataHora.Width = 90;
            // 
            // colTipo
            // 
            this.colTipo.Text = "TIPO";
            this.colTipo.Width = 100;
            // 
            // colOwner
            // 
            this.colOwner.Text = "OWNER";
            this.colOwner.Width = 100;
            // 
            // colNome
            // 
            this.colNome.Text = "NOME";
            this.colNome.Width = 200;
            // 
            // colAnalista
            // 
            this.colAnalista.Text = "ANALISTA";
            this.colAnalista.Width = 100;
            // 
            // colEmpresa
            // 
            this.colEmpresa.Text = "EMPRESA";
            this.colEmpresa.Width = 67;
            // 
            // colOSUSER
            // 
            this.colOSUSER.Text = "OS USER";
            // 
            // colDBUser
            // 
            this.colDBUser.Text = "DB USER";
            // 
            // colHost
            // 
            this.colHost.Text = "HOST";
            // 
            // tmrDigitacao
            // 
            this.tmrDigitacao.Interval = 1000;
            this.tmrDigitacao.Tick += new System.EventHandler(this.tmrDigitacao_Tick);
            // 
            // lblPesquisaSecundaria
            // 
            this.lblPesquisaSecundaria.AutoSize = true;
            this.lblPesquisaSecundaria.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPesquisaSecundaria.ForeColor = System.Drawing.Color.Blue;
            this.lblPesquisaSecundaria.Location = new System.Drawing.Point(708, 32);
            this.lblPesquisaSecundaria.Name = "lblPesquisaSecundaria";
            this.lblPesquisaSecundaria.Size = new System.Drawing.Size(187, 13);
            this.lblPesquisaSecundaria.TabIndex = 33;
            this.lblPesquisaSecundaria.Text = "Pesquisa Secundária: Nenhuma";
            // 
            // lvwPesquisaSecundaria
            // 
            this.lvwPesquisaSecundaria.AllowDrop = true;
            this.lvwPesquisaSecundaria.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwPesquisaSecundaria.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lvwPesquisaSecundaria.FullRowSelect = true;
            this.lvwPesquisaSecundaria.GridLines = true;
            this.lvwPesquisaSecundaria.HideSelection = false;
            this.lvwPesquisaSecundaria.Location = new System.Drawing.Point(711, 58);
            this.lvwPesquisaSecundaria.Name = "lvwPesquisaSecundaria";
            this.lvwPesquisaSecundaria.Size = new System.Drawing.Size(351, 61);
            this.lvwPesquisaSecundaria.TabIndex = 34;
            this.lvwPesquisaSecundaria.UseCompatibleStateImageBehavior = false;
            this.lvwPesquisaSecundaria.View = System.Windows.Forms.View.Details;
            this.lvwPesquisaSecundaria.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwPesquisaSecundaria_ColumnClick);
            this.lvwPesquisaSecundaria.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvwPesquisaSecundaria_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "DATA HORA";
            this.columnHeader1.Width = 90;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "TIPO";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "OWNER";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "NOME";
            this.columnHeader4.Width = 200;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "ANALISTA";
            this.columnHeader5.Width = 100;
            // 
            // lblStatusListaPrincipal
            // 
            this.lblStatusListaPrincipal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatusListaPrincipal.AutoSize = true;
            this.lblStatusListaPrincipal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusListaPrincipal.ForeColor = System.Drawing.Color.Red;
            this.lblStatusListaPrincipal.Location = new System.Drawing.Point(9, 401);
            this.lblStatusListaPrincipal.Name = "lblStatusListaPrincipal";
            this.lblStatusListaPrincipal.Size = new System.Drawing.Size(92, 13);
            this.lblStatusListaPrincipal.TabIndex = 35;
            this.lblStatusListaPrincipal.Text = "0 itens listados";
            // 
            // lblStatusListaSecundaria
            // 
            this.lblStatusListaSecundaria.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatusListaSecundaria.AutoSize = true;
            this.lblStatusListaSecundaria.Location = new System.Drawing.Point(713, 126);
            this.lblStatusListaSecundaria.Name = "lblStatusListaSecundaria";
            this.lblStatusListaSecundaria.Size = new System.Drawing.Size(76, 13);
            this.lblStatusListaSecundaria.TabIndex = 36;
            this.lblStatusListaSecundaria.Text = "0 itens listados";
            // 
            // lvwRelatorio
            // 
            this.lvwRelatorio.AllowDrop = true;
            this.lvwRelatorio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwRelatorio.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6});
            this.lvwRelatorio.FullRowSelect = true;
            this.lvwRelatorio.GridLines = true;
            this.lvwRelatorio.HideSelection = false;
            this.lvwRelatorio.Location = new System.Drawing.Point(711, 142);
            this.lvwRelatorio.Name = "lvwRelatorio";
            this.lvwRelatorio.Size = new System.Drawing.Size(351, 256);
            this.lvwRelatorio.TabIndex = 38;
            this.lvwRelatorio.UseCompatibleStateImageBehavior = false;
            this.lvwRelatorio.View = System.Windows.Forms.View.Details;
            this.lvwRelatorio.DoubleClick += new System.EventHandler(this.lvwRelatorio_DoubleClick);
            this.lvwRelatorio.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvwRelatorio_MouseDown);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Relatório";
            this.columnHeader6.Width = 610;
            // 
            // chkConsultarLogBranch
            // 
            this.chkConsultarLogBranch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkConsultarLogBranch.AutoSize = true;
            this.chkConsultarLogBranch.Checked = true;
            this.chkConsultarLogBranch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkConsultarLogBranch.Location = new System.Drawing.Point(819, 125);
            this.chkConsultarLogBranch.Name = "chkConsultarLogBranch";
            this.chkConsultarLogBranch.Size = new System.Drawing.Size(147, 17);
            this.chkConsultarLogBranch.TabIndex = 40;
            this.chkConsultarLogBranch.Text = "Consultar LOG do Branch";
            this.chkConsultarLogBranch.UseVisualStyleBackColor = true;
            // 
            // lblStatusRelatorio
            // 
            this.lblStatusRelatorio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusRelatorio.AutoSize = true;
            this.lblStatusRelatorio.Location = new System.Drawing.Point(711, 401);
            this.lblStatusRelatorio.Name = "lblStatusRelatorio";
            this.lblStatusRelatorio.Size = new System.Drawing.Size(28, 13);
            this.lblStatusRelatorio.TabIndex = 41;
            this.lblStatusRelatorio.Text = "XXX";
            // 
            // mnuContextoLvw
            // 
            this.mnuContextoLvw.Name = "mnuMenu";
            this.mnuContextoLvw.ShowImageMargin = false;
            this.mnuContextoLvw.Size = new System.Drawing.Size(36, 4);
            this.mnuContextoLvw.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuContextoLvw_ItemClicked);
            // 
            // mnuContextoBotoes
            // 
            this.mnuContextoBotoes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPesquisarTodasAlteracoes,
            this.mnuPesquisarUltimaSemana,
            this.mnuPesquisarUltimas24Horas,
            this.mnuPesquisarUltimas72Horas,
            this.mnuListarConflitos,
            this.mnuChamarPreecheUsuarios,
            this.mnuChamarProcuraConflitos});
            this.mnuContextoBotoes.Name = "mnuContextoBotoes";
            this.mnuContextoBotoes.Size = new System.Drawing.Size(291, 158);
            this.mnuContextoBotoes.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuContextoBotoes_ItemClicked);
            // 
            // mnuPesquisarTodasAlteracoes
            // 
            this.mnuPesquisarTodasAlteracoes.Name = "mnuPesquisarTodasAlteracoes";
            this.mnuPesquisarTodasAlteracoes.Size = new System.Drawing.Size(290, 22);
            this.mnuPesquisarTodasAlteracoes.Tag = "PESQUISAR_TUDO";
            this.mnuPesquisarTodasAlteracoes.Text = "Pesquisar todas as alterações";
            // 
            // mnuPesquisarUltimaSemana
            // 
            this.mnuPesquisarUltimaSemana.Name = "mnuPesquisarUltimaSemana";
            this.mnuPesquisarUltimaSemana.Size = new System.Drawing.Size(290, 22);
            this.mnuPesquisarUltimaSemana.Tag = "PESQUISAR_ULTIMA_SEMANA";
            this.mnuPesquisarUltimaSemana.Text = "Pesquisar alterações da última semana";
            // 
            // mnuPesquisarUltimas24Horas
            // 
            this.mnuPesquisarUltimas24Horas.Name = "mnuPesquisarUltimas24Horas";
            this.mnuPesquisarUltimas24Horas.Size = new System.Drawing.Size(290, 22);
            this.mnuPesquisarUltimas24Horas.Tag = "PESQUISAR_ULTIMAS_24_HORAS";
            this.mnuPesquisarUltimas24Horas.Text = "Pesquisar alterações das últimas 24 horas";
            // 
            // mnuPesquisarUltimas72Horas
            // 
            this.mnuPesquisarUltimas72Horas.Name = "mnuPesquisarUltimas72Horas";
            this.mnuPesquisarUltimas72Horas.Size = new System.Drawing.Size(290, 22);
            this.mnuPesquisarUltimas72Horas.Tag = "PESQUISAR_ULTIMAS_72_HORAS";
            this.mnuPesquisarUltimas72Horas.Text = "Pesquisar alterações das últimas 72 horas";
            // 
            // mnuListarConflitos
            // 
            this.mnuListarConflitos.Name = "mnuListarConflitos";
            this.mnuListarConflitos.Size = new System.Drawing.Size(290, 22);
            this.mnuListarConflitos.Tag = "LISTAR_CONFLITOS";
            this.mnuListarConflitos.Text = "Listar conflitos";
            // 
            // mnuChamarPreecheUsuarios
            // 
            this.mnuChamarPreecheUsuarios.Name = "mnuChamarPreecheUsuarios";
            this.mnuChamarPreecheUsuarios.Size = new System.Drawing.Size(290, 22);
            this.mnuChamarPreecheUsuarios.Tag = "CHAMAR_PREENCHE_USUARIOS";
            this.mnuChamarPreecheUsuarios.Text = "Chamar \"Preenche Usuários\"";
            // 
            // mnuChamarProcuraConflitos
            // 
            this.mnuChamarProcuraConflitos.Name = "mnuChamarProcuraConflitos";
            this.mnuChamarProcuraConflitos.Size = new System.Drawing.Size(290, 22);
            this.mnuChamarProcuraConflitos.Tag = "CHAMAR_PROCURA_CONFLITOS";
            this.mnuChamarProcuraConflitos.Text = "Chamar \"Procura Conflitos\"";
            // 
            // btMenuContextoBotoes
            // 
            this.btMenuContextoBotoes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btMenuContextoBotoes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btMenuContextoBotoes.Location = new System.Drawing.Point(606, 29);
            this.btMenuContextoBotoes.Name = "btMenuContextoBotoes";
            this.btMenuContextoBotoes.Size = new System.Drawing.Size(24, 24);
            this.btMenuContextoBotoes.TabIndex = 45;
            this.btMenuContextoBotoes.Text = ">";
            this.btMenuContextoBotoes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btMenuContextoBotoes.UseVisualStyleBackColor = true;
            this.btMenuContextoBotoes.Click += new System.EventHandler(this.btMenuContextoBotoes_Click);
            // 
            // btPesquisar
            // 
            this.btPesquisar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btPesquisar.Image = global::Oracle_Tools.Properties.Resources.Lupa_Muito_Pequena;
            this.btPesquisar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btPesquisar.Location = new System.Drawing.Point(583, 29);
            this.btPesquisar.Name = "btPesquisar";
            this.btPesquisar.Size = new System.Drawing.Size(24, 24);
            this.btPesquisar.TabIndex = 46;
            this.btPesquisar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btPesquisar.UseVisualStyleBackColor = true;
            this.btPesquisar.Click += new System.EventHandler(this.btPesquisar_Click);
            this.btPesquisar.MouseEnter += new System.EventHandler(this.btPesquisar_MouseEnter);
            // 
            // mnuMenu
            // 
            this.mnuMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuInfoBanco});
            this.mnuMenu.Location = new System.Drawing.Point(0, 0);
            this.mnuMenu.Name = "mnuMenu";
            this.mnuMenu.Size = new System.Drawing.Size(1074, 24);
            this.mnuMenu.TabIndex = 47;
            this.mnuMenu.Text = "menuStrip1";
            // 
            // mnuInfoBanco
            // 
            this.mnuInfoBanco.Name = "mnuInfoBanco";
            this.mnuInfoBanco.Size = new System.Drawing.Size(76, 20);
            this.mnuInfoBanco.Text = "Info Banco";
            this.mnuInfoBanco.Click += new System.EventHandler(this.mnuInfoBanco_Click);
            // 
            // tmrMnuContextoBotoes
            // 
            this.tmrMnuContextoBotoes.Tick += new System.EventHandler(this.tmrMnuContextoBotoes_Tick);
            // 
            // tmrMnuContextoLvw
            // 
            this.tmrMnuContextoLvw.Tick += new System.EventHandler(this.tmrMnuContextoLvw_Tick);
            // 
            // btColorir
            // 
            this.btColorir.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btColorir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btColorir.Location = new System.Drawing.Point(649, 28);
            this.btColorir.Name = "btColorir";
            this.btColorir.Size = new System.Drawing.Size(56, 24);
            this.btColorir.TabIndex = 48;
            this.btColorir.Text = "Colorir";
            this.btColorir.UseVisualStyleBackColor = true;
            this.btColorir.Click += new System.EventHandler(this.btColorir_Click);
            // 
            // frmControleConcorrencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1074, 423);
            this.Controls.Add(this.btColorir);
            this.Controls.Add(this.mnuMenu);
            this.Controls.Add(this.btPesquisar);
            this.Controls.Add(this.btMenuContextoBotoes);
            this.Controls.Add(this.lblStatusRelatorio);
            this.Controls.Add(this.chkConsultarLogBranch);
            this.Controls.Add(this.lvwRelatorio);
            this.Controls.Add(this.lblStatusListaSecundaria);
            this.Controls.Add(this.lblStatusListaPrincipal);
            this.Controls.Add(this.lvwPesquisaSecundaria);
            this.Controls.Add(this.lblPesquisaSecundaria);
            this.Controls.Add(this.chkSelecionarTodos);
            this.Controls.Add(this.lvwObjetos);
            this.Controls.Add(this.txtPesquisa);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuMenu;
            this.Name = "frmControleConcorrencia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Controle de concorrência de Objetos";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mnuContextoBotoes.ResumeLayout(false);
            this.mnuMenu.ResumeLayout(false);
            this.mnuMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPesquisa;
        private System.Windows.Forms.CheckBox chkSelecionarTodos;
        public System.Windows.Forms.ListView lvwObjetos;
        private System.Windows.Forms.Timer tmrDigitacao;
        private System.Windows.Forms.ColumnHeader colDataHora;
        private System.Windows.Forms.ColumnHeader colTipo;
        private System.Windows.Forms.ColumnHeader colOwner;
        private System.Windows.Forms.ColumnHeader colNome;
        private System.Windows.Forms.ColumnHeader colAnalista;
        private System.Windows.Forms.Label lblPesquisaSecundaria;
        public System.Windows.Forms.ListView lvwPesquisaSecundaria;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Label lblStatusListaPrincipal;
        private System.Windows.Forms.Label lblStatusListaSecundaria;
        public System.Windows.Forms.ListView lvwRelatorio;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader colOSUSER;
        private System.Windows.Forms.ColumnHeader colDBUser;
        private System.Windows.Forms.ColumnHeader colHost;
        private System.Windows.Forms.ColumnHeader colEmpresa;
        private System.Windows.Forms.CheckBox chkConsultarLogBranch;
        private System.Windows.Forms.Label lblStatusRelatorio;
        private System.Windows.Forms.ContextMenuStrip mnuContextoLvw;
        private System.Windows.Forms.ContextMenuStrip mnuContextoBotoes;
        private System.Windows.Forms.ToolStripMenuItem mnuPesquisarTodasAlteracoes;
        private System.Windows.Forms.ToolStripMenuItem mnuPesquisarUltimaSemana;
        private System.Windows.Forms.ToolStripMenuItem mnuPesquisarUltimas24Horas;
        private System.Windows.Forms.ToolStripMenuItem mnuListarConflitos;
        private System.Windows.Forms.ToolStripMenuItem mnuChamarPreecheUsuarios;
        private System.Windows.Forms.ToolStripMenuItem mnuChamarProcuraConflitos;
        private System.Windows.Forms.Button btMenuContextoBotoes;
        private System.Windows.Forms.Button btPesquisar;
        private System.Windows.Forms.ToolStripMenuItem mnuPesquisarUltimas72Horas;
        private System.Windows.Forms.MenuStrip mnuMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuInfoBanco;
        private System.Windows.Forms.Timer tmrMnuContextoBotoes;
        private System.Windows.Forms.Timer tmrMnuContextoLvw;
        private System.Windows.Forms.Button btColorir;
    }
}