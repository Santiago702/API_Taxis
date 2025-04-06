using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using APITaxi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace APITaxi.Controllers
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TransaccionController : ControllerBase
    {
        private readonly string cadenaSQL;

        public TransaccionController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        //[Authorize]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Transaccion> lista = new List<Transaccion>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("listar_transaccion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Transaccion()
                            {
                                IdTransaccion = Convert.ToInt32(lector["id_transaccion"]),
                                IdUsuario = Convert.ToInt32(lector["id_usuario"]),
                                Accion = lector["accion"].ToString(),
                                Modelo = lector["modelo"].ToString(),
                                Fecha = Convert.ToDateTime(lector["fecha"]),
                                Hora = TimeSpan.Parse(lector["hora"].ToString())
                            });
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", response = lista });
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("Obtener/{IdTransaccion:int}")]
        public IActionResult Obtener(int IdTransaccion)
        {
            Transaccion transaccion = new Transaccion();
            List<Transaccion> lista = new List<Transaccion>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("listar_transaccion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Transaccion()
                            {
                                IdTransaccion = Convert.ToInt32(lector["id_transaccion"]),
                                IdUsuario = Convert.ToInt32(lector["id_usuario"]),
                                Accion = lector["accion"].ToString(),
                                Modelo = lector["modelo"].ToString(),
                                Fecha = Convert.ToDateTime(lector["fecha"]),
                                Hora = TimeSpan.Parse(lector["hora"].ToString())
                            });
                        }
                    }
                }

                transaccion  = lista.Where(item => item.IdTransaccion == IdTransaccion).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", response = transaccion });
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
            }
        }

        [HttpPost]
        //[Authorize(Policy = "Empresa-Conductor")]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Transaccion objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("guardar_transaccion", conexion);
                    comando.Parameters.AddWithValue("id_usuario", objeto.IdUsuario);
                    comando.Parameters.AddWithValue("accion", objeto.Accion);
                    comando.Parameters.AddWithValue("modelo", objeto.Modelo);
                    comando.Parameters.AddWithValue("fecha", objeto.Fecha);
                    comando.Parameters.AddWithValue("hora", objeto.Hora);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Guardado" });
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        [HttpPut]
        //[Authorize(Policy = "Empresa-Conductor")]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Transaccion objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("editar_transaccion", conexion);
                    comando.Parameters.AddWithValue("id_transaccion", objeto.IdTransaccion);
                    comando.Parameters.AddWithValue("id_usuario", objeto.IdUsuario == 0 ? DBNull.Value : objeto.IdUsuario);
                    comando.Parameters.AddWithValue("accion", string.IsNullOrEmpty(objeto.Accion) ? DBNull.Value : objeto.Accion);
                    comando.Parameters.AddWithValue("modelo", string.IsNullOrEmpty(objeto.Modelo) ? DBNull.Value : objeto.Modelo);
                    comando.Parameters.AddWithValue("fecha", objeto.Fecha == DateTime.MinValue ? DBNull.Value : objeto.Fecha);
                    comando.Parameters.AddWithValue("hora", objeto.Hora == TimeSpan.Zero ? DBNull.Value : objeto.Hora);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Editado" });
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        [HttpDelete]
        
        [Route("Eliminar/{IdTransaccion:int}")]
        public IActionResult Eliminar(int IdTransaccion)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("eliminar_transaccion", conexion);
                    comando.Parameters.AddWithValue("id_transaccion", IdTransaccion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Eliminado" });
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
