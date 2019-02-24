using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Kalladystine.DataModels.Generators.Models;
using System.ComponentModel;

namespace Kalladystine.DataModels.Generators.Activities
{
    public sealed class AddProperty : NativeActivity
    {
        [RequiredArgument]
        [Category("New property model")]
        public InArgument<string> PropertyName { get; set; }
        [RequiredArgument]
        [Category("New property model")]
        public InArgument<string> PropertyTypeName { get; set; }
        [Category("New property model")]
        public InArgument<string> Description { get; set; }
        [Category("New property model")]
        public InArgument<bool> RequiresInitialization { get; set; }
        [Category("New property model")]
        public InArgument<bool> MarkRequiredInBuilder { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            
            var prModel = new PropertyModel(PropertyName.Get(context), PropertyTypeName.Get(context), Description.Get(context), RequiresInitialization.Get(context), MarkRequiredInBuilder.Get(context));
            ((ClassModel)context.Properties.Find("ClassModel")).AddProperty(prModel);

        }
    }
}
