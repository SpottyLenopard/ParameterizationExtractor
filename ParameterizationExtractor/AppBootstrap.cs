using Fclp;
using Quipu.ParameterizationExtractor.Common;
using Quipu.ParameterizationExtractor.Configs;
using Quipu.ParameterizationExtractor.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor
{
    public class AppArgs : IAppArgs
    {
        public AppArgs()
        {
           
        }

        public string ConnectionString { get; set; }
        public string PathToPackage { get; set;  }
        public string ConnectionName { get; set; }
        public string OutputFolder { get; set; }

        public static IAppArgs GetAppArgs()
        {
            var p = new FluentCommandLineParser<AppArgs>();

            p.Setup<string>(_ => _.ConnectionName)
                .As('n', "connectionName")
                .SetDefault("SourceDB");

            p.Setup<string>(_ => _.PathToPackage)
                .As('p', "package")
                .WithDescription("Path extraction package")
                .Required();

            p.Setup<string>(_ => _.ConnectionString)
                .As('c', "connectionString");

            p.Setup<string>(_ => _.OutputFolder)
                .As('o', "outputFolder")
                .SetDefault("Output");

            p.Parse(Environment.GetCommandLineArgs());

            return p.Object;
        }
    }

    public static class DI
    {
        private readonly static Lazy<CompositionContainer> _container = new Lazy<CompositionContainer>(() => GetConfiguredContainer());
        private static CompositionContainer GetConfiguredContainer()
        {
            var catalog = new AggregateCatalog(new AssemblyCatalog(typeof(AppArgs).Assembly), new AssemblyCatalog(typeof(IUnitOfWork).Assembly));
            var container = new CompositionContainer(catalog, true); // thread safe ha!
            var batch = new CompositionBatch();
            var ser = new ConfigSerializer();

            batch.AddExportedValue<IAppArgs>(AppArgs.GetAppArgs());
            batch.AddExportedValue<ICanSerializeConfigs>(ser);
            batch.AddExportedValue<IExtractConfiguration>(ser.GetGlobalConfig());
            batch.AddExportedValue<ILog>(new ConsoleLogger());
            batch.AddExportedValue<IFileService>(new FileService());
            
            container.Compose(batch);

            return container;
        }
       
        private static CompositionContainer Container { get { return _container.Value; } }

        public static T GetInstance<T>()
        {
            return Container.GetExportedValue<T>();
        }

        public static T GetInstance<T>(string key)
        {
            return Container.GetExportedValue<T>(key);
        }
    }
   
}
