using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Quipu.ParameterizationExtractor.Logic.Interfaces;
using Quipu.ParameterizationExtractor.Logic.Model;
using Quipu.ParameterizationExtractor.Logic.MSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParameterizationExtractor.WebUI.DataAccess
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string defaultConnStr;
        private readonly IHttpContextAccessor _http;
        public UnitOfWorkFactory(IOptions<AppSettings> appSet, IHttpContextAccessor http)
        {
            defaultConnStr = appSet.Value.ConnectionStrings?.DefaultConnection;
            _http = http;
        }
        public IUnitOfWork GetUnitOfWork()
        {
            StringValues connectionString = string.Empty;

            if (_http.HttpContext.Request.Headers.TryGetValue("ConnectionString",out connectionString))
            {
                return GetUnitOfWork(connectionString);
            }

            return GetUnitOfWork(defaultConnStr);
        }

        public IUnitOfWork GetUnitOfWork(string source)
        {
            return new UnitOfWork(source);
        }
    }
}
