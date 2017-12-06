if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_Authorizations]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_Authorizations]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_Depts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_Depts]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_EntityPermissions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_EntityPermissions]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_ExceptionLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_ExceptionLog]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_Resources]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_Resources]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_Roles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_Roles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_RolesInResources]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_RolesInResources]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_SqlAdmin]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_SqlAdmin]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_UserLayout]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_UserLayout]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_Users]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_Users]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_UsersInRoles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_UsersInRoles]
GO

CREATE TABLE [dbo].[bw_Authorizations] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[FromUserId] [uniqueidentifier] NOT NULL ,
	[ToUserId] [uniqueidentifier] NOT NULL ,
	[BeginTime] [datetime] NOT NULL ,
	[IsFullAuthorized] [bit] NOT NULL ,
	[EndTime] [datetime] NOT NULL ,
	[Enabled] [bit] NOT NULL ,
	[CreatedTime] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bw_Depts] (
	[DpId] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[DpName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[ParentDpId] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[DpFullName] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[DpLevel] [int] NULL ,
	[DeptOrderNo] [int] NULL ,
	[IsTmpDp] [bit] NULL ,
	[Type] [tinyint] NULL ,
	[CreatedTime] [datetime] NULL ,
	[LastModTime] [datetime] NULL ,
	[Creator] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[LastModifier] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bw_EntityPermissions] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[EntityType] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[EntityId] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[PermissionType] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[PermissionValue] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bw_ExceptionLog] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Message] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL ,
	[Describe] [varchar] (500) COLLATE Chinese_PRC_CI_AS NULL ,
	[ClientIP] [varchar] (32) COLLATE Chinese_PRC_CI_AS NULL ,
	[ServerIP] [varchar] (32) COLLATE Chinese_PRC_CI_AS NULL ,
	[PageURL] [varchar] (200) COLLATE Chinese_PRC_CI_AS NULL ,
	[ExceptionTime] [datetime] NULL ,
	[ExceptionContent] [nvarchar] (1000) COLLATE Chinese_PRC_CI_AS NULL ,
	[StackTrace] [ntext] COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[bw_Resources] (
	[ResourceId] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[ParentId] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[Type] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Name] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Alias] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL ,
	[Enabled] [bit] NULL ,
	[CreatedTime] [datetime] NOT NULL ,
	[Visible] [bit] NULL ,
	[SortIndex] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bw_Roles] (
	[RoleId] [uniqueidentifier] NOT NULL ,
	[ParentId] [uniqueidentifier] NULL ,
	[IsInheritable] [bit] NULL ,
	[RoleName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Comment] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL ,
	[BeginTime] [datetime] NULL ,
	[EndTime] [datetime] NULL ,
	[CreatedTime] [datetime] NULL ,
	[LastModTime] [datetime] NULL ,
	[Creator] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[LastModifier] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[SortOrder] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bw_RolesInResources] (
	[RoleId] [uniqueidentifier] NOT NULL ,
	[ResourceId] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bw_SqlAdmin] (
	[UserName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Password] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bw_UserLayout] (
	[UserID] [int] NOT NULL ,
	[WindowID] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Order] [int] NULL ,
	[CellNum] [int] NULL ,
	[Custom] [nvarchar] (500) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bw_Users] (
	[UserId] [uniqueidentifier] NOT NULL ,
	[UserName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Password] [nvarchar] (128) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Email] [nvarchar] (128) COLLATE Chinese_PRC_CI_AS NULL ,
	[Mobile] [nvarchar] (16) COLLATE Chinese_PRC_CI_AS NULL ,
	[Tel] [nvarchar] (16) COLLATE Chinese_PRC_CI_AS NULL ,
	[EmployeeId] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[RealName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[Type] [tinyint] NULL ,
	[Status] [int] NULL ,
	[DpId] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[Ext_Int] [int] NULL ,
	[Ext_Decimal] [decimal](18, 0) NULL ,
	[Ext_Str1] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL ,
	[Ext_Str2] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL ,
	[Ext_Str3] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL ,
	[CreatedTime] [datetime] NULL ,
	[LastModTime] [datetime] NULL ,
	[Creator] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[LastModifier] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[bw_UsersInRoles] (
	[UserId] [uniqueidentifier] NOT NULL ,
	[RoleId] [uniqueidentifier] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[bw_Authorizations] WITH NOCHECK ADD 
	CONSTRAINT [PK_bw_Authorizations] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bw_Depts] WITH NOCHECK ADD 
	CONSTRAINT [PK_bw_Depts] PRIMARY KEY  CLUSTERED 
	(
		[DpId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bw_EntityPermissions] WITH NOCHECK ADD 
	CONSTRAINT [PK_bw_EntityPermissions] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bw_Resources] WITH NOCHECK ADD 
	CONSTRAINT [PK_bw_Resources] PRIMARY KEY  CLUSTERED 
	(
		[ResourceId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bw_Roles] WITH NOCHECK ADD 
	CONSTRAINT [PK_bw_Roles] PRIMARY KEY  CLUSTERED 
	(
		[RoleId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bw_RolesInResources] WITH NOCHECK ADD 
	CONSTRAINT [PK_bw_RolesInResources] PRIMARY KEY  CLUSTERED 
	(
		[RoleId],
		[ResourceId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bw_SqlAdmin] WITH NOCHECK ADD 
	CONSTRAINT [PK_bw_SqlAdmin] PRIMARY KEY  CLUSTERED 
	(
		[UserName]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bw_UserLayout] WITH NOCHECK ADD 
	CONSTRAINT [PK_bw_UserLayout] PRIMARY KEY  CLUSTERED 
	(
		[UserID],
		[WindowID]
	) WITH  FILLFACTOR = 70  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bw_Users] WITH NOCHECK ADD 
	CONSTRAINT [PK_bw_Users] PRIMARY KEY  CLUSTERED 
	(
		[UserId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bw_UsersInRoles] WITH NOCHECK ADD 
	CONSTRAINT [PK_bw_UsersInRoles] PRIMARY KEY  CLUSTERED 
	(
		[UserId],
		[RoleId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[bw_Authorizations] ADD 
	CONSTRAINT [DF_bw_Authorizations_CreatedTime] DEFAULT (getdate()) FOR [CreatedTime]
GO

 CREATE  INDEX [IX_bw_Authorizations] ON [dbo].[bw_Authorizations]([FromUserId], [ToUserId], [BeginTime], [EndTime]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[bw_Resources] ADD 
	CONSTRAINT [DF_bw_Resources_Enabled] DEFAULT (1) FOR [Enabled],
	CONSTRAINT [DF_bw_Resources_CreatedTime] DEFAULT (getdate()) FOR [CreatedTime],
	CONSTRAINT [DF_bw_Resources_Visible] DEFAULT (1) FOR [Visible],
	CONSTRAINT [DF_bw_Resources_SortIndex] DEFAULT (1) FOR [SortIndex]
GO

ALTER TABLE [dbo].[bw_Roles] ADD 
	CONSTRAINT [DF_bw_Roles_IsInherited] DEFAULT (0) FOR [IsInheritable],
	CONSTRAINT [DF_bw_Roles_SortOrder] DEFAULT (10) FOR [SortOrder]
GO

ALTER TABLE [dbo].[bw_UserLayout] ADD 
	CONSTRAINT [DF_bw_UserLayout_UserID] DEFAULT (0) FOR [UserID],
	CONSTRAINT [DF_bw_UserLayout_Order] DEFAULT (1) FOR [Order],
	CONSTRAINT [DF_bw_UserLayout_CellNum] DEFAULT (1) FOR [CellNum],
	CONSTRAINT [DF_bw_UserLayout_Custom] DEFAULT (0) FOR [Custom]
GO

ALTER TABLE [dbo].[bw_Users] ADD 
	CONSTRAINT [DF_bw_Users_UserId] DEFAULT (newid()) FOR [UserId],
	CONSTRAINT [DF_bw_Users_Status] DEFAULT (0) FOR [Status]
GO

 CREATE  INDEX [idx_bw_Users_1] ON [dbo].[bw_Users]([UserName], [EmployeeId]) ON [PRIMARY]
GO
-----------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_bw_GetUserDpId]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_bw_GetUserDpId]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
	获取指定用户的科室部门编号或者部门编号.
*/
CREATE FUNCTION dbo.fn_bw_GetUserDpId(
	@UserName nvarchar(50),
	@IsRoomDpId bit -- 是否科室部门编号.是则返回科室部门编号,否则返回部门编号.
)  
RETURNS nvarchar(50)
AS  
BEGIN 

DECLARE @UserDpId nvarchar(50)
SET @UserDpId = (SELECT DpId FROM bw_Users Where UserName = @UserName)

declare @RootDpId nvarchar(50), @DeptDpId nvarchar(50), @RoomDpId nvarchar(50)
declare @LengthOfDpId int
select @LengthOfDpId = len(@UserDpId)
Set @RootDpId = '34920440002'	--广州移动通信公司

--广州部门编码长度为15，科室为17，四分公司的相应地需要减2
if charindex('3492044000201', @UserDpId) > 0	-- 属于广州移动本部
begin
	select @DeptDpId = substring(@UserDpId, 1, 15), @RoomDpId = substring(@UserDpId, 1, 17)
	if @LengthOfDpId < 17 --如果直属于部门，则将科室设置为一个无法取到的值
		set @RoomDpId = @DeptDpId + 'XX'
end
else	--四分公司
begin
	select @DeptDpId = substring(@UserDpId, 1, 13), @RoomDpId = substring(@UserDpId, 1, 15)
	if @LengthOfDpId < 15 --如果直属于部门，则将科室设置为一个无法取到的值
		set @RoomDpId = @DeptDpId + 'XX'
end

DECLARE @Values nvarchar(50)
IF @IsRoomDpId = 1
	SET @Values = @RoomDpId
ELSE
	SET @Values = @DeptDpId

RETURN @Values

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


-----------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bw_Authorizations_Detail]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bw_Authorizations_Detail]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bw_RolesInResources]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bw_RolesInResources]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bw_UsersInRoles_Enabled]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bw_UsersInRoles_Enabled]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bw_Users_Detail]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bw_Users_Detail]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE VIEW dbo.vw_bw_RolesInResources
AS
SELECT dbo.bw_RolesInResources.RoleId, dbo.bw_RolesInResources.ResourceId
FROM dbo.bw_RolesInResources INNER JOIN
      dbo.bw_Resources ON 
      dbo.bw_RolesInResources.ResourceId = dbo.bw_Resources.ResourceId INNER JOIN
      dbo.bw_Roles ON dbo.bw_RolesInResources.RoleId = dbo.bw_Roles.RoleId
