using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AIP.DB.SectionParameter;

namespace AIP.DB
{

    public enum LanguageCategory
    {
        NoCategory = 0,
        Menu = 1,
        Text = 2,
        Template = 3
    }
    public enum YesNo
    {
        No = 1,
        Yes = 2
    }
    public enum SubClassType
    {
        None = 0,
        AIP = 1,
        Route = 2,
        Routesegment = 3,
        Significantpointreference = 4,
        Subsection = 5,
        Amendment = 6,
        Sec36Table,
        Navaid,
        Designatedpoint,
        LocationTable
    }

    //public enum TemporalityEntityType
    //{
    //    None = 0,
    //    AIPFile = 1,
    //    Abbreviation = 2,
    //    SUP = 3,
    //    AIC = 4
    //}

    /// <summary>
    /// Section names. If AD2 and AD3, require additional information
    /// </summary>
    /// <remarks>
    /// While mapping is manual, used enum instead of table for more stable and control work
    /// </remarks>
    /// 
    /// ATTENTION: Add new sections at the end of list, 
    ///            because they are stored in the database as ordered integer
    public enum SectionName
    {
        None = 0,

        [SectionOption(All, ShowOrder = 10)] // TODO: write ShowOrder for all elements of SectionName
        GEN01,

        [SectionOption(Generate | TextSection | PDFPages, ShowOrder = 20, GenerateOrder = 30)]
        GEN02,

        [SectionOption(All & ~Fill & ~AIXM_Source & ~GenerateSubsection, ShowOrder = 30)]
        GEN03,

        [SectionOption(Generate | TextSection | PDFPages, GenerateOrder = Int32.MaxValue, ShowOrder = 40)] // Lastest section to generate
        GEN04, // no data

        [SectionOption(All, ShowOrder = 50)]
        GEN05, 

        [SectionOption(All & ~AIXM_Source & ~Fill & ~AMDT & ~GenerateSubsection, ShowOrder = 60)]
        GEN06, // no data

        [SectionOption(All, ShowOrder = 70)]
        GEN11,

        [SectionOption(All, ShowOrder = 80)]
        GEN12, 

        [SectionOption(All, ShowOrder = 90)]
        GEN13,

        [SectionOption(All, ShowOrder = 100)]
        GEN14,

        [SectionOption(All, ShowOrder = 110)]
        GEN15,

        [SectionOption(All, ShowOrder = 120)]
        GEN16,

        [SectionOption(All, ShowOrder = 130)]
        GEN17,

        [SectionOption(All, ShowOrder = 140)]
        GEN21, 
        GEN21_1,
        GEN21_2,
        GEN21_3,
        GEN21_4,
        GEN21_5,
        GEN21_6,

        [SectionOption(All & ~GenerateSubsection, ShowOrder = 150, GenerateOrder = 50)]
        GEN22,

        [SectionOption(All, ShowOrder = 160)]
        GEN23,

        [SectionOption(Fill | Generate | TextSection | PDFPages | AMDT | AIXM_Source, ShowOrder = 170)]
        GEN24,

        [SectionOption(All, ShowOrder = 180)]
        GEN25, // problem in mapping, mapped without rules


        [SectionOption(All, ShowOrder = 190)]
        GEN26,

        [SectionOption(All, ShowOrder = 200)]
        GEN27,

        [SectionOption(All, ShowOrder = 210)]
        GEN31,
        GEN31_1,
        GEN31_2,
        GEN31_3,
        GEN31_4,
        GEN31_5,
        GEN31_6,

        [SectionOption(All, ShowOrder = 220)]
        GEN32, 
        GEN32_1,
        GEN32_2,
        GEN32_3,
        GEN32_4,
        GEN32_5,
        GEN32_6,
        GEN32_7,
        GEN32_8,

        [SectionOption(All, ShowOrder = 230)]
        GEN33, 
        GEN33_1,
        GEN33_2,
        GEN33_3,
        GEN33_4,
        GEN33_5,
        GEN33_6,

        [SectionOption(All, ShowOrder = 240)]
        GEN34, 
        GEN34_1,
        GEN34_2,
        GEN34_3,
        GEN34_4,
        GEN34_5,

        [SectionOption(All, ShowOrder = 250)]
        GEN35,
        GEN35_1,
        GEN35_2,
        GEN35_3,
        GEN35_4,
        GEN35_5,
        GEN35_6,
        GEN35_7,
        GEN35_8,
        GEN35_9,

        [SectionOption(All, ShowOrder = 260)]
        GEN36, 
        GEN36_1,
        GEN36_2,
        GEN36_3,
        GEN36_4,
        GEN36_5,
        GEN36_6,


        [SectionOption(All, ShowOrder = 270)]
        GEN41,

