using System.Linq;
using System.Threading.Tasks;
using GeradorDeRotas.Services;
using GeradorDeRotas.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace GeradorDeRotas.Controllers
{
	public class PessoasController : Controller
    {
        private readonly PessoaService _pessoaService;
        private readonly EquipeService _equipeService;
        public PessoasController(PessoaService pessoaService, EquipeService equipeService)
        {
            _pessoaService = pessoaService;
            _equipeService = equipeService;
        }

        // GET: PessoasController
        [Authorize]
        public async Task<ActionResult> Index() => View(await _pessoaService.Get());

        // GET: PessoasController/Details/5
        [Authorize]
        public async Task<ActionResult> Details(string id) => View(await _pessoaService.Get(id));

        // GET: PessoasController/Create
        [Authorize]
        public ActionResult Create() => View();

        // POST: PessoasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(Pessoa pessoa)
        {
            try
            {
                if (!pessoa.Valido)
                {
                    TempData["camposObrigatorios"] = "Campo Obrigatorio!";
                    return View();
                }

                if (!CpfValidator.CpfValido(pessoa.Cpf))
                {
                    TempData["cpfInvalido"] = "CPF inválido!";
                    return View();
                }

                if (await _pessoaService.GetByCpf(pessoa.Cpf) != null)
                {
                    TempData["cpfCadastrado"] = "CPF já cadastrado!";
                    return View();
                }

                TempData["pessoaSucesso"] = "Pessoa criada com sucesso!";

                await _pessoaService.Create(pessoa);
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: PessoasController/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(string id)
        {
            var pessoa = await _pessoaService.Get(id);
            if (pessoa == null)
                return NotFound("Pessoa não encontrada!");
            return View(pessoa);
        }

        // POST: PessoasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(string id, Pessoa pessoa)
        {
            try
            {
                var equipe = await _equipeService.GetByCpf(pessoa.Cpf);
                if (equipe != null)
                {
                    equipe.Pessoas.RemoveAll(x => x.Cpf == pessoa.Cpf);
                    pessoa.Disponivel = false;
                    equipe.Pessoas.Add(pessoa);
                    await _equipeService.Update(equipe.Id, equipe);
                }

                if (!CpfValidator.CpfValido(pessoa.Cpf))
                {
                    TempData["cpfInvalido"] = "CPF inválido!";
                    return View();
                }

                pessoa.SetId(id);
                await _pessoaService.Update(id, pessoa);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var pessoa = await _pessoaService.Get(id);

                var equipe = await _equipeService.GetByCpf(pessoa.Cpf);
                if (equipe != null)
                {
                    equipe.Pessoas.RemoveAll(x => x.Cpf == pessoa.Cpf);

                    if (!equipe.Pessoas.Any())
                        equipe.Disponivel = false;

                    await _equipeService.Update(equipe.Id, equipe);
                }

				

                await _pessoaService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {

                return RedirectToAction(nameof(Index));
            }
        }
    }
}