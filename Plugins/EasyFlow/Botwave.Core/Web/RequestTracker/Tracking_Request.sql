if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Tracking_Request]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Tracking_Request]
GO

CREATE TABLE [dbo].[Tracking_Request] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[Path] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Query] [nvarchar] (1024) COLLATE Chinese_PRC_CI_AS NULL ,
	[UserHostAddress] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[UserAgent] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[BrowserType] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[BrowserName] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[BrowserVersion] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[Platform] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[RequestTime] [datetime] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Tracking_Request] WITH NOCHECK ADD 
	CONSTRAINT [PK_Tracking_Request] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 
GO

