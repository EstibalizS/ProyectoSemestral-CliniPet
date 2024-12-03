---------------FINAL VERSION------------
-- Creación de la base de datos jerhgkeurjhg
CREATE DATABASE CliniPet;

-------------------------------------
-- Creación de las tablas
CREATE TABLE Cliente (
    Cedula NVARCHAR(20) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Teléfono NVARCHAR(15),
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Dirección NVARCHAR(255) NOT NULL,
    CantidadDeMascotas INT CHECK (CantidadDeMascotas <= 2)
);

----Creación del cliente dummy para el caso de clientes no registrados
INSERT INTO Cliente (Cedula, Nombre, Teléfono, Email, Dirección, CantidadDeMascotas)
VALUES ('---', 'Contado', '---', '---', '---', 0);


CREATE TABLE Especie (
    EspecieID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL
);

-- Inserción de las especies
INSERT INTO Especie (Nombre) VALUES
('Perro'), 
('Gato');

SELECT * FROM Especie;


CREATE TABLE Raza (
    RazaID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    EspecieID INT,
    FOREIGN KEY (EspecieID) REFERENCES Especie(EspecieID)
);

-- Inserción de razas de perros
INSERT INTO Raza (Nombre, EspecieID) VALUES
('Labrador Retriever', 1), 
('Golden Retriever', 1),
('Bulldog', 1),
('Beagle', 1),
('Poodle', 1),
('Rottweiler', 1),
('German Shepherd', 1),
('Dachshund', 1),
('Chihuahua', 1),
('Boxer', 1),
('Doberman Pinscher', 1),
('Schnauzer', 1),
('Yorkshire Terrier', 1),
('Shih Tzu', 1),
('Cocker Spaniel', 1),
('Pug', 1),
('Basset Hound', 1),
('Maltese', 1),
('French Bulldog', 1),
('Collie', 1),
('Chow Chow', 1),
('Airedale Terrier', 1),
('Bernese Mountain Dog', 1),
('Great Dane', 1),
('Australian Shepherd', 1),
('Border Collie', 1),
('Cavalier King Charles Spaniel', 1),
('Husky Siberiano', 1),
('Saint Bernard', 1),
('Pit Bull Terrier', 1),
('Italian Greyhound', 1),
('Bull Terrier', 1),
('Akita', 1),
('Shiba Inu', 1),
('Cairn Terrier', 1),
('Basenji', 1),
('Cocker Spaniel Inglés', 1),
('Weimaraner', 1),
('Irish Wolfhound', 1),
('Jack Russell Terrier', 1),
('Newfoundland', 1),
('Samoyed', 1),
('Australian Cattle Dog', 1),
('American Staffordshire Terrier', 1),
('Tibetan Mastiff', 1),
('Criollo', 1); 

-- Inserción de razas de gatos
INSERT INTO Raza (Nombre, EspecieID) VALUES
('Siamés', 2),  
('Persa', 2),
('Maine Coon', 2),
('Ragdoll', 2),
('Bengal', 2),
('British Shorthair', 2),
('Sphynx', 2),
('Abyssinian', 2),
('Birmano', 2),
('Exótico de Pelo Corto', 2),
('Oriental', 2),
('Scottish Fold', 2),
('Burmese', 2),
('Norwegian Forest', 2),
('Russian Blue', 2),
('American Shorthair', 2),
('Savannah', 2),
('Himalayo', 2),
('Chartreux', 2),
('Manx', 2),
('Devon Rex', 2),
('Cornish Rex', 2),
('Turkish Van', 2),
('Singapura', 2),
('Tonkinese', 2),
('Egyptian Mau', 2),
('Munchkin', 2),
('British Longhair', 2),
('Japanese Bobtail', 2),
('LaPerm', 2),
('Turkish Angora', 2),
('Bombay', 2),
('Somali', 2),
('Oriental Longhair', 2),
('Balinese', 2),
('Cymric', 2),
('Korat', 2),
('American Curl', 2);

