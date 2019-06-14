using System;
using System.Collections.Generic;
using Aran.Geometries;
using Aran.Aim.Features;

namespace Aran.PANDA.Conventional.Racetrack
{
	public delegate void NavaidListEventHandler ( object sender, NavaidListEventArg argNavList );
	public delegate void DsgPntListEventHandler ( object sender, DsgPntListEventArg argDsgPntList );
	public delegate void NavaidEventHandler ( object sender, NavaidEventArg argNavaid );
	public delegate void DsgPntEventHandler ( object sender, DsgPntEventArg argDsgPnt );
	public delegate void DirectionEventHandler ( object sender, DirectionEventArg argDir );
	public delegate void DistanceEventHandler ( object sender, DistanceEventArg argDist );
	public delegate void CommonEventHandler (object sender, CommonEventArg arg);
	public delegate void IntervalEventHandler (object sender, SpeedInterval interval);
	public delegate void AppliedValueChangedEventHandler ( bool isEqual );
    public delegate void CategoryListChangedEventHandler (List<string> categoryList);

    public class NavaidListEventArg : EventArgs
    {
        public NavaidListEventArg ( List<Navaid> navaidList )
        {
            NavaidList = navaidList;
        }

        public readonly List<Navaid> NavaidList;
    }

    public class DsgPntListEventArg : EventArgs
    {
        public DsgPntListEventArg ( List<DesignatedPoint> dsgPntList )
        {
            DsgPntList = dsgPntList;
        }

        public readonly List<DesignatedPoint> DsgPntList;
    }

    public class NavaidEventArg : EventArgs
    {
        public NavaidEventArg ( List<NavaidPntPrj> navPntPrjList)
        {
            NavaidPntPrjList = new List<NavaidPntPrj> ();
			foreach ( NavaidPntPrj navPntPrj in navPntPrjList )
				NavaidPntPrjList.Add ( navPntPrj );
        } 

        public readonly List<NavaidPntPrj> NavaidPntPrjList;
    }

    public class DsgPntEventArg : EventArgs
    {
        public DsgPntEventArg ( Point dsgPntPrj )
        {
            DsgPntPrj = dsgPntPrj;
        }

        public readonly Point DsgPntPrj;
    }

    public class DirectionEventArg : EventArgs
    {
        public DirectionEventArg ( double direction, double dirForGui )
        {
            Direction = direction;
            DirectionForGui = dirForGui;
        }

        public readonly double Direction;
        public readonly double DirectionForGui;
    }

    public class DistanceEventArg : EventArgs
    {
        public DistanceEventArg ( double nomDistInPrj, double nomDistForGui, double nomDistInGeo )
        {
            //DistanceInPrj = nomDistInPrj;
            DistanceForGui = nomDistForGui;
            //DistanceInGeo = nomDistInGeo;
        }

        //public double DistanceInPrj;
        //public double DistanceInGeo;
        public readonly double DistanceForGui;
    }

	public class CommonEventArg : EventArgs
	{
		public CommonEventArg ( double value )
		{
			Value = value;
		}

		public double Value
		{
			get; private set;
		}
	}
}
