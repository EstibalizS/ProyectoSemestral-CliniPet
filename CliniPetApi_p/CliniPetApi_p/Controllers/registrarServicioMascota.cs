using Microsoft.AspNetCore.Mvc;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using CliniPetApi_p.Models;  //Referencia a la clase


namespace CliniPetApi_p.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class registrarServicioMascota : Controller
    {
        private readonly IConfiguration _configuration;
        public registrarServicioMascota(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarServicioMascota([FromBody] ServicioMascotaRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { error = "The request field is required." });
            }

            string connectionString = _configuration.GetConnectionString("BDConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("RegistrarServicioMascota", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    //parametros del sp
                    command.Parameters.AddWithValue("@IDMascota", request.IDMascota); 
                    command.Parameters.AddWithValue("@IDITEM", request.IDITEM); 
                    command.Parameters.AddWithValue("@IDFactura", request.IDFactura);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return Ok(new { mensaje = "Servicio registrado exitosamente" });
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
