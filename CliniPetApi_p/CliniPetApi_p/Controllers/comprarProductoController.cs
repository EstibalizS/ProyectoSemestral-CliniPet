using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using CliniPetApi_p.Models; //Referencia a la clase

namespace CliniPetApi_p.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class comprarProductoController : Controller
    {
        private readonly IConfiguration _configuration;

        public comprarProductoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("comprar")]
        public async Task<IActionResult> ComprarProducto([FromBody] comprarProductoRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { error = "The request field is required." });
            }
            string connectionString = _configuration.GetConnectionString("BDConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("ComprarProducto", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    //parametros del sp
                    command.Parameters.AddWithValue("@IDITEM", request.IDITEM); 
                    command.Parameters.AddWithValue("@Cantidad", request.Cantidad); 
                    command.Parameters.AddWithValue("@IDFactura", request.IDFactura);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return Ok(new { mensaje = "Producto agregado a ventas  exitosamente" });
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
