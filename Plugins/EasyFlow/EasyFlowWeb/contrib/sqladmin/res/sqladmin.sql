if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_SqlAdmin]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_SqlAdmin]
GO

CREATE TABLE [dbo].[bw_SqlAdmin] (
	[UserName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Password] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[bw_SqlAdmin] WITH NOCHECK ADD 
	CONSTRAINT [PK_bw_SqlAdmin] PRIMARY KEY  CLUSTERED 
	(
		[UserName]
	)  ON [PRIMARY] 
GO

insert into [dbo].[bw_SqlAdmin] ([UserName], [Password]) values ('matrix', 'sdfEIf45(])=')