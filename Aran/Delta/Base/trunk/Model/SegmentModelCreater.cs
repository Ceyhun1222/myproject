using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.Features;

namespace Aran.Delta.Model
{
    internal class SegmentModelCreater
    {
        private readonly IList<IPointModel> _pointList;

        public SegmentModelCreater(IList<IPointModel> pointList)
        {
            _pointList = pointList;
        }

        public RouteSegmentModel GetSegmentModel(Aran.Aim.Features.RouteSegment routeSegment)
        {
            var segmentModel = new RouteSegmentModel(ObjectStatus.Existing)
            {
                RSegment = routeSegment,
            };

            if (routeSegment.Start == null || routeSegment.End == null)
                return null;

            SetSegmentPoints(segmentModel, routeSegment);

            segmentModel.InitializeSegment();
            segmentModel.Status = ObjectStatus.Existing;
            return segmentModel;
        }

        private void SetSegmentPoints(RouteSegmentModel segmentModel, RouteSegment routeSegment)
        {
            var startPtModel = GetSegmentPointFeature(routeSegment.Start);
            if (startPtModel != null)
            {
                segmentModel.StartPointList.Add(startPtModel);
                segmentModel.SelectedStartPoint = startPtModel;
            }

            var endPtModel = GetSegmentPointFeature(routeSegment.End);
            if (endPtModel != null)
            {

                segmentModel.EndPointList.Add(endPtModel);
                segmentModel.SelectedEndPoint = endPtModel;
            }
        }

        private IPointModel GetSegmentPointFeature(EnRouteSegmentPoint segmentPoint)
        {
            var ptChoice = segmentPoint.PointChoice;

            IPointModel ptModel = null;

            switch (ptChoice.Choice)
            {
                case SignificantPointChoice.AirportHeliport:
                    ptModel = _pointList.FirstOrDefault(
                        point => point.Identifier == ptChoice.AirportReferencePoint?.Identifier);
                    break;
                case SignificantPointChoice.DesignatedPoint:
                    ptModel = _pointList.FirstOrDefault(
                        point => point.Identifier == ptChoice.FixDesignatedPoint.Identifier);
                    break;
                case SignificantPointChoice.Navaid:
                {
                    ptModel = _pointList.FirstOrDefault(
                        point => point.Identifier == ptChoice.NavaidSystem.Identifier);
                }
                    break;
                case SignificantPointChoice.RunwayCentrelinePoint:
                {
                    ptModel = _pointList.FirstOrDefault(
                        point => point.Identifier == ptChoice.RunwayPoint.Identifier);
                }
                    break;
            }

            return ptModel;
        }
    }
}
