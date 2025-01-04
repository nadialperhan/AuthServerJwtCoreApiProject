using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Entity;
using AuthServer.Core.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sharedlayer.Configurations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class TokenService : ITokenService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly CustomTokenOptions _tokenOptions;
        public TokenService(UserManager<AppUser> userManager, IOptions<CustomTokenOptions> options)
        {
            _userManager = userManager;
            _tokenOptions = options.Value;
        }
        private string CreateRefreshToken()
        {
            var numberbyte = new Byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberbyte);

            return Convert.ToBase64String(numberbyte);
        }
        private async Task< IEnumerable<Claim>> GetClaim(AppUser appUser, List<string> audience)
        {
            var userroles = await _userManager.GetRolesAsync(appUser);
            var userlist = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,appUser.Id),
                new Claim(JwtRegisteredClaimNames.Email,appUser.Email),
                new Claim(ClaimTypes.Name,appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            userlist.AddRange(audience.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            userlist.AddRange(userroles.Select(x => new Claim(ClaimTypes.Role, x)));
            return userlist;
        }
        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, client.ClientId.ToString())
            };

            claims.AddRange(client.Audience.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            return claims;

        }
        public TokenDto CreateToken(AppUser appUser)
        {
            //burda 
            //SigningCredentials signingCredentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256Signature);
            //JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(..., signingCredentials: signingCredentials);
            //ile hmacsha256 ile jwt header oluşturuldu

            //daha sonra payload için issuer: _tokenOptions.Issuer,
            //expires: accessTokenExpression,
            //        notBefore: DateTime.Now,
            //        claims: GetClaim(appUser, _tokenOptions.Audience),  değerleri verildi

            //imzalama yapıldı
            //var token = handler.WriteToken(jwtSecurityToken);
            //burda header ve payload base64 ile birleştirilerek imzalandı
            //bu birleştirilmiş veriyi güvenlik anahtarınız ve belirlediğiniz algoritma (örneğin HMAC SHA256) kullanarak imzalarsınız.
            var accessTokenExpression = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var refreshTokenExpression = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration);
            var securitykey = SignService.GetSymetricSecurityKey(_tokenOptions.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                expires: accessTokenExpression,
                notBefore: DateTime.Now,
                claims: GetClaim(appUser, _tokenOptions.Audience).Result,
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            var tokenDdto = new TokenDto()
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpression = accessTokenExpression,
                RefreshTokenExpression = refreshTokenExpression
            };
            return tokenDdto;
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpression = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securitykey = SignService.GetSymetricSecurityKey(_tokenOptions.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                expires: accessTokenExpression,
                notBefore: DateTime.Now,
                claims: GetClaimsByClient(client),
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            var tokenDdto = new ClientTokenDto()
            {
                AccessToken = token,
                AccessTokenExpression = accessTokenExpression,
            };
            return tokenDdto;
        }
    }
}
