using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Model
{
    public class SqlBuildStrategy
    {
        public SqlBuildStrategy()
        {
            ThrowExecptionIfNotExists = false;
            NoInserts = false;
            AsIsInserts = false;
        }
        public SqlBuildStrategy(bool throwExecptionIfNotExists, bool noInserts, bool asIsInserts)
        {
            ThrowExecptionIfNotExists = throwExecptionIfNotExists;
            NoInserts = noInserts;
            AsIsInserts = asIsInserts;
        }

        public bool ThrowExecptionIfNotExists { get; private set; }
        public bool NoInserts { get; private set; }
        public bool AsIsInserts { get; private set; }
    }
}
