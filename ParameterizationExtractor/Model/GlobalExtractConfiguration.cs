using Quipu.ParameterizationExtractor.Common;
using Quipu.ParameterizationExtractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Model
{
    public class GlobalExtractConfiguration : SingletonBase<GlobalExtractConfiguration>, IExtractConfiguration
    {

        private readonly IList<string> _fieldsToExclude;
        private readonly IDictionary<string, UniqueColumnsCollection> _uniqueColums;
        public GlobalExtractConfiguration()
        {
            _fieldsToExclude = new List<string>();
            _uniqueColums = new Dictionary<string, UniqueColumnsCollection>();
            DefaultExtractStrategy = new FKDependencyExtractStrategy();

            FillConfigForDebug();
        }

        private void FillConfigForDebug()
        {
            
            DefaultExtractStrategy = new OnlyChildrenExtractStrategy();
            DefaultSqlBuildStrategy = new SqlBuildStrategy();
            FieldsToExclude.Add("CreatorId");
            FieldsToExclude.Add("Created");

            UniqueColums.Add("BusinessProcessSteps", new UniqueColumnsCollection() { "BPTypeCode", "StepStartStatusId", "StepEndStatusId" });
            UniqueColums.Add("bpProcessingConditions", new UniqueColumnsCollection() { "ConditionText" });
            UniqueColums.Add("Presentations", new UniqueColumnsCollection() { "Name", "Code" });
            UniqueColums.Add("FlowDefinitions", new UniqueColumnsCollection() { "Code" });
            UniqueColums.Add("Scripts", new UniqueColumnsCollection() { "Code" });
            UniqueColums.Add("PaymentMessageContent", new UniqueColumnsCollection() { "Code" });
            UniqueColums.Add("BpTypeDefaultExecutionParameters", new UniqueColumnsCollection() { "ParameterName", "BPTypeCode" });
            UniqueColums.Add("BPAttrTypes", new UniqueColumnsCollection() { "Code" });
            UniqueColums.Add("sys_Parameters", new UniqueColumnsCollection() { "ParameterName" });
            UniqueColums.Add("Cls_PaymentMessageTypes", new UniqueColumnsCollection() { "Code" });
            UniqueColums.Add("PaymentMessageTypeFields", new UniqueColumnsCollection() { "Code" });
            UniqueColums.Add("ServiceJobs", new UniqueColumnsCollection() { "Title", "Description" });
            UniqueColums.Add("ServiceJobItems", new UniqueColumnsCollection() { "MethodName", "ObjectAssemblyName", "ObjectTypeName" });
        }

        public IList<string> FieldsToExclude
        {
            get
            {
                return _fieldsToExclude;
            }
        }

        public IDictionary<string, UniqueColumnsCollection> UniqueColums
        {
            get
            {
                return _uniqueColums;
            }
        }

        public ExtractStrategy DefaultExtractStrategy { get; set; }
        public SqlBuildStrategy DefaultSqlBuildStrategy { get; set; }
    }
}
