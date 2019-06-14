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
	public abstract partial class AM_FeatureBase : AM_AbstractFeature
	{
        [CrudPropertyConfiguration("Horizontal Accuracy", PropertyCategory = PropertyCategories.Metadata, PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> hacc { get; set; }

        [CrudPropertyConfiguration("Horizontal Resolution", PropertyCategory = PropertyCategories.Metadata, PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomHorResolution> hres { get; set; }

        
        [CrudPropertyConfiguration("Integrity", PropertyCategory = PropertyCategories.Metadata)]
        public AM_Nullable<double> integr { get; set; }
    }
}