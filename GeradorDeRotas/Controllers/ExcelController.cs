using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using GeradorDeRotas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace GeradorDeRotas.Controllers
{
    public class ExcelController : Controller
    {
        private readonly ExcelService _excelService;
        private readonly string[] _extensoesPermitidas = { ".xlsx", ".xls" };
        public ExcelController(ExcelService testeService)
        {
            _excelService = testeService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Index()
        {
            var files = HttpContext.Request.Form.Files;

            if (!files.Any() || !_extensoesPermitidas.Contains(Path.GetExtension(files[0].FileName).ToLowerInvariant()))
            {
                TempData["arquivoInvalido"] = "Nenhum arquivo selecionado!";
                return RedirectToAction(nameof(Upload));
            }

            var xls = new XLWorkbook(files[0].OpenReadStream());
            var planilha = xls.Worksheets.First();

            var columns = new List<string>();
            var qtdColumns = planilha.Columns().Count();

            var dt = new DataTable();

            for (int i = 1; i < qtdColumns; i++)
            {
                var col = planilha.Column(i).FirstCell().Value.ToString();
                dt.Columns.Add(col);
                columns.Add(col);
            }

            var linha = 2;
            var continuar = true;

            while (continuar)
            {
                var d = new string[columns.Count()];
                int index = 0;
                for (int i = 1; i <= columns.Count(); i++)
                {
                    d[index] = planilha.Column(i).Cell(linha).Value.ToString();
                    index++;
                }

                continuar = d.Any(x => !string.IsNullOrEmpty(x));

                if (continuar) dt.Rows.Add(d);

                linha++;
            }


            dt.DefaultView.Sort = "cep ASC";
            dt = dt.DefaultView.ToTable();

            var excel = new Excel();

            for (int i = 1; i < dt.Rows.Count - 1; i++)
            {
                var dictionary = new Dictionary<string, string>();
                int j = 0;
                foreach (var item in columns)
                {
                    dictionary.Add(item, dt.Rows[i][j].ToString());
                    j++;
                }
                var json = JsonConvert.SerializeObject(dictionary);
                var document = BsonSerializer.Deserialize<BsonDocument>(json);
                excel.ArquivosExcel.Add(document);
            }

            _excelService.Remove();
            _excelService.Create(excel);

            TempData["arquivoSucesso"] = "Sucesso";

            return RedirectToAction(nameof(Upload));
        }

        [Authorize]
        public IActionResult Upload()
        {
            return View();
        }
    }
}