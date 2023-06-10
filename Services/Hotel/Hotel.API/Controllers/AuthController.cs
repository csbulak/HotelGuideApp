using Hotel.Core.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.BaseController;
using Shared.Dtos;

namespace Hotel.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Auth([FromBody] LoginDto loginDto)
        {
            return CreateActionResultInstance(await _userService.Auth(loginDto.Email, loginDto.Password));
        }
    }
}
