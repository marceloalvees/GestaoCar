using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Domain.Validation;

namespace Domain.Entities
{
    public class Manufacturer : Entity
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }
        public int FoundationYear { get; private set; }
        [MaxLength(255)]
        public string Website { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }

        protected Manufacturer() { }
        public Manufacturer(string name, string country, int foundationYear, string website)
        {
            Validate(name, country, foundationYear, website);
            Name = name;
            Country = country;
            FoundationYear = foundationYear;
            Website = website;
        }

        private void Validate(string name, string country, int foundationYear, string website)
        {
            DomainValidation.When(string.IsNullOrEmpty(name), "Nome invalido");
            DomainValidation.When(string.IsNullOrEmpty(country), "País invalido");
            DomainValidation.When(foundationYear < 1950 || foundationYear > DateTime.Now.Year, "Ano de Fundação Invalido");
            DomainValidation.When(string.IsNullOrEmpty(website), "Website Invalido");
            DomainValidation.When(!IsValidWebsite(website), "Website Invalido");
        }
        private static bool IsValidWebsite(string website)
        {
            if (string.IsNullOrWhiteSpace(website))
                return false;

            // Regex simples para validar URLs http(s)://www.alguma-coisa.extensão
            var pattern = @"^(http(s)?://)?([\w\-]+\.)+[\w\-]+(/[^\s]*)?$";
            return Regex.IsMatch(website, pattern, RegexOptions.IgnoreCase);
        }

    }
}
