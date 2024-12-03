using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace CliniPetApi_p.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActualizarClienteController : Controller
    {
        private readonly IConfiguration _configuration;

        public ActualizarClienteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarCliente([FromQuery] string cedula, [FromQuery] string telefono, [FromQuery] string email, [FromQuery] string direccion)
        {
            // Validar que los campos no estén vacíos
            if (string.IsNullOrEmpty(cedula))
            {
                return BadRequest(new { error = "La cédula no puede estar vacía." });
            }

            if (string.IsNullOrEmpty(telefono))
            {
                return BadRequest(new { error = "El campo de teléfono debe ser llenado." });
            }

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { error = "El campo de correo electrónico debe ser llenado." });
            }

            if (string.IsNullOrEmpty(direccion))
            {
                return BadRequest(new { error = "El campo de dirección debe ser llenado." });
            }

            // Lógica para actualizar los datos del cliente, llamando al procedimiento almacenado
            string connectionString = _configuration.GetConnectionString("BDConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("EXEC ActualizarCliente @Cedula, @Telefono, @Email, @Direccion", connection))
                {
                    command.Parameters.AddWithValue("@Cedula", cedula);
                    command.Parameters.AddWithValue("@Telefono", telefono);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Direccion", direccion);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return Ok(new { message = "Datos del cliente actualizados exitosamente." });
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
