using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using Telerik.Fixed.Legacy;
using Telerik.WinControls;

namespace Telerik.Examples.WinControls.PanelsLabels.CollapsiblePanel
{
    [ToolboxItem(true)]
    public class PersonInfoControl : RadControl
    {
        private PersonInfoElement personInfoElement;

        protected override void CreateChildItems(RadElement parent)
        {
            this.personInfoElement = new PersonInfoElement();
            parent.Children.Add(this.personInfoElement);
        }

        protected override System.Drawing.Size DefaultSize
        {
            get
            {
                return RadControl.GetDpiScaledSize(new System.Drawing.Size(300, 130));
            }
        }

        public string PersonName
        {
            get { return this.personInfoElement.NameElement.Text; }
            set { this.personInfoElement.NameElement.Text = value; }
        }

        public string PersonEmail 
        {
            get { return this.personInfoElement.EmailElement.Text; }
            set { this.personInfoElement.EmailElement.Text = value; }
        }

        public string PersonPhone
        {
            get { return this.personInfoElement.PhoneElement.Text; }
            set { this.personInfoElement.PhoneElement.Text = value; }
        }

        public Image PersonImage
        {
            get { return this.personInfoElement.ImageElement.Image; }
            set { this.personInfoElement.ImageElement.Image = value; }
        }
    }
}
