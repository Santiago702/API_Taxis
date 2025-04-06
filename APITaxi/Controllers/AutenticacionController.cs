using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APITaxi.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using System.Text;
using APITaxiV2.Funciones;



namespace APITaxi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly string secretKey;
        private readonly string cadenaSQL;

        public AutenticacionController(IConfiguration config)
        {
            secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpPost]
        [Route("Validar")]
        public IActionResult Validar([FromBody] Login request)
        {
            //Here-----------------------

            List<Persona> personas = new List<Persona>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("listar_persona", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            personas.Add(new Persona()
                            {
                                IdPersona = Convert.ToInt32(lector["id_persona"]),
                                Correo = lector["correo"].ToString(),
                                Contrasena = lector["contrasena"].ToString(),
                                Foto = lector["foto"].ToString(),
                                Nombre = lector["nombre"].ToString(),
                                Telefono = Convert.ToInt64(lector["telefono"]),
                                Direccion = lector["direccion"].ToString(),
                                Ciudad = lector["ciudad"].ToString(),
                                Estado = Convert.ToBoolean(lector["estado"]),
                                IdRol = Convert.ToInt32(lector["id_rol"]),

                            });
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }

            //Desencriptado-----------------------

            string contrasena = Encriptado.Desencriptar(request.Contrasena);
            request.Contrasena = contrasena;

            if (personas.Any())
            {
                var validado = personas.Where(item => item.Correo == request.Correo && item.Contrasena == request.Contrasena).FirstOrDefault();

                if (validado != null)
                {
                    var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                    var claims = new ClaimsIdentity();

                    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Correo));
                    claims.AddClaim(new Claim("id_rol", validado.IdRol.ToString()));

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = claims,
                        Expires = DateTime.UtcNow.AddMinutes(45),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                    string tokenCreado = tokenHandler.WriteToken(tokenConfig);

                    return StatusCode(StatusCodes.Status200OK, new { token = tokenCreado });
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
                }

            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new { token = "", mensaje = "No se encontraron Personas" });
            }

        }

    }
}
