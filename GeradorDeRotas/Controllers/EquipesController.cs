using GeradorDeRotas.Models;
using GeradorDeRotas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GeradorDeRotas.Controllers
{
    public class EquipesController : Controller
    {
        private readonly EquipeService _equipeService;
        private readonly ExcelService _excelService;
        private readonly PessoaService _pessoaService;
        private readonly SaveExcelService _saveExcelService;
        public EquipesController(
            EquipeService equipeService,
            ExcelService excelService,
            PessoaService pessoaService,
            SaveExcelService saveExcelService)
        {
            _equipeService = equipeService;
            _excelService = excelService;
            _pessoaService = pessoaService;
            _saveExcelService = saveExcelService;
        }

        // GET: EquipesController
        [Authorize]
        public ActionResult Index() => View(_equipeService.Get());

        // GET: EquipesController/Details/5
        [Authorize]
        public ActionResult Details(string id) => View(_equipeService.Get(id));

        // GET: EquipesController/Create
        [Authorize]
        public ActionResult Create()
        {
            var pessoas = _pessoaService.GetDisponivel();
            ViewBag.Pessoas = new MultiSelectList(pessoas, "Cpf", "Nome");
            ViewBag.Servicos = new SelectList(_excelService.GetServicos());

            return View();
        }

        // POST: EquipesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Equipe equipe)
        {
            try
            {
                foreach (var cpf in equipe.Cpfs)
                {
                    var pessoa = _pessoaService.GetByCpf(cpf);
                    equipe.Pessoas.Add(pessoa);
                    pessoa.Disponivel = false;
                    _pessoaService.Update(pessoa.Id, pessoa);
                }
                _equipeService.Create(equipe);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EquipesController/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            var equipe = _equipeService.Get(id);
            if (equipe == null)
                return NotFound("Equipe não encontrada!");
            return View(equipe);
        }

        // POST: EquipesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(string id, Equipe equipe)
        {
            try
            {
                equipe.SetId(id);
                _equipeService.Update(id, equipe);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: EquipesController/Delete/5
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            try
            {
                var equipe = _equipeService.Get(id);
                foreach (var pessoa in equipe.Pessoas)
                {
                    pessoa.Disponivel = true;
                    _pessoaService.Update(pessoa.Id, pessoa);
                }

                _equipeService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public ActionResult ListarCidades(string servico)
        {
            var cidades = _excelService.GetCidadesByServico(servico);

            return Json(new { Cidades = cidades });
        }
    }
}