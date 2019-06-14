﻿using System;
using Telerik.QuickStart.WinControls;

namespace Telerik.Examples.WinControls.TreeView.ExportWithDpl
{
    public partial class Form1 : ExternalExampleHostForm
    {
        private readonly string ExternalExampleName = "TreeView";

         public Form1(string themeName)
        {
            this.ThemeName = themeName;
        }

        protected override string GetExecutablePath()
        {
            return @"\..\..\ExportWithDpl\bin\ExportWithDpl.exe";
        }

        protected override string GetEntryPointAsString()
        {
            return "ExportWithDpl."+ExternalExampleName+".Form1";
        }

        protected override string GetExternalProcessArguments(string excutablePath)
        {
            return String.Format("{0} {1}", ExternalExampleName, String.IsNullOrEmpty(this.ThemeName) ? "TelerikMetro" : this.ThemeName);
        }

        protected override bool CanOpenMultipleInstances
        {
            get
            {
                return true;
            }
        }
    }
}
