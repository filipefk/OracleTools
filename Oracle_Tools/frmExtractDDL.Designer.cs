namespace Oracle_Tools
{
    partial class frmExtractDDL
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExtractDDL));
            this.cboOwners = new System.Windows.Forms.ComboBox();
            this.lblOwner = new System.Windows.Forms.Label();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cboTipoObjeto = new System.Windows.Forms.ComboBox();
            this.btListarObjetos = new System.Windows.Forms.Button();
            this.lvwObjetos = new System.Windows.Forms.ListView();
            this.colOwner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNome = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTipo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDataCriacao = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDataAlteracao = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMensagem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colUltimoAlterador = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btExtrairLvw = new System.Windows.Forms.Button();
            this.txtCaminhoLvw = new System.Windows.Forms.TextBox();
            this.lblCaminhoLvw = new System.Windows.Forms.Label();
            this.btEscolherPastaLvw = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btEscolherPastaTxt = new System.Windows.Forms.Button();
            this.lblCaminhoTxt = new System.Windows.Forms.Label();
            this.txtCaminhoTxt = new System.Windows.Forms.TextBox();
            this.btExtrairTxt = new System.Windows.Forms.Button();
            this.txtObjetos = new System.Windows.Forms.TextBox();
            this.stStatusStrip = new System.Windows.Forms.StatusStrip();
            this.pbrStatus = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusLista = new System.Windows.Forms.Label();
            this.chkSelecionarTodos = new System.Windows.Forms.CheckBox();
            this.btUltimasCompilacoes = new System.Windows.Forms.Button();
            this.mnuMenu = new System.Windows.Forms.MenuStrip();
            this.mnuArquivo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuArquivoConectar = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuArquivoParametros = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuArquivoInfoBanco = new System.Windows.Forms.ToolStripMenuItem();
            this.txtDias = new System.Windows.Forms.TextBox();
            this.btListarInvalidos = new System.Windows.Forms.Button();
            this.btRecompilarInvalidos = new System.Windows.Forms.Button();
            this.lblNomeObjeto = new System.Windows.Forms.Label();
            this.txtNomeObjeto = new System.Windows.Forms.TextBox();
            this.mnuContextoLvw = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmrTimerMenuContexto = new System.Windows.Forms.Timer(this.components);
            this.stStatusStrip.SuspendLayout();
            this.mnuMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboOwners
            // 
            this.cboOwners.FormattingEnabled = true;
            this.cboOwners.Location = new System.Drawing.Point(50, 26);
            this.cboOwners.Name = "cboOwners";
            this.cboOwners.Size = new System.Drawing.Size(153, 21);
            this.cboOwners.TabIndex = 0;
            // 
            // lblOwner
            // 
            this.lblOwner.AutoSize = true;
            this.lblOwner.Location = new System.Drawing.Point(12, 30);
            this.lblOwner.Name = "lblOwner";
            this.lblOwner.Size = new System.Drawing.Size(41, 13);
            this.lblOwner.TabIndex = 1;
            this.lblOwner.Text = "Owner:";
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(209, 30);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(31, 13);
            this.lblTipo.TabIndex = 3;
            this.lblTipo.Text = "Tipo:";
            // 
            // cboTipoObjeto
            // 
            this.cboTipoObjeto.FormattingEnabled = true;
            this.cboTipoObjeto.Location = new System.Drawing.Point(236, 27);
            this.cboTipoObjeto.Name = "cboTipoObjeto";
            this.cboTipoObjeto.Size = new System.Drawing.Size(153, 21);
            this.cboTipoObjeto.TabIndex = 2;
            // 
            // btListarObjetos
            // 
            this.btListarObjetos.Location = new System.Drawing.Point(591, 28);
            this.btListarObjetos.Name = "btListarObjetos";
            this.btListarObjetos.Size = new System.Drawing.Size(56, 21);
            this.btListarObjetos.TabIndex = 4;
            this.btListarObjetos.Text = "Listar";
            this.btListarObjetos.UseVisualStyleBackColor = true;
            this.btListarObjetos.Click += new System.EventHandler(this.btListarObjetos_Click);
            // 
            // lvwObjetos
            // 
            this.lvwObjetos.AllowDrop = true;
            this.lvwObjetos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwObjetos.CheckBoxes = true;
            this.lvwObjetos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colOwner,
            this.colNome,
            this.colTipo,
            this.colDataCriacao,
            this.colDataAlteracao,
            this.colStatus,
            this.colMensagem,
            this.colUltimoAlterador});
            this.lvwObjetos.FullRowSelect = true;
            this.lvwObjetos.GridLines = true;
            this.lvwObjetos.HideSelection = false;
            this.lvwObjetos.Location = new System.Drawing.Point(12, 54);
            this.lvwObjetos.Name = "lvwObjetos";
            this.lvwObjetos.Size = new System.Drawing.Size(1193, 261);
            this.lvwObjetos.TabIndex = 15;
            this.lvwObjetos.UseCompatibleStateImageBehavior = false;
            this.lvwObjetos.View = System.Windows.Forms.View.Details;
            this.lvwObjetos.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwObjetos_ColumnClick);
            this.lvwObjetos.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvwObjetos_ItemChecked);
            this.lvwObjetos.SelectedIndexChanged += new System.EventHandler(this.lvwObjetos_SelectedIndexChanged);
            this.lvwObjetos.DoubleClick += new System.EventHandler(this.lvwObjetos_DoubleClick);
            this.lvwObjetos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvwObjetos_KeyDown);
            this.lvwObjetos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvwObjetos_MouseDown);
            // 
            // colOwner
            // 
            this.colOwner.Text = "     Owner";
            this.colOwner.Width = 106;
            // 
            // colNome
            // 
            this.colNome.Text = "Nome";
            this.colNome.Width = 349;
            // 
            // colTipo
            // 
            this.colTipo.Text = "Tipo";
            this.colTipo.Width = 136;
            // 
            // colDataCriacao
            // 
            this.colDataCriacao.Text = "Data Criação";
            this.colDataCriacao.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colDataCriacao.Width = 120;
            // 
            // colDataAlteracao
            // 
            this.colDataAlteracao.Text = "Data Alteração";
            this.colDataAlteracao.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colDataAlteracao.Width = 120;
            // 
            // colStatus
            // 
            this.colStatus.Text = "Status";
            this.colStatus.Width = 77;
            // 
            // colMensagem
            // 
            this.colMensagem.Text = "Mensagem";
            this.colMensagem.Width = 69;
            // 
            // colUltimoAlterador
            // 
            this.colUltimoAlterador.Text = "Último Alterador";
            this.colUltimoAlterador.Width = 200;
            // 
            // btExtrairLvw
            // 
            this.btExtrairLvw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExtrairLvw.Location = new System.Drawing.Point(1039, 321);
            this.btExtrairLvw.Name = "btExtrairLvw";
            this.btExtrairLvw.Size = new System.Drawing.Size(166, 40);
            this.btExtrairLvw.TabIndex = 16;
            this.btExtrairLvw.Text = "Extrair Objetos Selecionados";
            this.btExtrairLvw.UseVisualStyleBackColor = true;
            this.btExtrairLvw.Click += new System.EventHandler(this.btExtrairLvw_Click);
            // 
            // txtCaminhoLvw
            // 
            this.txtCaminhoLvw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCaminhoLvw.Location = new System.Drawing.Point(69, 336);
            this.txtCaminhoLvw.Name = "txtCaminhoLvw";
            this.txtCaminhoLvw.Size = new System.Drawing.Size(910, 20);
            this.txtCaminhoLvw.TabIndex = 17;
            // 
            // lblCaminhoLvw
            // 
            this.lblCaminhoLvw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCaminhoLvw.AutoSize = true;
            this.lblCaminhoLvw.Location = new System.Drawing.Point(12, 339);
            this.lblCaminhoLvw.Name = "lblCaminhoLvw";
            this.lblCaminhoLvw.Size = new System.Drawing.Size(51, 13);
            this.lblCaminhoLvw.TabIndex = 18;
            this.lblCaminhoLvw.Text = "Caminho:";
            // 
            // btEscolherPastaLvw
            // 
            this.btEscolherPastaLvw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEscolherPastaLvw.Location = new System.Drawing.Point(985, 335);
            this.btEscolherPastaLvw.Name = "btEscolherPastaLvw";
            this.btEscolherPastaLvw.Size = new System.Drawing.Size(27, 21);
            this.btEscolherPastaLvw.TabIndex = 19;
            this.btEscolherPastaLvw.Text = "...";
            this.btEscolherPastaLvw.UseVisualStyleBackColor = true;
            this.btEscolherPastaLvw.Click += new System.EventHandler(this.btEscolherPastaLvw_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(0, 367);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1218, 8);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // btEscolherPastaTxt
            // 
            this.btEscolherPastaTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEscolherPastaTxt.Location = new System.Drawing.Point(985, 501);
            this.btEscolherPastaTxt.Name = "btEscolherPastaTxt";
            this.btEscolherPastaTxt.Size = new System.Drawing.Size(27, 21);
            this.btEscolherPastaTxt.TabIndex = 24;
            this.btEscolherPastaTxt.Text = "...";
            this.btEscolherPastaTxt.UseVisualStyleBackColor = true;
            this.btEscolherPastaTxt.Click += new System.EventHandler(this.btEscolherPastaTxt_Click);
            // 
            // lblCaminhoTxt
            // 
            this.lblCaminhoTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCaminhoTxt.AutoSize = true;
            this.lblCaminhoTxt.Location = new System.Drawing.Point(12, 505);
            this.lblCaminhoTxt.Name = "lblCaminhoTxt";
            this.lblCaminhoTxt.Size = new System.Drawing.Size(51, 13);
            this.lblCaminhoTxt.TabIndex = 23;
            this.lblCaminhoTxt.Text = "Caminho:";
            // 
            // txtCaminhoTxt
            // 
            this.txtCaminhoTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCaminhoTxt.Location = new System.Drawing.Point(69, 502);
            this.txtCaminhoTxt.Name = "txtCaminhoTxt";
            this.txtCaminhoTxt.Size = new System.Drawing.Size(910, 20);
            this.txtCaminhoTxt.TabIndex = 22;
            // 
            // btExtrairTxt
            // 
            this.btExtrairTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExtrairTxt.Location = new System.Drawing.Point(1039, 491);
            this.btExtrairTxt.Name = "btExtrairTxt";
            this.btExtrairTxt.Size = new System.Drawing.Size(166, 40);
            this.btExtrairTxt.TabIndex = 21;
            this.btExtrairTxt.Text = "Extrair Objetos Listados";
            this.btExtrairTxt.UseVisualStyleBackColor = true;
            this.btExtrairTxt.Click += new System.EventHandler(this.btExtrairTxt_Click);
            // 
            // txtObjetos
            // 
            this.txtObjetos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObjetos.Location = new System.Drawing.Point(12, 381);
            this.txtObjetos.Multiline = true;
            this.txtObjetos.Name = "txtObjetos";
            this.txtObjetos.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtObjetos.Size = new System.Drawing.Size(1193, 104);
            this.txtObjetos.TabIndex = 25;
            this.txtObjetos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtObjetos_KeyDown);
            // 
            // stStatusStrip
            // 
            this.stStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbrStatus,
            this.lblStatus});
            this.stStatusStrip.Location = new System.Drawing.Point(0, 534);
            this.stStatusStrip.Name = "stStatusStrip";
            this.stStatusStrip.Size = new System.Drawing.Size(1217, 22);
            this.stStatusStrip.TabIndex = 26;
            this.stStatusStrip.Text = "statusStrip1";
            // 
            // pbrStatus
            // 
            this.pbrStatus.Name = "pbrStatus";
            this.pbrStatus.Size = new System.Drawing.Size(100, 16);
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.Color.Blue;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(44, 17);
            this.lblStatus.Text = "Parado";
            // 
            // lblStatusLista
            // 
            this.lblStatusLista.AutoSize = true;
            this.lblStatusLista.Location = new System.Drawing.Point(653, 32);
            this.lblStatusLista.Name = "lblStatusLista";
            this.lblStatusLista.Size = new System.Drawing.Size(181, 13);
            this.lblStatusLista.TabIndex = 27;
            this.lblStatusLista.Text = "0 itens listados - 0 itens selecionados";
            // 
            // chkSelecionarTodos
            // 
            this.chkSelecionarTodos.AutoSize = true;
            this.chkSelecionarTodos.Location = new System.Drawing.Point(17, 60);
            this.chkSelecionarTodos.Name = "chkSelecionarTodos";
            this.chkSelecionarTodos.Size = new System.Drawing.Size(15, 14);
            this.chkSelecionarTodos.TabIndex = 28;
            this.chkSelecionarTodos.UseVisualStyleBackColor = true;
            this.chkSelecionarTodos.CheckedChanged += new System.EventHandler(this.chkSelecionarTodos_CheckedChanged);
            // 
            // btUltimasCompilacoes
            // 
            this.btUltimasCompilacoes.Location = new System.Drawing.Point(878, 28);
            this.btUltimasCompilacoes.Name = "btUltimasCompilacoes";
            this.btUltimasCompilacoes.Size = new System.Drawing.Size(114, 21);
            this.btUltimasCompilacoes.TabIndex = 29;
            this.btUltimasCompilacoes.Text = "Ultimas Compilações";
            this.btUltimasCompilacoes.UseVisualStyleBackColor = true;
            this.btUltimasCompilacoes.Click += new System.EventHandler(this.btUltimasCompilacoes_Click);
            // 
            // mnuMenu
            // 
            this.mnuMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuArquivo});
            this.mnuMenu.Location = new System.Drawing.Point(0, 0);
            this.mnuMenu.Name = "mnuMenu";
            this.mnuMenu.Size = new System.Drawing.Size(1217, 24);
            this.mnuMenu.TabIndex = 30;
            this.mnuMenu.Text = "menuStrip1";
            // 
            // mnuArquivo
            // 
            this.mnuArquivo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuArquivoConectar,
            this.mnuArquivoParametros,
            this.toolStripMenuItem1,
            this.mnuArquivoInfoBanco});
            this.mnuArquivo.Name = "mnuArquivo";
            this.mnuArquivo.Size = new System.Drawing.Size(61, 20);
            this.mnuArquivo.Text = "Arquivo";
            // 
            // mnuArquivoConectar
            // 
            this.mnuArquivoConectar.Name = "mnuArquivoConectar";
            this.mnuArquivoConectar.Size = new System.Drawing.Size(134, 22);
            this.mnuArquivoConectar.Text = "Conectar";
            this.mnuArquivoConectar.Click += new System.EventHandler(this.mnuArquivoConectar_Click);
            // 
            // mnuArquivoParametros
            // 
            this.mnuArquivoParametros.Name = "mnuArquivoParametros";
            this.mnuArquivoParametros.Size = new System.Drawing.Size(134, 22);
            this.mnuArquivoParametros.Text = "Parâmetros";
            this.mnuArquivoParametros.Click += new System.EventHandler(this.mnuArquivoParametros_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(131, 6);
            // 
            // mnuArquivoInfoBanco
            // 
            this.mnuArquivoInfoBanco.Name = "mnuArquivoInfoBanco";
            this.mnuArquivoInfoBanco.Size = new System.Drawing.Size(134, 22);
            this.mnuArquivoInfoBanco.Text = "Info Banco";
            this.mnuArquivoInfoBanco.Click += new System.EventHandler(this.mnuArquivoInfoBanco_Click);
            // 
            // txtDias
            // 
            this.txtDias.Location = new System.Drawing.Point(840, 28);
            this.txtDias.Name = "txtDias";
            this.txtDias.Size = new System.Drawing.Size(32, 20);
            this.txtDias.TabIndex = 31;
            this.txtDias.Text = "15";
            this.txtDias.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btListarInvalidos
            // 
            this.btListarInvalidos.Location = new System.Drawing.Point(998, 28);
            this.btListarInvalidos.Name = "btListarInvalidos";
            this.btListarInvalidos.Size = new System.Drawing.Size(88, 21);
            this.btListarInvalidos.TabIndex = 32;
            this.btListarInvalidos.Text = "Listar Inválidos";
            this.btListarInvalidos.UseVisualStyleBackColor = true;
            this.btListarInvalidos.Click += new System.EventHandler(this.btListarInvalidos_Click);
            // 
            // btRecompilarInvalidos
            // 
            this.btRecompilarInvalidos.Location = new System.Drawing.Point(1092, 28);
            this.btRecompilarInvalidos.Name = "btRecompilarInvalidos";
            this.btRecompilarInvalidos.Size = new System.Drawing.Size(113, 21);
            this.btRecompilarInvalidos.TabIndex = 33;
            this.btRecompilarInvalidos.Text = "Recompilar Inválidos";
            this.btRecompilarInvalidos.UseVisualStyleBackColor = true;
            this.btRecompilarInvalidos.Click += new System.EventHandler(this.btRecompilarInvalidos_Click);
            // 
            // lblNomeObjeto
            // 
            this.lblNomeObjeto.AutoSize = true;
            this.lblNomeObjeto.Location = new System.Drawing.Point(395, 31);
            this.lblNomeObjeto.Name = "lblNomeObjeto";
            this.lblNomeObjeto.Size = new System.Drawing.Size(38, 13);
            this.lblNomeObjeto.TabIndex = 35;
            this.lblNomeObjeto.Text = "Nome:";
            // 
            // txtNomeObjeto
            // 
            this.txtNomeObjeto.Location = new System.Drawing.Point(430, 28);
            this.txtNomeObjeto.Name = "txtNomeObjeto";
            this.txtNomeObjeto.Size = new System.Drawing.Size(155, 20);
            this.txtNomeObjeto.TabIndex = 36;
            this.txtNomeObjeto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNomeObjeto_KeyDown);
            // 
            // mnuContextoLvw
            // 
            this.mnuContextoLvw.Name = "mnuMenu";
            this.mnuContextoLvw.ShowImageMargin = false;
            this.mnuContextoLvw.Size = new System.Drawing.Size(36, 4);
            this.mnuContextoLvw.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuContextoLvw_ItemClicked);
            // 
            // tmrTimerMenuContexto
            // 
            this.tmrTimerMenuContexto.Tick += new System.EventHandler(this.tmrTimerMenuContexto_Tick);
            // 
            // frmExtractDDL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1217, 556);
            this.Controls.Add(this.txtNomeObjeto);
            this.Controls.Add(this.cboOwners);
            this.Controls.Add(this.lblNomeObjeto);
            this.Controls.Add(this.btRecompilarInvalidos);
            this.Controls.Add(this.btListarInvalidos);
            this.Controls.Add(this.txtDias);
            this.Controls.Add(this.btUltimasCompilacoes);
            this.Controls.Add(this.lblStatusLista);
            this.Controls.Add(this.btListarObjetos);
            this.Controls.Add(this.cboTipoObjeto);
            this.Controls.Add(this.lblTipo);
            this.Controls.Add(this.lblOwner);
            this.Controls.Add(this.chkSelecionarTodos);
            this.Controls.Add(this.stStatusStrip);
            this.Controls.Add(this.mnuMenu);
            this.Controls.Add(this.txtObjetos);
            this.Controls.Add(this.btEscolherPastaTxt);
            this.Controls.Add(this.lblCaminhoTxt);
            this.Controls.Add(this.txtCaminhoTxt);
            this.Controls.Add(this.btExtrairTxt);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btEscolherPastaLvw);
            this.Controls.Add(this.lblCaminhoLvw);
            this.Controls.Add(this.txtCaminhoLvw);
            this.Controls.Add(this.btExtrairLvw);
            this.Controls.Add(this.lvwObjetos);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuMenu;
            this.MinimumSize = new System.Drawing.Size(368, 413);
            this.Name = "frmExtractDDL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Extract DDL Oracle";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmExtractDDL_FormClosed);
            this.stStatusStrip.ResumeLayout(false);
            this.stStatusStrip.PerformLayout();
            this.mnuMenu.ResumeLayout(false);
            this.mnuMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboOwners;
        private System.Windows.Forms.Label lblOwner;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cboTipoObjeto;
        private System.Windows.Forms.Button btListarObjetos;
        public System.Windows.Forms.ListView lvwObjetos;
        private System.Windows.Forms.ColumnHeader colOwner;
        public System.Windows.Forms.ColumnHeader colNome;
        public System.Windows.Forms.ColumnHeader colTipo;
        public System.Windows.Forms.ColumnHeader colDataCriacao;
        public System.Windows.Forms.ColumnHeader colDataAlteracao;
        private System.Windows.Forms.Button btExtrairLvw;
        private System.Windows.Forms.TextBox txtCaminhoLvw;
        private System.Windows.Forms.Label lblCaminhoLvw;
        private System.Windows.Forms.Button btEscolherPastaLvw;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btEscolherPastaTxt;
        private System.Windows.Forms.Label lblCaminhoTxt;
        private System.Windows.Forms.TextBox txtCaminhoTxt;
        private System.Windows.Forms.Button btExtrairTxt;
        private System.Windows.Forms.TextBox txtObjetos;
        private System.Windows.Forms.StatusStrip stStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar pbrStatus;
        private System.Windows.Forms.Label lblStatusLista;
        private System.Windows.Forms.CheckBox chkSelecionarTodos;
        private System.Windows.Forms.Button btUltimasCompilacoes;
        private System.Windows.Forms.MenuStrip mnuMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuArquivo;
        private System.Windows.Forms.ToolStripMenuItem mnuArquivoConectar;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.TextBox txtDias;
        private System.Windows.Forms.Button btListarInvalidos;
        private System.Windows.Forms.Button btRecompilarInvalidos;
        private System.Windows.Forms.ColumnHeader colMensagem;
        private System.Windows.Forms.ToolStripMenuItem mnuArquivoInfoBanco;
        private System.Windows.Forms.Label lblNomeObjeto;
        private System.Windows.Forms.TextBox txtNomeObjeto;
        private System.Windows.Forms.ColumnHeader colUltimoAlterador;
        private System.Windows.Forms.ContextMenuStrip mnuContextoLvw;
        private System.Windows.Forms.ToolStripMenuItem mnuArquivoParametros;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Timer tmrTimerMenuContexto;
    }
}