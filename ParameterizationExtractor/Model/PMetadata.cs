using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Model
{
    public class PTableMetadata : HashSet<PFieldMetadata>
    {
        public PTableMetadata()
        {
            UniqueColumnsCollection = new List<string>();
        }

        private PFieldMetadata _pk;
        public PFieldMetadata PK
        {
            get {
                _pk = _pk ?? this.First(_ => _.IsPK);

                return _pk;
            }
        }
        public string TableName { get; set; }
        public IList<string> UniqueColumnsCollection { get; set; }
    }
     
    public class PFieldMetadata 
    {
        public PFieldMetadata()
        {

        }

        public string SqlType { get; set; }
        public bool IsPK { get; set; }
        public bool IsIdentity { get; set; }
        public string FieldName { get; set; }
        public Type FieldType { get; set; }

        public override bool Equals(object obj)
        {
            var p = obj as PFieldMetadata;
            if (p != null)
                return p.FieldName == this.FieldName;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    [DebuggerDisplay("{ParentTable}-{ReferencedTable}-{ParentColumn}-{ReferencedColumn}")]
    public class PDependentTable
    {
        public string Name { get; set; } 
        public string ParentTable { get; set; }
        public string ParentColumn { get; set; }
        public string ReferencedTable { get; set; }
        public string ReferencedColumn { get; set; }
    }    
}
