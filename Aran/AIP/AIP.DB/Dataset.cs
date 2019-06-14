//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
//using System.Diagnostics.CodeAnalysis;
//using System.Globalization;
//using System.Reflection;
//using Telerik.WinControls.Enumerations;
//using Aran.Aim.Enums;

//namespace AIP.Dataset
//{
//    //SetSqlGenerator("System.Data.SqlClient", new DefaultValueSqlServerMigrationSqlGenerator());
//    //**
//    // CreationDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
//    // EditionDate = c.DateTime(defaultValueSql: "getutcdate()"),
//    //**
//    // Add C:\Windows\system32\mstdc.exe to firewall allow list
//    //**

//    public class RootEntity
//    {
       
//    }
//    public interface ISection
//    {
//        [DisplayName("Section Status"), Description("Section Status")]
//        [Browsable(true), Editable(true), Category("Edit")]
//        SectionStatusEnum SectionStatus { get; set; }
//    }

//    public abstract class BaseEntity
//    {
//        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int id { get; set; }
//        // Parent Data
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int? eAIPID { get; set; }

//        [Browsable(false), Display(AutoGenerateField = false)]
//        public eAIP eAIP { get; set; }

//        [Browsable(false), ReadOnly(true)]
//        public BaseEntityType BaseEntityType { get; set; }
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int? BaseEntityID { get; set; }
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public virtual BaseEntity Parent { get; set; }
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public virtual ICollection<BaseEntity> Children { get; set; }

//        public BaseEntity()
//        {
//            Children = new List<BaseEntity>();
//        }

//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int? OrderNumber { get; set; }


//        [NotMapped]
//        [ReadOnly(true)]
//        public int? ChildNumberHidden
//        {
//            get { return Children.Count; }
//        }
//    }



//    public class SimpleEntity
//    {
//        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int id { get; set; }

//        // Parent Data
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int? eAIPID { get; set; }

//        [Browsable(false), Display(AutoGenerateField = false)]
//        public eAIP eAIP { get; set; }
//    }


//    public class MetaData
//    {
//        public MetaData()
//        {
//            DataVersion = 0;
//        }

//        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        //[SqlDefaultValue(DefaultValue = "getutcdate()")]
//        //[Column("CreationDate")]
//        //public DateTime? CreationDate { get; set; }

//        [Column("DataVersion")]
//        public int DataVersion { get; set; }
//    }

//    [Table("ENR31")]
//    public class ENR31 : BaseEntity, ISection
//    {
//        public ENR31()
//        {
//            MetaData = new MetaData();
//            NIL = NILReason.None;
//        }
//        public string Title { get; set; }
//        public MetaData MetaData { get; set; }
//        [Browsable(true), Editable(true), Category("Edit")]
//        public SectionStatusEnum SectionStatus { get; set; }
//        public NILReason NIL { get; set; }
//    }

//    [Table("ENR32")]
//    public class ENR32 : BaseEntity, ISection
//    {
//        public ENR32()
//        {
//            MetaData = new MetaData();
//        }
//        public string Title { get; set; }
//        public MetaData MetaData { get; set; }
//        public SectionStatusEnum SectionStatus { get; set; }
//        public NILReason NIL { get; set; }
//    }

//    [Table("ENR33")]
//    public class ENR33 : BaseEntity, ISection
//    {
//        public ENR33()
//        {
//            MetaData = new MetaData();
//            NIL = NILReason.None;
//        }
//        public string Title { get; set; }
//        public MetaData MetaData { get; set; }
//        public SectionStatusEnum SectionStatus { get; set; }
//        public NILReason NIL { get; set; }
//    }

//    [Table("ENR34")]
//    public class ENR34 : BaseEntity, ISection
//    {
//        public ENR34()
//        {
//            MetaData = new MetaData();
//            NIL = NILReason.None;
//        }
//        public string Title { get; set; }
//        public MetaData MetaData { get; set; }
//        public SectionStatusEnum SectionStatus { get; set; }
//        public NILReason NIL { get; set; }
//    }

//    [Table("ENR35")]
//    public class ENR35 : BaseEntity, ISection
//    {
//        public ENR35()
//        {
//            MetaData = new MetaData();
//            NIL = NILReason.None;
//        }
//        [Category("Edit"), DisplayName("Title"), Description("Enter title here")]
//        public string Title { get; set; }
//        public MetaData MetaData { get; set; }
//        public SectionStatusEnum SectionStatus { get; set; }
//        public NILReason NIL { get; set; }
//    }

