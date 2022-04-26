using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Nome { get; set; }

        public string Username { get; set; }

        public string Senha { get; set; }

        [BsonIgnore]
		public string NovaSenha { get; set; }

		public bool Valido => Nome != null && Username != null && Senha != null;

        public bool AlteracaoValida => Username != null && Senha != null && NovaSenha != null;

        public bool LoginValido(Usuario login) => login != null && login.Username == Username && login.Senha == Senha;
    }
}