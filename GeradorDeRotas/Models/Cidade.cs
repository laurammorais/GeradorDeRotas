using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GeradorDeRotas.Models
{
    public class Cidade
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; } = ObjectId.GenerateNewId().ToString();
        public List<Equipe> Equipe { get; set; }
        public List<Cidade> Endereco { get; set; }
        public List<string> Servicos { get; set; }
    }
}