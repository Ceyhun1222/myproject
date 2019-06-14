using Aerodrome.Enums;
using BusinessCore;
using Framework.Attributes;
//using Framework.Stasy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace Aerodrome.Features
{
    public partial class AM_AbstractFeature : BusinessObjectBase<AM_AbstractFeature>
    {
        

        [CrudPropertyConfiguration("Start life", DisplayInGrid = false, PropertyCategory = PropertyCategories.Metadata, PropertyRequirement = PropertyRequirements.Mandatory)]
        [Description("Start of life of the feature, indicating the date and time at which the feature starts to exist")]
        public DateTime stfeat { get; set; }


        [CrudPropertyConfiguration("End life", DisplayInGrid = false, PropertyCategory = PropertyCategories.Metadata)]
        [Description("End of life of the feature, indicating the date and time at which the feature ceases to exist")]
        public AM_Nullable<DateTime> endfeat { get; set; }


        [CrudPropertyConfiguration("Start valid", DisplayInGrid = false, PropertyCategory = PropertyCategories.Metadata, PropertyRequirement = PropertyRequirements.Mandatory)]
        [Description("Date and time at which the data contained in the feature state starts to be effective ")]
        public DateTime stvalid { get; set; }

        [CrudPropertyConfiguration("End valid", DisplayInGrid = false, PropertyCategory = PropertyCategories.Metadata)]
        [Description("Date and time at which the data contained in the feature state ceases to be effective")]
        public AM_Nullable<DateTime> endvalid { get; set; }

        [CrudPropertyConfiguration("Interpretation", DisplayInGrid = false, PropertyCategory = PropertyCategories.Metadata)]
        [Description("How the feature state is to be interpreted")]
        public AM_Nullable<AM_InterpretationType> interp { get; set; }


        [Description ( "Feature type" )]
		public virtual Feat_Type Feattype { get; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        [Description("Special unique identifier permanently assigned to a feature by the data provider")]
        [MaxLength(36)]
        public string idnumber { get; set; }


        [CrudPropertyConfiguration("Source", DisplayInGrid = false, PropertyCategory = PropertyCategories.Metadata)]
        [Description("Name of entity or organization that supplied data according to RTCA DO-200B/EUROCAE ED-76A")]
        [MaxLength(254)]
        public AM_Nullable<string> source { get; set; }

        [CrudPropertyConfiguration("Revision date", DisplayInGrid = false, PropertyCategory = PropertyCategories.Metadata)]
        [Description("Last revision date and time of data")]
        public AM_Nullable<DateTime> revdate { get; set; }

        [PrimaryKey]       
        public string featureID { get; set; }

        //[CrudPropertyConfiguration]
        [Browsable(false)]
        public int sequence { get; set; }

        private int _correction;
        //[CrudPropertyConfiguration("Correction")]
        [Browsable(false)]
        public int correction
        {
            get
            {
                return _correction;
            }
            set
            {
                _correction = value;
                SendPropertyChanged("correction");
            }
        }
        [Browsable(false)]
        public override string Descriptor => "No name";
    }
}
