using System.Collections.Generic;

namespace GeradorDeRotas.Models
{
    public class Word
    {
        public List<string> Rotas { get; set; } = new List<string>();
        public string Cidade { get; set; }
        public string Servico { get; set; }
        public List<string> EquipeIds { get; set; } = new List<string>();
    }
}