using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Quipu.ParameterizationExtractor.Logic.Interfaces;
using Quipu.ParameterizationExtractor.Logic.Model;
using ParameterizationExtractor.Logic.Templates;

namespace Quipu.ParameterizationExtractor.Logic.MSSQL
{
    [Export(typeof(ISqlBuilder))]
    public class MSSqlBuilder : ISqlBuilder
    {
        public string Build(IEnumerable<PRecord> tables, ISourceSchema schema)
        {
            var template = new DefaultTemplate();
            template.Session = new Dictionary<string, object>();
            template.Session.Add("source", tables);
            template.Session.Add("schema", schema);

            return template.TransformText();
        }        
    }
}
