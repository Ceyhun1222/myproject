using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.QuickStart.WinControls;
using Telerik.Examples.WinControls.Editors.ComboBox;
using Telerik.WinControls;

namespace Telerik.Examples.WinControls.Buttons.Button
{
	public partial class Form1 : EditorExampleBaseForm
	{
		public Form1()
		{
			InitializeComponent();

            this.radRadioImgBeforeTxt.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            this.radButton1.DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
        }

        private void radRadioImgAboveTxt_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (this.radRadioImgAboveTxt.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                radButton3.TextImageRelation = TextImageRelation.ImageAboveText;
            }

            if (this.radRadioImgBeforeTxt.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                radButton3.TextImageRelation = TextImageRelation.ImageBeforeText;
            }

            if (this.radRadioTxtAboveImg.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                radButton3.TextImageRelation = TextImageRelation.TextAboveImage;
            }

            if (this.radRadioTxtBeforeImg.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                radButton3.TextImageRelation = TextImageRelation.TextBeforeImage;
            }

            if (this.radRadioOverlay.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                radButton3.TextImageRelation = TextImageRelation.Overlay;
            }
        }

        protected override void WireEvents()
        {
            this.radRadioTxtAboveImg.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radRadioImgAboveTxt_ToggleStateChanged);
            this.radRadioImgBeforeTxt.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radRadioImgAboveTxt_ToggleStateChanged);
            this.radRadioTxtBeforeImg.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radRadioImgAboveTxt_ToggleStateChanged);
            this.radRadioOverlay.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radRadioImgAboveTxt_ToggleStateChanged);
        }
	}
}