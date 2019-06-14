using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;
using Aran.PANDA.Common;

namespace Aran.Delta.Settings.model
{
    public enum SnapClassType 
    {
        DesignatedPoint,
        Airspace,
        RouteSegment,
        Route_Intersects,
        Navaids
    }

    public class SnapModel:IPackable
    {
        private readonly string[] classNames = new string[]{
            "Designated Point","Airspace","Route Segment",
            "Route Intersects","Navaids"};

        public SnapModel()
        {
            SnapClassList = new List<SnapClass>();
            for (int i = 0; i < Enum.GetValues(typeof(SnapClassType)).Length; i++)
            {
                var snapClass = new SnapClass();
                snapClass.Name = classNames[i];
                snapClass.Type = (SnapClassType)i;
                snapClass.IsSelected = false;
                SnapClassList.Add(snapClass);
            }
          
        }
        public int Tolerance { get; set; }
        public IList<SnapClass> SnapClassList { get; set; }

        public void Pack(PackageWriter writer)
        {
            writer.PutInt32((int)Tolerance);
            writer.PutInt32((int)SnapClassList.Count);
            foreach (var snapClass in SnapClassList)
            {
                writer.PutString(snapClass.Name);
                writer.PutInt32((int)snapClass.Type);
                writer.PutBool(snapClass.IsSelected);
            }
        }

        public void Unpack(PackageReader reader)
        {
            try
            {
                Tolerance = reader.GetInt32();

                SnapClassList = new List<SnapClass>();
                int snapClassCount = reader.GetInt32();
                for (int i = 0; i < snapClassCount; i++)
                {
                    SnapClass snapClass = new SnapClass();
                    snapClass.Name = reader.GetString();
                    snapClass.Type = (SnapClassType)reader.GetInt32();
                    snapClass.IsSelected = reader.GetBool();
                    SnapClassList.Add(snapClass);
                }
            }
                //If exception is occured then it is new project
            catch (Exception)
            {
               
                
            }
        }

        private void CreateDefaultParam() 
        {
            Tolerance = 5;
            SnapClassList = new List<SnapClass>();
            
        }
    }

    public class SnapClass 
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public SnapClassType Type { get; set; }
    }
}
