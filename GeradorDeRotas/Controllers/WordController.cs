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
using Models;
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

            if (excel == null)
            {
                TempData["necessarioExportar"] = "Falha!";
                ViewBag.Rotas = new SelectList(new List<string>());
                ViewBag.Servicos = new SelectList(new List<string>());
                ViewBag.Cidades = new SelectList(new List<string>());
                ViewBag.Equipes = new MultiSelectList(new List<string>(), "Id", "NomeEquipe");
                return View();
            }

            foreach (var item in excel.ArquivosExcel.First())
                rotas.Add(item.Name);

            rotas.RemoveAll(x => x == "CONTRATO" || x == "ASSINANTE" || x == "ENDEREÇO" || x == "CEP" || x == "OS" || x == "TIPO OS");

            ViewBag.Rotas = new SelectList(rotas);
            ViewBag.Servicos = new SelectList(_excelService.GetServicos());
            ViewBag.Cidades = new SelectList(new List<string>());
            ViewBag.Equipes = new MultiSelectList(new List<string>(), "Id", "NomeEquipe");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Export(Word word)
        {
            try
            {
                if (word.Servico == null)
                {
                    TempData["semServico"] = "Nenhum serviço selecionado.";
                    return RedirectToAction(nameof(Export));
                }

                var stream = new MemoryStream();
                var doc = DocX.Create(stream);

                var paragrafo = doc.InsertParagraph();
                paragrafo.Append($"ROTA TRABALHO - {DateTime.Now:d}").Font("Times New Roman").FontSize(18).Bold().Alignment = Xceed.Document.NET.Alignment.center;

                paragrafo = doc.InsertParagraph();
                paragrafo.Append("RETORNOS").Font("Times New Roman").FontSize(15).Bold().Alignment = Xceed.Document.NET.Alignment.center;


                var excel = _excelService.Get();
                excel.ArquivosExcel.RemoveAll(x => x.GetValue("SERVIÇO") != word.Servico);

                var equipes = new List<Equipe>();

                foreach (var equipeId in word.EquipeIds)
                    equipes.Add(await _equipeService.Get(equipeId));

                var dadosEquipe = excel.ArquivosExcel.FindAll(x => x.GetValue("CIDADE") == word.Cidade).OrderBy(x => x.GetValue("CEP"));

                var qtdServicosEquipe = Math.Floor((double)dadosEquipe.Count() / equipes.Count);
                var resto = dadosEquipe.Count() % equipes.Count;

                var indice = 0;
                var indiceEquipe = 0;

                foreach (var dadoEquipe in dadosEquipe)
                {
                    if (qtdServicosEquipe <= 0 || (resto > 0 && indice == qtdServicosEquipe + 1) || (resto == 0 && indice == qtdServicosEquipe))
                    {
                        indiceEquipe++;
                        indice = 0;
                    }

                    var equipe = equipes[indiceEquipe];

                    if (indice == 0)
                    {
                        paragrafo = doc.InsertParagraph();
                        paragrafo = doc.InsertParagraph();
                        paragrafo = doc.InsertParagraph();
                        paragrafo = doc.InsertParagraph();
                        paragrafo.Append($"Nome da Equipe: {equipe.NomeEquipe}").Font("Times New Roman").FontSize(15).Bold();
                    }

                    paragrafo = doc.InsertParagraph();
                    paragrafo = doc.InsertParagraph();
                    dadoEquipe.TryGetValue("CONTRATO", out var contrato);
                    dadoEquipe.TryGetValue("ASSINANTE", out var assinante);
                    dadoEquipe.TryGetValue("PERÍODO", out var periodo);
                    paragrafo.Append($"Contrato: {contrato}  -  Assinante: {assinante}  -  Período: {periodo}").Font("Times New Roman").FontSize(14).Bold().UnderlineStyle(Xceed.Document.NET.UnderlineStyle.singleLine);

                    paragrafo = doc.InsertParagraph();
                    paragrafo.Append($"Endereço: {dadoEquipe.GetValue("ENDEREÇO")} - {dadoEquipe.GetValue("CEP")}").Font("Times New Roman").FontSize(14);

                    paragrafo = doc.InsertParagraph();
                    paragrafo.Append($"O.S: {dadoEquipe.GetValue("OS")}  -  ").Font("Times New Roman").FontSize(14);
                    paragrafo.Append($"TIPO O.S: {dadoEquipe.GetValue("TIPO OS")}").Font("Times New Roman").FontSize(14).Color(Color.White).Highlight(Xceed.Document.NET.Highlight.red);

                    foreach (var rota in word.Rotas)
                    {
                        paragrafo = doc.InsertParagraph();
                        paragrafo.Append($"{rota[0] + rota[1..].ToLower()}: {dadoEquipe.GetValue(rota)}").Font("Times New Roman").FontSize(14);
                    }

                    indice++;
                }

                doc.Save();

                TempData["exportarSucesso"] = "Sucesso!";

                return File(stream.ToArray(), "application/octet-stream", "Rotas.docx");
            }
            catch
            {
                return RedirectToAction(nameof(Export));
            }
        }
    }
}