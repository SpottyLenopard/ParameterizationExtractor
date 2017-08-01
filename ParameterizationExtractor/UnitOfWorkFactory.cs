using Quipu.ParameterizationExtractor.Common;
using Quipu.ParameterizationExtractor.Logic.Interfaces;
using Quipu.ParameterizationExtractor.Logic.MSSQL;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor
{
    [Export(typeof(IUnitOfWorkFactory))]
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IAppArgs _args;
        [ImportingConstructor]
        public UnitOfWorkFactory(IAppArgs args)
        {
            Affirm.ArgumentNotNull(args, "args");

            _args = args;
        }

        public IUnitOfWork GetUnitOfWork(string source)
        {
            Affirm.NotNullOrEmpty(source, "source");

            return new UnitOfWork(source);
        }

        public IUnitOfWork GetUnitOfWork()
        {
            var connection = _args.ConnectionString ?? ConfigurationManager.ConnectionStrings[_args.ConnectionName].ConnectionString;

            Affirm.NotNullOrEmpty(connection, "Connection string can not be null or empty!");

            return GetUnitOfWork(connection);
        }
    }
}
