using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NinaApp.Core.Entities;
using NinaApp.Core.ServiceContracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NinaApp.Infrastructure.Authentication
{
  public class JwtTokenGenerator : IJwtTokenGenerator
  {
    private readonly IConfiguration _configuration;
    public JwtTokenGenerator(IConfiguration configuration) {
      _configuration = configuration;
    }
    public string GenerateToken(User user)
    {
      //Create the claims
      List<Claim> claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Sub, user.ID.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

      //Get the key from settings
      SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!));
      SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      //Create the token description
      SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiryMinutes"]!)),
        Issuer = _configuration["JwtSettings:Issuer"],
        Audience = _configuration["JwtSettings:Audience"],
        SigningCredentials = credentials,
      };

      //Generate the string
      JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }
  }
}