//    [Table("ENR36")]
//    public class ENR36 : BaseEntity
//    {
//        public ENR36()
//        {
//            MetaData = new MetaData();
//            NIL = NILReason.None;
//        }
//        [Category("Edit"), DisplayName("Title"), Description("Enter title here")]
//        public string Title { get; set; }
//        public MetaData MetaData { get; set; }
//        [Browsable(true), Editable(true), Category("Edit")]
//        public SectionStatusEnum SectionStatus { get; set; }
//        public NILReason NIL { get; set; }

//        public virtual ICollection<Sec36Table> Sec36Table { get; set; }
//    }

//    [Table("Sec36Table")]
//    public class Sec36Table : SimpleEntity
//    {
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int? ENR36id { get; set; }

//        [Browsable(false), Display(AutoGenerateField = false)]
//        public virtual ENR36 ENR36 { get; set; }

//        public string Name { get; set; }
//        public string Ident { get; set; }
//        public string X { get; set; }
//        public string Y { get; set; }

//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int rowspan { get; set; }

//        [Display(AutoGenerateField = false)]
//        [AIP_HiddenProperty]
//        public virtual ICollection<Sec36Table2> Sec36Table2 { get; set; }
//    }



//    [Table("Sec36Table2")]
//    public class Sec36Table2 : SimpleEntity
//    {
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int? Sec36Tableid { get; set; }
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public virtual Sec36Table Sec36Table { get; set; }
//        public string inboundCourse { get; set; }
//        public string turnDirection { get; set; }
//        public string speedLimit { get; set; }
//        public string upperLimit { get; set; }
//        public string lowerLimit { get; set; }
//        public string duration { get; set; }
//        public string length { get; set; }
//    }

//    [Table("ENR41")]
//    public class ENR41 : BaseEntity
//    {

//        public ENR41()
//        {
//            MetaData = new MetaData();
//            Navaid = new List<Navaid>();            
//        }

//        [Category("Edit"), DisplayName("Title"), Description("Enter title here")]
//        public string Title { get; set; }

//        [Browsable(false),ReadOnly(true)]
//        public MetaData MetaData { get; set; }
//        public virtual ICollection<Navaid> Navaid { get; set; }

//        [Browsable(true), Editable(true), Category("Edit")]

//        public SectionStatusEnum SectionStatus { get; set; }

//    }
    
//    [Table("Navaid")]
//    public class Navaid : SimpleEntity
//    {
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int? ENR41id { get; set; }

//        [Browsable(false), Display(AutoGenerateField = false)]
//        public virtual ENR41 ENR41 { get; set; }

//        [ReadOnly(true)]
//        public string Navaidtype { get; set; }
//        //[ReadOnly(true)]
//        public string Navaidname { get; set; }
//        [ReadOnly(true)]
//        public string Navaidident { get; set; }
//        [ReadOnly(true)]
//        public string Navaidmagneticvariation { get; set; }
//        [ReadOnly(true)]
//        public string Navaiddeclination { get; set; }
//        [ReadOnly(true)]
//        public string Navaidfrequency { get; set; }
//        [ReadOnly(true)]
//        public string Navaidhours { get; set; }
//        [ReadOnly(true)]
//        public string Latitude { get; set; }
//        [ReadOnly(true)]
//        public string Longitude { get; set; }
//        [ReadOnly(true)]
//        public string Navaidelevation { get; set; }

//        [TypeConverter(typeof(ArrayToStringTypeConverter))]
//        [ReadOnly(true)]
//        public string[] Navaidremarks { get; set; }

//        [TypeConverter(typeof(YesNoConverter))]
//        [ReadOnly(false)]
//        [Category("Edit"), DisplayName("List Navaid?"), Description("Please select, is Navaid should be available in the AIP?")]
//        public YesNo NavaidListed { get; set; }
//    }