select*from Raza where Nombre = 'Criollo'
CREATE TABLE Mascota (
    IDMascota INT IDENTITY(10000, 1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Especie NVARCHAR(20) NOT NULL CHECK (Especie IN ('Gato', 'Perro', '-')),
    Peso DECIMAL(5,2) NOT NULL,
    Edad NVARCHAR(30) NOT NULL, -- Almacena la edad en formato año, dias, meses lo que se necesite porque si declaro por meses, año, dias solo pensé en ponerlos separados, asi que mejor que sea una cadena 
    FechaRegistro DATETIME DEFAULT GETDATE(),
    CedulaCliente NVARCHAR(20) NOT NULL FOREIGN KEY REFERENCES Cliente(Cedula),
    RazaID INT FOREIGN KEY (RazaID) REFERENCES Raza(RazaID),
	Genero NVARCHAR(10) NOT NULL CHECK (Genero IN ('Macho', 'Hembra', '-')),
	Foto VARBINARY(MAX),
);
--- Hacer este insert  primero,  seleccionen todo  hasta el paso 3 gracias, atte la gerencia :)
-- 1. Activar IDENTITY_INSERT para la tabla Mascota
SET IDENTITY_INSERT Mascota ON;
--2
INSERT INTO Mascota (IDMascota,Nombre,Especie, Peso, Edad, CedulaCliente,Genero)
VALUES(0, '---', '-', 0, '-', '---', '-')
--3
SET IDENTITY_INSERT Mascota OFF;


CREATE TABLE Servicio_Producto (
    IDITEM INT IDENTITY(0100,1) PRIMARY KEY,
    NombreProducto NVARCHAR(100) NOT NULL,
    Tipo NVARCHAR(50) NOT NULL CHECK (Tipo IN ('Servicio', 'Producto')),
    PrecioITEM MONEY NOT NULL
);

-- Inserción de Servicios
INSERT INTO Servicio_Producto (NombreProducto, Tipo, PrecioITEM)
VALUES
('Consulta Veterinaria General', 'Servicio', 30.00),
('Consulta Especializada', 'Servicio', 50.00),
('Vacunación Antirrábica', 'Servicio', 20.00),
('Vacunación Triple Felina', 'Servicio', 25.00),
('Vacunación Polivalente Canina', 'Servicio', 35.00),
('Desparasitación Interna', 'Servicio', 15.00),
('Desparasitación Externa', 'Servicio', 20.00),
('Limpieza Dental Básica', 'Servicio', 40.00),
('Limpieza Dental Completa', 'Servicio', 60.00),
('Baño Antipulgas', 'Servicio', 25.00),
('Corte de Uñas', 'Servicio', 10.00),
('Corte de Pelo Estándar', 'Servicio', 30.00),
('Corte de Pelo Estilizado', 'Servicio', 50.00),
('Microchip e Identificación', 'Servicio', 45.00),
('Consulta de Emergencia', 'Servicio', 70.00),
('Radiografía', 'Servicio', 80.00),
('Ultrasonido', 'Servicio', 120.00),
('Hospitalización (por día)', 'Servicio', 150.00),
('Cirugía General', 'Servicio', 500.00),
('Cirugía Especializada', 'Servicio', 1200.00),
('Terapia Física para Mascotas', 'Servicio', 60.00),
('Asesoramiento Nutricional', 'Servicio', 35.00),
('Consulta Dermatológica', 'Servicio', 45.00),
('Consulta Cardiológica', 'Servicio', 60.00),
('Consulta de Comportamiento', 'Servicio', 55.00);

-- Inserción de Productos
INSERT INTO Servicio_Producto (NombreProducto, Tipo, PrecioITEM)
VALUES
('Alimento para Perros (15kg)', 'Producto', 65.00),
('Alimento para Gatos (10kg)', 'Producto', 50.00),
('Arena Sanitaria para Gatos', 'Producto', 25.00),
('Juguete de Cuerda para Perros', 'Producto', 15.00),
('Pelota de Goma para Mascotas', 'Producto', 10.00),
('Collar Antipulgas para Perros', 'Producto', 18.99),
('Collar Antipulgas para Gatos', 'Producto', 16.99),
('Champú Antipulgas', 'Producto', 12.50),
('Champú Hipoalergénico', 'Producto', 14.00),
('Cepillo para Mascotas', 'Producto', 8.00),
('Cama para Perros', 'Producto', 45.00),
('Cama para Gatos', 'Producto', 40.00),
('Transportadora Pequeña', 'Producto', 50.00),
('Transportadora Mediana', 'Producto', 65.00),
('Transportadora Grande', 'Producto', 80.00),
('Rascador para Gatos', 'Producto', 70.00),
('Plato de Comida Antideslizante', 'Producto', 12.00),
('Plato Doble para Mascotas', 'Producto', 15.00),
('Correa Retráctil para Perros', 'Producto', 20.00),
('Arnés para Perros', 'Producto', 25.00),
('Arnés para Gatos', 'Producto', 18.00),
('Kit de Cepillos Dentales', 'Producto', 10.00),
('Comida Húmeda para Perros (6 latas)', 'Producto', 18.00),
('Comida Húmeda para Gatos (6 latas)', 'Producto', 16.00),
('Snacks Dentales para Perros', 'Producto', 12.00),
('Snacks para Gatos', 'Producto', 10.00);

