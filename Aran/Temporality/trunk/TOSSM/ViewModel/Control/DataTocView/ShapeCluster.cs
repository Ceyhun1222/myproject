using System;
using ESRI.ArcGIS.Geometry;

namespace TOSSM.ViewModel.Control.DataTocView
{
    public class ShapeCluster
    {
        public IPoint CenterPoint;
        public int Count;

        public bool IsCenterPointVisible;

        public ShapeCluster(IPoint point)
        {
            CenterPoint = point;
            IsCenterPointVisible = true;
            Count = 1;
        }

        public bool TryConsume(ShapeInfo info, ICalculationContext calculation)
        {
            if (!IsCenterPointVisible) return false;
           
            if (!info.IsConsideredAsPoint && !info.IsPoint)
            {
                return false;
            }

            var canConsume = Math.Abs(CenterPoint.X - info.CenterPoint.X) <= calculation.ScreenPixelXInMap &&
                   Math.Abs(CenterPoint.Y - info.CenterPoint.Y) <= calculation.ScreenPixelYInMap;
            if (canConsume)
            {
                Consume(info);
            }
            return canConsume;
        }

        public void Consume(ShapeInfo point)
        {
            point.IsConsumed = true;
            Count++;
        }


        public void ResetCenterPointVisibility(IEnvelope extent)
        {
            var envelope = CenterPoint.Envelope;
            envelope.Intersect(extent);
            IsCenterPointVisible = !envelope.IsEmpty;
        }
    }
}