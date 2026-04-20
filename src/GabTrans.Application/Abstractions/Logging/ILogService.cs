using System;

namespace GabTrans.Application.Abstractions.Logging
{
    public interface ILogService
    {
        void LogInfo(string className, string methodName, string message);

        void LogError(string className, string methodName, Exception ex);
    }
}

