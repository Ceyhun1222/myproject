

namespace Aran.PANDA.Constants
{
	public class NamedConstantObject : AranObject, IAranCloneable
	{
		public NamedConstantObject()
		{
			Name = "";
			Unit = "";
			DefinedIn = "";
			Comment = "";
			Multiplier = 0.0;
			Assigned = false;
		}

		public string Name { get; protected set; }
		public string Unit { get; protected set; }
		public string DefinedIn { get; protected set; }
		public string Comment { get; protected set; }
		public double Multiplier { get; protected set; }
		public bool Assigned { get; protected set; }

		public void SetAssigned(bool value)
		{
			Assigned = value;
		}

		#region IAranCloneable Members

		public virtual AranObject Clone()
		{
			NamedConstantObject result = new NamedConstantObject();
			result.Assign(this);
			return result;
		}

		public void Assign(AranObject source)
		{
			NamedConstantObject result = (NamedConstantObject)source;
			Name = result.Name;
			Unit = result.Unit;
			Multiplier = result.Multiplier;
			DefinedIn = result.DefinedIn;
			Comment = result.Comment;
			Assigned = result.Assigned;
		}

		#endregion
	}
}
