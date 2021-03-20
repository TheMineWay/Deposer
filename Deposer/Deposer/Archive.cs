using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Deposer
{
    class Archive
    {
        public static string Map(DirectoryInfo directory, string destinationPath)
        {
            return "";
        }

        public class DirectoryMap
        {
            public List<DirectoryMap> directories = new List<DirectoryMap>();
            public List<File> files = new List<File>();

            public string name;

            public DirectoryMap(DirectoryInfo directory)
            {
                name = directory.Name;
                try
                {
                    foreach (DirectoryInfo dir in directory.GetDirectories()) directories.Add(new DirectoryMap(dir));
                } catch(Exception e)
                {

                }
                try
                {
                    foreach (FileInfo file in directory.GetFiles()) files.Add(new File(file));
                } catch (Exception e)
                {

                }
            }
        }

        public class File
        {
            public string name;

            public File(FileInfo file)
            {
                name = file.Name;
            }
        }
    }
}