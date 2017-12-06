/***********************************************************************
修改 bwdf_FormItemInstances 数据表为备份数据表(分表). 
备份数据表(分表)名称: bwdf_FormItemInstances_5
-------------------------------------------------------
建议替换 bwdf_FormItemInstances_5 为指定分表名(如: bwdf_FormItemInstances_6).
***********************************************************************/
-- 重建全文索引
EXEC sp_fulltext_catalog 'XQPCatalog', 'rebuild'
GO

-- 删除 bwdf_FormItemInstances 全文索引
EXEC sp_fulltext_table 'bwdf_FormItemInstances', 'drop'
GO

-- 为 bwdf_FormItemInstances 重命名.
EXEC sp_rename 'bwdf_FormItemInstances', 'bwdf_FormItemInstances_5'
GO

-- 删除 bwdf_FormItemInstances 键以及索引等关系
ALTER TABLE [dbo].[bwdf_FormItemInstances_5] DROP
	CONSTRAINT [PK_bwdf_FormItemInstance]
GO

ALTER TABLE [dbo].[bwdf_FormItemInstances_5] DROP
	CONSTRAINT [FK_bwdf_FormItemInstance_bwdf_FormInstances]
GO

ALTER TABLE [dbo].[bwdf_FormItemInstances_5] DROP
	CONSTRAINT [FK_bwdf_FormItemInstance_bwdf_FormItemDefinitions]
GO

IF EXISTS (SELECT name FROM sysindexes
         WHERE name = 'FK_bwdf_FormItemInstance_FormItemDefinitionId')
   DROP INDEX bwdf_FormItemInstances_5.[FK_bwdf_FormItemInstance_FormItemDefinitionId]
GO
IF EXISTS (SELECT name FROM sysindexes
         WHERE name = 'FK_bwdf_FormItemInstance_FormInstanceId')
   DROP INDEX bwdf_FormItemInstances_5.[FK_bwdf_FormItemInstance_FormInstanceId]
GO

-- 创建主键等
ALTER TABLE [dbo].[bwdf_FormItemInstances_5] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwdf_FormItemInstances_5] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO
 CREATE  INDEX [FK_bwdf_FormItemInstances_5_FormItemDefinitionId] ON [dbo].[bwdf_FormItemInstances_5]([FormItemDefinitionId]) ON [PRIMARY]
