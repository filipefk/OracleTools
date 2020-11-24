namespace Oracle_Tools
{
    partial class frmScriptAgendado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScriptAgendado));
            this.dtDataHora = new System.Windows.Forms.DateTimePicker();
            this.txtQualScript = new System.Windows.Forms.TextBox();
            this.btSelecionarArquivo = new System.Windows.Forms.Button();
            this.optQuery = new System.Windows.Forms.RadioButton();
            this.optNoQuery = new System.Windows.Forms.RadioButton();
            this.lblQuando = new System.Windows.Forms.Label();
            this.lblQualScript = new System.Windows.Forms.Label();
            this.lblBanco = new System.Windows.Forms.Label();
            this.btSelecionarBanco = new System.Windows.Forms.Button();
            this.txtQualBanco = new System.Windows.Forms.TextBox();
            this.btAgendar = new System.Windows.Forms.Button();
            this.lvwAgendamentos = new System.Windows.Forms.ListView();
            this.colDataHora = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBanco = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colScript = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTipo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btIniciarParar = new System.Windows.Forms.Button();
            this.stStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrProcura = new System.Windows.Forms.Timer(this.components);
            this.stStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtDataHora
            // 
            this.dtDataHora.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtDataHora.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDataHora.Location = new System.Drawing.Point(12, 25);
            this.dtDataHora.Name = "dtDataHora";
            this.dtDataHora.Size = new System.Drawing.Size(128, 20);
            this.dtDataHora.TabIndex = 0;
            // 
            // txtQualScript
            // 
            this.txtQualScript.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQualScript.Location = new System.Drawing.Point(330, 25);
            this.txtQualScript.Name = "txtQualScript";
            this.txtQualScript.Size = new System.Drawing.Size(379, 20);
            this.txtQualScript.TabIndex = 1;
            // 
            // btSelecionarArquivo
            // 
            this.btSelecionarArquivo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSelecionarArquivo.Location = new System.Drawing.Point(709, 24);
            this.btSelecionarArquivo.Name = "btSelecionarArquivo";
            this.btSelecionarArquivo.Size = new System.Drawing.Size(27, 20);
            this.btSelecionarArquivo.TabIndex = 12;
            this.btSelecionarArquivo.Text = "...";
            this.btSelecionarArquivo.UseVisualStyleBackColor = true;
            this.btSelecionarArquivo.Click += new System.EventHandler(this.btSelecionarArquivo_Click);
            // 
            // optQuery
            // 
            this.optQuery.AutoSize = true;
            this.optQuery.Location = new System.Drawing.Point(12, 51);
            this.optQuery.Name = "optQuery";
            this.optQuery.Size = new System.Drawing.Size(53, 17);
            this.optQuery.TabIndex = 13;
            this.optQuery.Text = "Query";
            this.optQuery.UseVisualStyleBackColor = true;
            // 
            // optNoQuery
            // 
            this.optNoQuery.AutoSize = true;
            this.optNoQuery.Checked = true;
            this.optNoQuery.Location = new System.Drawing.Point(70, 51);
            this.optNoQuery.Name = "optNoQuery";
            this.optNoQuery.Size = new System.Drawing.Size(70, 17);
            this.optNoQuery.TabIndex = 14;
            this.optNoQuery.TabStop = true;
            this.optNoQuery.Text = "No Query";
            this.optNoQuery.UseVisualStyleBackColor = true;
            // 
            // lblQuando
            // 
            this.lblQuando.AutoSize = true;
            this.lblQuando.Location = new System.Drawing.Point(12, 9);
            this.lblQuando.Name = "lblQuando";
            this.lblQuando.Size = new System.Drawing.Size(65, 13);
            this.lblQuando.TabIndex = 15;
            this.lblQuando.Text = "Data e Hora";
            // 
            // lblQualScript
            // 
            this.lblQualScript.AutoSize = true;
            this.lblQualScript.Location = new System.Drawing.Point(327, 9);
            this.lblQualScript.Name = "lblQualScript";
            this.lblQualScript.Size = new System.Drawing.Size(34, 13);
            this.lblQualScript.TabIndex = 16;
            this.lblQualScript.Text = "Script";
            // 
            // lblBanco
            // 
            this.lblBanco.AutoSize = true;
            this.lblBanco.Location = new System.Drawing.Point(146, 9);
            this.lblBanco.Name = "lblBanco";
            this.lblBanco.Size = new System.Drawing.Size(38, 13);
            this.lblBanco.TabIndex = 19;
            this.lblBanco.Text = "Banco";
            // 
            // btSelecionarBanco
            // 
            this.btSelecionarBanco.Location = new System.Drawing.Point(289, 25);
            this.btSelecionarBanco.Name = "btSelecionarBanco";
            this.btSelecionarBanco.Size = new System.Drawing.Size(27, 20);
            this.btSelecionarBanco.TabIndex = 18;
            this.btSelecionarBanco.Text = "...";
            this.btSelecionarBanco.UseVisualStyleBackColor = true;
            this.btSelecionarBanco.Click += new System.EventHandler(this.btSelecionarBanco_Click);
            // 
            // txtQualBanco
            // 
            this.txtQualBanco.BackColor = System.Drawing.Color.White;
            this.txtQualBanco.Location = new System.Drawing.Point(146, 25);
            this.txtQualBanco.Name = "txtQualBanco";
            this.txtQualBanco.ReadOnly = true;
            this.txtQualBanco.Size = new System.Drawing.Size(145, 20);
            this.txtQualBanco.TabIndex = 17;
            this.txtQualBanco.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQualBanco_KeyDown);
            // 
            // btAgendar
            // 
            this.btAgendar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAgendar.Location = new System.Drawing.Point(748, 23);
            this.btAgendar.Name = "btAgendar";
            this.btAgendar.Size = new System.Drawing.Size(72, 21);
            this.btAgendar.TabIndex = 20;
            this.btAgendar.Text = "Agendar";
            this.btAgendar.UseVisualStyleBackColor = true;
            this.btAgendar.Click += new System.EventHandler(this.btAgendar_Click);
            // 
            // lvwAgendamentos
            // 
            this.lvwAgendamentos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwAgendamentos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDataHora,
            this.colBanco,
            this.colScript,
            this.colTipo});
            this.lvwAgendamentos.FullRowSelect = true;
            this.lvwAgendamentos.GridLines = true;
            this.lvwAgendamentos.HideSelection = false;
            this.lvwAgendamentos.Location = new System.Drawing.Point(15, 74);
            this.lvwAgendamentos.Name = "lvwAgendamentos";
            this.lvwAgendamentos.Size = new System.Drawing.Size(805, 212);
            this.lvwAgendamentos.TabIndex = 21;
            this.lvwAgendamentos.UseCompatibleStateImageBehavior = false;
            this.lvwAgendamentos.View = System.Windows.Forms.View.Details;
            this.lvwAgendamentos.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwAgendamentos_ColumnClick);
            // 
            // colDataHora
            // 
            this.colDataHora.Text = "Data e Hora";
            this.colDataHora.Width = 105;
            // 
            // colBanco
            // 
            this.colBanco.Text = "Banco";
            this.colBanco.Width = 98;
            // 
            // colScript
            // 
            this.colScript.Text = "Script";
            this.colScript.Width = 434;
            // 
            // colTipo
            // 
            this.colTipo.Text = "Tipo";
            this.colTipo.Width = 72;
            // 
            // btIniciarParar
            // 
            this.btIniciarParar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btIniciarParar.Location = new System.Drawing.Point(709, 292);
            this.btIniciarParar.Name = "btIniciarParar";
            this.btIniciarParar.Size = new System.Drawing.Size(111, 26);
            this.btIniciarParar.TabIndex = 22;
            this.btIniciarParar.Text = "Iniciar";
            this.btIniciarParar.UseVisualStyleBackColor = true;
            this.btIniciarParar.Click += new System.EventHandler(this.btIniciarParar_Click);
            // 
            // stStatus
            // 
            this.stStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.stStatus.Location = new System.Drawing.Point(0, 321);
            this.stStatus.Name = "stStatus";
            this.stStatus.Size = new System.Drawing.Size(832, 22);
            this.stStatus.TabIndex = 23;
            this.stStatus.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.Color.Blue;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(44, 17);
            this.lblStatus.Text = "Parado";
            // 
            // tmrProcura
            // 
            this.tmrProcura.Interval = 60000;
            this.tmrProcura.Tick += new System.EventHandler(this.tmrProcura_Tick);
            // 
            // frmScriptAgendado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 343);
            this.Controls.Add(this.stStatus);
            this.Controls.Add(this.btIniciarParar);
            this.Controls.Add(this.lvwAgendamentos);
            this.Controls.Add(this.btAgendar);
            this.Controls.Add(this.lblBanco);
            this.Controls.Add(this.btSelecionarBanco);
            this.Controls.Add(this.txtQualBanco);
            this.Controls.Add(this.lblQualScript);
            this.Controls.Add(this.lblQuando);
            this.Controls.Add(this.optNoQuery);
            this.Controls.Add(this.optQuery);
            this.Controls.Add(this.btSelecionarArquivo);
            this.Controls.Add(this.txtQualScript);
            this.Controls.Add(this.dtDataHora);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(482, 117);
            this.Name = "frmScriptAgendado";
            this.Text = "Agendador de Scripts";
            this.stStatus.ResumeLayout(false);
            this.stStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtDataHora;
        private System.Windows.Forms.TextBox txtQualScript;
        public System.Windows.Forms.Button btSelecionarArquivo;
        private System.Windows.Forms.RadioButton optQuery;
        private System.Windows.Forms.RadioButton optNoQuery;
        private System.Windows.Forms.Label lblQuando;
        private System.Windows.Forms.Label lblQualScript;
        private System.Windows.Forms.Label lblBanco;
        public System.Windows.Forms.Button btSelecionarBanco;
        private System.Windows.Forms.TextBox txtQualBanco;
        public System.Windows.Forms.Button btAgendar;
        private System.Windows.Forms.ListView lvwAgendamentos;
        private System.Windows.Forms.ColumnHeader colDataHora;
        private System.Windows.Forms.ColumnHeader colBanco;
        private System.Windows.Forms.ColumnHeader colScript;
        private System.Windows.Forms.ColumnHeader colTipo;
        private System.Windows.Forms.Button btIniciarParar;
        private System.Windows.Forms.StatusStrip stStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Timer tmrProcura;
    }
}