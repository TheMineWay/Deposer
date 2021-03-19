using System;
using System.IO;
using System.Threading;

namespace Deposer
{
    class Program
    {
        public static Config config = new Config();
        enum MainOptions
        {
            navigate,
            exit
        }
        static void Main(string[] args)
        {
            /* PROGRAM INIT */
            try
            {
                InitData();
            } catch(Exception e)
            {
                Document.Error(e.Message);
                Document.Error("Error while initing the program");
                return;
            }
            /* ALL LOADED */
            Console.Title = "Déposer";

            /* MAIN MENU */
            while(true)
            {
                MainOptions option = Document.Menu<MainOptions>.DisplayMenu(new Document.Menu<MainOptions>[] {
                    new Document.Menu<MainOptions>(Lang.Get("menu_navigate"), MainOptions.navigate),
                    new Document.Menu<MainOptions>(Lang.Get("menu_exit"), MainOptions.exit)
                });
                if (option == MainOptions.exit) break;
                
                switch(option)
                {
                    default: Document.Error(Lang.Get("error:not_implemented")); break;
                    case MainOptions.navigate: SelectNavigation(); break;
                }
            }
        }

        static void SelectNavigation()
        {
            DriveInfo[] units = DriveInfo.GetDrives();Document.Menu<DirectoryInfo>[] options = new Document.Menu<DirectoryInfo>[units.Length];
            int i = 0;
            foreach (DriveInfo _unit in units)
            {
                options[i] = new Document.Menu<DirectoryInfo>($"{_unit.Name} {_unit.DriveFormat}",new DirectoryInfo(_unit.Name));
                i++;
            }
            DirectoryInfo unit = Document.Menu<DirectoryInfo>.DisplayMenu(options); //Display
            Navigator.Navigate(unit);
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
