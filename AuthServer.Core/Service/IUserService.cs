using AuthServer.Core.DTOs;
using Sharedlayer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Service
{
    public interface IUserService
    {
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<Response<UserAppDto>> GetUserByName(string userName);
        Task<Response<NoDataDto>> CreateUserRoles(string username);
    }
}
