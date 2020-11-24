namespace Oracle_Tools
{
    partial class frmQueryAlert
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQueryAlert));
            this.tmrExecuta = new System.Windows.Forms.Timer(this.components);
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.btParar = new System.Windows.Forms.Button();
            this.btExecutar = new System.Windows.Forms.Button();
            this.optAvisarCasoNaoRetorneNada = new System.Windows.Forms.RadioButton();
            this.optAvisarCasoRetorneAlgo = new System.Windows.Forms.RadioButton();
            this.lblExecutando = new System.Windows.Forms.Label();
            this.txtTempo = new System.Windows.Forms.TextBox();
            this.lblTempo = new System.Windows.Forms.Label();
            this.lblTimerInterval = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.optAvisarCasoNaoRetorneErro = new System.Windows.Forms.RadioButton();
            this.optAvisarCasoRetorneErro = new System.Windows.Forms.RadioButton();
            this.mnuMenu = new System.Windows.Forms.MenuStrip();
            this.mnuInfoBanco = new System.Windows.Forms.ToolStripMenuItem();
            this.txtMensagens = new System.Windows.Forms.TextBox();
            this.lvwResultado = new System.Windows.Forms.ListView();
            this.chkMostrarResultado = new System.Windows.Forms.CheckBox();
            this.mnuMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrExecuta
            // 
            this.tmrExecuta.Interval = 1000;
            this.tmrExecuta.Tick += new System.EventHandler(this.tmrExecuta_Tick);
            // 
            // txtQuery
            // 
            this.txtQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuery.Location = new System.Drawing.Point(12, 57);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(868, 125);
            this.txtQuery.TabIndex = 1;
            this.txtQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQuery_KeyDown);
            // 
            // btParar
            // 
            this.btParar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btParar.Location = new System.Drawing.Point(786, 390);
            this.btParar.Name = "btParar";
            this.btParar.Size = new System.Drawing.Size(94, 32);
            this.btParar.TabIndex = 10;
            this.btParar.Text = "Parar Execução";
            this.btParar.UseVisualStyleBackColor = true;
            this.btParar.Click += new System.EventHandler(this.btParar_Click);
            // 
            // btExecutar
            // 
            this.btExecutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExecutar.Location = new System.Drawing.Point(786, 334);
            this.btExecutar.Name = "btExecutar";
            this.btExecutar.Size = new System.Drawing.Size(94, 32);
            this.btExecutar.TabIndex = 9;
            this.btExecutar.Text = "Executar";
            this.btExecutar.UseVisualStyleBackColor = true;
            this.btExecutar.Click += new System.EventHandler(this.btExecutar_Click);
            // 
            // optAvisarCasoNaoRetorneNada
            // 
            this.optAvisarCasoNaoRetorneNada.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.optAvisarCasoNaoRetorneNada.AutoSize = true;
            this.optAvisarCasoNaoRetorneNada.Location = new System.Drawing.Point(11, 357);
            this.optAvisarCasoNaoRetorneNada.Name = "optAvisarCasoNaoRetorneNada";
            this.optAvisarCasoNaoRetorneNada.Size = new System.Drawing.Size(164, 17);
            this.optAvisarCasoNaoRetorneNada.TabIndex = 7;
            this.optAvisarCasoNaoRetorneNada.Text = "Avisar caso não retorne nada";
            this.optAvisarCasoNaoRetorneNada.UseVisualStyleBackColor = true;
            // 
            // optAvisarCasoRetorneAlgo
            // 
            this.optAvisarCasoRetorneAlgo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.optAvisarCasoRetorneAlgo.AutoSize = true;
            this.optAvisarCasoRetorneAlgo.Checked = true;
            this.optAvisarCasoRetorneAlgo.Location = new System.Drawing.Point(11, 334);
            this.optAvisarCasoRetorneAlgo.Name = "optAvisarCasoRetorneAlgo";
            this.optAvisarCasoRetorneAlgo.Size = new System.Drawing.Size(139, 17);
            this.optAvisarCasoRetorneAlgo.TabIndex = 6;
            this.optAvisarCasoRetorneAlgo.TabStop = true;
            this.optAvisarCasoRetorneAlgo.Text = "Avisar caso retorne algo";
            this.optAvisarCasoRetorneAlgo.UseVisualStyleBackColor = true;
            // 
            // lblExecutando
            // 
            this.lblExecutando.AutoSize = true;
            this.lblExecutando.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExecutando.Location = new System.Drawing.Point(13, 31);
            this.lblExecutando.Name = "lblExecutando";
            this.lblExecutando.Size = new System.Drawing.Size(112, 24);
            this.lblExecutando.TabIndex = 11;
            this.lblExecutando.Text = "Desativado";
            // 
            // txtTempo
            // 
            this.txtTempo.Location = new System.Drawing.Point(277, 31);
            this.txtTempo.Name = "txtTempo";
            this.txtTempo.Size = new System.Drawing.Size(46, 20);
            this.txtTempo.TabIndex = 12;
            this.txtTempo.Text = "1000";
            this.txtTempo.TextChanged += new System.EventHandler(this.txtTempo_TextChanged);
            // 
            // lblTempo
            // 
            this.lblTempo.AutoSize = true;
            this.lblTempo.Location = new System.Drawing.Point(231, 34);
            this.lblTempo.Name = "lblTempo";
            this.lblTempo.Size = new System.Drawing.Size(43, 13);
            this.lblTempo.TabIndex = 13;
            this.lblTempo.Text = "Tempo:";
            // 
            // lblTimerInterval
            // 
            this.lblTimerInterval.Location = new System.Drawing.Point(329, 34);
            this.lblTimerInterval.Name = "lblTimerInterval";
            this.lblTimerInterval.Size = new System.Drawing.Size(45, 17);
            this.lblTimerInterval.TabIndex = 14;
            this.lblTimerInterval.Text = "1.000";
            this.lblTimerInterval.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(380, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "milisegundos";
            // 
            // optAvisarCasoNaoRetorneErro
            // 
            this.optAvisarCasoNaoRetorneErro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.optAvisarCasoNaoRetorneErro.AutoSize = true;
            this.optAvisarCasoNaoRetorneErro.Location = new System.Drawing.Point(11, 403);
            this.optAvisarCasoNaoRetorneErro.Name = "optAvisarCasoNaoRetorneErro";
            this.optAvisarCasoNaoRetorneErro.Size = new System.Drawing.Size(158, 17);
            this.optAvisarCasoNaoRetorneErro.TabIndex = 22;
            this.optAvisarCasoNaoRetorneErro.Text = "Avisar caso não retorne erro";
            this.optAvisarCasoNaoRetorneErro.UseVisualStyleBackColor = true;
            // 
            // optAvisarCasoRetorneErro
            // 
            this.optAvisarCasoRetorneErro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.optAvisarCasoRetorneErro.AutoSize = true;
            this.optAvisarCasoRetorneErro.Location = new System.Drawing.Point(11, 380);
            this.optAvisarCasoRetorneErro.Name = "optAvisarCasoRetorneErro";
            this.optAvisarCasoRetorneErro.Size = new System.Drawing.Size(137, 17);
            this.optAvisarCasoRetorneErro.TabIndex = 24;
            this.optAvisarCasoRetorneErro.Text = "Avisar caso retorne erro";
            this.optAvisarCasoRetorneErro.UseVisualStyleBackColor = true;
            // 
            // mnuMenu
            // 
            this.mnuMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuInfoBanco});
            this.mnuMenu.Location = new System.Drawing.Point(0, 0);
            this.mnuMenu.Name = "mnuMenu";
            this.mnuMenu.Size = new System.Drawing.Size(892, 24);
            this.mnuMenu.TabIndex = 50;
            this.mnuMenu.Text = "menuStrip1";
            // 
            // mnuInfoBanco
            // 
            this.mnuInfoBanco.Name = "mnuInfoBanco";
            this.mnuInfoBanco.Size = new System.Drawing.Size(76, 20);
            this.mnuInfoBanco.Text = "Info Banco";
            this.mnuInfoBanco.Click += new System.EventHandler(this.mnuInfoBanco_Click);
            // 
            // txtMensagens
            // 
            this.txtMensagens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMensagens.BackColor = System.Drawing.Color.White;
            this.txtMensagens.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMensagens.Location = new System.Drawing.Point(181, 334);
            this.txtMensagens.Multiline = true;
            this.txtMensagens.Name = "txtMensagens";
            this.txtMensagens.ReadOnly = true;
            this.txtMensagens.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMensagens.Size = new System.Drawing.Size(599, 86);
            this.txtMensagens.TabIndex = 51;
            // 
            // lvwResultado
            // 
            this.lvwResultado.AllowDrop = true;
            this.lvwResultado.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwResultado.CheckBoxes = true;
            this.lvwResultado.FullRowSelect = true;
            this.lvwResultado.GridLines = true;
            this.lvwResultado.HideSelection = false;
            this.lvwResultado.Location = new System.Drawing.Point(11, 211);
            this.lvwResultado.Name = "lvwResultado";
            this.lvwResultado.Size = new System.Drawing.Size(868, 117);
            this.lvwResultado.TabIndex = 52;
            this.lvwResultado.UseCompatibleStateImageBehavior = false;
            this.lvwResultado.View = System.Windows.Forms.View.Details;
            this.lvwResultado.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvwResultado_KeyDown);
            // 
            // chkMostrarResultado
            // 
            this.chkMostrarResultado.AutoSize = true;
            this.chkMostrarResultado.Location = new System.Drawing.Point(11, 188);
            this.chkMostrarResultado.Name = "chkMostrarResultado";
            this.chkMostrarResultado.Size = new System.Drawing.Size(112, 17);
            this.chkMostrarResultado.TabIndex = 53;
            this.chkMostrarResultado.Text = "Mostrar Resultado";
            this.chkMostrarResultado.UseVisualStyleBackColor = true;
            // 
            // frmQueryAlert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 434);
            this.Controls.Add(this.chkMostrarResultado);
            this.Controls.Add(this.lvwResultado);
            this.Controls.Add(this.txtMensagens);
            this.Controls.Add(this.mnuMenu);
            this.Controls.Add(this.optAvisarCasoRetorneErro);
            this.Controls.Add(this.optAvisarCasoNaoRetorneErro);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblTimerInterval);
            this.Controls.Add(this.lblTempo);
            this.Controls.Add(this.txtTempo);
            this.Controls.Add(this.lblExecutando);
            this.Controls.Add(this.btParar);
            this.Controls.Add(this.btExecutar);
            this.Controls.Add(this.optAvisarCasoNaoRetorneNada);
            this.Controls.Add(this.optAvisarCasoRetorneAlgo);
            this.Controls.Add(this.txtQuery);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(492, 418);
            this.Name = "frmQueryAlert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Executa Query";
            this.mnuMenu.ResumeLayout(false);
            this.mnuMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrExecuta;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.Button btParar;
        private System.Windows.Forms.Button btExecutar;
        private System.Windows.Forms.RadioButton optAvisarCasoNaoRetorneNada;
        private System.Windows.Forms.RadioButton optAvisarCasoRetorneAlgo;
        private System.Windows.Forms.Label lblExecutando;
        private System.Windows.Forms.TextBox txtTempo;
        private System.Windows.Forms.Label lblTempo;
        private System.Windows.Forms.Label lblTimerInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton optAvisarCasoNaoRetorneErro;
        private System.Windows.Forms.RadioButton optAvisarCasoRetorneErro;
        private System.Windows.Forms.MenuStrip mnuMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuInfoBanco;
        private System.Windows.Forms.TextBox txtMensagens;
        public System.Windows.Forms.ListView lvwResultado;
        private System.Windows.Forms.CheckBox chkMostrarResultado;
    }
}

