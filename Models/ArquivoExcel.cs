using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    [BsonIgnoreExtraElements]
    public class ArquivoExcel
    {
        [BsonElement("CIDADE")]
        public string Cidade { get; set; }

        [BsonElement("SERVIÇO")]
        public string Servico { get; set; }
    }
}