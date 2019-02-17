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

    public sealed class PackageModelScope : NativeActivity
    {
        #region NativeActivityProperties
        [Browsable(false)]
        public Collection<Variable> Variables { get; }

        [Browsable(false)]
        public Activity Body { get; set; }
        #endregion

        #region PackageModelProperties
        [RequiredArgument]
        [Category("Required")]
        public InArgument<string> PackageId { get; set; }
        [RequiredArgument]
        [Category("Required")]
        public InArgument<string> Description { get; set; }
        [RequiredArgument]
        [Category("Required")]
        public InArgument<Version> Version { get; set; }
        [RequiredArgument]
        [Category("Required")]
        public InArgument<string> Authors { get; set; }
        [RequiredArgument]
        [Category("Required")]
        public InArgument<GeneratorDirectorySet> DirectorySet { get; set; }
        [RequiredArgument]
        [Category("Required")]
        public InArgument<string> ModelsAssemblyGuid { get; set; }
        [RequiredArgument]
        [Category("Required")]
        public InArgument<string> ActivitiesAssemblyGuid { get; set; }

        [Category("Additional")]
        public InArgument<string> Owners { get; set; }
        [Category("Additional")]
        public InArgument<string> LicenseUrl { get; set; }
        [Category("Additional")]
        public InArgument<string> ProjectUrl { get; set; }
        [Category("Additional")]
        public InArgument<string> IconUrl { get; set; }
        [Category("Additional")]
        public InArgument<bool> RequireLicenseAcceptance { get; set; }
        [Category("Additional")]
        public InArgument<string> ReleaseNotes { get; set; }
        [Category("Additional")]
        public InArgument<string> Copyright { get; set; }
        [Category("Additional")]
        public InArgument<string> Tags { get; set; }
        #endregion

        #region Constructors
        public PackageModelScope()
        {
            Variables = new Collection<Variable>();
            Body = new Sequence { DisplayName = "Body" };
        }

        #endregion

        public OutArgument<PackageModel> PackageModel { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            var pModel = new PackageModel();

            pModel.Id = PackageId.Get(context);
            pModel.Description = Description.Get(context);
            pModel.Version = Version.Get(context);
            pModel.Authors = Authors.Get(context);
            pModel.Owners = Owners.Get(context);
            pModel.LicenseUrl = LicenseUrl.Get(context);
            pModel.ProjectUrl = ProjectUrl.Get(context);
            pModel.IconUrl = IconUrl.Get(context);
            pModel.RequireLicenseAcceptance = RequireLicenseAcceptance.Get(context);
            pModel.ReleaseNotes = ReleaseNotes.Get(context);
            pModel.Copyright = Copyright.Get(context);
            pModel.Tags = Tags.Get(context);
            pModel.DirectorySet = DirectorySet.Get(context);
            pModel.ModelsAssemblyGuid = ModelsAssemblyGuid.Get(context);
            pModel.ActivitiesAssemblyGuid = ActivitiesAssemblyGuid.Get(context);

            this.PackageModel.Set(context, pModel);

            context.Properties.Add("PackageModel", pModel);

            context.ScheduleActivity(this.Body);
        }
    }
}
