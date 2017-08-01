using Quipu.ParameterizationExtractor.Logic.Configs;
using Quipu.ParameterizationExtractor.Logic.Interfaces;
using Quipu.ParameterizationExtractor.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Common
{
    public static class TestPackages
    {
        static SourceForScript CreateDebugPackageBP()
        {
            var pckg = new SourceForScript() { Order = 10, ScriptName = "_IMT_SWIFT_103_INC2" };


            pckg.RootRecords.Add(new RecordsToExtract("BusinessProcessesTypes", "bptypecode = '_IMT_SWIFT_103_INC2'"));
            pckg.TablesToProcess.Add(new TableToExtract("BusinessProcessSteps", new FKDependencyExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("BpTypeStepPresentations", new FKDependencyExtractStrategy(new List<string>() { "BusinessProcessSteps" })));
            pckg.TablesToProcess.Add(new TableToExtract("Presentations", new OnlyOneTableExtractStrategy(), new SqlBuildStrategy(true, true, false)));
            pckg.TablesToProcess.Add(new TableToExtract("Scripts", new OnlyOneTableExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("BPTransactionTemplates", new FKDependencyExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("bpProcessingConditions", new OnlyOneTableExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("BpTypes4PaymentSystems", new FKDependencyExtractStrategy(new List<string>() { "BusinessProcessesTypes" })));

            pckg.TablesToProcess.Add(new TableToExtract("BpTypeDefaultExecutionParameters", new FKDependencyExtractStrategy(new List<string>() { "BusinessProcessesTypes" })));
            pckg.TablesToProcess.Add(new TableToExtract("BPAttrTypes", new FKDependencyExtractStrategy(new List<string>() { "BusinessProcessesTypes" })));
            pckg.TablesToProcess.Add(new TableToExtract("GlobalParameters", new OnlyOneTableExtractStrategy(), new SqlBuildStrategy(true, true, false)));
            pckg.TablesToProcess.Add(new TableToExtract("sys_Parameters", new OnlyOneTableExtractStrategy(), new SqlBuildStrategy(true, true, false)));
            return pckg;
        }

        static SourceForScript CreateDebugPackageGlobalParameters()
        {
            var pckg = new SourceForScript() { Order = 9, ScriptName = "GlobalParameters" };
            pckg.RootRecords.Add(new RecordsToExtract("GlobalParameters", "Code in ('SSERootDir')"));
            pckg.RootRecords.Add(new RecordsToExtract("sys_Parameters", "ParameterName in ('StateMachine','SwiftReason')"));

            return pckg;
        }

        static SourceForScript CreateDebugPackagePresentations()
        {
            var pckg = new SourceForScript() { Order = 0, ScriptName = "Presentations" };
            pckg.RootRecords.Add(new RecordsToExtract("Presentations", ""));
            pckg.TablesToProcess.Add(new TableToExtract("PresentationValidationRules", new OnlyOneTableExtractStrategy()));
            return pckg;
        }
        static SourceForScript CreateDebugBPStatuses()
        {
            var pckg = new SourceForScript() { Order = 1, ScriptName = "Cls_BPStatuses" };
            pckg.RootRecords.Add(new RecordsToExtract("Cls_BPStatuses", "BPStatusId between 20 and 41"));
            pckg.TablesToProcess.Add(new TableToExtract("Cls_BPStatuses", new OnlyOneTableExtractStrategy(), new SqlBuildStrategy(false, false, true)));
            return pckg;
        }

        static SourceForScript CreateDebugFlowDefinitions()
        {
            var pckg = new SourceForScript() { Order = 2, ScriptName = "FlowDefinitions" };
            pckg.RootRecords.Add(new RecordsToExtract("FlowDefinitions", "Code = '_IMT_SWIFT_103_INC2'"));
            return pckg;
        }


        static SourceForScript CreateDebugPaymentMessageTypeFields()
        {
            var pckg = new SourceForScript() { Order = 3, ScriptName = "PaymentMessageTypeFields" };
            pckg.RootRecords.Add(new RecordsToExtract("PaymentMessageTypeFields", ""));

            pckg.TablesToProcess.Add(new TableToExtract("PaymentMessageContent", new FKDependencyExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("Cls_PaymentMessageTypes", new OnlyOneTableExtractStrategy(), new SqlBuildStrategy(true, true, false)));
            return pckg;
        }

        static SourceForScript CreateDebugServiceJobSchedules()
        {
            var pckg = new SourceForScript() { Order = 4, ScriptName = "SchedulesAndChains" };
            pckg.RootRecords.Add(new RecordsToExtract("ServiceJobItems"// @"ServiceJobScheduleId in (1559,1560,1561,1563,1562,1565)"

             , @"[Title] in (
                'SWIFT SE processor',
                'SWIFT SE Parse inc TB',
                'SWIFT SE Concat4AML',
                'SWIFT SE Push2AML',
                'SWIFT SE PushAfterAML')"
                ));
            pckg.TablesToProcess.Add(new TableToExtract("ServiceJobs", new FKDependencyExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("ServiceJobItems", new FKDependencyExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("ServiceJobSchedules", new OnlyChildrenExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("ServiceJobItemRelations", new FKDependencyExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("ServiceJobScheduleParameters", new OnlyOneTableExtractStrategy()));
            //pckg.TablesToProcess.Add(new TableToExtract("ServiceJobSchedulesDependencies", new OnlyOneTableExtractStrategy()));

            return pckg;
        }

        static SourceForScript CreateDebugScripts()
        {
            var pckg = new SourceForScript() { Order = 5, ScriptName = "Scripts" };
            pckg.RootRecords.Add(new RecordsToExtract("Scripts", @""));
            return pckg;
        }

        public static IPackage Get()
        {
            var pckgs = new Package()
            {
                Scripts = new List<SourceForScript>() {
                  CreateDebugPackageBP()
                , CreateDebugPackagePresentations()
                , CreateDebugScripts()
                ,
                CreateDebugServiceJobSchedules()
                , CreateDebugPaymentMessageTypeFields()
                , CreateDebugFlowDefinitions()
                , CreateDebugPackageGlobalParameters()
                , CreateDebugBPStatuses() // ?
            }
            };
            return pckgs;
        }

    }
}
