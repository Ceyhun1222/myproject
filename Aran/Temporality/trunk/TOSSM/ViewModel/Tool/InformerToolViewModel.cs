using System;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Tool
{
    public class InformerToolViewModel : ToolViewModel
    {
        public static string ToolContentId = "Informer";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/info.png", UriKind.RelativeOrAbsolute);

        private string _informationText="Useful information";
        public string InformationText
        {
            get => _informationText;
            set => _informationText = value;
        }

        public InformerToolViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;
        }
    }
}
