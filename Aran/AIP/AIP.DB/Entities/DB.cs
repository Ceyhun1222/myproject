using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Telerik.WinControls.Enumerations;
using Aran.Aim.Enums;
using System.Collections.ObjectModel;
using System.Linq;
using Telerik.WinControls.UI;

namespace AIP.DB
{
    [Table("Section")]
    public class AIPSection : ISection, IRootEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public int? eAIPID { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public eAIP eAIP { get; set; }
        

        [Browsable(true), Editable(true), Category("Edit")]
        public SectionStatusEnum SectionStatus { get; set; }
        public SectionName SectionName { get; set; }

        [Category("Edit"), DisplayName("Title"), Description("Enter title here")]
        public string Title { get; set; }
        public NILReason NIL { get; set; }


        [Browsable(false), Display(AutoGenerateField = false)]
        public int? AirportHeliportID { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public AirportHeliport AirportHeliport { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public virtual ICollection<SubClass> Children { get; set; }
    }

    [Table("AirportHeliport")]
    public class AirportHeliport : IRootEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        [ForeignKey("AIPSection")]
        public int id { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public int? eAIPID { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public eAIP eAIP { get; set; }
        
        [Browsable(false), Display(AutoGenerateField = false)]
        public List<AIPSection> AIPSection { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public CodeAirportHeliport? Type { get; set; }

        public string Name { get; set; }

        public string LocationIndicatorICAO { get; set; }
    }

    public class PDFPage : IRootEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public int? eAIPID { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public eAIP eAIP { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public SectionName Section { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public int? AirportHeliportID { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public AirportHeliport AirportHeliport { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public int Page { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public int? PageeAIPID { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public eAIP PageeAIP { get; set; }

    }

    [Table("Sec36Table")]
    public class Sec36Table : SubClass
    {

        public string Name { get; set; }
        public string Ident { get; set; }
        public string X { get; set; }
        public string Y { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public int rowspan { get; set; }

        [Display(AutoGenerateField = false)]
        public virtual ICollection<Sec36Table2> Sec36Table2 { get; set; }
    }

    [Table("Sec36Table2")]
    public class Sec36Table2 : SimpleEntity
    {
        [Browsable(false), Display(AutoGenerateField = false)]
        public int? Sec36Tableid { get; set; }
        [Browsable(false), Display(AutoGenerateField = false)]
        public virtual Sec36Table Sec36Table { get; set; }
        public string inboundCourse { get; set; }
        public string turnDirection { get; set; }
        public string speedLimit { get; set; }
        public string upperLimit { get; set; }
        public string lowerLimit { get; set; }
        public string duration { get; set; }
        public string length { get; set; }
    }
    

    [Table("Navaid")]
    public class Navaid : SubClass
    {
       
        [ReadOnly(true)]
        public string Navaidtype { get; set; }
        //[ReadOnly(true)]
        public string Navaidname { get; set; }
        [ReadOnly(true)]
        public string Navaidident { get; set; }
        [ReadOnly(true)]
        public string Navaidmagneticvariation { get; set; }
        [ReadOnly(true)]
        public string Navaiddeclination { get; set; }
        [ReadOnly(true)]
        public string Navaidfrequency { get; set; }
        [ReadOnly(true)]
        public string Navaidhours { get; set; }
        [ReadOnly(true)]
        public string Latitude { get; set; }
        [ReadOnly(true)]
        public string Longitude { get; set; }
        [ReadOnly(true)]
        public string Navaidelevation { get; set; }

        [TypeConverter(typeof(ArrayToStringTypeConverter))]
        [ReadOnly(true)]
        public string[] Navaidremarks { get; set; }

        [TypeConverter(typeof(YesNoConverter))]
        [ReadOnly(true)]
        //[Category("Edit"), DisplayName("List Navaid?"), Description("Please select, is Navaid should be available in the AIP?")]
        public YesNo NavaidListed { get; set; }
    }
    

    [Table("Designatedpoint")]
    public class Designatedpoint : SubClass
    {

        public string Designatedpointident { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
        
        public string SIDSTAR { get; set; }

        [TypeConverter(typeof(YesNoConverter))]
        public YesNo DesignatedpointListed { get; set; }
    }

    [Table("Route")]
    public class Route : SubClass
    {        
        public string Routedesignator { get; set; }
        public string RouteRNP { get; set; }
        public string Routesegmentusage { get; set; }
        [TypeConverter(typeof(ArrayToStringTypeConverter))]
        public string[] Routesegmentremark { get; set; }
        public string Significantpointremark { get; set; }
        public string Routeremark { get; set; }
    }

    [Table("LocationTable")]
    public class LocationTable : SubClass
    {
        public string Caption { get; set; }

        [Display(AutoGenerateField = false)]
        public virtual ICollection<LocationDefinition> LocationDefinition { get; set; }
    }

    [Table("LocationDefinition")]
    public class LocationDefinition : SimpleEntity
    {
        [Browsable(false), Display(AutoGenerateField = false)]
        public int? LocationTableid { get; set; }
        [Browsable(false), Display(AutoGenerateField = false)]
        public virtual LocationTable LocationTable { get; set; }

        public string LocationIdent { get; set; }
        public string LocationName { get; set; }
        public YesNo LocationDefinitionAFS { get; set; }
        public LocationDefinitionType LocationDefinitionType { get; set; }
    }

    [Table("Subsection")]
    public class Subsection : SubClass
    {
        public string Title { get; set; }

        public string Content { get; set; }        
    }


    [Table("AIPAmendment")]
    public class Amendment : SubClass
    {
        public AmendmentType Type { get; set; }
        public AmendmentStatus AmendmentStatus { get; set; }

        [Browsable(false)]
        public Description Description { get; set; }

        [Browsable(false)]
        public virtual ICollection<Group> Group { get; set; }
        public string Number { get; set; }
        public string Year { get; set; }
        [Browsable(false)]
        public string Publicationdate { get; set; }
        [Browsable(false)]
        public string Effectivedate { get; set; }
        public string Remarks { get; set; }
        public string Title { get; set; }
    }

    [Table("Description")]
    public class Description : SimpleEntity
    {
        public string Title { get; set; }
    }

    [Table("Group")]// Not a user group!!! It is a part of Amendment
    public class Group : SimpleEntity
    {
        [Browsable(false), Display(AutoGenerateField = false)]
        public int? Amendmentid { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public virtual Amendment Amendment { get; set; }

        public Description Description { get; set; }

        public GroupType Type { get; set; }
    }
    

    [Table("eAISpackage")]
    public class eAISpackage : IUserChangesEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }

        public eAISpackage()
        {
            Version = "2.0";
            ICAOcountrycode = "EV";
            lang = "en-GB";
            Status = Status.Work;
            Effectivedate =
                BaseLib.Airac.AiracCycle.AiracCycleList?.FirstOrDefault(x => x.Airac > DateTime.UtcNow.AddDays(84))
                    ?.Airac ?? DateTime.UtcNow;
            //Publicationdate = Effectivedate.AddDays(-42);
        }

        [Category("Main properties"), DisplayName("Description"), Description("Description")]
        [PropertyOrder(10)]
        public string Description { get; set; }

        [Browsable(false)]
        [Category("Main properties"), DisplayName("eAIP"), Description("eAIP")]
        [PropertyOrder(15)]
        public eAIPpackage eAIPpackage { get; set; }

        [Category("Main properties"), DisplayName("ICAO Country code"), Description("ICAO Country code")]
        [PropertyOrder(20)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public string ICAOcountrycode { get; set; }

        [Category("Main properties"), DisplayName("State"), Description("State")]
        [PropertyOrder(30)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public string State { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        [PropertyOrder(40)]
        public string Publishingstate { get; set; }

        [Category("Main properties"), DisplayName("Publishing organisation"), Description("Publishing organisation")]
        [Browsable(false), Display(AutoGenerateField = false)]
        [PropertyOrder(50)]
        public string Publishingorganisation { get; set; }

        [Category("Main properties"), DisplayName("Publishing date"), Description("Publishing date")]
        [DataEntryOption(Visible = true, ReadOnly = false)]
        [PropertyOrder(60)]
        public DateTime? Publicationdate { get; set; }

        [Category("Main properties"), DisplayName("Effective date"), Description("Effective date"), DataType("DateTime2")]
        [PropertyOrder(70)]
        public DateTime Effectivedate { get; set; }

        [Category("Main properties"), DisplayName("Language"), Description("Enter language")]
        [Browsable(false), Display(AutoGenerateField = false)]
        [PropertyOrder(80)]
        public string lang { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        [PropertyOrder(85)]
        public string Version { get; set; }

        [Category("Main properties"), DisplayName("Status"), Description("Status")]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(87)]
        public Status Status { get; set; }


        // User Created
        [Category("Main properties"), DisplayName("Created by")]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(90)]
        public virtual User CreatedUser { get; set; }

        [Category("Main properties"), DisplayName("Create date"), DataType("DateTime2")]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(95)]
        public DateTime CreatedDate { get; set; }

        [Category("Main properties"), DisplayName("Created by"), Description("Created by")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [PropertyOrder(100)]
        // User Id
        public int? CreatedUserId { get; set; }

        // User Created
        [Category("Main properties"), DisplayName("Changed by")]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(110)]
        public virtual User ChangedUser { get; set; }

        [Category("Main properties"), DisplayName("Change date"), DataType("DateTime2")]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(120)]
        public DateTime ChangedDate { get; set; }

        [Category("Main properties"), DisplayName("Changed by"), Description("User")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [PropertyOrder(130)]
        // User Id
        public int? ChangedUserId { get; set; }

    }

    [Table("eAIPpackage")]
    public class eAIPpackage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }
        public eAIPpackage()
        {
            Languageversion = new LanguageVersion[] { new LanguageVersion() { lang = "en-GB" } };
        }

        [Category("Main properties"), DisplayName("Language"), Description("Language")]
        public LanguageVersion[] Languageversion { get; set; }

        [Category("Main properties"), DisplayName("eAIP"), Description("eAIP")]
        public eAIP eAIP { get; set; }

        [Category("Main properties"), DisplayName("Package name"), Description("Package name")]
        public string packagename { get; set; }
    }

    public class LanguageVersion
    {
        public LanguageVersion()
        {
            ShowType = showType.replace;
        }

        public showType ShowType { get; set; }
        public string href { get; set; }
        public string lang { get; set; }
    }

    [Table("AIP")]
    public class eAIP
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }

        public eAIP()
        {
            Toc = YesNo.Yes;
            Version = "2.0";
            ICAOcountrycode = "EV";
            lang = "en-GB";
        }
        
        public virtual ICollection<AIPSection> Section { get; set; }

        [Browsable(false)]
        public Amendment Amendment { get; set; }

        [Category("Main properties"), DisplayName("ICAO Country code"), Description("ICAO Country code")]
        public string ICAOcountrycode { get; set; }

        [Category("Main properties"), DisplayName("State"), Description("State")]
        public string State { get; set; }

        [Category("Main properties"), DisplayName("Publishing state"), Description("Publishing state")]
        public string Publishingstate { get; set; }

        [Category("Main properties"), DisplayName("Publishing organisation"), Description("Publishing organisation")]
        public string Publishingorganisation { get; set; }

        [Category("Main properties"), DisplayName("Edition"), Description("Edition")]
        public string Edition { get; set; }

        [Category("Main properties"), DisplayName("Publishing date"), Description("Publishing date")]
        public DateTime? Publicationdate { get; set; }

        [Category("Main properties"), DisplayName("Effective date"), Description("Effective date"), DataType("DateTime2")]
        public DateTime Effectivedate { get; set; }

        [Browsable(false)]
        [Category("Main properties")]
        public YesNo Toc { get; set; }

        [Category("Main properties"), DisplayName("Remarks"), Description("Remarks")]
        public string Remarks { get; set; }
        [Browsable(false)]
        public string @class { get; set; }
        [Browsable(false)]
        public string @base { get; set; }

        [Category("Main properties"), DisplayName("Title"), Description("Enter title here")]
        public string title { get; set; }

        [Category("Main properties"), DisplayName("Language"), Description("Enter language")]
        public string lang { get; set; }

        [Category("Main properties"), DisplayName("Version"), Description("Version")]
        public string Version { get; set; }

        [Category("Main properties"), DisplayName("Status"), Description("Status")]
        public Status AIPStatus { get; set; }

    }




    [Table("RouteSegment")]
    public class Routesegment : SubClass
    {
        public string RoutesegmentRNP { get; set; }

        public string Routesegmenttruetrack { get; set; }

        public string Routesegmentreversetruetrack { get; set; }

        public string Routesegmentmagtrack { get; set; }

        public string Routesegmentreversemagtrack { get; set; }

        public string Routesegmentlength { get; set; }

        public string Routesegmentwidth { get; set; }

        public string Routesegmentupper { get; set; }

        public string Routesegmentlower { get; set; }

        public string Routesegmentminimum { get; set; }

        public string Routesegmentloweroverride { get; set; }

        public string RoutesegmentATC { get; set; }

        public string Routesegmentairspaceclass { get; set; }

        public string RoutesegmentCOP { get; set; }

        public virtual ICollection<Routesegmentusagereference> Routesegmentusagereference { get; set; }

        public string Routesegmentremarkreference { get; set; }

    }

    [Table("Routesegmentusagereference")]
    public class Routesegmentusagereference : SimpleEntity
    {
        [Browsable(false), Display(AutoGenerateField = false)]
        public int? Routesegmentid { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public virtual Routesegment Routesegment { get; set; }

        public string Routesegmentusagedirection { get; set; }

        public string Routesegmentusageleveltype { get; set; }

        public string Routesegmentusagemaximallevel { get; set; }

        public string Routesegmentusageminimallevel { get; set; }
        public string Ref { get; set; }

    }



    [Table("Significantpointreference")]
    public class Significantpointreference : SubClass
    {
        public Significantpointreference()
        {
            Navaidindication = new List<Navaidindication>();
        }
        public string SignificantpointATC { get; set; }

        public string Significantpointdescription { get; set; }
        public string Ref { get; set; }

        public virtual ICollection<Navaidindication> Navaidindication { get; set; }
    }

    [Table("Navaidindication")]
    public class Navaidindication : SimpleEntity
    {
        [Browsable(false), Display(AutoGenerateField = false)]
        public int? Significantpointreferenceid { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public virtual Significantpointreference Significantpointreference { get; set; }

        public string Navaidindicationradial { get; set; }

        public string Navaidindicationdistance { get; set; }

    }
    [Serializable]
    [Table("eAIPOptions")]
    public class eAIPOptions
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }

        public virtual ICollection<LanguageReference> LanguageReferences { get; set; }
    }

    [Serializable]
    [Table("LanguageReference")]
    public class LanguageReference
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }
        [Browsable(false), Display(AutoGenerateField = false)]
        public int? eAIPOptionsId { get; set; }
        [Browsable(false), Display(AutoGenerateField = false)]
        public virtual eAIPOptions eAIPOptions { get; set; }

        public string Name { get; set; }
        [Browsable(false), Display(AutoGenerateField = false)]
        public string Value { get; set; }
        [Browsable(false), Display(AutoGenerateField = false)]
        public language AIXMValue { get; set; }
        [Category("Language"), DisplayName("Language"), Description("Language")]
        public virtual Collection<LanguageText> LanguageText { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    [Serializable]
    [Table("LanguageText")]
    public class LanguageText
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public int? LanguageReferenceId { get; set; }
        [Browsable(false), Display(AutoGenerateField = false)]
        public virtual LanguageReference LanguageReference { get; set; }

        public LanguageCategory LanguageCategory { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }


    [Serializable]
    [Table("ChartNumber")]
    public class ChartNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }

        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Browsable(false)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        public virtual ICollection<AIPFile> AIPFile { get; set; }
        public ChartNumber()
        {
            AIPFile = new List<AIPFile>();
        }
        public override string ToString()
        {
            return Name;
        }
    }

    [Table("AIPFile")]
    public class AIPFile : TemporalityEntity
    {
        public AIPFile()
        {
            EffectivedateFrom = DateTime.UtcNow.Date;
            Version = 1;
        }

        [Category("Edit"), DisplayName("AIP Section"), Description("Select AIP section")]
        public SectionName SectionName { get; set; }

        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [Category("Edit"), DisplayName("Chart number"), Description("Select chart number")]
        public virtual ChartNumber ChartNumber { get; set; }

        [DataEntryOption(Visible = true, ReadOnly = false)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        //[Browsable(false)]
        [Category("Edit"), DisplayName("Select chart number"), Description("Select chart number")]
        public int? ChartNumberId { get; set; }

        [Category("Edit"), DisplayName("Airport/Heliport"), Description("Select Airport/Heliport, if required")]
        public string AirportHeliport { get; set; }

        [Category("Edit"), DisplayName("Title"), Description("Enter file title here"), DataEntryOption(Visible = true, ReadOnly = false)]
        public string Title { get; set; }

        [Category("Edit"), DisplayName("Description"), Description("Enter file description, if any")]
        public string Description { get; set; }

        [Category("Edit"), DisplayName("File"), Description("Select file"), GridViewOption(Visible = false)]
        public string FileName { get; set; }

        [Category("Edit"), DisplayName("Order number"), Description("Enter order number within section, if required"), 
            GridViewOption(Visible = true, ReadOnly = false), 
            DataEntryOption(Visible = false, ReadOnly = true)]
        public int Order { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public int? AIPFileDataId { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public AIPFileData AIPFileData { get; set; }

    }

    [Table("FileData")]
    public class AIPFileData
    {
        public AIPFileData()
        {
            AIPFile = new List<AIPFile>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public byte[] Data { get; set; }

        public virtual ICollection<AIPFile> AIPFile { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public AIPFileDataHash AIPFileDataHash { get; set; }

    }

    [Table("FileDataHash")]
    public class AIPFileDataHash
    {
        [Key]
        [Browsable(false), Display(AutoGenerateField = false)]
        [ForeignKey("AIPFileData")]
        public int id { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public string Hash { get; set; }

        public virtual AIPFileData AIPFileData { get; set; }

    }


    [Table("AIPPage")]
    public class AIPPage : IRootEntity, IUserChangesEntity
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public int? eAIPID { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public eAIP eAIP { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public PageType PageType { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public DocType DocType { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public int? AIPPageDataId { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public AIPPageData AIPPageData { get; set; }

        // User Created
        [Category("Edit"), DisplayName("Created by")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(90)]
        public virtual User CreatedUser { get; set; }

        [Category("Edit"), DisplayName("Create date"), DataType("DateTime2")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(95)]
        public DateTime CreatedDate { get; set; }

        [Category("Edit"), DisplayName("Created by"), Description("Created by")]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [PropertyOrder(100)]
        // User Id
        public int? CreatedUserId { get; set; }

        // User Created
        [Category("Edit"), DisplayName("Changed by")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(110)]
        public virtual User ChangedUser { get; set; }

        [Category("Edit"), DisplayName("Change date"), DataType("DateTime2")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(120)]
        public DateTime ChangedDate { get; set; }

        [Category("Edit"), DisplayName("Changed by"), Description("User")]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [PropertyOrder(130)]
        // User Id
        public int? ChangedUserId { get; set; }
    }

    [Table("AIPPageData")]
    public class AIPPageData
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public byte[] Data { get; set; }
        
        [Browsable(false), Display(AutoGenerateField = false)]
        public string Hash { get; set; }

    }

    [Table("User")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }
        
        [Browsable(false)]
        public int TossId { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    [Table("Abbreviation")]
    public class Abbreviation : TemporalityEntity
    {
        public Abbreviation()
        {
            EffectivedateFrom = DateTime.UtcNow.Date;
            Version = 1;
        }
        
        [Display(AutoGenerateField = false), DataType("VARCHAR"),StringLength(41), Index(IsUnique = false)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        public string IdKey { get; set; }
        
        [Category("Edit"), DisplayName("Ident"), Description("Enter Identificator here"), DataEntryOption(Visible = true, ReadOnly = false)]
        [GridViewOption(Visible = true, ReadOnly = true, MaxWidth = 100)]
        public string Ident { get; set; }

        [Category("Edit"), DisplayName("Description"), Description("Enter details"), DataEntryOption(Visible = true, ReadOnly = false, RowSpan = 5)]
        [GridViewOption(Visible = true, ReadOnly = true, RenderHTML = true)]
        public string Details { get; set; }
    }
    
    [Table("Supplement")]
    public class Supplement : TemporalityEntity
    {
        public Supplement()
        {
            //EffectivedateFrom = DateTime.UtcNow.Date;
            //Version = 1;
            EffectivedateFrom =
                BaseLib.Airac.AiracCycle.AiracCycleList?.FirstOrDefault(x => x.Airac > DateTime.UtcNow.AddDays(84))
                    ?.Airac ?? DateTime.UtcNow;
            Publicationdate = DateTime.UtcNow.AddDays(7);
        }

        [Category("Edit"), DisplayName("Title"), Description("Enter title here"),
         DataEntryOption(Visible = true, ReadOnly = false)]
        [PropertyOrder(100)]
        public string Title { get; set; }

        [Category("Edit"), DisplayName("Description"), Description("Enter description here"),
         DataEntryOption(Visible = false, ReadOnly = false), GridViewOption(Visible = false)]
        [PropertyOrder(10000)]
        public string Description { get; set; }


        [Category("Edit"), DisplayName("Number"), Description("Enter number here"),
         DataEntryOption(Visible = true, ReadOnly = false)]
        [PropertyOrder(300)]
        public string Number { get; set; }

        [Category("Edit"), DisplayName("Year"), Description("Enter year here"),
         DataEntryOption(Visible = true, ReadOnly = false), Display(AutoGenerateFilter = false)]
        [PropertyOrder(400)]
        public string Year { get; set; }

        [Category("Edit"), DisplayName("AIP section(s) affected"), Description("Enter AIP section(s) affected"),
         DataEntryOption(Visible = true, ReadOnly = false), Display(AutoGenerateFilter = false)]
        [PropertyOrder(500)]
        public string Affects { get; set; }

        [Category("Edit"), DisplayName("Effective date till"), Description("Effective date till"), DataType("DateTime2")]
        [ReadOnly(false)]
        [DataEntryOption(Visible = true, ReadOnly = false)]
        [GridViewOption(Visible = true, ReadOnly = false)]
        [PropertyOrder(60)]
        public override DateTime? EffectivedateTo { get; set; }

        [Category("Edit"), DisplayName("Select Language"), Description("Select language, if required")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [PropertyOrder(80)]
        // Language Id
        public override int? LanguageReferenceId { get; set; }

        [Category("Edit"), DisplayName("Publication date"), Description("Publication date"), DataType("DateTime2")]
        [GridViewOption(Visible = true, ReadOnly = false, MaxWidth = 120)]
        [DataEntryOption(Visible = true, ReadOnly = false)]
        [PropertyOrder(45)]
        public DateTime Publicationdate { get; set; }

        
    }


    [Table("Circular")]
    public class Circular : TemporalityEntity
    {
        public Circular()
        {
            //EffectivedateFrom = DateTime.UtcNow.Date;
            //Version = 1;
            Series = "A";
            EffectivedateFrom =
                BaseLib.Airac.AiracCycle.AiracCycleList?.FirstOrDefault(x => x.Airac > DateTime.UtcNow.AddDays(84))
                    ?.Airac ?? DateTime.UtcNow;
            Publicationdate = DateTime.UtcNow.AddDays(7);

        }

        [Category("Edit"), DisplayName("Title"), Description("Enter title here"),
         DataEntryOption(Visible = true, ReadOnly = false)]
        [PropertyOrder(100)]
        public string Title { get; set; }

        [Category("Edit"), DisplayName("Description"), Description("Enter description here"),
         DataEntryOption(Visible = false, ReadOnly = false), GridViewOption(Visible = false)]
        [PropertyOrder(10000)]
        public string Description { get; set; }

        [Category("Edit"), DisplayName("Number"), Description("Enter number here"),
         DataEntryOption(Visible = true, ReadOnly = false)]
        [PropertyOrder(300)]
        public string Number { get; set; }

        [Category("Edit"), DisplayName("Year"), Description("Enter year here"),
         DataEntryOption(Visible = true, ReadOnly = false), Display(AutoGenerateFilter = false)]
        [PropertyOrder(400)]
        public string Year { get; set; }

        [Category("Edit"), DisplayName("Series"), Description("Enter Series here"),
         DataEntryOption(Visible = true, ReadOnly = false), Display(AutoGenerateFilter = false)]
        [PropertyOrder(500)]
        public string Series { get; set; }

        [Category("Edit"), DisplayName("Remarks"), Description("Enter Remarks here"),
         DataEntryOption(Visible = true, ReadOnly = false), Display(AutoGenerateFilter = false)]
        [PropertyOrder(600)]
        public string Remarks { get; set; }

        [Category("Edit"), DisplayName("Effective date till"), Description("Effective date till"), DataType("DateTime2")]
        [ReadOnly(false)]
        [DataEntryOption(Visible = true, ReadOnly = false)]
        [GridViewOption(Visible = true, ReadOnly = false)]
        [PropertyOrder(60)]
        public override DateTime? EffectivedateTo { get; set; }


        [Category("Edit"), DisplayName("Select Language"), Description("Select language, if required")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [PropertyOrder(80)]
        // Language Id
        public override int? LanguageReferenceId { get; set; }

        [Category("Edit"), DisplayName("Publication date"), Description("Publication date"), DataType("DateTime2")]
        [GridViewOption(Visible = true, ReadOnly = false, MaxWidth = 120)]
        [DataEntryOption(Visible = true, ReadOnly = false)]
        [PropertyOrder(45)]
        public DateTime Publicationdate { get; set; }
    }



    [Table("DBConfig")]
    public class DBConfig
    {
        [Key]
        public Cfg Key { get; set; }
        public string Value { get; set; }
        public DBConfig(Cfg _key, string _value)
        {
            Key = _key;
            Value = _value;
        }

        public DBConfig()
        {
        }
    }

    /// <summary>
    /// Database settings. Add settings there
    /// </summary>
    public enum Cfg
    {
        None = 0,
        IsAbbreviationAdded,
        IsTextsAdded,
        OrganizationAuthorityIdentifier,
        ContactName,
        TransferProtocol,
        TransferHost,
        TransferUser,
        TransferPass,
        TransferHostKey,
        TransferRemoteFolder,
    }
}