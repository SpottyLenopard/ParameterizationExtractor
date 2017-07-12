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
        private readonly IDictionary<string, UniqueColumnsCollection> _uniqueColums = new Dictionary<string, UniqueColumnsCollection>();
        private readonly IDictionary<string, string> _rootRecords = new Dictionary<string, string>();
        private readonly IList<string> _exceptions = new List<string>();

        public PackageTemplate()
        {

        }
        public IDictionary<string, UniqueColumnsCollection> UniqueColums
        {
            get
            {
                return _uniqueColums;
            }
        }

        public IList<string> Exceptions { get { return _exceptions; } }

        public IDictionary<string, string> RootRecords
        {
            get
            {
                return _rootRecords;
            }
        }
    }
}
