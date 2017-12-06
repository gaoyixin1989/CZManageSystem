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

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_WorkflowSettings]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bwwf_WorkflowSettings]
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
	[CountersignedCondition] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[ParallelActivitySetId] [uniqueidentifier] NULL ,
	[RejectOption] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL 
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
	[ExternalEntityId] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[ActorDescription] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL 
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
	[ExternalEntityId] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[ActorDescription] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL 
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
	[Requirement] [nvarchar] (1000) COLLATE Chinese_PRC_CI_AS NULL ,
	[CommentCount] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bwwf_WorkflowSettings] (
	[WorkflowName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[BasicFields] [nvarchar] (4) COLLATE Chinese_PRC_CI_AS NULL ,
	[WorkflowAlias] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[AliasImage] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[TaskNotifyMinCount] [int] NULL ,
	[UndoneMaxCount] [int] NULL 
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

ALTER TABLE [dbo].[bwwf_WorkflowSettings] WITH NOCHECK ADD 
	CONSTRAINT [PK_bwwf_WorkflowSettings] PRIMARY KEY  CLUSTERED 
	(
		[WorkflowName]
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

 CREATE  INDEX [IDX_bwwf_Activities_WorkflowId] ON [dbo].[bwwf_Activities]([WorkflowId]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[bwwf_ActivitySet] ADD 
	CONSTRAINT [DF_bwwf_ActivitySet_SetId] DEFAULT (newid()) FOR [SetId]
GO

ALTER TABLE [dbo].[bwwf_Tracking_Activities] ADD 
	CONSTRAINT [DF_bwwf_Tracking_UserActivities_IsCompleted] DEFAULT (0) FOR [IsCompleted],
	CONSTRAINT [DF_bwwf_Tracking_Activities_OperateType] DEFAULT (0) FOR [OperateType],
	CONSTRAINT [DF_bwwf_Tracking_UserActivities_CreatedTime] DEFAULT (getdate()) FOR [CreatedTime]
GO

 CREATE  INDEX [IDX_bwwf_Tracking_Activities_ActivityId] ON [dbo].[bwwf_Tracking_Activities]([ActivityId]) ON [PRIMARY]
GO

 CREATE  INDEX [IDX_bwwf_Tracking_Activities_WorkflowInstanceId] ON [dbo].[bwwf_Tracking_Activities]([WorkflowInstanceId]) ON [PRIMARY]
GO

 CREATE  INDEX [IDX_bwwf_Tracking_Activities_Completed_ActivityId] ON [dbo].[bwwf_Tracking_Activities_Completed]([ActivityId]) ON [PRIMARY]
GO

 CREATE  INDEX [IDX_bwwf_Tracking_Activities_Completed_WorkflowInstanceId] ON [dbo].[bwwf_Tracking_Activities_Completed]([WorkflowInstanceId]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[bwwf_Tracking_Comments] ADD 
	CONSTRAINT [DF_bwwf_Tracking_Comments_CreatedTime] DEFAULT (getdate()) FOR [CreatedTime]
GO

 CREATE  INDEX [IDX_bwwf_Tracking_Comments_WorkflowInstanceId] ON [dbo].[bwwf_Tracking_Comments]([WorkflowInstanceId]) ON [PRIMARY]
GO

 CREATE  INDEX [IDX_bwwf_Tracking_Comments_ActivityInstanceId] ON [dbo].[bwwf_Tracking_Comments]([ActivityInstanceId]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[bwwf_Tracking_Todo] ADD 
	CONSTRAINT [DF_bwwf_Tracking_Todo_IsReaded] DEFAULT (0) FOR [State],
	CONSTRAINT [DF_xqp_ActivityProcessor_ProcessType] DEFAULT (0) FOR [OperateType]
GO

 CREATE  INDEX [IDX_bwwf_Tracking_Todo_ActivityInstanceId] ON [dbo].[bwwf_Tracking_Todo]([ActivityInstanceId]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[bwwf_Tracking_Workflows] ADD 
	CONSTRAINT [DF_bwwf_Tracking_WorkflowWorkItems_Status] DEFAULT (0) FOR [State],
	CONSTRAINT [DF_bwwf_Tracking_WorkflowWorkItems_IsSecrecy] DEFAULT (0) FOR [Secrecy],
	CONSTRAINT [DF_bwwf_Tracking_WorkflowWorkItems_Urgency] DEFAULT (0) FOR [Urgency],
	CONSTRAINT [DF_bwwf_Tracking_Workflows_CommentCount] DEFAULT (0) FOR [CommentCount]
GO

 CREATE  INDEX [IDX_bwwf_Tracking_Workflows_WorkflowId] ON [dbo].[bwwf_Tracking_Workflows]([WorkflowId]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[bwwf_WorkflowSettings] ADD 
	CONSTRAINT [DF_bwwf_WorkflowSettings_MaxCreationUndone] DEFAULT (0) FOR [UndoneMaxCount]
GO

ALTER TABLE [dbo].[bwwf_Workflows] ADD 
	CONSTRAINT [DF_bwwf_Workflows_Enabled] DEFAULT (0) FOR [Enabled],
	CONSTRAINT [DF_bwwf_Workflows_IsCurrent] DEFAULT (0) FOR [IsCurrent],
	CONSTRAINT [DF_bwwf_Workflows_Version] DEFAULT (1) FOR [Version],
	CONSTRAINT [DF_bwwf_Workflows_CreatedTime] DEFAULT (getdate()) FOR [CreatedTime],
	CONSTRAINT [DF_bwwf_Workflows_LastModTime] DEFAULT (getdate()) FOR [LastModTime],
	CONSTRAINT [DF_bwwf_Workflows_IsDeleted] DEFAULT (0) FOR [IsDeleted]
GO

--------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_bwwf_GetCurrentActivityNames]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_bwwf_GetCurrentActivityNames]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_bwwf_GetCurrentActors]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_bwwf_GetCurrentActors]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_bwwf_GetTodoActors]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_bwwf_GetTodoActors]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_bwwf_GetWorkflowSheetId]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_bwwf_GetWorkflowSheetId]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
获取指定流程实例的当前流程步骤名称字符串.
*/
CREATE  FUNCTION dbo.fn_bwwf_GetCurrentActivityNames(
	@WorkflowInstanceId uniqueidentifier
)  
RETURNS nvarchar(1000)
AS  

