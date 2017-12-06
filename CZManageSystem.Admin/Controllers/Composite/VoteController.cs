using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Composite;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CZManageSystem.Admin.Base.FlowInstance;

namespace CZManageSystem.Admin.Controllers.Composite
{
    public class VoteController : BaseController
    {
        // GET: Vote 
        #region Fields
        IVoteApplyService voteApplyService = new VoteApplyService();
        IVoteAnserService voteAnserService = new VoteAnserService();
        IVoteAppQidService voteAppQidService = new VoteAppQidService();
        IVoteJoinPersonService voteJoinPersonService = new VoteJoinPersonService();
        IVoteQuestionService voteQuestionService = new VoteQuestionService();
        IV_VoteDetailService v_VoteDetailService = new V_VoteDetailService();
        IVoteRoleService voteRoleService = new VoteRoleService();
        IVoteSelectedAnserService voteSelectedAnserService = new VoteSelectedAnserService();
        IVoteThemeInfoService voteThemeInfoService = new VoteThemeInfoService();
        IVoteTidQieService voteTidQieService = new VoteTidQieService();
        IVoteQuestionTempService voteQuestionTempService = new VoteQuestionTempService();
        ISysDeptmentService sysDeptmentService = new SysDeptmentService();
        ISysUserService sysUserService = new SysUserService();
        IVoteTrackingTodoService voteTrackingTodoService = new VoteTrackingTodoService();
        #endregion
        #region ActionResult
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexMy()
        {
            return View();
        }
        /// <summary>
        /// 我的投票列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="applyTitle"></param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 10, string applyTitle = null)
        {
            int count = 0;
            object obj = WorkContext.CurrentUser.UserName == Base.Admin.UserName ?
                (obj = new { ApplyTitle = applyTitle }) :
                (obj = new { ApplyTitle = applyTitle, CreatorID = WorkContext.CurrentUser.UserId });
            var modelList = voteApplyService.GetForPaging(out count, obj, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);

            return Json(new { items = modelList, count = count });

        }

        /// <summary>
        /// 投票列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="applyTitle"></param>
        /// <returns></returns>
        public ActionResult GetListData_(int pageIndex = 1, int pageSize = 10, string applyTitle = null)
        {
            int count = 0;
            var modelList = voteApplyService.GetForPaging_(out count, WorkContext.CurrentUser, new { ApplyTitle = applyTitle }, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);


            return Json(new { items = modelList, count = count });

        }
        public ActionResult GetQuestionTemp()
        {
            var modelList = voteQuestionTempService.List().Where(q => q.CreatorID == WorkContext.CurrentUser.UserId).Select(v => new
            {
                v.AnswerNum,
                v.Creator,
                v.CreateTime,
                v.CreatorID,
                v.IsDel,
                v.MaxValue,
                v.MinValue,
                v.QuestionID,
                v.QuestionTitle,
                v.QuestionType,
                v.Remark,
                v.State,
                VoteAnsers = v.VoteAnserTemps.Select(a => new
                {
                    a.AnserContent,
                    a.AnserID,
                    a.AnserScore,
                    a.MaxValue,
                    a.MinValue,
                    a.QuestionID,
                    a.SortOrder
                })
            }).ToList();

            return Json(modelList);
        }
        public ActionResult Detail(int? ThemeID = null)
        {
            ViewData["ThemeID"] = ThemeID;// != null ? QuestionID : "0";
            return View();
        }


