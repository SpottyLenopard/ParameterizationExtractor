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
        private readonly IList<RecordToExtract> _rootRecords = new List<RecordToExtract>();
        private readonly IList<string> _exceptions = new List<string>();
        private readonly IList<TableToExtract> _tablesToProcess = new List<TableToExtract>(); 

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

        public IList<RecordToExtract> RootRecords
        {
            get
            {
                return _rootRecords;
            }
        }

        public IList<TableToExtract> TablesToProcess
        {
            get
            {
                return _tablesToProcess;
            }
        }
    }
}
