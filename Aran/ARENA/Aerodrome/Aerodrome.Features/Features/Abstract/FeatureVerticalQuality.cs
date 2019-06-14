using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Aerodrome.DataType;
using Aerodrome.Enums;
using Framework.Attributes;

namespace Aerodrome.Features
{
	public abstract partial class AM_FeatureVerticalQuality : AM_FeatureBase
	{        
        [CrudPropertyConfiguration("Vertical Accuracy",PropertyCategory = PropertyCategories.Metadata, PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> vacc { get; set; }
       
        [CrudPropertyConfiguration("Vertical Resolution", PropertyCategory = PropertyCategories.Metadata, PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomVerResolution> vres { get; set; }
    }
}