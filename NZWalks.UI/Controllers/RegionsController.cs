using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        // GET: RegionsController
        public RegionsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();
            try
            {
                //Get all regions from web api
                var client = _clientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7159/api/Regions");
            
                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return View(response);
        }

    }
}
