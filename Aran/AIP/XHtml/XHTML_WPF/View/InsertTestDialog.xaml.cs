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
    public partial class InsertTestDialog : RadRichTextBoxWindow
    {
        #region Fields

        private Action<char, FontFamily> insertSymbolCallback;

        #endregion

        #region Constructors

        public InsertTestDialog()
        {
            InitializeComponent();
        }

        #endregion



        #region Events Hanlders



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
