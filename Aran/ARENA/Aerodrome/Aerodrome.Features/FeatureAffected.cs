using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Aerodrome.Enums;

namespace Aerodrome.Features
{
	public class FeatureAffected
	{
        public void AddAffectedFeat ( Feat_Base feat_type, string id )
		{
			Feat_Type = feat_type;
			Id = id;
		}

        public Feat_Base Feat_Type { get; set; }

        public string Id { get; set; }
    }
}