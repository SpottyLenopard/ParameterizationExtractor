using Quipu.ParameterizationExtractor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Interfaces
{
    public interface IDependencyBuilder
    {
        Task<IEnumerable<PRecord>> PrepareAsync(IPackageTemplate template);
    }

    public interface IPackageTemplate
    {
        IList<RecordToExtract> RootRecords { get; }
        IList<string> Exceptions { get; }                
        IList<TableToExtract> TablesToProcess { get; }
    }

    public interface IExtractConfiguration
    {
        IList<string> FieldsToExclude { get; }
        IDictionary<string, UniqueColumnsCollection> UniqueColums { get; }
    }
}