--------------------------------------------
CREATE TABLE Factura (
    IDFactura INT IDENTITY(01000,1) PRIMARY KEY,
	CedulaCliente NVARCHAR(20) NOT NULL FOREIGN KEY REFERENCES Cliente(Cedula),
	IDMascota INT NULL FOREIGN KEY REFERENCES Mascota(IDMascota),
    Fecha DATETIME DEFAULT GETDATE(),
	subtotalf DECIMAL(10, 2) NULL,    -- quedan como null para indicar que aun no has sido calculados 
	ITBMSFactura DECIMAL (10, 2) NULL,
	totalFactura DECIMAL (10, 2) NULL
);

CREATE TABLE Venta (
    IDVenta INT IDENTITY(01,1) PRIMARY KEY,
	IDFactura INT NOT NULL FOREIGN KEY REFERENCES Factura(IDFactura),
    IDITEM INT NOT NULL FOREIGN KEY REFERENCES Servicio_Producto(IDITEM),
    CantidadVendida INT NOT NULL CHECK (CantidadVendida > 0),
	PrecioBruto MONEY NOT NULL,
    ITBMSLinea MONEY NOT NULL, 
    totalLinea MONEY NOT NULL
);


CREATE TABLE Inventario (
	IDInventario INT IDENTITY (010,1),
	IDVenta INT NULL FOREIGN KEY REFERENCES Venta(IDVenta),
	IDITEM INT NOT NULL FOREIGN KEY REFERENCES Servicio_Producto(IDITEM),
	EntradaInventario INT NOT NULL CHECK (EntradaInventario >= 0),
    SalidaInventario INT NOT NULL DEFAULT 0 CHECK (SalidaInventario >= 0),
	CantidadDisponible INT NOT NULL DEFAULT 0
);

CREATE TABLE MascotaCodigoQR (
    ID INT PRIMARY KEY,
	IDMascota INT,
    CodigoQR VARBINARY(MAX), 
    FechaGeneracion DATETIME DEFAULT GETDATE(),
	FOREIGN KEY (IDMascota) REFERENCES Mascota(IDMascota)
);

------------------------Procedimientos Almacenado----------------------------
--Procedimiento para registrar al cliente
 
CREATE PROCEDURE RegistrarCliente
    @Cedula NVARCHAR(20),
    @Nombre NVARCHAR(100),
    @Teléfono NVARCHAR(15),
    @Email NVARCHAR(100),
    @Dirección NVARCHAR(255),
    @CantidadDeMascotas INT -- Añadir parámetro para la cantidad de mascotas
AS
BEGIN
    -- Validar el formato de la cédula
    IF NOT (
        @Cedula LIKE '[1-9]%'        -- Provincias de 1 a 9
        OR @Cedula LIKE '10-%'        -- Provincia 10
        OR @Cedula LIKE 'E-%'         -- Extranjeros
        OR @Cedula LIKE '[A-Z][0-9]%' -- Pasaportes
    )
    BEGIN
        -- Mensaje de depuración
        PRINT 'Valor de cédula ingresado: ' + @Cedula;
        RAISERROR ('El formato de la cédula no es válido.', 16, 1);
        RETURN;
    END;

    -- Verificar si la cédula ya existe
    IF EXISTS (SELECT 1 FROM Cliente WHERE Cedula = @Cedula)
    BEGIN
        RAISERROR ('La cédula ya está registrada. No se puede usar para otro cliente.', 16, 1);
        RETURN;
    END;

    -- Validar el formato del email
    IF NOT (@Email LIKE '%@%.%')
    BEGIN
        RAISERROR ('El formato del email no es válido.', 16, 1);
        RETURN;
    END;

    -- Validar que la cantidad de mascotas no sea mayor a 2 y sea un número positivo
    IF @CantidadDeMascotas < 0 OR @CantidadDeMascotas > 2
    BEGIN
        RAISERROR ('La cantidad de mascotas debe ser un número positivo y no puede ser mayor a 2.', 16, 1);
        RETURN;
    END;

    -- Insertar el nuevo cliente con la cantidad de mascotas
    INSERT INTO Cliente (Cedula, Nombre, Teléfono, Email, Dirección, CantidadDeMascotas)
    VALUES (@Cedula, @Nombre, @Teléfono, @Email, @Dirección, @CantidadDeMascotas);
