using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Entity;
using AuthServer.Core.Repositories;
using AuthServer.Core.Service;
using AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sharedlayer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _genericRepository;

        public AuthenticationService(IOptions<List<Client>> clients, ITokenService tokenService, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> genericRepository)
        {
            _clients = clients.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }
        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user==null) return Response<TokenDto>.Fail("email veya pass yanlış", 400, true);

            if (!(await _userManager.CheckPasswordAsync(user,loginDto.Password))) return Response<TokenDto>.Fail("email veya pass yanlış", 400, true);

            var token = _tokenService.CreateToken(user);
            var userrefreshtoken = await _genericRepository.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();
            if (userrefreshtoken==null)
            {
                await _genericRepository.AddAsync(new UserRefreshToken() { UserId=user.Id,ExpireTime=token.RefreshTokenExpression,RefreshToken=token.RefreshToken});
            }
            else
            {
                userrefreshtoken.ExpireTime = token.RefreshTokenExpression;
                userrefreshtoken.RefreshToken = token.RefreshToken;                   
            }
            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(token, 200);

        }

        public  Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto loginDto)
        {
            var client = _clients.SingleOrDefault(x => x.ClientId == loginDto.ClientId && x.ClientSecret == loginDto.ClientSecret);
            if (client==null)
            {
                return Response<ClientTokenDto>.Fail("clientid veya secret not found",404,true);
            }
            var token = _tokenService.CreateTokenByClient(client);
            return  Response<ClientTokenDto>.Success(token, 200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshtoken)
        {
            var refreshtokenn = await _genericRepository.Where(x => x.RefreshToken == refreshtoken).SingleOrDefaultAsync();
            if (refreshtoken==null)
            {
                return Response<TokenDto>.Fail("not found", 404, true);
            }
            var user = await _userManager.FindByIdAsync(refreshtokenn.UserId);
            if (user==null)
            {
                return Response<TokenDto>.Fail("userid not found", 404, true);
            }
            var tokendto = _tokenService.CreateToken(user);
            refreshtokenn.RefreshToken = tokendto.RefreshToken;
            refreshtokenn.ExpireTime = tokendto.RefreshTokenExpression;

            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(tokendto,200);
        }

        public async Task<Response<NoDataDto>> RevokeRefreshTokenAsync(string refreshtoken)
        {
            var isrefreshtoken = await _genericRepository.Where(x => x.RefreshToken == refreshtoken).SingleOrDefaultAsync();
            if (isrefreshtoken==null)
            {
                return Response<NoDataDto>.Fail(" not found", 404, true);

            }
             _genericRepository.Remove(isrefreshtoken);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(200);
        }
    }
}
