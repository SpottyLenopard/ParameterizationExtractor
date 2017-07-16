using Quipu.ParameterizationExtractor.Common;
using Quipu.ParameterizationExtractor.Configs;
using Quipu.ParameterizationExtractor.Interfaces;
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
    public class AppBootstrap
    {
        private readonly static Lazy<CompositionContainer> _container = new Lazy<CompositionContainer>(() => GetConfiguredContainer());

        public AppBootstrap()
        {
            
        }

        public async Task Init(CancellationToken token)
        {
            var schema = Container.GetExportedValue<ISourceSchema>();
            await schema.Init(token);
        } 

        public static CompositionContainer Container { get { return _container.Value; } }

        private static CompositionContainer GetConfiguredContainer()
        {
            var asm = Assembly.GetAssembly(typeof(AppBootstrap));
            var catalog = new AssemblyCatalog(asm);
            var container = new CompositionContainer(catalog);
            var batch = new CompositionBatch();
            var ser = new ConfigSerializer();

            batch.AddExportedValue<ICanSerializeConfigs>(ser);
            batch.AddExportedValue<IExtractConfiguration>(ser.GetGlobalConfig());
            batch.AddExportedValue<ILog>(new ConsoleLogger());
            batch.AddExportedValue<IFileService>(new FileService());

            container.Compose(batch);

            return container;
        }
    }
}
