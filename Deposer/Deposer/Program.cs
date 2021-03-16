﻿using System;
using System.IO;

namespace Deposer
{
    class Program
    {
        public static Config config = new Config();
        static void Main(string[] args)
        {
            try
            {
                InitData();
                Console.Title = "Déposer";
                Navigator.FileViewer(@"C:/");
            } catch(Exception e)
            {
                Document.Error("Error while initing the program");
            }
        }

        static void InitData()
        {
            Console.Title = "Loading basic files...";
            /* LANG */
            Directory.CreateDirectory("lang");
            FileInfo[] dialogFiles = new FileInfo[] {new FileInfo(@"lang/english.json")};
            Lang.LoadDialogs(dialogFiles);
        }
    }
    class Vector2
    {
        public int x = 0, y = 0;

        public Vector2() { }
        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 Zero()
        {
            return new Vector2();
        }
    }
}
