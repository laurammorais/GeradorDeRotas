using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GeradorDeRotas.Models
{
    public class Pessoa
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; } = ObjectId.GenerateNewId().ToString();

        [Required(ErrorMessage = "Campo obrigatório.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Campo obrigatório.")]
        public string Nome { get; set; }

        public string Cpf { get; set; }

        public bool Disponivel { get; set; } = true;

        public void SetId(string id) => Id = id;
    }
}