using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalladystine.DataModels.Generators.Models;

namespace Kalladystine.DataModels.Generators.CodeGenerators
{
    internal static class CsTranslator
    {
        internal static string WrapClasses(IEnumerable<string> csClassSources, AssemblyModel model, IEnumerable<ClassModel> classes = null)
        {
            var sb = new StringBuilder();
            AddUsings(GetCommonNamespaces(), sb);
            AddAssemblyTags(model, sb);
            AddNamespace(model.TopNamespace, sb);
            csClassSources.ToList().ForEach(s => sb.Append(s));
            if (model.AssemblyType == Enums.AssemblyTypes.Designers)
            {
                AddDesignerRegisterMetadataClass(sb, classes);
            }
            else
            {
                AddEmptyRegisterMetadataClass(sb);
            }
                

            CloseBrace(sb);

            return sb.ToString();
        }

        private static void AddDesignerRegisterMetadataClass(StringBuilder sb, IEnumerable<ClassModel> classes)
        {
            sb.AppendLine("public class DesignerMetadata : IRegisterMetadata");
            OpenBrace(sb); // open DesignerMetadata

            sb.AppendLine("public void Register()");
            OpenBrace(sb); // open Register
            sb.AppendLine("AttributeTableBuilder attributeTableBuilder = new AttributeTableBuilder();");

            foreach (var designerClass in classes.Where(c => c.Properties.Any(p => p.MarkRequiredInBuilder)))
            {
                sb.AppendLine($"attributeTableBuilder.AddCustomAttributes(typeof({designerClass.Name}), (Attribute)new DesignerAttribute(typeof(Build{designerClass.Name}Designer)));");
            }

            sb.AppendLine("MetadataStore.AddAttributeTable(attributeTableBuilder.CreateTable());");
            CloseBrace(sb); // close Register
            CloseBrace(sb); // close DesignerMetadata

        }

        internal static string GenerateCsClass(ClassModel model)
        {
            var sb = new StringBuilder();

            AddClassName(model, sb);
            AddProperties(model, sb);

            AddConstructors(model, sb);

            CloseBrace(sb);

            return sb.ToString();
        }

        internal static string GenerateActivityClass(ClassModel cModel)
        {
            var sb = new StringBuilder();
            GenerateBuilderActivity(cModel, sb);
            return sb.ToString();
        }

        private static void AddAssemblyTags(AssemblyModel model, StringBuilder sb)
        {
            sb.AppendLine("[assembly: AssemblyTitle(\"" + model.AssemblyTitle + "\")]");
            sb.AppendLine("[assembly: AssemblyDescription(\"" + model.AssemblyDescription + "\")]");
            sb.AppendLine("[assembly: AssemblyCopyright(\"" + model.AssemblyCopyright + "\")]");
            sb.AppendLine("[assembly: ComVisible(false)]");
            sb.AppendLine("[assembly: Guid(\"" + model.AssemblyGuid + "\")]");
            sb.AppendLine("[assembly: AssemblyVersion(\"" + model.AssemblyVersion.ToString(4) + "\")]");
            sb.AppendLine("[assembly: TargetFramework(\".NETFramework, Version = v4.5.2\", FrameworkDisplayName = \".NET Framework 4.5.2\")]");
            sb.AppendLine();
        }

        private static void AddNamespace(string @namespace, StringBuilder sb)
        {
            sb.AppendLine($"namespace {@namespace}");
            OpenBrace(sb);
        }

        private static void AddConstructors(ClassModel model, StringBuilder sb)
        {
            // default parameterless constructor
            AddConstructor(sb, model.Name, model.Properties.Where(p => p.RequiresInitialization), true);

            // with parameters requiring initialization or required in builder, if any
            if (model.Properties.Any(p => p.RequiresInitialization || p.MarkRequiredInBuilder))
            {
                AddConstructor(sb, model.Name, model.Properties.Where(p => p.RequiresInitialization || p.MarkRequiredInBuilder));
            }
            // full constructor with all props only if not all are required
            if (!model.Properties.All(p => p.RequiresInitialization || p.MarkRequiredInBuilder))
            {
                AddConstructor(sb, model.Name, model.Properties);
            }
        }

