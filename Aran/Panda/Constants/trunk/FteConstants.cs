
namespace Aran.PANDA.Constants
{
   public class FteContant : NamedConstantObject
	{
		public FteContant()
			: base()
		{
			Value = 0.0;
		}

		public override AranObject Clone()
		{
			FteContant result = new FteContant();
			result.Assign(this);
			result.Value = Value;
			return result;
		}

		public double Value { get; protected set; }
	}
    
}

