using Quipu.ParameterizationExtractor.Interfaces;
using Quipu.ParameterizationExtractor.Model;
using Quipu.ParameterizationExtractor.MSSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quipu.ParameterizationExtractor.Templates;
using Quipu.ParameterizationExtractor.Common;

namespace ParameterizationExtractor
{
    public class Program
    {
        public static ILog _log;        

        static void FillConfigForDebug()
        {
            var GlobalConfiguration = GlobalExtractConfiguration.GetInstance();
            GlobalConfiguration.DefaultExtractStrategy = new OnlyChildrenExtractStrategy();
            GlobalConfiguration.DefaultSqlBuildStrategy = new SqlBuildStrategy();
            GlobalConfiguration.FieldsToExclude.Add("CreatorId");
            GlobalConfiguration.FieldsToExclude.Add("Created");

            GlobalConfiguration.UniqueColums.Add("BusinessProcessSteps", new UniqueColumnsCollection() { "BPTypeCode", "StepStartStatusId", "StepEndStatusId" });
            GlobalConfiguration.UniqueColums.Add("bpProcessingConditions", new UniqueColumnsCollection() { "ConditionText" });
            GlobalConfiguration.UniqueColums.Add("Presentations", new UniqueColumnsCollection() { "Name", "Code" });
            GlobalConfiguration.UniqueColums.Add("FlowDefinitions", new UniqueColumnsCollection() {"Code" });
            GlobalConfiguration.UniqueColums.Add("Scripts", new UniqueColumnsCollection() { "Code" });
            GlobalConfiguration.UniqueColums.Add("PaymentMessageContent", new UniqueColumnsCollection() { "Code" });
            GlobalConfiguration.UniqueColums.Add("BpTypeDefaultExecutionParameters", new UniqueColumnsCollection() { "ParameterName", "BPTypeCode" });
            GlobalConfiguration.UniqueColums.Add("BPAttrTypes", new UniqueColumnsCollection() { "Code" });
            GlobalConfiguration.UniqueColums.Add("sys_Parameters", new UniqueColumnsCollection() { "ParameterName" });
            GlobalConfiguration.UniqueColums.Add("Cls_PaymentMessageTypes", new UniqueColumnsCollection() { "Code" });
            GlobalConfiguration.UniqueColums.Add("PaymentMessageTypeFields", new UniqueColumnsCollection() { "Code" });
            GlobalConfiguration.UniqueColums.Add("ServiceJobs", new UniqueColumnsCollection() { "Title", "Description" });
            GlobalConfiguration.UniqueColums.Add("ServiceJobItems", new UniqueColumnsCollection() { "MethodName", "ObjectAssemblyName", "ObjectTypeName" });
        }

