CREATE DATABASE [TOKA]
GO
USE [TOKA]
GO
/****** Object:  Table [dbo].[Tb_PersonasFisicas]    Script Date: 13/05/2022 06:14:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tb_PersonasFisicas](
	[IdPersonaFisica] [int] IDENTITY(1,1) NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[FechaActualizacion] [datetime] NULL,
	[Nombre] [varchar](50) NULL,
	[ApellidoPaterno] [varchar](50) NULL,
	[ApellidoMaterno] [varchar](50) NULL,
	[RFC] [varchar](13) NULL,
	[FechaNacimiento] [date] NULL,
	[UsuarioAgrega] [int] NULL,
	[Activo] [bit] NULL,
 CONSTRAINT [PK_Tb_PersonasFisicas] PRIMARY KEY CLUSTERED 
(
	[IdPersonaFisica] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tb_Usuario]    Script Date: 13/05/2022 06:14:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tb_Usuario](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Passwd] [varchar](300) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[FechaRegistro] [datetime] NULL,
 CONSTRAINT [PK_Tb_Usuario] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tb_PersonasFisicas] ADD  CONSTRAINT [DF_Tb_PersonasFisicas_FechaRegistro]  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Tb_PersonasFisicas] ADD  CONSTRAINT [DF_Tb_PersonasFisicas_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Tb_Usuario] ADD  CONSTRAINT [DF_Tb_Usuario_FechaRegistro]  DEFAULT (getdate()) FOR [FechaRegistro]
GO
/****** Object:  StoredProcedure [dbo].[sp_ActualizarPersonaFisica]    Script Date: 13/05/2022 06:14:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_ActualizarPersonaFisica]
(
    @IdPersonaFisica INT,
    @Nombre VARCHAR(50),
    @ApellidoPaterno VARCHAR(50),
    @ApellidoMaterno VARCHAR(50),
    @RFC VARCHAR(13),
    @FechaNacimiento DATE,
    @UsuarioAgrega INT
)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ID INT,
            @ERROR VARCHAR(500);
    BEGIN TRY
        IF NOT EXISTS
        (
            SELECT *
            FROM dbo.Tb_PersonasFisicas
            WHERE IdPersonaFisica = @IdPersonaFisica
                  AND Activo = 1
        )
        BEGIN
            SELECT @ERROR = 'La persona fisica no existe.';
            THROW 50000, @ERROR, 1;
        END;

        UPDATE dbo.Tb_PersonasFisicas
        SET Nombre = @Nombre,
            ApellidoPaterno = @ApellidoPaterno,
            ApellidoMaterno = @ApellidoMaterno,
            RFC = @RFC,
            FechaNacimiento = @FechaNacimiento
        WHERE IdPersonaFisica = @IdPersonaFisica;
        SELECT @IdPersonaFisica AS ERROR,
               'Registro modificado exitosamente' AS MENSAJEERROR;
    END TRY
    BEGIN CATCH
        PRINT ERROR_MESSAGE();
        SELECT ERROR_NUMBER() * -1 AS ERROR,
               ISNULL(@ERROR, 'Error al actualizar el registro.') AS MENSAJEERROR;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_AgregarPersonaFisica]    Script Date: 13/05/2022 06:14:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===================================================================
CREATE PROCEDURE [dbo].[sp_AgregarPersonaFisica]
(
    @Nombre VARCHAR(50),
    @ApellidoPaterno VARCHAR(50),
    @ApellidoMaterno VARCHAR(50),
    @RFC VARCHAR(13),
    @FechaNacimiento DATE,
    @UsuarioAgrega INT
)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ID INT,
            @ERROR VARCHAR(500);
    BEGIN TRY
        IF LEN(@RFC) != 13
        BEGIN
            SELECT @ERROR = 'El RFC no es válido';
            THROW 50000, @ERROR, 1;
        END;
        IF EXISTS
        (
            SELECT *
            FROM dbo.Tb_PersonasFisicas
            WHERE RFC = @RFC
                  AND Activo = 1
        )
        BEGIN
            SELECT @ERROR = 'El RFC ya existe en el sistema';
            THROW 50000, @ERROR, 1;
        END;

        INSERT INTO dbo.Tb_PersonasFisicas
        (
            FechaRegistro,
            FechaActualizacion,
            Nombre,
            ApellidoPaterno,
            ApellidoMaterno,
            RFC,
            FechaNacimiento,
            UsuarioAgrega,
            Activo
        )
        VALUES
        (   GETDATE(),        -- FechaRegistro - datetime
            NULL,             -- FechaActualizacion - datetime
            @Nombre,          -- Nombre - varchar(50)
            @ApellidoPaterno, -- ApellidoPaterno - varchar(50)
            @ApellidoMaterno, -- ApellidoMaterno - varchar(50)
            @RFC,             -- RFC - varchar(13)
            @FechaNacimiento, -- FechaNacimiento - date
            @UsuarioAgrega,   -- UsuarioAgrega - int
            1                 -- Activo - bit
            );

        SELECT @ID = SCOPE_IDENTITY();
        SELECT @ID AS ERROR,
               'Registro exitoso' AS MENSAJEERROR;
    END TRY
    BEGIN CATCH
        PRINT ERROR_MESSAGE();
        SELECT ERROR_NUMBER() * -1 AS ERROR,
               ISNULL(@ERROR, 'Error al guardar el registro.') AS MENSAJEERROR;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_EliminarPersonaFisica]    Script Date: 13/05/2022 06:14:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_EliminarPersonaFisica]
(@IdPersonaFisica INT)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ID INT,
            @ERROR VARCHAR(500);
    BEGIN TRY
        IF NOT EXISTS
        (
            SELECT *
            FROM dbo.Tb_PersonasFisicas
            WHERE IdPersonaFisica = @IdPersonaFisica
                  AND Activo = 1
        )        
		BEGIN
            SELECT @ERROR = 'La persona fisica no existe.';
            THROW 50000, @ERROR, 1;
        END;

        UPDATE dbo.Tb_PersonasFisicas
        SET Activo = 0
        WHERE IdPersonaFisica = @IdPersonaFisica;

        SELECT @IdPersonaFisica AS ERROR,
               'Registro eliminado exitosamente' AS MENSAJEERROR;

    END TRY
    BEGIN CATCH
        PRINT ERROR_MESSAGE();
        SELECT ERROR_NUMBER() * -1 AS ERROR,
               ISNULL(@ERROR, 'Error al actualizar el registro.') AS MENSAJEERROR;
    END CATCH;
END;
GO
