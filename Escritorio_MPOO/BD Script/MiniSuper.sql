USE master
GO
CREATE DATABASE MiniSuper
GO
USE MiniSuper
GO
CREATE TABLE Rol (
	IdRol INT PRIMARY KEY IDENTITY (1, 1)
   ,Descripcion VARCHAR(200) NOT NULL
   ,
	--Pistas de Auditoria
	IdUsuarioRegistro INT NOT NULL
   ,FechaRegistro DATETIME NOT NULL
   ,IdUsuarioActualiza INT NULL
   ,FechaActualizacion DATETIME NULL
   ,Activo BIT NOT NULL
)
GO
CREATE TABLE Usuarios (
	IdUsuario INT PRIMARY KEY IDENTITY (1, 1)
   ,IdRol INT FOREIGN KEY REFERENCES Rol (IdRol)
   ,NombreCompleto VARCHAR(200) NOT NULL
   ,Correo VARCHAR(200) NOT NULL
   ,Cargo VARCHAR(200) NOT NULL
   ,Login VARCHAR(200) NOT NULL UNIQUE
   ,Password VARBINARY(MAX) NOT NULL
   ,
	--Pistas de Auditoria
	IdUsuarioRegistro INT NOT NULL
   ,FechaRegistro DATETIME NOT NULL
   ,IdUsuarioActualiza INT NULL
   ,FechaActualizacion DATETIME NULL
   ,Activo BIT NOT NULL
)
GO
CREATE TABLE UnidadesMedidas (
	IdUnidadMedida INT PRIMARY KEY IDENTITY (1, 1)
   ,Descripcion VARCHAR(200) NOT NULL
   ,
	--Pistas de Auditoria
	IdUsuarioRegistro INT NOT NULL
   ,FechaRegistro DATETIME NOT NULL
   ,IdUsuarioActualiza INT NULL
   ,FechaActualizacion DATETIME NULL
   ,Activo BIT NOT NULL
)
GO
CREATE TABLE Productos (
	IdProducto INT PRIMARY KEY IDENTITY (1, 1)
   ,IdUnidadMedida INT FOREIGN KEY REFERENCES UnidadesMedidas (IdUnidadMedida)
   ,CodigoBarra INT NOT NULL
   ,Descripcion VARCHAR(200) NOT NULL
   ,PrecioUnitario DECIMAL(18, 2) NOT NULL
   ,PorcentajeUtilidad DECIMAL(18, 2) NOT NULL
   ,PorcentajeDescuento DECIMAL(18, 2) NOT NULL
   ,
	--Pistas de Auditoria
	IdUsuarioRegistro INT NOT NULL
   ,FechaRegistro DATETIME NOT NULL
   ,IdUsuarioActualiza INT NULL
   ,FechaActualizacion DATETIME NULL
   ,Activo BIT NOT NULL
)
GO
CREATE TABLE Inventario (
	IdInventario INT PRIMARY KEY IDENTITY (1, 1)
   ,IdProducto INT FOREIGN KEY REFERENCES Productos (IdProducto)
   ,Lote INT NOT NULL
   ,Cantidad INT NOT NULL
   ,FechaCaducidad DATETIME NOT NULL
   ,
	--Pistas de Auditoria
	IdUsuarioRegistro INT NOT NULL
   ,FechaRegistro DATETIME NOT NULL
   ,IdUsuarioActualiza INT NULL
   ,FechaActualizacion DATETIME NULL
   ,Activo BIT NOT NULL
)
GO
CREATE TABLE Clientes (
	IdCliente INT PRIMARY KEY IDENTITY (1, 1)
   ,NombreCompleto VARCHAR(200) NOT NULL
   ,Identificacion VARCHAR(200) NOT NULL
   ,Celular VARCHAR(8) NULL
   ,Correo VARCHAR(200) NULL
   ,
	--Pistas de Auditoria
	IdUsuarioRegistro INT NOT NULL
   ,FechaRegistro DATETIME NOT NULL
   ,IdUsuarioActualiza INT NULL
   ,FechaActualizacion DATETIME NULL
   ,Activo BIT NOT NULL
)
GO
CREATE TABLE Facturas (
	IdFactura INT PRIMARY KEY IDENTITY (1, 1)
   ,IdCliente INT FOREIGN KEY REFERENCES Clientes (IdCliente)
   ,CodigoFactura VARCHAR(100) NOT NULL
   ,FechaFactura DATETIME NOT NULL
   ,TotalSinIVA DECIMAL(18, 2) NOT NULL
   ,Descuento DECIMAL(18, 2) NOT NULL
   ,IVA DECIMAL(18, 2) NOT NULL
   ,TotalPagar DECIMAL(18, 2) NOT NULL
   ,
	--Pistas de Auditoria
	IdUsuarioRegistro INT NOT NULL
   ,FechaRegistro DATETIME NOT NULL
   ,IdUsuarioActualiza INT NULL
   ,FechaActualizacion DATETIME NULL
   ,Activo BIT NOT NULL
)
GO
CREATE TABLE DetallesFacturas (
	IdDetalleFactura INT PRIMARY KEY IDENTITY (1, 1)
   ,IdProducto INT FOREIGN KEY REFERENCES Productos (IdProducto)
   ,IdFactura INT FOREIGN KEY REFERENCES Facturas (IdFactura)
   ,PrecioUnitario DECIMAL(18, 2) NOT NULL
   ,Cantidad DECIMAL(18, 2) NOT NULL
   ,TotalSinIVA DECIMAL(18, 2) NOT NULL
   ,IVA DECIMAL(18, 2) NOT NULL
   ,Descuento DECIMAL(18, 2) NOT NULL
   ,TotalPagar DECIMAL(18, 2) NOT NULL
   ,
	--Pistas de Auditoria
	IdUsuarioRegistro INT NOT NULL
   ,FechaRegistro DATETIME NOT NULL
   ,IdUsuarioActualiza INT NULL
   ,FechaActualizacion DATETIME NULL
   ,Activo BIT NOT NULL
)
GO
--Procedimientos almacenados (stored procedures)
CREATE PROC InsertarUsuario @IdRol INT,
@NombreCompleto VARCHAR(200),
@Correo VARCHAR(200),
@Cargo VARCHAR(200),
@Login VARCHAR(200),
@Password VARBINARY(MAX),
@IdUsuarioRegistro INT
AS
BEGIN
	INSERT INTO Usuarios (IdRol, NombreCompleto, Correo, Cargo, Login, Password, IdUsuarioRegistro, FechaRegistro, Activo)
		VALUES (@IdRol, @NombreCompleto, @Correo, @Cargo, @Login, @Password, @IdUsuarioRegistro, GETDATE(), 1)
	SELECT
		SCOPE_IDENTITY() AS ID;
