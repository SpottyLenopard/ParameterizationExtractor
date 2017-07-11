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
        Task<IEnumerable<PTable>> PrepareAsync(IPackageTemplate template);
    }

    public interface IPackageTemplate
    {
        IDictionary<string,string> RootRecords { get; }
        IDictionary<string, UniqueColumnsCollection> Exceptions { get; }
    }

}
