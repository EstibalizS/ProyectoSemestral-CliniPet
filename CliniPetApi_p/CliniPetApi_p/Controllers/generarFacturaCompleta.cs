using Microsoft.AspNetCore.Mvc;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using CliniPetApi_p.Models; //Referencia a la clase




namespace CliniPetApi_p.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class generarFacturaCompleta : Controller
    {
        private readonly IConfiguration _configuration;

        public generarFacturaCompleta(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Genera el id de la factura
        [HttpPost("generar")]
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

                    command.Parameters.AddWithValue("@CedulaCliente", request.CedulaCliente);
                    command.Parameters.AddWithValue("@IDMascota", (object)request.IDMascota ?? DBNull.Value);

                    try
                    {
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
        //función adicional para mostrar el nombre de la mascota en lugar de tener que escribir el id

        [HttpGet("mascotas/{cedulaCliente}")]
        public async Task<IActionResult> ObtenerMascotasPorCedula(string cedulaCliente)
        {
            string connectionString = _configuration.GetConnectionString("BDConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT IDMascota, Nombre FROM Mascota WHERE CedulaCliente = @CedulaCliente", connection))
                {
                    command.Parameters.AddWithValue("@CedulaCliente", cedulaCliente);

                    try
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            var mascotas = new List<object>();

                            while (await reader.ReadAsync())
                            {
                                mascotas.Add(new
                                {
                                    IDMascota = reader.GetInt32(0),
                                    Nombre = reader.GetString(1)
                                });
                            }

                            if (mascotas.Count > 0)
                            {
                                return Ok(mascotas);
                            }
                            else
                            {
                                return NotFound(new { error = "No se encontraron mascotas para esta cédula." });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        return BadRequest(new { error = ex.Message });
                    }
                }
            }
        }

        //registra servicio realizado a mascota en venta
        [HttpPost("registrar-servicio")]
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

        //registra la compra de un producto en la tabla venta
        [HttpPost("comprar-producto")]
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

                    command.Parameters.AddWithValue("@IDITEM", request.IDITEM);
                    command.Parameters.AddWithValue("@Cantidad", request.Cantidad);
                    command.Parameters.AddWithValue("@IDFactura", request.IDFactura);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return Ok(new { mensaje = "Producto agregado a ventas exitosamente" });
                    }
                    catch (SqlException ex)
                    {
                        return BadRequest(new { error = ex.Message });
                    }
                }
            }
        }

        //función para obtener nombre de los productos 
        [HttpGet("productos")]
        public async Task<IActionResult> ObtenerProductos()
        {
            string connectionString = _configuration.GetConnectionString("BDConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT IDITEM, NombreProducto FROM Servicio_Producto WHERE Tipo = 'Producto' ", connection))
                    try
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            var productos = new List<object>();

                            while (await reader.ReadAsync())
                            {
                                productos.Add(new
                                {
                                    idItem = reader.GetInt32(0), // El ID del producto
                                    nombre = reader.GetString(1) // El nombre del producto
                                });
                            }

                            if (productos.Count > 0)
                            {
                                return Ok(productos);
                            }
                            else
                            {
                                return NotFound(new { error = "No se encontraron productos." });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        return BadRequest(new { error = ex.Message });
                    }
            }
        }

        //funcion para  obtener los nombres de los servicios

        //funcion para  obtener los nombres de los servicios
        [HttpGet("servicios")]
        public async Task<IActionResult> ObtenerServicios()
        {
            string connectionString = _configuration.GetConnectionString("BDConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT IDITEM, NombreProducto FROM Servicio_Producto WHERE Tipo = 'Servicio'", connection)) // Ajusta la consulta según tu estructura
                {
                    try
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            var servicios = new List<object>();

                            while (await reader.ReadAsync())
                            {
                                servicios.Add(new
                                {
                                    idItem = reader.GetInt32(0), // El ID del servicio
                                    nombre = reader.GetString(1) // El nombre del servicio
                                });
                            }

                            if (servicios.Count > 0)
                            {
                                return Ok(servicios);
                            }
                            else
                            {
                                return NotFound(new { error = "No se encontraron servicios." });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        return BadRequest(new { error = ex.Message });
                    }
                }
            }
        }


        //completa la factura
        [HttpPost("completar-factura")]
        public async Task<IActionResult> CompletarFactura([FromBody] CompletarFacturaRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { error = "The request body is required." });
            }

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

                    command.Parameters.AddWithValue("@IDFactura", request.IDFactura);

                    try
                    {
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            return Ok(new { mensaje = "La factura ha sido completada.", idFactura = request.IDFactura });
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
        // Este controlador recibe la ID de la factura y devuelve los detalles

        [HttpGet("verFactura/{idFactura}")]
        public async Task<IActionResult> VerFactura(int idFactura)
        {
            if (idFactura <= 0)
            {
                return BadRequest(new { error = "El ID de la factura es inválido." });
            }

            string connectionString = _configuration.GetConnectionString("BDConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Recuperamos los detalles de la factura desde la base de datos
                var facturaDetails = await ObtenerDetallesFactura(idFactura, connection);
                var facturaResumen = await ObtenerResumenFactura(idFactura, connection);

                // Si no se encuentran productos, se devuelve un error
                if (facturaDetails == null || !facturaDetails.Any())
                {
                    return NotFound(new { error = "No se encontraron productos para la factura proporcionada." });
                }

                // Devolvemos los datos en formato JSON (respondiendo adecuadamente a una API)
                var resultado = new
                {
                    DetallesFactura = facturaDetails,
                    ResumenFactura = facturaResumen
                };

                return Ok(resultado); // Usamos Ok para devolver una respuesta con los datos en formato JSON
            }
        }

        // Método para obtener los detalles de la factura
        private async Task<IEnumerable<object>> ObtenerDetallesFactura(int idFactura, SqlConnection connection)
        {
            var productos = new List<object>();

            using (SqlCommand command = new SqlCommand(
                "SELECT sp.NombreProducto, v.CantidadVendida, v.PrecioBruto, v.ITBMSLinea, v.totalLinea " +
                "FROM Venta v " +
                "JOIN Servicio_Producto sp ON v.IDITEM = sp.IDITEM " +
                "WHERE v.IDFactura = @IDFactura", connection))
            {
                command.Parameters.AddWithValue("@IDFactura", idFactura);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        productos.Add(new
                        {
                            NombreProducto = reader["NombreProducto"].ToString(),
                            CantidadVendida = Convert.ToInt32(reader["CantidadVendida"]),
                            PrecioUnitario = Convert.ToDecimal(reader["PrecioBruto"]),
                            ITBMSLinea = Convert.ToDecimal(reader["ITBMSLinea"]),
                            TotalLinea = Convert.ToDecimal(reader["totalLinea"])
                        });
                    }
                }
            }

            return productos;
        }

        // Método para obtener el resumen de la factura (totales)
        private async Task<object> ObtenerResumenFactura(int idFactura, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(
                "SELECT f.Fecha, f.subtotalf, f.ITBMSFactura, f.totalFactura " +
                "FROM Factura f " +
                "WHERE f.IDFactura = @IDFactura", connection))
            {
                command.Parameters.AddWithValue("@IDFactura", idFactura);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new
                        {
                            Fecha = reader["Fecha"].ToString(),
                            Subtotal = Convert.ToDecimal(reader["subtotalf"]),
                            ITBMSFactura = Convert.ToDecimal(reader["ITBMSFactura"]),
                            TotalFactura = Convert.ToDecimal(reader["totalFactura"])
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }
    }
}
