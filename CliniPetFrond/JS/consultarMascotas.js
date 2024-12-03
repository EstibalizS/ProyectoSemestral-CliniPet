async function consultar() {
    const cedula = document.getElementById("cedula").value.trim();
    const idMascota = document.getElementById("idMascota").value.trim();
    const messageDiv = document.getElementById("message"); // Contenedor para mensajes de error
    const resultsTableBody = document.getElementById("resultsTableBody"); // Seleccionamos el tbody de la tabla

    // Limpiar mensajes y tabla
    if (messageDiv) messageDiv.innerHTML = ""; // Limpiar el mensaje de error
    resultsTableBody.innerHTML = ""; // Limpiar los resultados previos

    // Validar que al menos un campo esté lleno
    if (!cedula && !idMascota) {
        messageDiv.innerHTML = '<p class="error">Por favor ingrese una cédula o un ID de mascota.</p>';
        messageDiv.style.display = 'block'; // Mostrar el mensaje
        return;
    }

    // Validar que el ID de mascota sea mayor o igual a 10000
    if (idMascota && (isNaN(idMascota) || parseInt(idMascota) < 10000)) {
        messageDiv.innerHTML = '<p class="error">El ID de la mascota debe ser un número mayor o igual a 10000.</p>';
        messageDiv.style.display = 'block'; // Mostrar el mensaje
        return;
    }

    try {
        // Construir la URL de la API con el nuevo endpoint
        let url = "https://localhost:7049/api/ConsultarClienteYMascota/consultar?";
        if (cedula) url += `cedula=${encodeURIComponent(cedula)}&`;
        if (idMascota) url += `idMascota=${encodeURIComponent(idMascota)}`;

        // Hacer la solicitud a la API
        const response = await fetch(url);

        if (response.ok) {
            const data = await response.json();

            // Mostrar los resultados en la tabla
            data.forEach((item) => {
                const row = document.createElement("tr");
                row.innerHTML = `
                    <td>${item.cedulaCliente}</td>
                    <td>${item.nombreCliente}</td>
                    <td>${item.teléfono}</td>
                    <td>${item.email}</td>
                    <td>${item.dirección}</td>
                    <td>${item.cantidadDeMascotas}</td>
                    <td>${item.idMascota}</td>
                    <td>${item.nombreMascota}</td>
                    <td>${item.especie}</td>
                    <td>${item.peso}</td>
                    <td>${item.edad}</td>
                    <td>${item.genero}</td>
                    <td>${item.fechaRegistro}</td>
                    <td>${item.razaMascota}</td>
                    <td><img src="${item.foto}" alt="Foto Mascota" width="50"></td>
                `;
                resultsTableBody.appendChild(row); // Agregar la fila a la tabla
            });

            // Hacer visible la tabla con los resultados
            document.getElementById("resultContainer").style.display = "block";
        } else if (response.status === 404) {
            messageDiv.innerHTML = '<p class="not-found">No se encontraron resultados para los criterios ingresados.</p>';
            messageDiv.style.display = 'block';
        } else {
            messageDiv.innerHTML = '<p class="error">Hubo un error en la consulta. Intente nuevamente más tarde.</p>';
            messageDiv.style.display = 'block';
        }
    } catch (error) {
        messageDiv.innerHTML = '<p class="error">Error en la conexión con el servidor.</p>';
        messageDiv.style.display = 'block';
        console.error("Error al consultar la API:", error);
    }
}
