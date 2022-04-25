using GeradorDeRotas.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeradorDeRotas.Controllers
{
    public class SelectController : Controller
    {
        private readonly ExcelService _excelService;
        public SelectController(ExcelService excelService) => _excelService = excelService;

        [HttpGet]
        public ActionResult FiltrarCidadesPorServico(string servico)
        {
            if (servico == null)
                return Json(new { Cidades = _excelService.GetCidades() });

            return Json(new { Cidades = _excelService.GetCidadesByServico(servico) });
        }

        [HttpGet]
        public ActionResult FiltrarServicosPorCidade(string cidade)
        {
            if (cidade == null)
                return Json(new { Servicos = _excelService.GetServicos() });

            return Json(new { Servicos = _excelService.GetServicosByCidade(cidade) });
        }
    }
}