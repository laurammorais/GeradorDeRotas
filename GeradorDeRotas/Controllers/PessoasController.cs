using GeradorDeRotas.Models;
using GeradorDeRotas.Services;
using GeradorDeRotas.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult Index() => View(_pessoaService.Get());

        // GET: PessoasController/Details/5
        [Authorize]
        public ActionResult Details(string id) => View(_pessoaService.Get(id));

        // GET: PessoasController/Create
        [Authorize]
        public ActionResult Create() => View();

        // POST: PessoasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Pessoa pessoa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pessoa.Cpf) || !CpfValidator.CpfValido(pessoa.Cpf))
                {
                    TempData["cpfInvalido"] = "CPF inválido!";
                    return View();
                }

                if (_pessoaService.GetByCpf(pessoa.Cpf) != null)
                {
                    TempData["cpfCadastrado"] = "CPF já cadastrado!";
                    return View();
                }

                _pessoaService.Create(pessoa);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PessoasController/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            var pessoa = _pessoaService.Get(id);
            if (pessoa == null)
                return NotFound("Pessoa não encontrada!");
            return View(pessoa);
        }

        // POST: PessoasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(string id, Pessoa pessoa)
        {
            try
            {
                var equipe = _equipeService.GetByCpf(pessoa.Cpf);
                if (equipe != null)
                {
                    equipe.Pessoas.RemoveAll(x => x.Cpf == pessoa.Cpf);
                    pessoa.Disponivel = false;
                    equipe.Pessoas.Add(pessoa);
                    _equipeService.Update(equipe.Id, equipe);
                }

                if (!CpfValidator.CpfValido(pessoa.Cpf))
                {
                    TempData["cpfInvalido"] = "CPF inválido!";
                    return View();
                }

                pessoa.SetId(id);
                _pessoaService.Update(id, pessoa);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpDelete]
        [Authorize]
        public ActionResult Delete(string id)
        {
            try
            {
                _pessoaService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {

                return RedirectToAction(nameof(Index));
            }
        }
    }
}