BEGIN 

declare @t table(ActivityId uniqueidentifier)
insert into @t (ActivityId)
select ActivityId from bwwf_Tracking_Activities where WorkflowInstanceId = @WorkflowInstanceId

if @@rowcount = 0 
	return '完成'

DECLARE @Values nvarchar(1000)
set @Values = ''
select @Values = @Values + a.ActivityName + ',' from @t as t left join bwwf_Activities as a on t.ActivityId = a.ActivityId

--return @Values
return substring(@Values,0,len(@Values))

/*
DECLARE @Activities table(Id int identity(1, 1) primary key, ActivityName nvarchar(100))

INSERT INTO @Activities(ActivityName)
SELECT ActivityName FROM bwwf_Activities 
WHERE ActivityId IN (
	SELECT ActivityId FROM vw_bwwf_Tracking_Activities
	WHERE WorkflowInstanceId = @WorkflowInstanceId 
		AND ActivityInstanceId NOT IN(
			SELECT tas.ActivityInstanceId FROM bwwf_Tracking_Activities_Set tas
			WHERE tas.SetId IN (SELECT ta2.PrevSetId
		         FROM vw_bwwf_Tracking_Activities ta2
		         WHERE ta2.WorkflowInstanceId = @WorkflowInstanceId)
	)
)


DECLARE @Count int
SET @Count = (SELECT COUNT(0) FROM @Activities)
IF @Count <= 1
	BEGIN
		SET @Values = (SELECT ActivityName FROM @Activities WHERE Id = 1)
	END
ELSE
	BEGIN
		DECLARE @Index int
		SET @Index = 1
		WHILE @Index <= @Count
		BEGIN
			IF @Index = 1
				SET @Values = (SELECT ActivityName FROM @Activities WHERE ID = @Index)
			ELSE
				SET @Values = @Values + ',' + (SELECT ActivityName FROM @Activities WHERE ID = @Index)
			SET @Index = @Index + 1
		END
	END

RETURN @Values*/
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
	获取指定流程实例的当前执行者(用户名/用户真实姓名).
*/
CREATE FUNCTION dbo.fn_bwwf_GetCurrentActors(
	@WorkflowInstanceId uniqueidentifier
)  
RETURNS nvarchar(1000)
AS  

BEGIN 

DECLARE @Actors table([Id] int identity(1, 1) primary key, Actor nvarchar(50), ActorName nvarchar(50))

INSERT INTO @Actors(Actor, ActorName)
SELECT UserName, RealName FROM bw_Users 
WHERE UserName IN(
	SELECT UserName FROM bwwf_Tracking_Todo
	WHERE ActivityInstanceId IN(
		SELECT ActivityInstanceId FROM bwwf_Tracking_Activities
		WHERE (IsCompleted = 0) AND (WorkflowInstanceId = @WorkflowInstanceId)
	)
)

DECLARE @Values nvarchar(1000)
DECLARE @Count int
SET @Count = (SELECT COUNT(0) FROM @Actors)
IF @Count >= 1
	BEGIN
		DECLARE @Index int
		SET @Index = 1
		WHILE @Index <= @Count
		BEGIN
			IF @Index = 1
				SET @Values = (SELECT Actor+'/'+ActorName FROM @Actors WHERE [ID] = @Index)
			ELSE
				SET @Values = @Values + ',' + (SELECT Actor+'/'+ActorName FROM @Actors WHERE [ID] = @Index)
			SET @Index = @Index + 1
		END
	END
ELSE
	BEGIN
		-- 获取流程步骤实例中最后一个完成的执行用户.(完成步骤，只显示用户真实姓名)
		SET @Values = (
			SELECT RealName FROM bw_Users
			WHERE UserName = (
				SELECT TOP 1 Actor FROM  bwwf_Tracking_Activities_Completed
				WHERE WorkflowInstanceId = @WorkflowInstanceId 
				ORDER BY FinishedTime DESC
			)
		)
	END

RETURN @Values

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
	获取指定流程步骤实例的执行者(用户名/用户真实姓名)，排除当前指定 Actor
