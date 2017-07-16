using Quipu.ParameterizationExtractor.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PackageProcessor
    {
        private readonly ISourceSchema _schema;
        private readonly ILog _log;
        private readonly IDependencyBuilder _builder;
        private readonly ISqlBuilder _sqlBuilder;
        private readonly IFileService _fileService;

        [ImportingConstructor]
        public PackageProcessor(ISourceSchema schema, ILog log, IDependencyBuilder builder, ISqlBuilder sqlBuilder, IFileService fileService)
        {
            Affirm.ArgumentNotNull(schema, "schema");
            Affirm.ArgumentNotNull(log, "log");
            Affirm.ArgumentNotNull(builder, "builder");
            Affirm.ArgumentNotNull(sqlBuilder, "sqlBuilder");
            Affirm.ArgumentNotNull(_fileService, "fileService");

            _schema = schema;
            _builder = builder;
            _sqlBuilder = sqlBuilder;
            _fileService = fileService;
            _log = log;
        }

        public async Task ExecuteAsync(CancellationToken token, IPackage pckg)
        {
            foreach (var scriptSource in pckg.Scripts)
            {
                var pTables = await _builder.PrepareAsync(token, scriptSource);
              
                if (!_fileService.DirectoryExists(".\\Output"))
                    _fileService.CreateDirectory(".\\Output");

                _fileService.Save(_sqlBuilder.Build(pTables, _schema), string.Format(".\\Output\\{0}_p_{1}.sql", scriptSource.Order, scriptSource.ScriptName));
            }
        }
    }
}
