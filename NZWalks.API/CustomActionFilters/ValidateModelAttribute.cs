using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NZWalks.API.CustomActionFilters;

/// <summary>
/// Custom action filter để validate model trước khi thực thi action
/// </summary>
public class ValidateModelAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Override phương thức OnActionExecuting để validate model
    /// </summary>
    /// <param name="context">Context của action đang được thực thi</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Kiểm tra ModelState
        if (context.ModelState.IsValid == false)
        {
            // Nếu model không hợp lệ, trả về BadRequest với danh sách lỗi
            context.Result = new BadRequestResult();
        }
    }
}