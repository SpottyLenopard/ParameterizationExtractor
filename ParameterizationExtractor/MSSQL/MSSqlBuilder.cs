using Quipu.ParameterizationExtractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quipu.ParameterizationExtractor.Model;
using Quipu.ParameterizationExtractor.Templates;

namespace Quipu.ParameterizationExtractor.MSSQL
{
    public class MSSqlBuilder : ISqlBuilder
    {
        public string Build(IEnumerable<PTable> tables)
        {
            var template = new DefaultTemplate();
            template.Session = new Dictionary<string, object>();
            template.Session.Add("source", tables);

            return template.TransformText();
        }        
    }
}
