using GeradorDeRotas.Utils;
using Models;
using MongoDB.Driver;

namespace GeradorDeRotas.Services
{
    public class SaveExcelService
    {
        private readonly IMongoCollection<SaveExcel> _saveExcel;
        public SaveExcelService(IMongoSettings settings)
        {
            var saveExcel = new MongoClient(settings.ConnectionString);
            var database = saveExcel.GetDatabase(settings.DatabaseName);
            _saveExcel = database.GetCollection<SaveExcel>(settings.ExcelCollectionName);
        }
        public SaveExcel Get() => _saveExcel.Find(excel => true).FirstOrDefault();
        public void Create(SaveExcel excel) => _saveExcel.InsertOne(excel);
        public void Remove() => _saveExcel.DeleteOne(x => true);
    }
}