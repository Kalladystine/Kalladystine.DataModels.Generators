using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Kalladystine.DataModels.Generators.Models;
using System.ComponentModel;

namespace Kalladystine.DataModels.Generators.Activities
{
    public sealed class CompileAndPackPackageModel : CodeActivity<bool>
    {
        [RequiredArgument]
        public InArgument<PackageModel> PackageModel { get; set; }

        protected override bool Execute(CodeActivityContext context)
        {
            return PackageModel.Get(context).CompileAndPack();
        }
    }
}
