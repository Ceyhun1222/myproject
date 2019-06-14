using System.Collections.Generic;
using System.Linq;
using PVT.Model;
using PVT.Settings;

namespace PVT.Graphics
{
    public class CommonHoldingDrawer : IHoldingDrawer
    {

        private readonly List<IHoldingDrawer> _drawers;


        public CommonHoldingDrawer(StyleOption options) : this()
        {
            var dottedDrawer = new PointHoldingDrawer{Style = options.PointStyle};
            Add(dottedDrawer);

            var curvedDrawer = new CurveHoldingDrawer {Style = options.NominalTrackStyle};
            Add(curvedDrawer);

            for (int i = 0; i < 10; i++)
            {
                Add(new ObstacleAssestmentAreaHoldingDrawer{Style = options.PrimaryProtectedAreaStyle, AreaIndex = i});
            }
        }

        public CommonHoldingDrawer()
        {
            _drawers = new List<IHoldingDrawer>();
        }

        public void Add(IHoldingDrawer drawer)
        {
            _drawers.Add(drawer);
        }

        public void Clear()
        {
            _drawers.Clear();
        }


        public void Draw(HoldingPattern obj)
        {
            foreach (var t in _drawers)
                t.Draw(obj);
        }

        public void Clean(HoldingPattern obj)
        {
            foreach (var t in _drawers)
                t.Clean(obj);
        }

        public void Clean()
        {
            foreach (var t in _drawers)
                t.Clean();
        }


        public bool IsEnabled()
        {
            return _drawers.Any(t => t.IsEnabled());
        }


        public void Draw(HoldingPattern pattern, int index)
        {
            foreach (var t in _drawers)
            {
                var areaDrawer = t as ObstacleAssestmentAreaHoldingDrawer;
                if(areaDrawer == null || areaDrawer.AreaIndex == index)
                    t.Draw(pattern);
            }
        }

        public void Clean(HoldingPattern pattern, int index)
        {
            foreach (var t in _drawers)
            {
                var areaDrawer = t as ObstacleAssestmentAreaHoldingDrawer;
                if (areaDrawer == null || areaDrawer.AreaIndex == index)
                    t.Clean(pattern);
            }
        }
    }
}