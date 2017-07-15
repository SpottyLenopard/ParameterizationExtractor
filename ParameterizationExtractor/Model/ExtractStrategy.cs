using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Model
{
    public abstract class ExtractStrategy
    {
        protected ExtractStrategy(bool processChildren, bool processParents) : this(processChildren, processParents, new List<string>())
        {
        }


        protected ExtractStrategy(bool processChildren, bool processParents, IList<string> dependencyToExclue)
        {
            ProcessChildren = processChildren;
            ProcessParents = processParents;
            DependencyToExclude = dependencyToExclue;
        }
        public bool ProcessChildren { get; private set; }
        public bool ProcessParents { get; private set; }       
        public IList<string> DependencyToExclude { get; private set; }

    }

    public class FKDependencyExtractStrategy: ExtractStrategy
    {
        public FKDependencyExtractStrategy() : base(true, true) { }
        public FKDependencyExtractStrategy(IList<string> dependencyToExclue) : base(true, true, dependencyToExclue) { }
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
