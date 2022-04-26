using System.Collections.Generic;
using System.Linq;
using GeradorDeRotas.Utils;
using Models;
using MongoDB.Driver;

namespace GeradorDeRotas.Services
{
	public class ExcelService
    {
        private readonly IMongoCollection<Excel> _excelService;
        public ExcelService(IMongoSettings settings)
        {
            var excel = new MongoClient(settings.ConnectionString);
            var database = excel.GetDatabase(settings.DatabaseName);
            _excelService = database.GetCollection<Excel>(settings.ExcelCollectionName);
        }

        public Excel Get() => _excelService.Find(excel => true).FirstOrDefault();
        public void Create(Excel excel) => _excelService.InsertOne(excel);
        public void Remove() => _excelService.DeleteOne(x => true);
        public IEnumerable<string> GetServicos() => _excelService.Find(excel => true).FirstOrDefault().ArquivosExcel.Select(x => x.GetValue("SERVIÇO").ToString()).Distinct();
        public IEnumerable<string> GetCidades() => _excelService.Find(excel => true).FirstOrDefault().ArquivosExcel.Select(x => x.GetValue("CIDADE").ToString()).Distinct();
        public IEnumerable<string> GetCidadesByServico(string servico) => _excelService.Find(excel => true).FirstOrDefault().ArquivosExcel.Where(x => x.GetValue("SERVIÇO").ToString() == servico).Select(x => x.GetValue("CIDADE").ToString()).Distinct();
        public IEnumerable<string> GetServicosByCidade(string cidade) => _excelService.Find(excel => true).FirstOrDefault().ArquivosExcel.Where(x => x.GetValue("CIDADE").ToString() == cidade).Select(x => x.GetValue("SERVIÇO").ToString()).Distinct();
    }
}