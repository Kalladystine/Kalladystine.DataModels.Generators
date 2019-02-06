using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kalladystine.DataModels.Generators.Models
{
    internal class AssemblyModel
    {
        internal string AssemblyTitle;
        internal string AssemblyDescription;
        internal string AssemblyCopyright;
        internal Version AssemblyVersion;
        internal string AssemblyGuid;
        internal Enums.AssemblyTypes AssemblyType;

        internal GeneratorDirectorySet DirectorySet;

        internal string TopNamespace
        {
            get
            {
                return AssemblyTitle;
            }
        }

        internal string AssemblySourceCode;

        internal string AssemblyOutputFullPath
        {
            get
            {
                return Path.Combine(DirectorySet.WorkingDirectory, AssemblyTitle + ".dll");
            }
        }

        internal void GenerateSourceCode(List<ClassModel> classes)
        {
            switch (AssemblyType)
            {
                case Enums.AssemblyTypes.Models:
                    GenerateModelsSourceCode(classes);
                    break;
                case Enums.AssemblyTypes.Activities:
                    GenerateActivitiesSourceCode(classes);
                    break;
                case Enums.AssemblyTypes.Designers:
                    GenerateActivityDesignersSourceCode(classes);
                    break;
                default:
                    throw new InvalidOperationException("No valid generate source code procedure for assembly type " + AssemblyType.ToString());
            }
        }

        internal bool TryCompile()
        {
            return TryCompile(null);
        }

        internal bool TryCompile(string[] additionalAssemblies)
        {
            System.CodeDom.Compiler.CompilerErrorCollection compilerErrors;
            return Compilers.CsCodeDomCompiler.TryCompileCsToAssembly(AssemblyOutputFullPath, AssemblySourceCode, additionalAssemblies, out compilerErrors);
        }

        private void GenerateModelsSourceCode(List<ClassModel> classes)
        {
            var sources = new List<string>(classes.Count);
            foreach (var cModel in classes)
            {
                sources.Add(CodeGenerators.CsTranslator.GenerateCsClass(cModel));
            }
            AssemblySourceCode = CodeGenerators.CsTranslator.WrapClasses(sources, this);
        }

        private void GenerateActivitiesSourceCode(List<ClassModel> classes)
        {
            var modelsWithBuilders = classes.Where(x => x.WithBuildActivity);
            var sources = new List<string>(modelsWithBuilders.Count());
            foreach (var cModel in modelsWithBuilders)
            {
                sources.Add(CodeGenerators.CsTranslator.GenerateActivityClass(cModel));
            }
            AssemblySourceCode = CodeGenerators.CsTranslator.WrapClasses(sources, this);
        }

        private void GenerateActivityDesignersSourceCode(List<ClassModel> classes)
        {
            var modelsWithBuilders = classes.Where(x => x.WithBuildActivity);
            var sources = new List<string>(modelsWithBuilders.Count());
            foreach (var cModel in modelsWithBuilders)
            {
                sources.Add(CodeGenerators.CsTranslator.GenerateBuilderActivityDesigner(cModel, TopNamespace));
            }
            AssemblySourceCode = CodeGenerators.CsTranslator.WrapClasses(sources, this, classes);
            Console.WriteLine(AssemblySourceCode);
        }

        internal static AssemblyModel FromPackageModel(PackageModel packageModel, Enums.AssemblyTypes assemblyType, string guid)
        {
            var asmblModel = new AssemblyModel();
            asmblModel.AssemblyTitle = packageModel.Id;
            asmblModel.AssemblyDescription = packageModel.Description;
            asmblModel.AssemblyCopyright = packageModel.Copyright;
            asmblModel.AssemblyVersion = packageModel.Version;
            asmblModel.DirectorySet = packageModel.DirectorySet;
            asmblModel.AssemblyType = assemblyType;
            asmblModel.AssemblyGuid = guid ?? Guid.NewGuid().ToString();
            return asmblModel;
        }

    }
}
