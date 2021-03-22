using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deposer
{
    class FileEditor
    {
        public static void Edit(FileInfo file)
        {
            Console.Clear();
            FileFormatter formatter = new FileFormatter(file);
            formatter.Print();
            Document.PressAny();
        }
    }

    class FileFormatter
    {
        FileInfo file;

        public FileFormatter(FileInfo file)
        {
            this.file = file;
        }
        public void Print()
        {
            Format(file);
        }

        public static void Format(FileInfo file)
        {
            string content = File.ReadAllText(file.FullName);
            switch(file.Extension.ToLower())
            {
                case ".json": JsonPrinter(content); break;
                default: Console.WriteLine(content); break;
            }
        }

        static void JsonPrinter(string content)
        {
            content = content.Replace("}","\n}");
            content = content.Replace("{", "{\n");
            Console.WriteLine(content);
        }
    }
}