using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quipu.ParameterizationExtractor.Common;
using Quipu.ParameterizationExtractor.Configs;
using System.Xml.Serialization;
using Quipu.ParameterizationExtractor.Model;
using System.IO;

namespace Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void SerializationTest()
        {
            var pkg = TestPackages.Get();

            var s = ConfigSerializer.GetInstance().SerializePackage(pkg);

            XmlSerializer serializer = new XmlSerializer(typeof(GlobalExtractConfiguration));

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, ConfigSerializer.GetInstance().GetGlobalConfig());
                var g = writer.ToString();
            }
        }
    }
}
