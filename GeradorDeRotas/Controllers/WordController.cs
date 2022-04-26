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
        private readonly EquipeService _equipeService;
        private readonly ExcelService _excelService;
        public WordController(
            EquipeService equipeService,
            ExcelService excelService)
        {
            _equipeService = equipeService;
            _excelService = excelService;
        }

        [Authorize]
        public ActionResult Export()
        {
            var rotas = new List<string>();

            var excel = _excelService.Get();

            foreach (var item in excel.ArquivosExcel.First())
                rotas.Add(item.Name);

            rotas.RemoveAll(x => x == "CONTRATO" || x == "ASSINANTE" || x == "ENDEREÇO" || x == "CEP" || x == "OS" || x == "TIPO OS");

            ViewBag.Rotas = new SelectList(rotas);
            ViewBag.Servicos = new SelectList(_excelService.GetServicos());
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

                var equipes = await _equipeService.GetDisponivel();

                if (word.Cidade != null)
                    equipes = equipes.FindAll(x => x.Cidade == word.Cidade);

                if ( !equipes.Any())
                {
                    TempData["semEquipe"] = "Nenhuma equipe encontrada com este filtro!";
                    return RedirectToAction(nameof(Export));
                }

                var excel = _excelService.Get();

                excel.ArquivosExcel.RemoveAll(x => x.GetValue("SERVIÇO") != word.Servico);

                foreach (var equipe in equipes)
                {
                    var dadosEquipe = excel.ArquivosExcel.Find(x => x.GetValue("CIDADE") == equipe.Cidade);

                    paragrafo = doc.InsertParagraph();
                    paragrafo = doc.InsertParagraph();
                    paragrafo = doc.InsertParagraph();
                    paragrafo = doc.InsertParagraph();
                    paragrafo.Append($"Nome da Equipe: {equipe.NomeEquipe}").Font("Times New Roman").FontSize(15).Bold();

					dadosEquipe.TryGetValue("CONTRATO", out var contrato);
					dadosEquipe.TryGetValue("ASSINANTE", out var assinante);
					dadosEquipe.TryGetValue("PERÍODO", out var periodo);
					paragrafo = doc.InsertParagraph();
                    paragrafo = doc.InsertParagraph();
                    paragrafo.Append($"Contrato: {contrato}  -  Assinante: {assinante}  -  Período: {periodo}").Font("Times New Roman").FontSize(14).Bold().UnderlineStyle(Xceed.Document.NET.UnderlineStyle.singleLine);

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

                    equipe.Servicos.Add(word.Servico);
                    equipe.IncrementarContagemDeServico();
                    await _equipeService.Update(equipe.Id, equipe);
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