using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;
using Aran.Panda.Common;
using ESRI.ArcGIS.esriSystem;

namespace Aran.Omega.TypeB.Settings
{
    public class TypeBSettings
    {
        public const string SettingsKey = "TypeB Settings";

        public TypeBSettings()
		{
            OLSModelList = new List<SettingsModel>();
            DbConnection = new DbConnectionModel();
		}

		public void Load(IVariantStream stream)
		{
            Unpack(stream);
		}

		public void Store(IVariantStream stream)
		{
			Pack(stream);
		}

		#region IPackable Members

        public List<SettingsModel> OLSModelList { get; set; }

        public QueryModel OLSQuery { get; set; }

        public InterfaceModel OLSInterface { get; set; }
        public DbConnectionModel DbConnection { get; set; }

		public void Pack(IVariantStream writer)
		{
            writer.Write(OLSModelList.Count);
            foreach (SettingsModel setModel in OLSModelList)
            {
                writer.Write((int)setModel.Type);
                writer.Write(setModel.ViewCaption);
                if (setModel.Type == MenuType.Query) 
                {
                    var queryModel = setModel as QueryModel;
                    if (queryModel == null) continue;
                    
                    writer.Write(queryModel.Radius);
                    writer.Write(queryModel.Organization.ToString());
                    writer.Write(queryModel.Aeroport.ToString());
                    writer.Write(queryModel.ValidationReportIsCheked);
                }
                else if (setModel.Type == MenuType.Interface)
                {
                    var interfaceModel = setModel as InterfaceModel;
                    if (interfaceModel == null) continue;

                    writer.Write((int)interfaceModel.DistanceUnit);
                    writer.Write(interfaceModel.DistancePrecision);

                    writer.Write((int)interfaceModel.HeightUnit);
                    writer.Write(interfaceModel.HeightPrecision);
                }
                else if (setModel.Type == MenuType.Surface)
                {
                    var surfaceModel = setModel as SurfaceModel;
                    if (surfaceModel == null) continue;
                    
                    writer.Write((int)surfaceModel.Surface);

                    writer.Write(surfaceModel.Symbol.Color);
                    writer.Write(surfaceModel.Symbol.Outline.Color);
                    writer.Write(surfaceModel.Symbol.Outline.Size);

                    writer.Write(surfaceModel.SelectedSymbol.Color);
                    writer.Write(surfaceModel.SelectedSymbol.Outline.Color);
                    writer.Write(surfaceModel.SelectedSymbol.Outline.Size);
                }
            }
            
            if (!DbConnection.IsEmpty) 
            {
                writer.Write(true);
                writer.Write(DbConnection.Host);
                writer.Write(DbConnection.Port);
                writer.Write(DbConnection.DbName);

                writer.Write(DbConnection.UserName);
                writer.Write(DbConnection.Password);
            
            }
		}

		public void Unpack(IVariantStream reader)
		{
            var tmpSurfaceList = new List<SettingsModel>(); ;
            try
            {
                var modelCount = (int)reader.Read();

                for (int i = 0; i < modelCount; i++)
                {
                    var tmpModelType = (MenuType)reader.Read();

                    if (tmpModelType == MenuType.Query)
                    {
                        OLSQuery = new QueryModel();
                        OLSQuery.ViewCaption = (string)reader.Read();
                        OLSQuery.Radius = (double)reader.Read();
                        OLSQuery.Organization = Guid.Parse((string)reader.Read());
                        OLSQuery.Aeroport = Guid.Parse((string)reader.Read());
                        OLSQuery.ValidationReportIsCheked = (bool)reader.Read();
                        tmpSurfaceList.Add(OLSQuery);
                    }
                    else if (tmpModelType == MenuType.Interface)
                    {
                        OLSInterface = new InterfaceModel();
                        OLSInterface.ViewCaption = (string)reader.Read();
                        OLSInterface.DistanceUnit = (VerticalDistanceType)reader.Read();
                        OLSInterface.DistancePrecision = (double)reader.Read();

                        OLSInterface.HeightUnit = (VerticalDistanceType)reader.Read();
                        OLSInterface.HeightPrecision = (double)reader.Read();
                        tmpSurfaceList.Add(OLSInterface);
                    }

                    else if (tmpModelType == MenuType.Surface)
                    {
                        var surfaceModel = new SurfaceModel();
                        surfaceModel.ViewCaption = (string)reader.Read();

                        surfaceModel.Surface = (Panda.Constants.SurfaceType)reader.Read();

                        surfaceModel.Symbol = new AranEnvironment.Symbols.FillSymbol();
                        surfaceModel.Symbol.Style = AranEnvironment.Symbols.eFillStyle.sfsVertical;
                        surfaceModel.Symbol.Color = (int)reader.Read();

                        surfaceModel.Symbol.Outline = new AranEnvironment.Symbols.LineSymbol();
                        surfaceModel.Symbol.Outline.Color = (int)reader.Read();
                        surfaceModel.Symbol.Outline.Size = (int)reader.Read();
                        surfaceModel.Symbol.Outline.Style = AranEnvironment.Symbols.eLineStyle.slsDash;

                        surfaceModel.SelectedSymbol = new AranEnvironment.Symbols.FillSymbol();
                        surfaceModel.SelectedSymbol.Color = (int)reader.Read();
                        surfaceModel.SelectedSymbol.Style = AranEnvironment.Symbols.eFillStyle.sfsVertical;

                        surfaceModel.SelectedSymbol.Outline = new AranEnvironment.Symbols.LineSymbol();
                        surfaceModel.SelectedSymbol.Outline.Color = (int)reader.Read();
                        surfaceModel.SelectedSymbol.Outline.Size = (int)reader.Read();
                        surfaceModel.SelectedSymbol.Outline.Style = AranEnvironment.Symbols.eLineStyle.slsDash;

                        tmpSurfaceList.Add(surfaceModel);
                    }

                }
                OLSModelList = tmpSurfaceList;
                try
                {
                    bool isDbConnection = (bool)reader.Read();
                    if (isDbConnection)
                    {
                        DbConnection = new DbConnectionModel();
                        DbConnection.Host = (string)reader.Read();
                        DbConnection.Port = (int)reader.Read();
                        DbConnection.DbName = (string)reader.Read();
                        DbConnection.UserName = (string)reader.Read();
                        DbConnection.Password = (string)reader.Read();
                        DbConnection.IsEmpty = false;

                    }
                }
                catch (Exception)
                {
                }
                
                
            }
            catch (Exception)
            {
                OLSModelList = tmpSurfaceList;
                //throw;
            }
		}
		#endregion

		
		private Guid _organization = Guid.Empty;
		private Guid _aeroport = Guid.Empty;
    }
}
