using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using CliniPetApi_p.Models; //Referencia a la clase



namespace CliniPetApi_p.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class completarFacturaController : Controller
    {
        private readonly IConfiguration _configuration;

        public completarFacturaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("CompletarFactura")]
        public async Task<IActionResult> CompletarFactura([FromBody] CompletarFacturaRequest request)
        {
            // Validar que el request no sea nulo
            if (request == null)
            {
                return BadRequest(new { error = "The request body is required." });
            }

            // Validar que el IDFactura sea positivo
            if (request.IDFactura <= 0)
            {
                return BadRequest(new { error = "The 'IDFactura' field is required and must be a positive integer." });
            }

            string connectionString = _configuration.GetConnectionString("BDConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("CompletarFactura", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    //parametros del sp
                    command.Parameters.AddWithValue("@IDFactura", request.IDFactura);

                    try
                    {
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        // Verificar si se afectaron filas
                        if (rowsAffected > 0)
                        {
                            return Ok(new { mensaje = "La factura ha sido completada." });
                        }
                        else
                        {
                            return NotFound(new { error = "No se encontró una factura con el ID proporcionado." });
                        }
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