END;

--------------------------------------------------------
--Procedimiento para registrar Mascota
drop procedure RegistrarMascota 
CREATE PROCEDURE RegistrarMascota
    @Nombre NVARCHAR(50),
    @Especie NVARCHAR(20),
    @Peso DECIMAL(5,2),
    @Edad NVARCHAR(30), 
    @CedulaCliente NVARCHAR(20),
    @RazaID INT,
    @Genero NVARCHAR(10),
    @Foto VARBINARY(MAX) = NULL
AS
BEGIN
    -- Verificar que el cliente exista en la base de datos
    IF NOT EXISTS (SELECT 1 FROM Cliente WHERE Cedula = @CedulaCliente)
    BEGIN
        RAISERROR ('El cliente no existe.', 16, 1);
        RETURN;
    END;

    -- Verificar que el cliente tiene menos de 2 mascotas registradas
    IF (SELECT COUNT(*) FROM Mascota WHERE CedulaCliente = @CedulaCliente) >= 2
    BEGIN
        RAISERROR ('El cliente ya tiene 2 mascotas registradas.', 16, 1);
        RETURN;
    END;

    -- Validar que el peso y la edad sean positivos
    IF @Peso <= 0
    BEGIN
        RAISERROR ('El peso debe ser mayor a cero.', 16, 1);
        RETURN;
    END;

    IF CAST(@Edad AS INT) <= 0
    BEGIN
        RAISERROR ('La edad debe ser mayor a cero.', 16, 1);
        RETURN;
    END;

    -- Verificar que la raza proporcionada es válida
    IF NOT EXISTS (SELECT 1 FROM Raza WHERE RazaID = @RazaID)
    BEGIN
        RAISERROR ('La raza proporcionada no es válida.', 16, 1);
        RETURN;
    END;

    -- Registrar la mascota
    INSERT INTO Mascota (Nombre, Especie, Peso, Edad, CedulaCliente, RazaID, Genero, Foto)
    VALUES (@Nombre, @Especie, @Peso, @Edad, @CedulaCliente, @RazaID, @Genero, @Foto);
--Actualizar la cantidad de mascotas del cliente
    UPDATE Cliente
    SET CantidadDeMascotas = CantidadDeMascotas + 1
    WHERE Cedula = @CedulaCliente;
	END;
select * from Mascota
select*From Cliente

-------------------------------------------------------------------------------------
-- procedimiento para consultar la mascota y el cliente

CREATE PROCEDURE ConsultarClienteYMascota
    @Cedula NVARCHAR(20) = NULL,  -- Define la longitud del NVARCHAR
    @IDMascota INT = NULL
AS
BEGIN
    SET NOCOUNT ON;  -- Evita que se devuelvan mensajes de conteo de filas afectadas

    SELECT 
        c.Cedula AS CedulaCliente,
        c.Nombre AS NombreCliente,
        c.Teléfono,
        c.Email,
        c.Dirección,
        c.CantidadDeMascotas,
        m.IDMascota,
        m.Nombre AS NombreMascota,
        m.Especie,
        m.Peso,
        m.Edad,
        m.Genero, 
        m.FechaRegistro,
        r.Nombre AS RazaMascota,
        m.Foto
    FROM 
        Cliente c
    LEFT JOIN 
        Mascota m ON c.Cedula = m.CedulaCliente
    LEFT JOIN 
        Raza r ON m.RazaID = r.RazaID
    WHERE 
        (@Cedula IS NULL OR c.Cedula = @Cedula)  -- Filtra por cédula si se proporciona
        AND (@IDMascota IS NULL OR m.IDMascota = @IDMascota);  -- Filtra por ID de mascota si se proporciona
END;

----------------------------------------------------------------------------------------------------------------
--procedimieto de actualizar registro de cliente

CREATE PROCEDURE ActualizarCliente
    @Cedula NVARCHAR(20),
    @Telefono NVARCHAR(15),
    @Email NVARCHAR(100),
    @Direccion NVARCHAR(255)
