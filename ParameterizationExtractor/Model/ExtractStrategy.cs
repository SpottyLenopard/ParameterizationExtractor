using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Model
{
    public abstract class ExtractStrategy
    {
        protected ExtractStrategy(bool processChildren, bool processParents)
        {
            ProcessChildren = processChildren;
            ProcessParents = processParents;
        }
        public bool ProcessChildren { get; protected set; }
        public bool ProcessParents { get; protected set; }

    }

    public class FKDependencyExtractStrategy: ExtractStrategy
    {
        public FKDependencyExtractStrategy() : base(true, true) { }
    }
    public class OnlyParentExtractStrategy : ExtractStrategy
    {
        public OnlyParentExtractStrategy() : base(false, true) { }
    }
    public class OnlyChildrenExtractStrategy : ExtractStrategy
    {
        public OnlyChildrenExtractStrategy() : base(true, false) { }
    }

    public class OnlyOneTableEtraction : ExtractStrategy
    {
        public OnlyOneTableEtraction() : base(false, false) { }
    }
}
