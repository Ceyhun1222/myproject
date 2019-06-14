using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Telerik.QuickStart.WinControls;
using Telerik.WinControls;

namespace Telerik.Examples.WinControls.Forms.AboutBox
{
    partial class RadAboutBox1 : ExamplesRadForm
    {
        private Assembly executingAssembly;

        public RadAboutBox1()
        {
            InitializeComponent();

            //  Initialize the AboutBox to display the product information from the assembly information.
            //  Change assembly information settings for your application through either:
            //  - Project->Properties->Application->Assembly Information
            //  - AssemblyInfo.cs
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.radLabelProductName.Text = AssemblyProduct;
            this.radLabelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.radLabelCopyright.Text = AssemblyCopyright.Substring(0, AssemblyCopyright.IndexOf("."));
            this.radLabelCompanyName.Text = AssemblyCompany;
            this.radTextBoxDescription.Text =
                "Telerik UI for WinForms includes over 110 UI controls that you can " + 
                "use to easily build unique and visually stunning Line of Business applications. " +
                "Boasting a well-designed architecture and CAB support, RadControls are perfectly " +
                "suited for your Enterprise desktop development.";
            this.logoPictureBox.Image = this.GetAboutBoxImage();

            this.ThemeNameChanged += new Telerik.WinControls.ThemeNameChangedEventHandler(RadAboutBox1_ThemeNameChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            string themeName = this.ThemeName;

            base.OnLoad(e);

            ThemeResolutionService.ApplyThemeToControlTree(this, themeName);
        }

        void RadAboutBox1_ThemeNameChanged(object source, Telerik.WinControls.ThemeNameChangedEventArgs args)
        {
            //String themeName = this.ThemeName;
            //this.radLabelProductName.ThemeName = themeName;
            //this.radLabelCompanyName.ThemeName = themeName;
            //this.radLabelCopyright.ThemeName = themeName;
            //this.radLabelVersion.ThemeName = themeName;
            //this.radTextBoxDescription.ThemeName = themeName;
            //this.okRadButton.ThemeName = themeName;
        }

        private Image GetAboutBoxImage()
        {
            Image img;
            Stream imageStream;
            string imageFileName = "AboutBox.png";

            if (executingAssembly == null)
            {
                this.executingAssembly = Assembly.GetExecutingAssembly();
            }

            imageStream = this.executingAssembly.GetManifestResourceStream(
                String.Format("Telerik.Examples.WinControls.Forms.AboutBox.{0}", imageFileName));
            img = Bitmap.FromStream(imageStream);

            if (img == null)
            {
                imageStream = this.executingAssembly.GetManifestResourceStream(
                String.Format(imageFileName));
                img = Bitmap.FromStream(imageStream);
            }

            return img;
        }



        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                // Get all Description attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                // If there aren't any Description attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Description attribute, return its value
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // If there aren't any Product attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Product attribute, return its value
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                return "Copyright © 2016 Progress Software Corporation. All rights Reserved.";

                //// Get all Copyright attributes on this assembly
                //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                //// If there aren't any Copyright attributes, return an empty string
                //if (attributes.Length == 0)
                //    return "";
                //// If there is a Copyright attribute, return its value
                //return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                return "Progress";

                //// Get all Company attributes on this assembly
                //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                //// If there aren't any Company attributes, return an empty string
                //if (attributes.Length == 0)
                //    return "";
                //// If there is a Company attribute, return its value
                //return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
