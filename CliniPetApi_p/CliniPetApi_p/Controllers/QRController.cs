using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QRCoder;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace CliniPetApi_p.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QRController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public QRController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método para generar el código QR con la información del cliente y la mascota
        [HttpGet("generarQR/")]
        public async Task<IActionResult> GenerarCodigoQR(string cedulaCliente)
        {
            
            string connectionString = _configuration.GetConnectionString("BDConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Prepara la consulta para obtener la información del cliente y su mascota
                string query = @"
                    SELECT c.Nombre, c.Cedula, m.Nombre AS NombreMascota, m.Especie, m.Peso
                    FROM Cliente c
                    INNER JOIN Mascota m ON c.Cedula = m.CedulaCliente
                    WHERE c.Cedula = @CedulaCliente";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar parámetro para la cédula del cliente
                    command.Parameters.AddWithValue("@CedulaCliente", cedulaCliente);

                    try
                    {
                        // Ejecutar la consulta y leer los resultados
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Obtener los datos del cliente y mascota
                                var nombreCliente = reader["Nombre"].ToString();
                                var cedula = reader["Cedula"].ToString();
                                var nombreMascota = reader["NombreMascota"].ToString();
                                var especie = reader["Especie"].ToString();
                                var peso = reader["Peso"].ToString();

                                // Concatenar la información para el QR
                                string data = $"Cliente: {nombreCliente}, Cédula: {cedula}, Mascota: {nombreMascota}, Especie: {especie}, Peso: {peso}kg";

                                // Generar el código QR usando la biblioteca QRCoder
                                /*QRCodeGenerator qrGenerator = new QRCodeGenerator();
                                QRCodeData qrCodeData = qrGenerator.CreateQrCode("Your data here", QRCodeGenerator.ECCLevel.Q);
                                QRCode qrCode = new QRCode(qrCodeData); */

                                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q))
                                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                                {
                                    byte[] qrCodeImage = qrCode.GetGraphic(20);
                                    return File(qrCodeImage, "image/png");
                                }

                                // Convertir el código QR a una imagen
                                /*using (Bitmap qrCodeImage = qrCode.GetGraphic(20))  
                                {
                                    // Convertir la imagen a un byte array
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                        byte[] byteImage = ms.ToArray();

                                        // Retornar la imagen en formato base64 
                                        
                                    }
                                }*/
                            }
                            else
                            {
                                return NotFound(new { error = "Cliente o mascota no encontrado." });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        return BadRequest(new { error = "Error al ejecutar la consulta: " + ex.Message });
                    }
                }
            }
        }
    }
}