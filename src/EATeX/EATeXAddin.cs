using EA;

namespace EATeX
{
    public class EATeXAddin : EAAddinBase
    {
        public override object EA_GetMenuItems(Repository Repository, string MenuLocation, string MenuName)
        {
            switch (MenuName)
            {
                case "":
                    return AddinMenu.Name;
                case AddinMenu.Name:
                    return new[]
                    {
                        AddinMenu.SubItems.GeneratePdf,
                        AddinMenu.MenuSeparator,
                        AddinMenu.SubItems.EditTemplate,
                        AddinMenu.SubItems.Settings,
                        AddinMenu.MenuSeparator,
                        AddinMenu.SubItems.About
                    };
            }

            return "";
        }
    }

    public static class AddinMenu
    {
        public const string Name = "-&EATeX";
        public const string MenuSeparator = "-";

        public static class SubItems
        {
            public const string GeneratePdf = "&Generate PDF document";
            public const string EditTemplate = "&Edit LaTeX template";
            public const string Settings = "&Settings";
            public const string About = "&About";
        }
    }
}