using System;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using LeitorDeArquivoExcel.Services;
using LeitorDeArquivoExcel.Utils;
using Models;

namespace LeitorDeArquivoExcel
{
    internal class Program
    {
        static void Main()
        {
            var xls = new XLWorkbook(Path.Combine(Directory.GetCurrentDirectory(), "Gerador de rotas.xlsx"));
            var planilha = xls.Worksheets.First(w => w.Name == "Planilha1");
            var totalLinhas = planilha.Rows().Count();

            var excel = new Excel();
            var linha = 2;

            while (planilha.Cell($"J{linha}").Value.ToString() != "")
             {
                DateTime? data = null, horario = null;

                var dataValida = DateTime.TryParse(planilha.Cell($"A{linha}").Value.ToString(), out var dataParse);
                var horarioValido = DateTime.TryParse(planilha.Cell($"U{linha}").Value.ToString(), out var horarioParse);

                if (dataValida)
                    data = dataParse;
                if (horarioValido)
                    horario = horarioParse;

                excel.ArquivosExcel.Add(new ArquivoExcel(
                    data,
                    planilha.Cell($"B{linha}").Value.ToString(),
                    planilha.Cell($"C{linha}").Value.ToString(),
                    planilha.Cell($"D{linha}").Value.ToString(),
                    planilha.Cell($"E{linha}").Value.ToString(),
                    planilha.Cell($"F{linha}").Value.ToString(),
                    planilha.Cell($"G{linha}").Value.ToString(),
                    planilha.Cell($"H{linha}").Value.ToString(),
                    planilha.Cell($"I{linha}").Value.ToString(),
                    int.Parse(planilha.Cell($"J{linha}").Value.ToString()),
                    planilha.Cell($"K{linha}").Value.ToString(),
                    planilha.Cell($"L{linha}").Value.ToString(),
                    planilha.Cell($"M{linha}").Value.ToString(),
                    planilha.Cell($"N{linha}").Value.ToString(),
                    planilha.Cell($"O{linha}").Value.ToString(),
                    planilha.Cell($"P{linha}").Value.ToString(),
                    planilha.Cell($"Q{linha}").Value.ToString(),
                    planilha.Cell($"R{linha}").Value.ToString(),
                    planilha.Cell($"S{linha}").Value.ToString(),
                    planilha.Cell($"T{linha}").Value.ToString(),
                    horario,
                    planilha.Cell($"V{linha}").Value.ToString(),
                    planilha.Cell($"W{linha}").Value.ToString(),
                    planilha.Cell($"X{linha}").Value.ToString(),
                    planilha.Cell($"Y{linha}").Value.ToString(),
                    planilha.Cell($"Z{linha}").Value.ToString(),
                    planilha.Cell($"AA{linha}").Value.ToString(),
                    int.Parse(planilha.Cell($"AB{linha}").Value.ToString()),
                    planilha.Cell($"AC{linha}").Value.ToString(),
                    planilha.Cell($"AD{linha}").Value.ToString(),
                    planilha.Cell($"AE{linha}").Value.ToString(),
                    planilha.Cell($"AF{linha}").Value.ToString(),
                    planilha.Cell($"AG{linha}").Value.ToString(),
                    planilha.Cell($"AH{linha}").Value.ToString(),
                    planilha.Cell($"AI{linha}").Value.ToString(),
                    planilha.Cell($"AJ{linha}").Value.ToString(),
                    planilha.Cell($"AK{linha}").Value.ToString(),
                    planilha.Cell($"AL{linha}").Value.ToString(),
                    planilha.Cell($"AM{linha}").Value.ToString()));

                linha++;
            }

            var excelService = new ExcelService(new MongoSettings());
            excelService.Remove();
            excelService.Create(excel);
        }
    }
}