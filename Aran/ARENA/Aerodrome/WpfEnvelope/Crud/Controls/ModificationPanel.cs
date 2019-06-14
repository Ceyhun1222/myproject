using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfEnvelope.Crud.Controls
{
	[TemplatePart(Name = "PART_OkButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_CancelButton", Type = typeof(Button))]
	public class ModificationPanel : PanelBase
	{
		// OPT: teilweise wrapping durch die Events, teilweise für Binding direkt: Nicht so schön
		private Button _okButton;
		public Button OkButton
		{
			get { return _okButton; }
		}

		private Button _cancelButton;
		public Button CancelButton
		{
			get { return _cancelButton; }
		}

		public Action Ok { get; set; }
		private void OnOk()
		{
			if (Ok != null)
				Ok();
		}

		public Action Cancel { get; set; }
		private void OnCancel()
		{
			if (Cancel != null)
				Cancel();
		}

		static ModificationPanel()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(ModificationPanel),
				new FrameworkPropertyMetadata(typeof(ModificationPanel)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			AssignTemplateControl(ref _okButton, "PART_OkButton");
			AssignTemplateControl(ref _cancelButton, "PART_CancelButton");

			_okButton.Click += new RoutedEventHandler((s, e) => OnOk());
			_cancelButton.Click += new RoutedEventHandler((s, e) => OnCancel());
		}
	}
}
