
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractProcedureRefObject : AbstractFeatureRefObject <AbstractProcedureRef>
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractProcedureRefObject; }
		}
	}
}
