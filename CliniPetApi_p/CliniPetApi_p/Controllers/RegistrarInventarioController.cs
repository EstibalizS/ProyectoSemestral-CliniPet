using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using CliniPetApi_p.Models;  //Referencia a la clase
using System.Threading.Tasks;

namespace CiliniPetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActualizarInventarioController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ActualizarInventarioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("actualizar")]
        public async Task<IActionResult> ActualizarInventario([FromBody] ActualizarInventarioRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { error = "The request field is required." });
            }

            string connectionString = _configuration.GetConnectionString("BDConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("ActualizarInventario", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@IDITEM", request.IDITEM);
                    command.Parameters.AddWithValue("@CantidadAgregada", request.CantidadAgregada);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return Ok(new { mensaje = "Inventario actualizado exitosamente" });
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