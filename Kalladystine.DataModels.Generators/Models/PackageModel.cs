using System;
using System.Collections.Generic;
using System.IO;
using Kalladystine.DataModels.Generators.Enums;

namespace Kalladystine.DataModels.Generators.Models
{
    [Serializable]
    public class PackageModel
    {
        public string Id;
        public string Description;
        public Version Version;
        public string Authors;
        public string Owners;
        public string LicenseUrl;
        public string ProjectUrl;
        public string IconUrl;
        public bool RequireLicenseAcceptance;
        public string ReleaseNotes;
        public string Copyright;
        public string Tags;

        public string ModelsAssemblyGuid;
        public string ActivitiesAssemblyGuid;
        public string ActivitiesDesignAssemblyGuid;

        public GeneratorDirectorySet DirectorySet;
        public List<ClassModel> Classes;
        private Dictionary<string, AssemblyModel> Assemblies;

        public string NupgkOutputFullPath
        {
            get
            {
                return Path.Combine(DirectorySet.NupkgStoreDirectory, Id + "." + Version.ToString(4) + ".nupkg");
            }
        }

        public PackageModel()
        {
            Assemblies = new Dictionary<string, AssemblyModel>();
            DirectorySet = new GeneratorDirectorySet();
            Classes = new List<ClassModel>();
        }


        public void AddClassModel(ClassModel model)
        {
            Classes.Add(model);
        }

        public bool CompileAndPack()
        {
            GenerateAndAddDefaultAssemblies();
            GenerateSourceCodes();
            
            if (CompileAssemblies())
            {
                return CreateNupkg();
            }

            return false;
        }

        private void GenerateAndAddDefaultAssemblies()
        {
            var modelsAssembly = AssemblyModel.FromPackageModel(this, AssemblyTypes.Models, ModelsAssemblyGuid);
            Assemblies.Add(AssemblyTypes.Models.ToString(), modelsAssembly);

            var activitiesAssembly = AssemblyModel.FromPackageModel(this, AssemblyTypes.Activities, ActivitiesAssemblyGuid);
            activitiesAssembly.AssemblyTitle += ".Activities";
            Assemblies.Add(AssemblyTypes.Activities.ToString(), activitiesAssembly);

            //var designersAssembly = AssemblyModel.FromPackageModel(this, AssemblyTypes.Designers, ActivitiesDesignAssemblyGuid);
            //designersAssembly.AssemblyTitle += ".Activities.Design";
            //Assemblies.Add(AssemblyTypes.Designers.ToString(), designersAssembly);
        }

        private void GenerateSourceCodes()
        {
            foreach (var asmbl in Assemblies.Values)
            {
                asmbl.GenerateSourceCode(Classes);
            }
        }

        private bool CompileAssemblies()
        {
            bool activitiesOk = false;
            bool modelsOk = Assemblies[AssemblyTypes.Models.ToString()].TryCompile();
            if (modelsOk)
            {
                activitiesOk = Assemblies[AssemblyTypes.Activities.ToString()].TryCompile(
                    new[] { Assemblies[AssemblyTypes.Models.ToString()].AssemblyOutputFullPath }
                    );
            }
            return activitiesOk;

            //if (activitiesOk)
            //{
            //    return Assemblies[AssemblyTypes.Designers.ToString()].TryCompile(
            //        new[] {
            //            Assemblies[AssemblyTypes.Models.ToString()].AssemblyOutputFullPath,
            //            Assemblies[AssemblyTypes.Activities.ToString()].AssemblyOutputFullPath,
            //        }
            //        );
            //}
            //return false;
        }

        private bool CreateNupkg()
        {
            return Packagers.NugetPackager.Pack(this);
        }

        public static Version CreateFullVersionFromMajorMinor(int majorVersion, int minorVersion)
        {
            var cachedNow = DateTime.Now;
            int buildNumber = (cachedNow.Date - new DateTime(2000, 1, 1)).Days;
            int revisionNumber = (int)(cachedNow - cachedNow.Date).TotalSeconds / 2;
            return new Version(majorVersion, minorVersion, buildNumber, revisionNumber);
        }
    }


}