//    [Table("ENR44")]
//    public class ENR44 : BaseEntity
//    {
//        public ENR44()
//        {
//            MetaData = new MetaData();
//            Designatedpoint = new List<Designatedpoint>();
//        }
//        public string Title { get; set; }
//        public MetaData MetaData { get; set; }
//        public virtual ICollection<Designatedpoint> Designatedpoint { get; set; }
//        [Browsable(true), Editable(true), Category("Edit")]
//        public SectionStatusEnum SectionStatus { get; set; }
//    }


//    [Table("Designatedpoint")]
//    public class Designatedpoint : SimpleEntity
//    {
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int? ENR44id { get; set; }

//        [Browsable(false), Display(AutoGenerateField = false)]
//        public virtual ENR44 ENR44 { get; set; }

//        public string Designatedpointident { get; set; }

//        public string Latitude { get; set; }

//        public string Longitude { get; set; }

//        [TypeConverter(typeof(ArrayToStringTypeConverter))]
//        public string[] SIDSTAR { get; set; }

//        [TypeConverter(typeof(YesNoConverter))]
//        public YesNo DesignatedpointListed { get; set; }
//    }

//    [Table("Route")]
//    public class Route : BaseEntity
//    {
//        [Browsable(false)]
//        [Display(AutoGenerateField = false)]
//        public MetaData MetaData { get; set; }

//        public Route()
//        {
//            MetaData = new MetaData();
//        }

//        public string Routedesignator { get; set; }
//        public string RouteRNP { get; set; }
//        public string Routesegmentusage { get; set; }
//        [TypeConverter(typeof(ArrayToStringTypeConverter))]
//        public string[] Routesegmentremark { get; set; }
//        public string Significantpointremark { get; set; }
//        public string Routeremark { get; set; }

//    }

//    [Table("GEN")]
//    public class GEN : BaseEntity
//    {
//        public GEN0 GEN0 { get; set; }

//        public GEN1 GEN1 { get; set; }
//        public MetaData MetaData { get; set; }

//        public GEN()
//        {
//            MetaData = new MetaData();
//        }
//    }

//    [Table("GEN0")]
//    public class GEN0 : BaseEntity
//    {
//        public GEN01 GEN01 { get; set; }
//        public GEN02 GEN02 { get; set; }
//        public MetaData MetaData { get; set; }

//        public GEN0()
//        {
//            MetaData = new MetaData();
//        }
//    }

//    [Table("GEN1")]
//    public class GEN1 : BaseEntity
//    {
//        public GEN11 GEN11 { get; set; }
//        public GEN12 GEN12 { get; set; }
//        public GEN13 GEN13 { get; set; }
//        public GEN14 GEN14 { get; set; }
//        public GEN15 GEN15 { get; set; }
//        public GEN16 GEN16 { get; set; }
//        public GEN17 GEN17 { get; set; }
//        public MetaData MetaData { get; set; }

//        public GEN1()
//        {
//            MetaData = new MetaData();
//        }
//    }

//    [Table("GEN01")]
//    public class GEN01 : BaseEntity
//    {
//        public string Title { get; set; }

//        public MetaData MetaData { get; set; }

//        public GEN01()
//        {
//            MetaData = new MetaData();
//        }
//        [Browsable(true), Editable(true), Category("Edit")]
//        public SectionStatusEnum SectionStatus { get; set; }

//    }

//    [Table("GEN02")]
//    public class GEN02 : BaseEntity
//    {
//        public string Title { get; set; }

//        public MetaData MetaData { get; set; }

//        public GEN02()
//        {
//            MetaData = new MetaData();
//        }
//        [Browsable(true), Editable(true), Category("Edit")]
//        public SectionStatusEnum SectionStatus { get; set; }

//    }

//    [Table("GEN11")]
//    public class GEN11 : BaseEntity, ISection
//    {
//        public string Title { get; set; }

//        public MetaData MetaData { get; set; }

//        public GEN11()
//        {
//            MetaData = new MetaData();
//        }
//        [Browsable(true), Editable(true), Category("Edit")]
//        public SectionStatusEnum SectionStatus { get; set; }

//    }

//    [Table("GEN12")]
//    public class GEN12 : BaseEntity
//    {
//        public string Title { get; set; }

//        public MetaData MetaData { get; set; }

//        public GEN12()
//        {
//            MetaData = new MetaData();
//        }
//        [Browsable(true), Editable(true), Category("Edit")]
//        public SectionStatusEnum SectionStatus { get; set; }

