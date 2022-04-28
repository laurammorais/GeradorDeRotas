using System.Threading.Tasks;
using GeradorDeRotas.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeradorDeRotas.Controllers
{
    public class SelectController : Controller
    {
        private readonly ExcelService _excelService;
        private readonly EquipeService _equipeService;
        public SelectController(
            ExcelService excelService,
            EquipeService equipeService)
        {
            _excelService = excelService;
            _equipeService = equipeService;
        }

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

        [HttpGet]
        public async Task<ActionResult> FiltrarEquipesPorCidade(string cidade) => Json(new { Equipes = await _equipeService.GetDisponivelByCidade(cidade) });
    }
}