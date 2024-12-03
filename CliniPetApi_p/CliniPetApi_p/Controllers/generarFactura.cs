using Microsoft.AspNetCore.Mvc;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using CliniPetApi_p.Models; //Referencia a la clase


namespace CliniPetApi_p.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class generarFactura : Controller
    {
        private readonly IConfiguration _configuration;

        public generarFactura(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        [HttpPost ("generar")]
        public async Task<IActionResult> GenerarFactura([FromBody] FacturaRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { error = "The request field is required." });
            }

            string connectionString = _configuration.GetConnectionString("BDConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(); 
                using (SqlCommand command = new SqlCommand("GenerarFactura", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@CedulaCliente", request.CedulaCliente);
                    command.Parameters.AddWithValue("@IDMascota", (object)request.IDMascota ?? DBNull.Value);
                    try
                    {
                        // Ejecutar el procedimiento y obtener el ID de la factura
                        var facturaId = (int)await command.ExecuteScalarAsync();
                        return Ok(new { mensaje = "Factura generada exitosamente", facturaId });
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
