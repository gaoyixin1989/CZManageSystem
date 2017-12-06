/****** 对象: 表 [dbo].[bwrpt_BaseItemSP]    脚本日期: 2009-4-8 15:50:00 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwrpt_BaseItemSP]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwrpt_BaseItemSP]
GO

/****** 对象: 表 [dbo].[bwrpt_BaseItemSP]    脚本日期: 2009-4-8 15:50:00 ******/
CREATE TABLE [dbo].[bwrpt_BaseItemSP] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[ReportID] [int] NOT NULL ,
	[Parameter] [varchar] (100) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[DataType] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[DefaultValue] [varchar] (200) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[bwrpt_BaseItemSP] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwrpt_BaseItemSP] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwrpt_BaseItemSP] ADD 
	CONSTRAINT [DF_bwrpt_BaseItemSP_DefaultValue] DEFAULT ('') FOR [DefaultValue]
GO

 CREATE  INDEX [IX_bwrpt_BaseItemSP] ON [dbo].[bwrpt_BaseItemSP]([ID]) ON [PRIMARY]
GO


exec sp_addextendedproperty N'MS_Description', N'参数类型', N'user', N'dbo', N'table', N'bwrpt_BaseItemSP', N'column', N'DataType'
GO
exec sp_addextendedproperty N'MS_Description', N'默认值', N'user', N'dbo', N'table', N'bwrpt_BaseItemSP', N'column', N'DefaultValue'
GO
exec sp_addextendedproperty N'MS_Description', N'流水号', N'user', N'dbo', N'table', N'bwrpt_BaseItemSP', N'column', N'ID'
GO
exec sp_addextendedproperty N'MS_Description', N'存储过程参数', N'user', N'dbo', N'table', N'bwrpt_BaseItemSP', N'column', N'Parameter'
GO
exec sp_addextendedproperty N'MS_Description', N'报表编号', N'user', N'dbo', N'table', N'bwrpt_BaseItemSP', N'column', N'ReportID'

GO

---------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id('DF_bw_Report_IsSenior'))
BEGIN

ALTER TABLE dbo.bwrpt_BaseInfo
	DROP CONSTRAINT DF_bw_Report_IsSenior

ALTER TABLE dbo.bwrpt_BaseInfo
	DROP COLUMN IsSenior

ALTER TABLE dbo.bwrpt_BaseInfo ADD
	SourceType int NULL

ALTER TABLE dbo.bwrpt_BaseInfo ADD CONSTRAINT
	DF_bwrpt_BaseInfo_SourceType DEFAULT 0 FOR SourceType

UPDATE bwrpt_BaseInfo SET SourceType = 0
END
GO

---------------------------------------------------------------------------------------------------

if not exists (select * from dbo.sysobjects where id = object_id('PK_bwrpt_AdminInfo'))
	ALTER TABLE dbo.bwrpt_AdminInfo ADD CONSTRAINT
		PK_bwrpt_AdminInfo PRIMARY KEY CLUSTERED 
		(
		ID
		) ON [PRIMARY]
GO
---------------------------------------------------------------------------------------------------
if not exists (select * from dbo.sysobjects where id = object_id('PK_bwrpt_AdminItem'))
	ALTER TABLE dbo.bwrpt_AdminItem ADD CONSTRAINT
		PK_bwrpt_AdminItem PRIMARY KEY CLUSTERED 
		(
		ID
		) ON [PRIMARY]

GO
---------------------------------------------------------------------------------------------------
