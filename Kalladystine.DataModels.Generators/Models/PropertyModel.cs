using System;

namespace Kalladystine.DataModels.Generators.Models
{
    [Serializable]
    public class PropertyModel
    {
        public string Name;
        public string TypeName;
        public bool RequiresInitialization;
        public bool MarkRequiredInBuilder;

        public PropertyModel()
        {
        }

        public PropertyModel(string name, string typeName, bool requiresInitialization = false, bool markRequired = false)
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException("Property name cannot be null, empty or whitespace only.", nameof(name))
                : name;
            TypeName = string.IsNullOrWhiteSpace(typeName)
                ? throw new ArgumentException("TypeName name cannot be null, empty or whitespace only.", nameof(typeName))
                : typeName;
            RequiresInitialization = requiresInitialization;
            MarkRequiredInBuilder = markRequired;
        }

        public PropertyModel(string name, Type type, bool requiresInitialization = false, bool markRequired = false)
            : this(name, type?.FullName, requiresInitialization, markRequired)
        {
        }
    }
}
