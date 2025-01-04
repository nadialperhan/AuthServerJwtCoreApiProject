using AuthServer.Core.DTOs;
using AuthServer.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sharedlayer.ExceptionCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
           
                var result = await _userService.CreateUserAsync(createUserDto);
                return ActionResultInstance(result);
            
            
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return ActionResultInstance(await _userService.GetUserByName(HttpContext.User.Identity.Name));
        }

        [HttpPost("CreateUserRoles/{username}")]
        public async Task<IActionResult> CreateUserRoles(string username)
        {
            return ActionResultInstance(await _userService.CreateUserRoles(username));
        }
    }
}
