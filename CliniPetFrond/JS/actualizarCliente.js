
document.getElementById('formActualizarCliente').addEventListener('submit', async function (event) {
    event.preventDefault();

    const cedula = document.getElementById('cedula').value.trim();
    const telefono = document.getElementById('telefono').value.trim();
    
    const direccion = document.getElementById('direccion').value.trim();
    const resultContainer = document.getElementById('resultContainer');

    // Limpiar mensajes previos
    resultContainer.style.display = "none";
    resultContainer.innerHTML = "";

    // Validar campos requeridos
    if (!cedula || !telefono || !email || !direccion) {
        resultContainer.style.display = "block";
        resultContainer.innerHTML = `<p class="error">Por favor complete todos los campos.</p>`;
        return;
    }

    try {
        const url = `http://localhost:7049/api/ActualizarCliente/actualizar?cedula=${encodeURIComponent(cedula)}&telefono=${encodeURIComponent(telefono)}&email=${encodeURIComponent(email)}&direccion=${encodeURIComponent(direccion)}`;

        const response = await fetch(url, { method: 'PUT' });
        const result = await response.json();

        if (response.ok) {
            resultContainer.style.display = "block";
            resultContainer.innerHTML = `<p class="success">${result.message || "Cliente actualizado exitosamente."}</p>`;
        } else {
            resultContainer.style.display = "block";
            resultContainer.innerHTML = `<p class="error">${result.error || "Error al actualizar el cliente."}</p>`;
        }
    } catch (error) {
        resultContainer.style.display = "block";
        resultContainer.innerHTML = `<p class="error">Error de conexión: ${error.message}</p>`;
    }
});
