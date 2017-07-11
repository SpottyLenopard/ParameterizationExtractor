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
  i.name                                as IndexName
, object_name(ic.OBJECT_ID)             as TableName
, col_name(ic.OBJECT_ID ,ic.column_id)  as ColumnName
, c.is_identity                         as IsIdentity 
from
sys.indexes                             as i
inner join sys.index_columns            as ic on i.OBJECT_ID = ic.OBJECT_ID
 and i.index_id = ic.index_id
inner join sys.[columns] c on c.[object_id] = ic.[object_id] and c.column_id = ic.column_id
where
 i.is_primary_key = 1
order by object_name(ic.OBJECT_ID)
";

        private readonly IEnumerable<PDependentTable> _dependentTables;
        private readonly IEnumerable<PTableMetadata> _tables;
        public MSSQLSourceSchema(IEnumerable<PDependentTable> dependentTables, IEnumerable<PTableMetadata> tables)
        {
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
    }
}
