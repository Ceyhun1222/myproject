using TOSSM.ViewModel.Tool.PropertyPrecision.Editor;

namespace TOSSM.View.Tool
{
    /// <summary>
    /// Interaction logic for PropertyPrecisionSubEditor.xaml
    /// </summary>
    public partial class PropertyPrecisionSubEditor 
    {
        public PropertyPrecisionSubEditor()
        {
            InitializeComponent();
            Loaded += (t, e) =>
            {
                var model = DataContext as PrecisionSubEditorViewModel;
                if (model != null)
                {
                    model.Load();
                }
            };
        }
    }
}
