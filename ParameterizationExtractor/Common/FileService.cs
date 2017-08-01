using Quipu.ParameterizationExtractor.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Quipu.ParameterizationExtractor.Common
{
    public class FileService : SingletonBase<FileService>, IFileService
    {
        public void CreateDirectory(string path)
        {
            System.IO.Directory.CreateDirectory(path);
        }

        public bool DirectoryExists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public void Save(string file, string path)
        {
            System.IO.File.WriteAllText(path, file);
        }

        public void Save(Stream file, string path)
        {
            Affirm.ArgumentNotNull(file, "file");

            using (var fileStream = File.Create(path))
            {
                file.Seek(0, SeekOrigin.Begin);
                file.CopyTo(fileStream);
            }
        }
    }
}
