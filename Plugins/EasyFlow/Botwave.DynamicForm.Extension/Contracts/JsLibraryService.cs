using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.DynamicForm.Extension.Domain;
using System.Collections;
using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm.Extension.Implements;

namespace Botwave.DynamicForm.Extension.Contracts
{
    public class JsLibraryService : IJsLibraryService
    {
        public IList<JsLibrary> GetLibraryList()
        {
            return IBatisMapper.Select<JsLibrary>("bwdf_JsLibrary_Select");
        }

        public IList<JsLibrary> GetLibraryListByType(int type)
        {
            return IBatisMapper.Select<JsLibrary>("bwdf_JsLibrary_Select_By_Type", type);
        }

        public JsLibrary GetLibraryById(int id)
        {
            return IBatisMapper.Load<JsLibrary>("bwdf_JsLibrary_Select_By_Id", id);
        }

        public void InsetJSLibrary(JsLibrary item)
        {
            IBatisMapper.Insert("bwdf_JsLibrary_Insert", item);
        }

        public int UpdateJSLibraryById(int id)
        {
            return IBatisMapper.Update("bwdf_JsLibrary_Update_By_Id", id);
        }

        public int  DeleteJSLibraryById(int id)
        {
            return IBatisMapper.Delete("bwdf_JsLibrary_Delete_By_Id", id);
        }
    }
}
