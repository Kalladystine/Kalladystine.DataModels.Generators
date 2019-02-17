using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Kalladystine.DataModels.Generators.Models;
using System.ComponentModel;

namespace Kalladystine.DataModels.Generators.Activities
{

    public sealed class CreateGeneratorDirectorySet : CodeActivity<GeneratorDirectorySet>
    {
        [RequiredArgument]
        [Category("Paths")]
        public InArgument<string> WorkingDirectory { get; set; }
        [RequiredArgument]
        [Category("Paths")]
        public InArgument<string> NupkgStoreDirectory { get; set; }
        [Category("Settings")]
        public InArgument<bool> CreateWorkingDirectoryIfNotExists { get; set; }
        [Category("Settings")]
        public InArgument<bool> CreateNupkgStoreDirectoryIfNotExists { get; set; }
        [Category("Settings")]
        public InArgument<bool> CleanWorkingDirectory { get; set; }
        [Category("Settings")]
        public InArgument<bool> EnsureDirectoriesAreValid { get; set; }

        public OutArgument<bool> DirectoriesAreValid { get; set; }

        protected override GeneratorDirectorySet Execute(CodeActivityContext context)
        {
            var dirSet = new GeneratorDirectorySet();
            dirSet.WorkingDirectory = WorkingDirectory.Get(context);
            dirSet.NupkgStoreDirectory = NupkgStoreDirectory.Get(context);
            dirSet.CreateWorkingDirectoryIfNotExists = CreateWorkingDirectoryIfNotExists.Get(context);
            dirSet.CreateNupkgStoreDirectoryIfNotExists = CreateNupkgStoreDirectoryIfNotExists.Get(context);
            dirSet.CleanWorkingDirectory = CleanWorkingDirectory.Get(context);

            if (EnsureDirectoriesAreValid.Get(context))
            {
                DirectoriesAreValid.Set(context, dirSet.EnsureDirectories());
            }

            return dirSet;
        }
    }
}
