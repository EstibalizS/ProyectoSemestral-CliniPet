document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("formRegistrarMascota");
    const especieInput = document.getElementById("especie");
    const razaSelect = document.getElementById("raza");
    const nombreInput = document.getElementById("nombreMascota");
    const pesoInput = document.getElementById("peso");
    const edadInput = document.getElementById("edad");
    const cedulaInput = document.getElementById("cedulaCliente");
    const generoSelect = document.getElementById("genero");
    const fotoInput = document.getElementById("foto");
    const responseMessage = document.getElementById("responseMessage"); 

    // Cargar razas según la especie seleccionada (Perro o Gato)
    especieInput.addEventListener("change", async () => {
        const especieId = especieInput.value.trim();
        let url = "";

    
        if (especieId === "Perro") {
            url = "https://localhost:7049/api/RegistrarMascota/razas/1"; // Perro
        } else if (especieId === "Gato") {
            url = "https://localhost:7049/api/RegistrarMascota/razas/2"; // Gato
        } else {
            razaSelect.innerHTML = '<option value="">Especie no válida</option>';
            console.error("Especie no válida:", especieId);
            return;
        }

        try {
            console.log(`Cargando razas desde: ${url}`);
            const response = await fetch(url);

            if (!response.ok) {
                throw new Error(`Error al cargar razas: ${response.statusText}`);
            }

            const razas = await response.json();
            console.log("Razas recibidas:", razas);

            // Verifica que las razas no estén vacías
            if (razas.length > 0) {
                razaSelect.innerHTML = '<option value="">Seleccione una raza</option>';
                razas.forEach((raza) => {
                    const option = document.createElement("option");
                    option.value = raza.razaID;  
                    option.textContent = raza.nombre;
                    razaSelect.appendChild(option);
                });
            } else {
                razaSelect.innerHTML = '<option value="">No se encontraron razas</option>';
            }
        } catch (error) {
            console.error("Error al cargar razas:", error);
            razaSelect.innerHTML = '<option value="">Error al cargar razas</option>';
        }
    });

    // Manejo del formulario para registrar la mascota
    form.addEventListener("submit", async (event) => {
        event.preventDefault();

        // Validar campos
        if (!especieInput.value || !razaSelect.value || !nombreInput.value || !pesoInput.value || !edadInput.value || !cedulaInput.value || !generoSelect.value || !fotoInput.files[0]) {
            alert("Por favor complete todos los campos.");
            return;
        }

        // Validar tipo de archivo de la foto
        const foto = fotoInput.files[0];
        if (foto && !foto.type.startsWith('image/')) {
            alert("El archivo de foto debe ser una imagen.");
            return;
        }

        // Convertir la especie seleccionada en número a 'Perro' o 'Gato'
        let especieSeleccionada = "";
        if (especieInput.value === "Perro") {
            especieSeleccionada = "Perro";
        } else if (especieInput.value === "Gato") {
            especieSeleccionada = "Gato";
        } else {
            alert("Especie seleccionada no válida.");
            return;
        }

        const formData = new FormData();
        formData.append("nombreMascota", nombreInput.value);
        formData.append("especie", especieSeleccionada);  // Se convierte a 'Perro' o 'Gato' antes de enviar
        formData.append("peso", pesoInput.value);
        formData.append("edad", edadInput.value);
        formData.append("cedulaCliente", cedulaInput.value);
        formData.append("razaID", razaSelect.value);
        formData.append("genero", generoSelect.value);
        formData.append("foto", fotoInput.files[0]);

        console.log("Datos que se van a enviar:", Object.fromEntries(formData));

        try {
            const response = await fetch("https://localhost:7049/api/RegistrarMascota/registrar", {
                method: "POST",
                body: formData,
            });

            if (!response.ok) {
                const errorDetails = await response.text(); // Capturamos los detalles del error del servidor
                throw new Error(`Error al registrar la mascota: ${errorDetails}`);
            }

            const data = await response.json();
            if (data.mensaje) {
                // Mostrar mensaje de éxito
                responseMessage.innerText = "Mascota registrada con éxito.";
                responseMessage.style.color = "green";
                form.reset();  // Resetear el formulario después de registrar
            } else {
                throw new Error("No se pudo registrar la mascota.");
            }
        } catch (error) {
            console.error("Error al registrar la mascota:", error);
            // Mostrar mensaje de error
            responseMessage.innerText = `Error al registrar la mascota: ${error.message}`;
            responseMessage.style.color = "red";
        }
    });
});
