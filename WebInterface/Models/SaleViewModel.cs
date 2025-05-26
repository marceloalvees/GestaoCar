namespace WebInterface.Models
{
    public class SaleViewModel
    {
        public int? Id { get; set; }
        public string VehicleModel { get; set; }
        public string DealershipName { get; set; }
        public string CustomerName { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime SaleDate { get; set; }

    }
}
