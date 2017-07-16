using Quipu.ParameterizationExtractor.Interfaces;
using Quipu.ParameterizationExtractor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Quipu.ParameterizationExtractor.Configs
{
    public class SourceForScript : ISourceForScript
    {                    
        public SourceForScript()
        {
            RootRecords = new List<RecordsToExtract>();
            TablesToProcess = new List<TableToExtract>();
        }

        [XmlAttribute()]
        public int Order { get; set; }

        [XmlAttribute()]
        public string ScriptName { get; set; }

        public List<RecordsToExtract> RootRecords { get; set; }
        [XmlIgnore]
        IList<RecordsToExtract> ISourceForScript.RootRecords
        {
            get
            {
                return RootRecords;
            }
        }

        public List<TableToExtract> TablesToProcess { get; set; }
        [XmlIgnore]
        IList<TableToExtract> ISourceForScript.TablesToProcess
        {
            get
            {
                return TablesToProcess;
            }
        }

        [XmlAttribute()]
        public string Comments
        {
            get; set;
        }
    }
}
