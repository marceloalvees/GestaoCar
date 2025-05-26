using System.ComponentModel.DataAnnotations;

namespace WebInterface.Models
{
    public class ManufacturerViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(100, ErrorMessage ="O campo Nome deve receber no maximo 100 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O campo País é obrigatório.")]
        [MaxLength(50)]
        public string Country { get; set; }
        [Required(ErrorMessage = "O campo Ano de Fundação é obrigatório.")]
        [Range(1950, int.MaxValue, ErrorMessage = "Ano de Fundação deve ser maior ou igual a 1950.")]
        public int FoundationYear { get; set; }
        [Required(ErrorMessage = "O campo Website é obrigatório.")]
        [DataType(DataType.Url, ErrorMessage = "O campo Website deve ser uma URL válida.")]
        [MaxLength(255)]
        public string Website { get; set; }
    }
}
