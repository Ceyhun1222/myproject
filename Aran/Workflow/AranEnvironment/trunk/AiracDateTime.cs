using Aran.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.AranEnvironment
{
    public struct AiracDateTime : IPackable
    {
        public AiracSelectionMode Mode { get; set; }

        public DateTime Value { get; set; }


        public static implicit operator AiracDateTime (DateTime dt)
        {
            var adt = new AiracDateTime();
            adt.Value = dt;
            adt.Mode = AiracSelectionMode.Custom;
            return adt;
        }

        public static implicit operator DateTime(AiracDateTime adt)
        {
            return adt.Value;
        }

        public void Pack(PackageWriter writer)
        {
            writer.PutEnum<AiracSelectionMode>(Mode);
            writer.PutDateTime(Value);
        }

        public void Unpack(PackageReader reader)
        {
            Mode = reader.GetEnum<AiracSelectionMode>();
            Value = reader.GetDateTime();
        }
    }

    public enum AiracSelectionMode
    {
        Airac,
        Custom
    }
}
