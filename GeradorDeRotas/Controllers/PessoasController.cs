using GeradorDeRotas.Models;
using GeradorDeRotas.Services;
using GeradorDeRotas.Validators;
using Microsoft.AspNetCore.Mvc;

namespace GeradorDeRotas.Controllers
{
    public class PessoasController : Controller
    {
        private readonly PessoaService _pessoaService;
        public PessoasController(PessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        // GET: PessoasController
        public ActionResult Index() => View(_pessoaService.Get());

        // GET: PessoasController/Details/5
        public ActionResult Details(string id) => View(_pessoaService.Get(id));

        // GET: PessoasController/Create
        public ActionResult Create() => View();

        // POST: PessoasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pessoa pessoa)
        {
            try
            {
                if (!CpfValidator.CpfValido(pessoa.Cpf))
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
        public ActionResult Edit(string id, Pessoa pessoa)
        {
            try
            {
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

        // GET: PessoasController/Delete/5
        public ActionResult Delete(string id)
        {
            var pessoa = _pessoaService.Get(id);
            if (pessoa == null)
                return NotFound("Pessoa não encontrada!");
            return View(pessoa);
        }

        // POST: PessoasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Pessoa pessoa)
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