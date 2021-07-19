BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[produtos]') AND [c].[name] = N'Nome');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [produtos] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [produtos] ALTER COLUMN [Nome] varchar(300) NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210712205223_TesteDeAlteracao', N'5.0.4');
GO

COMMIT;
GO

