using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Aim.Storage;
using Aran.Temporality.Common.Interface;

namespace Aran.Temporality.Aim.Provider
{
    class TemporalityProvider : IDbProvider
    {
        private IStorage<Feature> _storage;


        #region Implementation of IDbProvider

        public string Open(string connectionString)
        {
            try
            {
                _storage = AimStorageFactory.CreateLocal(connectionString);
                return string.Empty;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void Close()
        {
        }

        public ConnectionState State
        {
            get { throw new NotImplementedException(); }
        }

        public int BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public InsertingResult Insert(Feature feature)
        {
            throw new NotImplementedException();
        }

        public InsertingResult Insert(Feature feature, bool insertAnyway)
        {
            throw new NotImplementedException();
        }

        public InsertingResult Insert(Feature feature, bool insertAnyway, bool asCorrection)
        {
            throw new NotImplementedException();
        }

        public InsertingResult Insert(Feature feature, int transactionId)
        {
            throw new NotImplementedException();
        }

        public InsertingResult Insert(Feature feature, int transactionId, bool insertAnyway)
        {
            throw new NotImplementedException();
        }

        public InsertingResult Insert(Feature feature, int transactionId, bool insertAnyway, bool asCorrection)
        {
            throw new NotImplementedException();
        }

        public InsertingResult Commit(int transactionId)
        {
            throw new NotImplementedException();
        }

        public InsertingResult Rollback(int transactionId)
        {
            throw new NotImplementedException();
        }

        public GettingResult GetVersionsOf(FeatureType featType, TimeSliceInterpretationType interpretation, Guid identifier = new Guid(), bool loadComplexProps = false, TimeSliceFilter timeSlicefilter = null, List<string> propList = null, Filter filter = null)
        {
            throw new NotImplementedException();
        }

        public GettingResult GetVersionsOf(IAbstractFeatureRef absFeatRef, TimeSliceInterpretationType interpretation, bool loadComplexProps = false, TimeSliceFilter timeSliceFilter = null, List<string> propList = null, Filter filter = null)
        {
            throw new NotImplementedException();
        }

        public GettingResult GetVersionsOf(AbstractFeatureType absFeatType, TimeSliceInterpretationType interpretation, bool loadComplexProps = false, TimeSliceFilter timeSlicefilter = null, List<string> propList = null, Filter filter = null)
        {
            throw new NotImplementedException();
        }

        //no filter
        public List<Feature> GetAllFeatuers(FeatureType featType, Guid identifier = new Guid())
        {
            throw new NotImplementedException();
        }

       
        public TimeSliceFilter TimeSliceFilter{ get; set; }

    

        public string[] GetLastErrors()
        {
            throw new NotImplementedException();
        }

  
        //filter
        public bool IsExists(Guid guid)
        {
            return false;
            //_storage.GetIds();
        }

        #endregion


        public InsertingResult Update(Feature feature)
        {
            throw new NotImplementedException();
        }

        public InsertingResult Update(Feature feature, int transactionId)
        {
            throw new NotImplementedException();
        }

        public InsertingResult Delete(Feature feature)
        {
            throw new NotImplementedException();
        }


        public InsertingResult InsertUser(User user)
        {
            throw new NotImplementedException();
        }

        public InsertingResult UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public InsertingResult DeleteUser(User user)
        {
            throw new NotImplementedException();
        }


        public GettingResult GetAllStoredFeatTypes()
        {
            throw new NotImplementedException();
        }

        public GettingResult GelAllStoredIdentifiers()
        {
            throw new NotImplementedException();
        }

        public GettingResult DeleteFeatIdentifiers(List<Guid> identifierList)
        {
            throw new NotImplementedException();
        }


        #region

        public User User
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public GettingResult ReadUsers()
        {
            throw new NotImplementedException();
        }

        public GettingResult ReadUserByName(string userName)
        {
            throw new NotImplementedException();
        }

        public GettingResult ReadUsersById(long userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
