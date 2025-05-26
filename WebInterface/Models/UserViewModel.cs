using System.ComponentModel.DataAnnotations;

namespace WebInterface.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "O Username é obrigatório.")]
        [StringLength(50, ErrorMessage = "O Username deve ter entre {2} e {1} caracteres.", MinimumLength = 3)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "O Tipo de Usuario é obrigatório.")]
        public string Role { get; set; }
    }
}
