using Aran.Aim.Enums;
using Aran.Aim.Features;
using GalaSoft.MvvmLight;

namespace Aran.PANDA.LegCreator.Model
{
	public class Transition : ObservableObject
	{
		private string _id;
		private ProcedureTransition _source;
		private CodeProcedurePhase _type;
		private string _instruction;
		private string _departureRunwayDirection;

		public string TransitionId
		{
			get
			{
				return _id;
			}
			set
			{
				Set ( ( ) => TransitionId, ref _id, value );
			}
		}

		public CodeProcedurePhase Type
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

		public string Instruction
		{
			get
			{
				return _instruction;
			}
			set
			{
				Set ( ( ) => Instruction, ref _instruction, value );
			}
		}

		public string DepartureRunwayDirection
		{
			get
			{
				return _departureRunwayDirection;
			}
			set
			{
				Set ( ( ) => DepartureRunwayDirection, ref _departureRunwayDirection, value );
			}
		}

		public void SetSourceTransition ( ProcedureTransition source )
		{
			_source = source;
		}

		public ProcedureTransition GetSourceTransition ( )
		{
			return _source;
		}
	}
}