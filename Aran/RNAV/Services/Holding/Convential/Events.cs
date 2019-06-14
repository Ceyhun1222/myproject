using System;
using System.Collections.Generic;
using Delib.Classes.Features.Organisation;
using Delib.Classes.Features.AirportHeliport;
using Delib.Classes.Features.Navaid;
using ARAN.GeometryClasses;

namespace Holding.Convential
{
    public delegate void EventHandlerOrgList ( object sender, EventArgOrgList argOrgList );
    public delegate void EventHandlerArpList ( object sender, EventArgAirportList argArpList );
    public delegate void EventHandlerNavList ( object sender, EventArgNavaidList argNavList );
    public delegate void EventHandlerArp ( object sender, EventArgAirport argAirport );
    public delegate void EventHandlerDsgPntList ( object sender, EventArgDsgPntList argDsgPntList );
    public delegate void EventHandlerNav ( object sender, EventArgNavaid argNavaid );
    public delegate void EventHandlerDsgPnt ( object sender, EventArgDsgPnt argDsgPnt );
    public delegate void EventHandlerDirection ( object sender, EventArgDirection argDir );
    public delegate void EventHandlerDistance ( object sender, EventArgNomDistance argDist );
    public delegate void EventHandlerIASInterval ( object sender, EventArgIASInterval argIASInterval );
    public delegate void EventHandlerCategoryList ( object sender, EventArgCategoryList argCatList );
    public delegate void EventHandlerTime ( object sender, EventArgTime argTime );
    public delegate void EventHandlerLimitingDistance ( object sender, EventArgLimitingDistance argLimDist );

    public class EventArgOrgList : EventArgs
    {
        public EventArgOrgList ( List<OrganisationAuthority> orgList )
        {
            OrganisationList = orgList;
        }

        public List<OrganisationAuthority> OrganisationList;
    }

    public class EventArgAirportList : EventArgs
    {
        public EventArgAirportList ( List<AirportHeliport> arpList )
        {
            AirportList = arpList;
        }

        public List<AirportHeliport> AirportList;
    }

    public class EventArgNavaidList : EventArgs
    {
        public EventArgNavaidList ( List<Navaid> navaidList )
        {
            NavaidList = navaidList;
        }

        public List<Navaid> NavaidList;
    }

    public class EventArgAirport : EventArgs
    {
        public EventArgAirport ( AirportHeliport airport )
        {
            Airport = airport;
        }

        public AirportHeliport Airport;
    }

    public class EventArgTmp : EventArgs
    {
        public EventArgTmp ( string val )
        {
            Value = val;
        }

        public string Value;
    }

    public class EventArgDsgPntList : EventArgs
    {
        public EventArgDsgPntList ( List<DesignatedPoint> dsgPntList )
        {
            DsgPntList = dsgPntList;
        }

        public List<DesignatedPoint> DsgPntList;
    }

    public class EventArgNavaid : EventArgs
    {
        public EventArgNavaid ( /* ProcedureTypeConv procTypeConv, */List<NavaidPntPrj> navPntPrjList)
        {
            //ProcTypeConv = procTypeConv;
            NavaidPntPrjList = new List<NavaidPntPrj> ();
            foreach ( NavaidPntPrj navPntPrj in navPntPrjList)
                NavaidPntPrjList.Add(navPntPrj.Clone ());
        }

        public List<NavaidPntPrj> NavaidPntPrjList;
        //public ProcedureTypeConv ProcTypeConv;
    }

    public class EventArgDsgPnt : EventArgs
    {
        public EventArgDsgPnt ( Point dsgPntPrj )
        {
            DsgPntPrj = dsgPntPrj;
        }

        public Point DsgPntPrj;
    }

    public class EventArgDirection : EventArgs
    {
        public EventArgDirection ( double direction, double dirForGUI )
        {
            Direction = direction;
            DirectionForGUI = dirForGUI;
        }

        public double Direction;
        public double DirectionForGUI;
    }

    public class EventArgNomDistance : EventArgs
    {
        public EventArgNomDistance ( double nomDistInPrj, double nomDistForGUI, double nomDistInGeo )
        {
            NomDistanceInPrj = nomDistInPrj;
            NomDistanceForGUI = nomDistForGUI;
            NomDistanceInGeo = nomDistInGeo;
        }

        public double NomDistanceInPrj;
        public double NomDistanceInGeo;
        public double NomDistanceForGUI;
    }

    public class EventArgIASInterval : EventArgs
    {
        public EventArgIASInterval ( double min, double max )
        {
            Min = min;
            Max = max;
        }

        public double Min, Max;
    }

    public class EventArgCategoryList : EventArgs
    {
        public EventArgCategoryList ( List<string> categoryList )
        {
            CategoryList = categoryList;
        }

        public List<string> CategoryList;
    }

    public class EventArgTime : EventArgs
    {
        public EventArgTime ( double time )
        {
            Time = time;
        }

        public double Time;
    }

    public class EventArgLimitingDistance : EventArgs
    {
        public EventArgLimitingDistance ( double min, double max )
        {
            Min = min;
            Max = max;
        }

        public double Min, Max;
    }
}