END
GO
CREATE PROC ActualizarUsuario @IdUsuario INT,
@IdRol INT,
@NombreCompleto VARCHAR(200),
@Correo VARCHAR(200),
@Cargo VARCHAR(200),
@Login VARCHAR(200),
@Password VARBINARY(MAX) = NULL,
@IdUsuarioActualiza INT
AS
BEGIN

	IF (ISNULL(@Password, 0) = 0)
	BEGIN
		UPDATE Usuarios
		SET IdRol = @IdRol
		   ,NombreCompleto = @NombreCompleto
		   ,Correo = @Correo
		   ,Cargo = @Cargo
		   ,Login = @Login
		   ,IdUsuarioActualiza = @IdUsuarioActualiza
		WHERE IdUsuario = @IdUsuario
	END
	ELSE
	BEGIN
		UPDATE Usuarios
		SET IdRol = @IdRol
		   ,NombreCompleto = @NombreCompleto
		   ,Correo = @Correo
		   ,Cargo = @Cargo
		   ,Login = @Login
		   ,Password = @Password
		   ,IdUsuarioActualiza = @IdUsuarioActualiza
		WHERE IdUsuario = @IdUsuario
	END
END
GO
CREATE PROC AnularUsuario @IdUsuario INT,
@IdUsuarioActualiza INT
AS
BEGIN
	UPDATE Usuarios
	SET Activo = 0
	   ,IdUsuarioActualiza = @IdUsuarioActualiza
	WHERE IdUsuario = @IdUsuario
