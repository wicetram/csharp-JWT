using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

class Program
{
    private static void Main()
    {
        var payload = new
        {
            meta = new
            {
                id = "",
                timestamp = "",
                source = "",
                clientInfo = new[]
                {
                    new { type = "channel", value = "" },
                    new { type = "email", value = "" },
                    new { type = "userId", value = "" }
                }
            }
        };

        // If "kid" and "alg" values are desired in the header content

        var jwtHeaderDto = new JwtHeaderDto
        {
            //Kty = "oct",
            Kid = "",
            //K = "8ut0KLzijwH9m-0L4EUGerwavnYhSUmsn4B5-VNpqig",
            Alg = "HS256"
        };

        var claims = new List<Claim>
        {
            new Claim("payload", JsonConvert.SerializeObject(payload))
        };

        var symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(jwtHeaderDto)))
        {
            KeyId = jwtHeaderDto.Kid
        };

        var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        // remove "type" field
        if (token is JwtSecurityToken jwt)
        {
            jwt.Header.Remove("typ");
        }

        var tokenString = tokenHandler.WriteToken(token);

        Console.WriteLine(tokenString);

        Console.ReadKey();
    }

    public class JwtHeaderDto
    {
        public string Kty { get; set; }
        public string Kid { get; set; }
        public string K { get; set; }
        public string Alg { get; set; }
    }
}
