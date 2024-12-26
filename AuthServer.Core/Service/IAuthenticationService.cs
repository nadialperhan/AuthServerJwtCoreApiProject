using AuthServer.Core.DTOs;
using Sharedlayer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Service
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshtoken);

        Task<Response<NoDataDto>> RevokeRefreshTokenAsync(string refreshtoken);

        Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto loginDto);

    }
}
