namespace Botwave.Easyflow.API
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class APIResult
    {
        public APIResult()
        {
        }

        public APIResult(string appAuth, string message)
        {
            this.AppAuth = appAuth;
            this.Message = message;
        }

        public string AppAuth { get; set; }

        public string Message { get; set; }
    }
}

