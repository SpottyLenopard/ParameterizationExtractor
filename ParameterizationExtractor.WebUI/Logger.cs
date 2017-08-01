using Microsoft.Extensions.Logging;
using Quipu.ParameterizationExtractor.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterizationExtractor.WebUI
{
    public class Logger : ILog
    {
        private readonly ILogger _logger;
        public Logger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("General");
        }
        public void Debug(string message)
        {
            _logger.LogDebug(message);
        }

        public void DebugFormat(string message, params object[] arg)
        {
            _logger.LogDebug(message,arg);
        }

        public void Error(Exception e)
        {
            var s = new StringBuilder();
            while (e != null)
            {
                DebugFormat("Err message {0} Err Stack: {1}", e.Message, e.StackTrace);
                e = e.InnerException;
            }

        }
    }
}
