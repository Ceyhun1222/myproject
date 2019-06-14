using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.CommonUtil.Util;
using XHTML_WPF;
using XHTML_WPF.ViewModel;

namespace TOSSM.View.Control
{
    /// <summary>
    /// Interaction logic for RichTextControl.xaml
    /// </summary>
    public partial class RichTextControl : UserControl
    {
        public RichTextControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly", typeof(bool), typeof(RichTextControl), new PropertyMetadata(default(bool), ReadModeChangedCallback));

        private static void ReadModeChangedCallback(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var control = source as RichTextControl;

                bool b = (bool)e.NewValue;
                if (control != null)
                    control.Image.Source = b
                        ? (ImageSource) Application.Current.TryFindResource("OpenBitmapImage")
                        : (ImageSource) Application.Current.TryFindResource("EditBitmapImage");
            }
            catch (Exception exception)
            {
                LogManager.GetLogger(typeof(RichTextControl)).Error(exception, "Error in the ReadModeChangedCallback");
                throw;
            }
        }

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(RichTextControl), new FrameworkPropertyMetadata(default(string), TextChangedCallback));

        private static void TextChangedCallback(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var control = source as RichTextControl;
                if (e.NewValue != null && control != null)
                {
                    control.TextBlock.Text = e.NewValue as string;
                    control.Viewer.Text = e.NewValue as string;
                }
            }
            catch (Exception exception)
            {
                LogManager.GetLogger(typeof(RichTextControl)).Error(exception, "Error in the TextChangedCallback");
                throw;
            }
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }


        public override void OnApplyTemplate()
        {
            try
            {
                base.OnApplyTemplate();
                var button = this.CommandButton;
                if (button != null)
                    button.Click += (sender, args) =>
                    {
                        var richTextDialog = new XhtmlEditor();
                        var viewModel = richTextDialog.DataContext as XhtmlEditorViewModel;
                        if (viewModel == null) return;
                        viewModel.HtmlText = Text;
                        viewModel.Editable = !IsReadOnly;
                        richTextDialog.ShowDialog();
                        if (!IsReadOnly && viewModel.IsSaved)
                            Text = viewModel.HtmlText;
                    };
                this.TextBlock.TextChanged += (sender, args) =>
                {
                    var textBox = sender as TextBox;
                    if (textBox != null) Text = textBox.Text;
                };
            }
            catch (Exception exception)
            {
                LogManager.GetLogger(typeof(RichTextControl)).Error(exception, "Error in the OnApplyTemplate");
                throw;
            }
        }
    }
}
