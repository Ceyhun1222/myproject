using System.Collections.Generic;
using System.Linq;
using PVT.Model;
using PVT.Settings;

namespace PVT.Graphics
{
    public class CommonProcedureDrawer : IProcedureDrawer
    {

        private readonly List<IProcedureDrawer> _drawers;


        public CommonProcedureDrawer(StyleOption options) : this()
        {
            var dottedDrawer = new PointProcedureDrawer {Style = options.PointStyle};
            Add(dottedDrawer);

            var curvedDrawer = new CurveProcedureDrawer {Style = options.NominalTrackStyle};
            Add(curvedDrawer);

            var primAreaDrawer = new PrimaryProttectedAreaProcedureDrawer
            {
                Style = options.PrimaryProtectedAreaStyle
            };
            Add(primAreaDrawer);

            var secAreaDrawer = new SecondaryProttectedAreaProcedureDrawer
            {
                Style = options.SecondaryProtectedAreaStyle
            };
            Add(secAreaDrawer);

            var fxAreaDrawer = new FixToleranceAreaProcedureeDrawer
            {
                Style = options.FixToleranceAreaStyle
            };
            Add(fxAreaDrawer);
        }

        public CommonProcedureDrawer()
        {
            _drawers = new List<IProcedureDrawer>();
        }

        public void Add(IProcedureDrawer drawer)
        {
            _drawers.Add(drawer);
        }

        public void Clear()
        {
            _drawers.Clear();
        }


        public void Clean()
        {
            foreach (var t in _drawers)
                t.Clean();
        }

        public void Clean(SegmentLeg leg)
        {
            foreach (var t in _drawers)
                t.Clean(leg);
        }

        public void Clean(SignificantPoint point)
        {
            foreach (var t in _drawers)
                t.Clean(point);
        }

        public void Clean(Transition transition)
        {
            foreach (var t in _drawers)
                t.Clean(transition);
        }

        public void Clean(ProcedureBase proc)
        {
            foreach (var t in _drawers)
                t.Clean(proc);
        }

        public void Draw(SegmentLeg leg)
        {
            foreach (var t in _drawers)
                t.Draw(leg);
        }

        public void Draw(SignificantPoint point)
        {
            foreach (var t in _drawers)
                t.Draw(point);
        }

        public void Draw(Transition transition)
        {
            foreach (var t in _drawers)
                t.Draw(transition);
        }

        public void Draw(ProcedureBase proc)
        {
            foreach (var t in _drawers)
                t.Draw(proc);
        }

        public bool IsEnabled()
        {
            return _drawers.Any(t => t.IsEnabled());
        }
    }
}
