using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using APITaxi.Models;

namespace APITaxi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitadoController : ControllerBase
    {
        private readonly string cadenaSQL;

        public InvitadoController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpPost]
        [Route("Placa")]
        public IActionResult ConsultarPlaca([FromBody] string Placa)
        {
            List<Invitado>lista = new List<Invitado>();
            Invitado invitado = new Invitado();
            try
            {
                using(var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("datos_placa", conexion);
                    comando.Parameters.AddWithValue("placa",Placa.ToUpper());
                    comando.CommandType = CommandType.StoredProcedure;
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Invitado()
                            {
                                Foto = lector["foto_conductor"].ToString(),
                                Nombre = lector["nombre_conductor"].ToString(),
                                NumeroCedula = Convert.ToInt64(lector["numero_documento"]),
                                EstadoConductor = Convert.ToBoolean(lector["estado_conductor"]),
                                GrupoSanguineo = lector["rh"].ToString(),
                                Eps = lector["eps"].ToString(),
                                Arl = lector["arl"].ToString(),
                                NombreEmpresa = lector["nombre_empresa"].ToString(),
                                Nit = lector["nit_empresa"].ToString(),
                                Placa = lector["placa_vehiculo"].ToString(),
                                EstadoVehiculo = Convert.ToBoolean(lector["estado_vehiculo"]),
                                Soat = lector["soat"].ToString(),
                                TecnicoMecanica = lector["tecnico_mecanica"].ToString()
                            });
                        }
                    }
                }
                invitado = lista.FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = invitado });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = invitado });
            }
        }

        [HttpPost]
        [Route("Documento")]
        public IActionResult ConsultarDocumento([FromBody] long Documento)
        {
            List<Invitado> lista = new List<Invitado>();
            Invitado invitado = new Invitado();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var comando = new SqlCommand("datos_documento", conexion);
                    comando.Parameters.AddWithValue("numero_documento", Documento);
                    comando.CommandType = CommandType.StoredProcedure;
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Invitado()
                            {
                                Foto = lector["foto_conductor"].ToString(),
                                Nombre = lector["nombre_conductor"].ToString(),
                                NumeroCedula = Convert.ToInt64(lector["numero_documento"]),
                                EstadoConductor = Convert.ToBoolean(lector["estado_conductor"]),
                                GrupoSanguineo = lector["rh"].ToString(),
                                Eps = lector["eps"].ToString(),
                                Arl = lector["arl"].ToString(),
                                NombreEmpresa = lector["nombre_empresa"].ToString(),
                                Nit = lector["nit_empresa"].ToString(),
                                Placa = lector["placa_vehiculo"].ToString(),
                                EstadoVehiculo = Convert.ToBoolean(lector["estado_vehiculo"]),
                                Soat = lector["soat"].ToString(),
                                TecnicoMecanica = lector["tecnico_mecanica"].ToString()
                            });
                        }
                    }
                }
                invitado = lista.FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = invitado });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = invitado });
            }
        }
    }
}
