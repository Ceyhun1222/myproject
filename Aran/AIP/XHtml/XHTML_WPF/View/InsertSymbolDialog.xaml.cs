using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.RichTextBoxUI.Dialogs;
using Telerik.Windows.Documents.UI.Extensibility;

namespace XHTML_WPF
{
    /// <summary>
    /// Interaction logic for InsertSymbolDialog.xaml
    /// </summary>
    /// 
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [CustomInsertSymbolWindow]
    public partial class InsertSymbolDialog : RadRichTextBoxWindow, IInsertSymbolWindow
    {
        #region Fields

        public Action<char, FontFamily> insertSymbolCallback;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RadInsertSymbolDialog"/> class.
        /// </summary>
        public InsertSymbolDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="insertSymbolCallback">The callback that will be invoked to insert symbols.</param>
        /// <param name="initialFont">The font which symbols will be loaded initially.</param>
        /// <param name="owner">The owner of the dialog.</param>
        public void Show(Action<char, FontFamily> insertSymbolCallback, FontFamily initialFont, RadRichTextBox owner)
        {
            this.insertSymbolCallback = insertSymbolCallback;
            this.SetOwner(owner);

            if (!this.IsOpen)
            {
                this.symbolPicker.SelectedFontFamily = initialFont;
                this.Show();
            }
        }

        #endregion

        #region Events Hanlders

        public void symbolPicker_SymbolSelected(object sender, Telerik.Windows.Controls.RichTextBoxUI.Dialogs.Symbols.SymbolEventArgs e)
        {
            if (this.insertSymbolCallback != null)
            {
                this.insertSymbolCallback(e.Symbol, this.symbolPicker.SelectedFontFamily);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RadWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.Close();
            }
        }

        protected override void OnClosed(WindowClosedEventArgs args)
        {
            base.OnClosed(args);

            this.insertSymbolCallback = null;
            this.Owner = null;
        }

        #endregion
    }
}
