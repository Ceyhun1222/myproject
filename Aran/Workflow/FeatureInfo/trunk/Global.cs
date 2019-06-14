using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Aim;
using System.Collections;
using Aran.Aim.Objects;

namespace Aran.Aim.FeatureInfo
{
    internal static class Global
    {
        public static List<BindingInfo> ToBindingInfoList (IAimObject aimObject)
        {
            List<BindingInfo> list = new List<BindingInfo> ();

            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex (aimObject);

            foreach (var propInfo in classInfo.Properties)
            {
				if (classInfo.AimObjectType == AimObjectType.Object && 
					propInfo.Index == (int) PropertyEnum.PropertyDBEntity.Id)
				{
					continue;
				}

                IAimProperty aimProp = aimObject.GetValue (propInfo.Index);

                if (aimProp != null &&
                    (aimProp.PropertyType != AimPropertyType.List ||
                    ((IList) aimProp).Count > 0))
                {
                    if (aimProp.PropertyType == AimPropertyType.List &&
                        propInfo.PropType.SubClassType == AimSubClassType.Choice)
                    {
                        IList propValList = aimProp as IList;
                        foreach (AObject choiceObj in propValList)
                        {
                            var bindInfo = BindingInfo.CreateInfo (propInfo, choiceObj);
                            if (bindInfo != null)
                                list.Add (bindInfo);
                        }
                    }
                    else
                    {
                        var bindInfo = BindingInfo.CreateInfo (propInfo, aimProp);
                        if (bindInfo != null)
                            list.Add (bindInfo);
                    }
                }
            }

			if (aimObject is Feature)
			{
				var feat = aimObject as Feature;
				if (feat.TimeSliceMetadata != null)
				{
					if (_metadataBindingInfo == null)
					{

						var propInfo = classInfo.Properties ["TimeSlice"];
						var tsPropInfo = propInfo.Clone ();
						tsPropInfo.Name = "Metadata";
						tsPropInfo.Tag = new Aran.Aim.Metadata.UI.UIPropInfo () {
							Caption = "Metadata"
						};

						tsPropInfo.PropType = new AimClassInfo () {
							AimObjectType = Aim.AimObjectType.Object,
							Tag = new Aran.Aim.Metadata.UI.UIClassInfo () {
								Caption = "Metadata"
							}
						};

						_metadataBindingInfo = BindingInfo.CreateInfo (tsPropInfo, feat.TimeSliceMetadata);
					}

					list.Insert (3, _metadataBindingInfo);
				}
			}

            return list;
        }

        public static string MakeSentence (string propName)
        {
            if (propName.Length > 0 && char.IsLower (propName [0]))
            {
                propName = char.ToUpper (propName [0]) + propName.Substring (1);
            }

            for (int i = 1; i < propName.Length - 1; i++)
            {
                if (char.IsUpper (propName [i]) && (char.IsLower (propName [i - 1]) || char.IsLower (propName [i + 1])))
                {
                    propName = propName.Insert (i, " ");
                    i++;
                }
            }

            return propName;
        }

        public static string DateTimeToString (DateTime dt)
        {
            //"yyyy-MM-dd T hh:mm:ss Z"
            return dt.ToString ("yyyy-MM-dd");
        }

        public static Feature GetFeature (FeatureType featureType, Guid identifier)
        {
            if (GettedFeature == null)
                return null;

            return GettedFeature (featureType, identifier);
        }

        public static GetFeatureHandler GettedFeature { get; set; }

        public static void DD2DMS (double val, out double xDeg, out double xMin, out double xSec, int Sign)
        {
            double x;
            double dx;

            x = Math.Abs (Math.Round (Math.Abs (val) * Sign, 10));

            xDeg = (int) x;
            dx = (x - xDeg) * 60;
            dx = Math.Round (dx, 8);
            xMin = (int) dx;
            xSec = (dx - xMin) * 60;
			if ( Math.Abs ( xSec - 60 ) <= CoordEpislon )
			{
				if ( xMin != 59 )
					xMin++;
				else
				{
					xDeg++;
					xMin = 0;
				}
				xSec = 0;
			}

			// xSec rounded to 2 as Vadims asked
			xSec = Math.Round (xSec, 2);
        }

        public static bool IsOSVErsionXP
        {
            get
            {
                if (_isOSVErsionXP != null)
                    return _isOSVErsionXP.Value;

                _isOSVErsionXP = false;

                OperatingSystem osInfo = System.Environment.OSVersion;

                // Determine the platform.
                switch (osInfo.Platform)
                {
                    // Platform is Windows 95, Windows 98, 
                    // Windows 98 Second Edition, or Windows Me.
                    case System.PlatformID.Win32Windows:

                        switch (osInfo.Version.Minor)
                        {
                            case 0:
                                Console.WriteLine ("Windows 95");
                                break;
                            case 10:
                                if (osInfo.Version.Revision.ToString () == "2222A")
                                    Console.WriteLine ("Windows 98 Second Edition");
                                else
                                    Console.WriteLine ("Windows 98");
                                break;
                            case 90:
                                Console.WriteLine ("Windows Me");
                                break;
                        }
                        break;

                    // Platform is Windows NT 3.51, Windows NT 4.0, Windows 2000,
                    // or Windows XP.
                    case System.PlatformID.Win32NT:

                        switch (osInfo.Version.Major)
                        {
                            case 3:
                                Console.WriteLine ("Windows NT 3.51");
                                break;
                            case 4:
                                Console.WriteLine ("Windows NT 4.0");
                                break;
                            case 5:
                                if (osInfo.Version.Minor == 0)
                                    Console.WriteLine ("Windows 2000");
                                else
                                {
                                    Console.WriteLine ("Windows XP");
                                    _isOSVErsionXP = true;
                                }
                                break;
                        } break;
                }

                return _isOSVErsionXP.Value;
            }
        }

        private static bool? _isOSVErsionXP;
		private static BindingInfo _metadataBindingInfo;
		private static readonly double CoordEpislon = 0.0001;

		public static BindingInfo MetadataBindingInfo
		{
			get { return _metadataBindingInfo; }
		}
    }
}