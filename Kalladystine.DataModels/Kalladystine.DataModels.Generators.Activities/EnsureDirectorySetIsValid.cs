using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Kalladystine.DataModels.Generators.Models;

namespace Kalladystine.DataModels.Generators.Activities
{
    public sealed class EnsureDirectorySetIsValid : CodeActivity<bool>
    {
        [RequiredArgument]
        public InArgument<GeneratorDirectorySet> DirectorySet { get; set; }

        protected override bool Execute(CodeActivityContext context)
        {
            return DirectorySet.Get(context).EnsureDirectories();
        }
    }
}
