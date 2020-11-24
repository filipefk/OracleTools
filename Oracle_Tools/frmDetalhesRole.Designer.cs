namespace Oracle_Tools
{
    partial class frmDetalhesRole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDetalhesRole));
            this.trvRole = new System.Windows.Forms.TreeView();
            this.txtNomeRole = new System.Windows.Forms.TextBox();
            this.btPesquisar = new System.Windows.Forms.Button();
            this.btRelatorio = new System.Windows.Forms.Button();
            this.btExpandirTudo = new System.Windows.Forms.Button();
            this.mnuMenu = new System.Windows.Forms.MenuStrip();
            this.mnuInfoBanco = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // trvRole
            // 
            this.trvRole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trvRole.Location = new System.Drawing.Point(12, 54);
            this.trvRole.Name = "trvRole";
            this.trvRole.Size = new System.Drawing.Size(754, 429);
            this.trvRole.TabIndex = 2;
            this.trvRole.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvRole_BeforeExpand);
            this.trvRole.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trvRole_MouseDown);
            // 
            // txtNomeRole
            // 
            this.txtNomeRole.Location = new System.Drawing.Point(12, 28);
            this.txtNomeRole.Name = "txtNomeRole";
            this.txtNomeRole.Size = new System.Drawing.Size(180, 20);
            this.txtNomeRole.TabIndex = 0;
            // 
            // btPesquisar
            // 
            this.btPesquisar.Location = new System.Drawing.Point(198, 27);
            this.btPesquisar.Name = "btPesquisar";
            this.btPesquisar.Size = new System.Drawing.Size(86, 21);
            this.btPesquisar.TabIndex = 1;
            this.btPesquisar.Text = "Pesquisar";
            this.btPesquisar.UseVisualStyleBackColor = true;
            this.btPesquisar.Click += new System.EventHandler(this.btPesquisar_Click);
            // 
            // btRelatorio
            // 
            this.btRelatorio.Location = new System.Drawing.Point(435, 27);
            this.btRelatorio.Name = "btRelatorio";
            this.btRelatorio.Size = new System.Drawing.Size(139, 21);
            this.btRelatorio.TabIndex = 4;
            this.btRelatorio.Text = "Gerar relatório";
            this.btRelatorio.UseVisualStyleBackColor = true;
            this.btRelatorio.Click += new System.EventHandler(this.btRelatorio_Click);
            // 
            // btExpandirTudo
            // 
            this.btExpandirTudo.Location = new System.Drawing.Point(290, 27);
            this.btExpandirTudo.Name = "btExpandirTudo";
            this.btExpandirTudo.Size = new System.Drawing.Size(139, 21);
            this.btExpandirTudo.TabIndex = 5;
            this.btExpandirTudo.Text = "Expandir tudo";
            this.btExpandirTudo.UseVisualStyleBackColor = true;
            this.btExpandirTudo.Click += new System.EventHandler(this.btExpandirTudo_Click);
            // 
            // mnuMenu
            // 
            this.mnuMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuInfoBanco});
            this.mnuMenu.Location = new System.Drawing.Point(0, 0);
            this.mnuMenu.Name = "mnuMenu";
            this.mnuMenu.Size = new System.Drawing.Size(778, 24);
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
            // frmDetalhesRole
            // 
            this.AcceptButton = this.btPesquisar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 495);
            this.Controls.Add(this.mnuMenu);
            this.Controls.Add(this.btExpandirTudo);
            this.Controls.Add(this.btRelatorio);
            this.Controls.Add(this.btPesquisar);
            this.Controls.Add(this.txtNomeRole);
            this.Controls.Add(this.trvRole);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDetalhesRole";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detalhes Role";
            this.mnuMenu.ResumeLayout(false);
            this.mnuMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView trvRole;
        private System.Windows.Forms.TextBox txtNomeRole;
        private System.Windows.Forms.Button btPesquisar;
        private System.Windows.Forms.Button btRelatorio;
        private System.Windows.Forms.Button btExpandirTudo;
        private System.Windows.Forms.MenuStrip mnuMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuInfoBanco;
    }
}