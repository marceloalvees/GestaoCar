using Application.Dto;
using Application.Interfaces;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace Application.Services 
{
    public class ExcelService : IExcelService
    {
        public async Task<byte[]> GenerateMonthlyReportInExcelAsync(IEnumerable<SaleDto> vendas)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Relatório");

            // Título do relatório
            ws.Cell("B1").Value = "GestãoCar - Relatório Mensal";
            ws.Cell("B1").Style.Font.FontSize = 18;
            ws.Cell("B1").Style.Font.Bold = true;
            ws.Range("B1:F2").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Font.SetFontColor(XLColor.FromHtml("#004e64"));

            int row = 4;

            // 1. Total de vendas
            ws.Cell(row, 1).Value = "Total de vendas";
            ws.Cell(row, 2).Value = vendas.Count();
            ws.Row(row).Style.Font.Bold = true;
            row += 2;

            // 2. Vendas por tipo de veículo
            ws.Cell(row, 1).Value = "Vendas por tipo de veículo";
            ws.Row(row).Style.Font.Bold = true;
            row++;
            var porTipo = vendas.GroupBy(v => v.Type)
                .Select(g => new { Tipo = g.Key, Total = g.Count() });
            foreach (var tipo in porTipo)
            {
                ws.Cell(row, 1).Value = tipo.Tipo;
                ws.Cell(row, 2).Value = tipo.Total;
                row++;
            }
            row++;


            // 3. Desempenho de cada concessionária
            ws.Cell(row, 1).Value = "Desempenho de cada concessionária";
            ws.Row(row).Style.Font.Bold = true;
            row++;
            ws.Cell(row, 1).Value = "Concessionária";
            ws.Cell(row, 2).Value = "Nº de vendas";
            ws.Cell(row, 3).Value = "Valor total";
            ws.Range(row, 1, row, 3).Style.Font.Bold = true;
            row++;
            var porConcessionaria = vendas.GroupBy(v => v.DealershipName)
                .Select(g => new { Concessionaria = g.Key, Total = g.Count(), Valor = g.Sum(v => v.SalePrice) });
            foreach (var c in porConcessionaria)
            {
                ws.Cell(row, 1).Value = c.Concessionaria;
                ws.Cell(row, 2).Value = c.Total;
                ws.Cell(row, 3).Value = c.Valor;
                row++;
            }

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return ms.ToArray();
        }

        public async Task<byte[]> GenerateMonthlyReportInPdfAsync(IEnumerable<SaleDto> vendas)
        {
            // Logo em SVG Base64 (pode-se usar PNG também)
            var logoSvgBase64 = "PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIzMiIgaGVpZ2h0PSIzMiIgZmlsbD0iIzAwNGU2NCIgY2xhc3M9ImJpIGJpLWNhci1mcm9udCIgdmlld0JveD0iMCAwIDE2IDE2Ij4KCTxwYXRoIGQ9Ik00IDlhMSAxIDAgMSAwIDAgMiAxIDEgMCAwIDAgMC0yem04IDFhMSAxIDAgMSAxLTIgMCAxIDEgMCAwIDEgMiAweiIvPgoJPHBhdGggZD0iTTIgN1Y2YTQgNCAwIDAgMSA0LTRoNGE0IDQgMCAwIDEgNCA0djFsLjQ0NyAxLjM0MkEyIDIgMCAwIDEgMTYgMTB2MmEyIDIgMCAwIDEtMiAydi0xYTEgMSAwIDAgMS0yIDB2LTFINHYxYTEgMSAwIDAgMS0yIDB2LTEhMiAyIDAgMCAxLTItMnYtMmMwLS41MzguMjE0LTEuMDU1LjU1My0xLjQ1OEwyIDd6bTQuNS0xQTIuNSAyLjUgMCAwIDEgNiAzLjVoNEEyLjUgMi41IDAgMCAxIDEyLjUgNnYxaC05VjZ6bS0xLjQ0NyAzLjM0MkExIDEgMCAwIDAgMSAxMHYyYTEgMSAwIDAgMCAxIDFoMTJhMSAxIDAgMCAwIDEtMXYtMmExIDEgMCAwIDAtLjA1My0uMzE2TDEzLjUgOC41aC0xMWwtLjQ0NyAxLjM0MnoiLz4KPC9zdmc+";

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Header().Row(row =>
                    {
                        // Logo
                        //row.RelativeColumn(0.15f).Height(50).AlignMiddle().AlignLeft().Image(ImageData.FromSvg(logoSvgBase64));
                        // Título
                        row.RelativeColumn().AlignMiddle().AlignCenter().Text("GestãoCar - Relatório Mensal")
                            .FontSize(22).Bold().FontColor("#004e64");
                    });

                    page.Content().Column(column =>
                    {
                        int totalVendas = vendas.Count();
                        column.Item().PaddingVertical(10).Text($"Total de vendas: {totalVendas}").FontSize(13).Bold();

                        // Vendas por tipo de veículo
                        column.Item().PaddingTop(20).Text("Vendas por tipo de veículo").FontSize(15).Bold().FontColor("#004e64");
                        var porTipo = vendas.GroupBy(v => v.Type)
                                            .Select(g => new { Tipo = g.Key, Total = g.Count() }).ToList();
                        column.Item().Table(tabela =>
                        {
                            tabela.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });
                            tabela.Header(header =>
                            {
                                header.Cell().Text("Tipo de veículo").Bold();
                                header.Cell().Text("Total").Bold();
                            });
                            foreach (var tipo in porTipo)
                            {
                                tabela.Cell().Text(tipo.Tipo ?? "-");
                                tabela.Cell().Text(tipo.Total.ToString());
                            }
                        });

                        // Desempenho de cada concessionária
                        column.Item().PaddingTop(20).Text("Desempenho de cada concessionária").FontSize(15).Bold().FontColor("#004e64");
                        var porConcessionaria = vendas.GroupBy(v => v.DealershipName)
                                                .Select(g => new {
                                                    Concessionaria = g.Key,
                                                    Total = g.Count(),
                                                    Valor = g.Sum(v => v.SalePrice)
                                                }).ToList();
                        column.Item().Table(tabela =>
                        {
                            tabela.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });
                            tabela.Header(header =>
                            {
                                header.Cell().Text("Concessionária").Bold();
                                header.Cell().Text("Nº de vendas").Bold();
                                header.Cell().Text("Valor total").Bold();
                            });
                            foreach (var c in porConcessionaria)
                            {
                                tabela.Cell().Text(c.Concessionaria ?? "-");
                                tabela.Cell().Text(c.Total.ToString());
                                tabela.Cell().Text($"R$ {c.Valor:N2}");
                            }
                        });
                    });
                });
            });
            QuestPDF.Settings.License = LicenseType.Community;
            using var ms = new MemoryStream();
            pdf.GeneratePdf(ms);
            return ms.ToArray();
        }
    }
}
