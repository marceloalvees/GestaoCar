using System.ComponentModel.DataAnnotations;

namespace WebInterface.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O username é obrigatório.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O Password é obrigatorio")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
