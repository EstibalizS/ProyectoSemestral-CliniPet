document.getElementById('buscarMascotas').addEventListener('click', async () => {
    const cedula = document.getElementById('cedula').value.trim();
    const mascotaSelect = document.getElementById('mascota');
    const mensaje = document.getElementById('mensaje');

    if (!cedula) {
        mensaje.style.display = 'block';
        mensaje.textContent = 'Por favor, ingrese la cédula del cliente.';
        return;
    }

    try {
        // llamada al  API para obtener las mascotas del cliente
        const response = await fetch(`https://localhost:7049/api/generarFacturaCompleta/mascotas/${cedula}`);
        if (!response.ok) {
            throw new Error('Error al obtener las mascotas del cliente.');
        }

        const mascotas = await response.json();
        

        // Depuración:
        console.log('Datos recibidos del API:', mascotas);

        // Limpia el combo box
        mascotaSelect.innerHTML = '<option value="" disabled selected>Seleccione una mascota</option>';

        // Llena el combo box con las mascotas obtenidas
        mascotas.forEach(mascota => {
            console.log(`Mascota cargada: ${mascota.nombre}, IDMascota: ${mascota.idMascota}`);
            const option = document.createElement('option');
            option.value = mascota.idMascota; // Ajusta según el atributo de tu API
            option.textContent = mascota.nombre;
            mascotaSelect.appendChild(option);
        });

        mascotaSelect.disabled = false;
        mensaje.style.display = 'none';
        document.getElementById('siguiente').disabled = false;

    } catch (error) {
        mensaje.style.display = 'block';
        mensaje.textContent = error.message || 'Hubo un error al procesar la solicitud.';
    }
});

// Habilitar botón "Siguiente" si hay algo escrito en el campo de cédula
document.getElementById('cedula').addEventListener('input', () => {
    const cedula = document.getElementById('cedula').value.trim();
    const siguienteButton = document.getElementById('siguiente');

    if (cedula) {
        siguienteButton.disabled = false; // Habilitar botón
    } else {
        siguienteButton.disabled = true; // Deshabilitar si se borra el texto
    }
});

// Lógica para el botón "Siguiente"
document.getElementById('siguiente').addEventListener('click', async () => {
    const cedulaCliente = document.getElementById('cedula').value.trim();
    const mascotaSelect = document.getElementById('mascota');
    const idMascota = mascotaSelect.value;

    idMascotaGlobal= mascotaSelect.value;

    console.log('CedulaCliente ingresada:', cedulaCliente);
    console.log('IDMascota seleccionado en el combo box:', idMascota);



    if (!cedulaCliente) {
        const mensaje = document.getElementById('mensaje');
        mensaje.style.display = 'block';
        mensaje.classList.add('error'); // Aplica la clase de error
        mensaje.textContent = 'Por favor, ingrese la cédula del cliente antes de continuar.';
        return;
    }

    const requestData = {
        CedulaCliente: cedulaCliente, // Cambia los nombres de las propiedades según lo que espera tu API
        IDMascota: idMascota
    };

    // Depuración: Verifica los datos que se enviarán al servidor
    console.log('Datos que se enviarán al API:', requestData);

    try {
        // Llama al API para generar la factura
        const response = await fetch('https://localhost:7049/api/generarFacturaCompleta/generar', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ CedulaCliente: cedulaCliente, IDMascota: idMascota })
        });

        if (!response.ok) {
            throw new Error('Error al generar la factura.');
        }

        const data = await response.json(); // Obtén la respuesta
        idFacturaGlobal = data.facturaId;

        const mensaje = document.getElementById('mensaje');
        mensaje.style.display = 'block';
        mensaje.classList.remove('error'); // Elimina la clase de error
        mensaje.classList.add('success'); // Aplica la clase de éxito
        mensaje.textContent = `Factura generada con éxito. ID de factura: ${data.facturaId}`;

        //llamado a la funcion que muestra la prefactura
        mostrarPrefacturaVacia();

        //coocarle el id de la factuira 
        document.getElementById('prefactura-id').textContent = data.facturaId;
        
    } catch (error) {
        console.error('Error en la generación de factura:', error);
        
        const mensaje = document.getElementById('mensaje');
        mensaje.style.display = 'block';
        mensaje.classList.remove('success'); // Elimina la clase de éxito
        mensaje.classList.add('error'); // Aplica la clase de error
        mensaje.textContent = `Hubo un error al generar la factura: ${error.message}`;
    }
});

