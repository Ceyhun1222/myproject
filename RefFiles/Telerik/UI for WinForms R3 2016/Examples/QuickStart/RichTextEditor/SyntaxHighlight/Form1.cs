﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;
using System.Management;

namespace Telerik.Examples.WinControls.RichTextEditor.SyntaxHighlight
{
    public partial class Form1 : ExternalExampleHostForm
    {
        private readonly string ExternalExampleName = "SyntaxHighlight";

        public Form1(string themeName)
        {
            this.ThemeName = themeName;
        }

        protected override string GetExecutablePath()
        {
            return @"\..\..\RichTextEditor\bin\RichTextEditor.exe";
        }

        protected override string GetEntryPointAsString()
        {
            return "RichTextEditor."+ExternalExampleName+".Form1";
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
