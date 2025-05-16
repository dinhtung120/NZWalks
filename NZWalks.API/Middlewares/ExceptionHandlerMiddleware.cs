using System.Net;
using System.Text.Json;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Middlewares;

/// <summary>
/// Middleware xử lý exception toàn cục cho ứng dụng
/// Bắt và xử lý tất cả các exception chưa được xử lý trong pipeline
/// </summary>
public class ExceptionHandlerMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructor khởi tạo middleware với các dependency cần thiết
    /// </summary>
    /// <param name="logger">Logger để ghi log</param>
    /// <param name="next">Delegate tiếp theo trong pipeline</param>
    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    /// <summary>
    /// Xử lý request và bắt các exception
    /// </summary>
    /// <param name="httpContext">Context của HTTP request</param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // Chuyển request đến middleware tiếp theo
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            // Ghi log lỗi
            _logger.LogError(ex, ex.Message);

            // Xử lý exception
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    /// <summary>
    /// Xử lý exception và trả về response phù hợp
    /// </summary>
    /// <param name="context">Context của HTTP request</param>
    /// <param name="exception">Exception cần xử lý</param>
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Thiết lập response type
        context.Response.ContentType = "application/json";

        // Thiết lập status code
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // Tạo response
        var response = new ErrorDto
        {
            StatusCode = context.Response.StatusCode,
            Message = "An error occurred while processing your request.",
            Details = exception.Message
        };

        // Chuyển response thành JSON
        var result = JsonSerializer.Serialize(response);

        // Gửi response
        await context.Response.WriteAsync(result);
    }
}