//funcion que muestra la prefactura
function mostrarPrefacturaVacia() {
    const prefactura = document.getElementById("prefactura");
    const mensaje = document.getElementById('mensaje');
    
    // Limpiar el contenido de la prefactura (si ya tiene algo de antes)
    document.getElementById("prefactura-cliente").textContent = '';
    document.getElementById("prefactura-fecha").textContent = '';
    document.getElementById("prefactura-id").textContent = '';
    
    // Mostrar el recuadro de la prefactura vacía
    prefactura.style.display = "block";
    
    // Ocultar los formularios de producto y servicio hasta que el usuario elija
    document.getElementById("form-producto").style.display = "none";
    document.getElementById("form-servicio").style.display = "none";

    // Habilitar los botones de producto y servicio
    document.getElementById("btnServicios").disabled = false;
    document.getElementById("btnProductos").disabled = false;
}

// Llamar a esta función cuando se haga clic en "Siguiente"
document.getElementById('siguiente').addEventListener('click', () => {
    // Mostrar la prefactura vacía
    mostrarPrefacturaVacia();
});

// Función que muestra el formulario de productos o servicios
function mostrarFormulario(tipo) {
    // Ocultar ambos formularios de productos y servicios
    document.getElementById("form-producto").style.display = "none";
    document.getElementById("form-servicio").style.display = "none";
    
    // Mostrar el formulario correspondiente
    if (tipo === "producto") {
        document.getElementById("form-producto").style.display = "block";
        // Cargar los productos desde la API
        cargarProductos();
    } else if (tipo === "servicio") {
        document.getElementById("form-servicio").style.display = "block";
        // Cargar los servicios desde la API
        cargarServicios();
    }
}

// Funciones para manejar los botones de "Servicios" y "Productos"
document.getElementById("btnServicios").addEventListener("click", function() {
    mostrarFormulario("servicio");
    //rellenar el id de la mascota
    if (idMascotaGlobal) {
        const mascotaInput = document.getElementById('mascota-id'); // Asegúrate de usar el ID correcto del campo
        mascotaInput.value = idMascotaGlobal;
    }
});

document.getElementById("btnProductos").addEventListener("click", function() {
    mostrarFormulario("producto");
});

// Funcion para cargar los nombres de los productos 
async function cargarProductos() {
    try {
        const response = await fetch('https://localhost:7049/api/generarFacturaCompleta/productos'); 
        if (!response.ok) throw new Error('Error al cargar productos.');
        const productos = await response.json();
        
        // Limpiar los productos anteriores
        const productosSelect = document.getElementById("productos");
        productosSelect.innerHTML = '<option value="" disabled selected>Seleccione un producto</option>';

        productos.forEach(producto => {
            const option = document.createElement('option');
            option.value = producto.idItem;
            option.textContent = producto.nombre;
            productosSelect.appendChild(option);
        });
        
    } catch (error) {
        console.error('Error al cargar productos:', error);
    }
}

//Funcion para cargar los nombres de los servicios
async function cargarServicios() {
    try {
        const response = await fetch('https://localhost:7049/api/generarFacturaCompleta/servicios'); 
        if (!response.ok) throw new Error('Error al cargar servicios.');
        const servicios = await response.json();
        
        // Limpiar los servicios anteriores
        const serviciosSelect = document.getElementById("servicios");
        serviciosSelect.innerHTML = '<option value="" disabled selected>Seleccione un servicio</option>';

        servicios.forEach(servicio => {
            const option = document.createElement('option');
            option.value = servicio.idItem;
            option.textContent = servicio.nombre;
            serviciosSelect.appendChild(option);
        });
        
    } catch (error) {
        console.error('Error al cargar servicios:', error);
    }
}

// Función para agregar un servicio
document.getElementById("btnAgregarServicio").addEventListener("click", async function () {
    const idItem = document.getElementById("servicios").value;
    const idMascota = idMascotaGlobal; // Asumiendo que lo tienes global
    const idFactura = idFacturaGlobal;

    if (!idItem) {
        const mensaje = document.getElementById('mensaje');
        mensaje.style.display = 'block';
        mensaje.classList.add('error');
        mensaje.textContent = 'Por favor, seleccione un servicio.';
        return;
    }

    //para verificar lo que se esta enviando 
    const requestData = { idMascota: idMascota, iditem: idItem, idFactura: idFactura };

    console.log('Datos que se enviarán al API para agregar servicio:', requestData);

    try {
        const response = await fetch('https://localhost:7049/api/generarFacturaCompleta/registrar-servicio', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ idMascota: idMascota, iditem: idItem, idFactura: idFactura })
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.error || 'Error al registrar el servicio.');
        }

        const mensaje = document.getElementById('mensaje');
        mensaje.style.display = 'block';
        mensaje.classList.remove('error');
        mensaje.classList.add('success');
        mensaje.textContent = 'Servicio agregado correctamente. Puedes agregar otro servicio si lo deseas.';

    } catch (error) {
        const mensaje = document.getElementById('mensaje');
        mensaje.style.display = 'block';
        mensaje.classList.remove('success');
        mensaje.classList.add('error');
        mensaje.textContent = `Hubo un error al agregar el servicio: ${error.message}`;
    }
});

