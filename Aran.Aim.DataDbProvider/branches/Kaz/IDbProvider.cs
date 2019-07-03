using Aran.Aim.Features;
using System.Collections.Generic;
using Aran.Aim.Data.Filters;
using System;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using System.Data;

namespace Aran.Aim.Data
{
    public interface IDbProvider
    {        
        void Open (string connectionString, out string errorString);

        void Close ();
        
        ConnectionState State
        {
            get;
        }

        int BeginTransaction ();
		InsertingResult Insert ( Feature feature );
		//InsertingResult Insert ( Feature feature, bool insertAnyway );
		//InsertingResult Insert ( Feature feature, bool insertAnyway, bool asCorrection );
		InsertingResult Insert ( Feature feature, int transactionId );		
		//InsertingResult Insert ( Feature feature, int transactionId, bool insertAnyway );
		//InsertingResult Insert ( Feature feature, int transactionId, bool insertAnyway, bool asCorrection );
		string Delete ( Feature feature );
		InsertingResult Commit ( int transactionId );
		InsertingResult Roollback ( int transactionId );
		GettingResult GetFeat ( FeatureType featType,
							  Guid identifier = default(Guid),
							  bool loadComplexProps = false,
							  List<string> propList = null,
							  Filter filter = null );
		GettingResult GetFeat ( IAbstractFeatureRef absFeatRef,
							  bool loadComplexProps = false,
							  List<string> propList = null,
							  Filter filter = null );
		GettingResult GetAllStoredFeatIndices ( );

		//GettingResult GetVersionsOf (FeatureType featType,
		//                      //TimeSliceInterpretationType interpretation,
		//                      Guid identifier = default(Guid),
		//                      bool loadComplexProps = false,
		//                      TimeSliceFilter timeSlicefilter = null,
		//                      List<string> propList = null,
		//                      Filter filter = null);

		//GettingResult GetVersionsOf (IAbstractFeatureRef absFeatRef,
		//                      //TimeSliceInterpretationType interpretation,
		//                      bool loadComplexProps = false,
		//                      TimeSliceFilter timeSliceFilter = null,
		//                      List<string> propList = null,
		//                      Filter filter = null);

		//GettingResult GetVersionsOf (AbstractFeatureType absFeatType,
		//                      //TimeSliceInterpretationType interpretation,
		//                      bool loadComplexProps = false,
		//                      TimeSliceFilter timeSlicefilter = null,
		//                      List<string> propList = null,
		//                      Filter filter = null);

		//TimeSliceFilter TimeSliceFilter
		//{
		//    get;
		//    set;
		//}

		string [] GetLastErrors ();
    }
}
