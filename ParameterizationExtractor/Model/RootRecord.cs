using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Model
{
    public class TableToExtract
    {
        public TableToExtract(string tableName) : this(tableName,new FKDependencyExtractStrategy())
        {

        }

        public TableToExtract(string tableName,  ExtractStrategy extractStrategy)
        {
            Affirm.NotNullOrEmpty(tableName, "tableName");
            Affirm.ArgumentNotNull(extractStrategy, "extractStrategy");

            TableName = tableName;
            ExtractStrategy = extractStrategy;
        }
        public string TableName { get; set; }

        public ExtractStrategy ExtractStrategy { get; private set; }
    }

    public class RecordToExtract
    {
        public RecordToExtract(string tableName, string pkValue)
        {
            Affirm.NotNullOrEmpty(tableName, "tableName");
            Affirm.NotNullOrEmpty(pkValue, "pkValue");

            TableName = tableName;
            PkValue = pkValue;
        }
        public string TableName { get; set; }
        public string PkValue { get; set; }
    }
}
