using System;
using System.IO;

namespace Oracle_Tools
{
    class csObjetoDeBanco
    {
        public enum enuClassificacaoObjeto
        {
            Indefinida,
            Novo,
            Alterado,
            Excluido
        }

        public string Owner = "";
        public string Nome = "";
        public string Tipo = "";
        public DateTime DataCriacao = DateTime.MinValue;
        public DateTime DataAlteracao = DateTime.MinValue;
        public string Status = "";
        public string Fonte = "";
        public string MensagemErro = "";
        public string UsuarioUltimaAlteracao = "";
        public enuClassificacaoObjeto ClassificacaoObjeto = enuClassificacaoObjeto.Indefinida;

        public string NomeArquivo
        {
            get
            {
                string _NomeArquivo;
                _NomeArquivo = this.Owner.Trim().ToUpper() + "." + this.Nome.Trim().ToUpper() + ".";
                switch (this.Tipo.Trim().ToUpper())
                {
                    case "JOB":
                        if (Directory.Exists("D:\\SVN_AES\\trunk\\OBJETOS_ORACLE\\" + this.Owner.Trim().ToUpper() + "\\JOB\\"))
                        {
                            string[] _Arquivos;
                            bool _Achei = false;
                            _Arquivos = Directory.GetFiles("D:\\SVN_AES\\trunk\\OBJETOS_ORACLE\\" + this.Owner.Trim().ToUpper() + "\\JOB\\", _NomeArquivo + "*", SearchOption.AllDirectories);
                            if (_Arquivos.Length > 0)
                            {
                                foreach (string _Arquivo in _Arquivos)
                                {
                                    if (_Arquivo.IndexOf(".svn") == -1)
                                    {
                                        _NomeArquivo = _NomeArquivo + csUtil.ParteNomeArquivo(_Arquivo, csUtil.enuParteNomeArquivo.Extencao);
                                        _Achei = true;
                                        break;
                                    }
                                }
                                if (!_Achei)
                                {
                                    _NomeArquivo = _NomeArquivo + "sql";
                                }
                            }
                            else
                            {
                                _NomeArquivo = _NomeArquivo + "sql";
                            }
                        }
                        else
                        {
                            _NomeArquivo = _NomeArquivo + "sql";
                        }
                        break;
                    case "VIEW":
                        _NomeArquivo = _NomeArquivo + "vw";
                        break;
                    case "SEQUENCE":
                        _NomeArquivo = _NomeArquivo + "sql";
                        break;
                    case "TRIGGER":
                        _NomeArquivo = _NomeArquivo + "trg";
                        break;
                    case "FUNCTION":
                        _NomeArquivo = _NomeArquivo + "fnc";
                        break;
                    case "PROCEDURE":
                        _NomeArquivo = _NomeArquivo + "prc";
                        break;
                    case "PACKAGE":
                        if (Directory.Exists("D:\\SVN_AES\\trunk\\OBJETOS_ORACLE\\" + this.Owner.Trim().ToUpper() + "\\PACKAGE\\"))
	                    {
                            string[] _Arquivos;
                            bool _Achei = false;
                            _Arquivos = Directory.GetFiles("D:\\SVN_AES\\trunk\\OBJETOS_ORACLE\\" + this.Owner.Trim().ToUpper() + "\\PACKAGE\\", _NomeArquivo + "pk*", SearchOption.AllDirectories);
                            if (_Arquivos.Length > 0)
                            {
                                foreach (string _Arquivo in _Arquivos)
                                {
                                    if (_Arquivo.IndexOf(".svn") == -1)
                                    {
                                        _NomeArquivo = _NomeArquivo + csUtil.ParteNomeArquivo(_Arquivo, csUtil.enuParteNomeArquivo.Extencao);
                                        _Achei = true;
                                    }
                                }
                                if (!_Achei)
                                {
                                    _NomeArquivo = _NomeArquivo + "pkb";
                                }
                            }
                            else
                            {
                                _NomeArquivo = _NomeArquivo + "pkb";
                            }
	                    }
                        else
	                    {
                            _NomeArquivo = _NomeArquivo + "pkb";
	                    }
                        break;
                    case "JAVA SOURCE":
                        _NomeArquivo = _NomeArquivo + "jsp";
                        break;
                }
                return _NomeArquivo;
            }
        }

    }
}
