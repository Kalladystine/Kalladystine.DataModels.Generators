using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using Kalladystine.DataModels.Generators.Activities;
using Kalladystine.DataModels.Generators.Activities.Design;

namespace Kalladystine.Statements.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();

            var dataModelsCategory = new CategoryAttribute("Kalladystine.DataModels");

            builder.AddCustomAttributes(typeof(PackageModelScope), dataModelsCategory);
            builder.AddCustomAttributes(typeof(ClassModelScope), dataModelsCategory);
            builder.AddCustomAttributes(typeof(AddProperty), dataModelsCategory);
            builder.AddCustomAttributes(typeof(AddGenericProperty<>), dataModelsCategory);
            builder.AddCustomAttributes(typeof(CreateGeneratorDirectorySet), dataModelsCategory);
            builder.AddCustomAttributes(typeof(CompileAndPackPackageModel), dataModelsCategory);

            builder.AddCustomAttributes(typeof(PackageModelScope), new DesignerAttribute(typeof(PackageModelScopeDesigner)));
            builder.AddCustomAttributes(typeof(ClassModelScope), new DesignerAttribute(typeof(ClassModelScopeDesigner)));
            builder.AddCustomAttributes(typeof(AddGenericProperty<>), new DesignerAttribute(typeof(AddGenericPropertyDesigner)));
            builder.AddCustomAttributes(typeof(AddProperty), new DesignerAttribute(typeof(AddPropertyDesigner)));
            builder.AddCustomAttributes(typeof(CreateGeneratorDirectorySet), new DesignerAttribute(typeof(CreateGeneratorDirectorySetDesigner)));
            builder.AddCustomAttributes(typeof(CompileAndPackPackageModel), new DesignerAttribute(typeof(CompileAndPackPackageModelDesigner)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
