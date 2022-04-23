using System.Collections.Generic;
using System.Linq;
using GeradorDeRotas.Models;
using GeradorDeRotas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GeradorDeRotas.Controllers
{
    public class WordController : Controller
    {
        private readonly SaveExcelService _saveExcelService;
        public WordController(SaveExcelService saveExcelService)
        {
            _saveExcelService = saveExcelService;
        }

        [Authorize]
        public ActionResult Export()
        {
            var rotas = new List<string>();

            var excel = _saveExcelService.Get();

            foreach (var linhaExcel in excel.ArquivosExcel)
            {
                foreach (var item in linhaExcel)
                {
                    rotas.Add(item.Name);
                }
            }

            ViewBag.Rotas = new SelectList(rotas);
            return View();
        }

        // POST: WordController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Export(Word word)
        {
            try
            {
                if (!word.Rotas.Any())
                {
                    TempData["rotasInvalidas"] = "Nenhuma rota selecionada!";
                    return RedirectToAction(nameof(Export));
                }

                return RedirectToAction(nameof(Export));
            }
            catch
            {
                return RedirectToAction(nameof(Export));
            }
        }
    }
}
