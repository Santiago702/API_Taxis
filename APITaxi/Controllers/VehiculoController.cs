using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using APITaxi.Models;
using System.Numerics;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace APITaxi.Controllers
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    //Authorize]
    [ApiController]
    public class VehiculoController : ControllerBase
    {
        private readonly string cadenaSQL;

        public VehiculoController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Authorize]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Vehiculo> lista = new List<Vehiculo>();
            try
            {
                using(var conexion  = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    var comando = new SqlCommand("listar_vehiculo", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    using(var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Vehiculo()
                            {
                                IdVehiculo = Convert.ToInt32(lector["id_vehiculo"]),
                                Placa = lector["placa"].ToString(),
                                Estado = Convert.ToBoolean(lector["estado"]),
                                Soat = lector["soat"].ToString(),
                                TecnicoMecanica = lector["tecnico_mecanica"].ToString(),
                                IdPersonaProp = Convert.ToInt32(lector["id_persona_prop"]),
                                IdEmpresa = Convert.ToInt32(lector["id_empresa"]),
                                VenceSoat = Convert.ToDateTime(lector["vence_soat"]),
                                VenceTecnicoMecanica = Convert.ToDateTime(lector["vence_tecnico_mecanica"])
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
        [Route("Obtener/{IdVehiculo:int}")]
        public IActionResult Obtener( int IdVehiculo)
        {
            Vehiculo vehiculo = new Vehiculo();
            List<Vehiculo> lista = new List<Vehiculo>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    var comando = new SqlCommand("listar_vehiculo", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Vehiculo()
                            {
                                IdVehiculo = Convert.ToInt32(lector["id_vehiculo"]),
                                Placa = lector["placa"].ToString(),
                                Estado = Convert.ToBoolean(lector["estado"]),
                                Soat = lector["soat"].ToString(),
                                TecnicoMecanica = lector["tecnico_mecanica"].ToString(),
                                IdPersonaProp = Convert.ToInt32(lector["id_persona_prop"]),
                                IdEmpresa = Convert.ToInt32(lector["id_empresa"]),
                                VenceSoat = Convert.ToDateTime(lector["vence_soat"]),
                                VenceTecnicoMecanica = Convert.ToDateTime(lector["vence_tecnico_mecanica"])
                            });
                        }
                    }
                }

                vehiculo = lista.Where(item => item.IdVehiculo == IdVehiculo).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = vehiculo });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = vehiculo });
            }
        }

        [HttpPost]
        [Authorize(Policy = "Empresa")]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody]Vehiculo objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    var comando = new SqlCommand("guardar_vehiculo", conexion);
                    comando.Parameters.AddWithValue("placa",objeto.Placa);
                    comando.Parameters.AddWithValue("estado", objeto.Estado);
                    comando.Parameters.AddWithValue("soat", objeto.Soat);
                    comando.Parameters.AddWithValue("tecnico_mecanica", objeto.TecnicoMecanica);
                    comando.Parameters.AddWithValue("id_persona_prop", objeto.IdPersonaProp);
                    comando.Parameters.AddWithValue("id_empresa", objeto.IdEmpresa);
                    comando.Parameters.AddWithValue("vence_soat", objeto.VenceSoat);
                    comando.Parameters.AddWithValue("vence_tecnico_mecanica", objeto.VenceTecnicoMecanica);

                    comando.CommandType = CommandType.StoredProcedure;
                    comando.ExecuteNonQuery();

                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Guardado"});
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message});
            }
        }

        [HttpPut]
        [Authorize(Policy = "Empresa")]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Vehiculo objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    var comando = new SqlCommand("editar_vehiculo", conexion);
                    comando.Parameters.AddWithValue("id_vehiculo", objeto.IdVehiculo == 0 ? DBNull.Value : objeto.IdVehiculo);
                    comando.Parameters.AddWithValue("placa", string.IsNullOrEmpty(objeto.Placa) ? DBNull.Value : objeto.Placa);
                    comando.Parameters.AddWithValue("estado", objeto.Estado);
                    comando.Parameters.AddWithValue("soat", string.IsNullOrEmpty(objeto.Soat) ? DBNull.Value : objeto.Soat);
                    comando.Parameters.AddWithValue("tecnico_mecanica", string.IsNullOrEmpty(objeto.TecnicoMecanica) ? DBNull.Value : objeto.TecnicoMecanica);
                    comando.Parameters.AddWithValue("id_persona_prop", objeto.IdPersonaProp == 0 ? DBNull.Value : objeto.IdPersonaProp);
                    comando.Parameters.AddWithValue("id_empresa", objeto.IdEmpresa == 0 ? DBNull.Value : objeto.IdEmpresa);
                    comando.Parameters.AddWithValue("vence_soat", objeto.VenceSoat == DateTime.MinValue ? DBNull.Value : objeto.VenceSoat);
                    comando.Parameters.AddWithValue("vence_tecnico_mecanica", objeto.VenceTecnicoMecanica == DateTime.MinValue ? DBNull.Value : objeto.VenceTecnicoMecanica);
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
        [Authorize(Policy = "Empresa-Admin")]
        [Route("Eliminar/{IdVehiculo:int}")]
        public IActionResult Eliminar(int IdVehiculo)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    var comando = new SqlCommand("eliminar_vehiculo", conexion);
                    comando.Parameters.AddWithValue("id_vehiculo", IdVehiculo);
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
