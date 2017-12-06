if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[task_bw_Users_UpdateData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[task_bw_Users_UpdateData]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
/*
用户同步
2009-04-27:新建用户时,需根据用户类型给用户分配默认角色
*/

CREATE    PROCEDURE dbo.task_bw_Users_UpdateData  
@UserName nvarchar(256) ,
@Email nvarchar(256) ,
@Mobile nvarchar(256) ,
@Tel nvarchar(256) ,
@EmployeeId nvarchar(100) ,
@RealName nvarchar(100) ,
@Type tinyint ,
@Status tinyint ,
@DpId nvarchar(100) ,
@Ext_Int int ,
@Ext_Decimal decimal(18,0) ,
@Ext_Str1 nvarchar(512) ,
@Ext_Str2 nvarchar(512) ,
@Ext_Str3 nvarchar(512) ,
@CreatedTime datetime ,
@LastModTime datetime 

AS

DECLARE @Creator nvarchar(50)
DECLARE @LastModifier nvarchar(50)

DECLARE @UserId uniqueidentifier
DECLARE @Password nvarchar(256)
SET @Password = 'zwuXWTTYtCJZiZ62Gb6m7Q=='

SET @Creator='admin'
SET @LastModifier=@Creator

--IF (NOT EXISTS(SELECT EmployeeId FROM dbo.bw_Users WHERE EmployeeId=@EmployeeId))
IF (NOT EXISTS(SELECT UserName FROM dbo.bw_Users WHERE UserName=@UserName))
	BEGIN -- 插入
		SELECT @UserId = newid()
		
		INSERT INTO bw_Users(
			[UserId],[UserName],[Password],[Email],[Mobile],[Tel],[EmployeeId],[RealName],[Type],[Status],[DpId],[Ext_Int],[Ext_Decimal],[Ext_Str1],[Ext_Str2],[Ext_Str3],[CreatedTime],[LastModTime],[Creator],[LastModifier]
		)VALUES(
			@UserId,@UserName,@Password,@Email,@Mobile,@Tel,@EmployeeId,@RealName,@Type,@Status,@DpId,@Ext_Int,@Ext_Decimal,@Ext_Str1,@Ext_Str2,@Ext_Str3,@CreatedTime,@LastModTime,@Creator,@LastModifier
		)
		
		--给新增用户分配默认角色
		DECLARE @DefaultRoleId UNIQUEIDENTIFIER
		IF(@Type = 0)
			SELECT @DefaultRoleId=[RoleId] FROM [bw_Roles] WHERE [RoleName]='内部用户'
		ELSE
			SELECT @DefaultRoleId=[RoleId] FROM [bw_Roles] WHERE [RoleName]='外部用户'
		
		INSERT INTO [bw_UsersInRoles] (
			[UserId],
			[RoleId]
		) VALUES (@UserId,@DefaultRoleId) 
	END
ELSE
	BEGIN -- 更新
		UPDATE bw_Users SET 
			[UserName] = @UserName,[Password] = @Password,[Email] = @Email,[Mobile] = @Mobile,[Tel] = @Tel,[EmployeeId] = @EmployeeId,[RealName] = @RealName,[Type] = @Type,[Status] = @Status,[DpId] = @DpId,[Ext_Int] = @Ext_Int,[Ext_Decimal] = @Ext_Decimal,[Ext_Str1] = @Ext_Str1,[Ext_Str2] = @Ext_Str2,[Ext_Str3] = @Ext_Str3,[CreatedTime] = @CreatedTime,[LastModTime] = @LastModTime,[Creator] = @Creator,[LastModifier] = @LastModifier
		--WHERE EmployeeId = @EmployeeId
		WHERE UserName=@UserName

	END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

