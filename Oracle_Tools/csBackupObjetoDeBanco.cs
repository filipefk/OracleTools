using System;
using System.Collections.Generic;
using System.Text;

namespace Oracle_Tools
{
    class csBackupObjetoDeBanco
    {
        public string Owner = "";
        public string Nome = "";
        public string Tipo = "";
        public DateTime DataBackup = DateTime.MinValue;
        public decimal IDUsuarioQueGerouBackup = 0;
        public string NomeUsuarioQueGerouBackup = "";
        public string EmailUsuarioQueGerouBackup = "";
        public decimal Id_Alteracao = 0;
        public string _Fonte = "";
    }
}
