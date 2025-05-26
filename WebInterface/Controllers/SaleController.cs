using System.Security.Claims;
using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebInterface.Models;

namespace WebInterface.Controllers
{
    /// <summary>
    /// Controlador responsável pelas operações de vendas.
    /// </summary>
    [Authorize] //Todos terão acesso ao ambiente de vendas
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IVehicleService _vehicleService;
        private readonly IDealershipService _dealershipService;

        public SaleController(ISaleService saleService, IVehicleService vehicleService, IDealershipService dealershipService)
        {
            _saleService = saleService;
            _vehicleService = vehicleService;
            _dealershipService = dealershipService;
        }

        /// <summary>
        /// Exibe a página principal de vendas com listas de veículos, concessionárias e vendas.
        /// </summary>
        /// <param name="cancelation">Token de cancelamento.</param>
        /// <returns>View com dados de vendas.</returns>
        public async Task<IActionResult> Index(CancellationToken cancelation)
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync(cancelation);
            var vehicleList = vehicles?.Success == true
                ? vehicles.Data.Select(ToViewModel).ToList()
                : new List<VehicleViewModel>();

            var dealerships = await _dealershipService.GetAllDealershipsAsync(cancelation);
            var dealershipList = dealerships?.Success == true
                ? dealerships.Data.Select(ToViewModel).ToList()
                : new List<DealershipViewModel>();

            var sales = await _saleService.GetAllSalesAsync(cancelation);
            var saleList = sales?.Success == true
                ? sales.Data.Select(ToViewModel).ToList()
                : new List<SaleViewModel>();
            if (vehicles?.Success != true)
                TempData["Error"] = "Erro ao carregar veículos.";

            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            ViewData["Role"] = role;
            ViewData["Vehicles"] = vehicleList;
            ViewData["Dealerships"] = dealershipList;
            ViewData["Sales"] = saleList;
            return View();
        }

        /// <summary>
        /// Cria uma nova venda.
        /// </summary>
        /// <param name="model">Dados da venda.</param>
        /// <param name="cancellation">Token de cancelamento.</param>
        /// <returns>Redireciona para a página principal de vendas.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateSaleViewModel model, CancellationToken cancellation)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Preencha todos os campos obrigatórios corretamente.";
                return RedirectToAction(nameof(Index));
            }
            var saleDto = ToDto(model);
            var result = await _saleService.AddSaleAsync(saleDto, cancellation);
            if (result.Success)
            {
                TempData["Success"] = "Venda criada com sucesso.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Obtém uma venda pelo seu ID.
        /// </summary>
        /// <param name="id">ID da venda.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Dados da venda ou NotFound.</returns>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var sale = await _saleService.GetSaleByIdAsync(id, cancellationToken);
            if (sale?.Success == true)
            {
                var viewModel = ToViewModel(sale.Data);
                return Ok(viewModel);
            }
            return NotFound();
        }

        /// <summary>
        /// Atualiza uma venda existente.
        /// </summary>
        /// <param name="id">ID da venda.</param>
        /// <param name="model">Dados atualizados da venda.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Redireciona para a página principal de vendas.</returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] CreateSaleViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Preencha todos os campos obrigatórios corretamente.";
                return RedirectToAction(nameof(Index));
            }
            var saleDto = ToDto(model);
            saleDto.Id = id;
            var result = await _saleService.UpdateSaleAsync(saleDto, cancellationToken);
            if (result.Success)
            {
                TempData["Success"] = "Venda atualizada com sucesso.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Exclui uma venda pelo ID.
        /// </summary>
        /// <param name="id">ID da venda.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Redireciona para a página principal de vendas.</returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _saleService.DeleteSaleAsync(id, cancellationToken);
            if (result.Success)
            {
                TempData["Success"] = "Venda excluída com sucesso.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Verifica se um cliente existe pelo CPF.
        /// </summary>
        /// <param name="cpf">CPF do cliente.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Retorna true se o cliente existe, caso contrário NotFound.</returns>
        [HttpGet("exist")]
        public async Task<IActionResult> ClientExist(string cpf, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return BadRequest("CPF não pode ser vazio.");
            }
            var result = await _saleService.ClientExistAsync(cpf, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Message);
        }

        private SaleDto ToDto(CreateSaleViewModel model)
        {
            return new SaleDto
            {
                VehicleName = model.VehicleName,
                DealershipName = model.DealershipName,
                SaleDate = model.SaleDate,
                SalePrice = model.SalePrice,
                CustomerName = model.CustomerName,
                CustomerCpf = model.CustomerCPF,
                CustomerPhone = model.CustomerPhone,
            };
        }

        private VehicleViewModel ToViewModel(VehicleDto vehicle)
        {
            return new VehicleViewModel
            {
                Model = vehicle.Model,
                ManufacturerName = vehicle.ManufacturerName,
                ManuFacturingYear = vehicle.ManuFacturingYear,
                Price = vehicle.Price,
                Type = vehicle.Type,
                Description = vehicle.Description
            };
        }
        private SaleViewModel ToViewModel(SaleDto sale)
        {
            return new SaleViewModel
            {
                VehicleModel = sale.VehicleName,
                DealershipName = sale.DealershipName,
                SaleDate = sale.SaleDate,
                SalePrice = sale.SalePrice,
                CustomerName = sale.CustomerName,
            };
        }
        private DealershipViewModel ToViewModel(DealershipDto dealership)
        {
            return new DealershipViewModel
            {
                Name = dealership.Name,
                Address = dealership.Address,
                City = dealership.City,
                State = dealership.State,
                ZipCode = dealership.ZipCode
            };
        }
    }
}
