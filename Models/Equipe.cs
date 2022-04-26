using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Equipe
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string NomeEquipe { get; set; }

        public List<Pessoa> Pessoas { get; set; } = new List<Pessoa>();

        public string Cidade { get; set; }

        public List<string> Servicos { get; set; } = new List<string>();

        public int ContagemDeServico { get; set; }

        public bool Disponivel { get; set; } = true;

        [BsonIgnore]
        public List<string> Cpfs { get; set; } = new List<string>();

        [BsonIgnore]
        public string Cep { get; set; }

        [BsonIgnore]
        public bool Valida => !string.IsNullOrEmpty(NomeEquipe) && !string.IsNullOrWhiteSpace(Cidade) && Cpfs.Any();

        [BsonIgnore]
        public bool EditarValido => !string.IsNullOrEmpty(NomeEquipe) && !string.IsNullOrWhiteSpace(Cidade);

        public void IncrementarContagemDeServico()
        {
            ContagemDeServico++;
            if (ContagemDeServico >= 5)
                Disponivel = false;
        }

        public void AlterarCidadeNome(string nomeEquipe, string cidade)
        {
            NomeEquipe = nomeEquipe;
            Cidade = cidade;
        }
    }
}