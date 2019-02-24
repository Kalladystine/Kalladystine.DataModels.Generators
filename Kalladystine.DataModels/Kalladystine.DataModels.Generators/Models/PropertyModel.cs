using System;

namespace Kalladystine.DataModels.Generators.Models
{
    [Serializable]
    public class PropertyModel
    {
        public string Name;
        public string TypeName;
        public string Description;
        public bool RequiresInitialization;
        public bool MarkRequiredInBuilder;

        public PropertyModel()
        {
        }

        public PropertyModel(string name, string typeName, string description = "", bool requiresInitialization = false, bool markRequired = false)
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException("Property name cannot be null, empty or whitespace only.", nameof(name))
                : name;
            TypeName = string.IsNullOrWhiteSpace(typeName)
                ? throw new ArgumentException("TypeName name cannot be null, empty or whitespace only.", nameof(typeName))
                : typeName;
            Description = description;
            RequiresInitialization = requiresInitialization;
            MarkRequiredInBuilder = markRequired;
        }

        public PropertyModel(string name, Type type, string description = "", bool requiresInitialization = false, bool markRequired = false)
            : this(name, type?.FullName, description, requiresInitialization, markRequired)
        {
        }
    }
}
