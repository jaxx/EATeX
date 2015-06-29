using System;
using EA;
using EATeX.UI;

namespace EATeX
{
    public class EATeXAddin : IEAInterop
    {
        private readonly EATeXConfig configuration;

        public EATeXAddin()
        {
            configuration = new EATeXConfig();
        }

        public string EA_Connect(Repository repository)
        {
            return null;
        }

        public void EA_Disconnect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public object EA_GetMenuItems(Repository repository, string menuLocation, string menuName)
        {
            if (menuLocation != "TreeView")
                return null;

            switch (menuName)
            {
                case "":
                    return AddinMenu.Name;
                case AddinMenu.Name:
                    return new[]
                    {
                        AddinMenu.SubItems.GenerateTex,
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

        public void EA_MenuClick(Repository repository, string menuLocation, string menuName, string itemName)
        {
            switch (itemName)
            {
                case AddinMenu.SubItems.GenerateTex:
                    var texGenerator = new LatexGenerator(repository.GetTreeSelectedPackage(), configuration);
                    texGenerator.Generate();
                    break;
                case AddinMenu.SubItems.Settings:
                    new SettingsWindow(configuration).ShowDialog();
                    break;
                case AddinMenu.SubItems.About:
                    new AboutWindow().ShowDialog();
                    break;
            }
        }
    }

    public static class AddinMenu
    {
        public const string Name = "-&EATeX";
        public const string MenuSeparator = "-";

        public static class SubItems
        {
            public const string GenerateTex = "&Generate LaTeX file...";
            public const string GeneratePdf = "&Generate PDF document";
            public const string EditTemplate = "&Edit LaTeX template";
            public const string Settings = "&Settings";
            public const string About = "&About EATeX";
        }
    }
}