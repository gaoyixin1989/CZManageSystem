if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fun_getUserDpId]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fun_getUserDpId]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fun_bw_getUserRoleNames]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fun_bw_getUserRoleNames]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[xqp_task_UpdateHrData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[xqp_task_UpdateHrData]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[xqp_wf_GetAuditUsersByOrg]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[xqp_wf_GetAuditUsersByOrg]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bw_Authorizations_Detail]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bw_Authorizations_Detail]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bw_RolesInResources_Enabled]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bw_RolesInResources_Enabled]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bw_UsersInRoles_Enabled]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bw_UsersInRoles_Enabled]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bw_Users_Detail]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bw_Users_Detail]
GO

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

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_Users]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_Users]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bw_UsersInRoles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[bw_UsersInRoles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gziams_OUDetail]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[gziams_OUDetail]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gziams_Leader]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[gziams_Leader]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gziams_UserDetail]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[gziams_UserDetail]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
	获取指定用户的科室部门编号或者部门编号.
*/
CREATE FUNCTION dbo.fun_getUserDpId(
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

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
 获取用户角色名称字符（各角色之间以逗号","隔开）.
 参数：
 @UserId：用户编号.
*/
CREATE FUNCTION dbo.fun_bw_getUserRoleNames (@UserId uniqueidentifier)  
RETURNS nvarchar(2000) AS  
BEGIN 

DECLARE @Roles table(Id int Identity(1, 1), RoleName nvarchar(50))
INSERT INTO @Roles
	SELECT RoleName FROM bw_Roles 
	WHERE RoleId IN(
		SELECT RoleId FROM bw_UsersInRoles WHERE UserId = @UserId
	)

DECLARE @Values nvarchar(2000)
DECLARE @Count int, @Index int
SET @Count = (SELECT COUNT(0) FROM @Roles)
SET @Index = 1
WHILE @Index <= @Count
BEGIN
	IF @Index = 1
		SET @Values = (SELECT RoleName FROM @Roles WHERE ID = @Index)
	ELSE
		SET @Values = @Values + ',' + (SELECT RoleName FROM @Roles WHERE ID = @Index)
	SET @Index = @Index + 1
END

RETURN @Values
END




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
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
	[CreatedTime] [datetime] NOT NULL 
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

CREATE TABLE [dbo].[gziams_OUDetail] (
	[DPID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[DPName] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[ParentDPID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[DPFullName] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[DPLevel] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[IsTmpDP] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[deptOrderNo] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[gziams_Leader] (
	[DPID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[POS] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[UserID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[Sequence] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[gziams_UserDetail] (
	[UserID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[UserName] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[LoginID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[Email] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[GZEmployeeID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[Mobile] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[TelephoneNumber] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[Birthday] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[DPID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[DPName] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[DPCode] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[ParentDPID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[DPFullName] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[JobTypeID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[desJobType] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[EmployeeClassID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[desEmployeeClass] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[SexID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[orderNo] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[UserRole] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[UserLigion] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[IsTmpUser] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[SeniorHrID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[EmployeeID] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL 
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
	CONSTRAINT [DF_bw_Resources_CreatedTime] DEFAULT (getdate()) FOR [CreatedTime]
GO

ALTER TABLE [dbo].[bw_Roles] ADD 
	CONSTRAINT [DF_bw_Roles_IsInherited] DEFAULT (0) FOR [IsInheritable],
	CONSTRAINT [DF_bw_Roles_SortOrder] DEFAULT (10) FOR [SortOrder]
GO

ALTER TABLE [dbo].[bw_Users] ADD 
	CONSTRAINT [DF_bw_Users_UserId] DEFAULT (newid()) FOR [UserId],
	CONSTRAINT [DF_bw_Users_Status] DEFAULT (0) FOR [Status]
GO

 CREATE  INDEX [idx_bw_Users_1] ON [dbo].[bw_Users]([UserName], [EmployeeId]) ON [PRIMARY]
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

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.xqp_task_UpdateHrData

AS
	--同步数据到 bw_Depts 
	DECLARE DeptCursor CURSOR FOR

	SELECT DpId, DpName, ParentDpId, DpFullName, DpLevel, DeptOrderNo, IsTmpDp  FROM dbo.gziams_OUDetail
	
	DECLARE @DpId nvarchar(100)	
	DECLARE @DpName nvarchar(50)
	DECLARE @ParentDpId nvarchar(50)
	DECLARE @DpFullName nvarchar(255)
	DECLARE @DpLevel nvarchar(50)
	DECLARE @DeptOrderNo nvarchar(50)
	DECLARE @IsTmpDp nvarchar(10)
	DECLARE @CreatedTime datetime
	
	SET @CreatedTime = GETDATE()
	
	OPEN DeptCursor
	
	FETCH NEXT FROM DeptCursor INTO @DpId, @DpName, @ParentDpId, @DpFullName, @DpLevel, @DeptOrderNo, @IsTmpDp
	WHILE (@@FETCH_STATUS = 0)
		BEGIN
			EXEC dbo.task_bw_Depts_UpdateData @DpId, @DpName, @ParentDpId, @DpFullName, @DpLevel, @DeptOrderNo, @IsTmpDp, @CreatedTime, @CreatedTime
			FETCH NEXT FROM DeptCursor INTO @DpId, @DpName, @ParentDpId, @DpFullName, @DpLevel, @DeptOrderNo, @IsTmpDp
		END
	CLOSE DeptCursor
	DEALLOCATE DeptCursor


	--同步数据到 bw_Users		
	DECLARE userCursor CURSOR FOR
	SELECT LoginID, Email, Mobile, TelephoneNumber, UserID, UserName, DpId FROM dbo.gziams_UserDetail
	
	DECLARE @UserName nvarchar(100)
	DECLARE @Email nvarchar(256)
	DECLARE @Mobile nvarchar(32)
	DECLARE @Tel nvarchar(32)
	DECLARE @EmployeeId nvarchar(100)
	DECLARE @RealName nvarchar(100)
	DECLARE @Type tinyint
	DECLARE @Status tinyint	
	DECLARE @LastModTime datetime
	
	SET @CreatedTime = GETDATE()
	SET @LastModTime = @CreatedTime
	 
	OPEN userCursor
	
	FETCH NEXT FROM userCursor INTO @UserName, @Email, @Mobile, @Tel, @EmployeeId, @RealName, @DpId
	WHILE (@@FETCH_STATUS = 0)
		BEGIN
			EXEC dbo.task_bw_Users_UpdateData @UserName, @Email, @Mobile, @Tel, @EmployeeId, @RealName, 0, 0, @DpId, null, null, null,null,null, @CreatedTime, @LastModTime
			FETCH NEXT FROM userCursor INTO @UserName, @Email, @Mobile, @Tel, @EmployeeId, @RealName, @DpId
		END
	CLOSE userCursor
	DEALLOCATE userCursor

	-- 内部用户不在同步列表中时，就禁用用户
	UPDATE bw_Users SET Status = 1
	WHERE UserName NOT IN(
		SELECT LoginID FROM gziams_UserDetail
	)
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
*/
CREATE procedure xqp_wf_GetAuditUsersByOrg
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

