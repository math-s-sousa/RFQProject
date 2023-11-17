USE [master]
GO
/****** Object:  Database [CUSTOM_RFQ]    Script Date: 14/11/2023 18:22:44 ******/
CREATE DATABASE [CUSTOM_RFQ]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CUSTOM_RFQ', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\CUSTOM_RFQ.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CUSTOM_RFQ_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\CUSTOM_RFQ_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [CUSTOM_RFQ] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CUSTOM_RFQ].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CUSTOM_RFQ] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET ARITHABORT OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CUSTOM_RFQ] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CUSTOM_RFQ] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CUSTOM_RFQ] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CUSTOM_RFQ] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET RECOVERY FULL 
GO
ALTER DATABASE [CUSTOM_RFQ] SET  MULTI_USER 
GO
ALTER DATABASE [CUSTOM_RFQ] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CUSTOM_RFQ] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CUSTOM_RFQ] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CUSTOM_RFQ] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CUSTOM_RFQ] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CUSTOM_RFQ] SET QUERY_STORE = OFF
GO
USE [CUSTOM_RFQ]
GO
/****** Object:  Table [dbo].[DbConfig]    Script Date: 14/11/2023 18:22:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbConfig](
	[BaseUrl] [nvarchar](150) NOT NULL,
	[DB] [nvarchar](80) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventSender]    Script Date: 14/11/2023 18:22:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventSender](
	[Guid] [nvarchar](50) NOT NULL,
	[DocEntry] [int] NOT NULL,
	[ObjType] [int] NOT NULL,
	[DB] [nvarchar](80) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[Status] [char](1) NOT NULL,
	[UserCode] [nvarchar](10) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SmtpConfig]    Script Date: 14/11/2023 18:22:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SmtpConfig](
	[Server] [nvarchar](50) NOT NULL,
	[Port] [int] NOT NULL,
	[SSL] [bit] NOT NULL,
	[Host] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Subject] [nvarchar](100) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[BaseUrl] [nvarchar](50) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [CUSTOM_RFQ] SET  READ_WRITE 
GO