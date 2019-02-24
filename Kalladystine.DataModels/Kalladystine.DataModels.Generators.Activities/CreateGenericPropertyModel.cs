using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Kalladystine.DataModels.Generators.Models;

namespace Kalladystine.DataModels.Generators.Activities
{
    public sealed class CreateGenericPropertyModel<T> : CodeActivity<PropertyModel>
    {
        [RequiredArgument]
        public InArgument<string> Name { get; set; }
        public InArgument<string> Description { get; set; }
        public InArgument<bool> RequiresInitialization { get; set; }
        public InArgument<bool> MarkRequiredInBuilder { get; set; }

        protected override PropertyModel Execute(CodeActivityContext context)
        {
            return new PropertyModel(Name.Get(context), typeof(T), Description.Get(context), RequiresInitialization.Get(context), MarkRequiredInBuilder.Get(context));
        }
    }
}
