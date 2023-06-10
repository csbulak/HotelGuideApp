using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace Shared.BaseController;

public class CustomBaseController : ControllerBase
{
    public IActionResult CreateActionResultInstance<T>(Response<T> response)
    {
        return new ObjectResult(response)
        {
            StatusCode = (int?)response.StatusCode
        };
    }
}