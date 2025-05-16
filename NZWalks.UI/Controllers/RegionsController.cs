using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers;

/// <summary>
/// Controller xử lý các request liên quan đến Region trong UI
/// </summary>
public class RegionsController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    /// <summary>
    /// Constructor khởi tạo controller với HttpClientFactory
    /// </summary>
    /// <param name="clientFactory">Factory để tạo HttpClient</param>
    public RegionsController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    /// <summary>
    /// Hiển thị danh sách tất cả các region
    /// </summary>
    /// <returns>View chứa danh sách region</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<RegionDto> response = new List<RegionDto>();
        try
        {
            // Tạo HttpClient và gọi API lấy danh sách region
            var client = _clientFactory.CreateClient();
            var httpResponseMessage = await client.GetAsync("https://localhost:7159/api/Regions");
            httpResponseMessage.EnsureSuccessStatusCode();

            // Đọc và chuyển đổi response thành danh sách RegionDto
            response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
        }
        catch (Exception e)
        {
            // Ghi log lỗi nếu có
            Console.WriteLine(e);
        }
        
        return View(response);
    }

    /// <summary>
    /// Hiển thị form thêm mới region
    /// </summary>
    /// <returns>View chứa form thêm mới</returns>
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    /// <summary>
    /// Xử lý thêm mới region
    /// </summary>
    /// <param name="model">Thông tin region cần thêm mới</param>
    /// <returns>Redirect về trang danh sách nếu thành công, ngược lại hiển thị lại form</returns>
    [HttpPost]
    public async Task<IActionResult> Add(AddRegionViewModel model)
    {
        // Tạo HttpClient và gọi API thêm mới region
        var client = _clientFactory.CreateClient();
        var httpRequestMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://localhost:7159/api/Regions"),
            Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
        };

        // Gửi request và đọc response
        var httpResponseMessage = await client.SendAsync(httpRequestMessage);
        httpResponseMessage.EnsureSuccessStatusCode();
        
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

        // Nếu thêm mới thành công, chuyển hướng về trang danh sách
        if (response is not null)
        {
            return RedirectToAction("Index", "Regions");
        }
        return View();
    }
}
