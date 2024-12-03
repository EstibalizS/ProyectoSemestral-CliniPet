using CliniPetApi_p.Models;  //Referencia a la clase
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;

namespace CliniPetApi_p.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrarMascotaController : Controller
    {
        private readonly IConfiguration _configuration;

        public RegistrarMascotaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método para obtener las razas dependiendo de la especie seleccionada (Perro, Gato)
        [HttpGet("razas/{especieId}")]
        public async Task<IActionResult> ObtenerRazas(int especieId)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("BDConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Cambiar la consulta dependiendo de la especie seleccionada
                    string query = especieId == 1
                        ? "SELECT RazaID, Nombre FROM Raza WHERE EspecieID = 1"  // Perro
                        : especieId == 2
                            ? "SELECT RazaID, Nombre FROM Raza WHERE EspecieID = 2"  // Gato
                            : null;

                    if (query == null)
                    {
                        return BadRequest(new { error = "Especie no válida." });
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        var razas = new List<object>();
                        while (await reader.ReadAsync())
                        {
                            razas.Add(new
                            {
                                RazaID = reader["RazaID"],
                                Nombre = reader["Nombre"]
                            });
                        }
                        return Ok(razas);
                    }
                }
            }
            catch (SqlException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Método para registrar una mascota
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarMascota([FromForm] MascotaModel request)
        {
            if (request == null)
            {
                return BadRequest(new { error = "El campo de solicitud es requerido." });
            }

            // Validación de la foto (si está presente)
            if (request.Foto != null)
            {
                // Verifica el tipo de archivo (imagen)
                if (request.Foto.ContentType.StartsWith("image/"))
                {
                    // Convierte la imagen a un arreglo de bytes
                    using (var memoryStream = new MemoryStream())
                    {
                        await request.Foto.CopyToAsync(memoryStream);
                        byte[] fotoBytes = memoryStream.ToArray();

                        // Procedemos a registrar la mascota con la imagen convertida a bytes
                        string connectionString = _configuration.GetConnectionString("BDConnection");

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            await connection.OpenAsync();
                            using (SqlCommand command = new SqlCommand("RegistrarMascota", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                // Parámetros del procedimiento almacenado
                                command.Parameters.AddWithValue("@Nombre", request.NombreMascota);
                                command.Parameters.AddWithValue("@Especie", request.Especie);
                                command.Parameters.AddWithValue("@Peso", request.Peso);
                                command.Parameters.AddWithValue("@Edad", request.Edad);
                                command.Parameters.AddWithValue("@CedulaCliente", request.CedulaCliente);
                                command.Parameters.AddWithValue("@RazaID", request.RazaID); // Utilizamos el RazaID enviado desde el frontend
                                command.Parameters.AddWithValue("@Genero", request.Genero);
                                command.Parameters.AddWithValue("@Foto", fotoBytes);  // Aquí pasamos la imagen como bytes

                                try
                                {
                                    await command.ExecuteNonQueryAsync();
                                    return Ok(new { mensaje = "Mascota registrada exitosamente." });
                                }
                                catch (SqlException ex)
                                {
                                    return BadRequest(new { error = ex.Message });
                                }
                            }
                        }
                    }
                }
                else
                {
                    return BadRequest(new { error = "El archivo debe ser una imagen." });
                }
            }
            else
            {
                return BadRequest(new { error = "Se requiere una foto para la mascota." });
            }
        }
    }
}