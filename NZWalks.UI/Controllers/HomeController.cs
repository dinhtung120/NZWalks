using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;

namespace NZWalks.UI.Controllers;

/// <summary>
/// Controller xử lý các request liên quan đến trang chủ và các trang tĩnh
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Constructor khởi tạo controller với logger
    /// </summary>
    /// <param name="logger">Logger để ghi log</param>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Hiển thị trang chủ
    /// </summary>
    /// <returns>View trang chủ</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Hiển thị trang chính sách bảo mật
    /// </summary>
    /// <returns>View trang chính sách bảo mật</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Xử lý và hiển thị trang lỗi
    /// </summary>
    /// <returns>View trang lỗi với thông tin chi tiết</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}