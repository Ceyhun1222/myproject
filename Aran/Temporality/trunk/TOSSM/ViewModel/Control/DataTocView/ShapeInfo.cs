using ESRI.ArcGIS.Geometry;

namespace TOSSM.ViewModel.Control.DataTocView
{
    public class ShapeInfo
    {
        public ShapeInfo(IGeometry shape, ICalculationContext calculation)
        {
            Shape = shape;
            if (Shape.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                IsPoint = true;
                CenterPoint = (IPoint)Shape;
            }
            else
            {
                var envelope = shape.Envelope;
                CenterPoint = new PointClass
                                  {
                                      X = (envelope.XMin + envelope.XMax)/2,
                                      Y = (envelope.YMin + envelope.YMax)/2
                                  };
            }
            CheckIfCanBeConsideredAsPoint(calculation);
        }

        public IGeometry Shape { get; private set; }

        public void CheckIfCanBeConsideredAsPoint(ICalculationContext calculation)
        {
            IsConsideredAsPoint = IsPoint||
                                  (
                                      Shape.Envelope.Width <= calculation.ScreenPixelXInMap
                                      &&
                                      Shape.Envelope.Height <= calculation.ScreenPixelYInMap
                                  );
        }

        public readonly bool IsPoint;
        public bool IsConsideredAsPoint;
        public IPoint CenterPoint;

        public bool IsConsumed;
        public bool IsVisible;

        public void ResetVisibility(IEnvelope extent)
        {
            var envelope = Shape.Envelope;
            envelope.Intersect(extent);
            IsVisible = !envelope.IsEmpty;
        }
    }
}