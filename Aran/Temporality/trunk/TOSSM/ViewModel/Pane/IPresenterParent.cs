using System;

namespace TOSSM.ViewModel.Pane
{
    public interface IPresenterParent
    {
        void ReloadData(DataPresenterModel model);
        DateTime AiracDate { get; set; }
    }
}