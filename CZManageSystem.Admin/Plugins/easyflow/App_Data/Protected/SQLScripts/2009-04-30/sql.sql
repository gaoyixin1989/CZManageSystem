if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_bwwf_GetWorkflowSheetId]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_bwwf_GetWorkflowSheetId]
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
	
	IF (ISNULL(@SheetId, '') = '') -- 当别名不存在时设置别名为 N.
	BEGIN
		SET @SheetId = 'N'
	END
	
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

