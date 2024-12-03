const express = require('express');
const cors = require('cors');
const app = express();

app.use(cors()); // Permite todas las solicitudes CORS
app.use(express.json());

app.put('/api/ActualizarCliente/actualizar', (req, res) => {
    // Lógica para actualizar el cliente
    res.json({ message: "Cliente actualizado correctamente." });
});

app.listen(7049, () => {
    console.log('Servidor escuchando en el puerto 7049');
});
