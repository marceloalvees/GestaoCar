using Domain.Entities;
using Domain.Enums;

namespace Application.Dto
{
    public class VehicleDto
    {
        public int? Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public int ManuFacturingYear { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string ManufacturerName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
