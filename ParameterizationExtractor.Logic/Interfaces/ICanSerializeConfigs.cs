using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Logic.Interfaces
{
    public interface ICanSerializeConfigs
    {
        IPackage GetPackage(string path);
        IExtractConfiguration GetGlobalConfig();
        string SerializePackage(IPackage pkg);
    }
}
