IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210403233037_Initial')
BEGIN
    CREATE TABLE [produtos] (
        [Id] uniqueidentifier NOT NULL,
        [Nome] varchar(250) NOT NULL,
        [Descricao] varchar(500) NOT NULL,
        [Ativo] bit NOT NULL,
        [Valor] decimal(18,2) NOT NULL,
        [DataCadastro] datetime2 NOT NULL,
        [Imagem] varchar(250) NOT NULL,
        [QuantidadeEstoque] int NOT NULL,
        CONSTRAINT [PK_produtos] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210403233037_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210403233037_Initial', N'5.0.4');
END;
GO

COMMIT;
GO

