using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Kalladystine.DataModels.Generators.Models;
using System.Activities.Statements;

namespace Kalladystine.DataModels.Generators.Activities
{

    public sealed class ClassModelScope : NativeActivity
    {
        #region NativeActivityProperties
        [Browsable(false)]
        public Collection<Variable> Variables { get; }

        [Browsable(false)]
        public Activity Body { get; set; }
        #endregion

        [RequiredArgument]
        public InArgument<string> Name { get; set; }
        public InArgument<bool> WithSerializableAttribute { get; set; }
        public InArgument<bool> WithBuildActivity { get; set; }

        public OutArgument<ClassModel> ClassModel { get; set; }

        #region Constructors
        public ClassModelScope()
        {
            Variables = new Collection<Variable>();
            Body = new Sequence { DisplayName = "Body" };
        }

        #endregion


        protected override void Execute(NativeActivityContext context)
        {
            var cModel = new ClassModel(Name.Get(context), new List<PropertyModel>(), WithSerializableAttribute.Get(context), WithBuildActivity.Get(context));

            this.ClassModel.Set(context, cModel);

            context.Properties.Add("ClassModel", cModel);
            ((PackageModel)context.Properties.Find("PackageModel")).AddClassModel(cModel);

            context.ScheduleActivity(this.Body);
        }
    }
}
