namespace Oracle_Tools
{
    partial class frmProcuraLock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProcuraLock));
            this.lvwLocks = new System.Windows.Forms.ListView();
            this.tmrProcura = new System.Windows.Forms.Timer(this.components);
            this.btScript1 = new System.Windows.Forms.Button();
            this.btScript2 = new System.Windows.Forms.Button();
            this.btParar = new System.Windows.Forms.Button();
            this.lblMensagem = new System.Windows.Forms.Label();
            this.chkAvisar = new System.Windows.Forms.CheckBox();
            this.mnuMenu = new System.Windows.Forms.MenuStrip();
            this.mnuInfoBanco = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwLocks
            // 
            this.lvwLocks.AllowDrop = true;
            this.lvwLocks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwLocks.FullRowSelect = true;
            this.lvwLocks.GridLines = true;
            this.lvwLocks.HideSelection = false;
            this.lvwLocks.Location = new System.Drawing.Point(12, 27);
            this.lvwLocks.Name = "lvwLocks";
            this.lvwLocks.Size = new System.Drawing.Size(817, 223);
            this.lvwLocks.TabIndex = 16;
            this.lvwLocks.UseCompatibleStateImageBehavior = false;
            this.lvwLocks.View = System.Windows.Forms.View.Details;
            this.lvwLocks.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvwLocks_MouseDown);
            // 
            // tmrProcura
            // 
            this.tmrProcura.Interval = 5000;
            this.tmrProcura.Tick += new System.EventHandler(this.tmrProcura_Tick);
            // 
            // btScript1
            // 
            this.btScript1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btScript1.Location = new System.Drawing.Point(12, 279);
            this.btScript1.Name = "btScript1";
            this.btScript1.Size = new System.Drawing.Size(205, 34);
            this.btScript1.TabIndex = 17;
            this.btScript1.Text = "Script 1";
            this.btScript1.UseVisualStyleBackColor = true;
            this.btScript1.Click += new System.EventHandler(this.btScript1_Click);
            this.btScript1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btScript1_MouseDown);
            // 
            // btScript2
            // 
            this.btScript2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btScript2.Location = new System.Drawing.Point(223, 279);
            this.btScript2.Name = "btScript2";
            this.btScript2.Size = new System.Drawing.Size(205, 34);
            this.btScript2.TabIndex = 18;
            this.btScript2.Text = "Script 2";
            this.btScript2.UseVisualStyleBackColor = true;
            this.btScript2.Click += new System.EventHandler(this.btScript2_Click);
            this.btScript2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btScript2_MouseDown);
            // 
            // btParar
            // 
            this.btParar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btParar.Location = new System.Drawing.Point(624, 279);
            this.btParar.Name = "btParar";
            this.btParar.Size = new System.Drawing.Size(205, 34);
            this.btParar.TabIndex = 19;
            this.btParar.Text = "Parar";
            this.btParar.UseVisualStyleBackColor = true;
            this.btParar.Click += new System.EventHandler(this.btParar_Click);
            // 
            // lblMensagem
            // 
            this.lblMensagem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMensagem.AutoSize = true;
            this.lblMensagem.Location = new System.Drawing.Point(12, 257);
            this.lblMensagem.Name = "lblMensagem";
            this.lblMensagem.Size = new System.Drawing.Size(41, 13);
            this.lblMensagem.TabIndex = 20;
            this.lblMensagem.Text = "Parado";
            // 
            // chkAvisar
            // 
            this.chkAvisar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAvisar.AutoSize = true;
            this.chkAvisar.Checked = true;
            this.chkAvisar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAvisar.Location = new System.Drawing.Point(624, 256);
            this.chkAvisar.Name = "chkAvisar";
            this.chkAvisar.Size = new System.Drawing.Size(153, 17);
            this.chkAvisar.TabIndex = 21;
            this.chkAvisar.Text = "Avisar caso encontre Lock";
            this.chkAvisar.UseVisualStyleBackColor = true;
            // 
            // mnuMenu
            // 
            this.mnuMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuInfoBanco});
            this.mnuMenu.Location = new System.Drawing.Point(0, 0);
            this.mnuMenu.Name = "mnuMenu";
            this.mnuMenu.Size = new System.Drawing.Size(841, 24);
            this.mnuMenu.TabIndex = 49;
            this.mnuMenu.Text = "menuStrip1";
            // 
            // mnuInfoBanco
            // 
            this.mnuInfoBanco.Name = "mnuInfoBanco";
            this.mnuInfoBanco.Size = new System.Drawing.Size(76, 20);
            this.mnuInfoBanco.Text = "Info Banco";
            this.mnuInfoBanco.Click += new System.EventHandler(this.mnuInfoBanco_Click);
            // 
            // frmProcuraLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 319);
            this.Controls.Add(this.mnuMenu);
            this.Controls.Add(this.chkAvisar);
            this.Controls.Add(this.lblMensagem);
            this.Controls.Add(this.btParar);
            this.Controls.Add(this.btScript2);
            this.Controls.Add(this.btScript1);
            this.Controls.Add(this.lvwLocks);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(666, 38);
            this.Name = "frmProcuraLock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Procura Lock no banco";
            this.mnuMenu.ResumeLayout(false);
            this.mnuMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListView lvwLocks;
        private System.Windows.Forms.Timer tmrProcura;
        private System.Windows.Forms.Button btScript1;
        private System.Windows.Forms.Button btScript2;
        private System.Windows.Forms.Button btParar;
        private System.Windows.Forms.Label lblMensagem;
        private System.Windows.Forms.CheckBox chkAvisar;
        private System.Windows.Forms.MenuStrip mnuMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuInfoBanco;
    }
}