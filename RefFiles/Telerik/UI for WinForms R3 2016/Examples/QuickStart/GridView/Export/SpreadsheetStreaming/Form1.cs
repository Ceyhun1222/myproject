﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;
using System.Management;

namespace Telerik.Examples.WinControls.GridView.Export.SpreadsheetStreaming
{
    public partial class Form1 : ExternalExampleHostForm
    {
        private readonly string ExternalExampleName = "SpreadsheetStreaming";

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
            return "ExportWithDpl." + ExternalExampleName + ".Form1";
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