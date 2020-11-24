using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace Oracle_Tools
{
    class csAgendaDeScripts
    {
        // Salvar Lista
        // Carregar Lista
        // Adicionar item
        // Remover item
        // Procurar item a ser executado

        public string CaminhoArquivoAgenda = "";
        public ArrayList ListaScriptsAgendados = new ArrayList();
        private string _ConteudoArquivoAgenda = "";

        public csAgendaDeScripts(string p_ArquivoAgenda)
        {
            if (File.Exists(p_ArquivoAgenda))
            {
                this.CarregouAgenda(p_ArquivoAgenda);
            }
            else
            {
                try
                {
                    File.WriteAllText(p_ArquivoAgenda, "", Encoding.Default);
                }
                catch (Exception _Exception)
                {
                    MessageBox.Show("Problemas para salvar o arquivo\n" + p_ArquivoAgenda + "\n" + _Exception.Message, "Construtor de csAgendaDeScripts", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            CaminhoArquivoAgenda = p_ArquivoAgenda;
        }

        public int AdicionarNaAgenda(csScriptAgendado p_csScriptAgendado)
        {
            return ListaScriptsAgendados.Add(p_csScriptAgendado);
        }

        public void RemoverDaAgenda(csScriptAgendado p_csScriptAgendado)
        {
            ListaScriptsAgendados.Remove(p_csScriptAgendado);
        }

        public bool SalvouAgenda(string p_ArquivoAgenda)
        {
            CaminhoArquivoAgenda = p_ArquivoAgenda;
            return this.SalvouAgenda();
        }

        public bool SalvouAgenda()
        {
            if (CaminhoArquivoAgenda.Trim().Length == 0)
            {
                MessageBox.Show("Arquivo de agenda não definido", "csAgendaDeScripts.SalvouAgenda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!File.Exists(CaminhoArquivoAgenda))
            {
                MessageBox.Show("Arquivo de agenda não encontrado\n" + CaminhoArquivoAgenda, "csAgendaDeScripts.SalvouAgenda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Listar itens da agenda e salvar no arquivo
            csScriptAgendado _csScriptAgendado;
            string _Linha = "";
            string _ConteudoArquivo = "";
            for (int agendamento = 0; agendamento < ListaScriptsAgendados.Count; agendamento++)
            {
                _csScriptAgendado = (csScriptAgendado)ListaScriptsAgendados[agendamento];
                _Linha = _csScriptAgendado.DataHora.ToString();
                _Linha = _Linha + "\t" + _csScriptAgendado.DadosBanco.Usuario;
                _Linha = _Linha + "\t" + csUtil.Encriptar(_csScriptAgendado.DadosBanco.Senha);
                _Linha = _Linha + "\t" + _csScriptAgendado.DadosBanco.Database;
                _Linha = _Linha + "\t" + _csScriptAgendado.CaminhoScript;
                _Linha = _Linha + "\t" + _csScriptAgendado.HeUmaQuery.ToString();
                _Linha = _Linha + "\r\n";
                _ConteudoArquivo = _ConteudoArquivo + _Linha;
            }

            try
            {
                File.WriteAllText(CaminhoArquivoAgenda, _ConteudoArquivo, Encoding.Default);
                return true;
            }
            catch (Exception _Exception)
            {
                MessageBox.Show("Problemas para salvar o arquivo\n" + CaminhoArquivoAgenda + "\n" + _Exception.Message, "SalvouAgenda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool CarregouAgenda(string p_ArquivoAgenda)
        {
            CaminhoArquivoAgenda = p_ArquivoAgenda;
            return this.CarregouAgenda();
        }

        public bool CarregouAgenda()
        {
            bool _Retorno = false;
            if (CaminhoArquivoAgenda.Trim().Length == 0)
            {
                MessageBox.Show("Arquivo de agenda não definido", "csAgendaDeScripts.CarregouAgenda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!File.Exists(CaminhoArquivoAgenda))
            {
                MessageBox.Show("Arquivo de agenda não encontrado\n" + CaminhoArquivoAgenda, "csAgendaDeScripts.CarregouAgenda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Abrir o arquivo e carregar para memória
            _ConteudoArquivoAgenda = File.ReadAllText(CaminhoArquivoAgenda, Encoding.Default);
            if (_ConteudoArquivoAgenda.Trim().Length > 0)
            {
                // Para cada linha do arquivo instanciar um objeto csScriptAgendado e adicionar na lista
                string[] _Linhas;
                string[] _Colunas;
                string _Conteudo = _ConteudoArquivoAgenda;
                csScriptAgendado _csScriptAgendado;

                _Conteudo = _Conteudo.Replace("\r", "");
                _Linhas = _Conteudo.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int linha = 0; linha < _Linhas.Length; linha++)
                {
                    _Colunas = _Linhas[linha].Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    _csScriptAgendado = new csScriptAgendado();
                    _csScriptAgendado.DataHora = DateTime.Parse(_Colunas[0]);
                    _csScriptAgendado.DadosBanco.Usuario = _Colunas[1];
                    _csScriptAgendado.DadosBanco.Senha = csUtil.Desencriptar(_Colunas[2]);
                    _csScriptAgendado.DadosBanco.Database = _Colunas[3];
                    _csScriptAgendado.CaminhoScript = _Colunas[4];
                    _csScriptAgendado.HeUmaQuery = bool.Parse(_Colunas[5]);
                    this.AdicionarNaAgenda(_csScriptAgendado);
                }
            }

            return _Retorno;
        }
    }
}

