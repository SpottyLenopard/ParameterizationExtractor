using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Common
{
    public static class StringExtensions 
    {
        public static bool IsEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
    }
}