*/
CREATE FUNCTION dbo.fn_bwwf_GetTodoActors(
	@ActivityInstanceId uniqueidentifier,
	@Actor nvarchar(50)
)  
RETURNS nvarchar(1000)
AS  

BEGIN 

DECLARE @Actors table([Id] int identity(1, 1) primary key, Actor nvarchar(50), ActorName nvarchar(50))

INSERT INTO @Actors(Actor, ActorName)
	SELECT UserName,RealName FROM bw_Users 
	WHERE UserName IN(
		SELECT UserName FROM bwwf_Tracking_Todo
		WHERE ActivityInstanceId = @ActivityInstanceId AND UserName <> @Actor
	)


DECLARE @Values nvarchar(1000)
DECLARE @Count int
SET @Count = (SELECT COUNT(0) FROM @Actors)
IF @Count = 0
	BEGIN
		RETURN ''
	END
ELSE
	BEGIN
		DECLARE @Index int
		SET @Index = 1
		WHILE @Index <= @Count
		BEGIN
			IF @Index = 1
				SET @Values = (SELECT Actor + '/'+ActorName FROM @Actors WHERE [ID] = @Index)
			ELSE
				SET @Values = @Values + ',' + (SELECT Actor + '/'+ ActorName FROM @Actors WHERE [ID] = @Index)
			SET @Index = @Index + 1
		END
	END

RETURN @Values

END



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



-- 生成流程实例工单号
CREATE FUNCTION  dbo.fn_bwwf_GetWorkflowSheetId (@WorkflowId uniqueidentifier, @FlowTime datetime)  
RETURNS nvarchar(20)  AS  
BEGIN 
	DECLARE @SheetId nvarchar(20)
	SELECT @SheetId  =  WorkflowAlias FROM bwwf_WorkflowSettings WHERE WorkflowName =(SELECT W.WorkflowName FROM bwwf_Workflows W  WHERE W.WorkflowId = @WorkflowId)
	DECLARE @No int

	SELECT @No = RIGHT(ISNULL(MAX(SheetId),0), 3) + 1001 
	FROM bwwf_Tracking_Workflows WHERE (WorkflowId IN (
		SELECT WorkflowId FROM bwwf_Workflows WHERE WorkflowName =(
			SELECT WorkflowName FROM bwwf_Workflows WHERE WorkflowId = @WorkflowId)))
	 AND (CONVERT(varchar(10), StartedTime, 120) = CONVERT(varchar(10),@FlowTime,120))

	SET @SheetId = @SheetId + CONVERT(varchar(10), @FlowTime,12) + RIGHT(@No, 3)
	RETURN @SheetId
END




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bwwf_SimpleSearch]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bwwf_SimpleSearch]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bwwf_Tracking_Activities_All_Ext]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bwwf_Tracking_Activities_All_Ext]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bwwf_Tracking_Assignments_Tasks]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bwwf_Tracking_Assignments_Tasks]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bwwf_Tracking_Todo]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bwwf_Tracking_Todo]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bwwf_Tracking_Activities_All]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bwwf_Tracking_Activities_All]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bwwf_Tracking_Workflows_TaskStat]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bwwf_Tracking_Workflows_TaskStat]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bwwf_Tracking_Workflows_TimeStat]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bwwf_Tracking_Workflows_TimeStat]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bwwf_Workflows_Detail]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bwwf_Workflows_Detail]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE VIEW dbo.vw_bwwf_Tracking_Activities_All
AS
(SELECT ActivityInstanceId, PrevSetId, WorkflowInstanceId, ActivityId, IsCompleted, 
      OperateType, CreatedTime, FinishedTime, Actor, Command, Reason, 
      ExternalEntityType, ExternalEntityId, ActorDescription
FROM bwwf_Tracking_Activities)
UNION
(SELECT ActivityInstanceId, PrevSetId, WorkflowInstanceId, ActivityId, IsCompleted, 
      OperateType, CreatedTime, FinishedTime, Actor, Command, Reason, 
      ExternalEntityType, ExternalEntityId, ActorDescription
FROM bwwf_Tracking_Activities_Completed)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



CREATE VIew vw_bwwf_Tracking_Workflows_TaskStat
AS
      --部门统计
	SELECT ISNULL(SUBSTRING(d.DpFullName, CHARINDEX('>', d.DpFullName) + 1, 50), '空') AS StatName, 
		  MAX(tw.StartedTime) AS StartedTime, MAX(tw.FinishedTime) 
	      AS FinishedTime, MAX(CONVERT(varchar(7), tw.FinishedTime, 23)) AS [Time], 
	      CAST(COUNT(*) AS varchar) AS StatInstance, 0 AS SType
	FROM bw_Depts d RIGHT OUTER JOIN
	      bw_Users u ON d.DpId = u.DpId RIGHT OUTER JOIN
	      bwwf_Tracking_Workflows tw ON u.UserName = tw.Creator
	WHERE (ISNULL(tw.FinishedTime, '') <> '')
	GROUP BY d.DpFullName

