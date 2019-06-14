using Aran.Aim.Features;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Aran.Aim.Enums;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using Aran.Queries;

namespace PVT.Model
{
    public class Procedure<T> : ProcedureBase where T : Procedure
    {

        public T Original { get; }
        protected static List<T> _procedures;

        public Procedure(T proc) : base(proc)
        {
            Original = proc;
            Name = Original.Name;
            if (Original.RNAV != null) RNAV = (bool) Original.RNAV;
            Aerodrome = new Airport(Engine.Environment.Current.DbProvider.GetAirport(Engine.Environment.Current.CurrentAeroport));
            DesignCriteria = Original.DesignCriteria.ToString();
            CodingStandard = Original.CodingStandard.ToString();
            TransitionNumber = Original.FlightTransition.Count;
            if (Original.AircraftCharacteristic?.Count > 0)
                AircraftCharacteristic = Original.AircraftCharacteristic[0].AircraftLandingCategory.Value.ToString();


            Transtions = new List<Transition>();
            for (var i = 0; i < TransitionNumber; i++)
            {
                var procType = Type;
                if (Original is StandardInstrumentDeparture)
                    procType = ProcedureType.StandardInstrumentDeparture;

                var transition = new Transition(Original.FlightTransition[i], procType);
                Transtions.Add(transition);
                if (transition.NotValid) NotValid = true;
            }


            if (Original.GuidanceFacility != null && Original.GuidanceFacility.Count > 0)
            {
                var navaid = Engine.Environment.Current.DbProvider.GetFeature(Aran.Aim.FeatureType.Navaid, Original.GuidanceFacility[0].Navaid.Identifier) as Aran.Aim.Features.Navaid;
                if (navaid != null)
                    Navaid = new Navaid(navaid);
            }

        }


        public static void Fetch()
        {
            _procedures = Engine.Environment.Current.DbProvider.GetProcedures<T>(Engine
                .Environment
                .Current.CurrentAeroport);
        }

    }

    public class ProcedureBase : Feature
    {
        public ProcedureType Type { get; protected set; }
        public bool NotValid { get; protected set; }
        public string Name { get; protected set; }
        public string Designator { get; protected set; }
        public Airport Aerodrome { get; protected set; }
        public string DesignCriteria { get; protected set; }
        public RunwayDirection RunwayDirection { get; protected set; }
        public string CodingStandard { get; protected set; }
        public int TransitionNumber { get; protected set; }
        public bool RNAV { get; protected set; }
        public List<Transition> Transtions { get; protected set; }
        public string AircraftCharacteristic { get; protected set; }
        public Navaid Navaid { get; protected set; }
        public string NavaidName => Navaid?.FullName;

        protected ProcedureBase(Aran.Aim.Features.Feature feature) : base(feature)
        {
            Reports = Engine.Environment.Current.DbProvider.GetFeatureReport(feature.Identifier).Select( x=> new FeatureReport(x)).ToList<FeatureReport>();
        }
    }

    public enum ProcedureType
    {
        InstrumentApproachProcedure,
        StandardInstrumentArrival,
        StandardInstrumentDeparture
    }
}
