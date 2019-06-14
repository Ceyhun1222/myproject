using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.AranEnvironment;
using Aran.Delta.Settings;
using Aran.Delta.Settings.model;
using Aran.Package;
using Aran.PANDA.Common;

namespace Aran.Delta.Settings
{
    public class DeltaSettings : IPackable
    {
        public DeltaSettings()
        {
            DeltaQuery = new QueryModel();
            DeltaInterface = new InterfaceModel();
            DeltaSnapModel = new SnapModel();
            SymbolModel = new SymbolModel();
        }
        public const string SettingsKey = "Delta Settings";

        public QueryModel DeltaQuery { get; private set; }

        public InterfaceModel DeltaInterface { get; private set; }

        public SnapModel DeltaSnapModel { get; set; }

        public SymbolModel SymbolModel { get; set; }


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

        public void Pack(PackageWriter writer)
        {
            //Query
            writer.PutDouble(DeltaQuery.Radius);
            writer.PutString(DeltaQuery.Organization.ToString());
            writer.PutString(DeltaQuery.Aeroport.ToString());
            
            //InterfaceParam
            writer.PutInt32((int) DeltaInterface.DistanceUnit);
            writer.PutDouble(DeltaInterface.DistancePrecision);
            
            writer.PutInt32((int) DeltaInterface.HeightUnit);
            writer.PutDouble(DeltaInterface.HeightPrecision);
            
            writer.PutInt32((int)DeltaInterface.CoordinateUnit);
            writer.PutDouble(DeltaInterface.CoordinatePrecision );

            writer.PutDouble(DeltaInterface.AnglePrecision);

            DeltaSnapModel.Pack(writer);
            SymbolModel.Pack(writer);
        }

        public void Unpack(PackageReader reader)
        {
            try
            {
                DeltaQuery.Radius = reader.GetDouble();
                DeltaQuery.Organization = Guid.Parse(reader.GetString());
                DeltaQuery.Aeroport = Guid.Parse(reader.GetString());

                DeltaInterface.DistanceUnit = (HorizantalDistanceType)reader.GetInt32();
                DeltaInterface.DistancePrecision = reader.GetDouble();

                DeltaInterface.HeightUnit = (VerticalDistanceType)reader.GetInt32();
                DeltaInterface.HeightPrecision = reader.GetDouble();

                DeltaInterface.CoordinateUnit = (CoordinateUnitType)reader.GetInt32();
                DeltaInterface.CoordinatePrecision = reader.GetDouble();

                try
                {
                    DeltaInterface.AnglePrecision = reader.GetDouble();
                }
                catch (Exception)
                {
                    DeltaInterface.AnglePrecision = 0.01;

                }
                
                DeltaSnapModel.Unpack(reader);
                SymbolModel.Unpack(reader);
            }
            catch (Exception)
            {
                MessageBox.Show("Old version!");
            }
           
        }
    }
}
