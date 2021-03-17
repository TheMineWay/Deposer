using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deposer
{
    class Document
    {
        public static void Error(string text, bool skipable = true, string details = "")
        {
            ConsoleColor current = Console.ForegroundColor;
            Console.ForegroundColor = Program.config.color_error;
            Console.WriteLine("\n[!] " + text);
            if (details.Length > 0) Lang.SayInFormat("details", new string[] { details });
            if (skipable && Program.config.skipErrors) return;
            PressAny();
            Console.ForegroundColor = current;
        }
        public static void Inform(string text)
        {
            ConsoleColor current = Console.ForegroundColor;
            Console.ForegroundColor = Program.config.color_information;
            Console.WriteLine("[*] " + text);
            Console.ForegroundColor = current;
        }
        public static void PressAny()
        {
            ConsoleColor current = Console.ForegroundColor;
            Console.ForegroundColor = Program.config.color_default;
            Lang.SayLn("press_any_key");
            Console.ReadKey();
            Console.ForegroundColor = current;
        }
    }
}