document.getElementById('formGenerarQR').addEventListener('submit', async function(event) {
    event.preventDefault();

    const cedulaCliente = document.getElementById('cedulaCliente').value.trim();
    const resultContainer = document.getElementById('resultContainer');
    const qrImage = document.getElementById('qrImage');
    const errorMessage = document.getElementById('errorMessage');

    // Limpiar mensajes previos
    resultContainer.style.display = "none";
    errorMessage.style.display = "none";
    qrImage.src = "";

    // Validar que la cédula no esté vacía
    if (!cedulaCliente) {
        errorMessage.textContent = "Por favor, ingrese una cédula válida.";
        errorMessage.style.display = "block";
        return;
    }

    
    try {
        // Hacer la solicitud a la API para generar el QR
        const response = await fetch(`https://localhost:7049/api/QR/generarQR/?cedulaCliente=${encodeURIComponent(cedulaCliente)}`);

        if (response.ok) {
            const qrBlob = await response.blob();
            const qrUrl = URL.createObjectURL(qrBlob);

            // Mostrar el QR generado
            qrImage.src = qrUrl;
            resultContainer.style.display = "block";
        } else if (response.status === 404) {
            const errorData = await response.json();
            errorMessage.textContent = `Error: ${errorData.error}`;
            errorMessage.style.display = "block";
        } else {
            errorMessage.textContent = `Error inesperado. Código de estado: ${response.status}`;
            errorMessage.style.display = "block";
        }
    } catch (error) {
        errorMessage.textContent = `Error al conectar con la API: ${error.message}`;
        errorMessage.style.display = "block";
    }
});

