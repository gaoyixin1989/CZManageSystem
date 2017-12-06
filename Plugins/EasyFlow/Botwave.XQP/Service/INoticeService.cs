using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service
{
    /// <summary>
    /// 公告（通知）的数据服务接口.
    /// </summary>
    public interface INoticeService
    {
        /// <summary>
        /// 插入指定公告(通知)数据到数据库.
        /// 返回新增的公告(通知)编号.
        /// </summary>
        /// <param name="item">指定公告(通知)的数据.</param>
        /// <returns>返回新增的公告(通知)编号.</returns>
        int InsertNotice(Notice item);

        /// <summary>
        /// 更新指定公告(通知)数据.
        /// 返回更新所影响的数据行数.
        /// </summary>
        /// <param name="item">指定公告(通知)的数据.</param>
        /// <returns>返回更新所影响的数据行数.</returns>
        int UpdateNotice(Notice item);

        /// <summary>
        /// 更新指定公告(通知)的可用性.
        /// 返回更新所影响的数据行数.
        /// </summary>
        /// <param name="noticeId">公告(通知)编号.</param>
        /// <param name="enabled">可用性.</param>
        /// <returns>返回更新所影响的数据行数.</returns>
        int UpdateNoticeEnabled(int noticeId, bool enabled);

        /// <summary>
        /// 删除指定公告(通知)编号的数据.
        /// 返回删除所影响的数据行数.
        /// </summary>
        /// <param name="noticeId">指定公告(通知)编号的数据.</param>
        /// <returns>返回删除所影响的数据行数.</returns>
        int DeleteNotice(int noticeId);

        /// <summary>
        /// 获取指定编号的公告(通知)信息.
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        Notice GetNotice(int noticeId);

        /// <summary>
        /// 获取指定关联实体类型以及编号的可用的公告信息列表.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        IList<Notice> GetNotices(string entityType, string entityId);


        /// <summary>
        /// 获取最新的指定个数的公告.
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="enabled"></param>
        /// <param name="noticeNum">公告数目.</param>
        /// <returns></returns>
        List<Notice> GetNoticeList(string creator, bool? enabled, int noticeNum);

        /// <summary>
        /// 分页获取指定条件的公告信息数据.
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="enabled"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        List<Notice> GetNoticeList(string creator, bool? enabled, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// 分页获取指定条件的公告信息数据表.
        /// </summary>        
        /// <param name="creator">公告创建人.为空时表示不限制公告创建人.</param>
        /// <param name="enabled">是否可用. null 时表示包括可用与不可用公告.</param>
        /// <param name="pageIndex">页面索引.</param>
        /// <param name="pageSize">每页显示的数据记录数.</param>
        /// <param name="recordCount">数据总记录数.</param>
        /// <returns></returns>
        DataTable GetNotices(string creator,
            bool? enabled,
            int pageIndex,
            int pageSize,
            ref int recordCount);
        
        /// <summary>
        /// 分页获取指定条件的公告信息数据表.
        /// </summary>
        /// <param name="creator">公告创建人.为空时表示不限制公告创建人.</param>
        /// <param name="entityType">公告类型.为空时表示不限制公告类型.</param>
        /// <param name="enabled">是否可用. null 时表示包括可用与不可用公告.</param>
        /// <param name="pageIndex">页面索引.</param>
        /// <param name="pageSize">每页显示的数据记录数.</param>
        /// <param name="recordCount">数据总记录数.</param>
        /// <returns></returns>
        DataTable GetNotices(string creator,
            string entityType,
            bool? enabled,
            int pageIndex,
            int pageSize,
            ref int recordCount);

        /// <summary>
        /// 分页获取指定条件的公告信息数据表.
        /// </summary>
        /// <param name="creator">公告创建人.为空时表示不限制公告创建人.</param>
        /// <param name="entityType">公告类型.为空时表示不限制公告类型.</param>
        /// <param name="entityType">公告类型实体编号.</param>
        /// <param name="enabled">是否可用. null 时表示包括可用与不可用公告.</param>
        /// <param name="pageIndex">页面索引.</param>
        /// <param name="pageSize">每页显示的数据记录数.</param>
        /// <param name="recordCount">数据总记录数.</param>
        /// <returns></returns>
        DataTable GetNotices(string creator,
            string entityType,
            string entityId,
            bool? enabled,
            int pageIndex,
            int pageSize,
            ref int recordCount);
    }
}
