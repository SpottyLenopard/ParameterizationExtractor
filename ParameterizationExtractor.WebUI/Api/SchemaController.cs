using Microsoft.AspNetCore.Mvc;
using Quipu.ParameterizationExtractor.Logic.Interfaces;
using Quipu.ParameterizationExtractor.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParameterizationExtractor.WebUI.Api
{
    public class SchemaController : Controller
    {
        private readonly IObjectMetaDataProvider _objectMetaProvider;
        public SchemaController(IObjectMetaDataProvider objectMetaProvider)
        {
            _objectMetaProvider = objectMetaProvider;
        }

        [Route("[Controller]/DependentTables/{tablename}")]
        public Task<IEnumerable<PDependentTable>> GetDependentTables(string tableName, CancellationToken cancellationToken)
        {
            return _objectMetaProvider.GetDependentTables(tableName, cancellationToken);
        }
       
        [Route("[Controller]/TableMetaData/{tablename}")]
        public Task<PTableMetadata> GetTableMetaData(string tableName, CancellationToken cancellationToken)
        {
            return _objectMetaProvider.GetTableMetaData(tableName, cancellationToken);
        }
    }
}
