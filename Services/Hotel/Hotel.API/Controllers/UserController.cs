using AutoMapper;
using Hotel.API.Dtos;
using Hotel.Core.Dtos;
using Hotel.Core.Entities;
using Hotel.Core.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BaseController;

namespace Hotel.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IRoleGroupService _roleGroupService;
        private readonly IUserRoleService _userRoleService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IRoleService roleService, IRoleGroupService roleGroupService, IUserRoleService userRoleService, IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _roleGroupService = roleGroupService;
            _userRoleService = userRoleService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleGroup([FromBody] CreateRoleGroupDto createRoleGroupDto)
        {
            return CreateActionResultInstance(await _roleGroupService.AddAsync(_mapper.Map<RoleGroup>(createRoleGroupDto)));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            return CreateActionResultInstance(await _roleService.CreateRole(createRoleDto));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            return CreateActionResultInstance(await _userService.CreateUser(createUserDto));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserRole([FromBody] CreateUserRoleDto createUserRoleDto)
        {
            return CreateActionResultInstance(await _userRoleService.CreateUserRole(createUserRoleDto));
        }
    }
}
