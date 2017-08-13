using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Quipu.ParameterizationExtractor.Common;
using Quipu.ParameterizationExtractor.Logic.Configs;
using Quipu.ParameterizationExtractor.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParameterizationExtractor.WebUI.Api
{
    public class PackageProcessorController : Controller
    {
        private readonly ISourceSchema _schema;
        private readonly ILog _log;
        private readonly ISqlBuilder _sqlBuilder;
        private readonly IDependencyBuilder _dependencyBuilder;
        public PackageProcessorController(ISourceSchema schema, ILog log, ISqlBuilder sqlBuilder, IDependencyBuilder dependencyBuilder)
        {
            Affirm.ArgumentNotNull(schema, "schema");
            Affirm.ArgumentNotNull(log, "log");
            Affirm.ArgumentNotNull(sqlBuilder, "sqlBuilder");
            Affirm.ArgumentNotNull(dependencyBuilder, "dependencyBuilder");

            _schema = schema;
            _sqlBuilder = sqlBuilder;
            _log = log;
            _dependencyBuilder = dependencyBuilder;
        }
        
        [HttpPost]
        [Authorize]
        [Route("[Controller]/Execute")]
        public async Task<IActionResult> ExecuteAsync(CancellationToken token, [FromBody]Package pckg)
        {
            try
            {
                await _schema.Init(token);

                _log.Debug("Starting processing of package...");
                var tasks = new List<Task<Tuple<string, string>>>();

                foreach (var scriptSource in pckg.Scripts)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        var pTables = await _dependencyBuilder.PrepareAsync(token, scriptSource);

                        return new Tuple<string, string>(_sqlBuilder.Build(pTables, _schema), "{0}_p_{1}.sql".FormIt(scriptSource.Order, scriptSource.ScriptName));

                    }));

                }

                await Task.WhenAll(tasks);

                _log.Debug("Finished processing of package.");

                return new FileContentResult(await PrepareStream(tasks.Select(_ => _.Result)), new MediaTypeHeaderValue("application/zip"))
                {
                    FileDownloadName = "scripts.zip"
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<byte[]> PrepareStream(IEnumerable<Tuple<string, string>> content)
        {
            var zipStream = new MemoryStream();

            using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                foreach (var t in content)
                {
                    var zipEntry = zip.CreateEntry(t.Item2);
                    using (var writer = new StreamWriter(zipEntry.Open()))
                    {
                        await writer.WriteAsync(t.Item1);
                    }
                }              
            }

            var b = zipStream.ToArray();
            zipStream.Dispose();

            return b;
        }
    }
}
