using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Common
{
    public class SingletonBase<T>
        where T:class,new()
    {
        private static readonly Lazy<T> lazy = new Lazy<T>(() => new T());
        public static T GetInstance()
        {
            return lazy.Value;
        }
    }
}
