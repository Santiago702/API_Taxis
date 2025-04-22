using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using APITaxi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace APITaxi.Controllers
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly string cadenaSQL;


        public PersonaController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Authorize]
        [Route("Lista")]

        //[AllowAnonymous]   // ----- Sin autorizacion
        public IActionResult Lista()
        {
            List<Persona> lista = new List<Persona>();

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
                            lista.Add(new Persona()
                            {
                                IdPersona = Convert.ToInt32(lector["id_persona"]),
                                Foto = lector["foto"].ToString(),
                                Nombre = lector["nombre"].ToString(),
                                NumeroCedula = Convert.ToInt64(lector["numero_cedula"]),
                                Telefono = Convert.ToInt64(lector["telefono"]),
                                Correo = lector["correo"].ToString(),
                                Direccion = lector["direccion"].ToString(),
                                Ciudad = lector["ciudad"].ToString(),
                                Estado = Convert.ToBoolean(lector["estado"]),
                                GrupoSanguineo = lector["grupo_sanguineo"].ToString(),
                                Eps = lector["eps"].ToString(),
                                Arl = lector["arl"].ToString(),
                                Contrasena = lector["contrasena"].ToString(),
                                IdEmpresa = Convert.ToInt32(lector["id_empresa"]),
                                DocumentoCedula = lector["documento_cedula"].ToString(),
                                DocumentoEps = lector["documento_eps"].ToString(),
                                DocumentoArl = lector["documento_arl"].ToString(),
                                IdRol = Convert.ToInt32(lector["id_rol"])

                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = lista });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("Obtener/{IdPersona:int}")]

        public IActionResult Obtener(int IdPersona)
        {
            Persona persona = new Persona();
            List<Persona> lista = new List<Persona>();

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
                            lista.Add(new Persona()
                            {
                                IdPersona = Convert.ToInt32(lector["id_persona"]),
                                Foto = lector["foto"].ToString(),
                                Nombre = lector["nombre"].ToString(),
                                NumeroCedula = Convert.ToInt64(lector["numero_cedula"]),
                                Telefono = Convert.ToInt64(lector["telefono"]),
                                Correo = lector["correo"].ToString(),
                                Direccion = lector["direccion"].ToString(),
                                Ciudad = lector["ciudad"].ToString(),
                                Estado = Convert.ToBoolean(lector["estado"]),
                                GrupoSanguineo = lector["grupo_sanguineo"].ToString(),
                                Eps = lector["eps"].ToString(),
                                Arl = lector["arl"].ToString(),
                                Contrasena = lector["contrasena"].ToString(),
                                IdEmpresa = Convert.ToInt32(lector["id_empresa"]),
                                DocumentoCedula = lector["documento_cedula"].ToString(),
                                DocumentoEps = lector["documento_eps"].ToString(),
                                DocumentoArl = lector["documento_arl"].ToString(),
                                IdRol = Convert.ToInt32(lector["id_rol"])
                            });
                        }
                    }
                }
                persona = lista.Where(item => item.IdPersona == IdPersona).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = persona });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = persona });
            }
        }


        [HttpPost]
        
        [Route("ObtenerCorreo")]

        public IActionResult ObtenerCorreo([FromBody]Login login)
        {
            Persona persona = new Persona();
            List<Persona> lista = new List<Persona>();

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
                            lista.Add(new Persona()
                            {
                                IdPersona = Convert.ToInt32(lector["id_persona"]),
                                Foto = lector["foto"].ToString(),
                                Nombre = lector["nombre"].ToString(),
                                NumeroCedula = Convert.ToInt64(lector["numero_cedula"]),
                                Telefono = Convert.ToInt64(lector["telefono"]),
                                Correo = lector["correo"].ToString(),
                                Direccion = lector["direccion"].ToString(),
                                Ciudad = lector["ciudad"].ToString(),
                                Estado = Convert.ToBoolean(lector["estado"]),
                                GrupoSanguineo = lector["grupo_sanguineo"].ToString(),
                                Eps = lector["eps"].ToString(),
                                Arl = lector["arl"].ToString(),
                                Contrasena = lector["contrasena"].ToString(),
                                IdEmpresa = Convert.ToInt32(lector["id_empresa"]),
                                DocumentoCedula = lector["documento_cedula"].ToString(),
                                DocumentoEps = lector["documento_eps"].ToString(),
                                DocumentoArl = lector["documento_arl"].ToString(),
                                IdRol = Convert.ToInt32(lector["id_rol"])
                            });
                        }
                    }
                }
                persona = lista.FirstOrDefault(item => item.Correo == login.Correo);
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = persona });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = persona });
            }


        }
        
        [HttpPost]
        [Authorize]
        [Route("Guardar")]
        //[Authorize(Policy = "Empresa")]
        public IActionResult Guardar([FromBody] Persona objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("guardar_persona", conexion);
                    comando.Parameters.AddWithValue("foto", objeto.Foto);
                    comando.Parameters.AddWithValue("nombre", objeto.Nombre);
                    comando.Parameters.AddWithValue("numero_cedula", objeto.NumeroCedula);
                    comando.Parameters.AddWithValue("telefono", objeto.Telefono);
                    comando.Parameters.AddWithValue("correo", objeto.Correo);
                    comando.Parameters.AddWithValue("direccion", objeto.Direccion);
                    comando.Parameters.AddWithValue("ciudad", objeto.Ciudad);
                    comando.Parameters.AddWithValue("estado", objeto.Estado);
                    comando.Parameters.AddWithValue("grupo_sanguineo", objeto.GrupoSanguineo);
                    comando.Parameters.AddWithValue("eps", objeto.Eps);
                    comando.Parameters.AddWithValue("arl", objeto.Arl);
                    comando.Parameters.AddWithValue("contrasena", objeto.Contrasena);
                    comando.Parameters.AddWithValue("id_empresa", objeto.IdEmpresa);
                    comando.Parameters.AddWithValue("documento_cedula", objeto.DocumentoCedula);
                    comando.Parameters.AddWithValue("documento_eps", objeto.DocumentoEps);
                    comando.Parameters.AddWithValue("documento_arl", objeto.DocumentoArl);
                    comando.Parameters.AddWithValue("id_rol", objeto.IdRol);
                    
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Guardado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }


        [HttpPut]
        [Authorize]
        [Route("Editar")]
        //[Authorize(Policy = "Secretaria")]
        //[Authorize(Policy = "Empresa")]
        public IActionResult Editar([FromBody] Persona objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("editar_persona", conexion);
                    comando.Parameters.AddWithValue("id_persona", objeto.IdPersona == 0 ? DBNull.Value : objeto.IdPersona);
                    comando.Parameters.AddWithValue("foto", string.IsNullOrEmpty(objeto.Foto) ? DBNull.Value : objeto.Foto);
                    comando.Parameters.AddWithValue("nombre", string.IsNullOrEmpty(objeto.Nombre) ? DBNull.Value : objeto.Nombre);
                    comando.Parameters.AddWithValue("numero_cedula", objeto.NumeroCedula == 0 ? DBNull.Value : objeto.NumeroCedula);
                    comando.Parameters.AddWithValue("telefono", objeto.Telefono == 0 ? DBNull.Value : objeto.Telefono);
                    comando.Parameters.AddWithValue("correo", string.IsNullOrEmpty(objeto.Correo) ? DBNull.Value : objeto.Correo);
                    comando.Parameters.AddWithValue("direccion", string.IsNullOrEmpty(objeto.Direccion) ? DBNull.Value : objeto.Direccion);
                    comando.Parameters.AddWithValue("ciudad", string.IsNullOrEmpty(objeto.Ciudad) ? DBNull.Value : objeto.Ciudad);
                    comando.Parameters.AddWithValue("estado", objeto.Estado);
                    comando.Parameters.AddWithValue("grupo_sanguineo", string.IsNullOrEmpty(objeto.GrupoSanguineo) ? DBNull.Value : objeto.GrupoSanguineo);
                    comando.Parameters.AddWithValue("eps", string.IsNullOrEmpty(objeto.Eps) ? DBNull.Value : objeto.Eps);
                    comando.Parameters.AddWithValue("arl", string.IsNullOrEmpty(objeto.Arl) ? DBNull.Value : objeto.Arl);
                    comando.Parameters.AddWithValue("contrasena", string.IsNullOrEmpty(objeto.Contrasena) ? DBNull.Value : objeto.Contrasena);
                    comando.Parameters.AddWithValue("id_empresa", objeto.IdEmpresa == 0 ? DBNull.Value : objeto.IdEmpresa);
                    comando.Parameters.AddWithValue("documento_cedula", string.IsNullOrEmpty(objeto.DocumentoCedula) ? DBNull.Value : objeto.DocumentoCedula);
                    comando.Parameters.AddWithValue("documento_eps", string.IsNullOrEmpty(objeto.DocumentoEps) ? DBNull.Value : objeto.DocumentoEps);
                    comando.Parameters.AddWithValue("documento_arl", string.IsNullOrEmpty(objeto.DocumentoArl) ? DBNull.Value : objeto.DocumentoArl);
                    comando.Parameters.AddWithValue("id_rol", objeto.IdRol == 0 ? DBNull.Value : objeto.IdRol);

                    comando.CommandType = CommandType.StoredProcedure;

                    comando.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }


        [HttpDelete]
        [Route("Eliminar/{IdPersona:int}")]
        [Authorize(Policy = "Admin")]  // Solo rol Admin puede acceder
        public IActionResult Eliminar(int IdPersona)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("eliminar_persona", conexion);
                    comando.Parameters.AddWithValue("id_persona", IdPersona);
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
