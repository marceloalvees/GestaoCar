using System.ComponentModel.DataAnnotations;
using Domain.Validation;

namespace Domain.Entities
{
    public class Dealership : Entity
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        [MaxLength(255)]
        public string Address { get; private set; }
        [MaxLength(50)]
        public string City { get; private set; }
        [MaxLength(50)]
        public string State { get; private set; }
        [MaxLength(10)]
        public string ZipCode { get; private set; }
        [MaxLength(15)]
        public string Phone { get; private set; }
        [MaxLength(100)]
        public string Email { get; private set; }
        public int MaxVehicleCapacity { get; private set; }

        public ICollection<Sale> Sales { get; private set; }
        protected Dealership() { }

        public Dealership(string name, string address, string city, string state, string zipCode, string phone, string email, int maxVehicleCapacity)
        {
            Validate(name, address, city, state, zipCode, phone, email);
            Name = name;
            Address = address;
            City = city;
            State = state;
            ZipCode = zipCode;
            Phone = phone;
            Email = email;
            MaxVehicleCapacity = maxVehicleCapacity;
        }

        public void Update(string name, string address, string city, string state, string zipCode, string phone, string email, int max)
        {
            Validate(name, address, city, state, zipCode, phone, email);
            Name = name;
            Address = address;
            City = city;
            State = state;
            ZipCode = zipCode;
            Phone = phone;
            Email = email;
            MaxVehicleCapacity = max;
        }

        private void Validate(string name, string address, string city, string state, string zipCode, string phone, string email)
        {
            DomainValidation.When(name.Length < 3, "Nome deve ter pelo menos 3 caracteres");
            DomainValidation.When(address.Length < 5, "Endereço deve ter pelo menos 5 caracteres");
            DomainValidation.When(city.Length < 2, "Cidade deve ter pelo menos 2 caracteres");
            DomainValidation.When(state.Length < 2, "Estado deve ter pelo menos 2 caracteres");
            DomainValidation.When(zipCode.Length < 5, "CEP deve ter pelo menos 5 caracteres");
            DomainValidation.When(email.Length < 5, "Email deve ter pelo menos 5 caracteres");
        }
    }
}
