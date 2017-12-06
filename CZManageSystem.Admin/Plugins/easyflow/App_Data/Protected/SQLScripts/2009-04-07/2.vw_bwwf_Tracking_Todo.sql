if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vw_bwwf_Tracking_Todo]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vw_bwwf_Tracking_Todo]
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
WHERE (ta.IsCompleted = 0) AND (tw.WorkflowInstanceId IS NOT NULL) AND (td.State <= 1)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