        private static void AddConstructor(StringBuilder sb, string className, IEnumerable<PropertyModel> properties, bool parameterless = false)
        {
            sb.AppendLine();
            sb.Append($"public {className}(");
            if (!parameterless)
            {
                sb.Append(String.Join(", ", properties.Select(p => p.TypeName + " " + p.Name)));
            }
            sb.AppendLine(")");
            OpenBrace(sb);
            if (parameterless)
            {
                foreach (var mProp in properties)
                {
                    sb.AppendLine($"this.{mProp.Name} = new {mProp.TypeName}();");
                }
            }
            else
            {
                foreach (var mProp in properties)
                {
                    sb.AppendLine($"this.{mProp.Name} = {mProp.Name};");
                }
            }
            
            CloseBrace(sb);
        }

        private static void AddProperties(ClassModel model, StringBuilder sb)
        {
            foreach (var property in model.Properties)
            {
                sb.AppendLine($"public {property.TypeName} {property.Name} {{ get; set; }}");
            }
        }

        private static void AddClassName(ClassModel model, StringBuilder sb)
        {
            if (model.WithSerializableAttribute)
            {
                sb.AppendLine("[Serializable]");
            }
            sb.AppendLine($"public class {model.Name}");
            OpenBrace(sb);
        }

        private static void AddUsings(IEnumerable<string> usingStatements, StringBuilder sb)
        {
            foreach (var usingStatement in usingStatements)
            {
                sb.AppendLine($"using {usingStatement};");
            }
            sb.AppendLine();
        }

        private static void OpenBrace(StringBuilder sb)
        {
            sb.AppendLine("{");
        }

        private static void CloseBrace(StringBuilder sb)
        {
            sb.AppendLine("}");
            sb.AppendLine();
        }

        private static void GenerateBuilderActivity(ClassModel model, StringBuilder sb)
        {
            sb.AppendLine();
            sb.AppendLine($"public sealed class Build{model.Name} : CodeActivity<{model.Name}>");
            OpenBrace(sb); // open class
            foreach (var property in model.Properties)
            {
                if (property.MarkRequiredInBuilder)
                {
                    sb.AppendLine("[RequiredArgument]");
                    sb.AppendLine("[Category(\"Required Fields\")]");
                }
                else
                {
                    sb.AppendLine("[DefaultValue(null)]");
                    sb.AppendLine("[Category(\"Optional Fields\")]");
                }
                
                sb.AppendLine($"public InArgument<{property.TypeName}> {property.Name} {{ get; set; }}");
            }
            sb.AppendLine();

            sb.AppendLine($"protected override {model.Name} Execute(CodeActivityContext context)");
            OpenBrace(sb); // open Execute
            sb.Append($"return new {model.Name}(");
            sb.Append(String.Join(", ", model.Properties.Select(p => p.Name + ".Get(context)")));
            sb.AppendLine(");");
            CloseBrace(sb); // close Execute

            // add runtime metadata for argument binding in designer
            //sb.AppendLine("protected override void CacheMetadata(CodeActivityMetadata metadata)");
            //OpenBrace(sb); // open cachemetadata

            //foreach (var requiredProp in model.Properties.Where(p => p.MarkRequiredInBuilder))
            //{
            //    sb.AppendLine("metadata.AddArgument(new RuntimeArgument(");
            //    sb.AppendLine($"\"{requiredProp.Name}\", typeof({requiredProp.TypeName}), ArgumentDirection.In));");
            //}
            
            //CloseBrace(sb); // close cachemetadata
            CloseBrace(sb); // close class
        }

        internal static string GenerateBuilderActivityDesigner(ClassModel cModel, string topNamespace)
        {
            var sb = new StringBuilder();
            GenerateBuilderActivityXaml(cModel, topNamespace, sb);
            GenerateBuilderActivityXamlCs(cModel, sb);
            return sb.ToString();
        }


