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
    [Authorize(Policy = "SuperAdmin")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly string cadenaSQL;

        public RolController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");

        }

        [HttpGet]
        [Route("Lista")]

        public IActionResult Lista()
        {
            List<Rol> lista = new List<Rol>();
            try
            {
                using(var conexion = new SqlConnection(cadenaSQL)) 
                { 
                    conexion.Open();
                    var comando = new SqlCommand("listar_rol", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    using(var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Rol()
                            {
                                IdRol = Convert.ToInt32(lector["id_rol"]),
                                Descripcion = lector["descripcion"].ToString()
                            });
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = lista });

            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
            }
        }

        [HttpGet]
        [Route("Obtener/{IdRol:int}")]

        public IActionResult Obtener(int IdRol)
        {
            List<Rol> lista = new List<Rol>();
            Rol rol = new Rol();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("listar_rol", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Rol()
                            {
                                IdRol = Convert.ToInt32(lector["id_rol"]),
                                Descripcion = lector["descripcion"].ToString()
                            });
                        }
                    }
                }

                rol = lista.Where(item => item.IdRol == IdRol).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = rol });

            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = rol });
            }
        }

        [HttpPost]
        [Route("Guardar")]

        public IActionResult Guardar([FromBody] Rol objeto)
        {
            

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("guardar_rol", conexion);
                    comando.Parameters.AddWithValue("descripcion", objeto.Descripcion);
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.ExecuteNonQuery();
                }

                
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK"});

            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message});
            }
        }

        [HttpPut]
        [Route("Editar")]

        public IActionResult Editar([FromBody] Rol objeto)
        {


            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("editar_rol", conexion);
                    comando.Parameters.AddWithValue("id_rol", objeto.IdRol == 0 ? DBNull.Value : objeto.IdRol);
                    comando.Parameters.AddWithValue("descripcion", objeto.Descripcion is null ? DBNull.Value : objeto.Descripcion);
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
        [Route("Eliminar/{IdRol:int}")]

        public IActionResult Eliminar(int IdRol)
        {


            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("eliminar_rol", conexion);
                    comando.Parameters.AddWithValue("id_rol", IdRol);
                    
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
