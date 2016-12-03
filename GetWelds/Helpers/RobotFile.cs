using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GetWelds.Converters
{

    public class RobotFile
    {
        private readonly FileInfo _fileInfo;

        public string FullName
        {
            get { return _fileInfo.FullName; }
        }


        public string Name
        {
            get { return _fileInfo.Name; }
        }
        public string Extension { get { return _fileInfo.Extension; } }
        // Etc.  
        public string DirectoryName { get { return _fileInfo.DirectoryName; } }

        public RobotFile(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }

        public RobotFile()
        {
            _fileInfo = new FileInfo(@"C:\Temp\FileInfo.tmp");
        }

    } 
}
