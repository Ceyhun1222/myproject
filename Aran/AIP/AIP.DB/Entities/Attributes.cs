using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.AttributeTargets;

namespace AIP.DB
{
    /// <inheritdoc />
    /// <summary>
    /// DataEntryOption is attribute to show/hide, and make read/edit information in the 
    /// forms which are use DataEntry
    /// Example is shown in the AIPFileForm form
    /// </summary>
    public class DataEntryOption : Attribute
    {
        public bool Visible { get; set; }
        public bool ReadOnly { get; set; }
        public int RowSpan { get; set; }

        public DataEntryOption()
        {
            RowSpan = 1;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// GridViewOption is attribute to show/hide, and make read/edit information in the 
    /// forms which are use DataGridView
    /// Example is shown in the AIPFile form
    /// </summary>
    public class GridViewOption : Attribute
    {
        public bool Visible { get; set; }
        public bool ReadOnly { get; set; }
        public bool RenderHTML { get; set; }
        public int MaxWidth { get; set; }

        public GridViewOption()
        {
            RenderHTML = false;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// SectionOption is attribute to 
    /// show what sections are requiring for auto Fill/Generate AIP
    /// and how they will be filled and generated
    /// </summary>
    /// 
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class SectionOptionAttribute : Attribute
    {
        internal SectionParameter SectionParameter; 

        // Constructor
        public SectionOptionAttribute(SectionParameter validOn)
        {
            SectionParameter = validOn;
            GenerateOrder = Int32.MaxValue-100;
            ShowOrder = 200;
        }

        // See SectionParameter
        public SectionParameter ValidOn => SectionParameter;

        // Order used for generating sections
        public int GenerateOrder { get; set; }

        // Order to show in the html menu, 
        // Order file eAIP-en-GB.xml and then EC toolbox generates html menu
        // Using for GEN 0.4 section and Cover page (Inserted/Deleted) pages
        public int ShowOrder { get; set; }
    }

    [Flags]
    public enum SectionParameter
    {
        // Default
        None = 0,

        // Require to write mapping data from AIXM into AIP DB using special Fill_SECTION_NAME method in the FillDB/ classes
        // Some Section may be defined as just a part of main sections and must have Fill = false (ex AD2xx and AD3xx)
        Fill = 1 << 0,

        // In addition to Fill, if for section using common method Fill_Subsection.
        // This section contain one or more subsections
        // Have higher priority that Fill,
        // If disabled and Fill enabled, Fill flag will be used
        FillSubsection = 1 << 1,

        // Like Fill, Build_SECTION_NAME method will be used from DS2XML class
        // Else set false
        Generate = 1 << 2,

        // Section may contains only subsections in the AIP.DB, already mapped using Fill or FillSubsection
        // Such sections will use common method to generate xml/html/pdf
        // Have higher priority that Generate,
        // ELSE - special for each section DS2XML.Build_SECTION_NAME method will be used
        GenerateSubsection = 1 << 3,

        // This section should be generated for menu
        // For example AD2 and AD3 not a TextSection and generating separately
        TextSection = 1 << 4,

        // This section should be used for analizing PDF content for amdt
        // Will detect pages changed and replace date with new one, if content changed
        PDFPages = 1 << 5,

        // Amendment should be generated for this section. HtmlDiff project will process xml.1 and xml.2 files to find changed
        // Else section will be created without showing changes
        // Prevous AIP must contains data in the AIP.DB and method generate it to create xml.1 file
        AMDT = 1 << 6,

        // AIXM Source must be used to generate this sections.
        // Else AIP DB (ex: Abbreviations or files) or no source will be used (ex: GEN 0.6 Table of Content generates by EC toolbox)
        // If, no data contain in the AIXM and getting from AIP DB set it to false
        AIXM_Source = 1 << 7,

        // All, Do not change
        // If new attribute
        All = ~0
    }
    
}
