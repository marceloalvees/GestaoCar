using System.ComponentModel.DataAnnotations;

namespace WebInterface.Models
{
    public class CreateSaleViewModel
    {
        [Required(ErrorMessage ="O campo Veiculo é obrigatorio")]
        public string VehicleName { get; set; }
        [Required(ErrorMessage = "O campo concessionária é obrigatorio")]
        public string DealershipName { get; set; }
        [Required(ErrorMessage = "O campo Nome do Cliente é obrigatório.")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "O campo CPF do Cliente é obrigatório.")]
        public string CustomerCPF { get; set; }
        [Required(ErrorMessage = "O campo Telefone do Cliente é obrigatório.")]
        public string CustomerPhone { get; set; }
        [Required(ErrorMessage = "O campo Endereço do Cliente é obrigatório.")]
        public DateTime SaleDate { get; set; }
        [Required(ErrorMessage = "O campo Preço de Venda é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O campo Preço de Venda deve ser um valor positivo.")]
        public decimal SalePrice { get; set; }
    }
}