AS
BEGIN
    -- Validar que la cédula no esté vacía
    IF @Cedula IS NULL OR @Cedula = ''
    BEGIN
        RAISERROR ('La cédula no puede estar vacía.', 16, 1);
        RETURN;
    END;

    -- Validar el formato de la cédula
    IF NOT (
        @Cedula LIKE '[1-9]%'          -- Provincias de 1 a 9
        OR @Cedula LIKE '10-%'          -- Provincia 10
        OR @Cedula LIKE 'E-%'           -- Extranjeros
        OR @Cedula LIKE '[A-Z][0-9]%'   -- Pasaportes
    )
    BEGIN
        RAISERROR ('El formato de la cédula no es válido.', 16, 1);
        RETURN;
    END;

    -- Verificar si la cédula existe en la base de datos
    IF NOT EXISTS (SELECT 1 FROM Cliente WHERE Cedula = @Cedula)
    BEGIN
        RAISERROR ('La cédula no existe en la base de datos.', 16, 1);
        RETURN;
    END;

    -- Validar el formato del teléfono (asegurarse que sea solo números)
    IF @Telefono IS NOT NULL AND @Telefono <> '' AND NOT @Telefono LIKE '[0-9]%' 
    BEGIN
        RAISERROR ('El formato del teléfono no es válido. Debe contener solo números.', 16, 1);
        RETURN;
    END;

    -- Validar el formato del correo electrónico (simple verificación de '@' y '.')
    IF @Email IS NOT NULL AND @Email <> '' AND NOT (@Email LIKE '%@%.%')
    BEGIN
        RAISERROR ('El formato del correo electrónico no es válido.', 16, 1);
        RETURN;
    END;

    -- Actualizar la información del cliente
    UPDATE Cliente
    SET Teléfono = @Telefono,
        Email = @Email,
        Dirección = @Direccion
    WHERE Cedula = @Cedula;

    PRINT 'Información del cliente actualizada exitosamente.';
END;
-----------------------------------------------------------------
--procedimiento para actualizar el registro de la mascota
drop procedure ActualizarMascota
CREATE PROCEDURE ActualizarMascota
    @IDMascota INT, 
    @NuevoPeso DECIMAL(5,2), 
    @NuevaEdad NVARCHAR(50)  
AS
BEGIN
    -- Evitar el conteo de filas afectadas
    SET NOCOUNT ON;

    -- Validar que el ID de mascota sea positivo y que exista
    IF @IDMascota IS NULL OR @IDMascota <= 0
    BEGIN
        RAISERROR ('El ID de la mascota debe ser un valor positivo.', 16, 1);
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM Mascota WHERE IDMascota = @IDMascota)
    BEGIN
        RAISERROR ('La mascota no existe.', 16, 1);
        RETURN;
    END;

    -- Validar que el peso sea positivo
    IF @NuevoPeso <= 0
    BEGIN
        RAISERROR ('El peso debe ser un número positivo o mayor que 0', 16, 1);
        RETURN;
    END;

    -- Validar que la nueva edad sea un número y mayor que la edad actual
    DECLARE @EdadActual NVARCHAR;
    SELECT @EdadActual = Edad FROM Mascota WHERE IDMascota = @IDMascota;

    -- Intentar convertir la nueva edad a INT
    DECLARE @NuevaEdadInt INT;
    BEGIN TRY
        SET @NuevaEdadInt = CAST(@NuevaEdad AS INT);
    END TRY
    BEGIN CATCH
        RAISERROR ('La nueva edad debe ser un número entero', 16, 1);
        RETURN;
    END CATCH;

    IF @NuevaEdadInt <= @EdadActual
    BEGIN
        RAISERROR ('La nueva edad debe ser mayor que la edad actual', 16, 1);
        RETURN;
    END;

    -- Actualizar los datos de la mascota
    UPDATE Mascota
    SET Peso = @NuevoPeso,
        Edad = @NuevaEdad  -- Almacena la nueva edad como texto
    WHERE IDMascota = @IDMascota;

    PRINT 'Información de la mascota actualizada exitosamente.';
END;

select*from Cliente
------------------------------------------------------------------

