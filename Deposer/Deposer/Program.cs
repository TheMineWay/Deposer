using System;
using System.IO;
using System.Threading;

namespace Deposer
{
    class Program
    {
        public readonly static string title = "\n\t██████╗ ███████╗██████╗  ██████╗ ███████╗███████╗██████╗ \n\t██╔══██╗██╔════╝██╔══██╗██╔═══██╗██╔════╝██╔════╝██╔══██╗\n\t██║  ██║█████╗  ██████╔╝██║   ██║███████╗█████╗  ██████╔╝\n\t██║  ██║██╔══╝  ██╔═══╝ ██║   ██║╚════██║██╔══╝  ██╔══██╗\n\t██████╔╝███████╗██║     ╚██████╔╝███████║███████╗██║  ██║\n\t╚═════╝ ╚══════╝╚═╝      ╚═════╝ ╚══════╝╚══════╝╚═╝  ╚═╝                                                         \n\t" + Lang.Get("by") + " TheMineWay";
        public static Config config = new Config();
        enum MainOptions
        {
            navigate,
            mapper,
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

            /* MAIN MENU */
            while(true)
            {
                Console.Title = "Déposer";
                MainOptions option = Document.Menu<MainOptions>.DisplayMenu(new Document.Menu<MainOptions>[] {
                    new Document.Menu<MainOptions>(Lang.Get("menu_navigate"), MainOptions.navigate),
                    new Document.Menu<MainOptions>(Lang.Get("menu_directory_mapper"), MainOptions.mapper),
                    new Document.Menu<MainOptions>(Lang.Get("menu_exit"), MainOptions.exit)
                },title);
                if (option == MainOptions.exit) break;
                
                switch(option)
                {
                    default: Document.Error(Lang.Get("error:not_implemented")); break;
                    case MainOptions.navigate: Navigator.UnitNavigator(); break;
                    case MainOptions.mapper: MapAssistant(); break;
                }
            }
        }

        enum MapOptions
        {
            map,
            exit
        }
        static void MapAssistant()
        {
            do
            {
                MapOptions option = Document.Menu<MapOptions>.DisplayMenu(new Document.Menu<MapOptions>[] {
                    new Document.Menu<MapOptions>(Lang.Get("map_menu_dirmap"),MapOptions.map),
                    new Document.Menu<MapOptions>(Lang.Get("menu_back"), MapOptions.exit)
                });
                if (option == MapOptions.exit) break;

                switch(option)
                {
                    default: Document.Error(Lang.Get("error:not_implemented")); break;
                    case MapOptions.map: SaveMap(); break;
                }
            } while (true);

            void SaveMap()
            {
                // Create a map of a directory
                Navigator.Element dir = Navigator.UnitNavigator(true, false, true,Lang.Get("createmap_what"));
                if (dir.type == Navigator.Type.cancel) return;
                Document.Load process = new Document.Load(1); // 0%
                Archive.DirectoryMap mapped = new Archive.DirectoryMap(new DirectoryInfo(dir.path));
                Navigator.Element where = Navigator.SelectNewFile("json");
                if (where.type == Navigator.Type.cancel) return;
                File.WriteAllText(where.path, Newtonsoft.Json.JsonConvert.SerializeObject(mapped));
                process.Completed(); // 100%
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
