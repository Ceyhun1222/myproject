using ESRI.ArcGIS.Carto;

namespace TOSSM.ViewModel.Control.DataTocView
{
    public interface ICalculationContext
    {
        double ScreenPixelXInMap { get; set; }
        double ScreenPixelYInMap { get; set; }
        IActiveView ActiveView { get; set; }
    }
}