        /// <summary>
        /// 下载数据
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download(string id = null)
        {
            try
            {
                int _id = 0;

                if (!int.TryParse(id, out _id))
                    return null;

                var voteApplyList = voteApplyService.FindById(_id);
                var modelList = voteApplyList?.VoteThemeInfo?.VoteTidQies?.Select(s => s.VoteQuestion).ToList();
                #region MyRegion
                //    ?.Select(v => new
                //{
                //    v.VoteQuestion.AnswerNum,
                //    v.VoteQuestion.Creator,
                //    v.VoteQuestion.CreateTime,
                //    v.VoteQuestion.CreatorID,
                //    v.VoteQuestion.IsDel,
                //    v.VoteQuestion.MaxValue,
                //    v.VoteQuestion.MinValue,
                //    v.VoteQuestion.QuestionID,
                //    v.VoteQuestion.QuestionTitle,
                //    QuestionType = GetQuestionType(v.VoteQuestion.QuestionType),
                //    v.VoteQuestion.Remark,
                //    v.VoteQuestion.State,
                //    //VoteAnsers= new string [] {"1","2" ,"3"}
                //    VoteAnsers = v?.VoteQuestion?.VoteAnsers?.Select(a => new
                //    {
                //        a.AnserContent,
                //        a.AnserScore
                //    })
                //}).ToList(); 
                #endregion
                if (modelList.Count < 1) 
                    return View("../Export/Message");
                //找出最长的列
                int columnsNum = 0;
                foreach (var item in modelList)
                {
                    int tempNum = item.VoteAnsers.Count();
                    if (tempNum > columnsNum)
                    {
                        columnsNum = tempNum;
                    }
                }
                int index = 0;
                string[] strT = new string[columnsNum * 2];
                string[] strD = new string[columnsNum * 2 + 4];
                strD[0] = "&=&={-2}";
                strD[1] = "&=Question.QuestionTitle";
                strD[2] = "&=Question.QuestionType";
                strD[3] = "&=Question.AnswerNum";
                for (int i = 0; i < columnsNum; i++)
                {

                    strT[index] = "选项内容_" + i;
                    strD[index + 4] = "&=Question.ColumnsC" + i;
                    index++;
                    strT[index] = "分数_" + i + "（空值默认0分）";
                    strD[index + 4] = "&=Question.ColumnsS" + i;
                    index++;
                }
                var dt = ListToDataTable(modelList, columnsNum);
                dt.TableName = ImportFileType.Question;




                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.Question + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Question);
                //设置集合变量
                designer.SetDataSource("strT", strT);
                designer.SetDataSource("strD", strD);
                designer.Process();
                designer.Save(ExportTempPath.Question_, FileFormatType.Excel2003XML);//保存格式

                designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Question_);
                designer.SetDataSource(dt);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();

                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }

