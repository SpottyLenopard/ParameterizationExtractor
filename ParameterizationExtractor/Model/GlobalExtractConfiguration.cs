using Quipu.ParameterizationExtractor.Common;
using Quipu.ParameterizationExtractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Model
{
    public class GlobalExtractConfiguration : SingletonBase<GlobalExtractConfiguration>, IExtractConfiguration
    {
        private readonly IList<string> _fieldsToExclude;
        private readonly IDictionary<string, UniqueColumnsCollection> _uniqueColums;
        public GlobalExtractConfiguration()
        {
            _fieldsToExclude = new List<string>();
            _uniqueColums = new Dictionary<string, UniqueColumnsCollection>();
            DefaultExtractStrategy = new FKDependencyExtractStrategy();
        }
        public IList<string> FieldsToExclude
        {
            get
            {
                return _fieldsToExclude;
            }
        }

        public IDictionary<string, UniqueColumnsCollection> UniqueColums
        {
            get
            {
                return _uniqueColums;
            }
        }

        public ExtractStrategy DefaultExtractStrategy { get; set; }         
    }
}
