using Quipu.ParameterizationExtractor.Common;
using Quipu.ParameterizationExtractor.Interfaces;
using Quipu.ParameterizationExtractor.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Quipu.ParameterizationExtractor.Configs
{
    public class ConfigSerializer : SingletonBase<ConfigSerializer>, ICanSerializeConfigs
    {
        private const string _pathToGlobalConfig = @"ExtractConfig.xml";
        public IExtractConfiguration GetGlobalConfig()
        {
            var serializer = new XmlSerializer(typeof(GlobalExtractConfiguration));

            using (var reader = new StreamReader(_pathToGlobalConfig))
            {
                return (IExtractConfiguration)serializer.Deserialize(reader);
            }

        }

        public IPackage GetPackage(string path)
        {
            var serializer = new XmlSerializer(typeof(Package));

            using (var reader = new StreamReader(path))
            {
                return (IPackage)serializer.Deserialize(reader);
            }
        }

        public string SerializePackage(IPackage pkg)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Package));

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, pkg);
                return writer.ToString();
            }

        }
    }
}
