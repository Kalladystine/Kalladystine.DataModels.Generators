using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Kalladystine.DataModels.Generators.Models;

namespace Kalladystine.DataModels.Generators.Activities
{
    public sealed class CreateClassModel : CodeActivity<ClassModel>
    {
        [RequiredArgument]
        public InArgument<string> Name { get; set; }
        public InArgument<List<PropertyModel>> Properties { get; set; }
        public InArgument<bool> WithSerializableAttribute { get; set; }
        public InArgument<bool> WithBuildActivity { get; set; }

        protected override ClassModel Execute(CodeActivityContext context)
        {
            return new ClassModel(Name.Get(context), Properties.Get(context), WithSerializableAttribute.Get(context), WithBuildActivity.Get(context));
        }
    }
}
