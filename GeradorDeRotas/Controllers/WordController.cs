using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GeradorDeRotas.Models;
using GeradorDeRotas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xceed.Words.NET;

namespace GeradorDeRotas.Controllers
{
    public class WordController : Controller
    {
        private readonly ExcelService _saveExcelService;
        private readonly EquipeService _equipeService;
        private readonly ExcelService _excelService;
        public WordController(
            ExcelService saveExcelService,
            EquipeService equipeService,
            ExcelService excelService)
        {
            _saveExcelService = saveExcelService;
            _equipeService = equipeService;
            _excelService = excelService;
        }

        [Authorize]
        public ActionResult Export()
        {
            var rotas = new List<string>();

            var excel = _saveExcelService.Get();

            foreach (var item in excel.ArquivosExcel.First())
                rotas.Add(item.Name);

            rotas.RemoveAll(x => x == "CONTRATO" || x == "ASSINANTE" || x == "ENDEREÇO" || x == "CEP" || x == "OS" || x == "TIPO OS");

            ViewBag.Rotas = new SelectList(rotas);
            ViewBag.Servicos = new SelectList(_saveExcelService.GetServicos());
            ViewBag.Cidades = new SelectList(_excelService.GetCidades());
            return View();
        }

        // POST: WordController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Export(Word word)
        {
            try
            {
                var stream = new MemoryStream();
                var doc = DocX.Create(stream);

                var paragrafo = doc.InsertParagraph();
                paragrafo.Append($"ROTA TRABALHO - {DateTime.Now:d}").Font("Times New Roman").FontSize(18).Bold().Alignment = Xceed.Document.NET.Alignment.center;

                paragrafo = doc.InsertParagraph();
                paragrafo.Append("RETORNOS").Font("Times New Roman").FontSize(15).Bold().Alignment = Xceed.Document.NET.Alignment.center;

                var equipes = await _equipeService.Get();
                var excel = _saveExcelService.Get();

                if (word.Servico != null)
                    equipes = equipes.FindAll(x => x.Servico == word.Servico);

                if (word.Cidade != null)
                    equipes = equipes.FindAll(x => x.Cidade == word.Cidade);

                if (!equipes.Any())
                {
                    TempData["semEquipe"] = "Nenhuma equipe encontrada com este filtro!";
                    return RedirectToAction(nameof(Export));
                }

                foreach (var equipe in equipes)
                {
                    var dadosEquipe = excel.ArquivosExcel.Find(x => x.GetValue("CIDADE") == equipe.Cidade && x.GetValue("SERVIÇO") == equipe.Servico);

                    paragrafo = doc.InsertParagraph();
                    paragrafo = doc.InsertParagraph();
                    paragrafo = doc.InsertParagraph();
                    paragrafo = doc.InsertParagraph();
                    paragrafo.Append($"Nome da Equipe: {equipe.NomeEquipe}").Font("Times New Roman").FontSize(15).Bold();

                    paragrafo = doc.InsertParagraph();
                    paragrafo = doc.InsertParagraph();
                    paragrafo.Append($"Contrato: {dadosEquipe.GetValue("CONTRATO")}  -  Assinante: {dadosEquipe.GetValue("ASSINANTE")}  -  Período: ").Font("Times New Roman").FontSize(14).Bold().UnderlineStyle(Xceed.Document.NET.UnderlineStyle.singleLine);

                    paragrafo = doc.InsertParagraph();
                    paragrafo.Append($"Endereço: {dadosEquipe.GetValue("ENDEREÇO")} - {dadosEquipe.GetValue("CEP")}").Font("Times New Roman").FontSize(14);

                    paragrafo = doc.InsertParagraph();
                    paragrafo.Append($"O.S: {dadosEquipe.GetValue("OS")}  -  ").Font("Times New Roman").FontSize(14);
                    paragrafo.Append($"TIPO O.S: {dadosEquipe.GetValue("TIPO OS")}").Font("Times New Roman").FontSize(14).Color(Color.White).Highlight(Xceed.Document.NET.Highlight.red);

                    foreach (var rota in word.Rotas)
                    {
                        paragrafo = doc.InsertParagraph();
                        paragrafo.Append($"{rota[0] + rota[1..].ToLower()}: {dadosEquipe.GetValue(rota)}").Font("Times New Roman").FontSize(14);
                    }
                }

                doc.Save();

                return File(stream.ToArray(), "application/octet-stream", "Rotas.docx");
            }
            catch
            {
                return RedirectToAction(nameof(Export));
            }
        }
    }
}