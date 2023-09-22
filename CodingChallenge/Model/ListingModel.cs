namespace CodingChallenge.Model
{
    public class ListingModel
    {
        public string Name { get; set; } = string.Empty;
        public decimal PricePerPassenger { get; set; }
        public VehicleTypeModel? VehicleType { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
