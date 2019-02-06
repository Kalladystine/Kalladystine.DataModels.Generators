using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Kalladystine.DataModels.Generators.Models;
using System.ComponentModel;

namespace Kalladystine.DataModels.Generators.Activities
{
    public sealed class AddClassModelToPackageModel : CodeActivity
    {
        [RequiredArgument]
        public InArgument<PackageModel> PackageModel { get; set; }

        [OverloadGroup("ExistingClass")]
        [RequiredArgument]
        [Category("Add existing class")]
        public InArgument<ClassModel> ClassModel { get; set; }

        [OverloadGroup("NewClass")]
        [RequiredArgument]
        [Category("Add new class")]
        public InArgument<string> ClassName { get; set; }
        [Category("Add new class")]
        public InArgument<List<PropertyModel>> ClassProperties { get; set; }
        [Category("Add new class")]
        public InArgument<bool> ClassWithSerializableAttribute { get; set; }
        [Category("Add new class")]
        public InArgument<bool> ClassWithBuildActivity { get; set; }
        [Category("Add new class")]
        public OutArgument<ClassModel> CreatedClass { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            if (ClassModel.Get(context) != null)
            {
                PackageModel.Get(context).AddClassModel(ClassModel.Get(context));
                return;
            }

            var cModel = new ClassModel(ClassName.Get(context), ClassProperties.Get(context) ?? new List<PropertyModel>(), ClassWithSerializableAttribute.Get(context), ClassWithBuildActivity.Get(context));
            PackageModel.Get(context).AddClassModel(cModel);
            CreatedClass.Set(context, cModel);
        }
    }
}
