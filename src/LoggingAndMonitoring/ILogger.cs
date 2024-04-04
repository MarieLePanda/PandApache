using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pandapache.src.LoggingAndMonitoring
{
    public interface ILogger
    {
        void Initialize(string logDirectory, string logFileName, int maxLogFiles, int maxSizeFile);
        void LogDebug(string message);
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);

    }
}
