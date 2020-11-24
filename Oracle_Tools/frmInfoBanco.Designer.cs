namespace Oracle_Tools
{
    partial class frmInfoBanco
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInfoBanco));
            this.lvwInfoBanco = new System.Windows.Forms.ListView();
            this.btCopiar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvwInfoBanco
            // 
            this.lvwInfoBanco.AllowDrop = true;
            this.lvwInfoBanco.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwInfoBanco.FullRowSelect = true;
            this.lvwInfoBanco.GridLines = true;
            this.lvwInfoBanco.HideSelection = false;
            this.lvwInfoBanco.Location = new System.Drawing.Point(12, 12);
            this.lvwInfoBanco.Name = "lvwInfoBanco";
            this.lvwInfoBanco.Size = new System.Drawing.Size(418, 355);
            this.lvwInfoBanco.TabIndex = 17;
            this.lvwInfoBanco.UseCompatibleStateImageBehavior = false;
            this.lvwInfoBanco.View = System.Windows.Forms.View.Details;
            // 
            // btCopiar
            // 
            this.btCopiar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCopiar.Location = new System.Drawing.Point(12, 373);
            this.btCopiar.Name = "btCopiar";
            this.btCopiar.Size = new System.Drawing.Size(233, 28);
            this.btCopiar.TabIndex = 18;
            this.btCopiar.Text = "Copiar dados para a área de transferência";
            this.btCopiar.UseVisualStyleBackColor = true;
            this.btCopiar.Click += new System.EventHandler(this.btCopiar_Click);
            // 
            // frmInfoBanco
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 413);
            this.Controls.Add(this.btCopiar);
            this.Controls.Add(this.lvwInfoBanco);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmInfoBanco";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Informações do banco de dados";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListView lvwInfoBanco;
        private System.Windows.Forms.Button btCopiar;
    }
}