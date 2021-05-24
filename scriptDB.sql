USE [master]
GO
/****** Object:  Database [MyDNNDatabase]    Script Date: 5/25/2021 12:38:06 AM ******/
CREATE DATABASE [MyDNNDatabase] ON  PRIMARY 
( NAME = N'MyDNNDatabase', FILENAME = N'c:\Program Files (x86)\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\MyDNNDatabase.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MyDNNDatabase_log', FILENAME = N'c:\Program Files (x86)\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\MyDNNDatabase_log.ldf' , SIZE = 5184KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MyDNNDatabase] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MyDNNDatabase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MyDNNDatabase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET ARITHABORT OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MyDNNDatabase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MyDNNDatabase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MyDNNDatabase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MyDNNDatabase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MyDNNDatabase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MyDNNDatabase] SET  MULTI_USER 
GO
ALTER DATABASE [MyDNNDatabase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MyDNNDatabase] SET DB_CHAINING OFF 
GO
USE [MyDNNDatabase]
GO
/****** Object:  User [MyDNNSQLUser]    Script Date: 5/25/2021 12:38:06 AM ******/
CREATE USER [MyDNNSQLUser] FOR LOGIN [MyDNNSQLUser] WITH DEFAULT_SCHEMA=[dbo]
GO
sys.sp_addrolemember @rolename = N'db_owner', @membername = N'MyDNNSQLUser'
GO
/****** Object:  Table [dbo].[Profesor]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profesor](
	[ID_Profesor] [int] IDENTITY(1,1) NOT NULL,
	[Nume] [nvarchar](50) NOT NULL,
	[Prenume] [nvarchar](50) NULL,
	[Email] [nvarchar](200) NULL,
	[GradDidactic] [nvarchar](50) NULL,
	[FacultateServiciu] [nvarchar](200) NULL,
	[EligibilRemunerare] [bit] NOT NULL,
 CONSTRAINT [PK_Profesor] PRIMARY KEY CLUSTERED 
(
	[ID_Profesor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProgramStudiu]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProgramStudiu](
	[ID_ProgramStudiu] [int] IDENTITY(1,1) NOT NULL,
	[Facultate] [nvarchar](50) NULL,
	[ID_TipCiclu] [int] NOT NULL,
	[DenumireScurta] [nvarchar](10) NOT NULL,
	[Denumire] [nvarchar](200) NULL,
	[NumarAbsolventi] [int] NOT NULL,
	[NumarVotanti] [int] NOT NULL,
 CONSTRAINT [PK_ProgramStudiu] PRIMARY KEY CLUSTERED 
(
	[ID_ProgramStudiu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RezultatVotProfesorProgramStudiu]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RezultatVotProfesorProgramStudiu](
	[ID_RezultatVot] [int] IDENTITY(1,1) NOT NULL,
	[ID_ProgramStudiu] [int] NOT NULL,
	[ID_Profesor] [int] NOT NULL,
	[NumarVoturi] [smallint] NOT NULL,
 CONSTRAINT [PK_EvaluareProfesorApreciat] PRIMARY KEY CLUSTERED 
(
	[ID_RezultatVot] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipCicluInvatamant]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipCicluInvatamant](
	[ID_TipCiclu] [int] IDENTITY(1,1) NOT NULL,
	[Denumire] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_TipCicluInvatamant] PRIMARY KEY CLUSTERED 
(
	[ID_TipCiclu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_DenumireTipCiclu] UNIQUE NONCLUSTERED 
(
	[ID_TipCiclu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RezultatVotProfesorProgramStudiu] ADD  CONSTRAINT [DF_RezultatVotProfesorProgramStudiu_NumarVoturi]  DEFAULT ((0)) FOR [NumarVoturi]
GO
ALTER TABLE [dbo].[RezultatVotProfesorProgramStudiu]  WITH NOCHECK ADD  CONSTRAINT [FK_EvaluareProfesorApreciat_Profesor] FOREIGN KEY([ID_Profesor])
REFERENCES [dbo].[Profesor] ([ID_Profesor])
GO
ALTER TABLE [dbo].[RezultatVotProfesorProgramStudiu] CHECK CONSTRAINT [FK_EvaluareProfesorApreciat_Profesor]
GO
ALTER TABLE [dbo].[RezultatVotProfesorProgramStudiu]  WITH NOCHECK ADD  CONSTRAINT [FK_EvaluareProfesorApreciat_ProgramStudiu] FOREIGN KEY([ID_ProgramStudiu])
REFERENCES [dbo].[ProgramStudiu] ([ID_ProgramStudiu])
GO
ALTER TABLE [dbo].[RezultatVotProfesorProgramStudiu] CHECK CONSTRAINT [FK_EvaluareProfesorApreciat_ProgramStudiu]
GO
/****** Object:  StoredProcedure [dbo].[spAdaugaFacultate]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bucur Valentin-Marian
-- Create date: 29.03.2020
-- Description:	Aceasta procedura insereaza o facultate noua in baza de date.
-- =============================================
CREATE PROCEDURE [dbo].[spAdaugaFacultate]
	-- Add the parameters for the stored procedure here
	@denumireScurta nvarchar(15),
	@denumire nvarchar(100),
	@responseMessage nvarchar(400) OUTPUT,
	@insertedID int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY 
	INSERT INTO Facultate ( DenumireScurta, Denumire ) VALUES ( @denumireScurta, @denumire )
	SET @responseMessage = 'SUCCESS: Record was inserted.';	
	SET @insertedID = SCOPE_IDENTITY(); 
END TRY
BEGIN CATCH  
	SET @responseMessage = 'FAILURE: Record was not inserted. ' + 
						   'Error ' + CONVERT(NVARCHAR, ERROR_NUMBER(), 1) + ': '+ ERROR_MESSAGE();
	SET @insertedID = -1;
END CATCH;   
END
GO
/****** Object:  StoredProcedure [dbo].[spAdaugaProfesor]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bucur Valentin-Marian
-- Create date: 29.03.2020
-- Description:	Aceasta procedura adauga un profesor nou in baza de date.
-- =============================================
CREATE PROCEDURE [dbo].[spAdaugaProfesor]
	-- Add the parameters for the stored procedure here
	@nume nvarchar(50),
	@prenume nvarchar(50),
	@email nvarchar(200),
	@gradDidactic nvarchar(50),
	@FacultateServiciu nvarchar(200),
	@eligibilRemunerare bit,
	@responseMessage nvarchar(400) OUTPUT,
	@insertedID int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY 
	INSERT INTO Profesor ( Nume, 
						   Prenume,
						   Email,
						   GradDidactic,
						   FacultateServiciu,
						   EligibilRemunerare )
				VALUES   ( @nume,
						   @prenume,
						   @email,
						   @gradDidactic,
						   @FacultateServiciu,
						   @eligibilRemunerare )
	SET @responseMessage = 'SUCCESS: Record was inserted.';	
	SET @insertedID = SCOPE_IDENTITY(); 
END TRY
BEGIN CATCH  
	SET @responseMessage = 'FAILURE: Record was not inserted. ' + 
						   'Error ' + CONVERT(NVARCHAR, ERROR_NUMBER(), 1) + ': '+ ERROR_MESSAGE();
	SET @insertedID = -1;
END CATCH;   

END
GO
/****** Object:  StoredProcedure [dbo].[spAdaugaProgramStudiu]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bucur Valentin-Marian
-- Create date: 29.03.2020
-- Description:	Aceasta procedura insereaza un program de studiu in baza de date.
-- =============================================
CREATE PROCEDURE [dbo].[spAdaugaProgramStudiu]
	-- Add the parameters for the stored procedure here
	@Facultate nvarchar(200),
	@idTipCiclu int,
	@denumireScurta nvarchar(10),
	@denumire nvarchar(200),
	@numarAbsolventi int,
	@numarVotanti int,
	@responseMessage nvarchar(400) OUTPUT,
	@insertedID int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY
	INSERT INTO ProgramStudiu ( Facultate,
								ID_TipCiclu,
								DenumireScurta,
								Denumire,
								NumarAbsolventi,
								NumarVotanti)
					   VALUES ( @Facultate,
								@idTipCiclu,
								@denumireScurta,
								@denumire,
								@numarAbsolventi,
								@numarVotanti)
	SET @responseMessage = 'SUCCESS: Record was inserted.';	
	SET @insertedID = SCOPE_IDENTITY(); 
END TRY
BEGIN CATCH  
	SET @responseMessage = 'FAILURE: Record was not inserted. ' + 
						   'Error ' + CONVERT(NVARCHAR, ERROR_NUMBER(), 1) + ': '+ ERROR_MESSAGE();
	SET @insertedID = -1;
END CATCH;

END
GO
/****** Object:  StoredProcedure [dbo].[spAdaugaRezultatVotProfesorProgramStudiu]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bucur Valentin-Marian
-- Create date: 29.03.2020
-- Description:	Aceasta procedura insereaza un rezultat al votului pentru un profesor la un program de studiu.
-- =============================================
CREATE PROCEDURE [dbo].[spAdaugaRezultatVotProfesorProgramStudiu] 
	-- Add the parameters for the stored procedure here
	@idProgramStudiu int,
	@idProfesor int,
	@numarVoturi smallint,
	@responseMessage nvarchar(400) OUTPUT,
	@insertedID int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY 
	INSERT INTO RezultatVotProfesorProgramStudiu ( ID_ProgramStudiu,
												   ID_Profesor,
												   NumarVoturi )
										  VALUES ( @idProgramStudiu,
												   @idProfesor,
												   @numarVoturi )
	SET @responseMessage = 'SUCCESS: Record was inserted.';	
	SET @insertedID = SCOPE_IDENTITY(); 
END TRY
BEGIN CATCH  
	SET @responseMessage = 'FAILURE: Record was not inserted. ' + 
						   'Error ' + CONVERT(NVARCHAR, ERROR_NUMBER(), 1) + ': '+ ERROR_MESSAGE();
	SET @insertedID = -1;
END CATCH;

END
GO
/****** Object:  StoredProcedure [dbo].[spAdaugaTipCicluInvatamant]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bucur Valentin-Marian
-- Create date: 29.03.2020
-- Description:	Aceasta procedura insereaza un tip de ciclu de invatamant in baza de date.
-- =============================================
CREATE PROCEDURE [dbo].[spAdaugaTipCicluInvatamant]
	-- Add the parameters for the stored procedure here
	@denumire nvarchar(100),
	@responseMessage nvarchar(400) OUTPUT,
	@insertedID int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY 
	INSERT INTO TipCicluInvatamant ( Denumire ) VALUES ( @denumire )
	SET @responseMessage = 'SUCCESS: Record was inserted.';	
	SET @insertedID = SCOPE_IDENTITY(); 
END TRY
BEGIN CATCH  
	SET @responseMessage = 'FAILURE: Record was not inserted. ' + 
						   'Error ' + CONVERT(NVARCHAR, ERROR_NUMBER(), 1) + ': '+ ERROR_MESSAGE();
	SET @insertedID = -1;
END CATCH;   
END
GO
/****** Object:  StoredProcedure [dbo].[spClearDatabase]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bucur Valentin-Marian
-- Create date: 8.04.2021
-- Description:	This clears all data in the database.
-- =============================================
CREATE PROCEDURE [dbo].[spClearDatabase]

AS
BEGIN
	EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'

	EXEC sp_MSForEachTable 'ALTER TABLE ? DISABLE TRIGGER ALL'

	EXEC sp_MSForEachTable 'DELETE FROM ?'

	EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'

	EXEC sp_MSForEachTable 'ALTER TABLE ? ENABLE TRIGGER ALL'

	EXEC sp_MSforeachtable @command1 = 'DBCC CHECKIDENT (''?'', RESEED, 0)'
END
GO
/****** Object:  StoredProcedure [dbo].[spEligibilRemunerareLicenta]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Precup Diana
-- Create date: 13.04.2021
-- Description:	Aceasta procedura genereaza lista cu profesorii eligibili pentru remunerare pentru fiecare faculate de la programul de studii licenta.
-- =============================================
CREATE PROCEDURE [dbo].[spEligibilRemunerareLicenta]
AS

BEGIN

SELECT
  Facultate.DenumireScurta,
  Profesor.Nume,
  Profesor.Prenume,
  TipCicluInvatamant.Denumire
FROM Facultate
JOIN Profesor
  ON Profesor.ID_FacultateServiciu=Facultate.ID_Facultate
JOIN ProgramStudiu
  ON ProgramStudiu.ID_Facultate = Facultate.ID_Facultate 
JOIN TipCicluInvatamant
  ON TipCicluInvatamant.ID_TipCiclu = ProgramStudiu.ID_TipCiclu
  WHERE TipCicluInvatamant.Denumire = 'Licenta'
    GROUP BY Profesor.Nume, Profesor.Prenume, TipCicluInvatamant.Denumire,  Facultate.DenumireScurta
	   ORDER BY Facultate.DenumireScurta

END
GO
/****** Object:  StoredProcedure [dbo].[spEligibilRemunerareLicentaSiMaster1]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Precup Diana
-- Create date: 13.04.2021
-- Description:	Aceasta procedura genereaza lista cu profesorii eligibili pentru remunerare pentru fiecare faculate de la programul de studii licenta si master.
-- =============================================
CREATE PROCEDURE [dbo].[spEligibilRemunerareLicentaSiMaster1]
AS

BEGIN

SELECT TOP 10 PERCENT
   Profesor.Nume, 
   Profesor.Prenume, 
   Facultate.DenumireScurta
FROM Facultate
INNER JOIN Profesor ON Profesor.ID_FacultateServiciu=Facultate.ID_Facultate
  WHERE EligibilRemunerare=1
      GROUP BY Profesor.Nume, Profesor.Prenume, Facultate.DenumireScurta
          ORDER BY Facultate.DenumireScurta

END
GO
/****** Object:  StoredProcedure [dbo].[spRezultateVoturiLicenta]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Precup Diana
-- Create date: 13.04.2021
-- Description:	Aceasta procedura genereaza lista cu profesorii in ordinea numarului de voturi de la programele de studii de licenta.
-- =============================================
CREATE PROCEDURE [dbo].[spRezultateVoturiLicenta]
	
AS

BEGIN

SELECT 
  Profesor.ID_Profesor,
  Profesor.Nume,
  Profesor.Prenume,
  Profesor.Email,
  Profesor.GradDidactic,
 SUM(RezultatVotProfesorProgramStudiu.NumarVoturi) AS VoturiTotale
FROM Profesor  
JOIN RezultatVotProfesorProgramStudiu
  ON Profesor.ID_Profesor = RezultatVotProfesorProgramStudiu.ID_Profesor
JOIN ProgramStudiu
  ON ProgramStudiu.ID_ProgramStudiu = RezultatVotProfesorProgramStudiu.ID_ProgramStudiu
JOIN TipCicluInvatamant
  ON TipCicluInvatamant.ID_TipCiclu = ProgramStudiu.ID_TipCiclu
  WHERE TipCicluInvatamant.Denumire = 'Licenta'
    GROUP BY Profesor.ID_Profesor, Profesor.Nume, Profesor.Prenume, Profesor.Email, Profesor.GradDidactic
      ORDER BY VoturiTotale DESC

END
GO
/****** Object:  StoredProcedure [dbo].[spRezultateVoturiLicentaSiMaster]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Precup Diana
-- Create date: 13.04.2021
-- Description:	Aceasta procedura genereaza lista cu profesorii in ordinea numarului de voturi de la programele de studii de licenta si master.
-- =============================================
CREATE PROCEDURE [dbo].[spRezultateVoturiLicentaSiMaster]

 AS

BEGIN

SELECT 
  Profesor.ID_Profesor,
  Profesor.Nume,
  Profesor.Prenume,
  Profesor.Email,
  Profesor.GradDidactic,
  ProgramStudiu.DenumireScurta,
  RezultatVotProfesorProgramStudiu.NumarVoturi,
  SUM(RezultatVotProfesorProgramStudiu.NumarVoturi) AS VoturiTotale
FROM Profesor  
JOIN RezultatVotProfesorProgramStudiu
  ON Profesor.ID_Profesor = RezultatVotProfesorProgramStudiu.ID_Profesor
JOIN ProgramStudiu
  ON RezultatVotProfesorProgramStudiu.ID_ProgramStudiu = ProgramStudiu.ID_ProgramStudiu
    GROUP BY Profesor.ID_Profesor, Profesor.Nume, Profesor.Prenume, Profesor.Email, Profesor.GradDidactic, ProgramStudiu.DenumireScurta, RezultatVotProfesorProgramStudiu.NumarVoturi
      ORDER BY VoturiTotale DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[spTest]    Script Date: 5/25/2021 12:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spTest]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Profesor
END
GO
USE [master]
GO
ALTER DATABASE [MyDNNDatabase] SET  READ_WRITE 
GO
