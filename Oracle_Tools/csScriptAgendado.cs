using System;
using System.Collections.Generic;
using System.Text;

namespace Oracle_Tools
{
    class csScriptAgendado
    {
        public DateTime DataHora = DateTime.MinValue;
        public csDadosConexao DadosBanco = new csDadosConexao();
        public string CaminhoScript = "";
        public bool HeUmaQuery = false;
    }
}
