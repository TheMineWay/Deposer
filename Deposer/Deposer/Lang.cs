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
                Dictionary<string, string> mapDialog = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(dialog.FullName).ToString());
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
        public static string GetInFormat(string id, string[] values)
        {
            string message = Get(id);
            for(int i = 0; i < values.Length; i++) message = message.Replace("{" + i + "}", values[i]);
            return message;
        }
        public static void Say(string id)
        {
            Console.Write(Get(id));
        }
        public static void SayLn(string id)
        {
            Console.WriteLine(Get(id));
        }
        public static void SayInFormat(string id, string[] values)
        {
            Console.Write(GetInFormat(id,values));
        }
        public static void SayInFormatLn(string id, string[] values)
        {
            Console.WriteLine(GetInFormat(id, values));
        }
    }
}