using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Deposer
{
    /* MULTILANGUAGE COMPONENT */
    class Lang
    {
        private static string language = "English";
        private static Dictionary<string,string> langStorage = new Dictionary<string, string>();

        public static void LoadDialogs(FileInfo[] dialogs)
        {
            foreach(FileInfo dialog in dialogs)
            {
                Dictionary<string, string> mapDialog = (Dictionary<string, string>)JsonConvert.DeserializeObject(File.ReadAllText(dialog.FullName));
                foreach(string key in mapDialog.Keys)
                {
                    if (langStorage.ContainsKey(key)) langStorage[key] = mapDialog[key];
                    else langStorage.Add(key,mapDialog[key]);
                }
            }
        }
        public static string Get(string id)
        {
            if (langStorage.ContainsKey(id)) return langStorage[id];
            return "[!] No dialog [!]";
        }
        public static void Say(string id)
        {
            Console.Write(Get(id));
        }
        public static void SayLn(string id)
        {
            Console.WriteLine(Get(id));
        }
    }
}