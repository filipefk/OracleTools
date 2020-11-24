namespace Oracle_Tools
{
    partial class frmDetalhesUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDetalhesUser));
            this.txtPesquisaUsuarioBanco = new System.Windows.Forms.TextBox();
            this.btPesquisarUsuarioBanco = new System.Windows.Forms.Button();
            this.mnuMenu = new System.Windows.Forms.MenuStrip();
            this.mnuInfoBanco = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPesquisaUsuarios = new System.Windows.Forms.TabControl();
            this.tabPagePermissoesOracle = new System.Windows.Forms.TabPage();
            this.trvUsuario = new System.Windows.Forms.TreeView();
            this.btRelatorio = new System.Windows.Forms.Button();
            this.btExpandirTudo = new System.Windows.Forms.Button();
            this.btExtrairDDLUsuario = new System.Windows.Forms.Button();
            this.tabPageLocalizarUsuario = new System.Windows.Forms.TabPage();
            this.txtRelatorio = new System.Windows.Forms.TextBox();
            this.btLocalizarUsuario = new System.Windows.Forms.Button();
            this.txtLocalizaUsuario = new System.Windows.Forms.TextBox();
            this.mnuMenu.SuspendLayout();
            this.tabPesquisaUsuarios.SuspendLayout();
            this.tabPagePermissoesOracle.SuspendLayout();
            this.tabPageLocalizarUsuario.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPesquisaUsuarioBanco
            // 
            this.txtPesquisaUsuarioBanco.Location = new System.Drawing.Point(8, 8);
            this.txtPesquisaUsuarioBanco.Name = "txtPesquisaUsuarioBanco";
            this.txtPesquisaUsuarioBanco.Size = new System.Drawing.Size(180, 20);
            this.txtPesquisaUsuarioBanco.TabIndex = 0;
            this.txtPesquisaUsuarioBanco.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPesquisaUsuarioBanco_KeyDown);
            // 
            // btPesquisarUsuarioBanco
            // 
            this.btPesquisarUsuarioBanco.Location = new System.Drawing.Point(194, 7);
            this.btPesquisarUsuarioBanco.Name = "btPesquisarUsuarioBanco";
            this.btPesquisarUsuarioBanco.Size = new System.Drawing.Size(86, 21);
            this.btPesquisarUsuarioBanco.TabIndex = 1;
            this.btPesquisarUsuarioBanco.Text = "Pesquisar";
            this.btPesquisarUsuarioBanco.UseVisualStyleBackColor = true;
            this.btPesquisarUsuarioBanco.Click += new System.EventHandler(this.btPesquisar_Click);
            // 
            // mnuMenu
            // 
            this.mnuMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuInfoBanco});
            this.mnuMenu.Location = new System.Drawing.Point(0, 0);
            this.mnuMenu.Name = "mnuMenu";
            this.mnuMenu.Size = new System.Drawing.Size(789, 24);
            this.mnuMenu.TabIndex = 48;
            this.mnuMenu.Text = "menuStrip1";
            // 
            // mnuInfoBanco
            // 
            this.mnuInfoBanco.Name = "mnuInfoBanco";
            this.mnuInfoBanco.Size = new System.Drawing.Size(76, 20);
            this.mnuInfoBanco.Text = "Info Banco";
            this.mnuInfoBanco.Click += new System.EventHandler(this.mnuInfoBanco_Click);
            // 
            // tabPesquisaUsuarios
            // 
            this.tabPesquisaUsuarios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPesquisaUsuarios.Controls.Add(this.tabPagePermissoesOracle);
            this.tabPesquisaUsuarios.Controls.Add(this.tabPageLocalizarUsuario);
            this.tabPesquisaUsuarios.Location = new System.Drawing.Point(0, 27);
            this.tabPesquisaUsuarios.Name = "tabPesquisaUsuarios";
            this.tabPesquisaUsuarios.SelectedIndex = 0;
            this.tabPesquisaUsuarios.Size = new System.Drawing.Size(789, 453);
            this.tabPesquisaUsuarios.TabIndex = 49;
            // 
            // tabPagePermissoesOracle
            // 
            this.tabPagePermissoesOracle.Controls.Add(this.btPesquisarUsuarioBanco);
            this.tabPagePermissoesOracle.Controls.Add(this.trvUsuario);
            this.tabPagePermissoesOracle.Controls.Add(this.btRelatorio);
            this.tabPagePermissoesOracle.Controls.Add(this.btExpandirTudo);
            this.tabPagePermissoesOracle.Controls.Add(this.txtPesquisaUsuarioBanco);
            this.tabPagePermissoesOracle.Controls.Add(this.btExtrairDDLUsuario);
            this.tabPagePermissoesOracle.Location = new System.Drawing.Point(4, 22);
            this.tabPagePermissoesOracle.Name = "tabPagePermissoesOracle";
            this.tabPagePermissoesOracle.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePermissoesOracle.Size = new System.Drawing.Size(781, 427);
            this.tabPagePermissoesOracle.TabIndex = 0;
            this.tabPagePermissoesOracle.Text = "Permissões no banco ORACLE";
            this.tabPagePermissoesOracle.UseVisualStyleBackColor = true;
            // 
            // trvUsuario
            // 
            this.trvUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trvUsuario.Location = new System.Drawing.Point(0, 33);
            this.trvUsuario.Name = "trvUsuario";
            this.trvUsuario.Size = new System.Drawing.Size(781, 394);
            this.trvUsuario.TabIndex = 8;
            this.trvUsuario.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvUsuario_BeforeExpand);
            this.trvUsuario.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trvUsuario_MouseDown);
            // 
            // btRelatorio
            // 
            this.btRelatorio.Location = new System.Drawing.Point(576, 7);
            this.btRelatorio.Name = "btRelatorio";
            this.btRelatorio.Size = new System.Drawing.Size(139, 21);
            this.btRelatorio.TabIndex = 7;
            this.btRelatorio.Text = "Gerar relatório";
            this.btRelatorio.UseVisualStyleBackColor = true;
            this.btRelatorio.Click += new System.EventHandler(this.btRelatorio_Click);
            // 
            // btExpandirTudo
            // 
            this.btExpandirTudo.Location = new System.Drawing.Point(431, 7);
            this.btExpandirTudo.Name = "btExpandirTudo";
            this.btExpandirTudo.Size = new System.Drawing.Size(139, 21);
            this.btExpandirTudo.TabIndex = 6;
            this.btExpandirTudo.Text = "Expandir tudo";
            this.btExpandirTudo.UseVisualStyleBackColor = true;
            this.btExpandirTudo.Click += new System.EventHandler(this.btExpandirTudo_Click);
            // 
            // btExtrairDDLUsuario
            // 
            this.btExtrairDDLUsuario.Location = new System.Drawing.Point(286, 7);
            this.btExtrairDDLUsuario.Name = "btExtrairDDLUsuario";
            this.btExtrairDDLUsuario.Size = new System.Drawing.Size(139, 21);
            this.btExtrairDDLUsuario.TabIndex = 4;
            this.btExtrairDDLUsuario.Text = "Extrair DDL do Usuário";
            this.btExtrairDDLUsuario.UseVisualStyleBackColor = true;
            this.btExtrairDDLUsuario.Click += new System.EventHandler(this.btExtrairDDLUsuario_Click);
            // 
            // tabPageLocalizarUsuario
            // 
            this.tabPageLocalizarUsuario.Controls.Add(this.txtRelatorio);
            this.tabPageLocalizarUsuario.Controls.Add(this.btLocalizarUsuario);
            this.tabPageLocalizarUsuario.Controls.Add(this.txtLocalizaUsuario);
            this.tabPageLocalizarUsuario.Location = new System.Drawing.Point(4, 22);
            this.tabPageLocalizarUsuario.Name = "tabPageLocalizarUsuario";
            this.tabPageLocalizarUsuario.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLocalizarUsuario.Size = new System.Drawing.Size(781, 427);
            this.tabPageLocalizarUsuario.TabIndex = 1;
            this.tabPageLocalizarUsuario.Text = "Localizar Usuário";
            this.tabPageLocalizarUsuario.UseVisualStyleBackColor = true;
            // 
            // txtRelatorio
            // 
            this.txtRelatorio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRelatorio.BackColor = System.Drawing.Color.White;
            this.txtRelatorio.Location = new System.Drawing.Point(0, 34);
            this.txtRelatorio.Multiline = true;
            this.txtRelatorio.Name = "txtRelatorio";
            this.txtRelatorio.ReadOnly = true;
            this.txtRelatorio.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRelatorio.Size = new System.Drawing.Size(785, 393);
            this.txtRelatorio.TabIndex = 52;
            this.txtRelatorio.WordWrap = false;
            this.txtRelatorio.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRelatorio_KeyDown);
            // 
            // btLocalizarUsuario
            // 
            this.btLocalizarUsuario.Location = new System.Drawing.Point(194, 7);
            this.btLocalizarUsuario.Name = "btLocalizarUsuario";
            this.btLocalizarUsuario.Size = new System.Drawing.Size(86, 21);
            this.btLocalizarUsuario.TabIndex = 51;
            this.btLocalizarUsuario.Text = "Localizar";
            this.btLocalizarUsuario.UseVisualStyleBackColor = true;
            this.btLocalizarUsuario.Click += new System.EventHandler(this.btLocalizarUsuario_Click);
            // 
            // txtLocalizaUsuario
            // 
            this.txtLocalizaUsuario.Location = new System.Drawing.Point(8, 8);
            this.txtLocalizaUsuario.Name = "txtLocalizaUsuario";
            this.txtLocalizaUsuario.Size = new System.Drawing.Size(180, 20);
            this.txtLocalizaUsuario.TabIndex = 50;
            this.txtLocalizaUsuario.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLocalizaUsuario_KeyDown);
            // 
            // frmDetalhesUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 477);
            this.Controls.Add(this.tabPesquisaUsuarios);
            this.Controls.Add(this.mnuMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDetalhesUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detalhes Usuário";
            this.mnuMenu.ResumeLayout(false);
            this.mnuMenu.PerformLayout();
            this.tabPesquisaUsuarios.ResumeLayout(false);
            this.tabPagePermissoesOracle.ResumeLayout(false);
            this.tabPagePermissoesOracle.PerformLayout();
            this.tabPageLocalizarUsuario.ResumeLayout(false);
            this.tabPageLocalizarUsuario.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPesquisaUsuarioBanco;
        private System.Windows.Forms.Button btPesquisarUsuarioBanco;
        private System.Windows.Forms.MenuStrip mnuMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuInfoBanco;
        private System.Windows.Forms.TabControl tabPesquisaUsuarios;
        private System.Windows.Forms.TabPage tabPagePermissoesOracle;
        private System.Windows.Forms.TreeView trvUsuario;
        private System.Windows.Forms.Button btRelatorio;
        private System.Windows.Forms.Button btExpandirTudo;
        private System.Windows.Forms.Button btExtrairDDLUsuario;
        private System.Windows.Forms.TabPage tabPageLocalizarUsuario;
        private System.Windows.Forms.Button btLocalizarUsuario;
        private System.Windows.Forms.TextBox txtLocalizaUsuario;
        private System.Windows.Forms.TextBox txtRelatorio;
    }
}