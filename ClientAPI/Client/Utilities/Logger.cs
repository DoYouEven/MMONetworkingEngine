using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Util
{
    public static class Logger
    {
        public static void ResetLogFile(string FileName)
        {
            string path = Directory.GetCurrentDirectory();
            path = path + "\\" + FileName + ".txt"; ;
            if (!File.Exists(path))
            {
                return;
            }
            File.WriteAllText(path, string.Empty);
        

        }
        public static void LogToFile(string FileName, string Data)
        {
            string path = Directory.GetCurrentDirectory();
            path = path + "\\" + FileName+".txt";
            string CurrentTime = "["+DateTime.Now.ToString("HH:mm:ss tt")+"]";
            if(! File.Exists(path))// == false ? File.CreateText(path) : File.AppendText(path);
           
                using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(CurrentTime + " Data : " +  Data);

            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(CurrentTime + " Data : " + Data);

                }
            }

        }
    }
}