UNION ALL
      --个人统计
	SELECT u.RealName AS StatName, MAX(tw.StartedTime) AS StartedTime, 
	      MAX(tw.FinishedTime) AS FinishedTime, 
		  MAX(CONVERT(varchar(7), tw.FinishedTime, 23)) AS [Time],
		  CAST(COUNT(*) AS varchar) AS StatInstance, 1 AS SType
	FROM bwwf_Tracking_Workflows tw LEFT OUTER JOIN
	      bw_Users u ON tw.Creator = u.UserName
	WHERE (ISNULL(tw.FinishedTime, '') <> '')
	GROUP BY u.RealName




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE VIEW vw_bwwf_Tracking_Workflows_TimeStat
as
      select Title as StatName,WorkflowInstanceId,StartedTime,FinishedTime,users.RealName AS Creator,WorkFlowId
      from bwwf_Tracking_Workflows as tw
      left join bw_Users as users on tw.Creator = users.UserName
      where State=2





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE VIEW dbo.vw_bwwf_Workflows_Detail
AS
SELECT w.WorkflowId, w.WorkflowName, w.Owner, w.Enabled, w.IsCurrent, w.Version, 
      w.Creator, w.Remark, w.LastModifier, w.CreatedTime, w.LastModTime, w.IsDeleted, 
      ws.WorkflowAlias, ws.AliasImage, ws.BasicFields, ws.TaskNotifyMinCount, 
      ws.UndoneMaxCount
FROM dbo.bwwf_Workflows w LEFT OUTER JOIN
      dbo.bwwf_WorkflowSettings ws ON w.WorkflowName = ws.WorkflowName

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE VIEW dbo.vw_bwwf_SimpleSearch
AS
SELECT tw.WorkflowInstanceId, ws.WorkflowAlias, ws.AliasImage, w.WorkflowName, 
      tw.Title, tw.SheetId, tw.State, 
      CASE tw.State WHEN 2 THEN '完成' WHEN 99 THEN '取消' ELSE dbo.fn_bwwf_GetCurrentActivityNames(tw.WorkflowInstanceId)
       END AS ActivityName, uc.RealName AS CreatorName, tw.StartedTime, 
      dbo.fn_bwwf_GetCurrentActors(tw.WorkflowInstanceId) AS CurrentActors
FROM dbo.bwwf_Tracking_Workflows tw LEFT OUTER JOIN
      dbo.bwwf_Workflows w ON tw.WorkflowId = w.WorkflowId LEFT OUTER JOIN
      dbo.bwwf_WorkflowSettings ws ON 
      w.WorkflowName = ws.WorkflowName LEFT OUTER JOIN
      dbo.bw_Users uc ON tw.Creator = uc.UserName
WHERE (tw.State >= 1)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE VIEW dbo.vw_bwwf_Tracking_Activities_All_Ext
AS
SELECT ta.ActivityInstanceId, ta.PrevSetId, ta.WorkflowInstanceId, ta.ActivityId, 
      ta.IsCompleted, ta.OperateType, ta.CreatedTime, ta.FinishedTime, ta.Actor, 
      ta.Command, ta.Reason, ta.ExternalEntityType, ta.ExternalEntityId, 
      ta.ActorDescription, tw.SheetId, tw.Title AS WorkItemTitle, a.ActivityName, 
      a.CountersignedCondition, a.RejectOption
FROM dbo.vw_bwwf_Tracking_Activities_All ta LEFT OUTER JOIN
      dbo.bwwf_Activities a ON ta.ActivityId = a.ActivityId LEFT OUTER JOIN
      dbo.bwwf_Tracking_Workflows tw ON ta.WorkflowInstanceId = tw.WorkflowInstanceId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE VIEW dbo.vw_bwwf_Tracking_Assignments_Tasks
AS
SELECT assigns.ActivityInstanceId, assigns.AssignedUser, assigns.AssigningUser, 
      assigns.AssignedTime, au.RealName AS AssignedRealName, ta.WorkflowInstanceId, 
      ta.ActivityId, ta.IsCompleted, ta.FinishedTime, a.ActivityName, tw.SheetId, tw.Title, 
      tw.Creator, twu.RealName AS CreatorName, w.WorkflowName, ws.WorkflowAlias, 
      ws.AliasImage, 
      (CASE tw.State WHEN 2 THEN '完成' WHEN 99 THEN '取消' ELSE dbo.fn_bwwf_getCurrentActivityNames(ta.WorkflowInstanceId)
       END) AS CurrentActivityNames, 
      dbo.fn_bwwf_getCurrentActors(ta.WorkflowInstanceId) AS CurrentActors
FROM dbo.bwwf_Tracking_Assignments assigns LEFT OUTER JOIN
      dbo.vw_bwwf_Tracking_Activities_All ta ON 
      ta.ActivityInstanceId = assigns.ActivityInstanceId LEFT OUTER JOIN
      dbo.bwwf_Activities a ON a.ActivityId = ta.ActivityId LEFT OUTER JOIN
      dbo.bwwf_Tracking_Workflows tw ON 
      tw.WorkflowInstanceId = ta.WorkflowInstanceId LEFT OUTER JOIN
      dbo.bw_Users au ON assigns.AssignedUser = au.UserName LEFT OUTER JOIN
      dbo.bw_Users twu ON tw.Creator = twu.UserName LEFT OUTER JOIN
      dbo.bwwf_Workflows w ON w.WorkflowId = tw.WorkflowId LEFT OUTER JOIN
      dbo.bwwf_WorkflowSettings ws ON ws.WorkflowName = w.WorkflowName



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE VIEW dbo.vw_bwwf_Tracking_Todo
AS
SELECT td.ActivityInstanceId, td.UserName, td.State, td.ProxyName, td.OperateType, 
      du.RealName AS ActorName, ta.WorkflowInstanceId, ta.ActivityId, ta.IsCompleted, 
      ta.CreatedTime, ta.FinishedTime, ta.Actor, ta.ExternalEntityType, ta.ExternalEntityId, 
      a.ActivityName, tw.SheetId, tw.Title, tw.Secrecy, tw.Urgency, tw.Importance, 
      w.WorkflowName, ws.WorkflowAlias, ws.AliasImage, tw.Creator, 
      wu.RealName AS CreatorName, tw.StartedTime, 
      dbo.fn_bwwf_GetTodoActors(td.ActivityInstanceId, '') AS TodoActors
