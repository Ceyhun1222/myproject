using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Windows.Documents;
using Aran.Aim.Enums;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Queries;

namespace PVT.Model
{
    public class Transition
    {
        public ProcedureTransition Original { get; }
        public Guid Identifier { get; private set; }
        public List<TransitionLeg> TransitionLegs { get; }
        public List<TransitionLeg> InitialLegs { get; }
        public List<TransitionLeg> FinalLegs { get; }
        public List<TransitionLeg> IntermediateLegs { get; }
        public List<TransitionLeg> MissedApproachLegs { get; }
        public List<TransitionLeg> DepartureLegs { get; }
        public List<TransitionLeg> ArrivalLegs { get; }
        public string Type { get; private set; }
        public bool NotValid { get; private set; }
        

        public Transition(ProcedureTransition transtion,ProcedureType procedureType)
        {
            Original = transtion;
            Identifier = Guid.NewGuid();
            TransitionLegs = new List<TransitionLeg>();
            InitialLegs = new List<TransitionLeg>();
            FinalLegs = new List<TransitionLeg>();
            IntermediateLegs = new List<TransitionLeg>();
            MissedApproachLegs = new List<TransitionLeg>();
            DepartureLegs = new List<TransitionLeg>();
            ArrivalLegs = new List<TransitionLeg>();
            Type = Original.Type?.ToString() ?? "Unknown";

            if (Type == "Unknown")
            {
                Engine.Environment.Current.Logger.Warn("Unknown transition type", transtion);
            }

            // var transitionLegList = SortTransitions(transtion.TransitionLeg, procedureType);
            var transitionLegList = transtion.TransitionLeg;
            
            //foreach (var t in transtion.TransitionLeg)
            foreach (var t in transitionLegList)
            {
                var leg = new TransitionLeg(t);
                if (leg.IsEmpty)
                    continue;

                if (t.TheSegmentLeg == null)
                    continue;
                TransitionLegs.Add(leg);
                switch (t.TheSegmentLeg.Type)
                {
                    case Aran.Aim.SegmentLegType.InitialLeg:
                        InitialLegs.Add(leg);
                        break;
                    case Aran.Aim.SegmentLegType.FinalLeg:
                        FinalLegs.Add(leg);
                        break;
                    case Aran.Aim.SegmentLegType.IntermediateLeg:
                        IntermediateLegs.Add(leg);
                        break;
                    case Aran.Aim.SegmentLegType.MissedApproachLeg:
                        MissedApproachLegs.Add(leg);
                        break;
                    case Aran.Aim.SegmentLegType.DepartureLeg:
                        DepartureLegs.Add(leg);
                        break;
                    case Aran.Aim.SegmentLegType.ArrivalLeg:
                        ArrivalLegs.Add(leg);
                        break;
                    default:
                        Engine.Environment.Current.Logger.Warn("Unknown leg", null);
                        break;
                }

            }
            CheckLegs(InitialLegs);
            CheckLegs(FinalLegs);
            CheckLegs(IntermediateLegs);
            CheckLegs(MissedApproachLegs);
            CheckLegs(DepartureLegs);
            CheckLegs(ArrivalLegs);
        }

        private void CheckLegs(IReadOnlyList<TransitionLeg> transitionLegs)
        {
            
            for (var i = 0; i < transitionLegs.Count; i++)
            {
                SegmentLeg prev = null;
                SegmentLeg next = null;
                var current = transitionLegs[i].SegmentLeg;
                if (i > 0) prev = transitionLegs[i - 1].SegmentLeg;
                if (i + 1 < transitionLegs.Count) next = transitionLegs[i + 1].SegmentLeg;
                if (current.NotVaild)
                {
                    if (prev != null && !prev.NotVaild && prev.Original.LegTypeARINC == Aran.Aim.Enums.CodeSegmentPath.VI)
                        current.SetValid();
                    else if(next != null && !next.NotVaild && next.Original.LegTypeARINC == Aran.Aim.Enums.CodeSegmentPath.VI)
                        current.SetValid();
                }

                if (current.NotVaild) NotValid = true;
            }
        }