        [SectionOption(All, ShowOrder = 280)]
        GEN42,

        [SectionOption(All & ~AIXM_Source & ~Fill & ~AMDT & ~GenerateSubsection, ShowOrder = 1010)]
        ENR01, 

        [SectionOption(All & ~AIXM_Source & ~Fill & ~AMDT & ~GenerateSubsection, ShowOrder = 1020)]
        ENR02,

        [SectionOption(All & ~AIXM_Source & ~Fill & ~AMDT & ~GenerateSubsection, ShowOrder = 1030)]
        ENR03,

        [SectionOption(All & ~AIXM_Source & ~Fill & ~AMDT & ~GenerateSubsection, ShowOrder = 1040)]
        ENR04,

        [SectionOption(All & ~AIXM_Source & ~Fill & ~AMDT & ~GenerateSubsection, ShowOrder = 1050)]
        ENR05,

        [SectionOption(All & ~AIXM_Source & ~Fill & ~AMDT & ~GenerateSubsection, ShowOrder = 1060)]
        ENR06, // no information for electron aip

        [SectionOption(All, ShowOrder = 1070)]
        ENR11,

        [SectionOption(All, ShowOrder = 1080)]
        ENR12,

        [SectionOption(All, ShowOrder = 1090)]
        ENR13,

        [SectionOption(All, ShowOrder = 1100)]
        ENR14,
        ENR14_1,
        ENR14_2,
        
        [SectionOption(All, ShowOrder = 1110)]
        ENR15,
        ENR15_1,
        ENR15_2,
        ENR15_3,
        ENR15_4,
        
        [SectionOption(All, ShowOrder = 1120)]
        ENR16,
        ENR16_1,
        ENR16_2,
        ENR16_3,
        ENR16_4,

        [SectionOption(All, ShowOrder = 1130)]
        ENR17,

        [SectionOption(All, ShowOrder = 1140)]
        ENR18,

        [SectionOption(All, ShowOrder = 1150)]
        ENR19,

        [SectionOption(All, ShowOrder = 1160)]
        ENR110,

        [SectionOption(All, ShowOrder = 1170)]
        ENR111,

        [SectionOption(All, ShowOrder = 1180)]
        ENR112,

        [SectionOption(All, ShowOrder = 1190)]
        ENR113,

        [SectionOption(All, ShowOrder = 1200)]
        ENR114,

        [SectionOption(All, ShowOrder = 1210)]
        ENR21,

        [SectionOption(All, ShowOrder = 1220)]
        ENR22,

        [SectionOption(Fill | Generate | TextSection | PDFPages | AMDT | AIXM_Source, ShowOrder = 1230)]
        ENR31,

        [SectionOption(Fill | Generate | TextSection | PDFPages | AMDT | AIXM_Source, ShowOrder = 1240)]
        ENR32,

        [SectionOption(Fill | Generate | TextSection | PDFPages | AMDT | AIXM_Source, ShowOrder = 1250)]
        ENR33,

        [SectionOption(Fill | Generate | TextSection | PDFPages | AMDT | AIXM_Source, ShowOrder = 1260)]
        ENR34,

        [SectionOption(All, ShowOrder = 1270)]
        ENR35,

        [SectionOption(All, ShowOrder = 1280)]
        ENR36,

        [SectionOption(Fill | Generate | TextSection | PDFPages | AMDT | AIXM_Source, ShowOrder = 1290, GenerateOrder = 100)]
        ENR41,

        [SectionOption(All, ShowOrder = 1300)]
        ENR42,

        [SectionOption(All, ShowOrder = 1310)]
        ENR43,

        [SectionOption(Fill | Generate | TextSection | PDFPages | AMDT | AIXM_Source, ShowOrder = 1320, GenerateOrder = 200)]
        ENR44,

        [SectionOption(All, ShowOrder = 1330)]
        ENR45,

        [SectionOption(All, ShowOrder = 1340)]
        ENR51,

        [SectionOption(All, ShowOrder = 1350)]
        ENR52, // problem

        [SectionOption(All, ShowOrder = 1360)]
        ENR53, // problem
        ENR53_1,
        ENR53_2,

        [SectionOption(All, ShowOrder = 1370)]
        ENR54,

        [SectionOption(All, ShowOrder = 1380)]
        ENR55, // problem

        [SectionOption(All, ShowOrder = 1390)]
        ENR56,

        [SectionOption(All, ShowOrder = 1400, GenerateOrder = 300)]
        ENR6,

        [SectionOption(All & ~AIXM_Source & ~Fill & ~AMDT & ~GenerateSubsection, ShowOrder = 2060)]
        AD06, // no information for electron aip