//    }
    

//    [Table("GEN13")]
//    public class GEN13 : BaseEntity
//    {
//        public string Title { get; set; }

//        public MetaData MetaData { get; set; }

//        public GEN13()
//        {
//            MetaData = new MetaData();
//        }
//        [Browsable(true), Editable(true), Category("Edit")]
//        public SectionStatusEnum SectionStatus { get; set; }

//    }


//    [Table("GEN14")]
//    public class GEN14 : BaseEntity
//    {
//        public string Title { get; set; }

//        public MetaData MetaData { get; set; }

//        public GEN14()
//        {
//            MetaData = new MetaData();
//        }
//        [Browsable(true), Editable(true), Category("Edit")]
//        public SectionStatusEnum SectionStatus { get; set; }

//    }


//    [Table("GEN15")]
//    public class GEN15 : BaseEntity
//    {
//        public string Title { get; set; }

//        public MetaData MetaData { get; set; }

//        public GEN15()
//        {
//            MetaData = new MetaData();
//        }
//        [Browsable(true), Editable(true), Category("Edit")]
//        public SectionStatusEnum SectionStatus { get; set; }

//    }


//    [Table("GEN16")]
//    public class GEN16 : BaseEntity
//    {
//        public string Title { get; set; }

//        public MetaData MetaData { get; set; }

//        public GEN16()
//        {
//            MetaData = new MetaData();
//        }
//        [Browsable(true), Editable(true), Category("Edit")]
//        public SectionStatusEnum SectionStatus { get; set; }

//    }


//    [Table("GEN17")]
//    public class GEN17 : BaseEntity
//    {
//        public string Title { get; set; }

//        public MetaData MetaData { get; set; }

//        public GEN17()
//        {
//            MetaData = new MetaData();
//        }
//        [Browsable(true), Editable(true), Category("Edit")]
//        public SectionStatusEnum SectionStatus { get; set; }

//    }

//    [Table("Subsection")]
//    public class Subsection : BaseEntity
//    {
//        public string Title { get; set; }

//        public string Content { get; set; }

//        public MetaData MetaData { get; set; }

//        public Subsection()
//        {
//            MetaData = new MetaData();
//        }
//    }


//    [Table("ENR")]
//    public class ENR : BaseEntity
//    {
//        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        //public int id { get; set; }
//        public ENR3 ENR3 { get; set; }
//        public ENR4 ENR4 { get; set; }
//        public MetaData MetaData { get; set; }

//        public ENR()
//        {
//            MetaData = new MetaData();
//        }
//    }

//    [Table("AIPAmendment")]
//    public class Amendment : BaseEntity
//    {
//        public AmendmentType Type { get; set; }
//        public AmendmentStatus AmendmentStatus { get; set; }
//        public Description Description { get; set; }
//        public virtual ICollection<Group> Group { get; set; }
//        public string Number { get; set; }
//        public string Year { get; set; }
//        public string Publicationdate { get; set; }
//        public string Effectivedate { get; set; }
//        public string Remarks { get; set; }
//        public string Title { get; set; }
//    }

//    [Table("Description")]
//    public class Description : SimpleEntity
//    {
//        public string Title { get; set; }
//    }

//    [Table("Group")]// Not a user group!!! It is a part of Amendment
//    public class Group : SimpleEntity
//    {
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int? Amendmentid { get; set; }

//        [Browsable(false), Display(AutoGenerateField = false)]
//        public virtual Amendment Amendment { get; set; }

//        public Description Description { get; set; }

//        public GroupType Type { get; set; }
//    }

//    [Table("ENR3")]
//    public class ENR3 : BaseEntity
//    {
//        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        //public int id { get; set; }
//        public ENR31 ENR31 { get; set; }
//        public ENR32 ENR32 { get; set; }
//        public ENR33 ENR33 { get; set; }
//        public ENR34 ENR34 { get; set; }
//        public ENR35 ENR35 { get; set; }
//        public ENR36 ENR36 { get; set; }
//        public MetaData MetaData { get; set; }
//        public ENR3()
//        {
//            MetaData = new MetaData();
//        }
//    }

