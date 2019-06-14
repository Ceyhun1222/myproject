using Aran.Aim.Enums;

namespace Aran.Panda.Conventional.Racetrack
{
	public class Procedure
	{
		public Procedure ( FixFacilities navaidList )
		{
			_navaids = navaidList;
		}

		public void SetType ( ProcedureTypeConv value )
		{
			if ( value == _type )
				return;
			Type = value;
		}

		public ProcedureTypeConv Type
		{
			get
			{
				return _type;
			}
			private set
			{
				_type = value;
				if ( _type == ProcedureTypeConv.VORDME )
				{
					_navaids.SetServiceTypes ( new CodeNavaidService [] { CodeNavaidService.VOR_DME }, _type );
				}
				else if ( _type == ProcedureTypeConv.VOR_NDB )
				{

					_navaids.SetServiceTypes ( new CodeNavaidService [] {   CodeNavaidService.VOR, CodeNavaidService.NDB, 
                                                                            CodeNavaidService.VOR_DME, CodeNavaidService.VORTAC, 
                                                                            CodeNavaidService.NDB_DME, CodeNavaidService.NDB_MKR },
																			_type );
				}
				else if ( _type == ProcedureTypeConv.VORVOR )
				{
					_navaids.SetServiceTypes ( new CodeNavaidService [] { CodeNavaidService.VOR, CodeNavaidService.VOR_DME, CodeNavaidService.VORTAC }, _type );
				}
			}
		}

		private ProcedureTypeConv _type = ProcedureTypeConv.NONE;
		private FixFacilities _navaids;
	}
}