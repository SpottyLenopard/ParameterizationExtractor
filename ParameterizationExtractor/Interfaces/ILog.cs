using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Interfaces
{
    public interface ILog
    {
        void Debug(string message);
        void DebugFormat(string message, params object[] arg);
        void Error(Exception e);
    }
}
