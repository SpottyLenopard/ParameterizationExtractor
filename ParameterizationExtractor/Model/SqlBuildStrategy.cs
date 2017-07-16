using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Quipu.ParameterizationExtractor.Model
{
    [Serializable]
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

        [XmlAttribute()]
        public bool ThrowExecptionIfNotExists { get; set; }
        [XmlAttribute()]
        public bool NoInserts { get; set; }
        [XmlAttribute()]
        public bool AsIsInserts { get; set; }
    }
}
