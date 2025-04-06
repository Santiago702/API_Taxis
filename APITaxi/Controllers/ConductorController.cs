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
    public class ConductorController : ControllerBase
    {
        private readonly string cadenaSQL;


        public ConductorController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }


        [HttpGet]
        [Authorize]
        [Route("Lista")]

        //[AllowAnonymous]   // ----- Sin autorizacion
        public IActionResult Lista()
        {
            List<Conductor> lista = new List<Conductor>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("listar_conductor", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Conductor()
                            {
                                IdConductor = Convert.ToInt32(lector["id_conductor"]),
                                Foto = lector["foto"].ToString(),
                                Nombre = lector["nombre"].ToString(),
                                NumeroCedula = Convert.ToInt64(lector["numero_cedula"]),
                                Telefono = Convert.ToInt64(lector["telefono"]),
                                Celular = Convert.ToInt64(lector["celular"]),
                                Correo = lector["correo"].ToString(),
                                Direccion = lector["direccion"].ToString(),
                                Ciudad = lector["ciudad"].ToString(),
                                Estado = Convert.ToBoolean(lector["estado"]),
                                GrupoSanguineo = lector["grupo_sanguineo"].ToString(),
                                Eps = lector["eps"].ToString(),
                                Arl = lector["arl"].ToString(),
                                DocumentoCedula = lector["documento_cedula"].ToString(),
                                DocumentoEps = lector["documento_eps"].ToString(),
                                DocumentoArl = lector["documento_arl"].ToString(),
                                IdEmpresa = Convert.ToInt32(lector["id_empresa"]),
                                Contrasena = lector["contrasena"].ToString()
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
        [Route("Obtener/{IdConductor:int}")]

        public IActionResult Obtener(int IdConductor)
        {
            Conductor conductor = new Conductor();
            List<Conductor> lista = new List<Conductor>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("listar_conductor", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Conductor()
                            {
                                IdConductor = Convert.ToInt32(lector["id_conductor"]),
                                Foto = lector["foto"].ToString(),
                                Nombre = lector["nombre"].ToString(),
                                NumeroCedula = Convert.ToInt64(lector["numero_cedula"]),
                                Telefono = Convert.ToInt64(lector["telefono"]),
                                Celular = Convert.ToInt64(lector["celular"]),
                                Correo = lector["correo"].ToString(),
                                Direccion = lector["direccion"].ToString(),
                                Ciudad = lector["ciudad"].ToString(),
                                Estado = Convert.ToBoolean(lector["estado"]),
                                GrupoSanguineo = lector["grupo_sanguineo"].ToString(),
                                Eps = lector["eps"].ToString(),
                                Arl = lector["arl"].ToString(),
                                DocumentoCedula = lector["documento_cedula"].ToString(),
                                DocumentoEps = lector["documento_eps"].ToString(),
                                DocumentoArl = lector["documento_arl"].ToString(),
                                IdEmpresa = Convert.ToInt32(lector["id_empresa"]),
                                Contrasena = lector["contrasena"].ToString()
                            });
                        }
                    }
                }
                conductor = lista.Where(item => item.IdConductor == IdConductor).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = conductor });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = conductor });
            }
        }


        [HttpPost]
        [Route("Guardar")]
        [Authorize(Policy = "Empresa")]  
        public IActionResult Guardar([FromBody] Conductor objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("guardar_conductor", conexion);
                    comando.Parameters.AddWithValue("foto", objeto.Foto);
                    comando.Parameters.AddWithValue("nombre", objeto.Nombre);
                    comando.Parameters.AddWithValue("numero_cedula", objeto.NumeroCedula);
                    comando.Parameters.AddWithValue("telefono", objeto.Telefono);
                    comando.Parameters.AddWithValue("correo", objeto.Correo);
                    comando.Parameters.AddWithValue("direccion", objeto.Direccion);
                    comando.Parameters.AddWithValue("ciudad", objeto.Ciudad);
                    comando.Parameters.AddWithValue("celular", objeto.Celular);
                    comando.Parameters.AddWithValue("estado", objeto.Estado);
                    comando.Parameters.AddWithValue("grupo_sanguineo", objeto.GrupoSanguineo);
                    comando.Parameters.AddWithValue("eps", objeto.Eps);
                    comando.Parameters.AddWithValue("arl", objeto.Arl);
                    comando.Parameters.AddWithValue("documento_cedula", objeto.DocumentoCedula);
                    comando.Parameters.AddWithValue("documento_eps", objeto.DocumentoEps);
                    comando.Parameters.AddWithValue("documento_arl", objeto.DocumentoArl);
                    comando.Parameters.AddWithValue("id_empresa", objeto.IdEmpresa);
                    comando.Parameters.AddWithValue("contrasena", objeto.Contrasena);
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
        [Route("Editar")]
        //[Authorize(Policy = "Secretaria")]
        [Authorize(Policy = "Empresa")]
        public IActionResult Editar([FromBody] Conductor objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("editar_conductor", conexion);
                    comando.Parameters.AddWithValue("id_conductor", objeto.IdConductor == 0 ? DBNull.Value : objeto.IdConductor);
                    comando.Parameters.AddWithValue("foto", string.IsNullOrEmpty(objeto.Foto) ? DBNull.Value : objeto.Foto);
                    comando.Parameters.AddWithValue("nombre", string.IsNullOrEmpty(objeto.Nombre) ? DBNull.Value : objeto.Nombre);
                    comando.Parameters.AddWithValue("numero_cedula", objeto.NumeroCedula == 0 ? DBNull.Value : objeto.NumeroCedula);
                    comando.Parameters.AddWithValue("telefono", objeto.Telefono == 0 ? DBNull.Value : objeto.Telefono);
                    comando.Parameters.AddWithValue("correo", string.IsNullOrEmpty(objeto.Correo) ? DBNull.Value : objeto.Correo);
                    comando.Parameters.AddWithValue("direccion", string.IsNullOrEmpty(objeto.Direccion) ? DBNull.Value : objeto.Direccion);
                    comando.Parameters.AddWithValue("ciudad", string.IsNullOrEmpty(objeto.Ciudad) ? DBNull.Value : objeto.Ciudad);
                    comando.Parameters.AddWithValue("celular", objeto.Celular == 0 ? DBNull.Value : objeto.Celular);
                    comando.Parameters.AddWithValue("estado", objeto.Estado);
                    comando.Parameters.AddWithValue("grupo_sanguineo", string.IsNullOrEmpty(objeto.GrupoSanguineo) ? DBNull.Value : objeto.GrupoSanguineo);
                    comando.Parameters.AddWithValue("eps", string.IsNullOrEmpty(objeto.Eps) ? DBNull.Value : objeto.Eps);
                    comando.Parameters.AddWithValue("arl", string.IsNullOrEmpty(objeto.Arl) ? DBNull.Value : objeto.Arl);
                    comando.Parameters.AddWithValue("documento_cedula", string.IsNullOrEmpty(objeto.DocumentoCedula) ? DBNull.Value : objeto.DocumentoCedula);
                    comando.Parameters.AddWithValue("documento_eps", string.IsNullOrEmpty(objeto.DocumentoEps) ? DBNull.Value : objeto.DocumentoEps);
                    comando.Parameters.AddWithValue("documento_arl", string.IsNullOrEmpty(objeto.DocumentoArl) ? DBNull.Value : objeto.DocumentoArl);
                    comando.Parameters.AddWithValue("id_empresa", objeto.IdEmpresa == 0 ? DBNull.Value : objeto.IdEmpresa);
                    comando.Parameters.AddWithValue("contrasena", string.IsNullOrEmpty(objeto.Contrasena) ? DBNull.Value : objeto.Contrasena);
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
        [Route("Eliminar/{IdConductor:int}")]
        [Authorize(Policy = "Admin")]  // Solo rol Admin puede acceder
        public IActionResult Eliminar(int IdConductor)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("eliminar_conductor", conexion);
                    comando.Parameters.AddWithValue("id_conductor", IdConductor);
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
