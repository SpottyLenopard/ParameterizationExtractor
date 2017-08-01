using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quipu.ParameterizationExtractor.Common;
using System.Threading;
using Quipu.ParameterizationExtractor;
using Quipu.ParameterizationExtractor.Configs;
using Quipu.ParameterizationExtractor.Logic.Interfaces;

namespace ParameterizationExtractor
{
    class Program
    {
        //todo add Raiserror clause "row inserted"/"row updated"
        //todo ass optional parameter "TargetDatabase" and add "use TargetDatabase" in each script
        //todo add indent as nest_level*tab to generate scripts
        //todo remove unused char(13)

        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.Write("Please specify path to package, usage ParameterizationExtractor.exe -p [path-to-package]");
                Console.ReadKey();
                return;
            }
            
            CancellationTokenSource cts = new CancellationTokenSource();

            System.Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            var _log = DI.GetInstance<ILog>();
            try
            {
                _log.Debug("Starting...");
                var bootstraper = DI.GetInstance<ISourceSchema>();

                bootstraper.Init(cts.Token)
                           .GetAwaiter()
                           .GetResult();

                var appArgs = DI.GetInstance<IAppArgs>();
                var pckg = DI.GetInstance<ICanSerializeConfigs>().GetPackage(appArgs.PathToPackage);
                var pckgProcessor = DI.GetInstance<PackageProcessor>();

                pckgProcessor.ExecuteAsync(cts.Token, pckg)
                             .GetAwaiter()
                             .GetResult();

                _log.Debug("Finished.");
            }
            catch (Exception e)
            {
                _log.Error(e);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        
    
        
    }
}
