using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public static class ClientTools
    {
        public static string GetClient(string? line)
        {
            string type = line.Split(' ')[0];
            return type;
        }

        public static string GetFilePath(string filePath, string fileName)
        {
            int counter = 0;
            string fullPath = filePath + Path.GetFileName(fileName);


            while (File.Exists(fullPath))
            {
                counter++;
                fullPath = $"{filePath}{Path.GetFileNameWithoutExtension(fileName)}({counter}){Path.GetExtension(fullPath)}";
            }
            return fullPath;
        }
    }
}
