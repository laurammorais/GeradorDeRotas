namespace GeradorDeRotas.Utils
{
    public class MongoSettings : IMongoSettings
    {
        public string ConnectionString { get; set; }
        public string EquipeCollectionName { get; set; }
        public string PessoaCollectionName { get; set; }
        public string DatabaseName { get; set; }
        public string ExcelCollectionName { get; set; }
    }
}