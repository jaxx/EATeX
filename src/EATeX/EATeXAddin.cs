﻿using EA;
using EATeX.UI;

namespace EATeX
{
    public class EATeXAddin : EAAddinBase
    {
        private readonly EATeXConfig configuration;

        public EATeXAddin()
        {
            configuration = new EATeXConfig();
        }

        public override object EA_GetMenuItems(Repository Repository, string MenuLocation, string MenuName)
        {
            switch (MenuName)
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

        public override void EA_MenuClick(Repository Repository, string MenuLocation, string MenuName, string ItemName)
        {
            switch (ItemName)
            {
                case AddinMenu.SubItems.GenerateTex:
                    var texGenerator = new LatexGenerator(Repository.GetTreeSelectedPackage(), configuration);
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