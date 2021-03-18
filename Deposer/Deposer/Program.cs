using System;
using System.IO;

namespace Deposer
{
    class Program
    {
        public static Config config = new Config();
        enum MainOptions
        {
            first,
            second
        }
        static void Main(string[] args)
        {
            try
            {
                InitData();
            } catch(Exception e)
            {
                Document.Error(e.Message);
                Document.Error("Error while initing the program");
                return;
            }
            Console.Title = "Déposer";
            //Navigator.Navigate(new DirectoryInfo(@"A:/"), true, true);
            //Document.Menu<MainOptions>.DisplayMenu(new Document.Menu<MainOptions>[] { new Document.Menu<MainOptions>("First", MainOptions.first) });
            //Console.WriteLine(Document.Boxyfy("Hola muy buenas a\ntodo el mundo desde aguacates.\nDa igual lo que pongas, el programa lo mete en una caja él solito", Document.TextStyle.justify));
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
