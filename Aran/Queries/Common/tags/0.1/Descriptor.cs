using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;

namespace Aran.Queries
{
	public class Descriptor : IPackable
	{
		public Descriptor()
			: this(Guid.Empty, string.Empty)
		{
		}

		public Descriptor(Guid identifier, string name) 
		{
			Identifier = identifier;
			Name = name;
		}

		public Guid Identifier { get; set;}

		public string Name { get; set; }

		public bool IsEmpty
		{
			get { return Identifier == Guid.Empty && Name == string.Empty; }
		}

        public override string ToString ()
        {
            if (Name == null)
                return base.ToString ();

            return Name;
        }

        #region IPackable Members

        public void Pack ( PackageWriter writer )
        {
            writer.PutString ( Identifier.ToString () );
            bool isNotNull = (Name != null);
            writer.PutBool (isNotNull);
            if (isNotNull)
                writer.PutString ( Name );
        }

        public void Unpack ( PackageReader reader )
        {
            Identifier = new Guid ( reader.GetString () );
            bool isNotNull = reader.GetBool ();
            if (isNotNull)
                Name = reader.GetString ();
        }

        #endregion
    }
}
