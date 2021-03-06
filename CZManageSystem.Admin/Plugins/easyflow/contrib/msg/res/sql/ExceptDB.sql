if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_ExceptionLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_ExceptionLog]
GO

CREATE TABLE [dbo].[bw_ExceptionLog] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Describe] [varchar] (500) COLLATE Chinese_PRC_CI_AS NULL ,
	[ClientIP] [varchar] (32) COLLATE Chinese_PRC_CI_AS NULL ,
	[ServerIP] [varchar] (32) COLLATE Chinese_PRC_CI_AS NULL ,
	[PageURL] [varchar] (200) COLLATE Chinese_PRC_CI_AS NULL ,
	[ExceptionTime] [datetime] NULL ,
	[ExceptionContent] [nvarchar] (1000) COLLATE Chinese_PRC_CI_AS NULL ,
	[StackTrace] [ntext] COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

