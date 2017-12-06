if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_bwdf_FormInstances_bwdf_FormDefinitions]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[bwdf_FormInstances] DROP CONSTRAINT FK_bwdf_FormInstances_bwdf_FormDefinitions
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_bwdf_FormItemDefinitions_bwdf_FormDefinitions]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[bwdf_FormItemDefinitions] DROP CONSTRAINT FK_bwdf_FormItemDefinitions_bwdf_FormDefinitions
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_bwdf_FormInstances_Contents_bwdf_FormInstances]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[bwdf_FormInstances_Contents] DROP CONSTRAINT FK_bwdf_FormInstances_Contents_bwdf_FormInstances
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_bwdf_FormItemInstance_bwdf_FormInstances]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[bwdf_FormItemInstances] DROP CONSTRAINT FK_bwdf_FormItemInstance_bwdf_FormInstances
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_bwdf_FormItemInstance_bwdf_FormItemDefinitions]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[bwdf_FormItemInstances] DROP CONSTRAINT FK_bwdf_FormItemInstance_bwdf_FormItemDefinitions
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwdf_FormDefinitionInExternals]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwdf_FormDefinitionInExternals]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwdf_FormDefinitions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwdf_FormDefinitions]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwdf_FormInstances]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwdf_FormInstances]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwdf_FormItemDefinitions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwdf_FormItemDefinitions]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwdf_FormItemInstances]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwdf_FormItemInstances]
GO

CREATE TABLE [dbo].[bwdf_FormDefinitionInExternals] (
	[FormDefinitionId] [uniqueidentifier] NOT NULL ,
	[EntityType] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[EntityId] [uniqueidentifier] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwdf_FormDefinitions] (
	[Id] [uniqueidentifier] NOT NULL ,
	[Name] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Version] [int] NOT NULL ,
	[IsCurrentVersion] [bit] NOT NULL ,
	[Enabled] [bit] NOT NULL ,
	[Comment] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL ,
	[TemplateContent] [ntext] COLLATE Chinese_PRC_CI_AS NULL ,
	[CreatedTime] [datetime] NOT NULL ,
	[LastModTime] [datetime] NOT NULL ,
	[Creator] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[LastModifier] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwdf_FormInstances] (
	[Id] [uniqueidentifier] NOT NULL ,
	[FormDefinitionId] [uniqueidentifier] NOT NULL ,
	[CreatedTime] [datetime] NOT NULL ,
	[LastModTime] [datetime] NOT NULL ,
	[Creator] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[LastModifier] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[FileCount] [int] NOT NULL ,
	[TableIndex] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwdf_FormItemDefinitions] (
	[Id] [uniqueidentifier] NOT NULL ,
	[FormDefinitionId] [uniqueidentifier] NULL ,
	[FName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[Name] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Comment] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[ItemDataType] [int] NOT NULL ,
	[ItemType] [int] NOT NULL ,
	[DataSource] [nvarchar] (1024) COLLATE Chinese_PRC_CI_AS NULL ,
	[DataBinder] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL ,
	[DefaultValue] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[Left] [int] NULL ,
	[Top] [int] NULL ,
	[Width] [int] NULL ,
	[Height] [int] NULL ,
	[RowExclusive] [bit] NULL ,
	[Require] [bit] NULL ,
	[ValidateType] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[MaxVal] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[MinVal] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[Op] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[OpTarget] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[ErrorMessage] [nvarchar] (100) COLLATE Chinese_PRC_CI_AS NULL ,
	[ShowSet] [nvarchar] (2000) COLLATE Chinese_PRC_CI_AS NULL ,
	[WriteSet] [nvarchar] (2000) COLLATE Chinese_PRC_CI_AS NULL ,
	[ReadonlySet] [nvarchar] (2000) COLLATE Chinese_PRC_CI_AS NULL ,
	[CreatedTime] [datetime] NULL 
) ON [PRIMARY]
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

ALTER TABLE [dbo].[bwdf_FormDefinitionInExternals] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwdf_FormDefinitionInExternals] PRIMARY KEY  CLUSTERED 
	(
		[FormDefinitionId],
		[EntityType],
		[EntityId]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwdf_FormDefinitions] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwdf_FormDefinitions] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwdf_FormInstances] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwdf_FormInstances] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwdf_FormItemDefinitions] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwdf_FormItemDefinitions] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwdf_FormItemInstances] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwdf_FormItemInstance] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwdf_FormDefinitions] ADD 
	CONSTRAINT [DF_bwdf_FormDefinitions_IsCurrentVersion] DEFAULT (1) FOR [IsCurrentVersion],
	CONSTRAINT [DF_bwdf_FormDefinitions_Enabled] DEFAULT (1) FOR [Enabled],
	CONSTRAINT [DF_bwdf_FormDefinitions_CreatedTime] DEFAULT (getdate()) FOR [CreatedTime],
	CONSTRAINT [DF_bwdf_FormDefinitions_LastModTime] DEFAULT (getdate()) FOR [LastModTime]
