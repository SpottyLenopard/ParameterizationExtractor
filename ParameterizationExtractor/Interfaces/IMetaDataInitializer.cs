using Quipu.ParameterizationExtractor.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Interfaces
{
    public interface IMetaDataInitializer
    {
        PFieldMetadata InitTableMetaData(DataRow metaData, IList<DataRow> indexesMetaData);
    }
}
