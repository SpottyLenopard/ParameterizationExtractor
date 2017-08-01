using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParameterizationExtractor.WebUI
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings  { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

}
