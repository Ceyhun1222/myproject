using System.Windows.Controls;
using TOSSM.Control;

namespace TOSSM.View.Document.Editor
{
    /// <summary>
    /// Interaction logic for FeatureSubEditor.xaml
    /// </summary>
    public partial class FeatureSubEditor : HierarchyControl
    {
        public FeatureSubEditor()
        {
            InitializeComponent();
        }

        #region Implementation of IFocusableDataGridHolder

        public override DataGrid FocusableDataGrid => datagridview;

        public override Panel SubHeaderPanel => HeaderPanel;

        #endregion
    }
}
