using System.Collections.Generic;
using System.Linq;
using GeradorDeRotas.Utils;
using Models;
using MongoDB.Driver;

namespace GeradorDeRotas.Services
{
    public class ExcelService
    {
        private readonly IMongoCollection<Excel> _excel;
        public ExcelService(IMongoSettings settings)
        {
            var excel = new MongoClient(settings.ConnectionString);
            var database = excel.GetDatabase(settings.DatabaseName);
            _excel = database.GetCollection<Excel>(settings.ExcelCollectionName);
        }
        public IEnumerable<string> GetServicos() => _excel.Find(excel => true).FirstOrDefault().ArquivosExcel.Select(x => x.Servico).Distinct();
        public IEnumerable<string> GetCidadesByServico(string servico) => _excel.Find(excel => true).FirstOrDefault().ArquivosExcel.Where(x => x.Servico == servico).Select(x => x.Cidade);
    }
}