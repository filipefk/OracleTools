namespace Oracle_Tools
{
    partial class frmSessoes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSessoes));
            this.lvwSessoes = new System.Windows.Forms.ListView();
            this.mnuMenu = new System.Windows.Forms.MenuStrip();
            this.mnuArquivo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuArquivoConectar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuArquivoInfoBanco = new System.Windows.Forms.ToolStripMenuItem();
            this.btAtualizar = new System.Windows.Forms.Button();
            this.chkSelecionarTodos = new System.Windows.Forms.CheckBox();
            this.mnuContextoLvw = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmrTimerMenuContexto = new System.Windows.Forms.Timer(this.components);
            this.btPreencherNomeUsuarios = new System.Windows.Forms.Button();
            this.mnuMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwSessoes
            // 
            this.lvwSessoes.AllowDrop = true;
            this.lvwSessoes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwSessoes.CheckBoxes = true;
            this.lvwSessoes.FullRowSelect = true;
            this.lvwSessoes.GridLines = true;
            this.lvwSessoes.HideSelection = false;
            this.lvwSessoes.Location = new System.Drawing.Point(12, 27);
            this.lvwSessoes.Name = "lvwSessoes";
            this.lvwSessoes.Size = new System.Drawing.Size(741, 294);
            this.lvwSessoes.TabIndex = 16;
            this.lvwSessoes.UseCompatibleStateImageBehavior = false;
            this.lvwSessoes.View = System.Windows.Forms.View.Details;
            this.lvwSessoes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwSessoes_ColumnClick);
            this.lvwSessoes.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvwSessoes_ItemChecked);
            this.lvwSessoes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvwSessoes_KeyDown);
            this.lvwSessoes.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwSessoes_MouseClick);
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
            // chkSelecionarTodos
            // 
            this.chkSelecionarTodos.AutoSize = true;
            this.chkSelecionarTodos.Location = new System.Drawing.Point(18, 33);
            this.chkSelecionarTodos.Name = "chkSelecionarTodos";
            this.chkSelecionarTodos.Size = new System.Drawing.Size(15, 14);
            this.chkSelecionarTodos.TabIndex = 33;
            this.chkSelecionarTodos.UseVisualStyleBackColor = true;
            this.chkSelecionarTodos.CheckedChanged += new System.EventHandler(this.chkSelecionarTodos_CheckedChanged);
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
            // btPreencherNomeUsuarios
            // 
            this.btPreencherNomeUsuarios.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btPreencherNomeUsuarios.Location = new System.Drawing.Point(531, 327);
            this.btPreencherNomeUsuarios.Name = "btPreencherNomeUsuarios";
            this.btPreencherNomeUsuarios.Size = new System.Drawing.Size(108, 27);
            this.btPreencherNomeUsuarios.TabIndex = 34;
            this.btPreencherNomeUsuarios.Text = "Buscar Nomes";
            this.btPreencherNomeUsuarios.UseVisualStyleBackColor = true;
            this.btPreencherNomeUsuarios.Click += new System.EventHandler(this.btPreencherNomeUsuarios_Click);
            // 
            // frmSessoes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 366);
            this.Controls.Add(this.btPreencherNomeUsuarios);
            this.Controls.Add(this.chkSelecionarTodos);
            this.Controls.Add(this.btAtualizar);
            this.Controls.Add(this.mnuMenu);
            this.Controls.Add(this.lvwSessoes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(270, 173);
            this.Name = "frmSessoes";
            this.Text = "Sessões";
            this.mnuMenu.ResumeLayout(false);
            this.mnuMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListView lvwSessoes;
        private System.Windows.Forms.MenuStrip mnuMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuArquivo;
        private System.Windows.Forms.ToolStripMenuItem mnuArquivoConectar;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuArquivoInfoBanco;
        private System.Windows.Forms.Button btAtualizar;
        private System.Windows.Forms.CheckBox chkSelecionarTodos;
        private System.Windows.Forms.ContextMenuStrip mnuContextoLvw;
        private System.Windows.Forms.Timer tmrTimerMenuContexto;
        private System.Windows.Forms.Button btPreencherNomeUsuarios;
    }
}