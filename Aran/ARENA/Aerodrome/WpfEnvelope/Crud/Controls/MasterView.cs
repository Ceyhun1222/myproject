using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Collections;
using System.Windows.Data;
using Aerodrome.Features;
using WpfEnvelope.WpfShell.UI.Converter;

namespace WpfEnvelope.Crud.Controls
{
	[TemplatePart(Name = "PART_EntityList", Type = typeof(Selector))]
    [TemplatePart(Name = "PART_SelectedEntityList", Type = typeof(Selector))]

    [TemplatePart(Name = "PART_SelectedToggleBtn", Type = typeof(ToggleButton))]

    public class MasterView : TemplatedControl
	{
		static MasterView()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(MasterView),
				new FrameworkPropertyMetadata(typeof(MasterView)));

        }

		public MasterView()
		{
			EntityTemplateDefinitions = new List<TemplateDefinition>();

           
        }

        private void _selectedToggleBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleButton).IsChecked.Value)
            {
                EntityList.Visibility = Visibility.Collapsed;
                SelectedEntityList.Visibility = Visibility.Visible;

                RefreshSelectedList();
                SelectedEntityList.ItemsSource = ((ListView)EntityList).SelectedItems;
            }
            else
            {
                SelectedEntityList.Visibility = Visibility.Collapsed;
                EntityList.Visibility = Visibility.Visible;
            }
        }


        private ToggleButton _selectedToggleBtn;
       
        private MetaTypeRegistration _mtr;

		private Selector _entityList;
		public Selector EntityList
		{
			get
			{
				return _entityList;
			}
		}

        private Selector _selectedEntityList;
        public Selector SelectedEntityList
        {
            get
            {
                return _selectedEntityList;
            }
        }

        public ControlTemplate DefaultTemplate { get; set; }
		public List<TemplateDefinition> EntityTemplateDefinitions { get; set; }

		public void RefreshTemplate(MetaTypeRegistration mtr)
		{
			this.Template = new ControlTemplate(this.GetType());
			ApplyTemplate();

			_mtr = mtr;
			var template = EntityTemplateDefinitions
				.Where(it => it.EntityType == _mtr.Type)
				.Select(it => it.Template)
				.FirstOrDefault();
			if (template != null)
				this.Template = template;
			else
				this.Template = DefaultTemplate;

			ApplyTemplate();
			RefreshList();
		}

        

        private void RefreshList()
		{
			var listView = EntityList as ListView;
			GridView gridView = null;
			if (listView != null)
				gridView = listView.View as GridView;
			if (gridView != null)
				gridView.Columns.Clear();
           
            if (gridView != null)
			{
				foreach (var it in _mtr.CrudProperties)
				{
					if (it.Value.DisplayInGrid)
					{
                        var gridColumn = new GridViewColumn()
                        {
                            Header = new GridViewColumnHeader()
                            {
                                Content = it.Value.GetDisplayName(it.Key),
                                Name = it.Key.Name
                            },
                            DisplayMemberBinding = new Binding(it.Key.Name)
                        };
                        if(it.Key.PropertyType.Name.Equals(typeof(AM_Nullable<Type>).Name))
                        {
                            var genArgs = it.Key.PropertyType.GenericTypeArguments;
                            Type argType = genArgs[0];

                            if(argType==typeof(string))
                            {
                                gridColumn.DisplayMemberBinding = new Binding
                                {
                                    Path = new PropertyPath(it.Key.Name),
                                    Converter = new NullableToStringConverter(),
                                };
                            }

                            if (argType == typeof(double)|| argType == typeof(int))
                            {
                                gridColumn.DisplayMemberBinding = new Binding
                                {
                                    Path = new PropertyPath(it.Key.Name),
                                    Converter = new NullableToStringConverter(),
                                };
                            }

                            if (argType == typeof(DateTime))
                            {
                                gridColumn.DisplayMemberBinding = new Binding
                                {
                                    Path = new PropertyPath(it.Key.Name),
                                    Converter = new NullableToStringConverter(),
                                };
                            }
                            if (argType.IsEnum)
                            {
                                gridColumn.DisplayMemberBinding = new Binding
                                {
                                    Path = new PropertyPath(it.Key.Name),
                                    Converter = new NullableToStringConverter(),
                                };
                            }

                        }
                        gridView.Columns.Add(gridColumn);

                       

                    }
				}
			}

			if (_mtr.DataSourceManager.NeedsManualRefresh)
				EntityList.ItemsSource = _mtr.DataSourceManager.All;
			else
			{
				var binding = new Binding()
				{
					Source = _mtr.DataSourceManager.All
				};
				EntityList.SetBinding(Selector.ItemsSourceProperty, binding);
			}
		}

        public void RefreshSelectedList()
        {
            var listView = SelectedEntityList as ListView;
            GridView gridView = null;
            if (listView != null)
                gridView = listView.View as GridView;
            if (gridView != null)
                gridView.Columns.Clear();

            if (gridView != null)
            {
                foreach (var it in _mtr.CrudProperties)
                {
                    if (it.Value.DisplayInGrid)
                    {
                        var gridColumn = new GridViewColumn()
                        {
                            Header = new GridViewColumnHeader()
                            {
                                Content = it.Value.GetDisplayName(it.Key),
                                Name = it.Key.Name
                            },
                            DisplayMemberBinding = new Binding(it.Key.Name)
                        };
                        if (it.Key.PropertyType.Name.Equals(typeof(AM_Nullable<Type>).Name))
                        {
                            var genArgs = it.Key.PropertyType.GenericTypeArguments;
                            Type argType = genArgs[0];

                            if (argType == typeof(string))
                            {
                                gridColumn.DisplayMemberBinding = new Binding
                                {
                                    Path = new PropertyPath(it.Key.Name),
                                    Converter = new NullableToStringConverter(),
                                };
                            }

                            if (argType == typeof(double) || argType == typeof(int))
                            {
                                gridColumn.DisplayMemberBinding = new Binding
                                {
                                    Path = new PropertyPath(it.Key.Name),
                                    Converter = new NullableToStringConverter(),
                                };
                            }

                            if (argType == typeof(DateTime))
                            {
                                gridColumn.DisplayMemberBinding = new Binding
                                {
                                    Path = new PropertyPath(it.Key.Name),
                                    Converter = new NullableToStringConverter(),
                                };
                            }
                            if (argType.IsEnum)
                            {
                                gridColumn.DisplayMemberBinding = new Binding
                                {
                                    Path = new PropertyPath(it.Key.Name),
                                    Converter = new NullableToStringConverter(),
                                };
                            }

                        }
                        gridView.Columns.Add(gridColumn);



                    }
                }
            }

            //if (_mtr.DataSourceManager.NeedsManualRefresh)
            //    EntityList.ItemsSource = _mtr.DataSourceManager.All;
            //else
            //{
            //    var binding = new Binding()
            //    {
            //        Source = _mtr.DataSourceManager.All
            //    };
            //    EntityList.SetBinding(Selector.ItemsSourceProperty, binding);
            //}
        }

        public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			AssignTemplateChildren();

            _selectedToggleBtn.Click += _selectedToggleBtn_Click;
        }
       
        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			base.OnTemplateChanged(oldTemplate, newTemplate);
			AssignTemplateChildren();
		}

		private void AssignTemplateChildren()
		{
			AssignTemplateControl(ref _entityList, new ListBox(), "PART_EntityList");
            AssignTemplateControl(ref _selectedEntityList, new ListBox(), "PART_SelectedEntityList");

            AssignTemplateControl(ref _selectedToggleBtn, new ToggleButton(), "PART_SelectedToggleBtn");
        }
	}
}
