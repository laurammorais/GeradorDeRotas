﻿using System.Threading.Tasks;
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
        private readonly ExcelService _saveExcelService;
        public EquipesController(
            EquipeService equipeService,
            ExcelService excelService,
            PessoaService pessoaService,
            ExcelService saveExcelService)
        {
            _equipeService = equipeService;
            _excelService = excelService;
            _pessoaService = pessoaService;
            _saveExcelService = saveExcelService;
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
            ViewBag.Servicos = new SelectList(_saveExcelService.GetServicos());

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
                foreach (var cpf in equipe.Cpfs)
                {
                    var pessoa = await _pessoaService.GetByCpf(cpf);
                    equipe.Pessoas.Add(pessoa);
                    pessoa.Disponivel = false;
                    await _pessoaService.Update(pessoa.Id, pessoa);
                }
                await _equipeService.Create(equipe);
                return RedirectToAction(nameof(Index));
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
                equipe.SetId(id);
                await _equipeService.Update(id, equipe);
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