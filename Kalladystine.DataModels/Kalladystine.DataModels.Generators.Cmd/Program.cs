using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Kalladystine.DataModels.Generators.Models;

namespace Kalladystine.DataModels.Generators.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var dirSet = new GeneratorDirectorySet();
            dirSet.WorkingDirectory = @"D:\Tmp\Sources\Test";
            dirSet.CreateWorkingDirectoryIfNotExists = true;
            dirSet.NupkgStoreDirectory = @"D:\Tmp\Packages";
            dirSet.CreateNupkgStoreDirectoryIfNotExists = false;
            if (!dirSet.EnsureDirectories())
            {
                throw new IOException("Some of the directories are not present :(");
            }
            var packageModel = new PackageModel();
            packageModel.DirectorySet = dirSet;
            packageModel.Id = "TestModels.AddressBook";
            packageModel.Description = "Test model activity pack with Address Book examples";
            packageModel.Version = PackageModel.CreateFullVersionFromMajorMinor(1, 0);
            packageModel.Authors = "Kalladystine";

            var streetAddressProperties = new List<PropertyModel>
            {
                new PropertyModel("City", typeof(string), "Name of the city", false, true),
                new PropertyModel("Street", typeof(string)),
                new PropertyModel("PostCode", typeof(string), "In standard format", false, true),
                new PropertyModel("HouseNumber", typeof(string)),
                new PropertyModel("ApartmentNumber", typeof(string))
            };
            var streeAddressModel = new ClassModel("StreetAddress", streetAddressProperties);
            packageModel.AddClassModel(streeAddressModel);

            var customerProperties = new List<PropertyModel>
            {
                new PropertyModel("Name", typeof(string)),
                new PropertyModel("Surname", typeof(string), "Full one", false, true),
                new PropertyModel("Addresses", "List<StreetAddress>", "All of them", true)
            };
            var customerModel = new ClassModel("Customer", customerProperties);
            packageModel.AddClassModel(customerModel);

            Console.WriteLine("Packaging... Result: " + packageModel.CompileAndPack());

            Console.WriteLine(JsonConvert.SerializeObject(packageModel, Formatting.Indented));
            Console.ReadKey();
        }
    }
}
