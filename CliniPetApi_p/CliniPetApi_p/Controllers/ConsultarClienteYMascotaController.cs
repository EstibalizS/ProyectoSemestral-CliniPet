using Microsoft.AspNetCore.Mvc;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CliniPetApi_p.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultarClienteYMascotaController : Controller
    {
        private readonly IConfiguration _configuration;

        public ConsultarClienteYMascotaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("consultar")]
        public async Task<IActionResult> ConsultarClienteYMascota([FromQuery] string? cedula, [FromQuery] int? idMascota)
        {
            // Validación de los parámetros de entrada
            if (string.IsNullOrEmpty(cedula) && !idMascota.HasValue)
            {
                return BadRequest(new { error = "Se debe proporcionar una cédula o un ID de mascota para la consulta." });
            }

            // Obtener la cadena de conexión desde el archivo de configuración
            string connectionString = _configuration.GetConnectionString("BDConnection");

            // Conexión a la base de datos usando SqlConnection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Preparar el comando para ejecutar el procedimiento almacenado
                using (SqlCommand command = new SqlCommand("ConsultarClienteYMascota", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros al comando
                    command.Parameters.AddWithValue("@Cedula", string.IsNullOrEmpty(cedula) ? (object)DBNull.Value : cedula);
                    command.Parameters.AddWithValue("@IDMascota", idMascota ?? (object)DBNull.Value);

                    try
                    {
                        // Ejecutar el procedimiento almacenado y leer los resultados
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var result = new List<object>();

                            // Leer los resultados de la consulta
                            while (await reader.ReadAsync())
                            {
                                result.Add(new
                                {
                                    CedulaCliente = reader["CedulaCliente"],
                                    NombreCliente = reader["NombreCliente"],
                                    Teléfono = reader["Teléfono"],
                                    Email = reader["Email"],
                                    Dirección = reader["Dirección"],
                                    CantidadDeMascotas = reader["CantidadDeMascotas"],
                                    IDMascota = reader["IDMascota"],
                                    NombreMascota = reader["NombreMascota"],
                                    Especie = reader["Especie"],
                                    Peso = reader["Peso"],
                                    Edad = reader["Edad"],
                                    Genero = reader["Genero"],
                                    FechaRegistro = reader["FechaRegistro"],
                                    RazaMascota = reader["RazaMascota"],
                                    Foto = reader["Foto"]
                                });
                            }

                            // Devolver los resultados si se encontraron
                            if (result.Count > 0)
                            {
                                return Ok(result);
                            }
                            else
                            {
                                // Si no se encontraron resultados, devolver un mensaje adecuado
                                return NotFound(new { message = "No se encontraron resultados para los parámetros proporcionados." });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        // Capturar cualquier error SQL y devolver un mensaje adecuado
                        return BadRequest(new { error = "Error al ejecutar la consulta: " + ex.Message });
                    }
                }
            }
        }

    }
}
