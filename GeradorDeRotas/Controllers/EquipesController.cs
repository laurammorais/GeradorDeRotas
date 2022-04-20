using System.Linq;
using GeradorDeRotas.Models;
using GeradorDeRotas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GeradorDeRotas.Controllers
{
    public class EquipesController : Controller
    {
        private readonly EquipeService _equipeService;
        private readonly ExcelService _excelService;
        private readonly PessoaService _pessoaService;
        public EquipesController(
            EquipeService equipeService,
            ExcelService excelService,
            PessoaService pessoaService)
        {
            _equipeService = equipeService;
            _excelService = excelService;
            _pessoaService = pessoaService;
        }

        // GET: EquipesController
        public ActionResult Index() => View(_equipeService.Get());

        // GET: EquipesController/Details/5
        public ActionResult Details(string id) => View(_equipeService.Get(id));

        // GET: EquipesController/Create
        public ActionResult Create([FromQuery] string servico = null)
        {
            var pessoas = _pessoaService.GetDisponivel();
            ViewBag.Pessoas = new MultiSelectList(pessoas, "Cpf", "Nome");
            pessoas.Remove(pessoas.Last());
            ViewBag.Servicos = new SelectList(_excelService.GetServicos());

            return View();
        }

        // POST: EquipesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: EquipesController/Delete/5
        public ActionResult Delete(string id)
        {
            var equipe = _equipeService.Get(id);
            if (equipe == null)
                return NotFound("Equipe não encontrada!");
            return View(equipe);
        }

        // POST: EquipesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(string id)
        {
            try
            {
                _equipeService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult SelectServices()
        {
            ViewBag.Servicos = _excelService.GetServicos();
            return View();
        }

        [HttpGet]
        public ActionResult Test([FromQuery] string servico)
        {
            ViewBag.Cidades = new SelectList(_excelService.GetCidadesByServico(servico));
            return RedirectToAction(nameof(Create), new { servico });
        }
    }
}