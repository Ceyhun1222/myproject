using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Features;

namespace AIP.GUI.Classes
{
    public class CategoryWithNotes
    {
        public string Category;
        public List<Note> Annotation;
    }

    public class ForGEN21_6
    {
        public string Name;
        public string Date;
    }

    public class ForGEN25
    {
        public string Id;
        public string StationName;
        public string Facility;
        public string Purpose;
    }

    public class ForGEN31
    {
        public string RP_AIS;
        public string Category;
        public List<Dictionary<string, string>> Row;

        public ForGEN31()
        {
            Row = new List<Dictionary<string, string>>();
        }
    }


    public class ForGEN33_6
    {
        public string UnitName;
        public string AdministrativeArea;
        public string City;
        public string Country;
        public string PostalCode;
        public string Phone;
        public string Fax;
        public string Email;
        public string AFSAddress;
        public string URL;
    }

    public class ForGEN35_7
    {
        public string Name;
        public string CallSign;
        public string Frequency;
        public string BroadcastPeriod;
        public string HoursOfService;
        public string AHIncluded;
        public string ContentsFormatRemarks;
    }

    public class ForGEN35_8
    {
        public string Name;
        public string Hours;
        public string FIRorCTA;
        public string SigmetValidity;
        public string SpecificProcedures;
        public string ATSUnitServed;
        public string AdditionalInfo;
    }

    public class ForENR21
    {
        public string Type;
        public List<ForENR21Row> Row;

        public ForENR21()
        {
            Row = new List<ForENR21Row>();
        }

        public class ForENR21Row
        {
            public string Name;
            public string Points;
            public string Limit;
            public string Class;
            public string UnitName;
            public string CallSign;
            public string Language;
            public string Freq;
            public string Notes;
        }
    }


    public class ForENR36
    {
        public string Name;
        public string Ident;
        public string Coordinates;
        public string Direction;
        public string InboundCourse;
        public List<string> SpeedLimit;
        public List<string> UpperLowerLimits;
        public List<string> Duration;
        public List<string> ControlUnit;
        public List<string> Frequency;
        public ForENR36()
        {
            SpeedLimit = new List<string>();
            UpperLowerLimits = new List<string>();
            Duration = new List<string>();
            ControlUnit = new List<string>();
            Frequency = new List<string>();
        }
        
    }

    public class ForENR42
    {
        public string name;
        public string Category;
        public List<Dictionary<string, string>> Row;

        public ForENR42()
        {
            Row = new List<Dictionary<string, string>>();
        }

        public ForENR42(string p1, string p2 = "")
        {
            name = p1;
            Category = p2;
            Row = new List<Dictionary<string, string>>();

        }
    }

    public class ForENR51
    {
        public string type;
        public List<Dictionary<string, string>> Row;

        public ForENR51()
        {
            Row = new List<Dictionary<string, string>>();
        }

    }

    public class ForENR52
    {
        public string Name;
        public string LateralLimits;
        public string UpperLowerLimits;
        public string SystemMeansOfActivation;
        public string Remarks;
        public string TimeOfActivity;
        public string RiskOfInterception;
    }

    public class ForENR53_1
    {
        public string Name;
        public string LateralLimits;
        public string VerticalLimits;
        public string AdvisoryMeasures;
        public string AuthorityResponsible;
        public string Remarks;
        public string ActivityTimes;
    }

    public class ForENR54
    {
        public string Notes;
        public List<Dictionary<string, string>> Row;

        public ForENR54()
        {
            Row = new List<Dictionary<string, string>>();
        }
    }
    public class ForENR55
    {
        public string Activity;
        public List<Dictionary<string, string>> Row;

        public ForENR55()
        {
            Row = new List<Dictionary<string, string>>();
        }
    }

    public class ForFileSection
    {
        public string Title;
        public string Description;
        public string FileName;
        public string FileType;

        public List<string> GraphicFiles;
    }

    public class ForAD13
    {
        public string type;
        public List<Dictionary<string, string>> Row;

        public ForAD13()
        {
            Row = new List<Dictionary<string, string>>();
        }
    }

    public class ForAD15
    {
        public string type;
        public string TitleName;
        public List<Dictionary<string, string>> Row;

        public ForAD15()
        {
            Row = new List<Dictionary<string, string>>();
        }
    }

