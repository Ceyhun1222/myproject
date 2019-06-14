using Aran.Aim;
using Aran.Aim.BusinessRules;
using Aran.Aim.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SbvrParser.TesterConsole
{
    public class CustomDataCreator
    {
        public System.Collections.IList Create(FeatureType featType, List<Guid> guidList)
        {
            switch(featType)
            {
                case FeatureType.Airspace:
                    return CreateAirspaceList(guidList);
                case FeatureType.TouchDownLiftOff:
                    return CreateTouchDownLiftOff(guidList);
            }
            return null;
        }


        private System.Collections.IList CreateAirspaceList(List<Guid> guidList)
        {
            var featList = new List<Airspace>();
            for (var i = 1; i <= 10; i++)
            {
                var item = CreateAirspace(i);
                item.GeometryComponent.Add(new AirspaceGeometryComponent
                {
                    TheAirspaceVolume = new AirspaceVolume
                    {
                        ContributorAirspace = new AirspaceVolumeDependency
                        {
                            Id = i,
                            TheAirspace = new Aran.Aim.DataTypes.FeatureRef(Guid.NewGuid())
                        }
                    }
                });
                featList.Add(item);
            }

            featList[1].GeometryComponent[0].TheAirspaceVolume = null;
            featList[3].GeometryComponent[0].TheAirspaceVolume.ContributorAirspace.TheAirspace = null;
            guidList.Add(featList[3].Identifier);

            return featList;
        }

        private Airspace CreateAirspace(int index)
        {
            var airspace = new Airspace
            {
                Identifier = Guid.NewGuid(),
                Designator = "Airspace - " + index
            };

            return airspace;
        }

        private System.Collections.IList CreateTouchDownLiftOff(List<Guid> guidList)
        {
            var list = new List<TouchDownLiftOff>();
            list.Add(CreateTouchDownLiftOff(0));
            return list;
        }

        private TouchDownLiftOff CreateTouchDownLiftOff(int index)
        {
            var feat = new TouchDownLiftOff
            {
                Identifier = Guid.NewGuid(),
                Designator = "TouchDownLiftOff - " + index,
                AimingPoint = new ElevatedPoint
                {
                    HorizontalAccuracy = new Aran.Aim.DataTypes.ValDistance(3.5, Aran.Aim.Enums.UomDistance.FT)
                }
            };

            return feat;
        }
    }
}
