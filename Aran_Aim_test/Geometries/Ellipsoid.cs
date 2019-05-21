using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;

namespace Aran.Geometries.SpatialReferences
{
	public class Ellipsoid : IPackable
	{
		public SpatialReferenceGeoType SrGeoType{get;set;}
        public double SemiMajorAxis { get; set; }
        public double Flattening { get; set; }
        public bool IsValid { get; set; }

		public Ellipsoid()
		{
			SrGeoType = SpatialReferenceGeoType.srgtWGS1984;
			SemiMajorAxis = 0;
			Flattening = 0;
			IsValid = false;
		}


        public void Pack (PackageWriter writer)
        {
            writer.PutEnum<SpatialReferenceGeoType> (SrGeoType);
            writer.PutDouble (SemiMajorAxis);
            writer.PutDouble (Flattening);
            writer.PutBool (IsValid);
        }

        public void Unpack (PackageReader reader)
        {
            SrGeoType = reader.GetEnum<SpatialReferenceGeoType> ();
            SemiMajorAxis = reader.GetDouble ();
            Flattening = reader.GetDouble ();
            IsValid = reader.GetBool ();
        }
    }
}
