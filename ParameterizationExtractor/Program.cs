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
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.Write("Please specify path to package, usage ParameterizationExtractor.exe [path-to-package]");
                Console.ReadKey();
                return;
            }
            
            CancellationTokenSource cts = new CancellationTokenSource();

            System.Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };
            var _log = AppBootstrap.Container.GetExportedValue<ILog>();
            try
            {
                var bootstraper = new AppBootstrap();
                bootstraper.Init(cts.Token).GetAwaiter().GetResult();

                var pckg = AppBootstrap.Container.GetExportedValue<ICanSerializeConfigs>().GetPackage(args[0]);
                var pckgProcessor = AppBootstrap.Container.GetExportedValue<PackageProcessor>();

                pckgProcessor.ExecuteAsync(cts.Token, pckg)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
           
            Console.ReadKey();
        }

        
    
        
    }
}
