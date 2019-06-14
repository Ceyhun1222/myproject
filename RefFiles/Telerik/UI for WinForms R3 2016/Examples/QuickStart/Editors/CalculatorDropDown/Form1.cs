using System;
using Telerik.Examples.WinControls.Editors.ComboBox;

namespace Telerik.Examples.WinControls.Editors.CalculatorDropDown
{
    public partial class Form1 : EditorExampleBaseForm
    {
        public Form1()
        {
            InitializeComponent();

            this.SelectedControl = this.radCalculatorDropDown1;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.radCalculatorDropDown1.CalculatorElement.ShowPopup();
        }
    }
}