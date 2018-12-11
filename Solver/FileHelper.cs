using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPSonline.AoC
{
    public class FileHelper
    {
        public static string GetInputLocation(int day, int year)
        {
            string assemblyPath = new Uri(Assembly.GetCallingAssembly().CodeBase).LocalPath;
            assemblyPath = Path.GetDirectoryName(assemblyPath);
            return Path.Combine(assemblyPath, $"Inputs\\{year}\\Day{day}.txt");
        }
    }
}
