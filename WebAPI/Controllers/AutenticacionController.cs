using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modelo;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        //variable para guardarbtener llave secreta
        private readonly string secretKey;
        public AutenticacionController(IConfiguration config)
        {
            //Obtener la llave secreta
            secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
        }

        //Metodo para autenticar usuario
        [HttpPost]
        [Route("Validar")]
        public IActionResult ValidarUsuario([FromBody] Usuario user)
        {
            if (user.Correo == "nachito@gmail.com" && user.Clave == "123")
            {
                var KeyBytes = Encoding.ASCII.GetBytes(secretKey);
                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Correo));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(32),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(KeyBytes), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
                string tokenCreado = tokenHandler.WriteToken(tokenConfig);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    token = tokenCreado,
                    expirate = DateTime.UtcNow.AddMinutes(12)
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });

            }
        }
    }
}
