using System;
using Aran.Aim;
using MvvmCore;

namespace NotamViewer.ViewModel
{
    public class SingleFeatureViewModel : ViewModelBase
    {
        public FeatureType FeatureType { get; set; }
        public string Description { get; set; }
        public SingleFeatureHistoryViewModel History { get; set; }
    }
}