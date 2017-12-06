using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.DynamicForm.Extension.Domain;

namespace Botwave.DynamicForm.Extension.Implements
{
    public interface IJsLibraryService
    {
        /// <summary>
        /// 获取JS库集合
        /// </summary>
        /// <returns></returns>
        IList<JsLibrary> GetLibraryList();

        /// <summary>
        /// 根据类型获取JS库集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<JsLibrary> GetLibraryListByType(int type);

        /// <summary>
        /// 根据ID获取JS方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JsLibrary GetLibraryById(int id);

        /// <summary>
        /// 插入JS库
        /// </summary>
        /// <param name="item"></param>
        void InsetJSLibrary(JsLibrary item);

        /// <summary>
        /// 更新JS库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int UpdateJSLibraryById(int id);

        /// <summary>
        /// 删除JS库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteJSLibraryById(int id);
    }
}
