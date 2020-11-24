namespace Oracle_Tools
{
    partial class frmDBLINKs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDBLINKs));
            this.lvwDBLINKs = new System.Windows.Forms.ListView();
            this.mnuMenu = new System.Windows.Forms.MenuStrip();
            this.mnuArquivo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuArquivoConectar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuArquivoInfoBanco = new System.Windows.Forms.ToolStripMenuItem();
            this.btAtualizar = new System.Windows.Forms.Button();
            this.mnuContextoLvw = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmrTimerMenuContexto = new System.Windows.Forms.Timer(this.components);
            this.btTestarTodos = new System.Windows.Forms.Button();
            this.mnuMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwDBLINKs
            // 
            this.lvwDBLINKs.AllowDrop = true;
            this.lvwDBLINKs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwDBLINKs.FullRowSelect = true;
            this.lvwDBLINKs.GridLines = true;
            this.lvwDBLINKs.HideSelection = false;
            this.lvwDBLINKs.Location = new System.Drawing.Point(12, 27);
            this.lvwDBLINKs.Name = "lvwDBLINKs";
            this.lvwDBLINKs.Size = new System.Drawing.Size(741, 294);
            this.lvwDBLINKs.TabIndex = 16;
            this.lvwDBLINKs.UseCompatibleStateImageBehavior = false;
            this.lvwDBLINKs.View = System.Windows.Forms.View.Details;
            this.lvwDBLINKs.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwDBLINKs_ColumnClick);
            this.lvwDBLINKs.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwDBLINKs_MouseClick);
            // 
            // mnuMenu
            // 
            this.mnuMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuArquivo});
            this.mnuMenu.Location = new System.Drawing.Point(0, 0);
            this.mnuMenu.Name = "mnuMenu";
            this.mnuMenu.Size = new System.Drawing.Size(765, 24);
            this.mnuMenu.TabIndex = 31;
            this.mnuMenu.Text = "menuStrip1";
            // 
            // mnuArquivo
            // 
            this.mnuArquivo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuArquivoConectar,
            this.toolStripMenuItem1,
            this.mnuArquivoInfoBanco});
            this.mnuArquivo.Name = "mnuArquivo";
            this.mnuArquivo.Size = new System.Drawing.Size(61, 20);
            this.mnuArquivo.Text = "Arquivo";
            // 
            // mnuArquivoConectar
            // 
            this.mnuArquivoConectar.Name = "mnuArquivoConectar";
            this.mnuArquivoConectar.Size = new System.Drawing.Size(131, 22);
            this.mnuArquivoConectar.Text = "Conectar";
            this.mnuArquivoConectar.Click += new System.EventHandler(this.mnuArquivoConectar_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(128, 6);
            // 
            // mnuArquivoInfoBanco
            // 
            this.mnuArquivoInfoBanco.Name = "mnuArquivoInfoBanco";
            this.mnuArquivoInfoBanco.Size = new System.Drawing.Size(131, 22);
            this.mnuArquivoInfoBanco.Text = "Info Banco";
            this.mnuArquivoInfoBanco.Click += new System.EventHandler(this.mnuArquivoInfoBanco_Click);
            // 
            // btAtualizar
            // 
            this.btAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAtualizar.Location = new System.Drawing.Point(645, 327);
            this.btAtualizar.Name = "btAtualizar";
            this.btAtualizar.Size = new System.Drawing.Size(108, 27);
            this.btAtualizar.TabIndex = 32;
            this.btAtualizar.Text = "Atualizar";
            this.btAtualizar.UseVisualStyleBackColor = true;
            this.btAtualizar.Click += new System.EventHandler(this.btAtualizar_Click);
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
            // btTestarTodos
            // 
            this.btTestarTodos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btTestarTodos.Location = new System.Drawing.Point(531, 327);
            this.btTestarTodos.Name = "btTestarTodos";
            this.btTestarTodos.Size = new System.Drawing.Size(108, 27);
            this.btTestarTodos.TabIndex = 34;
            this.btTestarTodos.Text = "Testar Todos";
            this.btTestarTodos.UseVisualStyleBackColor = true;
            this.btTestarTodos.Click += new System.EventHandler(this.btTestarTodos_Click);
            // 
            // frmDBLINKs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 366);
            this.Controls.Add(this.btTestarTodos);
            this.Controls.Add(this.btAtualizar);
            this.Controls.Add(this.mnuMenu);
            this.Controls.Add(this.lvwDBLINKs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(270, 173);
            this.Name = "frmDBLINKs";
            this.Text = "DBLINKs";
            this.mnuMenu.ResumeLayout(false);
            this.mnuMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListView lvwDBLINKs;
        private System.Windows.Forms.MenuStrip mnuMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuArquivo;
        private System.Windows.Forms.ToolStripMenuItem mnuArquivoConectar;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuArquivoInfoBanco;
        private System.Windows.Forms.Button btAtualizar;
        private System.Windows.Forms.ContextMenuStrip mnuContextoLvw;
        private System.Windows.Forms.Timer tmrTimerMenuContexto;
        private System.Windows.Forms.Button btTestarTodos;
    }
}