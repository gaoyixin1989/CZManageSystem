using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.DynamicForm.Extension.Implements;
using System.Data.SqlClient;
using System.Data;
using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.Extension.Contracts
{
    public class DataFormLibraryService : IDataFormLibraryService
    {
        public string IsFormLibrary()
        {
            SqlParameter[] pa = new SqlParameter[1];
            //pa[0] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            //pa[0].Value = Id;
            string result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "select Name from bwdf_FormDefinitions", pa).ToString();
            return result;
            
        }

        public Guid InsertFormDefinition(FormDefinition definition)
        {
            definition.Id = Guid.NewGuid();

            IBatisMapper.Insert("bwdf_Definitions_Insert", definition);

            return definition.Id;
        }

        public int UpdateFormDefinition(FormDefinition definition)
        {
            return IBatisMapper.Update("bwdf_Definitions_Update",definition);
        }

        public int DeleteFormDefinition(Guid id)
        {
            return IBatisMapper.Update("bwdf_Definitions_Delete", id);
        }
    }
}
