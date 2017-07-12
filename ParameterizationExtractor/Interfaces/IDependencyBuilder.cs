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
        IDictionary<string,string> RootRecords { get; }
        IList<string> Exceptions { get; }
        IDictionary<string, UniqueColumnsCollection> UniqueColums { get; }
    }

    public interface IExtractConfiguration
    {
        
    }
}
