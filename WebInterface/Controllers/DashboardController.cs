using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebInterface.Controllers
{
    /// <summary>
    /// Controlador responsável pelo dashboard de vendas.
    /// Apenas usuários com o papel "Gerente" têm acesso.
    /// </summary>
    [Authorize(Roles = "Gerente")]
    [Route("dashboard")]
    public class DashboardController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IExcelService _reportService;

        public DashboardController(ISaleService saleService, IExcelService reportService)
        {
            _saleService = saleService;
            _reportService = reportService;
        }

        /// <summary>
        /// Exibe a página inicial do dashboard.
        /// </summary>
        /// <returns>View do dashboard.</returns>
        public ActionResult Index()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            ViewData["Role"] = role;
            return View();
        }

        /// <summary>
        /// Retorna as vendas do mês/ano informado, podendo gerar relatório em Excel ou PDF.
        /// </summary>
        /// <param name="month">Mês das vendas.</param>
        /// <param name="year">Ano das vendas.</param>
        /// <param name="format">Formato do relatório: "excel", "pdf" ou vazio para exibir na view.</param>
        /// <returns>Arquivo de relatório ou view com dados das vendas.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Month(int month, int year, string format)
        {
            var sales = await _saleService.GetSaleByMonthAndYearAsync(month, year, CancellationToken.None);

            if (format == "excel")
            {
                var excel = await _reportService.GenerateMonthlyReportInExcelAsync(sales.Data);
                return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"monthly-report-{month:D2}-{year}.xlsx");
            }
            else if (format == "pdf")
            {
                var pdf = await _reportService.GenerateMonthlyReportInPdfAsync(sales.Data);
                return File(pdf, "application/pdf", $"monthly-report-{month:D2}-{year}.pdf");
            }
            return View(sales.Data);
        }

        /// <summary>
        /// Retorna dados agregados para gráficos de vendas do mês/ano informado.
        /// </summary>
        /// <param name="month">Mês das vendas.</param>
        /// <param name="year">Ano das vendas.</param>
        /// <returns>JSON com tipos de veículos e concessionárias.</returns>
        [HttpPost("grafic")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MonthGrafic(int month, int year)
        {
            var sales = await _saleService.GetSaleByMonthAndYearAsync(month, year, CancellationToken.None);

            var tiposVeiculo = sales.Data
                .GroupBy(x => x.Type)
                .Select(g => new { tipo = g.Key, total = g.Count() })
                .ToList();

            var concessionarias = sales.Data
                .GroupBy(x => x.DealershipName)
                .Select(g => new { nome = g.Key, quantidade = g.Count() })
                .ToList();

            return Json(new
            {
                tiposVeiculo,
                concessionarias
            });
        }
    }
}
