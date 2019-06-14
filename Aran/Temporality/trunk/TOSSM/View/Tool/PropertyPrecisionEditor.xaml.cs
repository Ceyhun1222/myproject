using TOSSM.ViewModel.Tool.PropertyPrecision;

namespace TOSSM.View.Tool
{
    /// <summary>
    /// Interaction logic for PropertyPrecisionEditor.xaml
    /// </summary>
    public partial class PropertyPrecisionEditor 
    {
        public PropertyPrecisionEditor()
        {
            InitializeComponent();

            Loaded += (t, e) =>
            {
                var model = DataContext as PrecisionEditorToolViewModel;
                if (model != null)
                {
                    model.Load();
                }
            };
        }
    }
}
