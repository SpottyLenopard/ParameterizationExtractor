using Quipu.ParameterizationExtractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Common
{
    public class ConsoleLogger : ILog
    {
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void DebugFormat(string message, params object[] arg)
        {
            Console.WriteLine(message, arg);
        }

        public void Error(Exception e)
        {
            var s = new StringBuilder();
            while (e != null)
            {
                Console.WriteLine("Err message {0} Err Stack: {1}", e.Message, e.StackTrace);
                e = e.InnerException;
            }
        }
    }
}
