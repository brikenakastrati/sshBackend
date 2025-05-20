//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace sshBackend1.Helpers
//{
//    public class JwtTokenHelper
//    {
//        private readonly IConfiguration _configuration;

//        public JwtTokenHelper(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public string GenerateToken(string username, string role)
//        {
//            var jwtSettings = _configuration.GetSection("JwtSettings");
//            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

//            var claims = new[]
//            {
//                new Claim(ClaimTypes.Name, username),
//                new Claim(ClaimTypes.Role, role)
//            };

//            var securityKey = new SymmetricSecurityKey(key);
//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: jwtSettings["Issuer"],
//                audience: jwtSettings["Audience"],
//                claims: claims,
//                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
//                signingCredentials: credentials
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }

//        public ClaimsPrincipal GetPrincipalFromToken(string token)
//        {
//            var jwtSettings = _configuration.GetSection("JwtSettings");
//            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

//            var tokenHandler = new JwtSecurityTokenHandler();
//            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,
//                IssuerSigningKey = new SymmetricSecurityKey(key),
//                ValidIssuer = jwtSettings["Issuer"],
//                ValidAudience = jwtSettings["Audience"]
//            }, out var validatedToken);

//            return principal;
//        }
//    }
//}
