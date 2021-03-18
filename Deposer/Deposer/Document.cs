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

        public static string Boxyfy(string text, TextStyle textStyle = TextStyle.center)
        {
            char boxChar = Program.config.boxchar;
            string[] lines = text.Split('\n');
            int max = 0;
            foreach (string line in lines) if (max < line.Length) max = line.Length;
            string result = InsertChars(max + 4, '*');
            foreach (string line in lines)
            {
                switch(textStyle)
                {
                    case TextStyle.center:
                        int spacing = (max - line.Length) / 2, extra = (max - line.Length) % 2 == 0 ? 0 : 1;
                        string nLine = boxChar.ToString() + InsertChars(spacing + 1, ' ');
                        nLine += line;
                        nLine += InsertChars(spacing + 1 + extra, ' ') + boxChar;
                        result += "\n" + nLine;
                        break;
                    case TextStyle.justify: result += $"\n{boxChar} {Justify(line,max)} {boxChar}"; break;
                    case TextStyle.leftAlign: result += $"\n{boxChar} {LeftAlign(line,max)} {boxChar}"; break;

                }
            }
            result += "\n" + InsertChars(max + 4, '*');
            return result;
        }

        static string InsertChars(int times, char nChar)
        {
            string returner = "";
            for (int i = 0; i < times; i++) returner += nChar;
            return returner;
        }

        static string Justify(string text, int width)
        {
            if(text.Contains(" "))
            {
                while(text.Length < width)
                {
                    List<int> positions = new List<int>();
                    for (int i = 0; i < text.Length; i++) if (text[i] == ' ') positions.Add(i);
                    foreach(int pos in positions)
                    {
                        if (text.Length >= width) break;
                        text = text.Insert(pos," ");
                    }
                }
                return text;
            } else return LeftAlign(text, width);
        }
        static string LeftAlign(string text, int width)
        {
            return text + InsertChars(width - text.Length, ' ');
        }

        public enum TextStyle
        {
            center,
            justify,
            leftAlign
        }

        public static bool GetYesNo()
        {
            Console.Write("(y/n)");
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.Y || key == ConsoleKey.S) return true;
            else return false;
        }

        public class Menu<T> where T : Enum
        {
            public string text;
            public bool enabled;
            public T option;

            public Menu(string text, T option, bool enabled = true)
            {
                this.text = text;
                this.enabled = enabled;
                this.option = option;
            }

            public void Print(bool newLine = true, ConsoleColor color = ConsoleColor.White)
            {
                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.Write(text + (newLine ? "\n" : ""));
                Console.ForegroundColor = current;
            }

            public static T DisplayMenu(Menu<T>[] options)
            {
                List<List<Menu<T>>> processed = new List<List<Menu<T>>>();
                int count = 0, current = 0;
                foreach(Menu<T> option in options)
                {
                    processed[current].Add(option);
                    count++;
                    if (count >= Program.config.elements_per_page)
                    {
                        count = 0;
                        current++;
                    }
                }
                if (processed.Count <= 0) processed.Add(new List<Menu<T>>()); //Empty menu

                int page = 0, selected = 0;
                while(true)
                {
                    int i = 0;
                    foreach(Menu<T> option in processed[page])
                    {
                        Program.config.Arrow(i == selected);
                        option.Print();
                        i++;
                    }

                    ConsoleKey key = Console.ReadKey().Key;
                    switch(key)
                    {
                        case ConsoleKey.Enter: return processed[page][selected].option;
                    }
                }

                void Down()
                {
                    selected++;
                    if (selected >= processed[page].Count) selected = 0;
                }
                void Up()
                {
                    selected--;
                    if (selected < 0) selected = processed[page].Count - 1;
                }
            }
        }
    }
}