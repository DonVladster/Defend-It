using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_It.IO_Components
{
    public static class ErrorLog
    {
        private const string Defaultpath = "ERROR LOG.txt";

        private static string Path;

        public static void Initialize(string path)
        {
            Path = path;
        }

        public static void Write(string message)
        {
            if (Path == null) Path = Defaultpath;

            using (var sw = new StreamWriter(Path, true))
            {
                sw.WriteLine(DateTime.Now+ "|" +  message);
            }
        }

    }
}
