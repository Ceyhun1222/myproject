using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using Aran.Package;
using Aran.PANDA.Common;

namespace Aran.Omega.SettingsUI
{
    public class OmegaSettings:IPackable
    {
        public const string SettingsKey = "Omega Settings";

        public OmegaSettings()
		{
            OLSModelList = new List<SettingsModel>();
		}

		public void Load(IAranEnvironment aranExtension)
		{
			if (aranExtension.HasExtKey(SettingsKey))
			{
				IPackable p = this;
				aranExtension.GetExtData(SettingsKey, p);
			}
		}

        public void Store(IAranEnvironment aranExtension)
		{
			IPackable p = this;
			aranExtension.PutExtData(SettingsKey, p);
		}

		#region IPackable Members

        public List<SettingsModel> OLSModelList { get; set; }

        public QueryModel OLSQuery { get; private set; }

        public InterfaceModel OLSInterface { get; private set; }

		public void Pack(PackageWriter writer)
		{
            writer.PutInt32(OLSModelList.Count);
            foreach (SettingsModel setModel in OLSModelList)
            {
                writer.PutInt32((int)setModel.Type);
                writer.PutString(setModel.ViewCaption);
                if (setModel.Type == MenuType.Query) 
                {
                    var queryModel = setModel as QueryModel;
                    if (queryModel == null) continue;
                    
                    writer.PutDouble(queryModel.Radius);
                    writer.PutString(queryModel.Organization.ToString());
                    writer.PutString(queryModel.Aeroport.ToString());
                    writer.PutBool(queryModel.ValidationReportIsCheked);
                }
                else if (setModel.Type == MenuType.Interface)
                {
                    var interfaceModel = setModel as InterfaceModel;
                    if (interfaceModel == null) continue;

                    writer.PutInt32((int)interfaceModel.DistanceUnit);
                    writer.PutDouble(interfaceModel.DistancePrecision);

                    writer.PutInt32((int)interfaceModel.HeightUnit);
                    writer.PutDouble(interfaceModel.HeightPrecision);
                }
                else if (setModel.Type == MenuType.Surface)
                {
                    var surfaceModel = setModel as SurfaceModel;
                    if (surfaceModel == null) continue;
                    
                    writer.PutInt32((int)surfaceModel.Surface);

                    writer.PutInt32(surfaceModel.Symbol.Color);
                    writer.PutInt32(surfaceModel.Symbol.Outline.Color);
                    writer.PutInt32(surfaceModel.Symbol.Outline.Size);

                    writer.PutInt32(surfaceModel.SelectedSymbol.Color);
                    writer.PutInt32(surfaceModel.SelectedSymbol.Outline.Color);
                    writer.PutInt32(surfaceModel.SelectedSymbol.Outline.Size);
                }
            }
		}

		public void Unpack(PackageReader reader)
		{
            var tmpSurfaceList = new List<SettingsModel>(); ;
            
            var modelCount = reader.GetInt32();

		    for (int i = 0; i < modelCount; i++)
            {
                var tmpModelType = (MenuType) reader.GetInt32();

                if (tmpModelType == MenuType.Query)
                 {
                     OLSQuery = new QueryModel();
                     OLSQuery.ViewCaption = reader.GetString();
                     OLSQuery.Radius = reader.GetDouble();
                     OLSQuery.Organization = Guid.Parse(reader.GetString());
                     OLSQuery.Aeroport = Guid.Parse(reader.GetString());
                     OLSQuery.ValidationReportIsCheked = reader.GetBool();
                     tmpSurfaceList.Add(OLSQuery);
                 }
                 else if (tmpModelType == MenuType.Interface)
                 {
                     OLSInterface = new InterfaceModel();
                     OLSInterface.ViewCaption = reader.GetString();
                     OLSInterface.DistanceUnit = (VerticalDistanceType)reader.GetInt32();
                     OLSInterface.DistancePrecision = reader.GetDouble();

                     OLSInterface.HeightUnit = (VerticalDistanceType)reader.GetInt32();
                     OLSInterface.HeightPrecision = reader.GetDouble();
                     tmpSurfaceList.Add(OLSInterface);
                 }

                 else if (tmpModelType == MenuType.Surface) 
                 {
                     var surfaceModel = new SurfaceModel();
                     surfaceModel.ViewCaption = reader.GetString();

                     surfaceModel.Surface = (PANDA.Constants.SurfaceType)reader.GetInt32();

                     surfaceModel.Symbol = new AranEnvironment.Symbols.FillSymbol();
                     surfaceModel.Symbol.Style = AranEnvironment.Symbols.eFillStyle.sfsVertical;
                     surfaceModel.Symbol.Color = reader.GetInt32();
                     
                     surfaceModel.Symbol.Outline = new AranEnvironment.Symbols.LineSymbol();
                     surfaceModel.Symbol.Outline.Color = reader.GetInt32();
                     surfaceModel.Symbol.Outline.Size = reader.GetInt32();
                     surfaceModel.Symbol.Outline.Style = AranEnvironment.Symbols.eLineStyle.slsDash;

                     surfaceModel.SelectedSymbol = new AranEnvironment.Symbols.FillSymbol();
                     surfaceModel.SelectedSymbol.Color = reader.GetInt32();
                     surfaceModel.SelectedSymbol.Style = AranEnvironment.Symbols.eFillStyle.sfsVertical;

                     surfaceModel.SelectedSymbol.Outline = new AranEnvironment.Symbols.LineSymbol();
                     surfaceModel.SelectedSymbol.Outline.Color = reader.GetInt32();
                     surfaceModel.SelectedSymbol.Outline.Size = reader.GetInt32();
                     surfaceModel.SelectedSymbol.Outline.Style = AranEnvironment.Symbols.eLineStyle.slsDash;
                     
                     tmpSurfaceList.Add(surfaceModel);
                 }
            }
		    OLSModelList = tmpSurfaceList;
			
		}
		#endregion

		
		private Guid _organization = Guid.Empty;
		private Guid _aeroport = Guid.Empty;
    }
}
