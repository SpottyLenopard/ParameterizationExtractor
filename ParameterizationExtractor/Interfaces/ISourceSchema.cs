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
    }
}
