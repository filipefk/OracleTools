namespace Oracle_Tools
{
    partial class frmLoginAD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoginAD));
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancelar = new System.Windows.Forms.Button();
            this.chkSalvarConexao = new System.Windows.Forms.CheckBox();
            this.btConexoesSalvas = new System.Windows.Forms.Button();
            this.mnuMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lblDominio = new System.Windows.Forms.Label();
            this.txtDominio = new System.Windows.Forms.TextBox();
            this.txtPorta = new System.Windows.Forms.TextBox();
            this.lblPorta = new System.Windows.Forms.Label();
            this.chkSalvarSenha = new System.Windows.Forms.CheckBox();
            this.tmrMenu = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(66, 6);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(171, 20);
            this.txtUsuario.TabIndex = 0;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(14, 9);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(46, 13);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Usuário:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(19, 35);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(41, 13);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Senha:";
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(66, 32);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.PasswordChar = '*';
            this.txtSenha.Size = new System.Drawing.Size(201, 20);
            this.txtSenha.TabIndex = 2;
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(12, 133);
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
            this.btCancelar.Location = new System.Drawing.Point(148, 133);
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
            this.chkSalvarConexao.Location = new System.Drawing.Point(12, 110);
            this.chkSalvarConexao.Name = "chkSalvarConexao";
            this.chkSalvarConexao.Size = new System.Drawing.Size(100, 17);
            this.chkSalvarConexao.TabIndex = 9;
            this.chkSalvarConexao.Text = "Salvar conexão";
            this.chkSalvarConexao.UseVisualStyleBackColor = true;
            this.chkSalvarConexao.CheckedChanged += new System.EventHandler(this.chkSalvarConexao_CheckedChanged);
            // 
            // btConexoesSalvas
            // 
            this.btConexoesSalvas.Location = new System.Drawing.Point(237, 5);
            this.btConexoesSalvas.Name = "btConexoesSalvas";
            this.btConexoesSalvas.Size = new System.Drawing.Size(30, 21);
            this.btConexoesSalvas.TabIndex = 10;
            this.btConexoesSalvas.Text = ">";
            this.btConexoesSalvas.UseVisualStyleBackColor = true;
            this.btConexoesSalvas.Click += new System.EventHandler(this.btConexoesSalvas_Click);
            this.btConexoesSalvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btConexoesSalvas_MouseDown);
            // 
            // mnuMenu
            // 
            this.mnuMenu.Name = "mnuMenu";
            this.mnuMenu.ShowImageMargin = false;
            this.mnuMenu.Size = new System.Drawing.Size(36, 4);
            this.mnuMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuMenu_ItemClicked);
            // 
            // lblDominio
            // 
            this.lblDominio.AutoSize = true;
            this.lblDominio.Location = new System.Drawing.Point(10, 61);
            this.lblDominio.Name = "lblDominio";
            this.lblDominio.Size = new System.Drawing.Size(50, 13);
            this.lblDominio.TabIndex = 11;
            this.lblDominio.Text = "Domínio:";
            // 
            // txtDominio
            // 
            this.txtDominio.Location = new System.Drawing.Point(66, 58);
            this.txtDominio.Name = "txtDominio";
            this.txtDominio.Size = new System.Drawing.Size(201, 20);
            this.txtDominio.TabIndex = 12;
            // 
            // txtPorta
            // 
            this.txtPorta.Location = new System.Drawing.Point(66, 84);
            this.txtPorta.Name = "txtPorta";
            this.txtPorta.Size = new System.Drawing.Size(201, 20);
            this.txtPorta.TabIndex = 14;
            // 
            // lblPorta
            // 
            this.lblPorta.AutoSize = true;
            this.lblPorta.Location = new System.Drawing.Point(25, 87);
            this.lblPorta.Name = "lblPorta";
            this.lblPorta.Size = new System.Drawing.Size(35, 13);
            this.lblPorta.TabIndex = 13;
            this.lblPorta.Text = "Porta:";
            // 
            // chkSalvarSenha
            // 
            this.chkSalvarSenha.AutoSize = true;
            this.chkSalvarSenha.Enabled = false;
            this.chkSalvarSenha.Location = new System.Drawing.Point(179, 110);
            this.chkSalvarSenha.Name = "chkSalvarSenha";
            this.chkSalvarSenha.Size = new System.Drawing.Size(88, 17);
            this.chkSalvarSenha.TabIndex = 15;
            this.chkSalvarSenha.Text = "Salvar senha";
            this.chkSalvarSenha.UseVisualStyleBackColor = true;
            // 
            // tmrMenu
            // 
            this.tmrMenu.Interval = 1;
            this.tmrMenu.Tick += new System.EventHandler(this.tmrMenu_Tick);
            // 
            // frmLoginAD
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancelar;
            this.ClientSize = new System.Drawing.Size(278, 178);
            this.Controls.Add(this.chkSalvarSenha);
            this.Controls.Add(this.txtPorta);
            this.Controls.Add(this.lblPorta);
            this.Controls.Add(this.txtDominio);
            this.Controls.Add(this.lblDominio);
            this.Controls.Add(this.btConexoesSalvas);
            this.Controls.Add(this.chkSalvarConexao);
            this.Controls.Add(this.btCancelar);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsuario);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLoginAD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Active Directory Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.CheckBox chkSalvarConexao;
        private System.Windows.Forms.Button btConexoesSalvas;
        private System.Windows.Forms.ContextMenuStrip mnuMenu;
        private System.Windows.Forms.Label lblDominio;
        private System.Windows.Forms.TextBox txtDominio;
        private System.Windows.Forms.TextBox txtPorta;
        private System.Windows.Forms.Label lblPorta;
        private System.Windows.Forms.CheckBox chkSalvarSenha;
        private System.Windows.Forms.Timer tmrMenu;
    }
}

