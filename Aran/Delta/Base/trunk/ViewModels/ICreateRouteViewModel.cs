using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Aran.Aim.Features;
using Aran.Delta.Model;

namespace Aran.Delta.ViewModels
{
    public interface ICreateRouteViewModel
    {
        ObservableCollection<Model.RouteSegmentModel> SegmentList { get; set; }
        List<Route> RouteList { get; set; }
        System.Collections.IList SelectedSegmentList { get; set; }
        RelayCommand AddCommand { get; }
        RelayCommand RemoveCommand { get; }
        RelayCommand SaveCommand { get; set; }
        new RelayCommand CloseCommand { get; }
        RelayCommand SplitCommand { get; }
        RelayCommand JoinCommand { get; }
        Route SelectedRoute { get; set; }
        RouteSegmentModel SelectedSegment { get; set; }
        int SelectedOperType { get; set; }
        string RouteName { get; set; }
        bool SaveIsEnabled { get; set; }
        Visibility NameVisibility { get; set; }
        Visibility EditIsVisible { get; }
        void Clear();
    }
}