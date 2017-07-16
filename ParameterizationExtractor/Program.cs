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

namespace ParameterizationExtractor
{
    public class Program
    {
        private static ILog _log;        
        
        static void Main(string[] args)
        {
            _log = new ConsoleLogger();

            CancellationTokenSource cts = new CancellationTokenSource();

            System.Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                MainAsync(TestPackages.Get(), cts.Token)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
           
            Console.ReadKey();
        }

        
        static async Task MainAsync(IEnumerable<IPackageTemplate> pckgs, CancellationToken token)
        {
            Affirm.ArgumentNotNull(pckgs, "pckgs");

            var schema = new MSSQLSourceSchema(UnitOfWorkFactory.GetInstance(), GlobalExtractConfiguration.GetInstance());

            await schema.Init();

            foreach (var pckg in pckgs)
            {
                var builder = new DependencyBuilder(UnitOfWorkFactory.GetInstance(), schema, _log, pckg, GlobalExtractConfiguration.GetInstance());

                var pTables = await builder.PrepareAsync(token);
                var sqlBuilder = new MSSqlBuilder();

                if (!FileService.GetInstance().DirectoryExists(".\\Output"))
                    FileService.GetInstance().CreateDirectory(".\\Output");

                FileService.GetInstance().Save(sqlBuilder.Build(pTables,schema), string.Format(".\\Output\\{0}_p_{1}.sql", pckg.Order, pckg.PackageName));
            }
            
            _log.Debug(string.Empty);
        }
    
        
    }
}
