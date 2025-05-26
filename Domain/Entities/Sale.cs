using Domain.Validation;

namespace Domain.Entities
{
    public class Sale : Entity
    {
        public int VehicleId { get; private set; }
        public int DealershipId { get; private set; }
        public int ClientId { get; private set; }
        public DateTime SaleDate { get; private set; }
        public decimal SalePrice { get; private set; }
        public string SaleProtocol { get; private set; }

        public Vehicle Vehicle { get; set; }
        public Dealership Dealership { get; set; }
        public Client Client { get; set; }

        protected Sale() { }

        public Sale(int vehicleId, int dealershipId,  int clientId, DateTime saleDate, decimal salePrice)
        {
            Validate(vehicleId, dealershipId, clientId, saleDate, salePrice);
            VehicleId = vehicleId;
            DealershipId = dealershipId;
            ClientId = clientId;
            SaleDate = saleDate;
            SalePrice = salePrice;
            SaleProtocol = GenerateSaleProtocol();
        }

        public void UpdateSale( DateTime saleDate, decimal salePrice)
        {
            SaleDate = saleDate;
            SalePrice = salePrice;
        }

        private static void Validate(int vehicleId, int dealershipId,  int clientId, DateTime saleDate, decimal salePrice)
        {
            DomainValidation.When(vehicleId <= 0, "Veículo inválido");
            DomainValidation.When(dealershipId <= 0, "Concessionária inválida");
            DomainValidation.When(clientId <= 0, "Cliente inválido");
            DomainValidation.When(saleDate == default(DateTime), "Data da venda inválida");
            DomainValidation.When(salePrice <= 0, "Preço de venda inválido");
        }
        private string GenerateSaleProtocol()
        {
            var random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }
    }
}
