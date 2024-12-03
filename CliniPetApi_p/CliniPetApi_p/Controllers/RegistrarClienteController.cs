using CliniPetApi_p.Models;  //Referencia a la clase
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

namespace CliniPetApi_p.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrarClienteController : Controller
    {
        private readonly IConfiguration _configuration;

        public RegistrarClienteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarCliente([FromBody] ClienteModel request)
        {
            // Verificar si el request es nulo
            if (request == null)
            {
                return BadRequest(new { error = "El campo de solicitud es requerido." });
            }

            // Validar si los campos están vacíos o nulos
            if (string.IsNullOrWhiteSpace(request.Cedula))
            {
                return BadRequest(new { error = "La cédula es requerida." });
            }

            if (string.IsNullOrWhiteSpace(request.NombreCliente))
            {
                return BadRequest(new { error = "El nombre del cliente es requerido." });
            }

            if (string.IsNullOrWhiteSpace(request.Teléfono))
            {
                return BadRequest(new { error = "El teléfono es requerido." });
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { error = "El email es requerido." });
            }

            if (string.IsNullOrWhiteSpace(request.Dirección))
            {
                return BadRequest(new { error = "La dirección es requerida." });
            }

            // Validar que la cantidad de mascotas sea un número positivo
            if (request.CantidadDeMascotas < 0)
            {
                return BadRequest(new { error = "La cantidad de mascotas debe ser un número positivo." });
            }

            string connectionString = _configuration.GetConnectionString("BDConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("RegistrarCliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@Cedula", request.Cedula);
                    command.Parameters.AddWithValue("@Nombre", request.NombreCliente);
                    command.Parameters.AddWithValue("@Teléfono", request.Teléfono);
                    command.Parameters.AddWithValue("@Email", request.Email);
                    command.Parameters.AddWithValue("@Dirección", request.Dirección);
                    command.Parameters.AddWithValue("@CantidadDeMascotas", request.CantidadDeMascotas);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return Ok(new { mensaje = "Cliente registrado exitosamente." });
                    }
                    catch (SqlException ex)
                    {
                        return BadRequest(new { error = ex.Message });
                    }
                }
            }
        }
    }
}
