using System;
using System.Collections.Generic;
using System.Linq;

namespace Kalladystine.DataModels.Generators.Models
{
    [Serializable]
    public class ClassModel
    {
        public string Name;
        public List<PropertyModel> Properties;
        public bool WithSerializableAttribute;
        public bool WithBuildActivity;

        public ClassModel()
        {
            this.Properties = new List<PropertyModel>();
        }

        public ClassModel(string name, IEnumerable<PropertyModel> properties, bool serializable = true, bool buildActivity = true)
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException("Class name cannot be null, empty or whitespace only.", nameof(name))
                : name;
            Properties = !properties.Any()
                ? new List<PropertyModel>()
                : properties.ToList();
            WithSerializableAttribute = serializable;
            WithBuildActivity = buildActivity;
        }

        public void AddProperty(PropertyModel property)
        {
            Properties.Add(property);
        }

        public void AddProperties(IEnumerable<PropertyModel> properties)
        {
            Properties.AddRange(properties);
        }
    }
}
