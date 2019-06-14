using MvvmCore;

namespace TOSSM.ViewModel.Document.Slot.Problems.ProblemList
{
    public abstract class ProblemListViewModel : ViewModelBase
    {
        public abstract void Clear();

        public abstract void Add(ViewModelBase problemViewModel);

        public abstract void Update();
    }
}
