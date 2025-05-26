using System.ComponentModel.DataAnnotations;
using Domain.Validation;

namespace Domain.Entities
{
    public class Client : Entity
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        [MaxLength(100)]
        public string CPF { get; private set; }
        [MaxLength(15)]
        public string Phone { get; private set; }

        public ICollection<Sale> Sales { get; private set; }

        protected Client() { }

        public Client(string name, string cpf, string phone)
        {
            Validate(name, cpf, phone);
            Name = name;
            CPF = Cpf(cpf);
            Phone = phone;
        }

        public void UpdateDetails(string name, string cpf, string phone)
        {
            Validate(name, cpf, phone);
            Name = name;
            CPF = Cpf(cpf);
            Phone = phone;
        }

        private void Validate(string name, string cpf, string phone)
        {
            DomainValidation.When(name.Length < 3, "Nome deve ter pelo menos 3 caracteres");
            DomainValidation.When(cpf.Length < 11, "CPF deve ter pelo menos 11 caracteres");
            DomainValidation.When(!IsValidCPF(cpf), "CPF inválido");
        }
        private string Cpf(string cpf)
        {
           return cpf.Replace(".", "").Replace("-", "");
        }
        
        private bool IsValidCPF(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11)
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}