FROM dbo.bwwf_Tracking_Todo td LEFT OUTER JOIN
      dbo.bwwf_Tracking_Activities ta ON 
      ta.ActivityInstanceId = td.ActivityInstanceId LEFT OUTER JOIN
      dbo.bwwf_Activities a ON a.ActivityId = ta.ActivityId LEFT OUTER JOIN
      dbo.bwwf_Tracking_Workflows tw ON 
      tw.WorkflowInstanceId = ta.WorkflowInstanceId LEFT OUTER JOIN
      dbo.bw_Users wu ON wu.UserName = tw.Creator LEFT OUTER JOIN
      dbo.bw_Users du ON du.UserName = td.UserName LEFT OUTER JOIN
      dbo.bwwf_Workflows w ON w.WorkflowId = tw.WorkflowId LEFT OUTER JOIN
      dbo.bwwf_WorkflowSettings ws ON ws.WorkflowName = w.WorkflowName
WHERE (ta.IsCompleted = 0) AND (tw.WorkflowInstanceId IS NOT NULL)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

---------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_ext_AdvancedSearch]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[bwwf_ext_AdvancedSearch]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_ext_GetActivitiesStatByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[bwwf_ext_GetActivitiesStatByName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_ext_GetAuditUsersByOrg]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[bwwf_ext_GetAuditUsersByOrg]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_ext_GetDoneTasks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[bwwf_ext_GetDoneTasks]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_ext_GetTaskNodeInstanceStatByTaskInstanceId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[bwwf_ext_GetTaskNodeInstanceStatByTaskInstanceId]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/*
高级查询
输出：类型 标题 受理号 当前步骤 发起人 创建时间 

条件：
工单发起时间 从 到  
流程：   步骤：  
发起人：   处理人：   
标题关键字：   受理号：   
内容关键字： 

认为至少指定了步骤、处理人、内容关键字中的一个条件 */
CREATE procedure dbo.bwwf_ext_AdvancedSearch
(
	@BeginTime varchar(50),
	@EndTime varchar(50),
	@Title	nvarchar(50),
	@SheetId	nvarchar(50),
	@WorkflowName nvarchar(50),
	@ActivityName nvarchar(50),
	@CreatorName nvarchar(50),
	@ProcessorName nvarchar(50),
	@Keywords	nvarchar(50),
	@PageIndex int,
	@PageSize int,
	@RecordCount  int OUTPUT
)
as

select @Title = isnull(@Title, ''), @SheetId = isnull(@SheetId, ''), 
	@WorkflowName = isnull(@WorkflowName, ''), @ActivityName = isnull(@ActivityName, ''),
	@CreatorName = isnull(@CreatorName, ''), @CreatorName = isnull(@ProcessorName, ''), @Keywords = isnull(@Keywords, '')

declare @sql nvarchar(4000), @JoinClause nvarchar(2000), @WhereClause nvarchar(500)
select @JoinClause = N'', 
	@WhereClause = N' where tw.State >= 1 and tw.StartedTime >= ''' + @BeginTime + ''' and tw.StartedTime <= ''' + @EndTime + '''';

if (@Title <> '')
	select @WhereClause = @WhereClause + N' and tw.Title like ''' + @Title + '%'''
if (@SheetId <> '')
	select @WhereClause = @WhereClause + N' and tw.SheetId like ''' + @SheetId + '%'''

if (@CreatorName <> '')
	select @JoinClause = @JoinClause + N' left join bw_Users as uc on tw.Creator = uc.UserName',
		@WhereClause = @WhereClause + N' and uc.RealName like ''' + @CreatorName + '%''';
if (@WorkflowName <> '')
	select @JoinClause = @JoinClause + N' left join bwwf_Workflows as w on tw.WorkflowId = w.WorkflowId',
		@WhereClause = @WhereClause + N' and w.WorkflowName = ''' + @WorkflowName + '''';

if (@ActivityName <> '')
	select @JoinClause = @JoinClause + N' left join 
	(
		select distinct ta.WorkflowInstanceId from bwwf_Tracking_Activities as ta left join bwwf_Activities as a on ta.ActivityId = a.ActivityId 
		where a.ActivityName = ''' + @ActivityName + '''
	) as a on tw.WorkflowInstanceId = a.WorkflowInstanceId',
	@WhereClause = @WhereClause + N' and a.WorkflowInstanceId is not null';
if (@ProcessorName <> '')
	select @JoinClause = @JoinClause + N' left join
	(
		select distinct ta.WorkflowInstanceId from bwwf_Tracking_Activities_Completed as ta left join bw_Users as u on ta.Actor = u.UserName 
		where u.RealName like ''' + @ProcessorName + '%''
	) as pc on tw.WorkflowInstanceId = pc.WorkflowInstanceId',
	@WhereClause = @WhereClause + N' and pc.WorkflowInstanceId is not null';

