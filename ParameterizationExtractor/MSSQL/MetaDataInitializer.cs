using Quipu.ParameterizationExtractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quipu.ParameterizationExtractor.Model;
using System.Data;

namespace Quipu.ParameterizationExtractor.MSSQL
{
    public class MetaDataInitializer : IMetaDataInitializer
    {
        private readonly Dictionary<string, Type> sqlToNetTypes = new Dictionary<string, Type>();

        public MetaDataInitializer()
        {
            sqlToNetTypes.Add("int", typeof(int));
            sqlToNetTypes.Add("bigint", typeof(long));
            sqlToNetTypes.Add("varchar", typeof(string));
            sqlToNetTypes.Add("nvarchar", typeof(string));
            sqlToNetTypes.Add("datetime", typeof(DateTime));
            sqlToNetTypes.Add("bit", typeof(bool));
            sqlToNetTypes.Add("numeric", typeof(float));

            sqlToNetTypes.Add("money", typeof(decimal));
            sqlToNetTypes.Add("smalldatetime", typeof(DateTime));
            sqlToNetTypes.Add("tinyint", typeof(Byte));
            sqlToNetTypes.Add("xml", typeof(string));
        }

        private Type GetNETType(string sqlType)
        {
            if (sqlToNetTypes.ContainsKey(sqlType))
            {
                return sqlToNetTypes[sqlType];
            }
            else
            {
                return typeof(object); 
            }
        }

        public PFieldMetadata InitTableMetaData(DataRow metaData, IList<DataRow> indexesMetaData)
        {
            var pkField = indexesMetaData?.FirstOrDefault()?["ColumnName"].ToString();
            
            var result = new PFieldMetadata()
            {
                FieldName = metaData["column_name"].ToString(),
                FieldType = GetNETType(metaData["data_type"].ToString()),
                SqlType = metaData["data_type"].ToString(),
                IsPK = !string.IsNullOrEmpty(pkField) && pkField == metaData["column_name"].ToString()
            };

            if (indexesMetaData != null && indexesMetaData.FirstOrDefault()?["IsIdentity"] != null)
                result.IsIdentity = bool.Parse(indexesMetaData.First()["IsIdentity"].ToString());

            return result;
        }
    }
}