END
GO
CREATE PROC ListarUsuarios 
@Todos BIT,
@IdUsuario INT = 0,
@LOGIN VARCHAR(200) = '',
@PASSWORD VARBINARY(MAX) = NULL
AS
BEGIN
	IF (@IdUsuario > 0)
	BEGIN
		SELECT
			IdUsuario
		   ,NombreCompleto
		   ,Correo
		   ,Cargo
		   ,Login
		   ,Rol.Descripcion AS Rol
		   ,Rol.IdRol
		FROM Usuarios
		INNER JOIN Rol
			ON Usuarios.IdRol = Rol.IdRol
		WHERE Usuarios.IdUsuario = @IdUsuario
		AND Usuarios.Activo = 1		
	END
	ELSE IF (@LOGIN <> '' AND ISNULL(@Password,0) = 0)
		BEGIN  
         	SELECT
			IdUsuario
		   ,NombreCompleto
		   ,Correo
		   ,Cargo
		   ,Login
		   ,Rol.Descripcion AS Rol
		   ,Rol.IdRol
		FROM Usuarios
		INNER JOIN Rol
			ON Usuarios.IdRol = Rol.IdRol
		WHERE Usuarios.Login like @LOGIN
         END        
	ELSE IF (@LOGIN <> '' AND ISNULL(@Password,0) <> 0)
		BEGIN  
         	SELECT
			IdUsuario
		   ,NombreCompleto
		   ,Correo
		   ,Cargo
		   ,Login
		   ,Rol.Descripcion AS Rol
		   ,Rol.IdRol
		FROM Usuarios
		INNER JOIN Rol
			ON Usuarios.IdRol = Rol.IdRol
		WHERE Usuarios.Login like @Login AND Password = @PASSWORD AND Usuarios.Activo = 1
         END    
	ELSE IF (@Todos = 1)
	BEGIN
		SELECT
			IdUsuario
		   ,NombreCompleto
		   ,Correo
		   ,Cargo
		   ,Login
		   ,Rol.Descripcion AS Rol
		   ,Rol.IdRol
		FROM Usuarios
		INNER JOIN Rol
			ON Usuarios.IdRol = Rol.IdRol
		WHERE Usuarios.Activo = 1
	END
END
GO
CREATE PROC InsertarUnidadMedida
@Descripcion VARCHAR(200),
@IdUsuarioRegistro INT
AS
BEGIN
	INSERT INTO UnidadesMedidas (Descripcion, IdUsuarioRegistro, FechaRegistro,Activo)
	VALUES (@Descripcion, @IdUsuarioRegistro, GETDATE(),1);
	SELECT SCOPE_IDENTITY()
END
GO
CREATE PROC ActualizarUnidadMedida
@IdUnidadMedida INT,
@Descripcion VARCHAR(200),
@IdUsuarioActualiza INT
AS
BEGIN
	UPDATE UnidadesMedidas 
SET Descripcion = @Descripcion
   ,IdUsuarioActualiza = @IdUsuarioActualiza
   ,FechaActualizacion = GETDATE()
WHERE IdUnidadMedida = @IdUnidadMedida;
END
GO
CREATE PROC AnularUnidadMedida
@IdUnidadMedida INT,
@IdUsuarioActualiza INT
AS
BEGIN
	UPDATE UnidadesMedidas 
SET IdUsuarioActualiza = @IdUsuarioActualiza
   ,FechaActualizacion = GETDATE()
   ,Activo = 0
WHERE IdUnidadMedida = @IdUnidadMedida;
END
GO
CREATE PROC ListarUnidadMedidas 
@Todos BIT,
@IdUnidadMedida INT = 0
AS
BEGIN
	IF (@Todos = 1)
	BEGIN
		SELECT IdUnidadMedida,Descripcion 
		FROM UnidadesMedidas
	END
	ELSE
	BEGIN
		SELECT IdUnidadMedida,Descripcion
		FROM UnidadesMedidas 
		WHERE IdUnidadMedida = @IdUnidadMedida
	END
END
GO
CREATE PROC InsertarRol @Descripcion VARCHAR(200),
@IdUsuarioRegistro INT
AS
BEGIN
	INSERT INTO Rol (Descripcion, IdUsuarioRegistro, FechaRegistro, Activo)
		VALUES (@Descripcion, @IdUsuarioRegistro, GETDATE(), 1);
	SELECT
		SCOPE_IDENTITY();
END
GO
CREATE PROC ActualizarRol @IdRol INT,
@Descripcion VARCHAR(200),
@IdUsuarioActualiza INT
AS
BEGIN
	UPDATE Rol
	SET Descripcion = @Descripcion
	   ,IdUsuarioActualiza = @IdUsuarioActualiza
	   ,FechaActualizacion = GETDATE()
	WHERE IdRol = @IdRol;
END
GO
CREATE PROC AnularRol @IdRol INT,
@IdUsuarioActualiza INT
AS
BEGIN
	UPDATE Rol
	SET Activo = 0
	   ,IdUsuarioActualiza = @IdUsuarioActualiza
	   ,FechaActualizacion = GETDATE()
	WHERE IdRol = @IdRol;