GO

 CREATE  INDEX [FK_bwdf_FormItemInstances_5_FormInstanceId] ON [dbo].[bwdf_FormItemInstances_5]([FormInstanceId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[bwdf_FormItemInstances_5] ADD 
	CONSTRAINT [FK_bwdf_bwdf_FormItemInstances_5_bwdf_FormInstances] FOREIGN KEY 
	(
		[FormInstanceId]
	) REFERENCES [dbo].[bwdf_FormInstances] (
		[Id]
	),
	CONSTRAINT [FK_bwdf_FormItemInstances_5_bwdf_FormItemDefinitions] FOREIGN KEY 
	(
		[FormItemDefinitionId]
	) REFERENCES [dbo].[bwdf_FormItemDefinitions] (
		[Id]
	) ON DELETE CASCADE 
GO

-- 创建全文索引
if (select DATABASEPROPERTY(DB_NAME(), N'IsFullTextEnabled')) <> 1 
exec sp_fulltext_database N'enable' 
GO

if not exists (select * from dbo.sysfulltextcatalogs where name = N'XQPCatalog')
exec sp_fulltext_catalog N'XQPCatalog', N'create' 
GO

exec sp_fulltext_table N'[dbo].[bwdf_FormItemInstances_5]', N'create', N'XQPCatalog', N'PK_bwdf_FormItemInstances_5'
GO

exec sp_fulltext_column N'[dbo].[bwdf_FormItemInstances_5]', N'Value_Str', N'add', 2052  
GO

exec sp_fulltext_column N'[dbo].[bwdf_FormItemInstances_5]', N'Value_Text', N'add', 2052  
GO

exec sp_fulltext_table N'[dbo].[bwdf_FormItemInstances_5]', N'activate'  
GO


/***********************************************************************
新增 bwdf_FormItemInstances 数据表.
***********************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwdf_FormItemInstances]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwdf_FormItemInstances]
GO

CREATE TABLE [dbo].[bwdf_FormItemInstances] (
	[Id] [uniqueidentifier] NOT NULL ,
	[FormItemDefinitionId] [uniqueidentifier] NOT NULL ,
	[FormInstanceId] [uniqueidentifier] NOT NULL ,
	[Value_Str] [nvarchar] (3000) COLLATE Chinese_PRC_CI_AS NULL ,
	[Value_Decimal] [decimal](18, 2) NULL ,
	[Value_Text] [ntext] COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-- 更新表主键
ALTER TABLE [dbo].[bwdf_FormItemInstances] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwdf_FormItemInstance] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

-- 更新表索引键
 CREATE  INDEX [FK_bwdf_FormItemInstance_FormItemDefinitionId] ON [dbo].[bwdf_FormItemInstances]([FormItemDefinitionId]) ON [PRIMARY]
GO

 CREATE  INDEX [FK_bwdf_FormItemInstance_FormInstanceId] ON [dbo].[bwdf_FormItemInstances]([FormInstanceId]) ON [PRIMARY]
GO

-- 更新表全文索引
if (select DATABASEPROPERTY(DB_NAME(), N'IsFullTextEnabled')) <> 1 
exec sp_fulltext_database N'enable' 
GO

if not exists (select * from dbo.sysfulltextcatalogs where name = N'XQPCatalog')
exec sp_fulltext_catalog N'XQPCatalog', N'create' 
GO

exec sp_fulltext_table N'[dbo].[bwdf_FormItemInstances]', N'create', N'XQPCatalog', N'PK_bwdf_FormItemInstance'
GO

exec sp_fulltext_column N'[dbo].[bwdf_FormItemInstances]', N'Value_Str', N'add', 2052  
GO

exec sp_fulltext_column N'[dbo].[bwdf_FormItemInstances]', N'Value_Text', N'add', 2052  
GO

exec sp_fulltext_table N'[dbo].[bwdf_FormItemInstances]', N'activate'  
GO

-- 更新表约束
ALTER TABLE [dbo].[bwdf_FormItemInstances] ADD 
	CONSTRAINT [FK_bwdf_FormItemInstance_bwdf_FormInstances] FOREIGN KEY 
	(
		[FormInstanceId]
	) REFERENCES [dbo].[bwdf_FormInstances] (
		[Id]
	),
	CONSTRAINT [FK_bwdf_FormItemInstance_bwdf_FormItemDefinitions] FOREIGN KEY 
	(
		[FormItemDefinitionId]
	) REFERENCES [dbo].[bwdf_FormItemDefinitions] (
		[Id]
	) ON DELETE CASCADE 
GO

----------------------------------------------------------------------------------------------

/***********************************************************************
重新移动 bwdf_FormInstances 未完成表单项到 bwdf_FormItemInstances 中.
***********************************************************************/
DECLARE @Uncompleted table(FormInstanceId uniqueidentifier)
INSERT INTO @Uncompleted(FormInstanceId)
	SELECT WorkflowInstanceId
	FROM bwwf_Tracking_Workflows
	WHERE State <= 1

INSERT INTO bwdf_FormItemInstances([Id], FormItemDefinitionId, FormInstanceId, Value_Str, Value_Decimal,Value_Text)
	SELECT [Id], FormItemDefinitionId, FormInstanceId, Value_Str, Value_Decimal,Value_Text
	FROM bwdf_FormItemInstances_5
	WHERE FormInstanceId IN(
		SELECT FormInstanceId FROM @Uncompleted
	)
DELETE FROM bwdf_FormItemInstances_5 WHERE FormInstanceId IN(
	SELECT FormInstanceId FROM @Uncompleted
)


/***********************************************************************
更新 bwdf_FormInstances 分表索引.
***********************************************************************/

UPDATE bwdf_FormInstances
SET TableIndex = 5
WHERE TableIndex = 0 AND [ID] IN(
	SELECT FormInstanceId FROM bwdf_FormItemInstances_5
)
