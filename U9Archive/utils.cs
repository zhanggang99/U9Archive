using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U9Archive
{
    class utils
    {
        public static void Log(string s)
        {
            StreamWriter sw = new StreamWriter("d:\\loginfo.log", true, Encoding.Default);
            sw.Write(s);
            sw.Close();
        }
        public static void LogError(string s)
        {
            StreamWriter sw = new StreamWriter("d:\\logerror.txt", true, Encoding.Default);
            sw.Write(s);
            sw.Close();
        }
    }
}
