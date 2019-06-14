using System;
using Aran.Aim.Features;
using Aran.Geometries;
using GalaSoft.MvvmLight;

namespace Aran.PANDA.LegCreator.Model
{
	public class Leg : ObservableObject
	{
		private Guid _id;
		private string _timeSlice;
		private string _endConditionDsg;
		private string _legPath;
		private string _legTypeArinc;
		private string _courseDirection;
		private string _courseType;
		private double _course;
		private string _type;
		private SegmentLeg _source;
		private uint _seqNumber;
		private string _transType;
		private MultiLineString _geo;
	    private ProcedureTransition _transition
            ;

	    public string TimeSlice
		{
			get
			{
				return _timeSlice;
			}
			set
			{
				Set ( ( ) => TimeSlice, ref _timeSlice, value );
			}
		}

		public Guid Id
		{
			get
			{
				return _id;
			}
			set
			{
				Set ( ( ) => Id, ref _id, value );
			}
		}

		public string Type
		{
			get
			{
				return _type;
			}
			set
			{
				Set ( ( ) => Type, ref _type, value );
			}
		}

		public string TransitionType
		{
			get
			{
				return _transType;
			}
			set
			{
				Set ( ( ) => TransitionType, ref _transType, value );
			}
		}

		public uint SequenceNumber
		{
			get
			{
				return _seqNumber;
			}
			set
			{
				Set ( ( ) => SequenceNumber, ref _seqNumber, value );
			}
		}

		public string LegTypeArinc
		{
			get
			{
				return _legTypeArinc;
			}
			set
			{
				Set ( ( ) => LegTypeArinc, ref _legTypeArinc, value );
			}
		}

		public string EndConditionDesignator
		{
			get
			{
				return _endConditionDsg;
			}
			set
			{
				Set ( ( ) => EndConditionDesignator, ref _endConditionDsg, value );
			}
		}

		public string LegPath
		{
			get
			{
				return _legPath;
			}
			set
			{
				Set ( ( ) => LegPath, ref _legPath, value );
			}
		}

		public double Course
		{
			get
			{
				return _course;
			}
			set
			{
				Set ( ( ) => Course, ref _course, value );
			}
		}

		public string CourseType
		{
			get
			{
				return _courseType;
			}
			set
			{
				Set ( ( ) => CourseType, ref _courseType, value );
			}
		}

		public string CourseDirection
		{
			get
			{
				return _courseDirection;
			}
			set
			{
				Set ( ( ) => CourseDirection, ref _courseDirection, value );
			}
		}

		internal void SetSourceSegmentLeg ( SegmentLeg source )
		{
			_source = source;
		}

		internal SegmentLeg GetSourceSegmentLeg ( )
		{
			return _source;
		}

		internal void SetProjGeo ( MultiLineString geo )
		{
			_geo = geo;
		}

		internal MultiLineString GetProjGeo ( )
		{
			return _geo;
		}

	    public void SetTransition(ProcedureTransition flight)
	    {
	        _transition = flight;
	    }

	    public ProcedureTransition GetTransition()
	    {
	        return _transition;
	    }
	}
}
