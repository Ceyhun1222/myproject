using System.Windows.Controls;
using Aran.Temporality.CommonUtil.Control;
using TOSSM.Control;

namespace TOSSM.View.Document.Editor
{
    /// <summary>
    /// Interaction logic for FeatureEditor.xaml
    /// </summary>
    public partial class FeatureEditor : HierarchyControl
    {
        public FeatureEditor()
        {
            InitializeComponent();
        }

        #region Implementation of IFocusableDataGridHolder

        public override DataGrid FocusableDataGrid => datagridview;

        public override Panel SubHeaderPanel => HeaderPanel;

        public override AnimatedScrollViewer HorisontalScrollViewer => MainScrollViewer;

        #endregion
    }
}
