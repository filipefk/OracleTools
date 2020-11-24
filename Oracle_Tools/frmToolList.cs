using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Oracle_Tools
{
    public partial class frmToolList : Form
    {
        public frmToolList()
        {
            InitializeComponent();
            btExtractDDLObjetos.Text = "Listar e Extrair DDL de objetos de banco        \r\nÚltimas compilações e Objetos inválidos        ";
            btExtractScriptUser.Text = "Listar e Extrair script de criação de usuários  ";
            btDetalhesUser.Text = "Relatório de permissões de um usuário          ";
        }
        
        private void btExtractDDLObjetos_Click(object sender, EventArgs e)
        {
            string _Parametro = "EXTRACT_DDL_SOURCE";
            Process Processo = new Process();
            Processo.StartInfo.FileName = Application.ExecutablePath;
            Processo.StartInfo.Arguments = @"""" + _Parametro + @"""";
            Processo.Start();
        }

        private void btExtractScriptUser_Click(object sender, EventArgs e)
        {
            string _Parametro = "EXTRACT_DDL_USER";
            Process Processo = new Process();
            Processo.StartInfo.FileName = Application.ExecutablePath;
            Processo.StartInfo.Arguments = @"""" + _Parametro + @"""";
            Processo.Start();
        }

        private void btDetalhesUser_Click(object sender, EventArgs e)
        {
            string _Parametro = "DETALHES_USER";
            Process Processo = new Process();
            Processo.StartInfo.FileName = Application.ExecutablePath;
            Processo.StartInfo.Arguments = @"""" + _Parametro + @"""";
            Processo.Start();
        }

        private void btQueryAlert_Click(object sender, EventArgs e)
        {
            string _Parametro = "QUERY_ALERT";
            Process Processo = new Process();
            Processo.StartInfo.FileName = Application.ExecutablePath;
            Processo.StartInfo.Arguments = @"""" + _Parametro + @"""";
            Processo.Start();
        }

        private void btProcuraLock_Click(object sender, EventArgs e)
        {
            string _Parametro = "PROCURA_LOCK_BANCO";
            Process Processo = new Process();
            Processo.StartInfo.FileName = Application.ExecutablePath;
            Processo.StartInfo.Arguments = @"""" + _Parametro + @"""";
            Processo.Start();
        }

        private void btControleConcorrencia_Click(object sender, EventArgs e)
        {
            string _Parametro = "CONTROLE_CONCORRENCIA";
            Process Processo = new Process();
            Processo.StartInfo.FileName = Application.ExecutablePath;
            Processo.StartInfo.Arguments = @"""" + _Parametro + @"""";
            Processo.Start();
        }

        private void btDetalhesRole_Click(object sender, EventArgs e)
        {
            string _Parametro = "DETALHES_ROLE";
            Process Processo = new Process();
            Processo.StartInfo.FileName = Application.ExecutablePath;
            Processo.StartInfo.Arguments = @"""" + _Parametro + @"""";
            Processo.Start();
        }

        private void btAgendadorDeScripts_Click(object sender, EventArgs e)
        {
            string _Parametro = "AGENDAR_SCRIPT";
            Process Processo = new Process();
            Processo.StartInfo.FileName = Application.ExecutablePath;
            Processo.StartInfo.Arguments = @"""" + _Parametro + @"""";
            Processo.Start();
        }

        private void btSessoesAtivas_Click(object sender, EventArgs e)
        {
            string _Parametro = "SESSOES";
            Process Processo = new Process();
            Processo.StartInfo.FileName = Application.ExecutablePath;
            Processo.StartInfo.Arguments = @"""" + _Parametro + @"""";
            Processo.Start();
        }

        private void btJobs_Click(object sender, EventArgs e)
        {
            string _Parametro = "JOBS_DE_BANCO";
            Process Processo = new Process();
            Processo.StartInfo.FileName = Application.ExecutablePath;
            Processo.StartInfo.Arguments = @"""" + _Parametro + @"""";
            Processo.Start();
        }



    }
}
