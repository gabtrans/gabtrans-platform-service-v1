using System;
using GabTrans.Application.Abstractions.Logging;
using Serilog;

namespace GabTrans.Infrastructure.Logging
{
    public class LogService : ILogService
    {
        public void LogInfo(string className, string methodName, string message)
        {
            Log.Information("\r\nClass Name: {0} \r\nMethod Name: {1} \r\nMessage: {2}  \r\n", className, methodName, message);
        }

        public void LogError(string className, string methodName, Exception ex)
        {
            Log.Error("\r\nClass Name: {0} \r\nMethod Name: {1} \r\nMessage: {2}  \r\n", className, methodName, ex);
        }
    }
}

