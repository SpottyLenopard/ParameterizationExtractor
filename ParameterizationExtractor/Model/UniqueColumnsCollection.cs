using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Quipu.ParameterizationExtractor.Model
{
    public class UniqueColumnsCollection
    {
        public UniqueColumnsCollection(string tableName, List<string> uniqueColumns)
        {
            TableName = tableName;
            UniqueColumns = uniqueColumns;
        }
        public UniqueColumnsCollection()
        {
            UniqueColumns = new List<string>();
        }
        [XmlAttribute()]
        public string TableName { get; set; }
        [XmlAttribute()]
        public List<string> UniqueColumns { get; set; }
    }
}
