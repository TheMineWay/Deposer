﻿using System;
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

        public static Element Navigate(DirectoryInfo dir, bool directories = true, bool files = true, bool selecting = false)
        {
            List<Element> elements = new List<Element>();
            List<List<Element>> storedFiles = new List<List<Element>>();
            GenerateList();

            int page = 0, selected = 0;
            while(true)
            {
                try
                {
                    Console.Clear();
                    //Draw toolBox
                    Console.WriteLine(Document.Boxyfy("SPACE: Select   ENTER: Navigate   ESCAPE: Return\nF: Add file   D: Add directory   R: Remove\nU: Change unity"));
                    Lang.SayInFormatLn("nav_pages_display", new string[] { (page + 1).ToString(), (storedFiles.ToArray().Length).ToString() });
                    if(storedFiles.Count <= 0)
                    {
                        Document.Inform(Lang.Get("empty_folder"));
                        storedFiles.Add(new List<Element>());
                    }
                    //Draw files and dirs
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
                        case ConsoleKey.D: CreateDir(dir.FullName); GenerateList(); break;
                        case ConsoleKey.R: if(storedFiles[page].Count > 0) Remove(storedFiles[page][selected]); GenerateList(); break;
                        case ConsoleKey.Enter:
                            if (storedFiles[page][selected].type != Type.directory) break;
                            Element action = Navigate(new DirectoryInfo(storedFiles[page][selected].path), directories, files, selecting); //Navigate into directory
                            if (action.type != Type.cancel) return action;
                            break;
                        case ConsoleKey.Spacebar:
                            if(!selecting)
                            {
                                Document.Error(Lang.Get("no_selecting_enabled"));
                                break;
                            }
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

            void GenerateList()
            {
                elements.Clear();
                storedFiles.Clear();
                elements.AddRange((from directory in Directory.GetDirectories(dir.FullName) select new Element(directory, Type.directory)).ToArray());
                if (files) elements.AddRange((from file in Directory.GetFiles(dir.FullName) select new Element(file, Type.file)).ToArray());

                storedFiles = FilePager(elements.ToArray());
            }
        }
        public static void Remove(Element element)
        {
            if(element.type == Type.file) Document.Inform(Lang.GetInFormat("confirm_delete_file", new string[] { new FileInfo(element.path).Name }));
            else if (element.type == Type.directory) Document.Inform(Lang.GetInFormat("confirm_delete_directory", new string[] { new DirectoryInfo(element.path).Name }));
            if(!Document.GetYesNo())
            {
                Document.Error(Lang.Get("cancelled"));
                return;
            }
            if (element.type == Type.file) File.Delete(element.path);
            else
            {
                int files = 0, folders = 0;
                try
                {
                    int[] removed = RemoveFolder(new DirectoryInfo(element.path));
                    files += removed[0];
                    folders += removed[1];
                    folders++;
                    new DirectoryInfo(element.path).Delete();
                } catch(Exception e)
                {
                    Document.Error("trouble",false,e.Message);
                }
                Document.Inform(Lang.GetInFormat("deleted_details",new string[] {files.ToString(),folders.ToString()}));
                Document.PressAny();
            }
            Document.Inform(Lang.Get("completed_deletion"));
        }
        public static int[] RemoveFolder(DirectoryInfo directory)
        {
            int removedFiles = 0, removedFolders = 0;
            foreach(DirectoryInfo dir in directory.GetDirectories())
            {
                int[] removed = RemoveFolder(dir);
                removedFiles += removed[0];
                removedFolders += removed[1];
                removedFolders++;
                dir.Delete();
            }
            foreach(FileInfo fil in directory.GetFiles())
            {
                fil.Delete();
                removedFiles++;
            }
            return new int[] {removedFiles,removedFolders};
        }
        public static void CreateDir(string currentPath)
        {
            Document.Inform(Lang.Get("create_directory:name") + " ");
            while(true)
            {
                string name = Console.ReadLine();
                if (name == "") return;
                else if(!Directory.Exists(currentPath + @$"/{name}"))
                {
                    try
                    {
                        Directory.CreateDirectory(currentPath + @$"/{name}");
                    } catch(Exception e)
                    {
                        Document.Error(Lang.Get("trouble"),false,e.Message);
                    }
                    return;
                }
                Document.Error(Lang.GetInFormat("directory_already_exists", new string[] {name}));
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