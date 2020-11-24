namespace Oracle_Tools
{
    partial class frmExtractUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExtractUser));
            this.cboOwners = new System.Windows.Forms.ComboBox();
            this.lblOwner = new System.Windows.Forms.Label();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cboProfile = new System.Windows.Forms.ComboBox();
            this.btListarUsuarios = new System.Windows.Forms.Button();
            this.lvwUsuarios = new System.Windows.Forms.ListView();
            this.colNome = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDataCriacao = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colProfile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDataLock = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDataExp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colUltimoLogin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btExtrairLvw = new System.Windows.Forms.Button();
            this.txtCaminhoLvw = new System.Windows.Forms.TextBox();
            this.lblCaminhoLvw = new System.Windows.Forms.Label();
            this.btEscolherPastaLvw = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btEscolherPastaTxt = new System.Windows.Forms.Button();
            this.lblCaminhoTxt = new System.Windows.Forms.Label();
            this.txtCaminhoTxt = new System.Windows.Forms.TextBox();
            this.btExtrairTxt = new System.Windows.Forms.Button();
            this.txtUsuarios = new System.Windows.Forms.TextBox();
            this.stStatusStrip = new System.Windows.Forms.StatusStrip();
            this.pbrStatus = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusLista = new System.Windows.Forms.Label();
            this.chkSelecionarTodos = new System.Windows.Forms.CheckBox();
            this.mnuMenu = new System.Windows.Forms.MenuStrip();
            this.mnuArquivo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuArquivoConectar = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuArquivoDetalhesUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.infoBancoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtNomeUsuario = new System.Windows.Forms.TextBox();
            this.lblNomeObjeto = new System.Windows.Forms.Label();
            this.mnuContextoLvw = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.stStatusStrip.SuspendLayout();
            this.mnuMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboOwners
            // 
            this.cboOwners.FormattingEnabled = true;
            this.cboOwners.Location = new System.Drawing.Point(118, 11);
            this.cboOwners.Name = "cboOwners";
            this.cboOwners.Size = new System.Drawing.Size(153, 21);
            this.cboOwners.TabIndex = 0;
            // 
            // lblOwner
            // 
            this.lblOwner.AutoSize = true;
            this.lblOwner.Location = new System.Drawing.Point(71, 14);
            this.lblOwner.Name = "lblOwner";
            this.lblOwner.Size = new System.Drawing.Size(41, 13);
            this.lblOwner.TabIndex = 1;
            this.lblOwner.Text = "Owner:";
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(279, 14);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(39, 13);
            this.lblTipo.TabIndex = 3;
            this.lblTipo.Text = "Profile:";
            // 
            // cboProfile
            // 
            this.cboProfile.FormattingEnabled = true;
            this.cboProfile.Location = new System.Drawing.Point(324, 11);
            this.cboProfile.Name = "cboProfile";
            this.cboProfile.Size = new System.Drawing.Size(153, 21);
            this.cboProfile.TabIndex = 2;
            // 
            // btListarUsuarios
            // 
            this.btListarUsuarios.Location = new System.Drawing.Point(682, 10);
            this.btListarUsuarios.Name = "btListarUsuarios";
            this.btListarUsuarios.Size = new System.Drawing.Size(89, 21);
            this.btListarUsuarios.TabIndex = 4;
            this.btListarUsuarios.Text = "Listar";
            this.btListarUsuarios.UseVisualStyleBackColor = true;
            this.btListarUsuarios.Click += new System.EventHandler(this.btListarUsuarios_Click);
            // 
            // lvwUsuarios
            // 
            this.lvwUsuarios.AllowDrop = true;
            this.lvwUsuarios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwUsuarios.CheckBoxes = true;
            this.lvwUsuarios.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colNome,
            this.colDataCriacao,
            this.colProfile,
            this.colStatus,
            this.colDataLock,
            this.colDataExp,
            this.colUltimoLogin});
            this.lvwUsuarios.FullRowSelect = true;
            this.lvwUsuarios.GridLines = true;
            this.lvwUsuarios.HideSelection = false;
            this.lvwUsuarios.Location = new System.Drawing.Point(12, 39);
            this.lvwUsuarios.Name = "lvwUsuarios";
            this.lvwUsuarios.Size = new System.Drawing.Size(994, 276);
            this.lvwUsuarios.TabIndex = 15;
            this.lvwUsuarios.UseCompatibleStateImageBehavior = false;
            this.lvwUsuarios.View = System.Windows.Forms.View.Details;
            this.lvwUsuarios.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwUsuarios_ColumnClick);
            this.lvwUsuarios.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvwUsuarios_ItemChecked);
            this.lvwUsuarios.SelectedIndexChanged += new System.EventHandler(this.lvwUsuarios_SelectedIndexChanged);
            this.lvwUsuarios.DoubleClick += new System.EventHandler(this.lvwUsuarios_DoubleClick);
            this.lvwUsuarios.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvwUsuarios_KeyDown);
            this.lvwUsuarios.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvwUsuarios_MouseDown);
            // 
            // colNome
            // 
            this.colNome.Text = "     Nome";
            this.colNome.Width = 118;
            // 
            // colDataCriacao
            // 
            this.colDataCriacao.Text = "Criação";
            this.colDataCriacao.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colDataCriacao.Width = 120;
            // 
            // colProfile
            // 
            this.colProfile.Text = "Profile";
            this.colProfile.Width = 149;
            // 
            // colStatus
            // 
            this.colStatus.Text = "Status";
            this.colStatus.Width = 150;
            // 
            // colDataLock
            // 
            this.colDataLock.Text = "Data de Lock";
            this.colDataLock.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colDataLock.Width = 120;
            // 
            // colDataExp
            // 
            this.colDataExp.Text = "Data Expiração";
            this.colDataExp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colDataExp.Width = 120;
            // 
            // colUltimoLogin
            // 
            this.colUltimoLogin.Text = "Último Login";
            this.colUltimoLogin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colUltimoLogin.Width = 120;
            // 
            // btExtrairLvw
            // 
            this.btExtrairLvw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExtrairLvw.Location = new System.Drawing.Point(840, 321);
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
            this.txtCaminhoLvw.Size = new System.Drawing.Size(711, 20);
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
            this.btEscolherPastaLvw.Location = new System.Drawing.Point(786, 335);
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
            this.groupBox1.Size = new System.Drawing.Size(1019, 8);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // btEscolherPastaTxt
            // 
            this.btEscolherPastaTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEscolherPastaTxt.Location = new System.Drawing.Point(786, 501);
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
            this.txtCaminhoTxt.Size = new System.Drawing.Size(711, 20);
            this.txtCaminhoTxt.TabIndex = 22;
            // 
            // btExtrairTxt
            // 
            this.btExtrairTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExtrairTxt.Location = new System.Drawing.Point(840, 491);
            this.btExtrairTxt.Name = "btExtrairTxt";
            this.btExtrairTxt.Size = new System.Drawing.Size(166, 40);
            this.btExtrairTxt.TabIndex = 21;
            this.btExtrairTxt.Text = "Extrair Objetos Listados";
            this.btExtrairTxt.UseVisualStyleBackColor = true;
            this.btExtrairTxt.Click += new System.EventHandler(this.btExtrairTxt_Click);
            // 
            // txtUsuarios
            // 
            this.txtUsuarios.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsuarios.Location = new System.Drawing.Point(12, 381);
            this.txtUsuarios.Multiline = true;
            this.txtUsuarios.Name = "txtUsuarios";
            this.txtUsuarios.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtUsuarios.Size = new System.Drawing.Size(994, 104);
            this.txtUsuarios.TabIndex = 25;
            this.txtUsuarios.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUsuarios_KeyDown);
            // 
            // stStatusStrip
            // 
            this.stStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbrStatus,
            this.lblStatus});
            this.stStatusStrip.Location = new System.Drawing.Point(0, 534);
            this.stStatusStrip.Name = "stStatusStrip";
            this.stStatusStrip.Size = new System.Drawing.Size(1018, 22);
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
            this.lblStatusLista.Location = new System.Drawing.Point(777, 14);
            this.lblStatusLista.Name = "lblStatusLista";
            this.lblStatusLista.Size = new System.Drawing.Size(181, 13);
            this.lblStatusLista.TabIndex = 27;
            this.lblStatusLista.Text = "0 itens listados - 0 itens selecionados";
            // 
            // chkSelecionarTodos
            // 
            this.chkSelecionarTodos.AutoSize = true;
            this.chkSelecionarTodos.Location = new System.Drawing.Point(17, 45);
            this.chkSelecionarTodos.Name = "chkSelecionarTodos";
            this.chkSelecionarTodos.Size = new System.Drawing.Size(15, 14);
            this.chkSelecionarTodos.TabIndex = 28;
            this.chkSelecionarTodos.UseVisualStyleBackColor = true;
            this.chkSelecionarTodos.CheckedChanged += new System.EventHandler(this.chkSelecionarTodos_CheckedChanged);
            // 
            // mnuMenu
            // 
            this.mnuMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuArquivo});
            this.mnuMenu.Location = new System.Drawing.Point(0, 0);
            this.mnuMenu.Name = "mnuMenu";
            this.mnuMenu.Size = new System.Drawing.Size(1018, 24);
            this.mnuMenu.TabIndex = 30;
            this.mnuMenu.Text = "menuStrip1";
            // 
            // mnuArquivo
            // 
            this.mnuArquivo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuArquivoConectar,
            this.mnuArquivoDetalhesUsuarios,
            this.infoBancoToolStripMenuItem});
            this.mnuArquivo.Name = "mnuArquivo";
            this.mnuArquivo.Size = new System.Drawing.Size(61, 20);
            this.mnuArquivo.Text = "Arquivo";
            // 
            // mnuArquivoConectar
            // 
            this.mnuArquivoConectar.Name = "mnuArquivoConectar";
            this.mnuArquivoConectar.Size = new System.Drawing.Size(167, 22);
            this.mnuArquivoConectar.Text = "Conectar";
            this.mnuArquivoConectar.Click += new System.EventHandler(this.mnuArquivoConectar_Click);
            // 
            // mnuArquivoDetalhesUsuarios
            // 
            this.mnuArquivoDetalhesUsuarios.Name = "mnuArquivoDetalhesUsuarios";
            this.mnuArquivoDetalhesUsuarios.Size = new System.Drawing.Size(167, 22);
            this.mnuArquivoDetalhesUsuarios.Text = "Detalhes Usuários";
            this.mnuArquivoDetalhesUsuarios.Click += new System.EventHandler(this.mnuArquivoDetalhesUsuarios_Click);
            // 
            // infoBancoToolStripMenuItem
            // 
            this.infoBancoToolStripMenuItem.Name = "infoBancoToolStripMenuItem";
            this.infoBancoToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.infoBancoToolStripMenuItem.Text = "Info Banco";
            this.infoBancoToolStripMenuItem.Click += new System.EventHandler(this.infoBancoToolStripMenuItem_Click);
            // 
            // txtNomeUsuario
            // 
            this.txtNomeUsuario.Location = new System.Drawing.Point(521, 11);
            this.txtNomeUsuario.Name = "txtNomeUsuario";
            this.txtNomeUsuario.Size = new System.Drawing.Size(155, 20);
            this.txtNomeUsuario.TabIndex = 38;
            // 
            // lblNomeObjeto
            // 
            this.lblNomeObjeto.AutoSize = true;
            this.lblNomeObjeto.Location = new System.Drawing.Point(486, 14);
            this.lblNomeObjeto.Name = "lblNomeObjeto";
            this.lblNomeObjeto.Size = new System.Drawing.Size(38, 13);
            this.lblNomeObjeto.TabIndex = 37;
            this.lblNomeObjeto.Text = "Nome:";
            // 
            // mnuContextoLvw
            // 
            this.mnuContextoLvw.Name = "mnuMenu";
            this.mnuContextoLvw.ShowImageMargin = false;
            this.mnuContextoLvw.Size = new System.Drawing.Size(36, 4);
            this.mnuContextoLvw.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuContextoLvw_ItemClicked);
            // 
            // frmExtractUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 556);
            this.Controls.Add(this.txtNomeUsuario);
            this.Controls.Add(this.lblNomeObjeto);
            this.Controls.Add(this.lblStatusLista);
            this.Controls.Add(this.btListarUsuarios);
            this.Controls.Add(this.cboProfile);
            this.Controls.Add(this.lblTipo);
            this.Controls.Add(this.lblOwner);
            this.Controls.Add(this.cboOwners);
            this.Controls.Add(this.chkSelecionarTodos);
            this.Controls.Add(this.stStatusStrip);
            this.Controls.Add(this.mnuMenu);
            this.Controls.Add(this.txtUsuarios);
            this.Controls.Add(this.btEscolherPastaTxt);
            this.Controls.Add(this.lblCaminhoTxt);
            this.Controls.Add(this.txtCaminhoTxt);
            this.Controls.Add(this.btExtrairTxt);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btEscolherPastaLvw);
            this.Controls.Add(this.lblCaminhoLvw);
            this.Controls.Add(this.txtCaminhoLvw);
            this.Controls.Add(this.btExtrairLvw);
            this.Controls.Add(this.lvwUsuarios);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuMenu;
            this.MinimumSize = new System.Drawing.Size(368, 413);
            this.Name = "frmExtractUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Extract User Oracle";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmExtractUser_FormClosed);
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
        private System.Windows.Forms.ComboBox cboProfile;
        private System.Windows.Forms.Button btListarUsuarios;
        public System.Windows.Forms.ListView lvwUsuarios;
        private System.Windows.Forms.Button btExtrairLvw;
        private System.Windows.Forms.TextBox txtCaminhoLvw;
        private System.Windows.Forms.Label lblCaminhoLvw;
        private System.Windows.Forms.Button btEscolherPastaLvw;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btEscolherPastaTxt;
        private System.Windows.Forms.Label lblCaminhoTxt;
        private System.Windows.Forms.TextBox txtCaminhoTxt;
        private System.Windows.Forms.Button btExtrairTxt;
        private System.Windows.Forms.TextBox txtUsuarios;
        private System.Windows.Forms.StatusStrip stStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar pbrStatus;
        private System.Windows.Forms.Label lblStatusLista;
        private System.Windows.Forms.CheckBox chkSelecionarTodos;
        private System.Windows.Forms.MenuStrip mnuMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuArquivo;
        private System.Windows.Forms.ToolStripMenuItem mnuArquivoConectar;
        private System.Windows.Forms.ColumnHeader colNome;
        private System.Windows.Forms.ColumnHeader colDataCriacao;
        private System.Windows.Forms.ColumnHeader colProfile;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ColumnHeader colDataLock;
        private System.Windows.Forms.ColumnHeader colDataExp;
        private System.Windows.Forms.ColumnHeader colUltimoLogin;
        private System.Windows.Forms.ToolStripMenuItem mnuArquivoDetalhesUsuarios;
        private System.Windows.Forms.ToolStripMenuItem infoBancoToolStripMenuItem;
        private System.Windows.Forms.TextBox txtNomeUsuario;
        private System.Windows.Forms.Label lblNomeObjeto;
        private System.Windows.Forms.ContextMenuStrip mnuContextoLvw;
    }
}