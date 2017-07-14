using Quipu.ParameterizationExtractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quipu.ParameterizationExtractor.Model;
using System.Data.SqlClient;
using System.Data;

namespace Quipu.ParameterizationExtractor.MSSQL
{
    public class MSSQLSourceSchema : ISourceSchema
    {
        public static string sqlFKs = @"        
SELECT  
    fk.name as [Name],
    OBJECT_NAME(fk.parent_object_id) 'ParentTable',
    c1.name 'ParentColumn',
    OBJECT_NAME(fk.referenced_object_id) 'ReferencedTable',
    c2.name 'ReferencedColumn'
FROM 
    sys.foreign_keys fk
INNER JOIN 
    sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
INNER JOIN
    sys.columns c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id
INNER JOIN
    sys.columns c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id
        ";

        public static string sqlPKColumns = @"
select
  C.[object_id]
, object_name(C.[object_id])             as TableName
, C.column_id
, C.is_identity as IsIdentity 
, C.is_computed as IsComputed 
, C.[name] as ColumnName
, T.[name] as TypeName
, TBase.[name] as BaseTypeName
, TBase.[name]+ case
  when (TBase.user_type_id in (165, 167, 173, 175, 231, 239)) then
   case
       when C.max_length = -1 then '(max)'
       else '('+
     case
         when T.system_type_id in (231, 239) then cast(C.max_length/2 as nvarchar(max))
         else cast(C.max_length as nvarchar(max))
     end
       +')'
       --else '('+cast(T.max_length as nvarchar(max))+')'
   end
  /*165-varbinary; 167-varchar; 173-binary; 175-char, 231-nvarchar; 239-nchar*/
  when (TBase.user_type_id in (106, 108))
   then '('+cast(C.[precision] as nvarchar(max))+','+cast(C.scale as nvarchar(max))+')'
  /*106-decimal; numeric-108*/
  else ''
     end
   as base_type_name_str
, case
      when exists
      (
       select * from sys.indexes i
       inner join sys.index_columns ic on ic.[object_id] = i.[object_id] and ic.index_id = i.index_id
       where ic.[object_id] = C.[object_id]
       and ic.column_id = C.column_id
       and i.is_primary_key = 1
      )
      then cast(1 as bit)
      else cast(0 as bit)
  end as IsInPK
from sys.[columns] C
inner join sys.types T on T.system_type_id = C.system_type_id and T.user_type_id = C.user_type_id
inner join sys.types TBase on T.system_type_id = TBase.system_type_id
 and TBase.system_type_id = TBase.user_type_id
where 
--C.[object_id] = object_id('BusinessProcesses_Id')
--C.[object_id] = object_id('LoanApplications')
--C.[object_id] = object_id('f_rep_GetContractStatement_byAccountNumber') 
 T.system_type_id not in (189, 240)
/*189-timestamp;240-all CLR*/
and C.is_computed = 0
order by C.column_id
";

        private readonly IEnumerable<PDependentTable> _dependentTables;
        private readonly IEnumerable<PTableMetadata> _tables;
        public MSSQLSourceSchema(IEnumerable<PDependentTable> dependentTables, IEnumerable<PTableMetadata> tables)
        {
            Affirm.ArgumentNotNull(dependentTables, "dependentTables");
            Affirm.ArgumentNotNull(tables, "tables");

            _tables = tables;
            _dependentTables = dependentTables;            
        }

        public IEnumerable<PDependentTable> DependentTables
        {
            get
            {
                return _dependentTables;
            }
        }

        public IEnumerable<PTableMetadata> Tables
        {
            get
            {
                return _tables;
            }
        }

        public PTableMetadata GetTableMetaData(string tableName)
        {
            return Tables.First(_ => _.TableName == tableName);
        }
    }
}