create table #t (WorkflowInstanceId uniqueidentifier, idx int Identity(1,1))

select @sql = N'insert into #t (WorkflowInstanceId) select tw.WorkflowInstanceId from bwwf_Tracking_Workflows as tw '
	+ @JoinClause + @WhereClause + N' order by tw.StartedTime desc';
exec(@sql)

set @RecordCount = @@rowcount

declare @BeginIndex int, @EndIndex int
select @BeginIndex = (@PageIndex - 1) * @PageSize + 1, @EndIndex = @PageIndex * @PageSize

select tw.WorkflowInstanceId, ws.WorkflowAlias, ws.AliasImage, tw.Title, tw.SheetId, tw.State,
	ActivityName = case tw.State 
		when 2 then '完成'
		when 99 then '取消'
		else dbo.fn_bwwf_GetCurrentActivityNames(tw.WorkflowInstanceId)
	end, 
	uc.RealName as CreatorName, tw.StartedTime,
	dbo.fn_bwwf_GetCurrentActors(tw.WorkflowInstanceId) AS CurrentActors
from #t as t 
	left join bwwf_Tracking_Workflows as tw on t.WorkflowInstanceId = tw.WorkflowInstanceId
	left join bwwf_Workflows as w on tw.WorkflowId = w.WorkflowId
	left join bwwf_WorkflowSettings as ws on w.WorkflowName = ws.WorkflowName
	left join bw_Users as uc on tw.Creator = uc.UserName 
where t.idx >= @BeginIndex and t.idx <= @EndIndex

drop table #t



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*
 获取流程步骤工单数统计
*/
CREATE procedure dbo.bwwf_ext_GetActivitiesStatByName
(
	@WorkflowName nvarchar(50),
	@StartDT datetime,
	@EndDT datetime
)
as

declare @t1 table(ActivityId uniqueidentifier, ActivityName nvarchar(50), InstanceCount int)
declare @t2 table(ActivityName nvarchar(50), InstanceCount int)

insert into @t1  (ActivityId, ActivityName, InstanceCount)
select ActivityId, ActivityName, 0 from bwwf_Activities as a left join bwwf_Workflows as w on a.WorkflowId = w.WorkflowId
where w.WorkflowName = @WorkflowName and w.IsCurrent = 1
order by a.SortOrder

--insert into @t1  (ActivityId, ActivityName, InstanceCount)
--select '00000000-0000-0000-0000-000000000000', '取消', 0

--统计步骤当前工单数
insert into @t2
select t.ActivityName, count(*) as InstanceCount from 
(
select a.ActivityName
from bwwf_Tracking_Activities as ta
	left join bwwf_Activities as a on ta.ActivityId = a.ActivityId
	left join bwwf_Workflows as w on w.WorkflowId = a.WorkflowId
where ta.CreatedTime >= @StartDT and ta.CreatedTime <= @EndDT and w.WorkflowName = @WorkflowName  
) as t
group by t.ActivityName
union all 
select ActivityName='完成', InstanceCount=count(*)
from bwwf_Tracking_Workflows as tw left join bwwf_Workflows as w on w.WorkflowId = tw.WorkflowId
where tw.State = 2 and w.WorkflowName = @WorkflowName
/*union all 
select ActivityName='取消', InstanceCount=count(*)
from bwwf_Tracking_Workflows as tw left join bwwf_Workflows as w on w.WorkflowId = tw.WorkflowId
where tw.State = 99 and w.WorkflowName = @WorkflowName*/

update t1 set 
	t1.InstanceCount = t2.InstanceCount
from @t1 as t1, @t2 as t2 
where t1.ActivityName = t2.ActivityName

select ActivityId as StatID,ActivityName as StatName,InstanceCount as StatInstance from @t1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
根据组织架构获取有审批权限的用户列表
Args：逗号分隔的类型，如1 1,2 
--所有上级主管1
--同部门上级主管2
--直接主管3
--同部门其他人员4
--同科室其他人员5
--室审核6  即同部门内所有室经理 改为本人所在室室经理
--部门审核7 即同部门内所有部门经理 如果有分管的就用分管的，否则正副经理都要
--公司领导审核8 即所有公司领导
可以将多种类型组合在一起
3492044000201                   	广州移动本部
3492044000205                   	番禺分公司
3492044000204                   	增城分公司
3492044000202                   	花都分公司
3492044000203                   	从化分公司
番禺等四分公司实际上是部门
-------------------
涉及表：
bw_Users
gziams_Leader
*/
CREATE procedure dbo.bwwf_ext_GetAuditUsersByOrg
(
	@UserName nvarchar(50),
	@Args nvarchar(50)
)
as

if (@Args is null) or (@Args = '')
begin
	select '' as UserName where 1 > 2
	return
end

