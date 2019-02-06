using System;
using System.IO;

namespace Kalladystine.DataModels.Generators.Models
{
    [Serializable]
    public class GeneratorDirectorySet
    {
        public string WorkingDirectory;
        public bool CreateWorkingDirectoryIfNotExists;
        public bool CleanWorkingDirectory = true;
        public string NupkgStoreDirectory;
        public bool CreateNupkgStoreDirectoryIfNotExists;

        public bool EnsureDirectories()
        {
            bool result = true;

            if (!Directory.Exists(WorkingDirectory))
            {
                if (CreateWorkingDirectoryIfNotExists)
                {
                    Directory.CreateDirectory(WorkingDirectory);
                }
                else
                {
                    result = false;
                }
            }
            else if (CleanWorkingDirectory)
            {
                foreach (var fse in Directory.EnumerateFileSystemEntries(WorkingDirectory))
                {
                    if (File.Exists(fse))
                    {
                        File.Delete(fse);
                    }
                    else if (Directory.Exists(fse))
                    {
                        Directory.Delete(fse);
                    }
                }
            }

            if (!Directory.Exists(NupkgStoreDirectory))
            {
                if (CreateNupkgStoreDirectoryIfNotExists)
                {
                    Directory.CreateDirectory(NupkgStoreDirectory);
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