//    [Table("ENR4")]
//    public class ENR4 : BaseEntity
//    {
//        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        //public int id { get; set; }
//        public ENR41 ENR41 { get; set; }
//        public ENR44 ENR44 { get; set; }
//        public MetaData MetaData { get; set; }
//        public ENR4()
//        {
//            MetaData = new MetaData();
//        }
//    }

//    [Table("eAISpackage")]
//    public class eAISpackage
//    {
//        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int id { get; set; }

//        public eAISpackage()
//        {
//            Version = "2.0";
//            ICAOcountrycode = "EV";
//            lang = "en-GB";
//            MetaData = new MetaData();
//            //eAIPpackage = new eAIPpackage();
//        }
//        [Browsable(false)]
//        public MetaData MetaData { get; set; }

//        [Category("Main properties"), DisplayName("Description"), Description("Description")]
//        public string Description { get; set; }

//        [Browsable(false)]
//        [Category("Main properties"), DisplayName("eAIP"), Description("eAIP")]
//        public eAIPpackage eAIPpackage { get; set; }

//        [Category("Main properties"), DisplayName("ICAO Country code"), Description("ICAO Country code")]
//        public string ICAOcountrycode { get; set; }

//        [Category("Main properties"), DisplayName("State"), Description("State")]
//        public string State { get; set; }

//        [Category("Main properties"), DisplayName("Publishing state"), Description("Publishing state")]
//        public string Publishingstate { get; set; }

//        [Category("Main properties"), DisplayName("Publishing organisation"), Description("Publishing organisation")]
//        public string Publishingorganisation { get; set; }

//        [Category("Main properties"), DisplayName("Publishing date"), Description("Publishing date")]
//        public DateTime? Publicationdate { get; set; }

//        [Category("Main properties"), DisplayName("Effective date"), Description("Effective date"), DataType("DateTime2")]
//        public DateTime Effectivedate { get; set; }

//        [Category("Main properties"), DisplayName("Language"), Description("Enter language")]
//        public string lang { get; set; }

//        [Category("Main properties"), DisplayName("Version"), Description("Version")]
//        public string Version { get; set; }

//        [Category("Main properties"), DisplayName("Status"), Description("Status")]
//        public Status Status { get; set; }

//    }

//    [Table("eAIPpackage")]
//    public class eAIPpackage
//    {
//        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int id { get; set; }
//        public eAIPpackage()
//        {
//            Languageversion = new LanguageVersion[] { new LanguageVersion() { lang = "en-GB" } };
//            MetaData = new MetaData();
//            //eAIP = new eAIP();
//        }
//        [Browsable(false)]
//        public MetaData MetaData { get; set; }

//        [Category("Main properties"), DisplayName("Language"), Description("Language")]
//        public LanguageVersion[] Languageversion { get; set; }

//        [Category("Main properties"), DisplayName("eAIP"), Description("eAIP")]
//        public eAIP eAIP { get; set; }

//        [Category("Main properties"), DisplayName("Package name"), Description("Package name")]
//        public string packagename { get; set; }


//    }

//    public class LanguageVersion
//    {
//        public LanguageVersion()
//        {
//            ShowType = showType.replace;
//        }

//        public showType ShowType { get; set; }
//        public string href { get; set; }
//        public string lang { get; set; }
//    }

//    [Table("AIP")]
//    public class eAIP
//    {
//        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int id { get; set; }

//        public eAIP()
//        {
//            Toc = YesNo.Yes;
//            Version = "2.0";
//            ICAOcountrycode = "EV";
//            lang = "en-GB";
//            MetaData = new MetaData();
//        }
//        [Browsable(false)]
//        public MetaData MetaData { get; set; }
//        [Browsable(false)]
//        public GEN GEN { get; set; }

//        [Browsable(false)]
//        public ENR ENR { get; set; }

//        [Browsable(false)]
//        public Amendment Amendment { get; set; }

//        [Category("Main properties"), DisplayName("ICAO Country code"), Description("ICAO Country code")]
//        public string ICAOcountrycode { get; set; }

//        [Category("Main properties"), DisplayName("State"), Description("State")]
//        public string State { get; set; }

//        [Category("Main properties"), DisplayName("Publishing state"), Description("Publishing state")]
//        public string Publishingstate { get; set; }

//        [Category("Main properties"), DisplayName("Publishing organisation"), Description("Publishing organisation")]
//        public string Publishingorganisation { get; set; }

