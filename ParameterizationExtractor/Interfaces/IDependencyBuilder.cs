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
        Task<IEnumerable<PRecord>> PrepareAsync();
    }

    public interface IPackageTemplate
    {
        IList<RecordsToExtract> RootRecords { get; }
        IList<string> Exceptions { get; }                
        IList<TableToExtract> TablesToProcess { get; }
        int Order { get; set; }
        string PackageName { get; set; }
        string Comments { get; set; }
    }

    public interface IExtractConfiguration
    {
        IList<string> FieldsToExclude { get; }
        IDictionary<string, UniqueColumnsCollection> UniqueColums { get; }
    }
}