        [SectionOption(All, ShowOrder = 2070)]
        AD11,
        AD11_1,
        AD11_2,
        AD11_3,
        AD11_4,
        AD11_5,
        
        [SectionOption(All, ShowOrder = 2080)]
        AD12,
        AD12_1,
        AD12_2,

        [SectionOption(All, ShowOrder = 2090)]
        AD13,

        [SectionOption(All, ShowOrder = 2100)]
        AD14,

        [SectionOption(All, ShowOrder = 2110)]
        AD15, // problem

        [SectionOption(All & ~GenerateSubsection & ~TextSection, ShowOrder = 3000)]
        AD2,
        [SectionOption(All & ~GenerateSubsection & ~TextSection, ShowOrder = 3000)]
        AD20 = AD2, // alias
        AD21, // part of airportHeliport method
        AD22, // part of airportHeliport method
        AD23, // part of airportHeliport method
        AD24, // part of airportHeliport method
        AD25, // part of airportHeliport method
        AD26, // part of airportHeliport method
        AD27, // part of airportHeliport method
        AD28, // part of airportHeliport method
        AD29, // part of airportHeliport method
        AD210, // part of airportHeliport method
        AD211, // part of airportHeliport method
        AD212, // part of airportHeliport method
        AD213, // part of airportHeliport method
        AD214, // part of airportHeliport method
        AD215, // part of airportHeliport method
        AD216, // part of airportHeliport method
        AD217, // part of airportHeliport method
        AD218, // part of airportHeliport method
        AD219, // part of airportHeliport method
        AD220, // part of airportHeliport method
        AD221, // part of airportHeliport method
        AD222, // part of airportHeliport method
        AD223, // part of airportHeliport method
        AD224, // part of airportHeliport method

        [SectionOption(All & ~GenerateSubsection & ~TextSection, ShowOrder = 4000)]
        AD3,
        [SectionOption(All & ~GenerateSubsection & ~TextSection, ShowOrder = 4000)]
        AD30 = AD3, // alias
        AD31, // part of airportHeliport method
        AD32, // part of airportHeliport method 
        AD33, // part of airportHeliport method
        AD34, // part of airportHeliport method
        AD35, // part of airportHeliport method
        AD36, // part of airportHeliport method
        AD37, // part of airportHeliport method
        AD38, // part of airportHeliport method
        AD39, // part of airportHeliport method
        AD310, // part of airportHeliport method
        AD311, // part of airportHeliport method
        AD312, // part of airportHeliport method
        AD313, // part of airportHeliport method
        AD314, // part of airportHeliport method
        AD315, // part of airportHeliport method
        AD316, // part of airportHeliport method
        AD317, // part of airportHeliport method
        AD318, // part of airportHeliport method
        AD319, // part of airportHeliport method
        AD320, // part of airportHeliport method
        AD321, // part of airportHeliport method
        AD322, // part of airportHeliport method
        AD323, // part of airportHeliport method

        [SectionOption(All & ~AIXM_Source & ~Fill & ~AMDT & ~GenerateSubsection, ShowOrder = 2010)]
        AD01,

        [SectionOption(All & ~AIXM_Source & ~Fill & ~AMDT & ~GenerateSubsection, ShowOrder = 2020)]
        AD02,

        [SectionOption(All & ~AIXM_Source & ~Fill & ~AMDT & ~GenerateSubsection, ShowOrder = 2030)]
        AD03,

        [SectionOption(All & ~AIXM_Source & ~Fill & ~AMDT & ~GenerateSubsection, ShowOrder = 2040)]
        AD04,

        [SectionOption(All & ~AIXM_Source & ~Fill & ~AMDT & ~GenerateSubsection, ShowOrder = 2050)]
        AD05
        // ^^^
        // New sections add above
    }
    

    public enum Status
    {
        None = 0,
        Work = 1,
        PreparePublish = 10,
        Published = 20,
        Uploaded = 30
    }

    public enum showType
    {
        @new,
        replace,
        embed,
        other,
        none,
    }

    public enum SectionStatusEnum
    {
        None = 0,
        Work = 1,
        Filled = 2
    }

    public enum NILReason
    {
        None = 0,
        Electronic = 1,
        Notavailable = 2,
        Inpreparation = 3,
        Notapplicable = 4,
    }

    public enum AmendmentType
    {
        AIRAC,
        NonAIRAC,
        Nonlisted,
    }

    public enum AmendmentStatus
    {
        None,
        NotAvailable,
        Available
    }

    public enum GroupType
    {
        Misc,
        GEN,
        ENR,
        AD
    }
    
    
    public enum LocationDefinitionType
    {
        ICAO,
        Nonstandard
    }


    public enum DocType
    {
        None,
        HTML,
        PDF
    }
    
    public enum PageType
    {
        None,
        Cover
    }
}