    public class ForAD22
    {
        public string ARP;
        public string ADsite;
        public string Direction;
        public string Elevation;
        public string RefTemperature;
        public string Geoid;
        public string MagneticVariation;
        public string DateMagneticVariation;
        public string MagneticVariationChange;
        public string NameOfAdministration_linkage;
        public string NameOfAdministration_email;
        public string NameOfAdministration_voice;
        public string NameOfAdministration_fax;
        public string TypeTraffic;
        public string Remark;

    }

    public class ForAD23
    {
        public string ADAdministration;
        public string CustomAndImmigration;
        public string HealthAndSanitation;
        public string AISBriefingOffice;
        public string ATSReportingOffice;
        public string METBriefingOffice;
        public string ATS;
        public string Fuelling;
        public string Handling;
        public string Security;
        public string Deicing;
        public string AHAvailability;
        public string Remarks;
    }

    public class ForAD24
    {
        public string CargoHandling;
        public string FuelTypes;
        public string OilTypes;
        public string FuellingFacilities;
        public string DeicingFacilities;
        public string HangarSpace;
        public string RepairFacilities;
        public string Remarks;
    }

    public class ForAD25
    {
        public string Hotels;
        public string Restaurants;
        public string Transportation;
        public string MedicalFacilities;
        public string BankPostOffice;
        public string TouristOffice;
        public string Remarks;
    }

    public class ForAD26
    {
        public string ADCategory;
        public string RescueEquipment;
        public string CapabilityForRemoval;
        public string Remarks;
    }

    public class ForAD27
    {
        public string TypesOfClearingEquipment;
        public string ClearancePriorities;
        public string Remarks;
    }

    public class ForAD28
    {
        public ForAD28()
        {
            ApronData = new List<Dictionary<string, string>>();
            TaxiwayData = new List<Dictionary<string, string>>();
        }
        public List<Dictionary<string, string>> ApronData;
        public List<Dictionary<string, string>> TaxiwayData;
        public string Altimeter;
        public string VORCheckpoints;
        public string INSCheckpoints;
        public string Remarks;
    }


    public class ForAD29
    {
        public ForAD29()
        {
            UseAircraft = new List<Dictionary<string, string>>();
            RunwayMarking = new List<Dictionary<string, string>>();
            TaxiwayMarking = new List<Dictionary<string, string>>();
        }
        public List<Dictionary<string, string>> UseAircraft;
        public List<Dictionary<string, string>> RunwayMarking;
        public List<Dictionary<string, string>> TaxiwayMarking;
        public string StopBars;
        public string Remarks;
    }

    public class ForAD210
    {
        public ForAD210()
        {
            AREA2 = new List<Dictionary<string, string>>();
            AREA3 = new List<Dictionary<string, string>>();
        }
        public List<Dictionary<string, string>> AREA2;
        public List<Dictionary<string, string>> AREA3;
        public string Area2Remarks;
        public string Area3Remarks;
    }

    public class ForAD211
    {
        public string AssociatedMETOffice;
        public string HoursOfService;
        public string METOfficeOutsideHours;
        public string OfficeResponsible;
        public string PeriodsOfValidity;
        public string IntervalOfIssuance;
        public string TrendForecast;
        public string IntervalOfIssuance2;
        public string BriefingConsultation;
        public string FlightDocumentation;
        public string Charts;
        public string SupplementaryEquipment;
        public string ATSUnits;
        public string AdditionalInformation;
    }

    public class ForAD212
    {
        public string Designator;
        public string TrueBearing;
        public string Dimensions;

        public string SurfaceCondition;
        public string ClassPCN;
        public string PavementTypePCN;
        public string PavementSubgradePCN;
        public string MaxTyrePressurePCN;
        public string EvaluationMethodPCN;
        public string RWYSurface;

        public string SWYSurfaceCondition;
        public string SWYClassPCN;
        public string SWYPavementTypePCN;
        public string SWYPavementSubgradePCN;
        public string SWYMaxTyrePressurePCN;
        public string SWYEvaluationMethodPCN;
        public string SWYSurface;

        public string GeoCoords;
        public string GeoidUndulation;
        public string Elevation;
        public string SWYDimensions;
        public string SlopeTDZ;
        public string CWYDimensions;
        public string StripDimensions;
        public string OFZ;
        public string Remarks;
    }

