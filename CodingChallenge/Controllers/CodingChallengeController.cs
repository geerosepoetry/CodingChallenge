using CodingChallenge.API.Filters;
using CodingChallenge.Contracts;
using CodingChallenge.Dto;
using CodingChallenge.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CodingChallenge.API.Controllers
{
    [Route("api/controller")]
    [LogActionFilter]
    [ApiController]
    public class CodingChallengeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IListingServices _listing;
        private readonly ILogger _logger;
        private readonly JsonSerializerOptions _options;      
        public CodingChallengeController(IConfiguration configuration,
                                        ILogger<CodingChallengeController> logger,
                                        IListingServices listingBL, JsonSerializerOptions options)
        {
            _configuration = configuration;
            _logger = logger;
            _listing = listingBL;
            _options = options;
            _configuration = configuration;
        }

        /// <summary>
        /// Get candidate name and phone detail.
        /// </summary>
        /// <returns>candidate</returns>
        [HttpGet("/candidate")]
        public IActionResult GetCandidate()
        {
            var candidate = new Candidate()
            {
                Name = "test",
                Phone = "test"
            };

            return Ok(candidate);
        }

        /// <summary>
        /// Get the city location based on IP address.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns>City</returns>
        [HttpGet("/location")]
        public async Task<IActionResult> GetLocation(string ipAddress)
        {
            //Simple Sanity check for valid IP address
            if (string.IsNullOrEmpty(ipAddress) || 
                !IPAddress.TryParse(ipAddress, out var address))
            {
                return BadRequest("Invalid IP address.");
            }
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string ipstackApiUrl = $"{_configuration["IpStackAPI:Url"]}{address}?access_key={_configuration["IpStackAPI:AccessKey"]}";
                    var response = await httpClient.GetAsync(ipstackApiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (content == null)
                        {
                            return new NoContentResult();
                        }
                        var locationInfo = JsonSerializer.Deserialize<LocationInfo>(content, _options);

                        return Ok(new { City = locationInfo?.City });
                    }
                    else
                    {
                        return BadRequest("Unable to retrieve location of the IP address.");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetLocation");
                return StatusCode(500, $"Internal Server Error: {e.Message}");
            }

        }

        /// <summary>
        /// Get sorted listings. 
        /// </summary>
        /// <param name="passengers"></param>
        /// <returns>Calculated listing</returns>
        [HttpGet("/listings")]
        public async Task<IActionResult> GetListings(int passengers)
        {
            //Sanity check number of passengers
            if (passengers <= 0)
            {
                return BadRequest("Number of passenger/s must be provided and greater than zero.");
            }
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(_configuration["JayrideAPI:Url"]);
                   
                    if (response.IsSuccessStatusCode)
                    {
                        var request = await response.Content.ReadAsStringAsync();
                        var quoteRequestData = JsonSerializer.Deserialize<QuoteRequestResponseModel>(request, _options);
                        if (quoteRequestData == null)
                        {
                            return NotFound();
                        }
                        var listings = _listing.FilterAndSortListing(quoteRequestData.Listings, passengers);
                        if (listings.Count == 0)
                        {
                            return NotFound(listings);
                        }
                        return Ok(listings);
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, "Error fetching data from external API.");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetListings");
                return StatusCode(500, $"Internal Server Error: {e.Message}");
            }
        }
    }
}
