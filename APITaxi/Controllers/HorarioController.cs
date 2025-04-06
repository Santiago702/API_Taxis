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
    //[Authorize]
    [ApiController]
    public class HorarioController : ControllerBase
    {
        private readonly string cadenaSQL;

        public HorarioController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Authorize]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Horario> lista = new List<Horario>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("listar_horario", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Horario()
                            {
                                IdHorario = Convert.ToInt32(lector["id_horario"]),
                                Fecha = Convert.ToDateTime(lector["fecha"]),
                                HoraInicio = TimeSpan.Parse(lector["hora_inicio"].ToString()),
                                HoraFin = TimeSpan.Parse(lector["hora_fin"].ToString()),
                                IdConductor = Convert.ToInt32(lector["id_conductor"]),
                                IdVehiculo = Convert.ToInt32(lector["id_vehiculo"])
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
        [Authorize]
        [Route("Obtener/{IdHorario:int}")]
        public IActionResult Obtener(int IdHorario)
        {
            Horario horario = new Horario();
            List<Horario> lista = new List<Horario>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("listar_horario", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Horario()
                            {
                                IdHorario = Convert.ToInt32(lector["id_horario"]),
                                Fecha = Convert.ToDateTime(lector["fecha"]),
                                HoraInicio = TimeSpan.Parse(lector["hora_inicio"].ToString()),
                                HoraFin = TimeSpan.Parse(lector["hora_fin"].ToString()),
                                IdConductor = Convert.ToInt32(lector["id_conductor"]),
                                IdVehiculo = Convert.ToInt32(lector["id_vehiculo"])
                            });
                        }
                    }
                }

                horario = lista.Where(item => item.IdHorario == IdHorario).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", response = horario });
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = horario });
            }
        }

        [HttpPost]
        [Authorize(Policy = "Empresa-Conductor")]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody]Horario objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("guardar_horario", conexion);
                    comando.Parameters.AddWithValue("fecha", objeto.Fecha);
                    comando.Parameters.AddWithValue("hora_inicio", objeto.HoraInicio);
                    comando.Parameters.AddWithValue("hora_fin", objeto.HoraFin);
                    comando.Parameters.AddWithValue("id_conductor", objeto.IdConductor);
                    comando.Parameters.AddWithValue("id_vehiculo", objeto.IdVehiculo);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.ExecuteNonQuery();

                }

                
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Guardado"});
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message});
            }
        }

        [HttpPut]
        [Authorize(Policy = "Empresa-Conductor")]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Horario objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("editar_horario", conexion);
                    comando.Parameters.AddWithValue("id_horario", objeto.IdHorario == 0 ? DBNull.Value : objeto.IdHorario);
                    comando.Parameters.AddWithValue("fecha", objeto.Fecha == DateTime.MinValue ? DBNull.Value : objeto.Fecha);
                    comando.Parameters.AddWithValue("hora_inicio", objeto.HoraInicio == TimeSpan.Zero ? DBNull.Value : objeto.HoraInicio);
                    comando.Parameters.AddWithValue("hora_fin", objeto.HoraFin == TimeSpan.Zero ? DBNull.Value : objeto.HoraFin);
                    comando.Parameters.AddWithValue("id_conductor", objeto.IdConductor == 0 ? DBNull.Value : objeto.IdConductor);
                    comando.Parameters.AddWithValue("id_vehiculo", objeto.IdVehiculo == 0 ? DBNull.Value : objeto.IdVehiculo);
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
        [Authorize]
        [Route("Eliminar/{IdHorario:int}")]
        public IActionResult Eliminar(int IdHorario)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("eliminar_horario", conexion);
                    comando.Parameters.AddWithValue("id_horario", IdHorario);
                    
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
