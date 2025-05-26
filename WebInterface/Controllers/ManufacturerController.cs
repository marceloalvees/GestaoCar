using System.Security.Claims;
using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebInterface.Models;

namespace WebInterface.Controllers
{
    [Authorize(Roles = "Gerente,Administrador")]
    [Route("manufacturer")]
    public class ManufacturerController : Controller
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }
        /// <summary>
        /// Exibe a página principal de fabricantes.
        /// </summary>
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var manufacturers = await _manufacturerService.GetAllManufacturersAsync(cancellationToken);
            var manufacturerList = manufacturers?.Success == true
                ? manufacturers.Data.Select(ToViewModel).ToList()
                : new List<ManufacturerViewModel>();

            if (manufacturers?.Success != true)
                TempData["Error"] = "Erro ao carregar fabricantes.";

            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            ViewData["Role"] = role;
            ViewData["Manufacturers"] = manufacturerList;
            return View();
        }

        #region Endpoints
        /// <summary>
        /// Retorna a lista de fabricantes.
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetManufacturers(CancellationToken cancellationToken)
        {
            var manufacturers = await _manufacturerService.GetAllManufacturersAsync(cancellationToken);
            if (manufacturers?.Success == true)
            {
                var viewModels = manufacturers.Data.Select(ToViewModel).ToList();
                return Ok(viewModels);
            }
            return NotFound();
        }

        /// <summary>
        /// Cria um novo fabricante.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ManufacturerViewModel model, CancellationToken cancellation)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Preencha todos os campos obrigatórios corretamente.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Fabricante já cadastrado com esse nome.";
            var addResult = await _manufacturerService.AddManufacturerAsync(ToDto(model), cancellation);
            TempData[addResult?.Success == true ? "Success" : "Error"] =
                addResult?.Success == true
                    ? "Fabricante adicionado com sucesso!"
                    : addResult.Message;
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Exibe o formulário de edição para um fabricante.
        /// </summary>
        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellation)
        {
            var result = await _manufacturerService.GetManufacturerByIdAsync(id, cancellation);
            if (result?.Success == true)
                return View(ToViewModel(result.Data));

            TempData["Error"] = "Erro ao buscar fabricante.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Atualiza um fabricante existente.
        /// </summary>
        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ManufacturerViewModel model, CancellationToken cancellation)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Preencha todos os campos obrigatórios corretamente.";
                // Retornar View(model) se desejar manter formulário aberto, ou RedirectToAction para reload da lista
                return RedirectToAction(nameof(Index));
            }

            var result = await _manufacturerService.UpdateManufacturerAsync(ToDto(model), cancellation);
            TempData[result?.Success == true ? "Success" : "Error"] =
                result?.Success == true
                    ? "Fabricante atualizado com sucesso!"
                    : "Erro ao atualizar fabricante.";
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Deleta um fabricante.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellation)
        {
            var result = await _manufacturerService.DeleteManufacturerAsync(id, cancellation);
            TempData[result?.Success == true ? "Success" : "Error"] =
                result?.Success == true
                    ? "Fabricante deletado com sucesso!"
                    : "Erro ao deletar fabricante.";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Private Methods

        private static ManufacturerViewModel ToViewModel(ManufacturerDto dto) =>
            new ManufacturerViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Country = dto.Country,
                FoundationYear = dto.FoundationYear,
                Website = dto.Website
            };


        private static ManufacturerDto ToDto(ManufacturerViewModel vm) =>
            new ManufacturerDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Country = vm.Country,
                FoundationYear = vm.FoundationYear,
                Website = vm.Website
            };
        #endregion

    }
}