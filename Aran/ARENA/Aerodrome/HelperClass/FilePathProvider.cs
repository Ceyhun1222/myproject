using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Metadata
{
    public class FilePathProvider
    {
        private string fileName = @"\AmdbOpenedProjects.ini";
        private string _filePath;

        public FilePathProvider(string filePath)
        {
            _filePath = filePath + fileName;
            if (!File.Exists(_filePath))
                File.Create(_filePath).Close();
        }
        public void Add(string targetPath)
        {
            var pathList = Read();
            if (pathList is null)
                pathList = new List<string>();
            if (!pathList.Contains(targetPath))
                pathList.Add(targetPath);
            Write(pathList);
        }

        public void Delete(string targetPath)
        {
            var pathList = Read();
            if (pathList is null)
                return;
            pathList.Remove(targetPath);
            Write(pathList);
        }

        public bool IsExistTargetPath(string targetPath)
        {
            var pathList = Read();
            if (pathList is null)
                return false;
            var isExist = pathList.Contains(targetPath);
            return isExist; 
        }

        private void Write(List<string> targetPathList)
        {
            string json = JsonConvert.SerializeObject(targetPathList);
            File.WriteAllText(_filePath, json);
        }

        private List<string> Read()
        {
            List<string> targetPathList =
 JsonConvert.DeserializeObject<List<string>>
                                 (File.ReadAllText(_filePath));

            return targetPathList;
        }
    }
}
