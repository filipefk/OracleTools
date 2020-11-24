using System;
using System.Windows.Forms;

namespace Oracle_Tools
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            if (args.Length > 0)
            {
                string _LihaDeComando = args[0].Trim();
                _LihaDeComando = _LihaDeComando.Trim().ToUpper();
                switch (_LihaDeComando)
                {
                    case "EXTRACT_DDL_SOURCE":
                        if (args.Length == 4)
                        {                                  // Usuário,        Senha,          Database Alias
                            Application.Run(new frmExtractDDL(args[1].Trim(), args[2].Trim(), args[3].Trim()));
                        }
                        else
                        {
                            Application.Run(new frmExtractDDL());
                        }
                        break;

                    case "EXTRACT_DDL_USER":
                        if (args.Length == 4)
                        {                                     // Usuário,        Senha,          Database Alias
                            Application.Run(new frmExtractUser(args[1].Trim(), args[2].Trim(), args[3].Trim()));
                        }
                        else
                        {
                            Application.Run(new frmExtractUser());
                        }
                        break;

                    case "DETALHES_USER":
                        if (args.Length == 4)
                        {                                     // Usuário,       Senha,          Database Alias
                            Application.Run(new frmDetalhesUser(args[1].Trim(), args[2].Trim(), args[3].Trim(), ""));
                        }
                        else if (args.Length == 5)
                        {                                     // Usuário,       Senha,          Database Alias, Nome Usuário  
                            Application.Run(new frmDetalhesUser(args[1].Trim(), args[2].Trim(), args[3].Trim(), args[4].Trim()));
                        }
                        else
                        {
                            Application.Run(new frmDetalhesUser());
                        }
                        break;

                    case "DETALHES_ROLE":
                        if (args.Length == 4)
                        {                                     // Usuário,        Senha,          Database Alias
                            Application.Run(new frmDetalhesRole(args[1].Trim(), args[2].Trim(), args[3].Trim(), ""));
                        }
                        else if (args.Length == 5)
                        {                                     // Usuário,       Senha,          Database Alias, Nome Role
                            Application.Run(new frmDetalhesRole(args[1].Trim(), args[2].Trim(), args[3].Trim(), args[4].Trim()));
                        }
                        else
                        {
                            Application.Run(new frmDetalhesRole());
                        }
                        break;

                    case "QUERY_ALERT":
                        if (args.Length == 4)
                        {                                     // Usuário,        Senha,          Database Alias
                            Application.Run(new frmQueryAlert(args[1].Trim(), args[2].Trim(), args[3].Trim()));
                        }
                        else
                        {
                            Application.Run(new frmQueryAlert());
                        }
                        break;

                    case "PROCURA_LOCK_BANCO":
                        if (args.Length == 4)
                        {                                     // Usuário,        Senha,          Database Alias
                            Application.Run(new frmProcuraLock(args[1].Trim(), args[2].Trim(), args[3].Trim()));
                        }
                        else
                        {
                            Application.Run(new frmProcuraLock());
                        }
                        break;

                    case "CONTROLE_CONCORRENCIA":
                        if (args.Length == 4)
                        {                                     // Usuário,        Senha,          Database Alias
                            //Application.Run(new frmControleConcorrencia(args[1].Trim(), args[2].Trim(), args[3].Trim()));
                        }
                        else
                        {
                            Application.Run(new frmControleConcorrencia());
                        }
                        break;

                    case "AGENDAR_SCRIPT":
                        Application.Run(new frmScriptAgendado());
                        break;

                    case "SESSOES":
                        if (args.Length == 4)
                        {                                  // Usuário,        Senha,          Database Alias
                            Application.Run(new frmSessoes(args[1].Trim(), args[2].Trim(), args[3].Trim()));
                        }
                        else
                        {
                            Application.Run(new frmSessoes());
                        }
                        break;

                    case "DBLINKS":
                        if (args.Length == 4)
                        {                                  // Usuário,        Senha,          Database Alias
                            Application.Run(new frmDBLINKs(args[1].Trim(), args[2].Trim(), args[3].Trim()));
                        }
                        else
                        {
                            Application.Run(new frmDBLINKs());
                        }
                        break;

                    case "JOBS_DE_BANCO":
                        if (args.Length == 4)
                        {                                  // Usuário,        Senha,          Database Alias
                            Application.Run(new frmJobs(args[1].Trim(), args[2].Trim(), args[3].Trim()));
                        }
                        else
                        {
                            Application.Run(new frmJobs());
                        }
                        break;

                    default:
                        Application.Run(new frmToolList());
                        break;

                }
            }
            else
            {
                Application.Run(new frmToolList());
            }
        }
    }
}
