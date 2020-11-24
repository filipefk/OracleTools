namespace Oracle_Tools
{
    partial class frmToolList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmToolList));
            this.btJobs = new System.Windows.Forms.Button();
            this.btSessoesAtivas = new System.Windows.Forms.Button();
            this.btAgendadorDeScripts = new System.Windows.Forms.Button();
            this.btDetalhesRole = new System.Windows.Forms.Button();
            this.btControleConcorrencia = new System.Windows.Forms.Button();
            this.btProcuraLock = new System.Windows.Forms.Button();
            this.btQueryAlert = new System.Windows.Forms.Button();
            this.btDetalhesUser = new System.Windows.Forms.Button();
            this.btExtractScriptUser = new System.Windows.Forms.Button();
            this.btExtractDDLObjetos = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btJobs
            // 
            this.btJobs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btJobs.Image = global::Oracle_Tools.Properties.Resources.Job_de_banco;
            this.btJobs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btJobs.Location = new System.Drawing.Point(386, 214);
            this.btJobs.Name = "btJobs";
            this.btJobs.Size = new System.Drawing.Size(366, 62);
            this.btJobs.TabIndex = 14;
            this.btJobs.Text = "Jobs de banco                                                 ";
            this.btJobs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btJobs.UseVisualStyleBackColor = true;
            this.btJobs.Click += new System.EventHandler(this.btJobs_Click);
            // 
            // btSessoesAtivas
            // 
            this.btSessoesAtivas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSessoesAtivas.Image = global::Oracle_Tools.Properties.Resources.Sessoes;
            this.btSessoesAtivas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btSessoesAtivas.Location = new System.Drawing.Point(386, 148);
            this.btSessoesAtivas.Name = "btSessoesAtivas";
            this.btSessoesAtivas.Size = new System.Drawing.Size(366, 62);
            this.btSessoesAtivas.TabIndex = 13;
            this.btSessoesAtivas.Text = "Sessões ativas                                                 ";
            this.btSessoesAtivas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btSessoesAtivas.UseVisualStyleBackColor = true;
            this.btSessoesAtivas.Click += new System.EventHandler(this.btSessoesAtivas_Click);
            // 
            // btAgendadorDeScripts
            // 
            this.btAgendadorDeScripts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btAgendadorDeScripts.Image = global::Oracle_Tools.Properties.Resources.CalendarioMedio;
            this.btAgendadorDeScripts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btAgendadorDeScripts.Location = new System.Drawing.Point(390, 282);
            this.btAgendadorDeScripts.Name = "btAgendadorDeScripts";
            this.btAgendadorDeScripts.Size = new System.Drawing.Size(366, 62);
            this.btAgendadorDeScripts.TabIndex = 12;
            this.btAgendadorDeScripts.Text = "Agendar execução de um script                       ";
            this.btAgendadorDeScripts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btAgendadorDeScripts.UseVisualStyleBackColor = true;
            this.btAgendadorDeScripts.Visible = false;
            this.btAgendadorDeScripts.Click += new System.EventHandler(this.btAgendadorDeScripts_Click);
            // 
            // btDetalhesRole
            // 
            this.btDetalhesRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btDetalhesRole.Image = ((System.Drawing.Image)(resources.GetObject("btDetalhesRole.Image")));
            this.btDetalhesRole.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btDetalhesRole.Location = new System.Drawing.Point(14, 214);
            this.btDetalhesRole.Name = "btDetalhesRole";
            this.btDetalhesRole.Size = new System.Drawing.Size(366, 62);
            this.btDetalhesRole.TabIndex = 11;
            this.btDetalhesRole.Text = "Visualizar detalhes de uma role                     ";
            this.btDetalhesRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btDetalhesRole.UseVisualStyleBackColor = true;
            this.btDetalhesRole.Click += new System.EventHandler(this.btDetalhesRole_Click);
            // 
            // btControleConcorrencia
            // 
            this.btControleConcorrencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btControleConcorrencia.Image = global::Oracle_Tools.Properties.Resources.Users;
            this.btControleConcorrencia.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btControleConcorrencia.Location = new System.Drawing.Point(386, 80);
            this.btControleConcorrencia.Name = "btControleConcorrencia";
            this.btControleConcorrencia.Size = new System.Drawing.Size(366, 62);
            this.btControleConcorrencia.TabIndex = 10;
            this.btControleConcorrencia.Text = "Controle de Concorrência de Objetos              ";
            this.btControleConcorrencia.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btControleConcorrencia.UseVisualStyleBackColor = true;
            this.btControleConcorrencia.Click += new System.EventHandler(this.btControleConcorrencia_Click);
            // 
            // btProcuraLock
            // 
            this.btProcuraLock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btProcuraLock.Image = global::Oracle_Tools.Properties.Resources.Locked;
            this.btProcuraLock.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btProcuraLock.Location = new System.Drawing.Point(386, 12);
            this.btProcuraLock.Name = "btProcuraLock";
            this.btProcuraLock.Size = new System.Drawing.Size(366, 62);
            this.btProcuraLock.TabIndex = 9;
            this.btProcuraLock.Text = "Procurar Lock de banco                                   ";
            this.btProcuraLock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btProcuraLock.UseVisualStyleBackColor = true;
            this.btProcuraLock.Click += new System.EventHandler(this.btProcuraLock_Click);
            // 
            // btQueryAlert
            // 
            this.btQueryAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btQueryAlert.Image = global::Oracle_Tools.Properties.Resources.Alerta;
            this.btQueryAlert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btQueryAlert.Location = new System.Drawing.Point(15, 282);
            this.btQueryAlert.Name = "btQueryAlert";
            this.btQueryAlert.Size = new System.Drawing.Size(366, 62);
            this.btQueryAlert.TabIndex = 8;
            this.btQueryAlert.Text = "Receber um alerta baseado em uma consulta";
            this.btQueryAlert.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btQueryAlert.UseVisualStyleBackColor = true;
            this.btQueryAlert.Click += new System.EventHandler(this.btQueryAlert_Click);
            // 
            // btDetalhesUser
            // 
            this.btDetalhesUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btDetalhesUser.Image = global::Oracle_Tools.Properties.Resources.Lupa_User;
            this.btDetalhesUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btDetalhesUser.Location = new System.Drawing.Point(15, 148);
            this.btDetalhesUser.Name = "btDetalhesUser";
            this.btDetalhesUser.Size = new System.Drawing.Size(366, 62);
            this.btDetalhesUser.TabIndex = 7;
            this.btDetalhesUser.Text = "Visualizar detalhes de um usuário                  ";
            this.btDetalhesUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btDetalhesUser.UseVisualStyleBackColor = true;
            this.btDetalhesUser.Click += new System.EventHandler(this.btDetalhesUser_Click);
            // 
            // btExtractScriptUser
            // 
            this.btExtractScriptUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btExtractScriptUser.Image = global::Oracle_Tools.Properties.Resources.Extract_User;
            this.btExtractScriptUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btExtractScriptUser.Location = new System.Drawing.Point(12, 80);
            this.btExtractScriptUser.Name = "btExtractScriptUser";
            this.btExtractScriptUser.Size = new System.Drawing.Size(369, 62);
            this.btExtractScriptUser.TabIndex = 6;
            this.btExtractScriptUser.Text = "Extrair script de criação de usuários               ";
            this.btExtractScriptUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btExtractScriptUser.UseVisualStyleBackColor = true;
            this.btExtractScriptUser.Click += new System.EventHandler(this.btExtractScriptUser_Click);
            // 
            // btExtractDDLObjetos
            // 
            this.btExtractDDLObjetos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btExtractDDLObjetos.Image = global::Oracle_Tools.Properties.Resources.Extract;
            this.btExtractDDLObjetos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btExtractDDLObjetos.Location = new System.Drawing.Point(12, 12);
            this.btExtractDDLObjetos.Name = "btExtractDDLObjetos";
            this.btExtractDDLObjetos.Size = new System.Drawing.Size(369, 62);
            this.btExtractDDLObjetos.TabIndex = 5;
            this.btExtractDDLObjetos.Text = "Extrair DDL de objetos de banco ORACLE       ";
            this.btExtractDDLObjetos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btExtractDDLObjetos.UseVisualStyleBackColor = true;
            this.btExtractDDLObjetos.Click += new System.EventHandler(this.btExtractDDLObjetos_Click);
            // 
            // frmToolList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 358);
            this.Controls.Add(this.btJobs);
            this.Controls.Add(this.btSessoesAtivas);
            this.Controls.Add(this.btAgendadorDeScripts);
            this.Controls.Add(this.btDetalhesRole);
            this.Controls.Add(this.btControleConcorrencia);
            this.Controls.Add(this.btProcuraLock);
            this.Controls.Add(this.btQueryAlert);
            this.Controls.Add(this.btDetalhesUser);
            this.Controls.Add(this.btExtractScriptUser);
            this.Controls.Add(this.btExtractDDLObjetos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmToolList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ORACLE Tools";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btExtractDDLObjetos;
        private System.Windows.Forms.Button btExtractScriptUser;
        private System.Windows.Forms.Button btDetalhesUser;
        private System.Windows.Forms.Button btQueryAlert;
        private System.Windows.Forms.Button btProcuraLock;
        private System.Windows.Forms.Button btControleConcorrencia;
        private System.Windows.Forms.Button btDetalhesRole;
        private System.Windows.Forms.Button btAgendadorDeScripts;
        private System.Windows.Forms.Button btSessoesAtivas;
        private System.Windows.Forms.Button btJobs;
    }
}