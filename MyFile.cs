using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AccessLog
{
    abstract class MyFile
    {        
        private readonly static string fileName;
        private readonly string _filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);

        public abstract string ReadingFromFile(string path);

        public abstract void WriteingToFile(string path);
    }
}
