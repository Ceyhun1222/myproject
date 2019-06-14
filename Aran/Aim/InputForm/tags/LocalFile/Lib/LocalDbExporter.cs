using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using System.IO;
using Aran.Aim.Package;
using Aran.Package;
using System.Xml;
using Aran.Aim.CAWProvider;
using Aran.Aim.Enums;

namespace Aran.Aim.InputFormLib
{
    public class LocalDbExporter
    {
        public LocalDbExporter ()
        {

        }

        public void Export (string dbFolderName, string xmlFileName)
        {
            List<string> allFilesList = new List<string> ();
            GetFiles (dbFolderName, allFilesList);

            List<Feature> featureList = new List<Feature> ();

            foreach (string fileName in allFilesList)
            {
                string dirName = Path.GetDirectoryName (fileName);
                string featureName = dirName.Substring (dirName.LastIndexOf ('\\') + 1);

                FeatureType featureType = (FeatureType) Enum.Parse (typeof (FeatureType), featureName);
                Feature feature = AimObjectFactory.CreateFeature (featureType);

                FileStream fs = File.Open (fileName, FileMode.Open);
                AranPackageReader pr = new AranPackageReader (fs);
                (feature as IPackable).Unpack (pr);
                fs.Close ();

                featureList.Add (feature);
            }

            WriteAllFeatureToXML (featureList, xmlFileName);
        }

        private void GetFiles (string dir, List<string> allFilesList)
        {
            string [] dirArr = Directory.GetDirectories (dir);
            foreach (string subDir in dirArr)
            {
                GetFiles (subDir, allFilesList);
            }

            string [] files = Directory.GetFiles (dir);
            allFilesList.AddRange (files);
        }

        private void WriteAllFeatureToXML (List<Feature> featureList, string xmlFileName)
        {
            XmlWriter xmlWriter = XmlWriter.Create (xmlFileName);

            AixmBasicMessage aixmBasicMessage = new AixmBasicMessage ();
            int writedAirspaceCount = 0;

            foreach (Feature feature in featureList)
            {
                if (feature.FeatureType == FeatureType.Airspace)
                {
                    if (writedAirspaceCount == 68)
                    {
                        writedAirspaceCount++;
                        continue;
                    }
                    else
                    {
                        writedAirspaceCount++;
                    }
                }

                if (feature is SegmentLeg)
                {
                    SegmentLeg sg = (SegmentLeg) feature;

                    try
                    {
                        if (sg.UpperLimitAltitude.Value == sg.LowerLimitAltitude.Value)
                        {
                            sg.VerticalAngle = null;
                        }
                    }
                    catch
                    {
                    }
                }

                feature.TimeSlice.Interpretation = TimeSliceInterpretationType.BASELINE;

                AixmFeatureList afl = new AixmFeatureList ();
                afl.Add (feature);
                aixmBasicMessage.HasMember.Add (afl);
            }

            aixmBasicMessage.WriteXml (xmlWriter);

            xmlWriter.Close ();
        }
    }
}
