using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;
using Telerik.WinControls;

namespace Telerik.Examples.WinControls.Editors.ComboBox
{
    /// <summary>
    /// This is a base control for all RadComboBox examples.
    /// </summary>
    public partial class EditorExampleBaseForm : ExamplesForm
    {
        public EditorExampleBaseForm()
        {
            InitializeComponent();

            this.radPanelDemoHolder.PanelElement.PanelFill.Visibility = ElementVisibility.Collapsed;
            this.radPanelDemoHolder.PanelElement.PanelBorder.Visibility = ElementVisibility.Collapsed;

            this.AutoScaleMode = AutoScaleMode.None;
        }

        /// <summary>
        /// Resets the location of the panel that holds the example so that it
        /// always resides in the middle of the available space.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            if (this.radPanelDemoHolder != null)
            {
                //float xCoord = ((float)(this.Width - this.radPanelDemoHolder.Width)) / 2;
                //float yCoord = ((float)(this.Height - this.radPanelDemoHolder.Height)) / 2;

                this.radPanelDemoHolder.Location = Point.Empty;
            }

            base.OnResize(e);
        }
    }
}
