--------------------------------------------------
-- 1) Gewünschte Tabellen eintragen
--    Schema + Tabellenname
--------------------------------------------------
DECLARE @TargetTables TABLE
(
    SchemaName SYSNAME,
    TableName  SYSNAME
);

INSERT INTO @TargetTables (SchemaName, TableName)
VALUES
    ('dbo', 'Games'),
    ('dbo', 'GameVersions')

--------------------------------------------------
-- 2) Sicherheitscheck:
--    Nur Tabellen übernehmen, die wirklich existieren
--------------------------------------------------
DECLARE @ValidatedTables TABLE
(
    SchemaName SYSNAME,
    TableName  SYSNAME
);

INSERT INTO @ValidatedTables (SchemaName, TableName)
SELECT tt.SchemaName, tt.TableName
FROM @TargetTables tt
INNER JOIN sys.tables t ON t.name = tt.TableName
INNER JOIN sys.schemas s ON s.schema_id = t.schema_id AND s.name = tt.SchemaName;

--------------------------------------------------
-- 3) Anzeigen, welche Tabellen betroffen sind
--------------------------------------------------
SELECT SchemaName, TableName
FROM @ValidatedTables
ORDER BY SchemaName, TableName;

--------------------------------------------------
-- 4) SQL zusammenbauen
--------------------------------------------------
DECLARE @sql NVARCHAR(MAX) = N'';

--------------------------------------------------
-- 5) Foreign Keys für diese Tabellen deaktivieren
--------------------------------------------------
SELECT @sql = @sql + N'ALTER TABLE ' + QUOTENAME(vt.SchemaName) + N'.' + QUOTENAME(vt.TableName) + N' NOCHECK CONSTRAINT ALL;' + CHAR(13) + CHAR(10)
FROM @ValidatedTables vt
ORDER BY vt.SchemaName, vt.TableName;

EXEC sp_executesql @sql;

--------------------------------------------------
-- 6) Inhalte löschen
--------------------------------------------------
SET @sql = N'';

SELECT @sql = @sql + N'DELETE FROM ' + QUOTENAME(vt.SchemaName) + N'.' + QUOTENAME(vt.TableName) + N';' + CHAR(13) + CHAR(10)
FROM @ValidatedTables vt
ORDER BY vt.SchemaName, vt.TableName;

EXEC sp_executesql @sql;

--------------------------------------------------
-- 7) Identity zurücksetzen
--    Nur bei Tabellen mit Identity-Spalte
--------------------------------------------------
SET @sql = N'';

SELECT @sql = @sql + N'DBCC CHECKIDENT (''' + vt.SchemaName + N'.' + vt.TableName + N''', RESEED, 0);' + CHAR(13) + CHAR(10)
FROM @ValidatedTables vt
INNER JOIN sys.tables t ON t.name = vt.TableName
INNER JOIN sys.schemas s ON s.schema_id = t.schema_id AND s.name = vt.SchemaName
INNER JOIN sys.identity_columns ic ON ic.object_id = t.object_id
ORDER BY vt.SchemaName, vt.TableName;

EXEC sp_executesql @sql;

--------------------------------------------------
-- 8) Constraints wieder aktivieren
--------------------------------------------------
SET @sql = N'';

SELECT @sql = @sql + N'ALTER TABLE ' + QUOTENAME(vt.SchemaName) + N'.' + QUOTENAME(vt.TableName) + N' WITH CHECK CHECK CONSTRAINT ALL;' + CHAR(13) + CHAR(10)
FROM @ValidatedTables vt
ORDER BY vt.SchemaName, vt.TableName;

EXEC sp_executesql @sql;