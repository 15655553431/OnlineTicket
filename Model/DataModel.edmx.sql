
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/09/2019 16:30:11
-- Generated from EDMX file: E:\毕业论文\倪敏在线汽车订票系统\OnlineTicket\Model\DataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [OnlineTicket];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PathInfoFlightInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FlightInfo] DROP CONSTRAINT [FK_PathInfoFlightInfo];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfo];
GO
IF OBJECT_ID(N'[dbo].[PathInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PathInfo];
GO
IF OBJECT_ID(N'[dbo].[FlightInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FlightInfo];
GO
IF OBJECT_ID(N'[dbo].[Ticketinfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ticketinfo];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserInfo'
CREATE TABLE [dbo].[UserInfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [GUID] nvarchar(64)  NULL,
    [Login] nvarchar(50)  NOT NULL,
    [Pwd] nvarchar(64)  NOT NULL,
    [UName] nvarchar(64)  NOT NULL,
    [SignDate] datetime  NOT NULL,
    [LoginDate] datetime  NULL,
    [IdCard] nvarchar(20)  NULL,
    [Phone] nvarchar(20)  NOT NULL,
    [Address] nvarchar(100)  NULL,
    [Status] int  NOT NULL,
    [Type] int  NOT NULL,
    [Other] nvarchar(200)  NULL
);
GO

-- Creating table 'PathInfo'
CREATE TABLE [dbo].[PathInfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Origin] nvarchar(50)  NOT NULL,
    [Destination] nvarchar(50)  NOT NULL,
    [Distance] float  NOT NULL,
    [Time] int  NOT NULL,
    [Number] nvarchar(50)  NOT NULL,
    [Status] int  NOT NULL,
    [AddDate] datetime  NOT NULL,
    [Other] nvarchar(200)  NULL
);
GO

-- Creating table 'FlightInfo'
CREATE TABLE [dbo].[FlightInfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [GUID] nvarchar(64)  NOT NULL,
    [LicenceTag] nvarchar(20)  NOT NULL,
    [Driver] nvarchar(50)  NOT NULL,
    [Number] nvarchar(64)  NOT NULL,
    [BusType] nvarchar(50)  NOT NULL,
    [GoTime] datetime  NOT NULL,
    [OverTime] int  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [Passengers] int  NOT NULL,
    [Status] int  NOT NULL,
    [Type] int  NOT NULL,
    [OneDate] datetime  NOT NULL,
    [PathInfoID] int  NOT NULL
);
GO

-- Creating table 'Ticketinfo'
CREATE TABLE [dbo].[Ticketinfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [GUID] nvarchar(64)  NOT NULL,
    [FGUID] nvarchar(64)  NOT NULL,
    [Origin] nvarchar(50)  NOT NULL,
    [Destination] nvarchar(50)  NOT NULL,
    [Money] decimal(18,0)  NOT NULL,
    [Number] nvarchar(50)  NOT NULL,
    [SeatNumber] int  NOT NULL,
    [Gotime] datetime  NOT NULL,
    [Arrtime] datetime  NOT NULL,
    [Overtime] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [IdCard] nvarchar(50)  NOT NULL,
    [Phone] nvarchar(20)  NOT NULL,
    [BuyDate] datetime  NOT NULL,
    [OutDate] datetime  NOT NULL,
    [BuyName] nvarchar(50)  NOT NULL,
    [BuyIdCard] nvarchar(50)  NOT NULL,
    [BuyPhone] nvarchar(20)  NOT NULL,
    [BuyType] int  NOT NULL,
    [LicenceTag] nvarchar(50)  NOT NULL,
    [Driver] nvarchar(50)  NOT NULL,
    [BusType] nvarchar(50)  NOT NULL,
    [Passengers] int  NOT NULL,
    [Status] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'UserInfo'
ALTER TABLE [dbo].[UserInfo]
ADD CONSTRAINT [PK_UserInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'PathInfo'
ALTER TABLE [dbo].[PathInfo]
ADD CONSTRAINT [PK_PathInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'FlightInfo'
ALTER TABLE [dbo].[FlightInfo]
ADD CONSTRAINT [PK_FlightInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Ticketinfo'
ALTER TABLE [dbo].[Ticketinfo]
ADD CONSTRAINT [PK_Ticketinfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PathInfoID] in table 'FlightInfo'
ALTER TABLE [dbo].[FlightInfo]
ADD CONSTRAINT [FK_PathInfoFlightInfo]
    FOREIGN KEY ([PathInfoID])
    REFERENCES [dbo].[PathInfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PathInfoFlightInfo'
CREATE INDEX [IX_FK_PathInfoFlightInfo]
ON [dbo].[FlightInfo]
    ([PathInfoID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------