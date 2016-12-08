
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/05/2016 19:32:59
-- Generated from EDMX file: C:\Users\XiaoXiong\Source\Repos\WX\微信功能了解\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [hbxls];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[wx_xx_d_offal]', 'U') IS NOT NULL
    DROP TABLE [dbo].[wx_xx_d_offal];
GO
IF OBJECT_ID(N'[dbo].[wx_xx_d_repertory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[wx_xx_d_repertory];
GO
IF OBJECT_ID(N'[dbo].[wx_xx_d_sterilize]', 'U') IS NOT NULL
    DROP TABLE [dbo].[wx_xx_d_sterilize];
GO
IF OBJECT_ID(N'[dbo].[wx_xx_government]', 'U') IS NOT NULL
    DROP TABLE [dbo].[wx_xx_government];
GO
IF OBJECT_ID(N'[dbo].[wx_xx_shop]', 'U') IS NOT NULL
    DROP TABLE [dbo].[wx_xx_shop];
GO
IF OBJECT_ID(N'[dbo].[wx_xx_user]', 'U') IS NOT NULL
    DROP TABLE [dbo].[wx_xx_user];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'wx_xx_d_offal'
CREATE TABLE [dbo].[wx_xx_d_offal] (
    [Id] int  NOT NULL,
    [Weight] float  NOT NULL,
    [Datetime] datetime  NOT NULL,
    [wx_xx_userId] int  NOT NULL,
    [wx_xx_shopId] int  NOT NULL
);
GO

-- Creating table 'wx_xx_d_repertory'
CREATE TABLE [dbo].[wx_xx_d_repertory] (
    [Id] int  NOT NULL,
    [CargoName] nvarchar(64)  NOT NULL,
    [Type] nchar(10)  NOT NULL,
    [Quantity] float  NOT NULL,
    [BatchNum] nvarchar(128)  NULL,
    [Expiration_date] nvarchar(16)  NULL,
    [Supplier] nvarchar(64)  NOT NULL,
    [Acceptor_Openid] varchar(64)  NOT NULL,
    [Date] datetime  NOT NULL,
    [wx_xx_userId] int  NOT NULL,
    [wx_xx_shopId] int  NOT NULL
);
GO

-- Creating table 'wx_xx_d_sterilize'
CREATE TABLE [dbo].[wx_xx_d_sterilize] (
    [Id] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Product] nchar(10)  NOT NULL,
    [wx_xx_userId] int  NOT NULL,
    [wx_xx_shopId] int  NOT NULL
);
GO

-- Creating table 'wx_xx_government'
CREATE TABLE [dbo].[wx_xx_government] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(64)  NOT NULL,
    [Address] nvarchar(256)  NULL,
    [Username] nvarchar(64)  NOT NULL,
    [Password] nvarchar(64)  NOT NULL
);
GO

-- Creating table 'wx_xx_shop'
CREATE TABLE [dbo].[wx_xx_shop] (
    [Id] int  NOT NULL,
    [Name] nvarchar(64)  NOT NULL,
    [Address] nvarchar(256)  NULL,
    [wx_xx_governmentId] int  NOT NULL
);
GO

