using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Services.Auth;

public class TokenGenerator : ITokenGenerator
{
    private readonly string _secretKey;
    private readonly TimeSpan _expires;

    public TokenGenerator(IConfiguration configuration)
    {
        var time = configuration.GetSection("Jwt:ExpireTime").Get<Dictionary<string, int>>()!;
        _expires = new TimeSpan(time["Hours"], time["Minutes"], 0);
        _secretKey = configuration.GetValue<string>("Jwt:SecretKey")!;
    }

    public ApiTokenResponse Generate(User user)
    {
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var apiTokenResponse = new ApiTokenResponse(_expires);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                ]
            ),
            Expires = apiTokenResponse.ExpiresDateTime,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        apiTokenResponse.Key = tokenHandler.WriteToken(securityToken);
        return apiTokenResponse;
    }
}
