using AuthServer.Core.DTOs;
using AuthServer.Core.Entity;
using AuthServer.Core.Service;
using Microsoft.AspNetCore.Identity;
using Sharedlayer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new AppUser()
            {
                Email = createUserDto.Email,
                UserName = createUserDto.UserName,
            };
            var result = await _userManager.CreateAsync(user, createUserDto.PassWord);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
            }
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user),200);
        }

        public async Task<Response<UserAppDto>> GetUserByName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user==null)
            {
                return Response<UserAppDto>.Fail("user not found",404,true);

            }
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }
    }
}
