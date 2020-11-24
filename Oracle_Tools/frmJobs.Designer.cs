namespace Oracle_Tools
{
    partial class frmJobs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmJobs));
            this.lvwJobs = new System.Windows.Forms.ListView();
            this.btAtualizar = new System.Windows.Forms.Button();
            this.chkSelecionarTodos = new System.Windows.Forms.CheckBox();
            this.lblStatusLista = new System.Windows.Forms.Label();
            this.mnuContextoLvw = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmrTimerMenuContexto = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lvwJobs
            // 
            this.lvwJobs.AllowDrop = true;
            this.lvwJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwJobs.CheckBoxes = true;
            this.lvwJobs.FullRowSelect = true;
            this.lvwJobs.GridLines = true;
            this.lvwJobs.HideSelection = false;
            this.lvwJobs.Location = new System.Drawing.Point(12, 12);
            this.lvwJobs.Name = "lvwJobs";
            this.lvwJobs.Size = new System.Drawing.Size(769, 366);
            this.lvwJobs.TabIndex = 16;
            this.lvwJobs.UseCompatibleStateImageBehavior = false;
            this.lvwJobs.View = System.Windows.Forms.View.Details;
            this.lvwJobs.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwJobs_ColumnClick);
            this.lvwJobs.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvwJobs_ItemChecked);
            this.lvwJobs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvwJobs_MouseDown);
            // 
            // btAtualizar
            // 
            this.btAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAtualizar.Location = new System.Drawing.Point(660, 384);
            this.btAtualizar.Name = "btAtualizar";
            this.btAtualizar.Size = new System.Drawing.Size(121, 32);
            this.btAtualizar.TabIndex = 17;
            this.btAtualizar.Text = "Atualizar";
            this.btAtualizar.UseVisualStyleBackColor = true;
            this.btAtualizar.Click += new System.EventHandler(this.btAtualizar_Click);
            // 
            // chkSelecionarTodos
            // 
            this.chkSelecionarTodos.AutoSize = true;
            this.chkSelecionarTodos.Location = new System.Drawing.Point(17, 17);
            this.chkSelecionarTodos.Name = "chkSelecionarTodos";
            this.chkSelecionarTodos.Size = new System.Drawing.Size(15, 14);
            this.chkSelecionarTodos.TabIndex = 29;
            this.chkSelecionarTodos.UseVisualStyleBackColor = true;
            this.chkSelecionarTodos.CheckedChanged += new System.EventHandler(this.chkSelecionarTodos_CheckedChanged);
            // 
            // lblStatusLista
            // 
            this.lblStatusLista.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatusLista.AutoSize = true;
            this.lblStatusLista.Location = new System.Drawing.Point(12, 394);
            this.lblStatusLista.Name = "lblStatusLista";
            this.lblStatusLista.Size = new System.Drawing.Size(181, 13);
            this.lblStatusLista.TabIndex = 30;
            this.lblStatusLista.Text = "0 itens listados - 0 itens selecionados";
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
            // frmJobs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 428);
            this.Controls.Add(this.lblStatusLista);
            this.Controls.Add(this.chkSelecionarTodos);
            this.Controls.Add(this.btAtualizar);
            this.Controls.Add(this.lvwJobs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmJobs";
            this.Text = "Jobs de banco";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListView lvwJobs;
        private System.Windows.Forms.Button btAtualizar;
        private System.Windows.Forms.CheckBox chkSelecionarTodos;
        private System.Windows.Forms.Label lblStatusLista;
        private System.Windows.Forms.ContextMenuStrip mnuContextoLvw;
        private System.Windows.Forms.Timer tmrTimerMenuContexto;
    }
}