GO

ALTER TABLE [dbo].[bwdf_FormInstances] ADD 
	CONSTRAINT [DF_bwdf_FormInstances_CreatedTime] DEFAULT (getdate()) FOR [CreatedTime],
	CONSTRAINT [DF_bwdf_FormInstances_LastModTime] DEFAULT (getdate()) FOR [LastModTime],
	CONSTRAINT [DF_bwdf_FormInstances_FileCount] DEFAULT (0) FOR [FileCount],
	CONSTRAINT [DF_bwdf_FormInstances_TableIndex] DEFAULT (0) FOR [TableIndex]
GO

 CREATE  INDEX [FK_bwdf_FormInstances_FormDefinitionId] ON [dbo].[bwdf_FormInstances]([FormDefinitionId]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[bwdf_FormItemDefinitions] ADD 
	CONSTRAINT [DF_bwdf_FormItemDefinitions_CreatedTime] DEFAULT (getdate()) FOR [CreatedTime]
GO

 CREATE  INDEX [FK_bwdf_FormItemDefinitions_FormDefinitionId] ON [dbo].[bwdf_FormItemDefinitions]([FormDefinitionId]) ON [PRIMARY]
GO

 CREATE  INDEX [FK_bwdf_FormItemInstance_FormItemDefinitionId] ON [dbo].[bwdf_FormItemInstances]([FormItemDefinitionId]) ON [PRIMARY]
GO

 CREATE  INDEX [FK_bwdf_FormItemInstance_FormInstanceId] ON [dbo].[bwdf_FormItemInstances]([FormInstanceId]) ON [PRIMARY]
GO

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

ALTER TABLE [dbo].[bwdf_FormInstances] ADD 
	CONSTRAINT [FK_bwdf_FormInstances_bwdf_FormDefinitions] FOREIGN KEY 
	(
		[FormDefinitionId]
	) REFERENCES [dbo].[bwdf_FormDefinitions] (
		[Id]
	) ON DELETE CASCADE 
GO

ALTER TABLE [dbo].[bwdf_FormItemDefinitions] ADD 
	CONSTRAINT [FK_bwdf_FormItemDefinitions_bwdf_FormDefinitions] FOREIGN KEY 
	(
		[FormDefinitionId]
	) REFERENCES [dbo].[bwdf_FormDefinitions] (
		[Id]
	) ON DELETE CASCADE 
GO

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

----------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwdf_ext_GetFormItemInstanceList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[bwdf_ext_GetFormItemInstanceList]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
	以分表形式获取指定表单实例的表单项实例数据列表.
	参数：@FormInstanceId,表示表单实例编号.
	
	分表的表示形式："bwdf_FormItemInstances_ + 数字"，如: "bwdf_FormItemInstances_1", 
	分表中只包含表单已经不需修改的表单项实例数据.
*/

CREATE PROCEDURE [dbo].[bwdf_ext_GetFormItemInstanceList]
(
	@FormInstanceId uniqueidentifier
)
AS

DECLARE @TableIndex int
DECLARE @TableName nvarchar(128)
DECLARE @Script nvarchar(3000)
SELECT @TableIndex = ISNULL(TableIndex, 0) FROM bwdf_FormInstances WHERE [Id] = @FormInstanceId
IF @TableIndex <= -1
	SET @TableIndex = 0
IF @TableIndex  = 0
	SET @TableName = 'bwdf_FormItemInstances'
ELSE
	SET @TableName = 'bwdf_FormItemInstances_' + CAST(@TableIndex AS nvarchar(32))

SET @Script = N'SELECT fii.Id, fii.FormItemDefinitionId, fii.FormInstanceId, fii.Value_Str AS [Value], 
				      fii.Value_Decimal AS DecimalValue, fii.Value_Text AS TextValue
				FROM ' + @TableName + N' fii LEFT JOIN 
					  bwdf_FormItemDefinitions fid ON fii.FormItemDefinitionId = fid.Id
				WHERE fii.FormInstanceId ='''+ CAST(@FormInstanceId AS NVARCHAR(50)) + N''''

EXEC (@Script)



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

----------------------------------------------------------