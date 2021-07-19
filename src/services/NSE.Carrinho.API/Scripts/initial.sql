BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210505132219_ItemsToItens', N'5.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [CarrinhoCliente] ADD [Desconto] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [CarrinhoCliente] ADD [TipoDesconto] int NULL;
GO

ALTER TABLE [CarrinhoCliente] ADD [ValorDesconto] decimal(18,2) NULL;
GO

ALTER TABLE [CarrinhoCliente] ADD [VoucherCodigo] varchar(50) NULL;
GO

ALTER TABLE [CarrinhoCliente] ADD [VoucherUtilizado] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [CarrinhoCliente] ADD [Voucher_Percentual] decimal(18,2) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210519003335_Voucher', N'5.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[CarrinhoCliente].[Voucher_Percentual]', N'Percentual', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210519003639_VoucherAjusteNomePercentual', N'5.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [CarrinhoItems] DROP CONSTRAINT [FK_CarrinhoItems_CarrinhoCliente_CarrinhoId];
GO

ALTER TABLE [CarrinhoItems] ADD CONSTRAINT [FK_CarrinhoItems_CarrinhoCliente_CarrinhoId] FOREIGN KEY ([CarrinhoId]) REFERENCES [CarrinhoCliente] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210525215420_CarrinhoCascade', N'5.0.4');
GO

COMMIT;
GO

