using System;
using System.Collections.Generic;
using PVT.Model;
using PVT.Model.Drawing;

namespace PVT.Graphics
{
    public abstract class ProcedureDrawerBase : DrawerBase, IProcedureDrawer
    {
        private  Dictionary<Guid, HandlerBase> Procedures => Handlers[typeof(ProcedureHandler)];
        private  Dictionary<Guid, HandlerBase> Transitions => Handlers[typeof(TransitionHandler)];
        private  Dictionary<Guid, HandlerBase> Legs => Handlers[typeof(LegHandler)];
        private  Dictionary<Guid, HandlerBase> SigPoints => Handlers[typeof(SignificantPointHandler)];
     


        protected ProcedureDrawerBase()
        {
            Handlers = new Dictionary<Type, Dictionary<Guid, HandlerBase>>
            {
                {typeof(ProcedureHandler), new Dictionary<Guid, HandlerBase>()},
                {typeof(TransitionHandler), new Dictionary<Guid, HandlerBase>()},
                {typeof(LegHandler), new Dictionary<Guid, HandlerBase>()},
                {typeof(SignificantPointHandler), new Dictionary<Guid, HandlerBase>()}
            };
            Style = new Styles();
        }


        public void Draw(SignificantPoint point)
        {
            if (!IsEnabled()) return;
            if (SigPoints.ContainsKey(point.Identifier))
                return;
            Aran.Geometries.Point loc;
            if (point.DesignatedPoint != null)
                loc = point.DesignatedPoint.Original.Location.Geo;
            else if (point.Navaid != null)
                loc = point.Navaid.Original.Location.Geo;
            else return;
            var prjLoc = Engine.Environment.Current.Geometry.ToPrj<Aran.Geometries.Point>(loc);
            var drawObject = new DrawObject(point.Identifier);
            drawObject.Add(Engine.Environment.Current.Graphics.Draw(prjLoc, Style.PointStyle.Geometry.Style, Style.PointStyle.Color.ToRGB(), Style.PointStyle.Size));
            SigPoints.Add(drawObject.Id, new SignificantPointHandler(drawObject));
        }

        public void Draw(SegmentLeg leg)
        {
            if (!IsEnabled()) return;
            if (Legs.ContainsKey(leg.Identifier))
                return;
            var handler = draw(leg);
            Legs.Add(handler.Identifier, handler);
        }

        public void Draw(Transition transition)
        {
            if (!IsEnabled()) return;
            if (Transitions.ContainsKey(transition.Identifier))
                return;
            var handler = draw(transition);
            Transitions.Add(handler.Identifier, handler);
        }

        public void Draw(ProcedureBase procedure)
        {
            if (!IsEnabled()) return;
            if (Procedures.ContainsKey(procedure.Identifier))
                return;
            var handler = draw(procedure);
            Procedures.Add(handler.Identifier, handler);
        }

        protected abstract DrawObject DrawLeg(SegmentLeg leg);

        public override void Clean()
        {
            CleanProcedures();
            CleanTransitions();
            CleanLegs();
            CleanSignificantPoints();
        }


        public void Clean(Transition transition)
        {
            if (Transitions.ContainsKey(transition.Identifier))
            {
                clean(Transitions[transition.Identifier] as TransitionHandler);
                Transitions.Remove(transition.Identifier);
            }
        }

        public void Clean(SegmentLeg leg)
        {
            if (Legs.ContainsKey(leg.Identifier))
            {
                clean(Legs[leg.Identifier] as LegHandler);
                Legs.Remove(leg.Identifier);
            }
        }

        public void Clean(SignificantPoint point)
        {
            if (SigPoints.ContainsKey(point.Identifier))
            {
                clean(SigPoints[point.Identifier] as SignificantPointHandler);
                SigPoints.Remove(point.Identifier);
            }
        }

        public void Clean(ProcedureBase proc)
        {
            if (Procedures.ContainsKey(proc.Identifier))
            {
                clean(Procedures[proc.Identifier] as ProcedureHandler);
                Procedures.Remove(proc.Identifier);
            }
        }
        private ProcedureHandler draw(ProcedureBase procedure)
        {
            var handler = new ProcedureHandler(procedure.Identifier);
            foreach (Transition t in procedure.Transtions)
            {
                handler.Transitions.Add(draw(t));
            }
            return handler;
        }


        private TransitionHandler draw(Transition transition)
        {
            var handler = new TransitionHandler(transition.Identifier);
            foreach (TransitionLeg t in transition.TransitionLegs)
            {
                handler.Legs.Add(draw(t.SegmentLeg));
            }
            return handler;
        }


        private LegHandler draw(SegmentLeg leg)
        {
            var handler = new LegHandler(DrawLeg(leg));
            return handler;
        }



        private SignificantPointHandler draw(SignificantPoint point)
        {
            Aran.Geometries.Point loc;
            if (point.DesignatedPoint != null)
                loc = point.DesignatedPoint.Original.Location.Geo;
            else if (point.Navaid != null)
                loc = point.Navaid.Original.Location.Geo;
            else return null;
            var prjLoc = Engine.Environment.Current.Geometry.ToPrj<Aran.Geometries.Point>(loc);
            var drawObject = new DrawObject(point.Identifier);
            drawObject.Add(Engine.Environment.Current.Graphics.Draw(prjLoc, Style.PointStyle.Geometry.Style, Style.PointStyle.Color.ToRGB(), Style.PointStyle.Size));
            var handler = new SignificantPointHandler(drawObject);
            return handler;
        }


        private void clean(SignificantPointHandler handler) => Delete(handler.Handler);

        private void clean(LegHandler handler) => Delete(handler.Handler);

        private void clean(TransitionHandler handler)
        {
            foreach (var t in handler.Legs)
                clean(t);
        }

        private void clean(ProcedureHandler handler)
        {
            foreach (var t in handler.Transitions)
                clean(t);
        }

        private void CleanSignificantPoints()
        {
            foreach (var entry in SigPoints)
            {
                clean(entry.Value as SignificantPointHandler);
            }
            SigPoints.Clear();
        }

        private void CleanLegs()
        {
            foreach (var entry in Legs)
            {
                clean(entry.Value as LegHandler);
            }
            Legs.Clear();
        }

        private void CleanTransitions()
        {
            foreach (var entry in Transitions)
            {
                clean(entry.Value as TransitionHandler);
            }
            Transitions.Clear();
        }

        private void CleanProcedures()
        {
            foreach (var entry in Procedures)
            {
                clean(entry.Value as ProcedureHandler);
            }
            Procedures.Clear();
        }
    }

  
}
