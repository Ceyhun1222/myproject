using System.Collections.Generic;
using System.Linq;
using Aran.Aim.Utilities;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;

namespace Aran.Temporality.Internal.WorkFlow.Routines
{
	internal class MissingLinkRoutine : AbstractCheckRoutine
	{
		#region Implementation of ICheckPublicSlotRoutine

	    public override int GetReportType()
	    {
	        return (int) ReportType.MissingLinkReport;
	    }


        public override bool CheckFeature(AimFeature aimFeature, List<ProblemReportUtil> problems)
		{
            CurrentOperationStatus.NextOperation();
            NextFeature();
            var result = true;
            var feat = aimFeature.Feature;
            var featurePropList = new List<RefFeatureProp>();
            AimMetadataUtility.GetReferencesFeatures(feat, featurePropList);

			foreach ( var featureProp in featurePropList )
			{
				if ( featureProp.FeatureType == 0 )
				{
				    result = false;
					problems.Add(new LinkProblemReportUtil
					{
						FeatureType = feat.FeatureType,
						Guid = feat.Identifier,
						ReferenceFeatureType = "Abstract",
						PropertyPath = string.Join ( ".", featureProp.PropInfoList.Select ( t => t.Name ) )
					} );
				}
				else
				{
				    var linkedFeature = Context.LoadFeature(featureProp.FeatureType, featureProp.RefIdentifier);
                       
                    if (linkedFeature?.Feature?.TimeSlice?.FeatureLifetime == null || linkedFeature.Feature.TimeSlice.FeatureLifetime.EndPosition <= Context.EffectiveDate)
					{
                        result = false;
						problems.Add ( new LinkProblemReportUtil
						{
							FeatureType = feat.FeatureType,
							Guid = feat.Identifier,
                            ReferenceFeatureType = featureProp.FeatureType.ToString(),
							ReferenceFeatureIdentifier = featureProp.RefIdentifier,
							PropertyPath = string.Join ( ".", featureProp.PropInfoList.Select ( t => t.Name ) )
						} );
					}
				}
				

			}

            if (!result)
            {
                NextError();
            }
            return result;
		}


		#endregion
	}
}