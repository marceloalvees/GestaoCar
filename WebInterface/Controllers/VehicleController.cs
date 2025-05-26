using System.Security.Claims;
using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebInterface.Models;

namespace WebInterface.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de veículos.
    /// Apenas usuários com o papel "Gerente" têm acesso.
    /// </summary>
    [Authorize(Roles = "Gerente, Administrador")]
    [Route("vehicle")]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IManufacturerService _manufacturerService;

        /// <summary>
        /// Construtor do VehicleController.
        /// </summary>
        /// <param name="vehicleService">Serviço de veículos.</param>
        /// <param name="manufacturerService">Serviço de fabricantes.</param>
        public VehicleController(IVehicleService vehicleService, IManufacturerService manufacturerService)
        {
            _vehicleService = vehicleService;
            _manufacturerService = manufacturerService;
        }

        /// <summary>
        /// Exibe a lista de veículos e fabricantes.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>View com os dados de veículos e fabricantes.</returns>
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync(cancellationToken);
            var vehicleList = vehicles?.Success == true
                ? vehicles.Data.Select(ToViewModel).ToList()
                : new List<VehicleViewModel>();
            if (vehicles?.Success != true)
                TempData["Error"] = "Erro ao carregar veículos.";
            var manufacturers = await _manufacturerService.GetAllManufacturersAsync(cancellationToken);
            var manufacturerList = manufacturers?.Success == true
                ? manufacturers.Data.Select(ToViewModel).ToList()
                : new List<ManufacturerViewModel>();

            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            ViewData["Role"] = role;
            ViewData["Manufacturers"] = manufacturerList;
            ViewData["Vehicles"] = vehicleList;
            return View();
        }

        /// <summary>
        /// Cria um novo veículo.
        /// </summary>
        /// <param name="vehicleModel">Dados do veículo.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Redireciona para a Index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] VehicleViewModel vehicleModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Preencha todos os campos obrigatórios corretamente.";
                return RedirectToAction(nameof(Index));
            }
            var vehicleDto = ToDto(vehicleModel);
            var result = await _vehicleService.AddVehicleAsync(vehicleDto, cancellationToken);
            if (result.Success)
            {
                TempData["Success"] = "Veículo criado com sucesso.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Obtém um veículo pelo ID.
        /// </summary>
        /// <param name="id">ID do veículo.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Veículo encontrado ou NotFound.</returns>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var vehicleResult = await _vehicleService.GetVehicleByIdAsync(id, cancellationToken);
            if (vehicleResult.Success)
            {
                var vehicleViewModel = ToViewModel(vehicleResult.Data);
                return Ok(vehicleViewModel);
            }
            return NotFound(vehicleResult.Message);
        }

        /// <summary>
        /// Atualiza um veículo existente.
        /// </summary>
        /// <param name="vehicleModel">Dados do veículo.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Redireciona para a Index.</returns>
        [HttpPut]
        public async Task<IActionResult> Update(VehicleViewModel vehicleModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Preencha todos os campos obrigatórios corretamente.";
                return RedirectToAction(nameof(Index));
            }
            var vehicleDto = ToDto(vehicleModel);
            var result = await _vehicleService.UpdateVehicleAsync(vehicleDto, cancellationToken);
            if (result.Success)
            {
                TempData["Success"] = "Veículo atualizado com sucesso.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Deleta um veículo pelo ID.
        /// </summary>
        /// <param name="id">ID do veículo.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Redireciona para a Index.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _vehicleService.DeleteVehicleAsync(id, cancellationToken);
            if (result.Success)
            {
                TempData["Success"] = "Veículo deletado com sucesso.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction("Index");
        }
        #region Private Methods
        private VehicleViewModel ToViewModel(VehicleDto vehicle)
        {
            return new VehicleViewModel
            {
                Model = vehicle.Model,
                ManufacturerName = vehicle.ManufacturerName,
                ManuFacturingYear = vehicle.ManuFacturingYear,
                Price = vehicle.Price,
                Type = GetType(vehicle.Type),
                Description = vehicle.Description
            };
        }
        private VehicleDto ToDto(VehicleViewModel vehicleModel)
        {
            return new VehicleDto
            {
                Model = vehicleModel.Model,
                ManufacturerName = vehicleModel.ManufacturerName,
                ManuFacturingYear = vehicleModel.ManuFacturingYear.GetValueOrDefault(),
                Price = vehicleModel.Price.GetValueOrDefault(),
                Type = vehicleModel.Type,
                Description = vehicleModel.Description
            };
        }
        private static ManufacturerViewModel ToViewModel(ManufacturerDto dto) =>
        new ManufacturerViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Country = dto.Country,
            FoundationYear = dto.FoundationYear,
            Website = dto.Website
        };

        private static string GetType(string type)
        {
            return type.ToLowerInvariant() switch
            {
                "car" => "Carro",
                "truck" => "Caminhão",
                _ => "Moto"
            };
        }
        #endregion
    }
}
