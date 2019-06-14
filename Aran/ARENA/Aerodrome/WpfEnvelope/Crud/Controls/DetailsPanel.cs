using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace WpfEnvelope.Crud.Controls
{
	[TemplatePart(Name = "PART_NewButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_EditButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_RemoveButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_OperationsPanel", Type = typeof(Panel))]
	public class DetailsPanel : PanelBase
	{
		// Ich muss backing fields nehmen, da das AssignTemplate nicht mit Properties funktioniert (ref und out params gehen nicht mit Props)

		private Panel _operationsPanel;
		public Panel OperationsPanel
		{
			get { return _operationsPanel; }
		}

		// OPT: teilweise wrapping durch die Events, teilweise für Binding direkt: Nicht so schön
		private Button _newButton;
		public Button NewButton
		{
			get { return _newButton; }
		}

		private Button _editButton;
		public Button EditButton
		{
			get { return _editButton; }
		}

		private Button _removeButton;
		public Button RemoveButton
		{
			get { return _removeButton; }
		}

		public Action New { get; set; }
		private void OnNew()
		{
			if (New != null)
				New();
		}

		public Action Edit { get; set; }
		private void OnEdit()
		{
			if (Edit != null)
				Edit();
		}

		public Action Remove { get; set; }
		private void OnRemove()
		{
			if (Remove != null)
				Remove();
		}

		static DetailsPanel()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(DetailsPanel),
				new FrameworkPropertyMetadata(typeof(DetailsPanel)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			AssignTemplateControl(ref _operationsPanel, new StackPanel(), "PART_OperationsPanel");
			AssignTemplateControl(ref _newButton, "PART_NewButton");
			AssignTemplateControl(ref _editButton, "PART_EditButton");
			AssignTemplateControl(ref _removeButton, "PART_RemoveButton");

			NewButton.IsEnabled = false;
			_newButton.Click += new RoutedEventHandler((s, e) => OnNew());
			EditButton.IsEnabled = false;
			_editButton.Click += new RoutedEventHandler((s, e) => OnEdit());
			RemoveButton.IsEnabled = false;
			_removeButton.Click += new RoutedEventHandler((s, e) => OnRemove());
		}
	}
}
