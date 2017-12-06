if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_Activities]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_Activities]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_ActivitySet]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_ActivitySet]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_Assignments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_Assignments]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_Tracking_Activities]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_Tracking_Activities]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_Tracking_Activities_Completed]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_Tracking_Activities_Completed]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_Tracking_Activities_Set]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_Tracking_Activities_Set]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_Tracking_Assignments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_Tracking_Assignments]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_Tracking_Comments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_Tracking_Comments]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_Tracking_Countersigned]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_Tracking_Countersigned]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_Tracking_Todo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_Tracking_Todo]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_Tracking_Workflows]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_Tracking_Workflows]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_Workflows]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_Workflows]
GO

CREATE TABLE [dbo].[bwwf_Activities] (
	[WorkflowId] [uniqueidentifier] NOT NULL ,
	[ActivityId] [uniqueidentifier] NOT NULL ,
	[ActivityName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[State] [int] NOT NULL ,
	[SortOrder] [int] NULL ,
	[PrevActivitySetId] [uniqueidentifier] NULL ,
	[NextActivitySetId] [uniqueidentifier] NULL ,
	[JoinCondition] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[SplitCondition] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[CommandRules] [ntext] COLLATE Chinese_PRC_CI_AS NULL ,
	[ExecutionHandler] [varchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[PostHandler] [varchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[AllocatorResource] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[AllocatorUsers] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[ExtendAllocators] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[ExtendAllocatorArgs] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[DefaultAllocator] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[DecisionType] [varchar] (10) COLLATE Chinese_PRC_CI_AS NULL ,
	[DecisionParser] [varchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[CountersignedCondition] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwwf_ActivitySet] (
	[SetId] [uniqueidentifier] NOT NULL ,
	[ActivityId] [uniqueidentifier] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwwf_Assignments] (
	[ActivityId] [uniqueidentifier] NOT NULL ,
	[AllocatorResource] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[AllocatorUsers] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[ExtendAllocators] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[ExtendAllocatorArgs] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[DefaultAllocator] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwwf_Tracking_Activities] (
	[ActivityInstanceId] [uniqueidentifier] NOT NULL ,
	[PrevSetId] [uniqueidentifier] NULL ,
	[WorkflowInstanceId] [uniqueidentifier] NOT NULL ,
	[ActivityId] [uniqueidentifier] NOT NULL ,
	[IsCompleted] [bit] NOT NULL ,
	[OperateType] [int] NULL ,
	[CreatedTime] [datetime] NULL ,
	[FinishedTime] [datetime] NULL ,
	[Actor] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[Command] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[Reason] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[ExternalEntityType] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[ExternalEntityId] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwwf_Tracking_Activities_Completed] (
	[ActivityInstanceId] [uniqueidentifier] NOT NULL ,
	[PrevSetId] [uniqueidentifier] NULL ,
	[WorkflowInstanceId] [uniqueidentifier] NOT NULL ,
	[ActivityId] [uniqueidentifier] NOT NULL ,
	[IsCompleted] [bit] NOT NULL ,
	[OperateType] [int] NULL ,
	[Actor] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[CreatedTime] [datetime] NULL ,
	[FinishedTime] [datetime] NULL ,
	[Command] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[Reason] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[ExternalEntityType] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[ExternalEntityId] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwwf_Tracking_Activities_Set] (
	[SetId] [uniqueidentifier] NOT NULL ,
	[ActivityInstanceId] [uniqueidentifier] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwwf_Tracking_Assignments] (
	[ActivityInstanceId] [uniqueidentifier] NOT NULL ,
	[AssignedUser] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[AssigningUser] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[AssignedTime] [datetime] NOT NULL ,
	[Message] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwwf_Tracking_Comments] (
	[Id] [uniqueidentifier] NOT NULL ,
	[WorkflowInstanceId] [uniqueidentifier] NOT NULL ,
	[ActivityInstanceId] [uniqueidentifier] NOT NULL ,
	[Message] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Creator] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[CreatedTime] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwwf_Tracking_Countersigned] (
	[ActivityInstanceId] [uniqueidentifier] NOT NULL ,
	[UserName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Message] [nvarchar] (1000) COLLATE Chinese_PRC_CI_AS NULL ,
	[CreatedTime] [datetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwwf_Tracking_Todo] (
	[ActivityInstanceId] [uniqueidentifier] NOT NULL ,
	[UserName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[State] [int] NOT NULL ,
	[ProxyName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[OperateType] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwwf_Tracking_Workflows] (
	[WorkflowInstanceId] [uniqueidentifier] NOT NULL ,
	[WorkflowId] [uniqueidentifier] NOT NULL ,
	[SheetId] [nvarchar] (100) COLLATE Chinese_PRC_CI_AS NULL ,
	[State] [int] NOT NULL ,
	[Creator] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[StartedTime] [datetime] NOT NULL ,
	[FinishedTime] [datetime] NULL ,
	[Title] [nvarchar] (200) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Secrecy] [int] NOT NULL ,
	[Urgency] [tinyint] NULL ,
	[Importance] [tinyint] NULL ,
	[ExpectFinishedTime] [datetime] NULL ,
	[Requirement] [nvarchar] (1000) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwwf_Workflows] (
	[WorkflowId] [uniqueidentifier] NOT NULL ,
	[WorkflowName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Owner] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Enabled] [bit] NOT NULL ,
	[IsCurrent] [bit] NOT NULL ,
	[Version] [int] NOT NULL ,
	[Creator] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Remark] [nvarchar] (3000) COLLATE Chinese_PRC_CI_AS NULL ,
	[LastModifier] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[CreatedTime] [datetime] NOT NULL ,
	[LastModTime] [datetime] NOT NULL ,
	[IsDeleted] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[bwwf_Activities] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwwf_Activities] PRIMARY KEY  CLUSTERED 
	(
		[ActivityId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwwf_ActivitySet] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwwf_ActivitySet] PRIMARY KEY  CLUSTERED 
	(
		[SetId],
		[ActivityId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwwf_Assignments] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwwf_ Assignments] PRIMARY KEY  CLUSTERED 
	(
		[ActivityId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwwf_Tracking_Activities] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwwf_Tracking_UserActivities] PRIMARY KEY  CLUSTERED 
	(
		[ActivityInstanceId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwwf_Tracking_Activities_Completed] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwwf_Tracking_Activies_Completed] PRIMARY KEY  CLUSTERED 
	(
		[ActivityInstanceId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwwf_Tracking_Activities_Set] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwwf_Tracking_Activities_Set] PRIMARY KEY  CLUSTERED 
	(
		[SetId],
		[ActivityInstanceId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwwf_Tracking_Assignments] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwwf_Tracking_Assignments] PRIMARY KEY  CLUSTERED 
	(
		[ActivityInstanceId],
		[AssignedUser],
		[AssigningUser],
		[AssignedTime]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwwf_Tracking_Comments] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwwf_tracking_Comments] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwwf_Tracking_Countersigned] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwwf_Tracking_Countersigned] PRIMARY KEY  CLUSTERED 
	(
		[ActivityInstanceId],
		[UserName]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwwf_Tracking_Todo] WITH NOCHECK ADD 
	CONSTRAINT [PK_xqp_ActivityProcessor] PRIMARY KEY  CLUSTERED 
	(
		[ActivityInstanceId],
		[UserName]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwwf_Tracking_Workflows] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwwf_tracking_WorkflowWorkItems] PRIMARY KEY  CLUSTERED 
	(
		[WorkflowInstanceId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwwf_Workflows] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwwf_Workflows] PRIMARY KEY  CLUSTERED 
	(
		[WorkflowId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bwwf_Activities] ADD 
	CONSTRAINT [DF_bwwf_Activities_WorkflowId] DEFAULT (newid()) FOR [WorkflowId],
	CONSTRAINT [DF_bwwf_Activities_ActivityId] DEFAULT (newid()) FOR [ActivityId],
	CONSTRAINT [DF_bwwf_Activities_State] DEFAULT (0) FOR [State],
	CONSTRAINT [DF_bwwf_Activities_SortOrder] DEFAULT (1) FOR [SortOrder]
GO

ALTER TABLE [dbo].[bwwf_ActivitySet] ADD 
	CONSTRAINT [DF_bwwf_ActivitySet_SetId] DEFAULT (newid()) FOR [SetId]
GO

ALTER TABLE [dbo].[bwwf_Tracking_Activities] ADD 
	CONSTRAINT [DF_bwwf_Tracking_UserActivities_IsCompleted] DEFAULT (0) FOR [IsCompleted],
	CONSTRAINT [DF_bwwf_Tracking_Activities_OperateType] DEFAULT (0) FOR [OperateType],
	CONSTRAINT [DF_bwwf_Tracking_UserActivities_CreatedTime] DEFAULT (getdate()) FOR [CreatedTime]
GO

ALTER TABLE [dbo].[bwwf_Tracking_Comments] ADD 
	CONSTRAINT [DF_bwwf_Tracking_Comments_CreatedTime] DEFAULT (getdate()) FOR [CreatedTime]
GO

ALTER TABLE [dbo].[bwwf_Tracking_Todo] ADD 
	CONSTRAINT [DF_bwwf_Tracking_Todo_IsReaded] DEFAULT (0) FOR [State],
	CONSTRAINT [DF_xqp_ActivityProcessor_ProcessType] DEFAULT (0) FOR [OperateType]
GO

ALTER TABLE [dbo].[bwwf_Tracking_Workflows] ADD 
	CONSTRAINT [DF_bwwf_Tracking_WorkflowWorkItems_Status] DEFAULT (0) FOR [State],
	CONSTRAINT [DF_bwwf_Tracking_WorkflowWorkItems_IsSecrecy] DEFAULT (0) FOR [Secrecy],
	CONSTRAINT [DF_bwwf_Tracking_WorkflowWorkItems_Urgency] DEFAULT (0) FOR [Urgency]
GO

ALTER TABLE [dbo].[bwwf_Workflows] ADD 
	CONSTRAINT [DF_bwwf_Workflows_Enabled] DEFAULT (0) FOR [Enabled],
	CONSTRAINT [DF_bwwf_Workflows_IsCurrent] DEFAULT (0) FOR [IsCurrent],
	CONSTRAINT [DF_bwwf_Workflows_Version] DEFAULT (1) FOR [Version],
	CONSTRAINT [DF_bwwf_Workflows_CreatedTime] DEFAULT (getdate()) FOR [CreatedTime],
	CONSTRAINT [DF_bwwf_Workflows_LastModTime] DEFAULT (getdate()) FOR [LastModTime],
	CONSTRAINT [DF_bwwf_Workflows_IsDeleted] DEFAULT (0) FOR [IsDeleted]
GO

