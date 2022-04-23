using GeradorDeRotas.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeradorDeRotas.Controllers
{
    public class CidadesController : Controller
    {
        private readonly ExcelService _excelService;
        public CidadesController(ExcelService excelService) => _excelService = excelService;

        [HttpGet]
        public ActionResult FiltrarPorServico(string servico)
        {
            var cidades = _excelService.GetCidadesByServico(servico);

            return Json(new { Cidades = cidades });
        }
    }
}