        public List<ProcedureTransitionLeg> SortTransitions(List<ProcedureTransitionLeg> transitoLegs,ProcedureType procType
           )
        {
            var result = new List<ProcedureTransitionLeg>();

            if (transitoLegs.Count == 0)
                return result;

            var geoOperators = new GeometryOperators();

            var refPoint = Engine.Environment.Current.DbProvider.GetAirport(Engine.Environment.Current.CurrentAeroport);
            var refPointPrj = Engine.Environment.Current.Geometry.ToPrj<Aran.Geometries.Point>(refPoint.ARP.Geo);

            ProcedureTransitionLeg ifLeg = null;
            if (refPointPrj != null)
            {
                var dynamicLegs = new List<dynamic>();
                foreach (var procedureTransition in transitoLegs)
                {
                    var segmentLeg = procedureTransition.TheSegmentLeg.GetFeature() as Aran.Aim.Features.SegmentLeg;

                    if (segmentLeg != null)
                    {
                        if (segmentLeg.Trajectory == null)
                        {
                            if (segmentLeg.LegTypeARINC.HasValue && segmentLeg.LegTypeARINC.Value == CodeSegmentPath.IF)
                                ifLeg = procedureTransition;
                            continue;
                        }
                        var trajectoryGeo =
                            Engine.Environment.Current.Geometry.ToPrj(segmentLeg.Trajectory.Geo);
                        try
                        {

                            //If LegTypeArinc is CF then return original order
                            if (trajectoryGeo.IsEmpty)
                                return transitoLegs;
                            var distToRefPoint = geoOperators.GetDistance(trajectoryGeo, refPointPrj);

                            dynamic expando = new ExpandoObject();
                            expando.Transition = procedureTransition;
                            expando.Distance = distToRefPoint;
                            expando.SegmentLeg = segmentLeg;
                            expando.TrajectoryGeo = trajectoryGeo;
                            dynamicLegs.Add(expando);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                }

                if (dynamicLegs.Count == 0) return result;

                dynamic firstLeg;
                if (procType == ProcedureType.StandardInstrumentDeparture)
                    firstLeg = dynamicLegs.OrderBy(tr => tr.Distance).ToList()[0];
                else
                    firstLeg = dynamicLegs.OrderByDescending(tr => tr.Distance).ToList()[0];

                result.Add(firstLeg.Transition);
                dynamicLegs.Remove(firstLeg);

                while (dynamicLegs.Count>0)
                {
                    var orderedLegs= dynamicLegs.OrderBy(
                            dynamicObj => geoOperators.GetDistance(firstLeg.TrajectoryGeo, dynamicObj.TrajectoryGeo)).ToList();

                    if (orderedLegs.Count == 0) break;
                    dynamic minDynamicObj = orderedLegs[0];

                    if (minDynamicObj != null)
                    {
                        result.Add(minDynamicObj.Transition);
                        dynamicLegs.Remove(minDynamicObj);
                        firstLeg = minDynamicObj;
                    }
                }
            }
            if (result.Count != transitoLegs.Count)
            {
                if(ifLeg!=null)
                    result.Insert(0,ifLeg);   
            }
            return result;
        }
    }

    public class TransitionLeg
    {
        public ProcedureTransitionLeg Original { get; private set; }
        public SegmentLeg SegmentLeg { get; }
        public bool IsEmpty { get; }
        public TransitionLeg(ProcedureTransitionLeg transtionLeg)
        {
            Original = transtionLeg;
            if (transtionLeg.TheSegmentLeg != null)
            {
                var segmentLegRef = transtionLeg.TheSegmentLeg as IAbstractFeatureRef;
                var segmentLeg = Engine.Environment.Current.DbProvider.GetFeature((Aran.Aim.FeatureType)segmentLegRef.FeatureTypeIndex, segmentLegRef.Identifier) as Aran.Aim.Features.SegmentLeg;
                if (segmentLeg != null)
                    SegmentLeg = new SegmentLeg(segmentLeg);
                else
                    IsEmpty = true;
            }
            else
                IsEmpty = true;
        }
    }

}
