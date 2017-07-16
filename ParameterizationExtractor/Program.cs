using Quipu.ParameterizationExtractor.Interfaces;
using Quipu.ParameterizationExtractor.Model;
using Quipu.ParameterizationExtractor.MSSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quipu.ParameterizationExtractor.Templates;
using Quipu.ParameterizationExtractor.Common;
using System.Threading;
using Quipu.ParameterizationExtractor;
using Quipu.ParameterizationExtractor.Configs;

namespace ParameterizationExtractor
{
    public class Program
    {
        private static ILog _log;
        private static IExtractConfiguration _globalConfig;
        static void Main(string[] args)
        {
            _log = new ConsoleLogger();
            if (!args.Any())
            {
                _log.DebugFormat("Please specify path to package, usage ParameterizationExtractor.exe [path-to-package]");
                Console.ReadKey();
                return;
            }
            
            _globalConfig = ConfigSerializer.GetInstance().GetGlobalConfig();
            CancellationTokenSource cts = new CancellationTokenSource();

            System.Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                var pckg = ConfigSerializer.GetInstance().GetPackage(args[0]);

                MainAsync(pckg, cts.Token)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
           
            Console.ReadKey();
        }

        
        static async Task MainAsync(IPackage pckg, CancellationToken token)
        {
            Affirm.ArgumentNotNull(pckg, "pckgs");

            var schema = new MSSQLSourceSchema(UnitOfWorkFactory.GetInstance(), _globalConfig);

            await schema.Init(token);

            foreach (var scriptSource in pckg.Scripts)
            {
                var builder = new DependencyBuilder(UnitOfWorkFactory.GetInstance(), schema, _log, scriptSource, _globalConfig);

                var pTables = await builder.PrepareAsync(token);
                var sqlBuilder = new MSSqlBuilder();

                if (!FileService.GetInstance().DirectoryExists(".\\Output"))
                    FileService.GetInstance().CreateDirectory(".\\Output");

                FileService.GetInstance().Save(sqlBuilder.Build(pTables,schema), string.Format(".\\Output\\{0}_p_{1}.sql", scriptSource.Order, scriptSource.ScriptName));
            }
            
            _log.Debug(string.Empty);
        }
    
        
    }
}