declare @DpId nvarchar(50), @RootDpId nvarchar(50), @DeptDpId nvarchar(50), @RoomDpId nvarchar(50)
select @DpId = DpId from bw_Users where UserName = @UserName and Status = 0 and Type = 0

if (@DpId is null)
begin
	select '' as UserName where 1 > 2
	return
end

declare @LengthOfDpId int
select @LengthOfDpId = len(@DpId), @RootDpId = '34920440002'	--广州移动通信公司

--广州部门编码长度为15，科室为17，四分公司的相应地需要减2
if charindex('3492044000201', @DpId) > 0	-- 属于广州移动本部
begin
	select @DeptDpId = substring(@DpId, 1, 15), @RoomDpId = substring(@DpId, 1, 17)
	if @LengthOfDpId < 17 --如果直属于部门，则将科室设置为一个无法取到的值
		set @RoomDpId = @DeptDpId + 'XX'
end
else	--四分公司
begin
	select @DeptDpId = substring(@DpId, 1, 13), @RoomDpId = substring(@DpId, 1, 15)
	if @LengthOfDpId < 15 --如果直属于部门，则将科室设置为一个无法取到的值
		set @RoomDpId = @DeptDpId + 'XX'
end
	

declare @t_users table(UserName nvarchar(50))

--所有上级主管1：转换为 同科室上级主管 + 同部门上级主管 + 公司领导
if charindex('1', @Args) > 0
begin
	--同科室上级主管
	insert into @t_users (UserName)
	select UserName from bw_Users where Status = 0 and EmployeeId in 
	(
		select UserId from gziams_Leader where DpId = @RoomDpId
	);

	set @Args = @Args + ',2,8'
end

--同部门上级主管2
if charindex('2', @Args)>0
	insert into @t_users (UserName)
	select UserName from bw_Users where Status = 0 and EmployeeId in 
	(
		select UserId from gziams_Leader where DpId = @DeptDpId
	)

--直接主管3
if charindex('3', @Args)>0
	insert into @t_users (UserName)
	select UserName from bw_Users where Status = 0 and EmployeeId in 
	(
		select UserId from gziams_Leader where DpId = @DpId
	)

--同部门其他人员4
if charindex('4', @Args)>0
	insert into @t_users (UserName)
	select UserName from bw_Users where Status = 0 and DpId like @DeptDpId + '%'

--同科室其他人员5
if charindex('5', @Args)>0
	insert into @t_users (UserName)
	select UserName from bw_Users where Status = 0 and DpId like @RoomDpId + '%'

--室审核，即同部门内所有室经理 改为本人所在室室经理
if charindex('6', @Args)>0
	insert into @t_users (UserName)
	select UserName from bw_Users where Status = 0 and EmployeeId in 
	(--10为正经理，11、12、13为副经理
		select UserId from gziams_Leader where DpId = @RoomDpId and [POS] in ('10', '11', '12', '13')
	)

--部门审核，即同部门内所有部门经理 改为 本科室的分管领导
if charindex('7', @Args)>0
	insert into @t_users (UserName)
	select UserName from bw_Users where Status = 0 and EmployeeId in 
	(
		select UserId from gziams_Leader where [POS] in ('21', '22') and DpId = @RoomDpId 
	)

--公司领导审核
if charindex('8', @Args)>0
	insert into @t_users (UserName)
	select UserName from bw_Users where Status = 0 and DpId = '349204400020100'	--所在组织为公司领导

--排除用户自己以及重复的目标用户
--select distinct UserName from @t_users where UserName <> @UserName
-- 不排序自己
select distinct UserName from @t_users

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
       待办事宜分页获取记录.
*/
CREATE  PROCEDURE dbo.bwwf_ext_GetDoneTasks
	@WorkflowName nvarchar(50),
	@Actor nvarchar(50),
	@Keywords nvarchar(200),
	@StartTime datetime,
	@EndTime datetime,
	@IsStart bit, 	-- 是否只选中我发起的.
	@PageIndex int,
	@PageSize int
AS

-- 获取当前查询的流程实例范围.
DECLARE @RecordCount int
DECLARE @ActivityInstanceIds table(InstanceIndex int identity(1, 1)  primary key, ActivityInstanceId uniqueidentifier)
INSERT INTO @ActivityInstanceIds(ActivityInstanceId)
SELECT ActivityInstanceId FROM bwwf_Tracking_Activities_Completed
WHERE (IsCompleted = 1) AND (Actor = @Actor) AND (FinishedTime between @StartTime and @EndTime)
	AND (WorkflowInstanceId IN (
			SELECT WorkflowInstanceId FROM bwwf_Tracking_Workflows 
			WHERE (@IsStart = 0 OR Creator = @Actor) 
				AND ((@Keywords = '') OR ((SheetId LIKE '%'+ @Keywords +'%') OR (Title LIKE '%'+ @Keywords +'%') OR (Creator LIKE '%'+ @Keywords +'%')))
				AND WorkflowId IN(
					SELECT WorkflowId  FROM bwwf_Workflows 
					WHERE (@WorkflowName = '') OR (WorkflowName = @WorkflowName)) 
		)
	)
ORDER BY CreatedTime DESC


