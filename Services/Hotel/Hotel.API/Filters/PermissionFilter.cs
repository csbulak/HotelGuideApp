using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.BaseController;
using Shared.Dtos;
using Shared.Enums;
using System.Net;
using System.Security.Claims;

namespace Hotel.API.Filters;

public class PermissionFilter : CustomBaseController, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var userRoles = context.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value);

        var actionName = context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ActionName;
        if (userRoles.Contains(actionName)) return;

        context.Result =
            CreateActionResultInstance(Response<ErrorDto>.Fail(ErrorCodes.Forbidden, HttpStatusCode.Forbidden));
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}