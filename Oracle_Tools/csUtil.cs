using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;

namespace Oracle_Tools
{
	public static class csUtil
    {

    #region Constantes Privadas
        private static string _CaminhoRegParametros = @"SOFTWARE\ORACLE_Tools\Parametros";
        private static string _CaminhoRegSubKey = @"SOFTWARE\ORACLE_Tools";
        private static string _CaminhoRegApp = "ORACLE_Tools";
    #endregion Constantes Privadas

        #region Número randômico

        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(min, max);
            }
        }

        #endregion  Número randômico

    #region Estruturas (struct)

        public struct stParametro
        {
            public string NomeParametro;
            public string ValoParametro;
            public string NomeAmigavel;

            public stParametro(string p_NomeParametro, string p_ValoParametro, string p_NomeAmigavel)
            {
                NomeParametro = p_NomeParametro;
                ValoParametro = p_ValoParametro;
                NomeAmigavel = p_NomeAmigavel;
            }
        }

    #endregion Estruturas (struct)

    #region Nome de arquivo
        public enum enuParteNomeArquivo
		{
			CaminhoCompleto,
			Pasta,
			Nome,
			Extencao,
			NomeExtencao
		}

        public static string PastaLocalExecutavel()
        {
            return csUtil.ParteNomeArquivo(Application.ExecutablePath, csUtil.enuParteNomeArquivo.Pasta);
        }

		public static string ParteNomeArquivo(string CaminhoCompleto, enuParteNomeArquivo QualParte)
		{
			string Retorno = "";
			string[] vetSplit;
			if (CaminhoCompleto.Trim().Length > 0)
			{
				switch (QualParte)
				{
					case enuParteNomeArquivo.CaminhoCompleto:
						Retorno = CaminhoCompleto;
						break;
					case enuParteNomeArquivo.Pasta:
						vetSplit = CaminhoCompleto.Split(new Char[] { '\\' });
						for (int i = 0; i <= vetSplit.Length - 2; i++)
						{
							Retorno = Retorno + vetSplit[i] + "\\";
						}
						break;
					case enuParteNomeArquivo.Nome:
						vetSplit = CaminhoCompleto.Split(new Char[] { '\\' });
						vetSplit = vetSplit[vetSplit.Length - 1].Split(new Char[] { '.' });
                        if (vetSplit.Length == 1)
                        {
                            Retorno = vetSplit[0];
                        }
                        else
                        {
                            for (int i = 0; i <= vetSplit.Length - 2; i++)
                            {
                                Retorno = Retorno + vetSplit[i];
                                if (i < vetSplit.Length - 2)
                                {
                                    Retorno = Retorno + ".";
                                }
                            }
                        }
						break;
					case enuParteNomeArquivo.Extencao:
						vetSplit = CaminhoCompleto.Split(new Char[] { '\\' });
						vetSplit = vetSplit[vetSplit.Length - 1].Split(new Char[] { '.' });
                        if (vetSplit.Length == 1)
                        {
                            Retorno = "";
                        }
                        else
                        {
                            Retorno = vetSplit[vetSplit.Length - 1];
                        }
						break;
					case enuParteNomeArquivo.NomeExtencao:
						vetSplit = CaminhoCompleto.Split(new Char[] { '\\' });
						Retorno = vetSplit[vetSplit.Length - 1];
						break;
				}
			}

			return Retorno;
		}

        public static bool CriouPasta(string p_CaminhoCompleto)
        {
            string _Caminho = "";

            string[] vetSplit = p_CaminhoCompleto.Split(new Char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            _Caminho = vetSplit[0] + "\\";
            try
            {
                for (int i = 1; i < vetSplit.Length; i++)
                {
                    _Caminho = _Caminho + vetSplit[i] + "\\";
                    if (!Directory.Exists(_Caminho))
                    {
                        Directory.CreateDirectory(_Caminho);
                    }
                }
            }
            catch (Exception _Exception)
            {
                MessageBox.Show("Problemas para criar a pasta\n" + p_CaminhoCompleto + "\n" + _Exception.ToString(), "CriouPasta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

    #endregion Nome de arquivo

    #region Registro do windows
        public enum enuChaveRegistro
		{
			ClassesRoot,
			CurrentUser,
			LocalMachine,
			Users,
			CurrentConfig
		}

		public static void RegSalva(enuChaveRegistro Chave, string SubChave, Hashtable Valores)
		{
			RegistryKey RegChave = AbreSubChave(Chave, SubChave);
			if (RegChave == null)
			{
				RegChave = CriaSubChave(Chave, SubChave);
			}

			foreach (string HashKey in Valores.Keys)
			{
				RegChave.SetValue(HashKey, Valores[HashKey]);
			}

		}

		public static Hashtable RegBusca(enuChaveRegistro Chave, string SubChave)
		{
			RegistryKey RegChave = AbreSubChave(Chave, SubChave);

			Hashtable Retorno = new Hashtable();
			if (RegChave != null)
			{
				string[] NomeCampos = RegChave.GetValueNames();
				foreach (string Nome in NomeCampos)
				{
					Retorno.Add(Nome, RegChave.GetValue(Nome));
				}
				RegChave.Close();
			}
			return Retorno;
		}

		public static Hashtable RegBusca(enuChaveRegistro Chave, string SubChave, string NomeCampo)
		{
			RegistryKey RegChave = AbreSubChave(Chave, SubChave);

			Hashtable Retorno = new Hashtable();
			Retorno.Add(NomeCampo, RegChave.GetValue(NomeCampo));
			return Retorno;
		}

		private static RegistryKey CriaSubChave(enuChaveRegistro Chave, string SubChave)
		{
			RegistryKey RegChave = null;

			string UsuarioLogado = Environment.UserDomainName + "\\" + Environment.UserName;
			RegistrySecurity SegurancaUsuario = new RegistrySecurity();
			RegistryAccessRule RegraUsuario = new RegistryAccessRule(UsuarioLogado, RegistryRights.FullControl, AccessControlType.Allow);
			SegurancaUsuario.AddAccessRule(RegraUsuario);

			string[] Chaves = SubChave.Split(new Char[] { '\\' });
			string SubChaveConcat = "";

			foreach (string SubChaveItem in Chaves)
			{
				if (SubChaveConcat.Length == 0)
					SubChaveConcat = SubChaveConcat + SubChaveItem;
				else
					SubChaveConcat = SubChaveConcat + "\\" + SubChaveItem;
				RegChave = AbreSubChave(Chave, SubChaveConcat);
				if (RegChave == null)
				{
					switch (Chave)
					{
						case enuChaveRegistro.ClassesRoot:
							RegChave = Registry.ClassesRoot.CreateSubKey(SubChaveConcat, RegistryKeyPermissionCheck.ReadWriteSubTree, SegurancaUsuario);
							break;
						case enuChaveRegistro.CurrentUser:
							RegChave = Registry.CurrentUser.CreateSubKey(SubChaveConcat, RegistryKeyPermissionCheck.ReadWriteSubTree, SegurancaUsuario);
							break;
						case enuChaveRegistro.LocalMachine:
							RegChave = Registry.LocalMachine.CreateSubKey(SubChaveConcat, RegistryKeyPermissionCheck.ReadWriteSubTree, SegurancaUsuario);
							break;
						case enuChaveRegistro.Users:
							RegChave = Registry.Users.CreateSubKey(SubChaveConcat, RegistryKeyPermissionCheck.ReadWriteSubTree, SegurancaUsuario);
							break;
						case enuChaveRegistro.CurrentConfig:
							RegChave = Registry.CurrentConfig.CreateSubKey(SubChaveConcat, RegistryKeyPermissionCheck.ReadWriteSubTree, SegurancaUsuario);
							break;
					}
				}
				RegChave.SetAccessControl(SegurancaUsuario);
			}
			return RegChave;
		}

		private static RegistryKey AbreSubChave(enuChaveRegistro Chave, string SubChave)
		{
			RegistryKey RegChave = null;
			switch (Chave)
			{
				case enuChaveRegistro.ClassesRoot:
					RegChave = Registry.ClassesRoot.OpenSubKey(SubChave, true);
					break;
				case enuChaveRegistro.CurrentUser:
					RegChave = Registry.CurrentUser.OpenSubKey(SubChave, true);
					break;
				case enuChaveRegistro.LocalMachine:
					RegChave = Registry.LocalMachine.OpenSubKey(SubChave, true);
					break;
				case enuChaveRegistro.Users:
					RegChave = Registry.Users.OpenSubKey(SubChave, true);
					break;
				case enuChaveRegistro.CurrentConfig:
					RegChave = Registry.CurrentConfig.OpenSubKey(SubChave, true);
					break;
			}
			return RegChave;
		}

    #endregion Registro do windows

    #region Parametros de usuário

        /// <summary>
        /// Carrega o valor salvo no registro em _CaminhoRegParametros.
        /// </summary>
        public static object CarregarPreferencia(string p_Campo)
        {
            object Retorno = null;
            try
            {
                RegistryKey ParametrosUsuario = Registry.CurrentUser.OpenSubKey(_CaminhoRegParametros, true);

                if (ParametrosUsuario != null)
                {
                    Retorno = ParametrosUsuario.GetValue(p_Campo);
                    return Retorno;
                }
                else
                {
                    
                    return null;
                }
            }
            catch (Exception _Exception)
            {
                MessageBox.Show("Problemas para ler o registro do windows\n" + _Exception.ToString(), "Carregar Preferência", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Salva o valor no registro em _CaminhoRegParametros.
        /// </summary>
        public static void SalvarPreferencia(string p_NomeCampo, object p_ValorCampo)
        {
            
            try
            {
                RegistryKey ParametrosUsuario = Registry.CurrentUser.OpenSubKey(_CaminhoRegParametros, true);

                if (ParametrosUsuario == null)
                {
                    ParametrosUsuario = Registry.CurrentUser.OpenSubKey(_CaminhoRegSubKey, true);
                    if (ParametrosUsuario == null)
                    {
                        ParametrosUsuario = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                        ParametrosUsuario.CreateSubKey(_CaminhoRegApp);
                    }
                    ParametrosUsuario = Registry.CurrentUser.OpenSubKey(_CaminhoRegParametros, true);
                    if (ParametrosUsuario == null)
                    {
                        ParametrosUsuario = Registry.CurrentUser.OpenSubKey(_CaminhoRegSubKey, true);
                        ParametrosUsuario.CreateSubKey("Parametros");
                    }
                    ParametrosUsuario = Registry.CurrentUser.OpenSubKey(_CaminhoRegParametros, true);
                }
                ParametrosUsuario.SetValue(p_NomeCampo, p_ValorCampo);
                //
            }
            catch (Exception _Exception)
            {
                MessageBox.Show("Problemas para escrever no registro do windows\n" + _Exception.ToString(), "Salvar Preferência", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        public static string[] ListaPreferencias()
        {
            try
            {
                RegistryKey ParametrosUsuario = Registry.CurrentUser.OpenSubKey(_CaminhoRegParametros, true);

                if (ParametrosUsuario != null)
                {
                    string[] _Retorno = ParametrosUsuario.GetValueNames();
                    return _Retorno;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception _Exception)
            {
                MessageBox.Show("Problemas para ler o registro do windows\n" + _Exception.ToString(), "Carregar Preferência", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }


    #endregion Parametros de usuário

    #region Formularios
        public static void CentralizaForm(Form Formulario)
		{
			if (Formulario.Width < Screen.PrimaryScreen.WorkingArea.Width)
			{
				Formulario.Left = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (Formulario.Width / 2);
			}
			else
			{
				Formulario.Left = 0;
			}

			if (Formulario.Height < Screen.PrimaryScreen.WorkingArea.Height)
			{
				Formulario.Top = (Screen.PrimaryScreen.WorkingArea.Height / 2) - (Formulario.Height / 2);
			}
			else
			{
				Formulario.Top = 0;
			}
		}
    #endregion Formularios

    #region Dialogs
        public static DialogResult InputBoxMultiLine(string p_Titulo, string p_Descricao, ref string p_Texto)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = p_Titulo;
            label.Text = p_Descricao;
            textBox.Text = p_Texto;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancelar";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            textBox.Font = new Font("Arial", 10);
            label.Font = new Font("Arial", 10);

            textBox.Multiline = true;

            label.AutoSize = true;
            textBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.Sizable;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            //form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            form.ClientSize = new Size(450, 350);
            form.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            DialogResult dialogResult = form.ShowDialog();
            p_Texto = textBox.Text;
            return dialogResult;
        }

        public static DialogResult InputBox(string p_Titulo, string p_Descricao, ref string p_Texto)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = p_Titulo;
            label.Text = p_Descricao;
            textBox.Text = p_Texto;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancelar";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            textBox.Font = new Font("Arial", 10);
            label.Font = new Font("Arial", 10);

            textBox.Multiline = false;

            label.AutoSize = true;
            textBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            //form.ClientSize = new Size(450, 350);
            form.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            DialogResult dialogResult = form.ShowDialog();
            p_Texto = textBox.Text;
            form.Dispose();
            GC.Collect();

            return dialogResult;
        }

        public static DialogResult EditaParametros(string p_Titulo, string p_Descricao, ref ArrayList p_ListaParametros)
        {
            Form frmParametrosRegistro = new Form();
            Label[] lblNomes = new Label[p_ListaParametros.Count];
            TextBox[] txtValores = new TextBox[p_ListaParametros.Count];
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            stParametro _ParametroRegistro = new stParametro();
            int _AlturaCampos = 0;
            
            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancelar";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            frmParametrosRegistro.Controls.AddRange(new Control[] { buttonOk, buttonCancel });

            frmParametrosRegistro.Text = p_Titulo;
            for (int i = 0; i < p_ListaParametros.Count; i++)
            {
                lblNomes[i] = new Label();
                txtValores[i] = new TextBox();

                frmParametrosRegistro.Controls.Add(lblNomes[i]);
                frmParametrosRegistro.Controls.Add(txtValores[i]);

                _ParametroRegistro = (stParametro)p_ListaParametros[i];
                if (_ParametroRegistro.NomeAmigavel != null)
                {
                    if (_ParametroRegistro.NomeAmigavel.Trim().Length == 0)
                    {
                        lblNomes[i].Text = _ParametroRegistro.NomeParametro;
                    }
                    else
                    {
                        lblNomes[i].Text = _ParametroRegistro.NomeAmigavel;
                    }
                }
                else
                {
                    lblNomes[i].Text = _ParametroRegistro.NomeParametro;
                }
                
                txtValores[i].Text = _ParametroRegistro.ValoParametro;

                lblNomes[i].Tag = _ParametroRegistro;

                txtValores[i].Font = new Font("Arial", 10);
                lblNomes[i].Font = new Font("Arial", 10);

                txtValores[i].Multiline = false;

                lblNomes[i].AutoSize = true;
            }

            lblNomes[0].SetBounds(9, 1, frmParametrosRegistro.ClientSize.Width - 20, 13);
            txtValores[0].SetBounds(12, 17, frmParametrosRegistro.ClientSize.Width - 20, 20);
            txtValores[0].Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;

            for (int i = 1; i < p_ListaParametros.Count; i++)
            {
                lblNomes[i].SetBounds(9, txtValores[i - 1].Location.Y + txtValores[i - 1].Height + 1, frmParametrosRegistro.ClientSize.Width - 20, 13);
                txtValores[i].SetBounds(12, lblNomes[i].Location.Y + lblNomes[i].Height + 1, frmParametrosRegistro.ClientSize.Width - 20, 20);
                txtValores[i].Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            }

            buttonOk.SetBounds(frmParametrosRegistro.ClientSize.Width - 75 - 7 - 75 - 7, frmParametrosRegistro.ClientSize.Height - 23 - 7, 75, 23);
            buttonCancel.SetBounds(frmParametrosRegistro.ClientSize.Width - 75 - 7, frmParametrosRegistro.ClientSize.Height - 23 - 7, 75, 23);
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            _AlturaCampos = lblNomes[0].Height + 1 + txtValores[0].Height + 1;

            frmParametrosRegistro.ClientSize = new Size(396, lblNomes[0].Location.Y + (_AlturaCampos * p_ListaParametros.Count) + buttonOk.Height + 5);
            
            frmParametrosRegistro.FormBorderStyle = FormBorderStyle.Sizable;
            frmParametrosRegistro.StartPosition = FormStartPosition.CenterScreen;
            frmParametrosRegistro.MinimizeBox = true;
            frmParametrosRegistro.MaximizeBox = true;
            frmParametrosRegistro.AcceptButton = buttonOk;
            frmParametrosRegistro.CancelButton = buttonCancel;
            frmParametrosRegistro.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            DialogResult dialogResult = frmParametrosRegistro.ShowDialog();

            for (int i = 0; i < p_ListaParametros.Count; i++)
            {
                _ParametroRegistro = (stParametro)p_ListaParametros[i];
                _ParametroRegistro.ValoParametro = txtValores[i].Text;
                p_ListaParametros[i] = _ParametroRegistro;
            }

            frmParametrosRegistro.Dispose();
            GC.Collect();

            return dialogResult;

        }

    #endregion Dialogs

    #region Uso do programador
        // Serve para gerar o código com a largura inicial de uma DataGridView.
        // Monte a DataGridView, preencha com os dados, ajuste a largura das colunas na tela em execução e chame esta
        // rotina que ela monta o código com a largura ajustada em tela e joga na área de trasferência, depois é
        // só colar o código na inicialização do formulario
		public static void LarguraColunasDataGridView(DataGridView Grid)
		{
			string Ret = "";

			foreach (DataGridViewColumn item in Grid.Columns)
			{
				Ret = Ret + Grid.Name + ".Columns[" + item.Index.ToString() + "].Width = " + item.Width.ToString() + ";\r\n";
			}
			Clipboard.Clear();
			if (Ret.Length > 0)
				Clipboard.SetText(Ret);
		}

        // Serve para gerar o código com a largura inicial de uma ListView.
        // Monte a ListView, preencha com os dados, ajuste a largura das colunas na tela em execução e chame esta
        // rotina que ela monta o código com a largura ajustada em tela e joga na área de trasferência, depois é
        // só colar o código na inicialização do formulario
        public static void LarguraColunasListView(ListView p_ListView)
        {
            string _Retorno = "";

            for (int coluna = 0; coluna < p_ListView.Columns.Count; coluna++)
            {
                _Retorno = _Retorno + p_ListView.Name + ".Columns[" + coluna.ToString() + "].Width = " + p_ListView.Columns[coluna].Width + ";\r\n";
            }
            Clipboard.Clear();
            if (_Retorno.Length > 0)
            {
                Clipboard.SetText(_Retorno);
            }
                
        }
    #endregion Uso do programador

    #region Encriptação

        private static readonly string PasswordHash = "HD6hhRDf";
        private static readonly string SaltKey = "LKKS@g46";
        private static readonly string VIKey = "UJ63@#dsgeuUIIkg";

		public static string EncriptarMD5(string Texto)
		{
			MD5CryptoServiceProvider Encriptador = new MD5CryptoServiceProvider();
			byte[] ascBytes = Encoding.ASCII.GetBytes(Texto);
			byte[] hashBytes = Encriptador.ComputeHash(ascBytes);

			StringBuilder sb = new StringBuilder();
			for (int c = 0; c < hashBytes.Length; c++)
				sb.AppendFormat("{0:x2}", hashBytes[c]);

			return sb.ToString();
		}

        public static string Encriptar(string p_Texto)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(p_Texto);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Desencriptar(string p_Texto)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(p_Texto);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

    #endregion Encriptação

    #region Associação de tipo de arquivo
        public static void Associar(string p_Extencao,
               string p_NomePrograma, string p_Descricao, string p_CaminhoIcone, string p_CaminhoEXE)
        {
            Registry.ClassesRoot.CreateSubKey(p_Extencao).SetValue("", p_NomePrograma);
            if (p_NomePrograma != null && p_NomePrograma.Length > 0)
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(p_NomePrograma))
                {
                    if (p_Descricao != null)
                        key.SetValue("", p_Descricao);
                    if (p_CaminhoIcone != null)
                        key.CreateSubKey("DefaultIcon").SetValue("", ToShortPathName(p_CaminhoIcone));
                    if (p_CaminhoEXE != null)
                        key.CreateSubKey(@"Shell\Open\Command").SetValue("",
                                    ToShortPathName(p_CaminhoEXE) + " \"%1\"");
                }
        }

        public static bool EstaAssociada(string p_Extencao)
        {
            return (Registry.ClassesRoot.OpenSubKey(p_Extencao, false) != null);
        }

        [DllImport("Kernel32.dll")]
        private static extern uint GetShortPathName(string lpszLongPath,
            [Out] StringBuilder lpszShortPath, uint cchBuffer);

        private static string ToShortPathName(string longName)
        {
            StringBuilder s = new StringBuilder(1000);
            uint iSize = (uint)s.Capacity;
            uint iRet = GetShortPathName(longName, s, iSize);
            return s.ToString();
        }
    #endregion Associação de tipo de arquivo

    #region Abrindo arquivos pelo windows

        public static void SalvarEAbrir(string p_Texto, string p_NomeArquivo)
        {
            string _CaminhoCompletoArquivo = Path.GetTempPath() + p_NomeArquivo;
            File.WriteAllText(_CaminhoCompletoArquivo, p_Texto, Encoding.Default);
            Process Processo = new Process();
            Processo.StartInfo.FileName = _CaminhoCompletoArquivo;
            Processo.Start();
        }

        public static void AbrirArquivo(string p_CaminhoCompletoArquivo)
        {
            Process Processo = new Process();
            Processo.StartInfo.FileName = p_CaminhoCompletoArquivo;
            Processo.Start();
        }

    #endregion Abrindo arquivos pelo windows
    }
}