SET @RecordCount = @@ROWCOUNT --(SELECT COUNT(0) FROM @ActivityInstanceIds)
DECLARE @StartIndex int, @EndIndex int
SET @StartIndex = (@PageIndex) * @PageSize
SET @EndIndex = (@PageIndex + 1) * @PageSize

SELECT ta.ActivityInstanceId, ta.WorkflowInstanceId,ta.PrevSetId,ta.ActivityId, ta.IsCompleted,
	ta.OperateType, ta.Actor, ta.CreatedTime, ta.FinishedTime,ta.Command, 
	a.ActivityName, tw.SheetId, tw.Title, tw.StartedTime,
	(CASE tw.State WHEN 2 THEN '完成' WHEN 99 THEN '取消'
		ELSE dbo.fn_bwwf_GetCurrentActivityNames(ta.WorkflowInstanceId)END) CurrentActivityNames,
	dbo.fn_bwwf_GetCurrentActors(ta.WorkflowInstanceId) CurrentActors,
	ws.WorkflowAlias, ws.AliasImage, uc.RealName CreatorName
FROM(SELECT ActivityInstanceId, WorkflowInstanceId,PrevSetId,ActivityId, IsCompleted,
		 OperateType, Actor, CreatedTime, FinishedTime,Command
	FROM bwwf_Tracking_Activities_Completed 
	WHERE ActivityInstanceId IN (
		SELECT ActivityInstanceId FROM @ActivityInstanceIds 
		WHERE InstanceIndex between @StartIndex and @endindex)
) ta
	LEFT JOIN bwwf_Activities a ON a.ActivityId = ta.ActivityId
	LEFT JOIN bwwf_Tracking_Workflows tw ON tw.WorkflowInstanceId = ta.WorkflowInstanceId
	LEFT JOIN bwwf_Workflows w ON w.WorkflowId = tw.WorkflowId
	LEFT JOIN bwwf_WorkflowSettings ws ON ws.WorkflowName = w.WorkflowName
	LEFT JOIN bw_Users uc ON uc.UserName = tw.Creator
ORDER BY ta.FinishedTime DESC
--ORDER BY ta.CreatedTime DESC

RETURN @RecordCount


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*
功能：根据任务实例编号返回任务的相关统计
参数：
	@taskInstanceId：任务实例编号
返回参数描述：
	unprocessed：待处理实例数
	processed：已处理实例数
	audited：已审核实例数
	unAudited：待审核实例数
	notAudited：审核不通过实例数
	readed:已读
	unReaded:未读
*/
CREATE Proc dbo.bwwf_ext_GetTaskNodeInstanceStatByTaskInstanceId
@taskInstanceId nvarchar(64)
AS

select unprocessed= (select count(*) from bwtask_NodeInstances as ni
	                        left join bwtask_Nodes as n on ni.TaskNodeId = n.[Id]
	                        left join bwtask_Instances as i on ni.TaskInstanceId = i.[Id]
                           		 where (ni.Status = 0 or ni.Status=1) and n.NodeType='process' and ni.TaskInstanceId=@taskInstanceId)      
,processed=(select count(*) from bwtask_NodeInstances as ni
	                        left join bwtask_Nodes as n on ni.TaskNodeId = n.[Id]
	                        left join bwtask_Instances as i on ni.TaskInstanceId = i.[Id]
                           		 where  ni.Status=2 and n.NodeType='process' and ni.TaskInstanceId=@taskInstanceId )
,readed=(select count(*) from bwtask_NodeInstances as ni
	                        left join bwtask_Nodes as n on ni.TaskNodeId = n.[Id]
	                        left join bwtask_Instances as i on ni.TaskInstanceId = i.[Id]
                           		 where  (ni.Status=2 or ni.Status=1) and n.NodeType='process' and ni.TaskInstanceId=@taskInstanceId )
,unReaded=(select count(*) from bwtask_NodeInstances as ni
	                        left join bwtask_Nodes as n on ni.TaskNodeId = n.[Id]
	                        left join bwtask_Instances as i on ni.TaskInstanceId = i.[Id]
                           		 where  ni.Status=0  and n.NodeType='process' and ni.TaskInstanceId=@taskInstanceId )
,audited=(select count(*) from bwtask_NodeInstances as ni
	                        left join bwtask_Nodes as n on ni.TaskNodeId = n.[Id]
	                        left join bwtask_Instances as i on ni.TaskInstanceId = i.[Id]
                           		 where  ni.Status=2 and n.NodeType='audit' and ni.TaskInstanceId=@taskInstanceId )
,unAudited=(select count(*) from bwtask_NodeInstances as ni
	                        left join bwtask_Nodes as n on ni.TaskNodeId = n.[Id]
	                        left join bwtask_Instances as i on ni.TaskInstanceId = i.[Id]
                           		 where  ni.Status=0 and n.NodeType='audit' and ni.TaskInstanceId=@taskInstanceId )
,notAudited=(select count(*) from bwtask_NodeInstances as ni
	                        left join bwtask_Nodes as n on ni.TaskNodeId = n.[Id]
	                        left join bwtask_Instances as i on ni.TaskInstanceId = i.[Id]
                           		 where  ni.Status=3 and n.NodeType='audit' and ni.TaskInstanceId=@taskInstanceId )
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
---------------------------------------------------------------------------------------------