    public class ForAD213
    {
        public string Designator;
        public string TORADistance;
        public string TODADistance;
        public string ASDADistance;
        public string LDADistance;
        public string Remarks;
    }

    public class ForAD214
    {
        public string Designator;
        public string ALSIntensityLevel;
        public string ALSType;
        public string ALSLength;
        public string THRColour;
        public string WBARColour;
        public string WBARDesc;
        public string VASIS;
        public string TDZColour;
        public string CLColour;
        public string CLIntensity;
        public string EDGEColour;
        public string EDGEIntensity;
        public string SWYColour;
        public string Remarks;
    }

    public class ForAD215
    {
        public ForAD215()
        {
            Characteristics = new List<Dictionary<string, string>>();
        }
        public string Location;
        public List<Dictionary<string, string>> Characteristics;
        public string LDILocation;
        public string WDILocation;
        public string TWYEdgeCl;
        public string SecondaryPowerSupply;
        public string Remarks;
    }

    public class ForAD216
    {
        public string Designator;
        public string Coords;
        public string GeoidUndulation;
        public string Elevation;
        public string Dimension;
        public string Composition;
        public string SurfaceProperties;
        public string TrueBearing;
        public string DeclaredDistance;
        public string ApproachLighting;
        public string Remarks;
    }
    public class ForAD217
    {
        public string Designator;
        public string Name;
        public string Coords;
        public string VerticalLimits;
        public string AispaceClassification;
        public string CallsignDetails;
        public string Language;
        public string TransitionAltitude;
        public string HoursApplicability;
        public string Remarks;
    }

    public class ForAD218
    {
        public ForAD218()
        {
            RadioCommunicationChannel = new List<Dictionary<string, string>>();
        }
        public string ServiceDesignation;
        public string CallSign;
        public List<Dictionary<string, string>> RadioCommunicationChannel;
        public string ServiceOperationalHours;
        public string Remarks;
    }

    public class ForAD219
    {
        public ForAD219()
        {
            NavaidEquipment = new List<Dictionary<string, string>>();
        }
        public string Type;
        public List<Dictionary<string, string>> NavaidEquipment;
        public string TypeSupportedOperation;
        public string Designator;
        public string HoursOfOperations;
        public string Remarks;
    }

    public class ForAD220
    {
        public string Designator;
        public string Remarks;
    }


    public class ForAD32
    {
        public string ARP;
        public string ADsite;
        public string Direction;
        public string Elevation;
        public string RefTemperature;
        public string Geoid;
        public string MagneticVariation;
        public string DateMagneticVariation;
        public string MagneticVariationChange;
        public string NameOfAdministration_linkage;
        public string NameOfAdministration_email;
        public string NameOfAdministration_voice;
        public string NameOfAdministration_fax;
        public string TypeTraffic;
        public string Remark;

    }

    public class ForAD33
    {
        public string ADAdministration;
        public string CustomAndImmigration;
        public string HealthAndSanitation;
        public string AISBriefingOffice;
        public string ATSReportingOffice;
        public string METBriefingOffice;
        public string ATS;
        public string Fuelling;
        public string Handling;
        public string Security;
        public string Deicing;
        public string AHAvailability;
        public string Remarks;
    }

    public class ForAD34
    {
        public string CargoHandling;
        public string FuelTypes;
        public string OilTypes;
        public string FuellingFacilities;
        public string DeicingFacilities;
        public string HangarSpace;
        public string RepairFacilities;
        public string Remarks;
    }

    public class ForAD35
    {
        public string Hotels;
        public string Restaurants;
        public string Transportation;
        public string MedicalFacilities;
        public string BankPostOffice;
        public string TouristOffice;
        public string Remarks;
    }

    public class ForAD36
    {
        public string ADCategory;
        public string RescueEquipment;
        public string CapabilityForRemoval;
        public string Remarks;
    }

    public class ForAD37
    {
        public string TypesOfClearingEquipment;
        public string ClearancePriorities;
        public string Remarks;
    }

    public class ForAD38
    {
        public ForAD38()
        {
            ApronData = new List<Dictionary<string, string>>();
            TaxiwayData = new List<Dictionary<string, string>>();
        }
        public List<Dictionary<string, string>> ApronData;
        public List<Dictionary<string, string>> TaxiwayData;
        public string Altimeter;
        public string VORCheckpoints;
        public string INSCheckpoints;
        public string Remarks;
    }