-- Creating table 'wx_xx_user'
CREATE TABLE [dbo].[wx_xx_user] (
    [Id] int  NOT NULL,
    [open_id] varchar(64)  NOT NULL,
    [cnname] nvarchar(16)  NOT NULL,
    [wx_xx_shopId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'wx_xx_d_offal'
ALTER TABLE [dbo].[wx_xx_d_offal]
ADD CONSTRAINT [PK_wx_xx_d_offal]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'wx_xx_d_repertory'
ALTER TABLE [dbo].[wx_xx_d_repertory]
ADD CONSTRAINT [PK_wx_xx_d_repertory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'wx_xx_d_sterilize'
ALTER TABLE [dbo].[wx_xx_d_sterilize]
ADD CONSTRAINT [PK_wx_xx_d_sterilize]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'wx_xx_government'
ALTER TABLE [dbo].[wx_xx_government]
ADD CONSTRAINT [PK_wx_xx_government]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'wx_xx_shop'
ALTER TABLE [dbo].[wx_xx_shop]
ADD CONSTRAINT [PK_wx_xx_shop]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'wx_xx_user'
ALTER TABLE [dbo].[wx_xx_user]
ADD CONSTRAINT [PK_wx_xx_user]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [wx_xx_governmentId] in table 'wx_xx_shop'
ALTER TABLE [dbo].[wx_xx_shop]
ADD CONSTRAINT [FK_wx_xx_governmentwx_xx_shop]
    FOREIGN KEY ([wx_xx_governmentId])
    REFERENCES [dbo].[wx_xx_government]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_wx_xx_governmentwx_xx_shop'
CREATE INDEX [IX_FK_wx_xx_governmentwx_xx_shop]
ON [dbo].[wx_xx_shop]
    ([wx_xx_governmentId]);
GO

-- Creating foreign key on [wx_xx_userId] in table 'wx_xx_d_offal'
ALTER TABLE [dbo].[wx_xx_d_offal]
ADD CONSTRAINT [FK_wx_xx_userwx_xx_d_offal]
    FOREIGN KEY ([wx_xx_userId])
    REFERENCES [dbo].[wx_xx_user]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_wx_xx_userwx_xx_d_offal'
CREATE INDEX [IX_FK_wx_xx_userwx_xx_d_offal]
ON [dbo].[wx_xx_d_offal]
    ([wx_xx_userId]);
GO

-- Creating foreign key on [wx_xx_userId] in table 'wx_xx_d_repertory'
ALTER TABLE [dbo].[wx_xx_d_repertory]
ADD CONSTRAINT [FK_wx_xx_userwx_xx_d_repertory]
    FOREIGN KEY ([wx_xx_userId])
    REFERENCES [dbo].[wx_xx_user]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_wx_xx_userwx_xx_d_repertory'
CREATE INDEX [IX_FK_wx_xx_userwx_xx_d_repertory]
ON [dbo].[wx_xx_d_repertory]
    ([wx_xx_userId]);
GO

-- Creating foreign key on [wx_xx_userId] in table 'wx_xx_d_sterilize'
ALTER TABLE [dbo].[wx_xx_d_sterilize]
ADD CONSTRAINT [FK_wx_xx_userwx_xx_d_sterilize]
    FOREIGN KEY ([wx_xx_userId])
    REFERENCES [dbo].[wx_xx_user]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_wx_xx_userwx_xx_d_sterilize'
CREATE INDEX [IX_FK_wx_xx_userwx_xx_d_sterilize]
ON [dbo].[wx_xx_d_sterilize]
    ([wx_xx_userId]);
GO

-- Creating foreign key on [wx_xx_shopId] in table 'wx_xx_d_offal'
ALTER TABLE [dbo].[wx_xx_d_offal]
ADD CONSTRAINT [FK_wx_xx_shopwx_xx_d_offal]
    FOREIGN KEY ([wx_xx_shopId])
    REFERENCES [dbo].[wx_xx_shop]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_wx_xx_shopwx_xx_d_offal'
CREATE INDEX [IX_FK_wx_xx_shopwx_xx_d_offal]
ON [dbo].[wx_xx_d_offal]
    ([wx_xx_shopId]);
GO

-- Creating foreign key on [wx_xx_shopId] in table 'wx_xx_d_repertory'
ALTER TABLE [dbo].[wx_xx_d_repertory]
ADD CONSTRAINT [FK_wx_xx_shopwx_xx_d_repertory]
    FOREIGN KEY ([wx_xx_shopId])
    REFERENCES [dbo].[wx_xx_shop]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_wx_xx_shopwx_xx_d_repertory'
CREATE INDEX [IX_FK_wx_xx_shopwx_xx_d_repertory]
ON [dbo].[wx_xx_d_repertory]
    ([wx_xx_shopId]);
GO

-- Creating foreign key on [wx_xx_shopId] in table 'wx_xx_d_sterilize'
ALTER TABLE [dbo].[wx_xx_d_sterilize]
ADD CONSTRAINT [FK_wx_xx_shopwx_xx_d_sterilize]
    FOREIGN KEY ([wx_xx_shopId])
    REFERENCES [dbo].[wx_xx_shop]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_wx_xx_shopwx_xx_d_sterilize'
CREATE INDEX [IX_FK_wx_xx_shopwx_xx_d_sterilize]
ON [dbo].[wx_xx_d_sterilize]
    ([wx_xx_shopId]);
GO

-- Creating foreign key on [wx_xx_shopId] in table 'wx_xx_user'
ALTER TABLE [dbo].[wx_xx_user]
ADD CONSTRAINT [FK_wx_xx_shopwx_xx_user]
    FOREIGN KEY ([wx_xx_shopId])
    REFERENCES [dbo].[wx_xx_shop]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_wx_xx_shopwx_xx_user'
CREATE INDEX [IX_FK_wx_xx_shopwx_xx_user]
ON [dbo].[wx_xx_user]
    ([wx_xx_shopId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------