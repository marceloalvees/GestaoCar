using System.Security.Claims;
using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebInterface.Models;

namespace WebInterface.Controllers
{
    /// <summary>
    /// Controlador responsável pelas operações de Concessionária.
    /// </summary>
    [Authorize(Roles = "Gerente,Administrador")]
    [Route("dealership")]
    public class DealershipController : Controller
    {
        private readonly IDealershipService _dealershipService;
        private readonly ICepService _cepService;

        public DealershipController(IDealershipService dealershipService, ICepService cepService)
        {
            _dealershipService = dealershipService;
            _cepService = cepService;
        }

        /// <summary>
        /// Exibe a lista de concessionárias.
        /// </summary>
        /// <returns>View com a lista de concessionárias.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            var dealerships = _dealershipService.GetAllDealershipsAsync(CancellationToken.None).Result;
            var dealershipList = dealerships?.Success == true
                ? dealerships.Data.Select(ToViewModel).ToList()
                : new List<DealershipViewModel>();
            if (dealerships?.Success != true)
                TempData["Error"] = "Erro ao carregar concessionárias.";

            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            ViewData["Role"] = role;
            ViewData["Dealerships"] = dealershipList;
            return View();
        }

        /// <summary>
        /// Obtém uma concessionária pelo ID.
        /// </summary>
        /// <param name="id">ID da concessionária.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Dados da concessionária ou NotFound.</returns>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var dealership = await _dealershipService.GetDealershipByIdAsync(id, cancellationToken);
            if (dealership?.Success == true)
            {
                var viewModel = ToViewModel(dealership.Data);
                return Ok(viewModel);
            }
            return NotFound();
        }

        /// <summary>
        /// Cria uma nova concessionária.
        /// </summary>
        /// <param name="model">Dados da concessionária.</param>
        /// <param name="cancellation">Token de cancelamento.</param>
        /// <returns>Redireciona para Index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] DealershipViewModel model, CancellationToken cancellation)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Preencha todos os campos obrigatórios corretamente.";
                return RedirectToAction(nameof(Index));
            }
            var dealershipDto = ToDto(model);
            var result = await _dealershipService.AddDealershipAsync(dealershipDto, cancellation);
            if (result.Success)
            {
                TempData["Success"] = "Concessionária criada com sucesso.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Busca endereço pelo CEP informado.
        /// </summary>
        /// <param name="cep">CEP no formato 00000-000.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Endereço encontrado ou NotFound.</returns>
        [HttpPost("cep")]
        public async Task<IActionResult> GetAddressByCep(string cep, CancellationToken cancellationToken)
        {
            var address = await _cepService.SearchZipCodeAsync(cep, cancellationToken);
            if (address?.Success == true)
            {
                return Ok(address);
            }
            return NotFound();
        }

        /// <summary>
        /// Atualiza uma concessionária existente.
        /// </summary>
        /// <param name="id">ID da concessionária.</param>
        /// <param name="model">Dados atualizados.</param>
        /// <param name="cancellation">Token de cancelamento.</param>
        /// <returns>Redireciona para Index.</returns>
        [HttpPut("update/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [FromForm] DealershipViewModel model, CancellationToken cancellation)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Preencha todos os campos obrigatórios corretamente.";
                return RedirectToAction(nameof(Index));
            }
            var dealershipDto = ToDto(model);
            var result = await _dealershipService.UpdateDealershipAsync(dealershipDto, cancellation);
            if (result.Success)
            {
                TempData["Success"] = "Concessionária atualizada com sucesso.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Exclui uma concessionária.
        /// </summary>
        /// <param name="id">ID da concessionária.</param>
        /// <param name="cancellation">Token de cancelamento.</param>
        /// <returns>Redireciona para Index.</returns>
        [HttpDelete("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellation)
        {
            var result = await _dealershipService.DeleteDealershipAsync(id, cancellation);
            if (result.Success)
            {
                TempData["Success"] = "Concessionária excluída com sucesso.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction("Index");
        }

        public DealershipViewModel ToViewModel(DealershipDto dto)
        {
            return new DealershipViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
                Phone = dto.Phone,
                Email = dto.Email,
                MaxCapacity = dto.MaxVehicleCapacity
            };
        }
        public DealershipDto ToDto(DealershipViewModel model)
        {
            return new DealershipDto
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                Phone = model.Phone,
                Email = model.Email,
                MaxVehicleCapacity = model.MaxCapacity
            };

        }
    }
}
