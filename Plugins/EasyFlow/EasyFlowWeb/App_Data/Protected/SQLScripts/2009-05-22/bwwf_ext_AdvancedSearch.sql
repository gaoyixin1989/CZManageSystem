if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwwf_ext_AdvancedSearch]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[bwwf_ext_AdvancedSearch]
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
	@CreatorName = isnull(@CreatorName, ''), @ProcessorName = isnull(@ProcessorName, ''), @Keywords = isnull(@Keywords, '')

declare @sql nvarchar(4000), @JoinClause nvarchar(2000), @WhereClause nvarchar(500)
select @JoinClause = N'', 
	@WhereClause = N' where tw.State >= 1 and tw.StartedTime >= ''' + @BeginTime + ''' and tw.StartedTime <= ''' + @EndTime + '''';

if (@Title <> '')
	select @WhereClause = @WhereClause + N' and tw.Title like ''' + @Title + '%'''
if (@SheetId <> '')
	select @WhereClause = @WhereClause + N' and tw.SheetId like ''' + @SheetId + '%'''

if (@CreatorName <> '')
	select @JoinClause = @JoinClause + N' left join bw_Users as uc on tw.Creator = uc.UserName',
		@WhereClause = @WhereClause + N' and (uc.RealName like ''' + @CreatorName + '%'' or uc.UserName like ''' + @CreatorName + '%'')';
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
		where u.RealName like ''' + @ProcessorName + '%'' or u.UserName like ''' + @ProcessorName + '%''
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

