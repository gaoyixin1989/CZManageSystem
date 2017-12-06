if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_ext_GetDoneTasks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[bwwf_ext_GetDoneTasks]
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

-- 会签数据
INSERT INTO @ActivityInstanceIds(ActivityInstanceId)
SELECT ActivityInstanceId FROM bwwf_Tracking_Countersigned
	WHERE [UserName] = @Actor 
	
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

