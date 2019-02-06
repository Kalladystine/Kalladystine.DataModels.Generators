using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Kalladystine.DataModels.Generators.Models;
using System.ComponentModel;

namespace Kalladystine.DataModels.Generators.Activities
{
    public sealed class AddGenericPropertyModelToClassModel<T> : CodeActivity
    {
        [RequiredArgument]
        public InArgument<ClassModel> ClassModel { get; set; }

        [OverloadGroup("ExistingPropertyModel")]
        [RequiredArgument]
        [Category("Existing property model")]
        public InArgument<PropertyModel> PropertyModel { get; set; }

        [OverloadGroup("NewPropertyModel")]
        [RequiredArgument]
        [Category("New property model")]
        public InArgument<string> PropertyName { get; set; }

        [Category("New property model")]
        public InArgument<bool> RequiresInitialization { get; set; }

        [Category("New property model")]
        public InArgument<bool> MarkRequiredInBuilder { get; set; }

        [Category("New property model")]
        public OutArgument<PropertyModel> CreatedPropertyModel { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            if (PropertyModel.Get(context) != null)
            {
                ClassModel.Get(context).AddProperty(PropertyModel.Get(context));
                return;
            }

            var pModel = new PropertyModel(PropertyName.Get(context), typeof(T), RequiresInitialization.Get(context), MarkRequiredInBuilder.Get(context));
            ClassModel.Get(context).AddProperty(pModel);
            CreatedPropertyModel.Set(context, pModel);
        }
    }
}
