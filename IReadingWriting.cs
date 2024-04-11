using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AccessLog
{
    interface IReadingWriting
    {
        string ReadingFromFile();

        void WriteingToFile(string message);
    }
}
