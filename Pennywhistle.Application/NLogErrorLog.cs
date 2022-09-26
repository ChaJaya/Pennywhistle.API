using NLog;

namespace Pennywhistle.Application
{
    /// <summary>
    /// Implements Nlog functions
    /// </summary>
    public static class NLogErrorLog
    {
        public static string CommonErrorMessage = "Issue with request, plese contact your admin";

        private static Logger error = LogManager.GetLogger("ErrorRule");
        private static Logger info = LogManager.GetLogger("InfoRule");


        public static void LogErrorMessages(string message)
        {
            error.Error(message);
        }

        public static void LogInfoMessage(string message)
        {
            info.Info(message);
        }

    }
}
