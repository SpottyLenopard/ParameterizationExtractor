using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Logic.Interfaces
{
    public interface IFileService
    {
        void Save(Stream file, string path);
        void Save(string file, string path);
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
    }
}