        public ActionResult GetDetailList(int pageIndex = 1, int pageSize = 10, string ApplyTitle = null, int? ThemeID = null)
        {
            int count = 0; //voteSelectedAnserService
            var modelList = v_VoteDetailService.GetForPaging(out count, new { CreatorID = WorkContext.CurrentUser.UserId, ApplyTitle, ThemeID }, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });

        }
        public ActionResult Edit(string id = null)
        {
            //有值=》编辑状态
            ViewData["id"] = id != null ? id : "0";
            ViewData["workflowName"] = WorkflowType.VoteApply;
            return View();
        }
        public ActionResult GetDataByID(string id, string type = null, bool isVoting = false, bool isView = false)
        {
            try
            {
                int _applyID = 0;
                int? ThemeID = null;
                bool IsShow = true;
                bool IsAction = false;

                int.TryParse(id, out _applyID);
                BasicInfoViewModel BasicInfo = new BasicInfoViewModel();
                VoteApply voteApply = new VoteApply();
                object Questions = new object();
                if (_applyID != 0)
                {
                    #region 修改
                    voteApply = voteApplyService.FindByFeldName(a => a.ApplyID == _applyID);
                    BasicInfo.ApplyID = voteApply.ApplyID;
                    BasicInfo.ApplySn = voteApply.ApplySn;
                    BasicInfo.ApplyTitle = voteApply.ApplyTitle;
                    BasicInfo.CreateTime = voteApply.CreateTime;
                    BasicInfo.Creator = voteApply.Creator;
                    BasicInfo.CreatorID = voteApply.CreatorID;

                    //var dpID = sysUserService.FindByFeldName(u => u.UserId == voteApply.CreatorID)?.DpId;
                    //if (dpID != null)
                    BasicInfo.DeptName = WorkContext.CurrentUser.Dept.DpName;//sysDeptmentService.FindByFeldName(d => d.DpId == dpID)?.DpName;
                    BasicInfo.StartTime = voteApply.StartTime;
                    BasicInfo.EndTime = voteApply.EndTime;
                    ThemeID = voteApply.ThemeID;
                    IsShow = CompareDate(voteApply.StartTime, voteApply.EndTime);
                    IsAction = !string.IsNullOrEmpty(voteApply.WorkflowInstanceId?.ToString()) ? true : false;
                    BasicInfo.IsProc = voteApply.IsProc;
                    BasicInfo.MemberName = voteApply.MemberName;
                    BasicInfo.MemberIDs = voteApply.MemberIDs;
                    BasicInfo.MemberType = voteApply.MemberType;
                    BasicInfo.Remark = voteApply.Remark;
                    BasicInfo.MobilePhone = voteApply.MobilePhone;
                    BasicInfo.ThemeName = voteApply.VoteThemeInfo?.ThemeName;
                    BasicInfo.ThemeType = voteApply.ThemeType;
                    Questions = voteApply.VoteThemeInfo?.VoteTidQies?.Select(q => q.VoteQuestion)?.Select(v => new
                    {
                        v.AnswerNum,
                        v.Creator,
                        v.CreateTime,
                        v.CreatorID,
                        v.IsDel,
                        v.MaxValue,
                        v.MinValue,
                        v.QuestionID,
                        v.QuestionTitle,
                        v.QuestionType,
                        v.Remark,
                        v.State,
                        VoteAnsers = v.VoteAnsers.Select(a => new
                        {
                            a.AnserContent,
                            a.AnserID,
                            AnserScore = a.AnserScore ?? 0,
                            a.MaxValue,
                            a.MinValue,
                            a.QuestionID,
                            a.SortOrder,
                            AnsersCount = string.IsNullOrEmpty(type) ? 0 : a.VoteSelectedAnsers.Count
                        }),
                        QuestionCount = string.IsNullOrEmpty(type) ? 0 : v.VoteSelectedAnsers.Count,
                        VoteSelectedAnsers = string.IsNullOrEmpty(type) ? new List<VoteSelectedAnser>().Select(lv => new
                        {
                            lv.OtherContent,
                            lv.Respondent
                        })
                        :
                        v.VoteSelectedAnsers.Select(l => new
                        {
                            l.OtherContent,
                            l.Respondent  //sysUserService
                                          //.List ().Where(u => u.UserId == l.UserID).FirstOrDefault ().RealName 
                                          // .FindByFeldName (u=>u.UserId == l.UserID ).RealName 
                        })


                    }
                    );
                    #endregion
                }
                else
                {
                    #region 增加
                    var user = WorkContext.CurrentUser;
                    var date = DateTime.Now;
                    //BasicInfo.ApplyID = 0; 
                    BasicInfo.ApplySn = "提交成功后生成";// date.ToFileTimeUtc().ToString();
                    BasicInfo.ApplyTitle = user.RealName + "的投票申请(" + date.ToString("yyyy-MM-dd") + ")";
                    BasicInfo.ThemeType = "投票";
                    BasicInfo.CreateTime = date;
                    BasicInfo.Creator = user.RealName;
                    BasicInfo.CreatorID = user.UserId;
                    BasicInfo.DeptName = WorkContext.CurrentUser.Dept.DpName;// sysDeptmentService.FindByFeldName(d => d.DpId == user.DpId)?.DpName;
                    BasicInfo.StartTime = date;
                    BasicInfo.EndTime = date;
                    BasicInfo.MobilePhone = user.Mobile;
                    //BasicInfo.ThemeName = voteApply.VoteThemeInfo?.ThemeName;
                    //BasicInfo.ThemeType = voteApply.ThemeType;

                    Questions = voteQuestionTempService.List().Where(q => q.CreatorID == user.UserId).Select(v => new
                    {
                        v.AnswerNum,
                        v.Creator,
                        v.CreateTime,
                        v.CreatorID,
                        v.IsDel,
                        v.MaxValue,
                        v.MinValue,
                        v.QuestionID,
                        v.QuestionTitle,
                        v.QuestionType,
                        v.Remark,
                        v.State,
                        VoteAnsers = v.VoteAnserTemps.Select(a => new
                        {
                            a.AnserContent,
                            a.AnserID,
                            a.AnserScore,
                            a.MaxValue,
                            a.MinValue,
                            a.QuestionID,
                            a.SortOrder
                        })
                    }).ToList();
                    #endregion

                }
                if (isVoting)
                {
                    isVoting = voteJoinPersonService.Contains(c => c.UserID == WorkContext.CurrentUser.UserId && c.ThemeID == voteApply.ThemeID);
                }
                if (isView)//是否是查看、是的话提交按钮不需要显示出来
                    IsShow = false;
                return Json(new { BasicInfo = BasicInfo, Questions = Questions, IsShow = IsShow, IsAction = IsAction, ThemeID = ThemeID, IsVoting = isVoting });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult Voting(string id = null, bool isView = false)
        {
            ViewData["id"] = id != null ? id : "0";
            ViewData["isView"] = isView;
            return View();
        }
        public ActionResult Statistics(string id = null)
        {
            ViewData["id"] = id != null ? id : "0";
            return View();
        }
        public ActionResult SaveVoting(List<SelectedAnserViewModel> list, int themeID)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            if (list.Count <= 0)
            {
                result.Message = "提交答题数据为空！";
                return Json(result);
            }
            var apply = voteApplyService.FindByFeldName(a => a.VoteThemeInfo.ThemeID == themeID);
            if (apply == null)
            {
                result.Message = "当前主题不存在！";
                return Json(result);
            }
            if (voteJoinPersonService.Contains(c => c.UserID == WorkContext.CurrentUser.UserId && c.ThemeID == themeID))
            {
                result.Message = "您已经提交过答卷！";
                return Json(result);
            }
            List<VoteSelectedAnser> voteSelectedAnserList = new List<VoteSelectedAnser>();
            foreach (var item in list)
            {
                voteSelectedAnserList.Add(
                    new VoteSelectedAnser()
                    {
                        QuestionID = item.QuestionID,
                        AnserID = item.AnserID,
                        OtherContent = item.OtherContent,
                        UserID = WorkContext.CurrentUser.UserId,
                        Respondent = WorkContext.CurrentUser.RealName
                    }
                    );
            }
            if (voteSelectedAnserService.InsertByList(voteSelectedAnserList))
            {
                VoteJoinPerson voteJoinPerson = new VoteJoinPerson()
                {
                    ThemeID = themeID,
                    UserID = WorkContext.CurrentUser.UserId,
                    UserName = WorkContext.CurrentUser.UserName,
                    RealName = WorkContext.CurrentUser.RealName,
                    Remark = ""
                };
                if (voteJoinPersonService.Insert(voteJoinPerson))
                {
                    var resultModel = voteTrackingTodoService.FindByFeldName(f => f.UserName == WorkContext.CurrentUser.UserName && f.ActivityInstanceId == apply.WorkflowInstanceId);
                    resultModel.IsCompleted = true;
                    resultModel.State = 2;
                    resultModel.FinishedTime = DateTime.Now;
                    result.IsSuccess = voteTrackingTodoService.Update(resultModel);
                }

            }
            return Json(result);
        }
        public ActionResult UpdateEndTimeIndex(int id)
        {
            var apply = voteApplyService.FindById(id);
            if (apply != null)
            {
                ViewData["ApplyID"] = apply.ApplyID;
                ViewData["StartTime"] = Convert.ToDateTime(apply.StartTime).ToString("yyyy-MM-dd HH:mm");
                ViewData["EndTime"] = Convert.ToDateTime(apply.EndTime).ToString("yyyy-MM-dd HH:mm");
            }
            return View();
        }
        /// <summary>
        /// 投票申请完成时进行推送
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UpdateEndTime(int ApplyID, DateTime StartTime, DateTime EndTime)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (StartTime>= EndTime)
            {
                result.Message = "开始时间不能大于结束时间！";
                return Json(result);
            }
            var apply = voteApplyService.FindById(ApplyID);
            if (apply != null)
            {
                apply.StartTime = StartTime;
                apply.EndTime = EndTime;
                result.IsSuccess = voteApplyService.Update(apply);
            }
            return Json(result);
        }
        /// <summary>
        /// 投票申请完成时进行推送
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PushVoteWork(int id)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                var voteApply = voteApplyService.FindByFeldName(a => a.ApplyID == id);
                if (voteApply == null)
                    return Json(result);
                if (voteApply.IsProc == 1)//检查是否已经处理推送了当前投票。防止重复
                    return Json(result);
                var userIds = voteApply.MemberIDs.Split(new char[] { ',' });
                var user = sysUserService.FindByFeldName(f => f.UserId == voteApply.CreatorID);
                List<VoteTrackingTodo> list = new List<VoteTrackingTodo>();
                for (int i = 0; i < userIds.Length; i++)
                {
                    if (string.IsNullOrEmpty(userIds[i]))//避免无效空字符
                        continue;
                    var userId = new Guid(userIds[i]);
                    var actor = sysUserService.FindByFeldName(f => f.UserId == userId);
                    if (actor == null)
                        continue;
                    list.Add(new VoteTrackingTodo()
                    {
                        ActivityInstanceId = new Guid(voteApply.WorkflowInstanceId?.ToString()),
                        ActorName = actor.RealName,
                        UserName = actor.UserName,
                        CreatedTime = voteApply.CreateTime,
                        StartedTime = voteApply.StartTime,
                        Creator = user?.UserName,
                        CreatorName = voteApply.Creator,
                        IsCompleted = false,
                        State = 0,
                        Title = voteApply.ApplyTitle,
                        OperateType = 0,
                        SheetId = voteApply.ApplySn,
                        ExternalEntityId = voteApply.ApplyID.ToString(),
                        ExternalEntityType = "0"
                    });
                }

                if (voteTrackingTodoService.InsertByList(list))
                {
                    voteApply.IsProc = 1;
                    if (voteApplyService.Update(voteApply))
                        result.IsSuccess = true;

                }
                //记录操作日志
                _sysLogService.WriteSysLog<SysOperationLog>(new SysOperationLog()
                {
                    Operation = OperationType.Push,
                    OperationDesc = (result.IsSuccess ? "投票推送成功:" : "投票推送失败:") + list.Count + "条",
                    OperationPage = Request.RawUrl,
                    RealName = WorkContext.CurrentUser.RealName,
                    UserName = WorkContext.CurrentUser.UserName
                });
            }
            catch (Exception ex)
            {
                result.Message = "投票推送失败！";// ex.Message;
                //记录错误日志
                _sysLogService.WriteSysLog<SysErrorLog>(new SysErrorLog()
                {
                    ErrorDesc = ex.ToString(),
                    ErrorPage = Request.RawUrl,
                    ErrorTitle = "投票推送失败",
                    RealName = WorkContext.CurrentUser.RealName,
                    UserName = WorkContext.CurrentUser.UserName
                });

            }
            return Json(result);
        }
        public ActionResult Save(BasicInfoViewModel applyViewModel, bool isAction = false, string nextActivity = "", string nextActors = "", string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            VoteApply voteApply = new VoteApply();
            VoteThemeInfo voteThemeInfo = new VoteThemeInfo();
            #region 申请单
            if (applyViewModel.ApplyID > 0)
            {
                voteApply = voteApplyService.FindByFeldName(a => a.ApplyID == applyViewModel.ApplyID);
                if (voteApply == null)
                {
                    result.Message = "当前记录不存在！";
                    return Json(result);
                }
                if (voteApply.TrackingWorkflow?.State == 2)
                {
                    result.Message = "当前记录已经审核通过，无法编辑！";
                    return Json(result);
                }
                voteApply.ApplySn = applyViewModel.ApplySn;
                voteApply.ApplyTitle = applyViewModel.ApplyTitle;
                voteApply.CreateTime = applyViewModel.CreateTime;
                voteApply.Creator = applyViewModel.Creator;
                voteApply.CreatorID = applyViewModel.CreatorID;
                voteApply.StartTime = applyViewModel.StartTime;
                voteApply.EndTime = applyViewModel.EndTime;
                voteApply.IsProc = applyViewModel.IsProc;
                voteApply.MemberName = applyViewModel.MemberName;
                voteApply.MemberIDs = applyViewModel.MemberIDs;
                voteApply.MemberType = applyViewModel.MemberType;
                voteApply.Remark = applyViewModel.Remark;
                voteApply.MobilePhone = applyViewModel.MobilePhone;
                voteApply.VoteThemeInfo.ThemeName = applyViewModel.ThemeName;
                voteApply.ThemeType = applyViewModel.ThemeType;

            }
            else
            {

                voteApply.ApplySn = applyViewModel.ApplySn;
                voteApply.ApplyTitle = applyViewModel.ApplyTitle;
                voteApply.CreateTime = applyViewModel.CreateTime;
                voteApply.Creator = applyViewModel.Creator;
                voteApply.CreatorID = applyViewModel.CreatorID;
                voteApply.StartTime = applyViewModel.StartTime;
                voteApply.EndTime = applyViewModel.EndTime;
                voteApply.IsProc = applyViewModel.IsProc;
                voteApply.MemberName = applyViewModel.MemberName;
                voteApply.MemberIDs = applyViewModel.MemberIDs;
                voteApply.MemberType = applyViewModel.MemberType;
                voteApply.Remark = applyViewModel.Remark;
                voteApply.MobilePhone = applyViewModel.MobilePhone;
                voteApply.VoteThemeInfo = new VoteThemeInfo();
                voteApply.VoteThemeInfo.ThemeName = applyViewModel.ThemeName;
                voteApply.ThemeType = applyViewModel.ThemeType;

            }
            #endregion

            #region 问题
            var Questions = voteQuestionTempService.List().Where(q => q.CreatorID == WorkContext.CurrentUser.UserId).ToList();

            foreach (var item in Questions)
            {
                VoteQuestion voteQuestion = new VoteQuestion();
                VoteTidQie voteTidQie = new VoteTidQie();
                voteQuestion.AnswerNum = item.AnswerNum;
                voteQuestion.CreateTime = item.CreateTime;
                voteQuestion.Creator = item.Creator;
                voteQuestion.CreatorID = item.CreatorID;
                voteQuestion.MaxValue = item.MaxValue;
                voteQuestion.MinValue = item.MinValue;
                //voteQuestion.QuestionID  = item.QuestionID ;
                voteQuestion.QuestionTitle = item.QuestionTitle;
                voteQuestion.QuestionType = item.QuestionType;
                voteQuestion.VoteAnsers = new List<VoteAnser>();
                foreach (var anser in item.VoteAnserTemps)
                {
                    VoteAnser voteAnser = new VoteAnser()
                    {
                        AnserContent = anser.AnserContent,
                        AnserScore = anser.AnserScore,
                        MaxValue = anser.MaxValue,
                        MinValue = anser.MinValue,
                        SortOrder = anser.SortOrder
                    };
                    voteQuestion.VoteAnsers.Add(voteAnser);
                }

                voteTidQie.VoteQuestion = voteQuestion;

                voteApply.VoteThemeInfo.VoteTidQies.Add(voteTidQie);
            }
            //清空临时表
            if (Questions?.Count > 0)
                voteQuestionTempService.DeleteByList(Questions);
            #endregion
            if (applyViewModel.ApplyID > 0)
            {
                if (voteApplyService.Update(voteApply))
                {
                    result.IsSuccess = true;
                }
            }
            else if (voteApplyService.Insert(voteApply))
            {
                result.IsSuccess = true;
            }
            if (isAction)
            {
                result.IsSuccess = false;
                Dictionary<string, string> dictField = new Dictionary<string, string>();
                dictField.Add("F1", voteApply.ApplyID.ToString());
                var workFlowResult = WorkFlowHelper.ActionFlow(WorkContext.CurrentUser.UserName, voteApply.ApplyTitle, dictField, nextActivity, nextActors, FlowInstance.WorkflowType.VoteApply);
                if (workFlowResult.Success > 0)
                {
                    voteApply.WorkflowInstanceId = workFlowResult.WorkFlow.WorkflowInstanceId;
                    voteApply.ApplySn = workFlowResult.WorkFlow.SheetId;
                    if (voteApplyService.Update(voteApply))
                    {
                        result.IsSuccess = true;
                    }

                    //提交成功后抄送
                    if (!string.IsNullOrEmpty(nextCC))
                        CommonFunction.PendingData(workFlowResult.WorkFlow.WorkflowInstanceId, nextCC);//抄送
                }
                else
                    result.Message += workFlowResult?.Errmsg;
            }

            return Json(result);
        }

        public ActionResult SaveQuestion(QuestionInfoViewModel questionViewModel)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            if (questionViewModel.QuestionID > 0)
                result = UpdateQuestion(questionViewModel);
            else
                result = AddQuestion(questionViewModel);

            return Json(result);
        }
        public ActionResult DeleteQuestion(int id)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var model = voteTidQieService.FindByFeldName(f => f.QuestionID == id);
            if (model == null)
            {
                var modelT = voteQuestionTempService.FindByFeldName(f => f.QuestionID == id);
                if (modelT == null)
                {
                    result.Message = "该问题不存在！";
                    return Json(result);
                }
                if (voteQuestionTempService.Delete(modelT))
                {
                    result.IsSuccess = true;
                    return Json(result);
                }
            }
            if (voteTidQieService.Delete(model))
                result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult Delete(int[] ids)
        {
            var models = voteApplyService.List().Where(f => ids.Contains(f.ApplyID)).ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该问题不存在！";
                return Json(result);
            }
            if (voteApplyService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }

        #endregion
        #region 方法 
        bool CompareDate(DateTime? star, DateTime? end)
        {
            var now = DateTime.Now;
            if (now > star && now < end)
                return true;
            return false;
        }

        SystemResult AddQuestion(QuestionInfoViewModel questionViewModel)
        {
            SystemResult result = new SystemResult() { IsSuccess = true };
            #region 增加
            VoteQuestionTemp voteQuestionTemp = new VoteQuestionTemp();
            voteQuestionTemp.CreatorID = WorkContext.CurrentUser.UserId;
            voteQuestionTemp.Creator = WorkContext.CurrentUser.RealName;
            voteQuestionTemp.SortOrder = voteQuestionService.List().Where(a => a.CreatorID == voteQuestionTemp.CreatorID).Count() + 1;
            voteQuestionTemp.AnswerNum = questionViewModel.AnswerNum;
            voteQuestionTemp.CreateTime = DateTime.Now;
            voteQuestionTemp.QuestionTitle = questionViewModel.QuestionTitle;
            voteQuestionTemp.QuestionType = questionViewModel.QuestionType;
            //voteQuestionTemp.VoteAnserTemps = new List<VoteAnserTemp>();
            foreach (var item in questionViewModel.VoteAnsers)
            {
                voteQuestionTemp.VoteAnserTemps.Add(
                    new VoteAnserTemp()
                    {
                        AnserContent = item.AnserContent,
                        AnserScore = item.AnserScore,
                        MaxValue = item.MaxValue,
                        MinValue = item.MinValue,
                        SortOrder = item.SortOrder

                    }
                    );
            }
            if (voteQuestionTempService.Insert(voteQuestionTemp))
            {
                result.data = new { QuestionID = voteQuestionTemp.QuestionID, CreatorID = voteQuestionTemp.CreatorID, Creator = voteQuestionTemp.Creator };
            }
            #endregion
            return result;
        }
        SystemResult UpdateQuestion(QuestionInfoViewModel questionViewModel)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var voteQuestionTemp = voteQuestionTempService.FindByFeldName(q => q.QuestionID == questionViewModel.QuestionID && q.CreatorID == questionViewModel.CreatorID);

            #region 临时表的
            if (voteQuestionTemp != null)
            {

                //VoteQuestionTemp voteQuestionTemp = new VoteQuestionTemp();
                //voteQuestionTemp.CreatorID = WorkContext.CurrentUser.UserId;
                //voteQuestionTemp.Creator = WorkContext.CurrentUser.RealName;
                voteQuestionTemp.AnswerNum = questionViewModel.AnswerNum;
                //voteQuestionTemp.CreateTime = DateTime.Now;
                voteQuestionTemp.QuestionTitle = questionViewModel.QuestionTitle;
                voteQuestionTemp.QuestionType = questionViewModel.QuestionType;
                voteQuestionTemp.VoteAnserTemps.Clear();
                foreach (var item in questionViewModel.VoteAnsers)
                {
                    voteQuestionTemp.VoteAnserTemps.Add(
                        new VoteAnserTemp()
                        {
                            AnserContent = item.AnserContent,
                            AnserScore = item.AnserScore,
                            MaxValue = item.MaxValue,
                            MinValue = item.MinValue,
                            SortOrder = item.SortOrder

                        }
                        );
                }
                if (voteQuestionTempService.Update(voteQuestionTemp))
                {
                    result.IsSuccess = true;
                    result.data = new { QuestionID = voteQuestionTemp.QuestionID, CreatorID = voteQuestionTemp.CreatorID, Creator = voteQuestionTemp.Creator };
                }
            }
            #endregion
            #region 正式表的
            else
            {
                var voteQuestion = voteQuestionService.FindByFeldName(q => q.QuestionID == questionViewModel.QuestionID && q.CreatorID == questionViewModel.CreatorID);
                if (voteQuestion != null)
                {
                    //voteQuestion.CreatorID = WorkContext.CurrentUser.UserId;
                    //voteQuestion.Creator = WorkContext.CurrentUser.RealName;
                    voteQuestion.AnswerNum = questionViewModel.AnswerNum;
                    //voteQuestion.CreateTime = DateTime.Now;
                    voteQuestion.QuestionTitle = questionViewModel.QuestionTitle;
                    voteQuestion.QuestionType = questionViewModel.QuestionType;
                    voteQuestion.VoteAnsers.Clear();
                    foreach (var item in questionViewModel.VoteAnsers)
                    {
                        voteQuestion.VoteAnsers.Add(
                            new VoteAnser()
                            {
                                AnserContent = item.AnserContent,
                                AnserScore = item.AnserScore,
                                MaxValue = item.MaxValue,
                                MinValue = item.MinValue,
                                SortOrder = item.SortOrder

                            }
                            );
                    }
                    if (voteQuestionService.Update(voteQuestion))
                    {
                        result.IsSuccess = true;
                        result.data = new { QuestionID = voteQuestion.QuestionID, CreatorID = voteQuestion.CreatorID, Creator = voteQuestion.Creator };
                    }
                }
            }
            #endregion
            return result;
        }

        static string GetQuestionType(string type)
        {
            string result = "单选";
            switch (type)
            {
                case "S":
                    result = "单选";
                    break;
                case "M":
                    result = "多选";
                    break;
                case "Q":
                    result = "简答";
                    break;
                default:
                    break;
            }
            return result;
        }




        #region 泛型集合转DataTable
        /// <summary>
        /// 泛型集合转DataTable
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <param name="entityList">泛型集合</param>
        /// <returns>DataTable</returns>
        public static DataTable ListToDataTable(IList<VoteQuestion> entityList, int columnsNum)
        {
            if (entityList == null) return null;
            DataTable dt = CreateTable<VoteQuestion>(columnsNum);
            Type entityType = typeof(VoteQuestion);
            //PropertyInfo[] properties = entityType.GetProperties();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
            foreach (VoteQuestion item in entityList)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyDescriptor property in properties)
                {
                    if (property.Name == "VoteAnsers")//为子集时
                    {
                        var ansers = item.VoteAnsers.ToList();
                        for (int i = 0; i < ansers.Count; i++)
                        {

                            row["ColumnsC" + i] = ansers[i].AnserContent;
                            row["ColumnsS" + i] = ansers[i].AnserScore;
                        }
                    }
                    else if (property.Name == "QuestionType")
                    {
                        row[property.Name] = (object)GetQuestionType(property.GetValue(item).ToString());
                    }
                    else
                        row[property.Name] = property.GetValue(item);
                }
                dt.Rows.Add(row);
            }

            return dt;
        }

        #endregion

        #region 创建DataTable的结构
        private static DataTable CreateTable<T>(int Columns)
        {
            Type entityType = typeof(T);
            //PropertyInfo[] properties = entityType.GetProperties();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
            //生成DataTable的结构
            DataTable dt = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                dt.Columns.Add(prop.Name);
            }
            //子集的字段
            for (int i = 0; i < Columns; i++)
            {
                dt.Columns.Add("ColumnsC" + i);
                dt.Columns.Add("ColumnsS" + i);
            }
            return dt;
        }
        #endregion

        #endregion
    }
}