using System;
using System.Collections.Generic;
using System.IO;

namespace TokyoU.FileSystem
{
    public class FileSysHelper
    {
        public static string ReadFileAsString(string path)
        {
            return File.ReadAllText(path);
        }

        public static IEnumerable<string> ReadFileAsLines(string path)
        {
            return File.ReadLines(path);
        }

        public static StreamReader OpenFileAsStream(string path)
        {
            return new StreamReader(path);
        }
        
        //随机方式
        public static FileStream OpenFileAsFileStream(string path, FileMode mode)
        {
            FileStream fileStream = new FileStream(path, mode);
            return fileStream;
        }
    }
}