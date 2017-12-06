using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Botwave.Workflow
{
    /// <summary>
    /// �����쳣��.
    /// </summary>
    [Serializable]
    public class WorkflowException : ApplicationException
    {
        /// <summary>
        /// ���췽��.
        /// </summary>
        public WorkflowException() : base() { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="message"></param>
        public WorkflowException(string message) : base(message) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="streamingContext"></param>
        protected WorkflowException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public WorkflowException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// δ�ҵ����̵��쳣��.
    /// </summary>
    [Serializable]
    public class WorkflowNotFoundException : WorkflowException
    {
        /// <summary>
        /// ���췽��.
        /// </summary>
        public WorkflowNotFoundException() : base() { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="message"></param>
        public WorkflowNotFoundException(string message) : base(message) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="streamingContext"></param>
        protected WorkflowNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public WorkflowNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// δ�ҵ����̻(����)���쳣��.
    /// </summary>
    [Serializable]
    public class ActivityNotFoundException : WorkflowException
    {
        /// <summary>
        /// ���췽��.
        /// </summary>
        public ActivityNotFoundException() : base() { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="message"></param>
        public ActivityNotFoundException(string message) : base(message) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="streamingContext"></param>
        protected ActivityNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ActivityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// ���̷����쳣.
    /// </summary>
    [Serializable]
    public class WorkflowAllocateException : WorkflowException
    {
        /// <summary>
        /// ���췽��.
        /// </summary>
        public WorkflowAllocateException() : base() { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="message"></param>
        public WorkflowAllocateException(string message) : base(message) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="streamingContext"></param>
        protected WorkflowAllocateException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public WorkflowAllocateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