        static void Main(string[] args)
        {
            _log = new ConsoleLogger();
            FillConfigForDebug();
            try
            {
                MainAsync()
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
           
            Console.ReadKey();
        }

        static PackageTemplate CreateDebugPackageBP()
        {
            var pckg = new PackageTemplate() { Order = 10, PackageName= "_IMT_SWIFT_103_INC2" };
            
          
            pckg.RootRecords.Add(new RecordsToExtract("BusinessProcessesTypes", "bptypecode = '_IMT_SWIFT_103_INC2'"));
            pckg.Exceptions.Add("Users");
            pckg.TablesToProcess.Add(new TableToExtract("BusinessProcessSteps", new FKDependencyExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("BpTypeStepPresentations", new FKDependencyExtractStrategy(new List<string>() { "BusinessProcessSteps" })));
            pckg.TablesToProcess.Add(new TableToExtract("Presentations", new OnlyOneTableExtractStrategy(),new SqlBuildStrategy(true,true,false)));
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

        static PackageTemplate CreateDebugPackageGlobalParameters()
        {
            var pckg = new PackageTemplate() { Order = 9, PackageName = "GlobalParameters" };
            pckg.RootRecords.Add(new RecordsToExtract("GlobalParameters", "Code in ('SSERootDir')"));
            pckg.RootRecords.Add(new RecordsToExtract("sys_Parameters", "ParameterName in ('StateMachine','SwiftReason')"));

            return pckg;
        }

        static PackageTemplate CreateDebugPackagePresentations()
        {
            var pckg = new PackageTemplate() { Order = 0, PackageName = "Presentations" };
            pckg.RootRecords.Add(new RecordsToExtract("Presentations", ""));         
            pckg.TablesToProcess.Add(new TableToExtract("PresentationValidationRules", new OnlyOneTableExtractStrategy()));
            return pckg;
        }
        static PackageTemplate CreateDebugBPStatuses()
        {
            var pckg = new PackageTemplate() { Order = 1, PackageName = "Cls_BPStatuses" };
            pckg.RootRecords.Add(new RecordsToExtract("Cls_BPStatuses", "BPStatusId between 20 and 41"));
            pckg.TablesToProcess.Add(new TableToExtract("Cls_BPStatuses", new OnlyOneTableExtractStrategy(), new SqlBuildStrategy(false, false, true)));
            return pckg;
        }

        static PackageTemplate CreateDebugFlowDefinitions()
        {
            var pckg = new PackageTemplate() { Order = 2, PackageName = "FlowDefinitions" };
            pckg.RootRecords.Add(new RecordsToExtract("FlowDefinitions", "Code = '_IMT_SWIFT_103_INC2'"));
            return pckg;
        }


        static PackageTemplate CreateDebugPaymentMessageTypeFields()
        {
            var pckg = new PackageTemplate() { Order = 3, PackageName = "PaymentMessageTypeFields" };
            pckg.RootRecords.Add(new RecordsToExtract("PaymentMessageTypeFields", ""));

            pckg.TablesToProcess.Add(new TableToExtract("PaymentMessageContent", new FKDependencyExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("Cls_PaymentMessageTypes", new OnlyOneTableExtractStrategy(), new SqlBuildStrategy(true,true,false)));
            return pckg;
        }

        static PackageTemplate CreateDebugServiceJobSchedules()
        {
            var pckg = new PackageTemplate() { Order = 4, PackageName = "SchedulesAndChains" };
            pckg.RootRecords.Add(new RecordsToExtract("ServiceJobItems"// @"ServiceJobScheduleId in (1559,1560,1561,1563,1562,1565)"

             , @"[Title] in (
                'SWIFT SE processor',
                'SWIFT SE Parse inc TB',
                'SWIFT SE Concat4AML',
                'SWIFT SE Push2AML',
                'SWIFT SE PushAfterAML')"
                ));
            pckg.Exceptions.Add("Users");
            pckg.TablesToProcess.Add(new TableToExtract("ServiceJobs", new FKDependencyExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("ServiceJobItems", new FKDependencyExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("ServiceJobSchedules", new OnlyChildrenExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("ServiceJobItemRelations", new FKDependencyExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("ServiceJobScheduleParameters", new OnlyOneTableExtractStrategy()));
            //pckg.TablesToProcess.Add(new TableToExtract("ServiceJobSchedulesDependencies", new OnlyOneTableExtractStrategy()));
            
            return pckg;
        }

        static PackageTemplate CreateDebugScripts()
        {
            var pckg = new PackageTemplate() { Order = 5, PackageName = "Scripts" };
            pckg.RootRecords.Add(new RecordsToExtract("Scripts", @""));
            pckg.Exceptions.Add("Users");
            return pckg;
        }

        static async Task MainAsync()
        {
            var pckgs = new List<IPackageTemplate>() {
                  CreateDebugPackageBP()
                , CreateDebugPackagePresentations()
                , CreateDebugScripts()
                ,
                CreateDebugServiceJobSchedules()
                , CreateDebugPaymentMessageTypeFields()
                , CreateDebugFlowDefinitions()
                , CreateDebugPackageGlobalParameters()
                , CreateDebugBPStatuses() // ?
            };

            //"ServiceJobItemRelationLog" "ServiceJobLog" "ServiceJobItemRelationLog"

            var schema = new MSSQLSourceSchema(UnitOfWorkFactory.GetInstance(), GlobalExtractConfiguration.GetInstance());

            await schema.Init();

            foreach (var pckg in pckgs)
            {
                var builder = new DependencyBuilder(UnitOfWorkFactory.GetInstance(), schema, _log, pckg);

                var pTables = await builder.PrepareAsync();
                var sqlBuilder = new MSSqlBuilder();

                if (!System.IO.Directory.Exists(".\\Output"))
                    System.IO.Directory.CreateDirectory(".\\Output");

                FileService.GetInstance().Save(sqlBuilder.Build(pTables,schema), string.Format(".\\Output\\{0}_p_{1}.sql", pckg.Order, pckg.PackageName));
            }
            
            _log.Debug(string.Empty);


            //Process(pTables);


        }
    
        
    }
}
