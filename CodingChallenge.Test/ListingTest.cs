using CodingChallenge.Business;
using CodingChallenge.Model;

namespace CodingChallenge.Test
{
    public class ListingTest
    {
        [Fact]
        public void FilterListings_Returns_FilteredListings()
        {
            //Arrange
            var service = new ListingServices();
            var listings = new List<ListingModel>()
            {
                new ListingModel { PricePerPassenger = 8, VehicleType = new VehicleTypeModel { MaxPassengers = 2 } },
                new ListingModel { PricePerPassenger = 10, VehicleType = new VehicleTypeModel { MaxPassengers = 3 } },
                new ListingModel { PricePerPassenger = 12, VehicleType = new VehicleTypeModel { MaxPassengers = 5 } }
            };
            int passengers = 3;

            //Act
            var filteredListings = service.FilterAndSortListing(listings, passengers);

            //Assert
            Assert.Collection(filteredListings, 
                listings => Assert.Equal(30, listings.TotalPrice),
                listings => Assert.Equal(36, listings.TotalPrice));
        }
    }
}