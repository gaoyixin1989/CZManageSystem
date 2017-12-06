/****** 对象: 表 [dbo].[bwwf_Designer_Activities]    脚本日期: 2009-4-21 16:34:47 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_Designer_Activities]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_Designer_Activities]
GO

/****** 对象: 表 [dbo].[bwwf_Designer_Activities]    脚本日期: 2009-4-21 16:34:48 ******/
CREATE TABLE [dbo].[bwwf_Designer_Activities] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[ActivityId] [uniqueidentifier] NOT NULL ,
	[X] [int] NOT NULL ,
	[Y] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[bwwf_Designer_Activities] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwwf_Designer_Activities] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwwf_Designer_Activities] ADD 
	CONSTRAINT [DF_bwwf_Designer_Activities_X] DEFAULT (0) FOR [X],
	CONSTRAINT [DF_bwwf_Designer_Activities_Y] DEFAULT (0) FOR [Y]
GO
------------------------- 删除 activities 表中的 X, Y, Selected 字段
if exists(select * from dbo.sysobjects
 where id = object_id(N'[dbo].[DF_bwwf_Activities_X]'))
BEGIN
	BEGIN TRANSACTION
	ALTER TABLE dbo.bwwf_Activities
		DROP CONSTRAINT DF_bwwf_Activities_X

	ALTER TABLE dbo.bwwf_Activities
		DROP CONSTRAINT DF_bwwf_Activities_Y

	ALTER TABLE dbo.bwwf_Activities
		DROP CONSTRAINT DF_bwwf_Activities_Selected

	ALTER TABLE dbo.bwwf_Activities
		DROP COLUMN X, Y, Selected

	COMMIT
END
---------------------------------------------------------------------------------