END
GO
CREATE PROC ListarRol 
@Todos BIT,
@IdRol INT = 0
AS
BEGIN
	IF (@Todos = 1)
	BEGIN
		SELECT
			IdRol
		   ,Descripcion AS Rol
		FROM Rol
	END
	ELSE
	BEGIN
		SELECT
			IdRol
		   ,Descripcion AS Rol
		FROM Rol
		WHERE IdRol = @IdRol
	END
END
GO
CREATE PROC InsertarProducto
@IdUnidadMedida INT,
@CodigoBarra INT,
@Descripcion VARCHAR(200),
@PrecioUnitario DECIMAL(18,2),
@PorcentajeUtilidad DECIMAL (18,2),
@PorcentajeDescuento DECIMAL(18,2),
@IdUsuarioRegistro INT
AS
BEGIN
	INSERT INTO Productos (IdUnidadMedida, CodigoBarra, Descripcion, PrecioUnitario, PorcentajeUtilidad, PorcentajeDescuento, IdUsuarioRegistro, FechaRegistro,Activo)
	VALUES (@IdUnidadMedida, @CodigoBarra, @Descripcion, @PrecioUnitario, @PorcentajeUtilidad, @PorcentajeDescuento, @IdUsuarioRegistro, GETDATE(), 1);
SELECT SCOPE_IDENTITY();
END
GO
CREATE PROC ActualizarProducto
@IdProducto INT,
@IdUnidadMedida INT,
@CodigoBarra INT,
@Descripcion VARCHAR(200),
@PrecioUnitario DECIMAL(18,2),
@PorcentajeUtilidad DECIMAL (18,2),
@PorcentajeDescuento DECIMAL(18,2),
@IdUsuarioActualiza INT
AS
BEGIN
	UPDATE Productos 
SET IdUnidadMedida = @IdUnidadMedida
   ,CodigoBarra = @CodigoBarra
   ,Descripcion = @Descripcion
   ,PrecioUnitario = @PrecioUnitario
   ,PorcentajeUtilidad = @PorcentajeUtilidad
   ,PorcentajeDescuento = @PorcentajeDescuento
   ,IdUsuarioActualiza = @IdUsuarioActualiza
   ,FechaActualizacion = GETDATE()
WHERE IdProducto = @IdProducto;
END
GO
CREATE PROC AnularProducto
@IdProducto INT,
@IdUsuarioActualiza INT
AS
BEGIN
	UPDATE Productos 
SET 
   IdUsuarioActualiza = @IdUsuarioActualiza
   ,FechaActualizacion = GETDATE()
   ,Activo = 0
WHERE IdProducto = @IdProducto;
END
GO
CREATE PROC ListarProductos
@Todos BIT,
@IdProducto INT = 0
AS
BEGIN
	IF (@Todos = 1)
		BEGIN
			SELECT p.IdProducto
				  ,p.IdUnidadMedida
				  ,um.Descripcion 'Unidad Medida'
				  ,p.CodigoBarra
				  ,p.Descripcion Producto
				  ,p.PrecioUnitario
				  ,p.PorcentajeUtilidad
				  ,p.PorcentajeDescuento
					FROM Productos p
					INNER JOIN UnidadesMedidas um ON p.IdUnidadMedida = um.IdUnidadMedida
					WHERE p.Activo = 1 AND um.Activo = 1
		END
	ELSE
		BEGIN
			SELECT p.IdProducto
				  ,p.IdUnidadMedida
				  ,um.Descripcion 'Unidad Medida'
				  ,p.CodigoBarra
				  ,p.Descripcion Producto
				  ,p.PrecioUnitario
				  ,p.PorcentajeUtilidad
				  ,p.PorcentajeDescuento
					FROM Productos p
					INNER JOIN UnidadesMedidas um ON p.IdUnidadMedida = um.IdUnidadMedida
					WHERE p.Activo = 1 AND um.Activo = 1 AND p.IdProducto = @IdProducto
		END
END
GO
-- Finalizado hasta la tabla Producto













GO

INSERT INTO Rol (Descripcion, IdUsuarioRegistro, FechaRegistro, Activo)
	VALUES ('Administrador', 1, GETDATE(), 1),
	('Gerente', 1, GETDATE(), 1),
	('Cajero', 1, GETDATE(), 1)
GO
EXEC InsertarUsuario @IdRol = 1
					,@NombreCompleto = 'Administrador de Sistema'
					,@Correo = 'admin@sistema.com.ni'
					,@Cargo = 'Developer'
					,@Login = 'admin'
					,@Password = 0x880487032B4117D1D22E55FF1A96AEB2
					,@IdUsuarioRegistro = 1
--Password del usuario admin es 123
GO