//        [Category("Main properties"), DisplayName("Edition"), Description("Edition")]
//        public string Edition { get; set; }

//        [Category("Main properties"), DisplayName("Publishing date"), Description("Publishing date")]
//        public DateTime? Publicationdate { get; set; }

//        [Category("Main properties"), DisplayName("Effective date"), Description("Effective date"), DataType("DateTime2")]
//        public DateTime Effectivedate { get; set; }

//        [Browsable(false)]
//        [Category("Main properties")]
//        public YesNo Toc { get; set; }

//        [Category("Main properties"), DisplayName("Remarks"), Description("Remarks")]
//        public string Remarks { get; set; }
//        [Browsable(false)]
//        public string @class { get; set; }
//        [Browsable(false)]
//        public string @base { get; set; }

//        [Category("Main properties"), DisplayName("Title"), Description("Enter title here")]
//        public string title { get; set; }

//        [Category("Main properties"), DisplayName("Language"), Description("Enter language")]
//        public string lang { get; set; }

//        [Category("Main properties"), DisplayName("Version"), Description("Version")]
//        public string Version { get; set; }

//        [Category("Main properties"), DisplayName("Status"), Description("Status")]
//        public Status AIPStatus { get; set; }

//    }




//    [Table("RouteSegment")]
//    public class Routesegment : BaseEntity
//    {
//        // Parent Data
//        public MetaData MetaData { get; set; }

//        public Routesegment()
//        {
//            MetaData = new MetaData();
//        }

//        public string RoutesegmentRNP { get; set; }

//        public string Routesegmenttruetrack { get; set; }

//        public string Routesegmentreversetruetrack { get; set; }

//        public string Routesegmentmagtrack { get; set; }

//        public string Routesegmentreversemagtrack { get; set; }

//        public string Routesegmentlength { get; set; }

//        public string Routesegmentwidth { get; set; }

//        public string Routesegmentupper { get; set; }

//        public string Routesegmentlower { get; set; }

//        public string Routesegmentminimum { get; set; }

//        public string Routesegmentloweroverride { get; set; }

//        public string RoutesegmentATC { get; set; }

//        public string Routesegmentairspaceclass { get; set; }

//        public string RoutesegmentCOP { get; set; }

//        public virtual ICollection<Routesegmentusagereference> Routesegmentusagereference { get; set; }

//        public string Routesegmentremarkreference { get; set; }

//    }

//    [Table("Routesegmentusagereference")]
//    public class Routesegmentusagereference : SimpleEntity
//    {
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int? Routesegmentid { get; set; }

//        [Browsable(false), Display(AutoGenerateField = false)]
//        public virtual Routesegment Routesegment { get; set; }

//        public string Routesegmentusagedirection { get; set; }

//        public string Routesegmentusageleveltype { get; set; }

//        public string Routesegmentusagemaximallevel { get; set; }

//        public string Routesegmentusageminimallevel { get; set; }
//        public string Ref { get; set; }

//    }



//    [Table("Significantpointreference")]
//    public class Significantpointreference : BaseEntity
//    {
//        public Significantpointreference()
//        {
//            Navaidindication = new List<Navaidindication>();
//        }
//        public string SignificantpointATC { get; set; }

//        public string Significantpointdescription { get; set; }
//        public string Ref { get; set; }

//        public virtual ICollection<Navaidindication> Navaidindication { get; set; }
//    }

//    [Table("Navaidindication")]
//    public class Navaidindication : SimpleEntity
//    {
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int? Significantpointreferenceid { get; set; }

//        [Browsable(false), Display(AutoGenerateField = false)]
//        public virtual Significantpointreference Significantpointreference { get; set; }

//        public string Navaidindicationradial { get; set; }

//        public string Navaidindicationdistance { get; set; }

//    }

//    [Table("eAIPOptions")]
//    public class eAIPOptions
//    {
//        public eAIPOptions()
//        {
            
//        }

//        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int id { get; set; }

//        public virtual ICollection<LanguageReference> LanguageReferences { get; set; }
//    }

//    [Table("LanguageReference")]
//    public class LanguageReference
//    {
//        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int id { get; set; }

//        public LanguageReference()
//        {

//        }

//        public int? eAIPOptionsId { get; set; }
//        public virtual eAIPOptions eAIPOptions { get; set; }

