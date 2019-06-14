using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ChoosePointNS
{
	public partial class SignificantPointChoiceControl : UserControl
	{
		private SignificantPointChoiceList _choiceList = SignificantPointChoiceList.AirportHeliport;
		private IWindowsFormsEditorService editorService = null;

		public SignificantPointChoiceControl(SignificantPointChoiceList choiceList, IWindowsFormsEditorService editService)
		{
			InitializeComponent();

			// Cache the SignificantPointChoiceList value provided by the
			// design-time environment.
			this.SignificantPointChoiceList = choiceList;

			// Cache the reference to the editor service.
			this.editorService = editService;
		}

		public SignificantPointChoiceList SignificantPointChoiceList
		{
			get
			{
				return this._choiceList;
			}

			set
			{
				if (this._choiceList != value)
				{
					ChckBxNavaid.Checked = (value & SignificantPointChoiceList.Navaid) == SignificantPointChoiceList.Navaid;
					ChckBxRunwayCntrlinePnt.Checked = (value & SignificantPointChoiceList.RunwayCentrelinePoint) == SignificantPointChoiceList.RunwayCentrelinePoint;
					ChckBxAirportHeliport.Checked = (value & SignificantPointChoiceList.AirportHeliport) == SignificantPointChoiceList.AirportHeliport;
					ChckBxTouchDownLiftOff.Checked = (value & SignificantPointChoiceList.TouchDownLiftOff) == SignificantPointChoiceList.TouchDownLiftOff;
					ChckBxDesignatedPoint.Checked = (value & SignificantPointChoiceList.DesignatedPoint) == SignificantPointChoiceList.DesignatedPoint;
					ChckBxPoint.Checked = (value & SignificantPointChoiceList.Point) == SignificantPointChoiceList.Point;
					this._choiceList = value;
				}
			}
		}

		private void ChoiceCheckBoxesCheckedChanged(object sender, EventArgs e)
		{
			this._choiceList = SignificantPointChoiceList.None;

			if (ChckBxNavaid.Checked)
				this._choiceList |= SignificantPointChoiceList.Navaid;

			if (ChckBxRunwayCntrlinePnt.Checked)
				this._choiceList |= SignificantPointChoiceList.RunwayCentrelinePoint;

			if (ChckBxAirportHeliport.Checked)
				this._choiceList |= SignificantPointChoiceList.AirportHeliport;

			if (ChckBxTouchDownLiftOff.Checked)
				this._choiceList |= SignificantPointChoiceList.TouchDownLiftOff;

			if (ChckBxDesignatedPoint.Checked)
				this._choiceList |= SignificantPointChoiceList.DesignatedPoint;

			if (ChckBxPoint.Checked)
				this._choiceList |= SignificantPointChoiceList.Point;

			this.Invalidate(false);
		}
	}

	internal class SignificantPointChoiceEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IWindowsFormsEditorService editorService = null;
			if (provider != null)
				editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

			if (editorService != null)
			{
				SignificantPointChoiceControl choiceControl = new SignificantPointChoiceControl((SignificantPointChoiceList)value, editorService);
				editorService.DropDownControl(choiceControl);
				value = choiceControl.SignificantPointChoiceList;
			}

			return value;
		}
	}
}
