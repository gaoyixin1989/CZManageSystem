using System;
using System.Collections.Generic;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// Ȩ����Դת���ӿ�.
    /// </summary>
    public interface IResourceTranslator
    {
        /// <summary>
        /// ����Դ����ת��Ϊ��Դ����/id.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        string Alias2Name(string alias);

        /// <summary>
        /// ����Դ����/idת��Ϊ����.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string Name2Alias(string name);
    }
}