        private static void GenerateBuilderActivityXaml(ClassModel cModel, string topNamespace, StringBuilder sb)
        {
            // Build{cModel.Name}Designer
            sb.AppendLine($"<sap:ActivityDesigner x:Class=\"{topNamespace}.Build{cModel.Name}Designer\"");
            sb.AppendLine("xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
            sb.AppendLine("xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
            sb.AppendLine("xmlns:sap=\"clr-namespace:System.Activities.Presentation;assembly=System.Activities.Presentation\"");
            sb.AppendLine("xmlns:sapc=\"clr-namespace:System.Activities.Presentation.Converters;assembly=System.Activities.Presentation\"");
            sb.AppendLine("xmlns:sapv=\"clr-namespace:System.Activities.Presentation.View;assembly=System.Activities.Presentation\"");
            sb.AppendLine("Collapsible=\"True\">");
            sb.AppendLine("<sap:ActivityDesigner.Resources>");
            sb.AppendLine("<ResourceDictionary>");
            sb.AppendLine("<sapc:ArgumentToExpressionConverter x:Key=\"ArgumentToExpressionConverter\" />");
            sb.AppendLine("</ResourceDictionary>");
            sb.AppendLine("</sap:ActivityDesigner.Resources>");

            GeneratePopertyXamlGrid(cModel, sb);

            sb.AppendLine("</sap:ActivityDesigner>");
        }

        private static void GeneratePopertyXamlGrid(ClassModel cModel, StringBuilder sb)
        {
            var requiredProps = cModel.Properties.Where(p => p.MarkRequiredInBuilder).ToArray();

            sb.AppendLine("<Grid>");

            sb.AppendLine("<Grid.ColumnDefinitions>");
            sb.AppendLine("<ColumnDefinition Width=\"Auto\"/>");
            sb.AppendLine("<ColumnDefinition Width=\"Auto\"/>");
            sb.AppendLine("</Grid.ColumnDefinitions>");

            sb.AppendLine("<Grid.RowDefinitions>");
            foreach (var requiredProp in requiredProps)
            {
                sb.AppendLine("<RowDefinition Height=\"Auto\"/>");
            }
            sb.AppendLine("</Grid.RowDefinitions>");

            // add field labels
            for (int i = 0; i < requiredProps.Length; i++)
            {
                sb.AppendLine($"<Label Grid.Row=\"{i}\" Grid.Column=\"0\" Content=\"{requiredProps[i].Name}\" HorizontalAlignment=\"Center\"/>");
            }

            // add field expression boxes
            for (int i = 0; i < requiredProps.Length; i++)
            {
                sb.AppendLine("<sapv:ExpressionTextBox");
                sb.AppendLine($"Grid.Row=\"{i}\" Grid.Column=\"1\"");
                sb.AppendLine("OwnerActivity=\"{Binding Path=ModelItem}\"");
                sb.AppendLine($"Expression=\"{{Binding Path=ModelItem.{requiredProps[i].Name}, Mode=TwoWay, Converter={{StaticResource ArgumentToExpressionConverter}}, \"ConverterParameter = In }}\"");
                sb.AppendLine($"HintText=\"{requiredProps[i].TypeName} Expression\" />");
            }

            sb.AppendLine("</Grid>");
        }

        private static void GenerateBuilderActivityXamlCs(ClassModel cModel, StringBuilder sb)
        {
            sb.AppendLine($"public partial class Build{cModel.Name}Designer");
            OpenBrace(sb);
            sb.AppendLine($"public Build{cModel.Name}Designer() {{ InitializeComponent(); }}");
            CloseBrace(sb);
        }

        private static void AddEmptyRegisterMetadataClass(StringBuilder sb)
        {
            sb.AppendLine();
            sb.AppendLine("public class RegisterMetadata : IRegisterMetadata");
            OpenBrace(sb);
            sb.AppendLine("public void Register() {}");
            CloseBrace(sb);
            sb.AppendLine();
        }

        private static string[] GetCommonNamespaces()
        {
            return new[] { "System", "System.Collections.Generic", "System.Activities", "System.Activities.Presentation.Metadata", "System.ComponentModel",
                "System.Diagnostics",
                "System.Reflection",
                "System.Runtime.CompilerServices",
                "System.Runtime.InteropServices",
                "System.Runtime.Versioning",
                "System.Windows.Markup"};
        }
    }
}