    public class ForAD39
    {
        public ForAD39()
        {
            UseAircraft = new List<Dictionary<string, string>>();
            RunwayMarking = new List<Dictionary<string, string>>();
            TaxiwayMarking = new List<Dictionary<string, string>>();
        }
        public List<Dictionary<string, string>> UseAircraft;
        public List<Dictionary<string, string>> RunwayMarking;
        public List<Dictionary<string, string>> TaxiwayMarking;
        public string Remarks;
    }

    public class ForAD310
    {
        public ForAD310()
        {
            AREA2 = new List<Dictionary<string, string>>();
        }
        public List<Dictionary<string, string>> AREA2;
        public string Area2Remarks;
    }

    public class ForAD311
    {
        public string AssociatedMETOffice;
        public string HoursOfService;
        public string METOfficeOutsideHours;
        public string OfficeResponsible;
        public string PeriodsOfValidity;
        public string IntervalOfIssuance;
        public string TrendForecast;
        public string IntervalOfIssuance2;
        public string BriefingConsultation;
        public string FlightDocumentation;
        public string Charts;
        public string SupplementaryEquipment;
        public string ATSUnits;
        public string AdditionalInformation;
    }

    public class ForAD312
    {
        public string HeliportType;
        public ForAD312()
        {
            HeliportData = new List<Dictionary<string, string>>();
        }
        public List<Dictionary<string, string>> HeliportData;
        public string TLOFDimensions;
        public string FATOTrueBearings;

        public string FATODimensions;
        public string SFCType;
        public string TLOFSFC;
        public string BRGStrength;
        public string TLOFCoordinates;
        public string FATOElevation;
        public string FATOSlope;
        public string SafetyArea;

        public string HELCWYDimensions;
        public string ObstacleFreeSector;
        public string Remarks;
    }

    public class ForAD313
    {
        public string Designator;
        public string TODAHDistance;
        public string RTODAHDistance;
        public string LDAHDistance;
        public string Remarks;
    }

    public class ForAD314
    {
        public string Designator;
        public string ALSIntensityLevel;
        public string ALSType;
        public string ALSLength;
        public string ALSAll;
        public string VASISType;


        public string FATOCharacteristics;
        public string LocationAPL;
        public string CharacteristicsAPL;
        public string LocationTLOF;
        public string CharacteristicsTLOF;
        public string WBARDesc;
        public string VASIS;
        public string TDZColour;
        public string CLColour;
        public string CLIntensity;
        public string EDGEColour;
        public string EDGEIntensity;
        public string SWYColour;
        public string Remarks;
    }

    public class ForAD315
    {
        public ForAD315()
        {
            Characteristics = new List<Dictionary<string, string>>();
        }
        public string Location;
        public List<Dictionary<string, string>> Characteristics;
        public string LDILocation;
        public string WDILocation;
        public string TWYEdgeCl;
        public string SecondaryPowerSupply;
        public string Remarks;
    }

    public class ForAD316
    {
        public string Designator;
        public string Name;
        public string Coords;
        public string VerticalLimits;
        public string AispaceClassification;
        public string CallsignDetails;
        public string Language;
        public string TransitionAltitude;
        public string HoursApplicability;
        public string Remarks;
    }

    public class ForAD317
    {
        public ForAD317()
        {
            RadioCommunicationChannel = new List<Dictionary<string, string>>();
        }
        public string ServiceDesignation;
        public string CallSign;
        public List<Dictionary<string, string>> RadioCommunicationChannel;
        public string ServiceOperationalHours;
        public string Remarks;
    }

    public class ForAD318
    {
        public ForAD318()
        {
            NavaidEquipment = new List<Dictionary<string, string>>();
        }
        public string Type;
        public List<Dictionary<string, string>> NavaidEquipment;
        public string TypeSupportedOperation;
        public string Designator;
        public string HoursOfOperations;
        public string Remarks;
    }

    public class eAIPMainXml
    {
        public string Lang;
        public string Country;
        public string EffectiveDate;
        public string PublicationDate;
        public string Company;
        public string Edition;
        public List<string> Sections;
        public List<string> AirportList;
        public List<string> HeliportList;
    }

    public class ForCover
    {
        public string EffectiveDate;
        public string PublicationDate;
        public string Amendment;


    }
}
