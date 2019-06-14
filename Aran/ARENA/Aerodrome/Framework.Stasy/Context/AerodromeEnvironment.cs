using Aerodrome.Enums;
using Aerodrome.Features;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Framework.Stasy.Helper;
using Framework.Stasy.SyncProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Stasy.Context
{
    public class AerodromeEnvironment
    {
        public AerodromeEnvironment()
        {
            DefaultValues = new List<FeatureDefaultValueInfo>();
            DefaultValues.Add(new FeatureDefaultValueInfo { NilReasonValue = NilReason.Null, ValueForString = "$Null", ValueForDouble = -32768, ValueForDateTime = new DateTime(1001, 01, 01) });
            DefaultValues.Add(new FeatureDefaultValueInfo { NilReasonValue = NilReason.Unknown, ValueForString = "$UNK", ValueForDouble = -32767});
            DefaultValues.Add(new FeatureDefaultValueInfo { NilReasonValue = NilReason.NotApplicable, ValueForString = "$NA", ValueForDouble = -32765, ValueForDateTime = new DateTime(1001, 01, 02) });
            DefaultValues.Add(new FeatureDefaultValueInfo { NilReasonValue = NilReason.NotEntered, ValueForString = "$NE", ValueForDouble = -32764, ValueForDateTime = new DateTime(1001, 01, 03) });

            HaccRequirementsValues = new Dictionary<Type, double>();
            HaccRequirementsValues.Add(typeof(AM_AerodromeReferencePoint), 30);
            HaccRequirementsValues.Add(typeof(AM_RunwayThreshold), 1);
            HaccRequirementsValues.Add(typeof(AM_RunwayCenterlinePoint), 1);
            HaccRequirementsValues.Add(typeof(AM_LandAndHoldShortOperationLocation), 0.5);
            HaccRequirementsValues.Add(typeof(AM_TaxiwayGuidanceLine), 0.5);
            HaccRequirementsValues.Add(typeof(AM_StandGuidanceLine), 0.5);
            HaccRequirementsValues.Add(typeof(AM_TaxiwayHoldingPosition), 0.5);
            HaccRequirementsValues.Add(typeof(AM_TaxiwayIntersectionMarking), 0.5);
            HaccRequirementsValues.Add(typeof(AM_RunwayExitLine), 0.5);            
            HaccRequirementsValues.Add(typeof(AM_ApronElement), 1);
            HaccRequirementsValues.Add(typeof(AM_DeicingArea), 1);
            HaccRequirementsValues.Add(typeof(AM_ParkingStandLocation), 1);
            HaccRequirementsValues.Add(typeof(AM_RunwayElement), 1);
            HaccRequirementsValues.Add(typeof(AM_Stopway), 1);
            HaccRequirementsValues.Add(typeof(AM_Blastpad), 1);
            HaccRequirementsValues.Add(typeof(AM_RunwayShoulder), 1);
            HaccRequirementsValues.Add(typeof(AM_TaxiwayElement), 1);
            HaccRequirementsValues.Add(typeof(AM_TaxiwayShoulder), 1);

            VaccRequirementsValues = new Dictionary<Type, double>();
            VaccRequirementsValues.Add(typeof(AM_AerodromeReferencePoint), 0.5);
            VaccRequirementsValues.Add(typeof(AM_RunwayThreshold), 0.25);
            VaccRequirementsValues.Add(typeof(AM_RunwayCenterlinePoint), 0.25);
            VaccRequirementsValues.Add(typeof(AM_TaxiwayGuidanceLine), 1);
            VaccRequirementsValues.Add(typeof(AM_StandGuidanceLine), 1);
        }

        #region UI properties 

        private IApplication m_application;
        public IApplication mxApplication
        {
            get { return m_application; }
            set { m_application = value; }
        }

        private IMap _mapControl;
        public IMap pMap
        {
            get { return _mapControl; }
            set
            {
                _mapControl = value;
            }
        }
        #endregion

        private Dictionary<Type, ITable> _tableDictionary;
        public Dictionary<Type, ITable> TableDictionary
        {
            get { return _tableDictionary ?? (_tableDictionary = new Dictionary<Type, ITable>()); }
            set { _tableDictionary = value; }
        }

        public List<FeatureDefaultValueInfo> DefaultValues { get; set; }

        public Dictionary<Type, double> VaccRequirementsValues { get; set; }

        public Dictionary<Type, double> HaccRequirementsValues { get; set; }

        public ApplicationContext Context;

        public GeoDbSyncProvider GeoDbProvider;

        public ISpatialReference SpatialReference { get; set; }

        public string MapDocumentName { get; set; }

        public string CurProjectName { get; set; }

        public string CurrentProjTempPath { get; set; }

        public bool ProjectNeedSaved { get; set; }

        public void FillAirtrackTableDic(IWorkspaceEdit workspaceEdit)
        {
            try
            {
                AerodromeDataCash.ProjectEnvironment.TableDictionary = new Dictionary<Type, ITable>();
                Dictionary<string, Type> tables = new Dictionary<string, Type>();

                Assembly asm = typeof(AM_AerodromeReferencePoint).Assembly;


                foreach (var amObj in Enum.GetValues(typeof(Feat_Type)))
                {
                    string featName = amObj.ToString().Replace("_", String.Empty);
                    Type amFeature = asm.GetType(asm.GetName().Name + ".AM_" + featName);

                    tables.Add(featName, amFeature);

                }



                foreach (var table in tables)
                {
                    if (EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, table.Key) != null)
                    {
                        AerodromeDataCash.ProjectEnvironment.TableDictionary.Add(table.Value,
                            EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, table.Key));
                    }
                }

            }
            catch (Exception ex)
            {

            }

        }

    }
}
