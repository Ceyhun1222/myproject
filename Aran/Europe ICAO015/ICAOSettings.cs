using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using Aran.Package;
using Aran.PANDA.Common;

namespace Europe_ICAO015
{
    public class ICAOSettings : IPackable
    {
        public const string SettingsKey = "Omega Settings";

        public ICAOSettings()
        {
            OLSModelList = new List<Aran.Omega.SettingsUI.SettingsModel>();
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

        public List<Aran.Omega.SettingsUI.SettingsModel> OLSModelList { get; set; }

        public Aran.Omega.SettingsUI.QueryModel OLSQuery { get; private set; }

        public Aran.Omega.SettingsUI.InterfaceModel OLSInterface { get; private set; }

        public void Pack(PackageWriter writer)
        {
            writer.PutInt32(OLSModelList.Count);
            foreach (Aran.Omega.SettingsUI.SettingsModel setModel in OLSModelList)
            {
                writer.PutInt32((int)setModel.Type);
                writer.PutString(setModel.ViewCaption);
                if (setModel.Type == Aran.Omega.SettingsUI.MenuType.Query)
                {
                    var queryModel = setModel as Aran.Omega.SettingsUI.QueryModel;
                    if (queryModel == null) continue;

                    writer.PutDouble(queryModel.Radius);
                    writer.PutString(queryModel.Organization.ToString());
                    writer.PutString(queryModel.Aeroport.ToString());
                    writer.PutBool(queryModel.ValidationReportIsCheked);
                }
                else if (setModel.Type == Aran.Omega.SettingsUI.MenuType.Interface)
                {
                    var interfaceModel = setModel as Aran.Omega.SettingsUI.InterfaceModel;
                    if (interfaceModel == null) continue;

                    writer.PutInt32((int)interfaceModel.DistanceUnit);
                    writer.PutDouble(interfaceModel.DistancePrecision);

                    writer.PutInt32((int)interfaceModel.HeightUnit);
                    writer.PutDouble(interfaceModel.HeightPrecision);
                }
                else if (setModel.Type == Aran.Omega.SettingsUI.MenuType.Surface)
                {
                    var surfaceModel = setModel as Aran.Omega.SettingsUI.SurfaceModel;
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
            var tmpSurfaceList = new List<Aran.Omega.SettingsUI.SettingsModel>(); ;

            var modelCount = reader.GetInt32();

            for (int i = 0; i < modelCount; i++)
            {
                var tmpModelType = (Aran.Omega.SettingsUI.MenuType)reader.GetInt32();

                if (tmpModelType == Aran.Omega.SettingsUI.MenuType.Query)
                {
                    OLSQuery = new Aran.Omega.SettingsUI.QueryModel();
                    OLSQuery.ViewCaption = reader.GetString();
                    OLSQuery.Radius = reader.GetDouble();
                    OLSQuery.Organization = Guid.Parse(reader.GetString());
                    OLSQuery.Aeroport = Guid.Parse(reader.GetString());
                    OLSQuery.ValidationReportIsCheked = reader.GetBool();
                    tmpSurfaceList.Add(OLSQuery);
                }
                else if (tmpModelType == Aran.Omega.SettingsUI.MenuType.Interface)
                {
                    OLSInterface = new Aran.Omega.SettingsUI.InterfaceModel();
                    OLSInterface.ViewCaption = reader.GetString();
                    OLSInterface.DistanceUnit = (VerticalDistanceType)reader.GetInt32();
                    OLSInterface.DistancePrecision = reader.GetDouble();

                    OLSInterface.HeightUnit = (VerticalDistanceType)reader.GetInt32();
                    OLSInterface.HeightPrecision = reader.GetDouble();
                    tmpSurfaceList.Add(OLSInterface);
                }

                else if (tmpModelType == Aran.Omega.SettingsUI.MenuType.Surface)
                {
                    var surfaceModel = new Aran.Omega.SettingsUI.SurfaceModel();
                    surfaceModel.ViewCaption = reader.GetString();

                    surfaceModel.Surface = (Aran.PANDA.Constants.SurfaceType)reader.GetInt32();

                    surfaceModel.Symbol = new Aran.AranEnvironment.Symbols.FillSymbol();
                    surfaceModel.Symbol.Style = Aran.AranEnvironment.Symbols.eFillStyle.sfsVertical;
                    surfaceModel.Symbol.Color = reader.GetInt32();

                    surfaceModel.Symbol.Outline = new Aran.AranEnvironment.Symbols.LineSymbol();
                    surfaceModel.Symbol.Outline.Color = reader.GetInt32();
                    surfaceModel.Symbol.Outline.Size = reader.GetInt32();
                    surfaceModel.Symbol.Outline.Style = Aran.AranEnvironment.Symbols.eLineStyle.slsDash;

                    surfaceModel.SelectedSymbol = new Aran.AranEnvironment.Symbols.FillSymbol();
                    surfaceModel.SelectedSymbol.Color = reader.GetInt32();
                    surfaceModel.SelectedSymbol.Style = Aran.AranEnvironment.Symbols.eFillStyle.sfsVertical;

                    surfaceModel.SelectedSymbol.Outline = new Aran.AranEnvironment.Symbols.LineSymbol();
                    surfaceModel.SelectedSymbol.Outline.Color = reader.GetInt32();
                    surfaceModel.SelectedSymbol.Outline.Size = reader.GetInt32();
                    surfaceModel.SelectedSymbol.Outline.Style = Aran.AranEnvironment.Symbols.eLineStyle.slsDash;

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
