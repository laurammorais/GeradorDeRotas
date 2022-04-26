using System.Linq;
using System.Threading.Tasks;
using GeradorDeRotas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

namespace GeradorDeRotas.Controllers
{
    public class EquipesController : Controller
    {
        private readonly EquipeService _equipeService;
        private readonly PessoaService _pessoaService;
        private readonly ExcelService _excelService;
        public EquipesController(
            EquipeService equipeService,
            PessoaService pessoaService,
            ExcelService excelService)
        {
            _equipeService = equipeService;
            _pessoaService = pessoaService;
            _excelService = excelService;
        }

        // GET: EquipesController
        [Authorize]
        public async Task<ActionResult> Index() => View(await _equipeService.Get());

        // GET: EquipesController/Details/5
        [Authorize]
        public async Task<ActionResult> Details(string id) => View(await _equipeService.Get(id));

        // GET: EquipesController/Create
        [Authorize]
        public async Task<ActionResult> Create()
        {
            var pessoas = await _pessoaService.GetDisponivel();
            ViewBag.Pessoas = new MultiSelectList(pessoas, "Cpf", "Nome");
            var excel = _excelService.Get();

            if (excel == null)
			{
                TempData["necessarioExportar"] = "Falha";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cidades = new SelectList(_excelService.GetCidades());

            return View();
        }

        // POST: EquipesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(Equipe equipe)
        {
            try
            {
                if (!equipe.Valida)
                {
                    TempData["equipeInvalida"] = "equipe inválida.";
                    return RedirectToAction(nameof(Create));
                }

                foreach (var cpf in equipe.Cpfs)
                {
                    var pessoa = await _pessoaService.GetByCpf(cpf);
                    equipe.Pessoas.Add(pessoa);
                    pessoa.Disponivel = false;
                    await _pessoaService.Update(pessoa.Id, pessoa);
                }

                TempData["equipeSucesso"] = "Equipe criada com sucesso!";

                await _equipeService.Create(equipe);
                return RedirectToAction(nameof(Create));
            }
            catch
            {
                return View();
            }
        }

        // GET: EquipesController/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Cidades = new SelectList(_excelService.GetCidades());

            var equipe = await _equipeService.Get(id);
            if (equipe == null)
                return NotFound("Equipe não encontrada!");
            return View(equipe);
        }

        // POST: EquipesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(string id, Equipe equipe)
        {
            try
            {
                if (!equipe.EditarValido)
                {
                    TempData["equipeInvalida"] = "equipe inválida.";
                    return RedirectToAction(nameof(Edit));
                }

                var equipeResponse = await _equipeService.Get(id);
                equipeResponse.AlterarCidadeNome(equipe.NomeEquipe, equipe.Cidade);

                await _equipeService.Update(id, equipeResponse);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: EquipesController/Delete/5
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var equipe = await _equipeService.Get(id);
                foreach (var pessoa in equipe.Pessoas)
                {
                    pessoa.Disponivel = true;
                    await _pessoaService.Update(pessoa.Id, pessoa);
                }

                await _equipeService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}