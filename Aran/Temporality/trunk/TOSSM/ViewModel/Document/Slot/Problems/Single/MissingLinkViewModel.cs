using MvvmCore;

namespace TOSSM.ViewModel.Document.Slot.Problems.Single
{
    public class MissingLinkViewModel : ViewModelBase
    {
        public string PropertyPath { get; set; }
        public string ReferenceFeatureType { get; set; }
        public string ReferenceFeatureIdentifier { get; set; }
    }
}
