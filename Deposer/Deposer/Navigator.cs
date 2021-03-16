using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Deposer
{
    class Navigator
    {
        public static string Navigate(string startPath, bool directories = true, bool files = true)
        {
            //List<Element>
            foreach(DirectoryInfo directory in (from directory in Directory.GetDirectories(startPath) select new DirectoryInfo(directory)).ToArray())
            {

            }
            return startPath;
        }

        public enum Type
        {
            file,
            directory
        }
        class Element
        {
            public string path;
            public FileInfo file;
            public Type type;
            public Element(string path, Type type) {
                this.type = type;
                this.path = path;
                file = new FileInfo(path);
            }
        }

        public static string FileViewer(string path)
        {
            List<Element> elements = new List<Element>();
            elements.AddRange((from directory in Directory.GetDirectories(path) select new Element(directory, Type.directory)).ToArray());
            elements.AddRange((from file in Directory.GetFiles(path) select new Element(file, Type.file)).ToArray());

            List<List<Element>> storedFiles = FilePager(elements.ToArray());

            int page = 0, selected = 0;
            while(true)
            {
                Console.Clear();
                Lang.Say("page");
                foreach(Element element in storedFiles[page])
                {
                    if (element.type == Type.directory) Console.ForegroundColor = Program.config.color_directory;
                    else Console.ForegroundColor = Program.config.color_file;
                    Console.WriteLine(element.file.Name);
                }
                Console.ForegroundColor = Program.config.color_default; //Reset default color

                //Read user input and process
                ConsoleKey key = Console.ReadKey().Key;
                switch(key)
                {
                    case ConsoleKey.RightArrow: Next(); break;
                    case ConsoleKey.LeftArrow: Back(); break;
                }

                void Next()
                {
                    page++;
                    if (page >= storedFiles.ToArray().Length) page = 0;
                }
                void Back()
                {
                    page--;
                    if (page < 0) page = storedFiles.ToArray().Length - 1;
                }
            }
        }
        private static List<List<Element>> FilePager(Element[] elements)
        {
            List<List<Element>> returner = new List<List<Element>>();
            int count = 0, index = 0;
            foreach(Element element in elements)
            {
                if(count == 0) returner.Add(new List<Element>());
                returner[index].Add(element);
                count++;
                if(count >= Program.config.elements_per_page)
                {
                    count = 0;
                    index++;
                }
            }
            return returner;
        }
    }
}