WHERE (dbo.bw_Resources.Enabled = 1) AND (dbo.bw_Roles.BeginTime <= GETDATE()) 
      AND (dbo.bw_Roles.EndTime >= GETDATE())


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE VIEW dbo.vw_bw_UsersInRoles_Enabled
AS
SELECT bu.UserId, bu.UserName, bu.EmployeeId, bu.RealName, br.RoleId, 
      br.RoleName
FROM dbo.bw_Users bu INNER JOIN
      dbo.bw_UsersInRoles buir ON bu.UserId = buir.UserId LEFT OUTER JOIN
      dbo.bw_Roles br ON buir.RoleId = br.RoleId
WHERE (br.BeginTime <= GETDATE()) AND (br.EndTime >= GETDATE())


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE VIEW dbo.vw_bw_Users_Detail
AS
SELECT TOP 100 PERCENT u.UserId, u.UserName, u.Password, u.Email, u.Mobile, u.Tel, 
      u.EmployeeId, u.RealName, u.Type, u.Status, u.DpId, u.Ext_Int, u.Ext_Decimal, 
      u.Ext_Str1, u.Ext_Str2, u.Ext_Str3, u.CreatedTime, u.LastModTime, u.Creator, 
      u.LastModifier, dp.DpFullName
FROM dbo.bw_Users u LEFT OUTER JOIN
      dbo.bw_Depts dp ON u.DpId = dp.DpId
ORDER BY u.LastModTime DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE VIEW dbo.vw_bw_Authorizations_Detail
AS
SELECT dbo.bw_Authorizations.Id, dbo.bw_Authorizations.FromUserId, 
      dbo.bw_Authorizations.ToUserId, dbo.bw_Authorizations.IsFullAuthorized, 
      dbo.bw_Authorizations.BeginTime, dbo.bw_Authorizations.EndTime, 
      dbo.bw_Authorizations.Enabled, FromUsers.RealName AS FromRealName, 
      ToUsers.RealName AS ToRealName, ToUsers.DpFullName AS ToDpFullName, 
      FromUsers.UserName, ToUsers.UserName AS ToUserName
FROM dbo.bw_Authorizations LEFT OUTER JOIN
      dbo.bw_Users FromUsers ON 
      dbo.bw_Authorizations.FromUserId = FromUsers.UserId LEFT OUTER JOIN
      dbo.vw_bw_Users_Detail ToUsers ON 
      dbo.bw_Authorizations.ToUserId = ToUsers.UserId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



-----------------------------------------------------------------