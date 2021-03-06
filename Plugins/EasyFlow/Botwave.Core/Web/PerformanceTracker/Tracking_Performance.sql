if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Tracking_Performance]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Tracking_Performance]
GO

CREATE TABLE [dbo].[Tracking_Performance] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[Resource] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Arguments] [nvarchar] (1024) COLLATE Chinese_PRC_CI_AS NULL ,
	[ActionTime] [datetime] NOT NULL ,
	[CostInterval] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Tracking_Performance] WITH NOCHECK ADD 
	CONSTRAINT [PK_Tracking_Performance] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 
GO

