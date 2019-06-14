using System.Collections.Generic;
using System.ComponentModel;
using Aran.Temporality.CommonUtil.Util;

namespace Aran.Temporality.CommonUtil.Control.TreeViewDragDrop
{
    public interface ITreeViewItemViewModel : INotifyPropertyChanged
    {
        MtObservableCollection<ITreeViewItemViewModel> Children { get; }
        bool HasDummyChild { get; }
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
        ITreeViewItemViewModel Parent { get; }
    }
}
