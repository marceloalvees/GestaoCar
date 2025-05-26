namespace Application.Dto
{
    public class SaleDto
    {
        public int? Id { get; set; }
        public string VehicleName { get; set; }
        public string DealershipName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCpf { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal SalePrice { get; set; }
        public string Type { get; set; }
    }
}
