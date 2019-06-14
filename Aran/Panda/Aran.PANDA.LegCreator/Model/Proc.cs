using System;
using System.Collections.ObjectModel;
using Aran.Aim.Features;
using GalaSoft.MvvmLight;

namespace Aran.PANDA.LegCreator.Model
{
	public class Proc : ObservableObject
	{
		private Guid _id;
		private string _timeSlice;
		private string _dsg;
		private string _name;
		private string _codingStnd;
		private bool _isRnav;
		private string _dsgnCriteria;
		private Procedure _sourceProc;
		private ObservableCollection<Leg> _legs;

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

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				Set ( ( ) => Name, ref _name, value );
			}
		}

		public string Designator
		{
			get
			{
				return _dsg;
			}
			set
			{
				Set ( ( ) => Designator, ref _dsg, value );
			}
		}

		public string DesignCriteria
		{
			get
			{
				return _dsgnCriteria;
			}
			set
			{
				Set ( ( ) => DesignCriteria, ref _dsgnCriteria, value );
			}
		}

		public bool IsRnav
		{
			get
			{
				return _isRnav;
			}
			set
			{
				Set ( ( ) => IsRnav, ref _isRnav, value );
			}
		}

		public string CodingStandard
		{
			get
			{
				return _codingStnd;
			}
			set
			{
				Set ( ( ) => CodingStandard, ref _codingStnd, value );
			}
		}

		internal void SetSourceProcedure ( Procedure proc)
		{
			_sourceProc = proc;
		}

		internal Procedure GetSourceProcedure ( )
		{
			return _sourceProc;
		}

		internal void SetLegs ( ObservableCollection<Leg> legs )
		{
			_legs = legs;
		}

		internal ObservableCollection<Leg> GetLegs ( )
		{
			return _legs;
		}
	}
}