// Función para agregar un producto
document.getElementById("btnAgregarProducto").addEventListener("click", async function () {
    const idItem = document.getElementById("productos").value;
    const cantidad = document.getElementById("producto-cantidad").value.trim();
    const idFactura = idFacturaGlobal

    if (!idItem || !cantidad || isNaN(cantidad) || cantidad <= 0) {
        const mensaje = document.getElementById('mensaje');
        mensaje.style.display = 'block';
        mensaje.classList.add('error');
        mensaje.textContent = 'Por favor, seleccione un producto y una cantidad válida.';
        return;
    }

    //para verificar lo que sera enviado al api
    const requestData = { iditem: idItem, cantidad: parseInt(cantidad), idFactura: idFactura };

    
    console.log('Datos que se enviarán al API para agregar producto:', requestData);

    try {
        const response = await fetch('https://localhost:7049/api/generarFacturaCompleta/comprar-producto', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ iditem: idItem, cantidad: parseInt(cantidad), idFactura: idFactura })
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.error || 'Error al registrar el producto.');
        }

        const mensaje = document.getElementById('mensaje');
        mensaje.style.display = 'block';
        mensaje.classList.remove('error');
        mensaje.classList.add('success');
        mensaje.textContent = 'Producto agregado correctamente. Puedes agregar otro producto si lo deseas.';
        // Reinicia el campo de cantidad pero deja el producto seleccionado
        document.getElementById("producto-cantidad").value = '';

    } catch (error) {
        console.error("Error al agregar producto:", error);
        const mensaje = document.getElementById('mensaje');
        mensaje.style.display = 'block';
        mensaje.classList.remove('success');
        mensaje.classList.add('error');
        mensaje.textContent = `Hubo un error al agregar el producto: ${error.message}`;
    }
});

async function fetchAndProcessResponse(response) {
    // Read and process response before consuming it
    const responseText = await response.text(); // Read it as plain text first
    //console.log("Response Body:", responseText);

    // Now process the response as JSON
    return JSON.parse(responseText);  // Parse the body as JSON
}

async function finalizarFactura() {
    // Utilizar el idFacturaGlobal directamente sin necesidad de obtenerlo de la URL
    const idFactura = idFacturaGlobal;

    // Log para verificar el valor de idFactura
    //console.log("ID de factura:", idFactura);

    if (!idFactura) {
        const mensaje = document.getElementById('mensaje');
        mensaje.style.display = 'block';
        mensaje.classList.add('error');
        mensaje.textContent = 'No se encontró el ID de la factura.';
        return;
    }

    // Crear el cuerpo de la solicitud para completar la factura
    const requestBody = {
        IDFactura: idFactura // Usamos el idFacturaGlobal directamente
    };

    // Log para verificar los datos que se van a enviar al servidor
    //console.log("Datos que se enviarán al servidor:", requestBody);

    try {
        // Llamada al endpoint para completar la factura
        const response = await fetch("https://localhost:7049/api/generarFacturaCompleta/completar-factura", {
            method: "POST", // Método POST para enviar datos
            headers: {
                "Content-Type": "application/json" // Asegúrate de enviar los datos como JSON
            },
            body: JSON.stringify(requestBody) // Convertir el cuerpo de la solicitud a formato JSON
        });

        //console.log("Response status:", response.status);
        //console.log("Response body:", await response.text()); // Log response body for debugging

       

        // Verificar si la respuesta fue exitosa
        if (!response.ok) {
            throw new Error(`Error al completar la factura. Código: ${response.status}`);
        }

        const data = await fetchAndProcessResponse(response);
        
        
        // Log para ver los datos recibidos del servidor
        //console.log("Datos recibidos del servidor:", data);

        // Verificar si la respuesta contiene un mensaje de éxito
        if (data.mensaje) {
            // Mostrar mensaje de éxito
            const mensaje = document.getElementById('mensaje');
            mensaje.style.display = 'block';
            mensaje.classList.remove('error');
            mensaje.classList.add('success');
            mensaje.textContent = data.mensaje; // Mostrar mensaje de éxito

            // Redirigir a la página de detalles de la factura
            window.location.href = `verFactura.html?idFactura=${idFactura}`;
        } else {
            // Mostrar mensaje de error si no se completó la factura
            const mensaje = document.getElementById('mensaje');
            mensaje.style.display = 'block';
            mensaje.classList.remove('success');
            mensaje.classList.add('error');
            mensaje.textContent = 'Hubo un error al completar la factura: ' + (data.error || "Error desconocido");
        }
    } catch (error) {
        // Capturar cualquier error y mostrar un mensaje de error
        console.error("Error al completar la factura:", error);
        const mensaje = document.getElementById('mensaje');
        mensaje.style.display = 'block';
        mensaje.classList.remove('success');
        mensaje.classList.add('error');
        mensaje.textContent = `Hubo un error al completar la factura: ${error.message}`;
    }
}

   

