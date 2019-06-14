using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections;
using Aerodrome.Features;
using Aerodrome.DataType;
using WpfEnvelope.WpfShell.UI.Converter;

namespace WpfEnvelope.WpfShell.UI.Controls
{
	public class SortListView : ListView
	{
		static SortListView()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(SortListView),
				new FrameworkPropertyMetadata(typeof(SortListView)));
		}

		public SortListView()
		{
			_headerClickHandler = new RoutedEventHandler(GridViewColumnHeader_Click);
		}

		private RoutedEventHandler _headerClickHandler;
		private ListSortDirection _lastDirection = ListSortDirection.Ascending;

		#region DefaultItemsContainerStyle

		public Style DefaultItemsContainerStyle
		{
			get { return (Style)GetValue(DefaultItemsContainerStyleProperty); }
			set { SetValue(DefaultItemsContainerStyleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for DefaultItemsContainerStyle.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DefaultItemsContainerStyleProperty =
			DependencyProperty.Register("DefaultItemsContainerStyle", typeof(Style), typeof(SortListView),
			new UIPropertyMetadata(null, new PropertyChangedCallback((s, e) =>
				{
					var me = s as SortListView;
					me.ItemContainerStyle = e.NewValue as Style;
				})));

		#endregion

		public override void EndInit()
		{
			base.EndInit();

			var gridView = View as GridView;
			if (gridView != null)
				gridView.Columns.CollectionChanged += new NotifyCollectionChangedEventHandler(GridViewColumns_CollectionChanged);
		}

		void GridViewColumns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (var it in e.NewItems)
				{
					var header = GetGridViewColumnHeader(it);
					if (header != null)
						header.Click += _headerClickHandler;
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (var it in e.OldItems)
				{
					var header = GetGridViewColumnHeader(it);
					if (header != null)
						header.Click -= _headerClickHandler;
				}
			}
		}

		private GridViewColumnHeader GetGridViewColumnHeader(object gridViewColumn)
		{
			var column = gridViewColumn as GridViewColumn;
			if (column != null)
				return column.Header as GridViewColumnHeader;
			else
				return null;
		}

		void GridViewColumnHeader_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;

			if (headerClicked != null)
			{
				if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
				{
					if (_lastDirection == ListSortDirection.Ascending)
						_lastDirection = ListSortDirection.Descending;
					else
						_lastDirection = ListSortDirection.Ascending;

					Sort(headerClicked.Name, _lastDirection);
				}
			}
		}

        private void Sort(string sortBy, ListSortDirection direction)
        {
           
            var castedList = ItemsSource.Cast<AM_AbstractFeature>();
            var firstElem = castedList.FirstOrDefault();
            var currentProperty = firstElem?.GetType().GetProperty(sortBy);
            if (currentProperty is null)
                return;
            if (currentProperty.PropertyType.Name.Equals(typeof(AM_Nullable<Type>).Name))
            {
                ListCollectionView dataView1 = (ListCollectionView)CollectionViewSource.GetDefaultView(ItemsSource);
                dataView1.SortDescriptions.Clear();
                if (direction == ListSortDirection.Ascending)
                    dataView1.CustomSort = new NullableTypeSorter(sortBy, true);
                else
                    dataView1.CustomSort = new NullableTypeSorter(sortBy, false);
                dataView1.Refresh();
                return;
            }

            if (currentProperty.PropertyType.Name.Equals(typeof(DataType<Enum>).Name))
            {
                var dataView1 = CollectionViewSource.GetDefaultView(ItemsSource);

                dataView1.SortDescriptions.Clear();

                SortDescription sd1 = new SortDescription(sortBy + "." + nameof(DataType<Enum>.Value), direction);
                dataView1.SortDescriptions.Add(sd1);
                dataView1.Refresh();
                return;
            }

            ICollectionView dataView = CollectionViewSource.GetDefaultView(ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
	}
}
