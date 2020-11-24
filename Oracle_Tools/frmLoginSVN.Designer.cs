namespace Oracle_Tools
{
    partial class frmLoginSVN
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoginSVN));
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancelar = new System.Windows.Forms.Button();
            this.chkSalvarConexao = new System.Windows.Forms.CheckBox();
            this.cboConexoesSalvas = new System.Windows.Forms.Button();
            this.mnuMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtURL_SVN = new System.Windows.Forms.TextBox();
            this.chkSalvarSenha = new System.Windows.Forms.CheckBox();
            this.tmrMenu = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(75, 33);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(457, 20);
            this.txtUsername.TabIndex = 0;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(23, 36);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(46, 13);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Usuario:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(28, 62);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(41, 13);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Senha:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(75, 59);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(485, 20);
            this.txtPassword.TabIndex = 2;
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(316, 105);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(119, 36);
            this.btOK.TabIndex = 6;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancelar
            // 
            this.btCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancelar.Location = new System.Drawing.Point(441, 105);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(119, 36);
            this.btCancelar.TabIndex = 7;
            this.btCancelar.Text = "Cancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // chkSalvarConexao
            // 
            this.chkSalvarConexao.AutoSize = true;
            this.chkSalvarConexao.Location = new System.Drawing.Point(12, 85);
            this.chkSalvarConexao.Name = "chkSalvarConexao";
            this.chkSalvarConexao.Size = new System.Drawing.Size(100, 17);
            this.chkSalvarConexao.TabIndex = 9;
            this.chkSalvarConexao.Text = "Salvar conexão";
            this.chkSalvarConexao.UseVisualStyleBackColor = true;
            this.chkSalvarConexao.CheckedChanged += new System.EventHandler(this.chkSalvarConexao_CheckedChanged);
            // 
            // cboConexoesSalvas
            // 
            this.cboConexoesSalvas.Location = new System.Drawing.Point(530, 32);
            this.cboConexoesSalvas.Name = "cboConexoesSalvas";
            this.cboConexoesSalvas.Size = new System.Drawing.Size(30, 21);
            this.cboConexoesSalvas.TabIndex = 10;
            this.cboConexoesSalvas.Text = ">";
            this.cboConexoesSalvas.UseVisualStyleBackColor = true;
            this.cboConexoesSalvas.Click += new System.EventHandler(this.cboConexoesSalvas_Click);
            this.cboConexoesSalvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cboConexoesSalvas_MouseDown);
            // 
            // mnuMenu
            // 
            this.mnuMenu.Name = "mnuMenu";
            this.mnuMenu.ShowImageMargin = false;
            this.mnuMenu.Size = new System.Drawing.Size(36, 4);
            this.mnuMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuMenu_ItemClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "URL SVN:";
            // 
            // txtURL_SVN
            // 
            this.txtURL_SVN.Location = new System.Drawing.Point(75, 6);
            this.txtURL_SVN.Name = "txtURL_SVN";
            this.txtURL_SVN.Size = new System.Drawing.Size(485, 20);
            this.txtURL_SVN.TabIndex = 12;
            // 
            // chkSalvarSenha
            // 
            this.chkSalvarSenha.AutoSize = true;
            this.chkSalvarSenha.Enabled = false;
            this.chkSalvarSenha.Location = new System.Drawing.Point(132, 85);
            this.chkSalvarSenha.Name = "chkSalvarSenha";
            this.chkSalvarSenha.Size = new System.Drawing.Size(88, 17);
            this.chkSalvarSenha.TabIndex = 13;
            this.chkSalvarSenha.Text = "Salvar senha";
            this.chkSalvarSenha.UseVisualStyleBackColor = true;
            // 
            // tmrMenu
            // 
            this.tmrMenu.Interval = 1;
            this.tmrMenu.Tick += new System.EventHandler(this.tmrMenu_Tick);
            // 
            // frmLoginSVN
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancelar;
            this.ClientSize = new System.Drawing.Size(572, 153);
            this.Controls.Add(this.chkSalvarSenha);
            this.Controls.Add(this.txtURL_SVN);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboConexoesSalvas);
            this.Controls.Add(this.chkSalvarConexao);
            this.Controls.Add(this.btCancelar);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLoginSVN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SVN Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.CheckBox chkSalvarConexao;
        private System.Windows.Forms.Button cboConexoesSalvas;
        private System.Windows.Forms.ContextMenuStrip mnuMenu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtURL_SVN;
        private System.Windows.Forms.CheckBox chkSalvarSenha;
        private System.Windows.Forms.Timer tmrMenu;
    }
}

