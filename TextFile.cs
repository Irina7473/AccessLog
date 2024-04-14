using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AccessLog
{
    internal class TextFile: IReadingWriting 
    {
        public static event Action<string> Notify;
        
        private readonly string _filePath;

        public TextFile (string path, string name)
        {
            try
            {
                _filePath = Path.Combine(path, name);
            }
            catch (Exception e) { Notify?.Invoke(e.ToString()); }
        }

        public TextFile(string name)
        {
            try
            {
                _filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
            }
            catch (Exception e) { Notify?.Invoke(e.ToString()); }
        }

        //Чтение из файла
        public string ReadingFromFile()
        {
            string data = "";
            if (File.Exists(_filePath))
            {
                try
                {
                    using (StreamReader reader = new(_filePath))
                    { data += reader.ReadToEnd(); }
                }
                catch (Exception e) { Notify?.Invoke(e.ToString()); }
            }
            return data;
        }        

        //Запись в файл
        public async void WriteingToFile(string message)
        {            
            try
            {
                using (StreamWriter writer = new(_filePath, false))
                { await writer.WriteLineAsync(message); }
            }
            catch (InvalidOperationException)
            {
                Notify?.Invoke("Поток в настоящее время используется предыдущей операцией записи.");
            }
            catch (Exception e) { Notify?.Invoke(e.ToString()); }
        }

    }
}
