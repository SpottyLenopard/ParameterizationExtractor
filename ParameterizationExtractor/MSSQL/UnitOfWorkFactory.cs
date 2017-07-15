using Quipu.ParameterizationExtractor.Common;
using Quipu.ParameterizationExtractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.MSSQL
{
    public class UnitOfWorkFactory : SingletonBase<UnitOfWorkFactory>, IUnitOfWorkFactory
    {
        public IUnitOfWork GetUnitOfWork()
        {
            //return new UnitOfWork("Server=dev3-kiev;Database=CWNET;Integrated Security=True;MultipleActiveResultSets=true;Pooling=True;Max Pool Size=2500");
            
            return new UnitOfWork(@"Server=vm-rof-sqltest\sql2012;Database=DEU_CWNET_ANONY_130417;Integrated Security=True;MultipleActiveResultSets=true;Pooling=True;Max Pool Size=2500");
        }
    }
}
