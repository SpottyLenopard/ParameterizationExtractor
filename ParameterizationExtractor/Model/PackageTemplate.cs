using Quipu.ParameterizationExtractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Model
{
    public class PackageTemplate : IPackageTemplate
    {
        private readonly IDictionary<string, UniqueColumnsCollection> _exceptions = new Dictionary<string, UniqueColumnsCollection>();
        private readonly IDictionary<string, string> _rootRecords = new Dictionary<string, string>();
        public PackageTemplate()
        {

        }
        public IDictionary<string, UniqueColumnsCollection> Exceptions
        {
            get
            {
                return _exceptions;
            }
        }

        public IDictionary<string, string> RootRecords
        {
            get
            {
                return _rootRecords;
            }
        }
    }
}
