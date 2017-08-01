using Quipu.ParameterizationExtractor.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Logic.Interfaces
{
    public interface IDependencyBuilder
    {
        Task<IEnumerable<PRecord>> PrepareAsync(CancellationToken cancellationToken, ISourceForScript template);
    }
     
}
