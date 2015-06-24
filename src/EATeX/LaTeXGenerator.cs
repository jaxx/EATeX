using System.Linq;
using EA;

namespace EATeX
{
    public class LaTeXGenerator
    {
        private readonly Package rootPackage;
        private EATeXConfig configuration;

        public LaTeXGenerator(Package package, EATeXConfig configuration)
        {
            rootPackage = package;
            this.configuration = configuration;
        }

        public void Generate()
        {
            GeneratePackageTex(rootPackage);
        }

        private void GeneratePackageTex(Package package)
        {
            var packageName = package.Name;
            var packageCreated = package.Created;
            var packageModified = package.Modified;
            var packageNotes = package.Notes;

            foreach (var requirement in package.Element.Requirements.Cast<Requirement>())
                GenerateRequirementTex(requirement);

            foreach (var childPackage in package.Packages.Cast<Package>())
                GeneratePackageTex(childPackage);
        }

        private void GenerateRequirementTex(Requirement requirement)
        {
            var reqName = requirement.Name;
            var reqType = requirement.Type;
            var reqStatus = requirement.Status;
            var reqLastUpdate = requirement.LastUpdate;
            var reqNotes = requirement.Notes;
        }
    }
}