-- Procedimiento para generar la factura del servicio prestado
CREATE PROCEDURE GenerarFactura
    @CedulaCliente NVARCHAR(20),
    @IDMascota INT = NULL -- Opcional si la factura no es específica de una mascota
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validar existencia del cliente
        IF NOT EXISTS (SELECT 1 FROM Cliente WHERE Cedula = @CedulaCliente)
        BEGIN
            RAISERROR ('El cliente no existe.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Insertar la factura
        INSERT INTO Factura (IDMascota, CedulaCliente, Fecha)
        VALUES (@IDMascota, @CedulaCliente, GETDATE());

        -- Obtener el ID de la factura recién insertada
        DECLARE @IDFactura INT = SCOPE_IDENTITY();

        -- Confirmar transacción
        COMMIT TRANSACTION;

        PRINT 'Factura generada exitosamente.';
        SELECT @IDFactura AS IDFactura; -- Devolver el ID de la factura para poder almacenar en tabla venta
    END TRY
    BEGIN CATCH
        -- Manejo de errores
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

-------------------------------------------------------------------------------------------------
---procedimiento de actualizar inventario

CREATE PROCEDURE ActualizarInventario
    @IDITEM INT,
    @CantidadAgregada INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Verificar que el producto exista
        IF NOT EXISTS (SELECT 1 FROM Servicio_Producto WHERE IDITEM = @IDITEM AND Tipo = 'Producto')
        BEGIN
            RAISERROR ('El producto no existe.', 16, 1);
            RETURN;
        END

        -- Obtener la última cantidad disponible
        DECLARE @CantidadDisponibleAnterior INT;
        SELECT TOP 1 @CantidadDisponibleAnterior = CantidadDisponible 
        FROM Inventario 
        WHERE IDITEM = @IDITEM
        ORDER BY IDInventario DESC; -- Aseguramos que obtenemos el último registro por IDInventario

        -- Calcular la nueva cantidad disponible
        DECLARE @NuevaCantidadDisponible INT = ISNULL(@CantidadDisponibleAnterior, 0) + @CantidadAgregada;

        -- Insertar nuevo registro en inventario
        INSERT INTO Inventario (IDITEM, EntradaInventario, SalidaInventario, CantidadDisponible)
        VALUES (@IDITEM, @CantidadAgregada, 0, @NuevaCantidadDisponible);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;


-----------------------------------------------------------------------------------------------
--Procedimiento almacenado para la compra de producto

CREATE PROCEDURE ComprarProducto
  @IDITEM INT,
    @Cantidad INT,
    @IDFactura INT -- Asociar la compra a una factura
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Verificar que el producto exista
        IF NOT EXISTS (SELECT 1 FROM Servicio_Producto WHERE IDITEM = @IDITEM AND Tipo = 'Producto')
        BEGIN
            RAISERROR ('El producto no existe.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Verificar que la factura exista
        IF NOT EXISTS (SELECT 1 FROM Factura WHERE IDFactura = @IDFactura)
        BEGIN
            RAISERROR ('La factura no existe.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Verificar la cantidad disponible en inventario
        DECLARE @CantidadDisponible INT;
        SELECT @CantidadDisponible = CantidadDisponible
        FROM Inventario 
        WHERE IDITEM = @IDITEM;



        IF @CantidadDisponible < @Cantidad
        BEGIN
            RAISERROR ('Cantidad insuficiente en inventario.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Obtener el precio del producto desde la tabla Servicio_Producto
        DECLARE @PrecioUnitario MONEY;
        SELECT @PrecioUnitario = PrecioITEM FROM Servicio_Producto WHERE IDITEM = @IDITEM;

        -- Validar que se encontró el precio del producto
        IF @PrecioUnitario IS NULL
        BEGIN
            RAISERROR ('No se encontró el precio del producto.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Calcular el ITBMS y el total de la línea
        DECLARE @ITBMSLinea MONEY = @PrecioUnitario * @Cantidad * 0.07;
        DECLARE @TotalLinea MONEY = @PrecioUnitario * @Cantidad + @ITBMSLinea;

        -- Insertar la venta
        INSERT INTO Venta (IDFactura, IDITEM, CantidadVendida, PrecioBruto, ITBMSLinea, TotalLinea)
        VALUES (@IDFactura, @IDITEM, @Cantidad, @PrecioUnitario * @Cantidad, @ITBMSLinea, @TotalLinea);

        -- Actualizar el inventario
        INSERT INTO Inventario (IDITEM, EntradaInventario, SalidaInventario, CantidadDisponible, IDVenta)
        VALUES (
            @IDITEM, 
            0, -- porque no ehay entrada
            @Cantidad, -- Cantidad vendida
            @CantidadDisponible - @Cantidad, -- Nueva cantidad disponible
            SCOPE_IDENTITY() -- ID de la venta recién creada
        );

        -- Confirmar la transacción
        COMMIT TRANSACTION;

        PRINT 'Compra de producto registrada exitosamente.';
    END TRY
    BEGIN CATCH
        -- Manejar errores y deshacer cambios
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

--------------------------------------------------------------------------------------------------------------------------
--Procedimiento almacenado para el registro del servicio realizado a la mascota

CREATE PROCEDURE RegistrarServicioMascota
    @IDMascota INT,
    @IDITEM INT,
    @IDFactura INT -- Se agrega este parámetro para asociar la venta a la factura
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Verificar que la mascota existe
        IF NOT EXISTS (SELECT 1 FROM Mascota WHERE IDMascota = @IDMascota)
        BEGIN
            RAISERROR ('La mascota no existe.', 16, 1);
            RETURN;
        END

        -- Verificar que el servicio existe
        IF NOT EXISTS (SELECT 1 FROM Servicio_Producto WHERE IDITEM = @IDITEM AND Tipo = 'Servicio')
        BEGIN
            RAISERROR ('El servicio no existe.', 16, 1);
            RETURN;
        END

        -- Verificar que la factura existe
        IF NOT EXISTS (SELECT 1 FROM Factura WHERE IDFactura = @IDFactura)
        BEGIN
            RAISERROR ('La factura no existe.', 16, 1);
            RETURN;
        END

        -- Obtener precio del servicio
        DECLARE @PrecioServicio MONEY = (SELECT PrecioITEM FROM Servicio_Producto WHERE IDITEM = @IDITEM);
        DECLARE @ITBMSLinea MONEY = @PrecioServicio * 0.07;
        DECLARE @totalLinea MONEY = @PrecioServicio + @ITBMSLinea;

        -- Registrar la venta asociada a la factura
        INSERT INTO Venta (IDFactura, IDITEM, CantidadVendida, PrecioBruto, ITBMSLinea, totalLinea)
        VALUES (@IDFactura, @IDITEM, 1, @PrecioServicio, @ITBMSLinea, @totalLinea);

        COMMIT TRANSACTION;

        PRINT 'Servicio registrado exitosamente.';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
----------------------------------------------------------------------
------------procedimiento para Completar la factura
CREATE PROCEDURE CompletarFactura
    @IDFactura INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Calcular el subtotal sumando todas las líneas de la factura
        DECLARE @Subtotal DECIMAL(10, 2);
        SELECT @Subtotal = SUM(PrecioBruto)
        FROM Venta
        WHERE IDFactura = @IDFactura;

        -- Calcular el ITBMS (7% del subtotal)
        DECLARE @ITBMS DECIMAL(10, 2) = @Subtotal * 0.07;

        -- Calcular el total (subtotal + ITBMS)
        DECLARE @Total DECIMAL(10, 2) = @Subtotal + @ITBMS;

        -- Actualizar los campos de la factura
        UPDATE Factura
        SET 
            subtotalf = @Subtotal,
            ITBMSFactura = @ITBMS,
            totalFactura = @Total
        WHERE IDFactura = @IDFactura;

        COMMIT TRANSACTION;

        PRINT 'Factura completada correctamente.';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

select*from Raza
select* from Factura
select* from Servicio_Producto where Tipo = 'Servicio'
--------------------------Para los reportes------------------------------------------------------------------------------------------------

--Procedimiento almacenado que obtiene el producto mas vendido

CREATE PROCEDURE ObtenerProductosMasVendidos
AS
BEGIN
    WITH ProductosVendidos AS (
        SELECT 
            SP.NombreProducto, 
            SUM(V.CantidadVendida) AS TotalVendido,
            ROW_NUMBER() OVER (ORDER BY SUM(V.CantidadVendida) DESC) AS RowNum
        FROM Venta V
        INNER JOIN Servicio_Producto SP ON V.IDITEM = SP.IDITEM
        WHERE SP.Tipo = 'Producto'
        GROUP BY SP.NombreProducto
    )
    SELECT NombreProducto, TotalVendido
    FROM ProductosVendidos
    WHERE RowNum <= 5; -- Top 5 productos más vendidos
END;


--------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE ObtenerServicioMasSolicitado
AS
BEGIN
    WITH ServiciosSolicitados AS (
        SELECT 
            SP.NombreProducto AS NombreServicio, 
            COUNT(V.IDVenta) AS TotalSolicitado,
            ROW_NUMBER() OVER (ORDER BY COUNT(V.IDVenta) DESC) AS RowNum
        FROM Venta V
        INNER JOIN Servicio_Producto SP ON V.IDITEM = SP.IDITEM
        WHERE SP.Tipo = 'Servicio'
        GROUP BY SP.NombreProducto
    )
    SELECT NombreServicio, TotalSolicitado
    FROM ServiciosSolicitados
    WHERE RowNum = 1; -- Servicio más solicitado
END;


----------------------------- data de prueba --------------------------------

INSERT INTO Cliente (Cedula, Nombre, Teléfono, Email, Dirección, CantidadDeMascotas)
VALUES 
('8-123-4567', 'Ana López', '8888-8888', 'ana.lopez@gmail.com', 'Calle Flores 123, San José', 2),
('9-234-5678', 'Carlos Martínez', '7777-7777', 'carlos.mtz@hotmail.com', 'Avenida Central, San Pedro', 1),
('7-345-6789', 'María Fernández', '6666-6666', 'maria.fernandez@yahoo.com', 'Barrio Los Olivos, Heredia', 2);


INSERT INTO Mascota (Nombre, Especie, Peso, Edad, CedulaCliente, RazaID, Genero, Foto)
VALUES 
('Rocky', 'Perro', 25.50, '5 años', '8-123-4567', 1, 'Macho', NULL), -- Labrador Retriever
('Luna', 'Gato', 4.20, '1 año', '9-234-5678', 24, 'Hembra', NULL), -- Siamés
('Max', 'Perro', 30.00, '7 años', '7-345-6789', 8, 'Macho', NULL); -- Beagle


select * from Mascota
----------------------------------------------------------------------------------------

INSERT INTO Inventario (IDITEM, EntradaInventario, SalidaInventario, CantidadDisponible)
VALUES
(125, 50, 0, 50),  -- Alimento para Perros (15kg)
(126, 40, 0, 40),  -- Alimento para Gatos (10kg)
(127, 30, 0, 30),  -- Arena Sanitaria para Gatos
(128, 20, 0, 20),  -- Juguete de Cuerda para Perros
(129, 25, 0, 25),  -- Pelota de Goma para Mascotas
(130, 15, 0, 15),  -- Collar Antipulgas para Perros
(131, 10, 0, 10),  -- Collar Antipulgas para Gatos
(132, 20, 0, 20),  -- Champú Antipulgas
(133, 15, 0, 15),  -- Champú Hipoalergénico
(134, 30, 0, 30),  -- Cepillo para Mascotas
(135, 10, 0, 10),  -- Cama para Perros
(136, 8, 0, 8),    -- Cama para Gatos
(137, 12, 0, 12),  -- Transportadora Pequeña
(138, 10, 0, 10),  -- Transportadora Mediana
(139, 5, 0, 5),    -- Transportadora Grande
(140, 6, 0, 6),    -- Rascador para Gatos
(141, 25, 0, 25),  -- Plato de Comida Antideslizante
(142, 20, 0, 20),  -- Plato Doble para Mascotas
(143, 15, 0, 15),  -- Correa Retráctil para Perros
(144, 18, 0, 18),  -- Arnés para Perros
(145, 12, 0, 12),  -- Arnés para Gatos
(146, 50, 0, 50),  -- Kit de Cepillos Dentales
(147, 25, 0, 25),  -- Comida Húmeda para Perros (6 latas)
(148, 30, 0, 30),  -- Comida Húmeda para Gatos (6 latas)
(149, 40, 0, 40),  -- Snacks Dentales para Perros
(150, 35, 0, 35);  -- Snacks para Gatos

---------------------------------
select*from Mascota where IDMascota = '0'

SELECT * FROM Factura
SELECT* FROM Servicio_Producto WHERE IDITEM = '126'

SELECT * FROM Cliente

SELECT * FROM Mascota

SELECT * FROM Inventario

SELECT name FROM sys.check_constraints
WHERE parent_object_id = OBJECT_ID('Mascota') AND name LIKE '%Genero%';

alter table Mascota
drop constraint CK__Mascota__Genero__440B1D61

ALTER TABLE Mascota
ADD CONSTRAINT CK_Mascota_Genero
CHECK (Genero IN ('Macho', 'Hembra', '-'))
