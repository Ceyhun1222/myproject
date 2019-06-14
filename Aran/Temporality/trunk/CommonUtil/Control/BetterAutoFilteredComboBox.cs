using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Aran.Temporality.CommonUtil.Control
{
    public class BetterAutoFilteredComboBox : ComboBox
    {
        private int _silenceEvents;
        private bool _isFilterActive;

        /// <summary>
        /// Static ctor to override CombBox properties to allow user to search.
        /// </summary>
        static BetterAutoFilteredComboBox()
        {
            IsTextSearchEnabledProperty.OverrideMetadata(typeof(BetterAutoFilteredComboBox), new FrameworkPropertyMetadata(true));
            IsEditableProperty.OverrideMetadata(typeof(BetterAutoFilteredComboBox), new FrameworkPropertyMetadata(true));
            DisplayMemberPathProperty.OverrideMetadata(typeof(BetterAutoFilteredComboBox), new FrameworkPropertyMetadata(null, DisplayMemberPathChanged));
        }

        private static void DisplayMemberPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var self = (BetterAutoFilteredComboBox)o;
            self.CoerceValue(SearchTextPathProperty);
        }

        /// <summary>
        /// Creates a new instance of <see cref="BetterAutoFilteredComboBox" />.
        /// </summary>
        public BetterAutoFilteredComboBox()
        {
            DependencyPropertyDescriptor textProperty = DependencyPropertyDescriptor.FromProperty(
                TextProperty, typeof(BetterAutoFilteredComboBox));
            textProperty.AddValueChanged(this, OnTextChanged);

            RegisterIsCaseSensitiveChangeNotification();
        }

        #region IsCaseSensitive Dependency Property
        /// <summary>
        /// The <see cref="DependencyProperty"/> object of the <see cref="IsCaseSensitive" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCaseSensitiveProperty =
            DependencyProperty.Register("IsCaseSensitive", typeof(bool), typeof(BetterAutoFilteredComboBox), new UIPropertyMetadata(false));

        /// <summary>
        /// Gets or sets the way the combo box treats the case sensitivity of typed text.
        /// </summary>
        /// <value>The way the combo box treats the case sensitivity of typed text.</value>
        [Description("The way the combo box treats the case sensitivity of typed text.")]
        [Category("AutoFiltered ComboBox")]
        [DefaultValue(true)]
        public bool IsCaseSensitive
        {
            [DebuggerStepThrough]
            get
            {
                return (bool)GetValue(IsCaseSensitiveProperty);
            }
            [DebuggerStepThrough]
            set
            {
                SetValue(IsCaseSensitiveProperty, value);
            }
        }

        protected virtual void OnIsCaseSensitiveChanged(object sender, EventArgs e)
        {
            if (IsCaseSensitive)
                IsTextSearchEnabled = false;

            RefreshFilter();
        }

        private void RegisterIsCaseSensitiveChangeNotification()
        {
            DependencyPropertyDescriptor.FromProperty(IsCaseSensitiveProperty, typeof(BetterAutoFilteredComboBox)).AddValueChanged(
                this, OnIsCaseSensitiveChanged);
        }
        #endregion

        #region DropDownOnFocus Dependency Property
        /// <summary>
        /// The <see cref="DependencyProperty"/> object of the <see cref="DropDownOnFocus" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DropDownOnFocusProperty =
            DependencyProperty.Register("DropDownOnFocus", typeof(bool), typeof(BetterAutoFilteredComboBox), new UIPropertyMetadata(true));

        /// <summary>
        /// Gets or sets the way the combo box behaves when it receives focus.
        /// </summary>
        /// <value>The way the combo box behaves when it receives focus.</value>
        [Description("The way the combo box behaves when it receives focus.")]
        [Category("AutoFiltered ComboBox")]
        [DefaultValue(true)]
        public bool DropDownOnFocus
        {
            [DebuggerStepThrough]
            get
            {
                return (bool)GetValue(DropDownOnFocusProperty);
            }
            [DebuggerStepThrough]
            set
            {
                SetValue(DropDownOnFocusProperty, value);
            }
        }
        #endregion

        #region | Handle selection |

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {

            if (!_isFilterActive &&
              ((e.Key > Key.A && e.Key < Key.Z) ||
              (e.Key > Key.OemSemicolon && e.Key < Key.Oem102)))
            {

                // Start activating the filter on the first key stroke.
                _isFilterActive = true;
            }
            else if (_isFilterActive && e.Key == Key.Return)
            {
                // When the filter is active and the user press return we handle it
                // by closing the drop-down and select the whole text.
                // If the auto-complete feature of the combo box had select an item this
                // item will be left selected, otherwise the text the user enter will
                // remain in the editable text box.
                // If we do not handle this case the combo box will clear the combo box because
                // during filtering no item is selected in the drop-down so the combo box does
                // not use auto-complete's selected item.
                object selectedValue = SelectedValue;
                ClearFilter();
                e.Handled = true;
                IsDropDownOpen = false;
                EditableTextBox.SelectAll();
                SelectedValue = selectedValue;
                return;
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            ClearFilter();
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);
            ClearFilter();
        }

        /// <summary>
        /// Called when <see cref="ComboBox.ApplyTemplate()"/> is called.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (EditableTextBox != null)
            {
                EditableTextBox.SelectionChanged += EditableTextBox_SelectionChanged; 
                EditableTextBox.PreviewKeyUp += EditableTextBox_PreviewKeyUp;
            }
        }

        void EditableTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (_isFilterActive)
            {
                if (e.Key == Key.Down)
                {
                    _isFilterActive = false;
                    e.Handled = true;
                    SelectedIndex = -1;
                    SelectedIndex = 0;
                }
                else if (e.Key == Key.Up)
                {
                    _isFilterActive = false;
                    e.Handled = true;
                    SelectedIndex = -1;
                    SelectedIndex = Items.Count - 1;
                }
            }
        }

        /// <summary>
        /// Gets the text box in charge of the editable portion of the combo box.
        /// </summary>
        protected TextBox EditableTextBox
        {
            get
            {
                return ((TextBox)Template.FindName("PART_EditableTextBox", this));
            }
        }

        private int _start, _length;

        private void EditableTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (_silenceEvents == 0 && _isFilterActive)
            {
                _silenceEvents++;
                int newStart = ((TextBox)(e.OriginalSource)).SelectionStart;
                int newLength = ((TextBox)(e.OriginalSource)).SelectionLength;

                if (newStart != _start || newLength != _length)
                {
                    _start = newStart;
                    _length = newLength;
                    RefreshFilter();
                }

                _silenceEvents--;
            }
        }
        #endregion

        #region | Handle focus |
        /// <summary>
        /// Invoked whenever an unhandled <see cref="UIElement.GotFocus" /> event
        /// reaches this element in its route.
        /// </summary>
        /// <param name="e">The <see cref="RoutedEventArgs" /> that contains the event data.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (ItemsSource != null && DropDownOnFocus)
            {
                IsDropDownOpen = true;
            }
        }
        #endregion

        #region | Handle filtering |


        /// <summary>
        /// Get or set path to a string based property that will be used to search for item.
        /// If this property is not set DisplayMemberPath is used instead.
        /// </summary>
        public string SearchTextPath
        {
            get { return (string)GetValue(SearchTextPathProperty); }
            set { SetValue(SearchTextPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchTextPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchTextPathProperty =
            DependencyProperty.Register("SearchTextPath", typeof(string), typeof(BetterAutoFilteredComboBox), new FrameworkPropertyMetadata(null, null, CoerceSearchTextPath));

        private static object CoerceSearchTextPath(DependencyObject o, object baseValue)
        {
            var self = (BetterAutoFilteredComboBox)o;
            if (baseValue == null) return self.DisplayMemberPath;
            return baseValue;
        }

        private void ClearFilter()
        {
            _isFilterActive = false;
            RefreshFilter();
        }

        private void RefreshFilter()
        {
            if (ItemsSource != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(ItemsSource);
                var collectionView = view as BindingListCollectionView;
                if (collectionView != null)
                {
                    if (collectionView.CanCustomFilter)
                    {
                        if (string.IsNullOrEmpty(FilterPrefix))
                        {
                            collectionView.CustomFilter = string.Empty;
                        }
                        else
                        {
                            // Keep the currently selected item in the combobox.
                            object currItem = SelectedItem;

                            // Change the filter on the view (which is a RepositoryView)
                            collectionView.CustomFilter = "//" + FilterPrefix;

                            // If we have a selected item in the combobox, select it in the view too.
                            if (currItem != null)
                            {
                                // Reselect the previous item
                                collectionView.MoveCurrentTo(currItem);
                            }
                        }
                    }
                }
                else
                {
                    view.Refresh();
                }

                //this.IsDropDownOpen = true;
            }
        }

        private bool FilterPredicate(object value)
        {
            // We don't like nulls.
            if (value == null)
                return false;

            // If there is no text, there's no reason to filter.
            if (string.IsNullOrEmpty(Text) || !_isFilterActive)
                return true;

            string prefix = Text;

            // If the end of the text is selected, do not mind it.
            if (_length > 0 && _start + _length == Text.Length)
            {
                prefix = prefix.Substring(0, _start);
            }

            return GetItemText(value).ToLower().Contains(prefix.ToLower());
        }

        private string FilterPrefix
        {
            get
            {
                // If there is no text, there's no reason to filter.
                if (Text.Length == 0)
                    return string.Empty;

                string prefix = Text;

                // If the end of the text is selected, do not mind it.
                if (_length > 0 && _start + _length == Text.Length)
                {
                    prefix = prefix.Substring(0, _start);
                }

                return prefix;
            }
        }

        private string GetItemText(object item)
        {
            if (string.IsNullOrEmpty(SearchTextPath))
            {
                return item.ToString();
            }

            Type t = item.GetType();
            object objValue = t.GetProperty(SearchTextPath).GetValue(item, null);

            string strValue = string.Empty;
            if (objValue != null) strValue = objValue.ToString();

            return strValue;
        }
        #endregion

        /// <summary>
        /// Called when the source of an item in a selector changes.
        /// </summary>
        /// <param name="oldValue">Old value of the source.</param>
        /// <param name="newValue">New value of the source.</param>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (newValue != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(newValue);
                if (!(view is BindingListCollectionView))
                {
                    view.Filter += FilterPredicate;
                }
            }

            if (oldValue != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(oldValue);
                if (!(view is BindingListCollectionView))
                {
                    if (view != null) view.Filter -= FilterPredicate;
                }
            }

            base.OnItemsSourceChanged(oldValue, newValue);
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (!IsTextSearchEnabled && _silenceEvents == 0)
            {
                RefreshFilter();

                // Manually simulate the automatic selection that would have been
                // available if the IsTextSearchEnabled dependency property was set.
                if (!string.IsNullOrEmpty(Text))
                {
                    foreach (object item in CollectionViewSource.GetDefaultView(ItemsSource))
                    {
                        string itemText = GetItemText(item);
                        int text = itemText.Length, prefix = Text.Length;
                        SelectedItem = item;

                        _silenceEvents++;
                        EditableTextBox.Text = itemText;
                        EditableTextBox.Select(prefix, text - prefix);
                        _silenceEvents--;
                        break;
                    }
                }
            }
        }
    }
}
