using Quipu.ParameterizationExtractor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Interfaces
{
    public interface ISourceSchema
    {
        IEnumerable<PTableMetadata> Tables { get; }
        IEnumerable<PDependentTable> DependentTables { get; }
        PTableMetadata GetTableMetaData(string tableName);
        bool WasInit { get; }
        Task Init();
        string Database { get; }
        string DataSource { get; }
    }
}
