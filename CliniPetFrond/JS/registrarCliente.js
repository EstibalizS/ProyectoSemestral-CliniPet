document.getElementById("clienteForm").addEventListener("submit", async function (event) {
    event.preventDefault();

    // Obtener los valores del formulario
    const cedula = document.getElementById("cedula").value;
    const nombre = document.getElementById("nombre").value;
    const telefono = document.getElementById("telefono").value;
    const email = document.getElementById("email").value;
    const direccion = document.getElementById("direccion").value;
    const cantidadDeMascotas = document.getElementById("cantidadDeMascotas").value;

    // Validación de la cantidad de mascotas
    if (cantidadDeMascotas != 0 && cantidadDeMascotas != 1 && cantidadDeMascotas != 2) {
        document.getElementById("responseMessage").innerText = "La cantidad de mascotas debe ser 0, 1 o 2.";
        document.getElementById("responseMessage").style.color = "red";
        return; // Detiene el envío del formulario si la cantidad no es válida
    }

    const clienteData = {
        Cedula: cedula,
        NombreCliente: nombre,
        Teléfono: telefono,
        Email: email,
        Dirección: direccion,
        CantidadDeMascotas: parseInt(cantidadDeMascotas, 10),
    };

    try {
        const response = await fetch("https://localhost:7049/api/RegistrarCliente/registrar", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(clienteData),
        });

        // Manejar la respuesta
        const result = await response.json();

        if (response.ok) {
            document.getElementById("responseMessage").innerText = "Cliente registrado con éxito.";
            document.getElementById("responseMessage").style.color = "green";
            // Limpiar los campos del formulario
            document.getElementById("clienteForm").reset();
        } else {
            document.getElementById("responseMessage").innerText = result.error || "No se pudo registrar al cliente.";
            document.getElementById("responseMessage").style.color = "red";
        }
    } catch (error) {
        console.error("Error al registrar cliente:", error);
        document.getElementById("responseMessage").innerText = "Ocurrió un error al procesar la solicitud.";
        document.getElementById("responseMessage").style.color = "red";
    }
});