//        public string Name { get; set; }

//        public string Value { get; set; }

//        public language AIXMValue { get; set; }

//        public virtual ICollection<LanguageText> LanguageText { get; set; }
//    }


//    [Table("LanguageText")]
//    public class LanguageText
//    {
//        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        [Browsable(false), Display(AutoGenerateField = false)]
//        public int id { get; set; }

//        public LanguageText()
//        {

//        }

//        public int? LanguageReferenceId { get; set; }
//        public virtual LanguageReference LanguageReference { get; set; }

//        public LanguageCategory LanguageCategory { get; set; }

//        public string Name { get; set; }

//        public string Value { get; set; }
//    }

//    public enum LanguageCategory
//    {
//        NoCategory = 0,
//        Menu = 1,
//        Text = 2
//    }
//        public enum YesNo
//    {
//        No = 1,
//        Yes = 2
//    }
//    public enum BaseEntityType : short
//    {
//        None = 0,
//        AIP = 1,
//        ENR = 2,
//        ENR3 = 3,
//        ENR31 = 4,
//        Route = 5,
//        Routesegment = 6,
//        ENR32 = 7,
//        ENR33 = 8,
//        ENR34 = 9,
//        ENR35 = 10,
//        ENR36 = 11,
//        Significantpointreference = 12,
//        ENR4 = 13,
//        ENR41 = 14,
//        ENR44 = 15,
//        GEN, //16
//        GEN0, //17
//        GEN01, //18
//        Subsection, //19
//        GEN11, //20
//        GEN1, //21
//        GEN12, //22
//        GEN13, //23
//        GEN14, //24
//        GEN15, //25
//        GEN16, //26
//        GEN17, //27
//        Amendment, //28
//        GEN02 //29
//    }

//    public class AIP_HiddenProperty : Attribute
//    {

//    }


//    public class AIP_ReadOnly : Attribute
//    {

//    }

//    public enum Status : byte
//    {
//        None = 0,
//        Work = 1,
//        Published = 2
//    }

//    public enum showType
//    {
//        @new,
//        replace,
//        embed,
//        other,
//        none,
//    }

//    public enum SectionStatusEnum
//    {
//        None = 0,
//        Work = 1,
//        Filled = 2
//    }

//    public enum NILReason : byte
//    {
//        None = 0,
//        Electronic = 1,
//        Notavailable = 2,
//        Inpreparation = 3,
//        Notapplicable = 4,
//    }

//    public enum AmendmentType : byte
//    {
//        AIRAC,
//        NonAIRAC,
//        Nonlisted,
//    }

//    public enum AmendmentStatus : byte
//    {
//        None,
//        NotAvailable,
//        Available
//    }

//    public enum GroupType : byte
//    {
//        Misc,
//        GEN,
//        ENR,
//        AD
//    }

//    public class ArrayToStringTypeConverter : TypeConverter
//    {
//        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
//        {
//            if (destinationType == typeof(string))
//            {
//                return true;
//            }
//            return base.CanConvertTo(context, destinationType);
//        }

//        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
//        {
//            string[] values = (string[])value;
//            return string.Join(",", values);
//        }

//        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
//        {
//            if (sourceType == typeof(string[]))
//            {
//                return true;
//            }
//            return base.CanConvertFrom(context, sourceType);
//        }

//        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
//        {
//            string values = (string)value;
//            return values.Split(',');
//        }
//    }

//    public class YesNoConverter : TypeConverter
//    {
//        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
//        {
//            return destinationType == typeof(ToggleState);
//        }
//        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
//        {
//            YesNo YesNoValue = (YesNo)value;

//            switch (YesNoValue)
//            {
//                case YesNo.Yes:
//                    return ToggleState.On;
//                case YesNo.No:
//                    return ToggleState.Off;
//            }

//            return base.ConvertTo(context, culture, value, destinationType);
//        }

//        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
//        {
//            return sourceType == typeof(ToggleState);
//        }
//        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
//        {
//            ToggleState state = (ToggleState)value;

//            switch (state)
//            {
//                case ToggleState.On:
//                    return YesNo.Yes;
//                case ToggleState.Off:
//                    return YesNo.No;
//            }
//            return base.ConvertFrom(context, culture, value);
//        }
//    }

//}