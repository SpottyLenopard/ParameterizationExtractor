using Quipu.ParameterizationExtractor.Logic.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Logic.Interfaces
{
    public interface IMetaDataInitializer
    {
        PFieldMetadata InitTableMetaData(DataRow metaData);
    }
}
