using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using APITaxi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
namespace APITaxi.Controllers
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly string cadenaSQL;

        public EmpresaController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Authorize]

        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Empresa>lista = new List<Empresa>();
            try
            {
                using(var conexion  = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("listar_empresa", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    using (var lector = comando.ExecuteReader())
                    {
                        while(lector.Read())
                        {
                            lista.Add(new Empresa()
                            {
                                IdEmpresa = Convert.ToInt32(lector["id_empresa"]),
                                Nombre = lector["nombre"].ToString(),
                                Nit = lector["nit"].ToString(),
                                Representante = lector["representante"].ToString(),
                                RedPrincipal = lector["red_principal"].ToString(),
                                RedSecundaria = lector["red_secundaria"].ToString(),
                                IdUsuario = Convert.ToInt32(lector["id_usuario"]),
                                Cupos = Convert.ToInt32(lector["cupos"])

                            });
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = lista });
            }
            catch (Exception error)
            {
                Console.WriteLine("Error en :" + error.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
            }
        }


        [HttpGet]
        [Authorize]
        [Route("Obtener/{IdEmpresa:int}")]
        public IActionResult Obtener(int IdEmpresa)
        {
            Empresa empresa = new Empresa();
            List<Empresa> lista = new List<Empresa>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("listar_empresa", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Empresa()
                            {
                                IdEmpresa = Convert.ToInt32(lector["id_empresa"]),
                                Nombre = lector["nombre"].ToString(),
                                Nit = lector["nit"].ToString(),
                                Representante = lector["representante"].ToString(),
                                RedPrincipal = lector["red_principal"].ToString(),
                                RedSecundaria = lector["red_secundaria"].ToString(),
                                IdUsuario = Convert.ToInt32(lector["id_usuario"]),
                                Cupos = Convert.ToInt32(lector["cupos"])

                            });
                        }
                    }
                }

                empresa = lista.Where(item => item.IdEmpresa == IdEmpresa).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = empresa });
            }
            catch (Exception error)
            {
                Console.WriteLine("Error en :" + error.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = empresa });
            }
        }

        [HttpPost]
        [Authorize(Policy = "Secretaria")]
        
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Empresa objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("guardar_empresa", conexion);
                    comando.Parameters.AddWithValue("nombre", objeto.Nombre);
                    comando.Parameters.AddWithValue("nit", objeto.Nit);
                    comando.Parameters.AddWithValue("representante",objeto.Representante);
                    comando.Parameters.AddWithValue("red_principal",objeto.RedPrincipal);
                    comando.Parameters.AddWithValue("red_secundaria",objeto.RedSecundaria);
                    comando.Parameters.AddWithValue("id_usuario",objeto.IdUsuario);
                    comando.Parameters.AddWithValue("cupos",objeto.Cupos);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.ExecuteNonQuery();

                }

                
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Guardado"});
            }
            catch (Exception error)
            {
                Console.WriteLine("Error en :" + error.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message});
            }
        }

        [HttpPut]
        [Authorize]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Empresa objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("editar_empresa", conexion);
                    comando.Parameters.AddWithValue("id_empresa", objeto.IdEmpresa == 0 ? DBNull.Value : objeto.IdEmpresa);
                    comando.Parameters.AddWithValue("nombre", string.IsNullOrEmpty(objeto.Nombre) ? DBNull.Value : objeto.Nombre);
                    comando.Parameters.AddWithValue("nit", string.IsNullOrEmpty(objeto.Nit) ? DBNull.Value : objeto.Nit);
                    comando.Parameters.AddWithValue("representante", string.IsNullOrEmpty(objeto.Representante) ? DBNull.Value : objeto.Representante);
                    comando.Parameters.AddWithValue("red_principal", string.IsNullOrEmpty(objeto.RedPrincipal) ? DBNull.Value : objeto.RedPrincipal);
                    comando.Parameters.AddWithValue("red_secundaria", string.IsNullOrEmpty(objeto.RedSecundaria) ? DBNull.Value : objeto.RedSecundaria);
                    comando.Parameters.AddWithValue("id_usuario", objeto.IdUsuario == 0 ? DBNull.Value : objeto.IdUsuario);
                    comando.Parameters.AddWithValue("cupos", objeto.Cupos == 0 ? DBNull.Value : objeto.Cupos);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.ExecuteNonQuery();

                }


                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Editado" });
            }
            catch (Exception error)
            {
                Console.WriteLine("Error en :" + error.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        [HttpDelete]
        [Authorize(Policy = "Secretaria-Admin")]
        [Route("Eliminar/{IdEmpresa:int}")]
        public IActionResult Eliminar(int IdEmpresa)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("eliminar_empresa", conexion);
                    comando.Parameters.AddWithValue("id_empresa", IdEmpresa);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.ExecuteNonQuery();

                }


                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Eliminado" });
            }
            catch (Exception error)
            {
                Console.WriteLine("Error en :" + error.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
