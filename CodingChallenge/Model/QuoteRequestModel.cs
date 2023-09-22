namespace CodingChallenge.Model
{
    public class QuoteRequestResponseModel
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public List<ListingModel>? Listings { get; set; }
    }
}
