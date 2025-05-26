using System.ComponentModel.DataAnnotations;

namespace WebInterface.Models
{
    public class NewVehicleViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "O campo Modelo é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Modelo deve receber no maximo 100 caracteres")]
        public string? Model { get; set; }
        [Required(ErrorMessage = "O campo Ano de Fabricação é obrigatório")]
        [Range(1950, int.MaxValue, ErrorMessage = "Ano de Fabricação deve ser maior ou igual a 1950")]
        public int? ManuFacturingYear { get; set; }
        //[Required(ErrorMessage = "O campo Preço é obrigatório")]
        //[Range(0, double.MaxValue, ErrorMessage = "O campo Preço deve ser maior ou igual a 0")]
        //public decimal? Price { get; set; }
        //[Required(ErrorMessage = "O campo Tipo de Veículo é obrigatório")]
        //public string? Type { get; set; } 
        [MaxLength(255, ErrorMessage = "O campo Descrição deve receber no maximo 255 caracteres")]
        public string? Description { get; set; }
        //[Required(ErrorMessage = "O campo Fabricante é obrigatório")]
        //public string? ManufacturerName { get; set; }
    }
}
