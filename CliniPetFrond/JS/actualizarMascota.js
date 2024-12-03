document.getElementById('formActualizarMascota').addEventListener('submit', async function (event) {
    event.preventDefault();

    // Obtener valores del formulario
    const idMascota = document.getElementById('idMascota').value.trim();
    const nuevoPeso = document.getElementById('nuevoPeso').value.trim();
    const nuevaEdad = document.getElementById('nuevaEdad').value.trim();
    const resultContainer = document.getElementById('resultContainer');

    // Limpiar mensajes previos
    resultContainer.style.display = "none";
    resultContainer.innerHTML = "";

    // Validar campos
    if (!idMascota || !nuevoPeso || !nuevaEdad) {
        resultContainer.style.display = "block";
        resultContainer.innerHTML = `<p class="error">Por favor complete todos los campos.</p>`;
        return;
    }

    try {
        // Configurar la URL del endpoint
        const url = `https://localhost:7049/api/ActualizarMascota/actualizar?idMascota=${encodeURIComponent(idMascota)}&nuevoPeso=${encodeURIComponent(nuevoPeso)}&nuevaEdad=${encodeURIComponent(nuevaEdad)}`;

        // Realizar la solicitud PUT
        const response = await fetch(url, { method: 'PUT' });
        const result = await response.json();

        // Mostrar el resultado
        if (response.ok) {
            resultContainer.style.display = "block";
            resultContainer.innerHTML = `<p class="success">${result.message || "Mascota actualizada exitosamente."}</p>`;
        } else {
            resultContainer.style.display = "block";
            resultContainer.innerHTML = `<p class="error">${result.error || "Error al actualizar la mascota."}</p>`;
        }
    } catch (error) {
        resultContainer.style.display = "block";
        resultContainer.innerHTML = `<p class="error">Error de conexión: ${error.message}</p>`;
    }
});