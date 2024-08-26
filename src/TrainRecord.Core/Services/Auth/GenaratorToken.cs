using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Services.Auth;

public class GenaratorToken : IGenaratorToken
{
    private readonly string _secretKey;

    public GenaratorToken(IConfiguration configuration)
    {
        _secretKey = configuration.GetSection("Jwt").GetSection("SecretKey").Value!;
    }

    public ApiTokenResponse Generate(User user)
    {
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var apiTokenResponse = new ApiTokenResponse(4);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                }
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
