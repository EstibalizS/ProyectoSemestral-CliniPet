//aqui va la funcion que carga toda la info para la factura final 

// Obtener el parámetro 'idFactura' de la URL
const urlParams = new URLSearchParams(window.location.search);
const idFactura = urlParams.get('idFactura'); // Aquí obtienes el valor del idFactura

// Verificar si idFactura se obtiene correctamente
console.log("ID de factura obtenido:", idFactura);

if (idFactura) {

    const url = `https://localhost:7049/api/generarFacturaCompleta/verFactura/${idFactura}`;
     console.log("URL construida:", url);
    // Si existe el idFactura, lo usamos para obtener los detalles de la factura
    fetch(`https://localhost:7049/api/generarFacturaCompleta/verFactura/${idFactura}`)
        .then(response => {
            console.log("Respuesta de la API:", response); // Verifica la respuesta completa

            // Verificar el estado de la respuesta
            if (!response.ok) {
                throw new Error(`Error en la respuesta de la API: ${response.status}`);
            }

            return response.json(); // Convertir la respuesta a JSON
        })
        .then(data => {
            console.log("Datos de la factura recibidos:", data); // Verifica los datos recibidos de la API

            const resumen = data.resumenFactura;  // Asegúrate de usar los nombres correctos
            document.getElementById("fechaFactura").textContent = resumen.fecha; // Cambié "Fecha" por "fecha"
            document.getElementById("subtotalFactura").textContent = resumen.subtotal.toFixed(2); // Cambié "Subtotal" por "subtotal"
            document.getElementById("itbmsFactura").textContent = resumen.itbmsFactura.toFixed(2); // Cambié "ITBMSFactura" por "itbmsFactura"
            document.getElementById("totalFactura").textContent = resumen.totalFactura.toFixed(2); // Cambié "TotalFactura" por "totalFactura"

            // Cargar los detalles de los productos
            const detalles = data.detallesFactura;  // Asegúrate de usar los nombres correctos
            const tbody = document.getElementById("tablaDetalles").getElementsByTagName("tbody")[0];
            detalles.forEach(detalle => {
                const row = tbody.insertRow();
                row.innerHTML = `
                    <td>${detalle.nombreProducto}</td> <!-- Cambié "NombreProducto" por "nombreProducto" -->
                    <td>${detalle.cantidadVendida}</td> <!-- Cambié "CantidadVendida" por "cantidadVendida" -->
                    <td>${detalle.precioUnitario.toFixed(2)}</td> <!-- Cambié "PrecioUnitario" por "precioUnitario" -->
                    <td>${detalle.itbmsLinea.toFixed(2)}</td> <!-- Cambié "ITBMSLinea" por "itbmsLinea" -->
                    <td>${detalle.totalLinea.toFixed(2)}</td> <!-- Cambié "TotalLinea" por "totalLinea" -->
                `;
            });
        })
        .catch(error => {
            console.error("Error al obtener los datos de la factura:", error);
            const mensaje = document.getElementById('mensaje');
            mensaje.style.display = 'block';
            mensaje.classList.remove('success');
            mensaje.classList.add('error');
            mensaje.textContent = `Hubo un error al agregar el servicio: ${error.message}`;
        });
} else {
    console.error('No se encontró el idFactura en la URL');
}
