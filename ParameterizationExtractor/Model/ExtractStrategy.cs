using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Quipu.ParameterizationExtractor.Model
{
    [Serializable]
    [XmlInclude(typeof(FKDependencyExtractStrategy))]
    [XmlInclude(typeof(OnlyParentExtractStrategy))]
    [XmlInclude(typeof(OnlyChildrenExtractStrategy))]
    [XmlInclude(typeof(OnlyOneTableExtractStrategy))]
    public abstract class ExtractStrategy
    {
        protected ExtractStrategy(bool processChildren, bool processParents) : this(processChildren, processParents, new List<string>())
        {
        }


        protected ExtractStrategy(bool processChildren, bool processParents, List<string> dependencyToExclue)
        {
            ProcessChildren = processChildren;
            ProcessParents = processParents;
            DependencyToExclude = dependencyToExclue;
        }
        [XmlAttribute()]
        public bool ProcessChildren { get; set; }
        [XmlAttribute()]
        public bool ProcessParents { get; set; }       
        public List<string> DependencyToExclude { get; set; }

    }

    public class FKDependencyExtractStrategy: ExtractStrategy
    {
        public FKDependencyExtractStrategy() : base(true, true) { }
        public FKDependencyExtractStrategy(List<string> dependencyToExclue) : base(true, true, dependencyToExclue) { }
    }

    public class OnlyParentExtractStrategy : ExtractStrategy
    {
        public OnlyParentExtractStrategy() : base(false, true) { }
    }

    public class OnlyChildrenExtractStrategy : ExtractStrategy
    {
        public OnlyChildrenExtractStrategy() : base(true, false) { }
    }

    public class OnlyOneTableExtractStrategy : ExtractStrategy
    {
        public OnlyOneTableExtractStrategy() : base(false, false) { }
    }
}
