using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Pessoa
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Nome { get; set; }

        public string Cpf { get; set; }

        public bool Disponivel { get; set; } = true;

        [BsonIgnore]
        public bool Valido => !string.IsNullOrWhiteSpace(Nome) && !string.IsNullOrWhiteSpace(Cpf);

        public void SetId(string id) => Id = id;
    }
}