using System.CodeDom.Compiler;
using System.IO;

namespace Kalladystine.DataModels.Generators.Compilers
{
    internal static class CsCodeDomCompiler
    {
        private static CodeDomProvider csCodeProvider;

        static CsCodeDomCompiler()
        {
            csCodeProvider = CodeDomProvider.CreateProvider("CSharp");
        }

        internal static bool TryCompileCsToAssembly(string assemblyOutputFullPath, string csCodeSource, string[] additionalAssemblies, out CompilerErrorCollection compilerErrors)
        {
            if (Path.GetExtension(assemblyOutputFullPath).ToLowerInvariant() != ".dll")
            {
                assemblyOutputFullPath += ".dll";
            }
            var parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = assemblyOutputFullPath;

            parameters.ReferencedAssemblies.AddRange(GetCommonAssemblies());
            if (additionalAssemblies != null)
            {
                foreach (var addAsmbl in additionalAssemblies)
                {
                    parameters.ReferencedAssemblies.Add(addAsmbl);
                }
            }

            if (assemblyOutputFullPath.EndsWith("Design.dll"))
            {
                System.Console.WriteLine(csCodeSource);
                parameters.EmbeddedResources.Add(@"D:\Tmp\manual\streetaddress.baml");
                compilerErrors = csCodeProvider.CompileAssemblyFromFile(parameters, @"D:\Tmp\manual\designer.txt").Errors;
                return compilerErrors.Count == 0;
            }

            return TryCompile(parameters, csCodeSource, out compilerErrors);
        }

        internal static bool TryCompileCsToAssembly(string assemblyOutputFullPath, string csCodeSource, out CompilerErrorCollection compilerErrors)
        {
            System.Console.WriteLine(csCodeSource);
            return TryCompileCsToAssembly(assemblyOutputFullPath, csCodeSource, null, out compilerErrors);
        }

        private static bool TryCompile(CompilerParameters parameters, string csCodeSource, out CompilerErrorCollection compilerErrors)
        {
            System.Console.WriteLine(csCodeSource);
            compilerErrors = csCodeProvider.CompileAssemblyFromSource(parameters, csCodeSource).Errors;
            return compilerErrors.Count == 0;
        }

        private static string[] GetCommonAssemblies()
        {
            return new[]
            {
                "Accessibility.dll",
                "Microsoft.CSharp.dll",
                "System.Activities.dll",
                "System.Activities.Presentation.dll",
                "System.ComponentModel.dll",
                "System.Configuration.dll",
                "System.Configuration.Install.dll",
                "System.Core.dll",
                "System.Data.dll",
                "System.Data.DataSetExtensions.dll",
                "System.Data.Linq.dll",
                "System.Data.OracleClient.dll",
                "System.Deployment.dll",
                "System.Design.dll",
                "System.DirectoryServices.dll",
                "System.dll",
                "System.Drawing.Design.dll",
                "System.Drawing.dll",
                "System.EnterpriseServices.dll",
                "System.Management.dll",
                "System.Messaging.dll",
                "System.Runtime.Remoting.dll",
                "System.Runtime.Serialization.dll",
                "System.Runtime.Serialization.Formatters.Soap.dll",
                "System.Security.dll",
                "System.ServiceModel.dll",
                "System.ServiceModel.Web.dll",
                "System.ServiceProcess.dll",
                "System.Transactions.dll",
                "System.Web.dll",
                "System.Web.Extensions.Design.dll",
                "System.Web.Extensions.dll",
                "System.Web.Mobile.dll",
                "System.Web.RegularExpressions.dll",
                "System.Web.Services.dll",
                "System.Windows.Forms.Dll",
                "System.Workflow.Activities.dll",
                "System.Workflow.ComponentModel.dll",
                "System.Workflow.Runtime.dll",
                "System.Xaml.dll",
                "System.Xml.dll",
                @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\WindowsBase.dll",
                @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\PresentationCore.dll",
                @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\PresentationFramework.dll",
                "System.Xml.Linq.dll"
            };
        }

    }
}
