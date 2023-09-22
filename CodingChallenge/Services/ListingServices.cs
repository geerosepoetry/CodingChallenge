using CodingChallenge.Contracts;
using CodingChallenge.Model;

namespace CodingChallenge.Business
{
    public class ListingServices: IListingServices
    {
        private List<ListingModel> FilterListings(List<ListingModel> listings, int passengers)
        {
            return listings.Where(listing => listing.VehicleType?.MaxPassengers >= passengers).ToList(); 
        }

        private List<ListingModel> CalculateTotalPrice(List<ListingModel> listings, int passengers)
        {
           foreach (var listing in listings)
            {
                listing.TotalPrice = listing.PricePerPassenger * passengers;
            }
           return listings;
        }

        private List<ListingModel> SortedListingsByTotalPrice(List<ListingModel> listings)
        {
            return listings.OrderBy(listing => listing.TotalPrice).ToList();
        }

        public List<ListingModel> FilterAndSortListing(List<ListingModel> listings, int passengers)
        {
            if (listings == null) return new List<ListingModel>();
            var filteredlisting = FilterListings(listings, passengers);
            if (filteredlisting.Count == 0)
            {
               return new List<ListingModel>();
            }
            var calculatedResult = CalculateTotalPrice(filteredlisting, passengers);
            return SortedListingsByTotalPrice(calculatedResult);
        }
       
    }
}
