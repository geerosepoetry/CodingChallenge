using CodingChallenge.Model;

namespace CodingChallenge.Contracts
{
    public interface IListingServices
    {
        List<ListingModel> FilterAndSortListing(List<ListingModel> listings, int passengers); 
    }
}
