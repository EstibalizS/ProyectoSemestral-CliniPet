using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace CliniPetApi_p.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActualizarMascotaController : Controller
    {
        private readonly IConfiguration _configuration;

        public ActualizarMascotaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarMascota([FromQuery] int? idMascota, [FromQuery] decimal? nuevoPeso, [FromQuery] int? nuevaEdad)
        {
            // Valida de que el idMascota no esté vacío
            if (!idMascota.HasValue)
            {
                return BadRequest(new { error = "El ID de la mascota no puede estar vacío." });
            }

            // Valida de que el peso no esté vacío
            if (!nuevoPeso.HasValue)
            {
                return BadRequest(new { error = "El campo de peso no puede estar vacío." });
            }

            // Valida de que la edad no esté vacía
            if (!nuevaEdad.HasValue)
            {
                return BadRequest(new { error = "El campo de edad no puede estar vacío." });
            }

            // Validación de que el ID de la mascota exista
            string connectionString = _configuration.GetConnectionString("BDConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT COUNT(1) FROM Mascota WHERE IDMascota = @IDMascota", connection))
                {
                    command.Parameters.AddWithValue("@IDMascota", idMascota.Value);
                    int count = (int)await command.ExecuteScalarAsync();
                    if (count == 0)
                    {
                        return NotFound(new { error = "El ID de la mascota no existe." });
                    }
                }
            }

            //Lógica para actualizar la mascota, llamando al procedimiento almacenado
            string updateQuery = "EXEC ActualizarMascota @IDMascota, @NuevoPeso, @NuevaEdad";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@IDMascota", idMascota.Value);
                    command.Parameters.AddWithValue("@NuevoPeso", nuevoPeso.Value);
                    command.Parameters.AddWithValue("@NuevaEdad", nuevaEdad.Value.ToString());

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return Ok(new { message = "Mascota actualizada exitosamente." });
                    }
                    catch (SqlException ex)
                    {
                        return BadRequest(new { error = "Error al actualizar la mascota: " + ex.Message });
                    }
                }
            }
        }



    }
}
