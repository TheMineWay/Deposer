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

        public enum Type
        {
            file,
            directory,
            cancel
        }
        public class Element
        {
            public string path;
            public Type type;
            public Element(string path, Type type) {
                this.type = type;
                this.path = path;
            }
            public Element()
            {
                type = Type.cancel;
            }
        }

        public static Element Navigate(DirectoryInfo dir, bool directories = true, bool files = true)
        {
            List<Element> elements = new List<Element>();
            elements.AddRange((from directory in Directory.GetDirectories(dir.FullName) select new Element(directory, Type.directory)).ToArray());
            if(files) elements.AddRange((from file in Directory.GetFiles(dir.FullName) select new Element(file, Type.file)).ToArray());

            List<List<Element>> storedFiles = FilePager(elements.ToArray());
            elements.Clear();

            int page = 0, selected = 0;
            while(true)
            {
                try
                {
                    Console.Clear();
                    Lang.SayInFormatLn("nav_pages_display", new string[] { (page + 1).ToString(), (storedFiles.ToArray().Length).ToString() });
                    if(storedFiles.Count <= 0)
                    {
                        Document.Inform(Lang.Get("empty_folder"));
                        Document.PressAny();
                        return new Element();
                    }
                    for (int i = 0; i < storedFiles[page].Count; i++)
                    {
                        Element element = storedFiles[page][i];
                        if (element.type == Type.directory) Console.ForegroundColor = Program.config.color_directory;
                        else Console.ForegroundColor = Program.config.color_file;
                        Program.config.Arrow(selected == i);
                        Console.Write(new FileInfo(element.path).Name + "\n");
                    }
                    Console.ForegroundColor = Program.config.color_default; //Reset default color

                    //Read user input and process
                    ConsoleKey key = Console.ReadKey().Key;
                    switch (key)
                    {
                        case ConsoleKey.RightArrow: Next(); break;
                        case ConsoleKey.LeftArrow: Back(); break;
                        case ConsoleKey.UpArrow: Up(); break;
                        case ConsoleKey.DownArrow: Down(); break;
                        case ConsoleKey.Escape: return new Element();
                        case ConsoleKey.Enter:
                            if (storedFiles[page][selected].type != Type.directory) break;
                            Element action = Navigate(new DirectoryInfo(storedFiles[page][selected].path), directories, files); //Navigate into directory
                            if (action.type != Type.cancel) return action;
                            break;
                        case ConsoleKey.Spacebar:
                            if ((storedFiles[page][selected].type == Type.directory && !directories) || (storedFiles[page][selected].type == Type.file && !files)) break;
                            return new Element(storedFiles[page][selected].path, storedFiles[page][selected].type);
                    }
                }
                catch(Exception e)
                {
                    Document.Error(Lang.Get("access_denied"));
                }

                void Next()
                {
                    page++;
                    if (page >= storedFiles.ToArray().Length) page = 0;
                    UpdatePage();
                }
                void Back()
                {
                    page--;
                    if (page < 0) page = storedFiles.ToArray().Length - 1;
                    UpdatePage();
                }
                void UpdatePage()
                {
                    if (selected >= storedFiles[page].Count) selected = storedFiles[page].Count - 1;
                }
                void Down()
                {
                    selected++;
                    if (selected >= storedFiles[page].Count) selected = 0;
                }
                void Up()
                {
                    selected--;
                    if (selected < 0) selected = storedFiles[page].Count - 1;
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