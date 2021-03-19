using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deposer
{
    class Config
    {
        /* COLORS */
        public ConsoleColor color_default =  ConsoleColor.White; //Text's default color
        public ConsoleColor color_file = ConsoleColor.DarkYellow;
        public ConsoleColor color_directory = ConsoleColor.White;
        public ConsoleColor color_arrow = ConsoleColor.Cyan;
        public ConsoleColor color_error = ConsoleColor.Red;
        public ConsoleColor color_information = ConsoleColor.Blue;

        /* SYMBOLS */
        public char table_horizontal = '-', table_vertical = '|', boxchar = '*';
        public string arrow = "-->";
        public char[] progress_arrow = new char[] {'-','>',' '}; // Length must be 3
        public void Arrow(bool display = true)
        {
            ConsoleColor current = Console.ForegroundColor;
            Console.ForegroundColor = color_arrow;
            if (display) Console.Write(arrow + " ");
            else
            {
                string printer = " ";
                for (int i = 0; i < arrow.Length; i++) printer += " ";
                Console.Write(printer);
            }
            Console.ForegroundColor = current;
        }

        /* DISPLAY */
        public int file_rows = 15;
        public int elements_per_page = 18;
        public int progress_bar_len = 40;

        /* ERRORS */
        public bool skipErrors = false;
    }
}