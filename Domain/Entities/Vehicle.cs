using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Validation;

namespace Domain.Entities
{
    public class Vehicle : Entity
    {
        [MaxLength(100)]
        public string Model { get; private set; }
        public int ManuFacturingYear { get; private set; }
        public decimal Price { get; private set; }
        public VehicleTypeEnum VehicleType { get; private set; }
        public string? Description { get; set; }
        public Manufacturer Manufacturer { get; private set; }
        public ICollection<Sale> Sales { get; private set; }

        protected Vehicle() { }
        public Vehicle(string model, int manuFacturingYear, decimal price, VehicleTypeEnum vehicleType, string? description)
        {
            Validate(model, manuFacturingYear, price, vehicleType);
            Model = model;
            ManuFacturingYear = manuFacturingYear;
            Price = price;
            VehicleType = vehicleType;
            Description = description;
        }

        private static void Validate(string model, int manuFacturingYear, decimal price, VehicleTypeEnum vehicleType)
        {
            DomainValidation.When(string.IsNullOrEmpty(model), "Modelo é Obrigatorio");
            DomainValidation.When(manuFacturingYear < 1950 || manuFacturingYear > DateTime.Now.Year, "Ano de Fabricação Invalido");
            DomainValidation.When(price <= 0, "Preço Invalido");
            DomainValidation.When(!Enum.IsDefined(typeof(VehicleTypeEnum), vehicleType), "Tipo de Veículo Invalido");
        }
        public void Update(string model, int manuFacturingYear, decimal price, VehicleTypeEnum vehicleType, string? description)
        {
            Validate(model, manuFacturingYear, price, vehicleType);
            Model = model;
            ManuFacturingYear = manuFacturingYear;
            Price = price;
            VehicleType = vehicleType;
            Description = description;
        }
        public void SetManufacturer(Manufacturer manufacturer)
        {
            DomainValidation.When(manufacturer == null, "Fabricante Invalido");
            Manufacturer = manufacturer;
        }

    }
}

