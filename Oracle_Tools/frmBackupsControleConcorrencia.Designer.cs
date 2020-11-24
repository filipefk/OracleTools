namespace Oracle_Tools
{
    partial class frmBackupsControleConcorrencia
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBackupsControleConcorrencia));
            this.lvwBackups = new System.Windows.Forms.ListView();
            this.colID_ALTERACAO = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDATA_HORA = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTIPO = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGeradorBackup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnuContextoLvw = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmrMnuContextoLvw = new System.Windows.Forms.Timer(this.components);
            this.stStatusStrip = new System.Windows.Forms.StatusStrip();
            this.pbrStatus = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.stStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwBackups
            // 
            this.lvwBackups.AllowDrop = true;
            this.lvwBackups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwBackups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colID_ALTERACAO,
            this.colDATA_HORA,
            this.colTIPO,
            this.colGeradorBackup});
            this.lvwBackups.FullRowSelect = true;
            this.lvwBackups.GridLines = true;
            this.lvwBackups.HideSelection = false;
            this.lvwBackups.Location = new System.Drawing.Point(12, 12);
            this.lvwBackups.Name = "lvwBackups";
            this.lvwBackups.Size = new System.Drawing.Size(680, 319);
            this.lvwBackups.TabIndex = 30;
            this.lvwBackups.UseCompatibleStateImageBehavior = false;
            this.lvwBackups.View = System.Windows.Forms.View.Details;
            this.lvwBackups.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwBackups_ColumnClick);
            this.lvwBackups.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvwBackups_ItemSelectionChanged);
            this.lvwBackups.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwBackups_MouseClick);
            // 
            // colID_ALTERACAO
            // 
            this.colID_ALTERACAO.Text = "ID do Backup";
            this.colID_ALTERACAO.Width = 103;
            // 
            // colDATA_HORA
            // 
            this.colDATA_HORA.Text = "Data Hora";
            this.colDATA_HORA.Width = 151;
            // 
            // colTIPO
            // 
            this.colTIPO.Text = "Tipo";
            this.colTIPO.Width = 174;
            // 
            // colGeradorBackup
            // 
            this.colGeradorBackup.Text = "Gerador do Backup";
            this.colGeradorBackup.Width = 233;
            // 
            // mnuContextoLvw
            // 
            this.mnuContextoLvw.Name = "mnuMenu";
            this.mnuContextoLvw.ShowImageMargin = false;
            this.mnuContextoLvw.Size = new System.Drawing.Size(36, 4);
            this.mnuContextoLvw.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuContextoLvw_ItemClicked);
            // 
            // tmrMnuContextoLvw
            // 
            this.tmrMnuContextoLvw.Tick += new System.EventHandler(this.tmrMnuContextoLvw_Tick);
            // 
            // stStatusStrip
            // 
            this.stStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbrStatus,
            this.lblStatus});
            this.stStatusStrip.Location = new System.Drawing.Point(0, 334);
            this.stStatusStrip.Name = "stStatusStrip";
            this.stStatusStrip.Size = new System.Drawing.Size(704, 22);
            this.stStatusStrip.TabIndex = 32;
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
            // frmBackupsControleConcorrencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 356);
            this.Controls.Add(this.stStatusStrip);
            this.Controls.Add(this.lvwBackups);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBackupsControleConcorrencia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Backups do objeto OWNER.NOME";
            this.stStatusStrip.ResumeLayout(false);
            this.stStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListView lvwBackups;
        private System.Windows.Forms.ColumnHeader colID_ALTERACAO;
        private System.Windows.Forms.ColumnHeader colDATA_HORA;
        private System.Windows.Forms.ColumnHeader colTIPO;
        private System.Windows.Forms.ColumnHeader colGeradorBackup;
        private System.Windows.Forms.ContextMenuStrip mnuContextoLvw;
        private System.Windows.Forms.Timer tmrMnuContextoLvw;
        private System.Windows.Forms.StatusStrip stStatusStrip;
        private System.Windows.Forms.ToolStripProgressBar pbrStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}