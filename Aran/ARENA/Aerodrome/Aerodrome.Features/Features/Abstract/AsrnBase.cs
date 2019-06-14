using Framework.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Aerodrome.Features
{
	public abstract partial class AM_AsrnBase : AM_AbstractFeature
	{
        [CrudPropertyConfiguration]
        [MaxLength(254)]
        public AM_Nullable<string> idnetwrk { get; set; }

        //Nullable
        public AM_DeicingGroup RelatedDeicGroup { get; set; }
    }
}
