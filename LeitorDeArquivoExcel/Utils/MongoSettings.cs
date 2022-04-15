namespace LeitorDeArquivoExcel.Utils
{
    public class MongoSettings
    {
        public string ExcelCollectionName { get; private set; } = "Excel";
        public string ConnectionString { get; private set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; private set; } = "GeradorDeRotas";
    }
}