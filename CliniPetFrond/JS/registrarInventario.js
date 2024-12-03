document.getElementById('formActualizarInventario').addEventListener('submit', async function(event) {
    event.preventDefault();

    const idItem = document.getElementById('idItem').value.trim();
    const cantidadAgregada = document.getElementById('cantidadAgregada').value.trim();
    const resultContainer = document.getElementById('resultContainer');
    const resultContent = document.getElementById('resultContent');

    // Limpiar mensajes previos
    resultContent.innerHTML = "";
    resultContainer.style.display = "none";

    // Validar campos requeridos
    if (!idItem || !cantidadAgregada) {
        resultContent.innerHTML = '<p class="error">Por favor complete todos los campos.</p>';
        resultContainer.style.display = "block";
        return;
    }

    try {
        // Crear el cuerpo de la solicitud
        const requestData = {
            IDITEM: parseInt(idItem),
            CantidadAgregada: parseInt(cantidadAgregada)
        };

        // Hacer la solicitud a la API
        const response = await fetch("https://localhost:7049/api/ActualizarInventario/actualizar", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestData)
        });

        // Manejar la respuesta de la API
        if (response.ok) {
            const data = await response.json();
            resultContent.innerHTML = '<p class="success">' + data.mensaje + '</p>';
        } else if (response.status === 400) {
            const errorData = await response.json();
            resultContent.innerHTML = '<p class="error">Error: ' + errorData.error + '</p>';
        } else {
            resultContent.innerHTML = '<p class="error">Error inesperado. Código de estado: ' + response.status + '</p>';
        }
    } catch (error) {
        resultContent.innerHTML = '<p class="error">Error al conectar con la API: ' + error.message + '</p>';
    }

    // Mostrar el contenedor de resultados
    resultContainer.style.display = "block";
});
