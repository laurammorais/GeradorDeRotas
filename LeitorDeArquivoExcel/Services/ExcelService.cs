using LeitorDeArquivoExcel.Utils;
using Models;
using MongoDB.Driver;

namespace LeitorDeArquivoExcel.Services
{
    public class ExcelService
    {
        private readonly IMongoCollection<Excel> _excel;
        public ExcelService(MongoSettings settings)
        {
            var excel = new MongoClient(settings.ConnectionString);
            var database = excel.GetDatabase(settings.DatabaseName);
            _excel = database.GetCollection<Excel>(settings.ExcelCollectionName);
        }

        public void Create(Excel excel) => _excel.InsertOne(excel);

        public void Remove() => _excel.DeleteOne(x => true);
    }
}