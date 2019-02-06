using System;
using System.IO;
using NuGet;

namespace Kalladystine.DataModels.Generators.Packagers
{
    internal static class NugetPackager
    {
        internal static bool Pack(Models.PackageModel model)
        {
            ManifestMetadata metadata = new ManifestMetadata()
            {
                Id = model.Id,
                Description = model.Description,
                Version = model.Version.ToString(4),
                Authors = model.Authors,
                Owners = model.Owners,
                LicenseUrl = model.LicenseUrl,
                ProjectUrl = model.ProjectUrl,
                IconUrl = model.IconUrl,
                RequireLicenseAcceptance = model.RequireLicenseAcceptance,
                ReleaseNotes = model.ReleaseNotes,
                Copyright = model.Copyright,
                Tags = model.Tags
            };

            PackageBuilder builder = new PackageBuilder();
            builder.PopulateFiles(model.DirectorySet.WorkingDirectory, new[] { new ManifestFile() { Source = "**", Target = "lib/net452", Exclude = "**.txt;**.nuspec" } });
            builder.Populate(metadata);
            using (FileStream stream = File.Open(model.NupgkOutputFullPath, FileMode.OpenOrCreate))
            {
                builder.Save(stream);
            }
            return File.Exists(model.NupgkOutputFullPath);
        }
    }
}
