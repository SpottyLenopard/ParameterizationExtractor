using Quipu.ParameterizationExtractor.Common;
using Quipu.ParameterizationExtractor.Logic.Interfaces;
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
    public class PackageProcessor
    {
        private readonly ISourceSchema _schema;
        private readonly ILog _log;
        private readonly ISqlBuilder _sqlBuilder;
        private readonly IFileService _fileService;
        private readonly IAppArgs _args;

        [ImportingConstructor]
        public PackageProcessor(ISourceSchema schema, ILog log, ISqlBuilder sqlBuilder, IFileService fileService, IAppArgs args)
        {
            Affirm.ArgumentNotNull(schema, "schema");
            Affirm.ArgumentNotNull(log, "log");
            Affirm.ArgumentNotNull(sqlBuilder, "sqlBuilder");
            Affirm.ArgumentNotNull(fileService, "fileService");
            Affirm.ArgumentNotNull(args, "args");

            _schema = schema;
            _sqlBuilder = sqlBuilder;
            _fileService = fileService;
            _log = log;
            _args = args;
        }

        public async Task ExecuteAsync(CancellationToken token, IPackage pckg)
        {
            _log.Debug("Starting processing of package...");
            if (!_fileService.DirectoryExists(_args.OutputFolder))
                _fileService.CreateDirectory(_args.OutputFolder);

            var tasks = new List<Task>();

            foreach (var scriptSource in pckg.Scripts)
            {
                tasks.Add(Task.Run(async () =>
                {                    
                    var pTables = await DI.GetInstance<IDependencyBuilder>()
                                            .PrepareAsync(token, scriptSource);

                    _fileService.Save(_sqlBuilder.Build(pTables, _schema), string.Format(".\\{0}\\{1}_p_{2}.sql", _args.OutputFolder, scriptSource.Order, scriptSource.ScriptName));
                }));
                              
            }            

            await Task.WhenAll(tasks);

            _log.Debug("Finished processing of package.");
        }
    }
}
