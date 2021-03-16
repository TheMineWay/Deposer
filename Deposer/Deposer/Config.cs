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

        /* SYMBOLS */
        public char table_horizontal = '-', table_vertical = '|';
        public string arrow = "-->";

        /* DISPLAY */
        public int file_rows = 15;
        public int elements_per_page = 6;
    }
}