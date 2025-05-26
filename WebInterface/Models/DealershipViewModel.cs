using System.ComponentModel.DataAnnotations;

namespace WebInterface.Models
{
    public class DealershipViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo Nome deve receber no maximo 100 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O campo Endereço é obrigatório.")]
        [MaxLength(255, ErrorMessage = "O campo Endereço deve receber no maximo 255 caracteres")]
        public string Address { get; set; }
        [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O campo Cidade deve receber no maximo 50 caracteres")]
        public string City { get; set; }
        [Required(ErrorMessage = "O campo Estado é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O campo Estado deve receber no maximo 50 caracteres")]
        public string State { get; set; }
        [Required(ErrorMessage = "O campo Cep é obrigatório.")]
        [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "O campo Cep deve estar no formato 12345-678.")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email deve ser um endereço de e-mail válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Capacidade Maxima é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacidade Maxima deve ser maior que 0.")]
        public int MaxCapacity { get; set; }

    }
}
