using Framework.Attributes;
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
	[TemplatePart(Name = "PART_GeneralPropertyPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_RelationPropertyPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_MetadataPropertyPanel", Type = typeof(Panel))]
    [StyleTypedProperty(Property = "DescriptionStyle", StyleTargetType = typeof(TextBlock))] // TODO
	public class PanelBase : TemplatedControl
	{
		private Panel _generalPropGrid;
        private Panel _relationPropGrid;
        private Panel _metadataPropGrid;

        public List<UIElement> ControlList = new List<UIElement>();

        static PanelBase()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(PanelBase),
				new FrameworkPropertyMetadata(typeof(PanelBase)));
            
        }

		public void AddField(string description, UIElement value, PropertyCategories category=PropertyCategories.General)
		{
            ControlList.Add(value);

            // TODO: Styling (siehe StyleTypedPropertyAttribute oben)
            TextBlock tbDescription = new TextBlock()
			{
				Text = description + ":"
			};

            switch (category)
            {
                case PropertyCategories.General:
                    _generalPropGrid.Children.Add(tbDescription);
                    _generalPropGrid.Children.Add(value);
                    break;
                case PropertyCategories.Metadata:
                    _metadataPropGrid.Children.Add(tbDescription);
                    _metadataPropGrid.Children.Add(value);
                    break;
                case PropertyCategories.Relational:
                    _relationPropGrid.Children.Add(tbDescription);
                    _relationPropGrid.Children.Add(value);
                    break;
                default:
                    _generalPropGrid.Children.Add(tbDescription);
                    _generalPropGrid.Children.Add(value);
                    break;

            }

			
		}

		public void ClearFields()
		{
            _metadataPropGrid.Children.Clear();
            _generalPropGrid.Children.Clear();
            _relationPropGrid.Children.Clear();
        }

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			AssignTemplateControl(ref _generalPropGrid, new WrapPanel(), "PART_GeneralPropertyPanel");
            AssignTemplateControl(ref _relationPropGrid, new WrapPanel(), "PART_RelationPropertyPanel");
            AssignTemplateControl(ref _metadataPropGrid, new WrapPanel(), "PART_MetadataPropertyPanel");
        }
	}
}
