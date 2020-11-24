using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Oracle_Tools
{
    public partial class frmInfoBanco : Form
    {
        public frmInfoBanco(string p_Usuario, string p_Senha, string p_Database)
        {
            InitializeComponent();

            csOracle _csOracle = new csOracle();
            string _Mensagem = "";

            string _Consulta = "";
            _Consulta = _Consulta + "SELECT 'SERVER_HOST' AS NOME, UPPER(SYS_CONTEXT('USERENV', 'SERVER_HOST')) AS VALOR FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'IP_ADRESS', UTL_INADDR.GET_HOST_ADDRESS FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'INSTANCE_NAME' AS NOME, UPPER(INSTANCE_NAME) AS VALOR FROM V$INSTANCE UNION ALL ";
            _Consulta = _Consulta + "SELECT 'DB_NAME', SYS_CONTEXT('USERENV', 'DB_NAME') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'TNSNAMES_ALIAS', '" + p_Database + "' FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'ULTIMO_RESTORE' AS NOME, TO_CHAR(MAX(RESETLOGS_TIME), 'DD/MM/YYYY HH24:MI:SS') FROM GV$DATABASE_INCARNATION UNION ALL ";
            _Consulta = _Consulta + "SELECT 'AUTHENTICATED_IDENTITY', SYS_CONTEXT('USERENV', 'AUTHENTICATED_IDENTITY') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'AUTHENTICATION_METHOD', SYS_CONTEXT('USERENV', 'AUTHENTICATION_METHOD') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'AUTHENTICATION_TYPE', SYS_CONTEXT('USERENV', 'AUTHENTICATION_TYPE') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'CURRENT_SCHEMA', SYS_CONTEXT('USERENV', 'CURRENT_SCHEMA') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'CURRENT_SCHEMAID', SYS_CONTEXT('USERENV', 'CURRENT_SCHEMAID') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'CURRENT_USER', SYS_CONTEXT('USERENV', 'CURRENT_USER') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'CURRENT_USERID', SYS_CONTEXT('USERENV', 'CURRENT_USERID') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'LOCAL_HOST', SYS_CONTEXT('USERENV', 'HOST') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'LOCAL_USER', SYS_CONTEXT('USERENV', 'OS_USER') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'ISDBA', SYS_CONTEXT('USERENV', 'ISDBA') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'LANG', SYS_CONTEXT('USERENV', 'LANG') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'LANGUAGE', SYS_CONTEXT('USERENV', 'LANGUAGE') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'MODULE', SYS_CONTEXT('USERENV', 'MODULE') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'NETWORK_PROTOCOL', SYS_CONTEXT('USERENV', 'NETWORK_PROTOCOL') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'NLS_CALENDAR', SYS_CONTEXT('USERENV', 'NLS_CALENDAR') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'NLS_CURRENCY', SYS_CONTEXT('USERENV', 'NLS_CURRENCY') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'NLS_DATE_FORMAT', SYS_CONTEXT('USERENV', 'NLS_DATE_FORMAT') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'NLS_DATE_LANGUAGE', SYS_CONTEXT('USERENV', 'NLS_DATE_LANGUAGE') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'NLS_SORT', SYS_CONTEXT('USERENV', 'NLS_SORT') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'NLS_TERRITORY', SYS_CONTEXT('USERENV', 'NLS_TERRITORY') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'SESSION_USER', SYS_CONTEXT('USERENV', 'SESSION_USER') FROM DUAL UNION ALL ";
            _Consulta = _Consulta + "SELECT 'SID', SYS_CONTEXT('USERENV', 'SID') FROM DUAL ";

            _csOracle.PreencheLvw(p_Usuario, p_Senha, p_Database, _Consulta, false, ref _Mensagem, ref lvwInfoBanco);
            this.ShowDialog();
        }

        private void btCopiar_Click(object sender, EventArgs e)
        {
            string _Texto = "";

            for (int coluna = 0; coluna < lvwInfoBanco.Columns.Count; coluna++)
			{
                if (coluna > 0)
                {
                    _Texto = _Texto + "\t";
                }
                _Texto = _Texto + lvwInfoBanco.Columns[coluna].Text;
			}

            _Texto = _Texto + "\r\n";

            for (int linha = 0; linha < lvwInfoBanco.Items.Count; linha++)
            {
                for (int coluna = 0; coluna < lvwInfoBanco.Columns.Count; coluna++)
                {
                    if (coluna > 0)
                    {
                        _Texto = _Texto + "\t";
                        _Texto = _Texto + lvwInfoBanco.Items[linha].SubItems[coluna].Text;
                    }
                    else
                    {
                        _Texto = _Texto + lvwInfoBanco.Items[linha].Text;
                    }
                }
                _Texto = _Texto + "\r\n";
            }

            Clipboard.SetData(DataFormats.Text, (object)_Texto);

            MessageBox.Show("Dados copiados para a área de transferência\r\nDados separados por TAB, colar no excel fica bom.", "Info Banco", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
