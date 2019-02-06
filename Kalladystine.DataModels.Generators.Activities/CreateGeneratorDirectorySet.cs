using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Kalladystine.DataModels.Generators.Models;

namespace Kalladystine.DataModels.Generators.Activities
{

    public sealed class CreateGeneratorDirectorySet : CodeActivity<GeneratorDirectorySet>
    {
        [RequiredArgument]
        public InArgument<string> WorkingDirectory { get; set; }
        [RequiredArgument]
        public InArgument<string> NupkgStoreDirectory { get; set; }
        public InArgument<bool> CreateWorkingDirectoryIfNotExists { get; set; }
        public InArgument<bool> CreateNupkgStoreDirectoryIfNotExists { get; set; }
        public InArgument<bool> CleanWorkingDirectory { get; set; }

        protected override GeneratorDirectorySet Execute(CodeActivityContext context)
        {
            var dirSet = new GeneratorDirectorySet();
            dirSet.WorkingDirectory = WorkingDirectory.Get(context);
            dirSet.NupkgStoreDirectory = NupkgStoreDirectory.Get(context);
            dirSet.CreateWorkingDirectoryIfNotExists = CreateWorkingDirectoryIfNotExists.Get(context);
            dirSet.CreateNupkgStoreDirectoryIfNotExists = CreateNupkgStoreDirectoryIfNotExists.Get(context);
            dirSet.CleanWorkingDirectory = CleanWorkingDirectory.Get(context);
            return dirSet;
        }
    }
}
