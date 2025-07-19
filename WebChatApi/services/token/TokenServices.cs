using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using WebChatApi.data;
using WebChatApi.model;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WaterProducts.services.token
{
    public class TokenServices : ITokenServices
    {
        private readonly JwtOptions jwtSetting;
        private readonly ApplicationData database;
        public TokenServices(IOptions<JwtOptions> options,ApplicationData database)
        {
            jwtSetting = options.Value;
            this.database = database;
        }

        public async Task<string> getGenerateToken(ApplicationUser user,IList<string> userRoles)
        {

            List<Claim> userClaims = new List<Claim>();

            userClaims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id));
            userClaims.Add(new Claim(ClaimTypes.Name, user.UserName!));
            userClaims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            foreach (string role in userRoles) {
                userClaims.Add(new Claim(ClaimTypes.Role,role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));

            SigningCredentials signing = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwt = new JwtSecurityToken(
                claims: userClaims,
                issuer: jwtSetting.Issuer,
                audience: jwtSetting.Audience,
                signingCredentials: signing,
                expires: DateTime.Now.AddDays(jwtSetting.ExpiresInDays)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        
        public async Task<string> getNewRefreshToken(string userId)
        {
           RefreshToken refreshToken = new RefreshToken
           {
               Token = Guid.NewGuid().ToString(),
               Expiration = DateTime.Now.AddDays(jwtSetting.RefreshTokenExpirationDays),
               UserId=userId,
           };
           database.RefreshTokens.Add(refreshToken);
           await database.SaveChangesAsync();
            return refreshToken.Token;
        }


    }
}
