using MvvmCore;

namespace TOSSM.ViewModel.Document.Slot.Problems.Single
{
    public class SyntaxErrorViewModel : ViewModelBase
    {
        public string PropertyPath { get; set; }
        public string Violation { get; set; }
        public string StringValue { get; set; }
    }
}
