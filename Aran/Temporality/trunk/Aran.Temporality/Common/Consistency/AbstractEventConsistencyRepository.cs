using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Internal.Implementation.Storage;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Aran.Temporality.Common.Consistency
{
    public abstract class AbstractConsistencyRepository<T> where T : class, ISerializable
    {
        #region Constructors

        public AbstractConsistencyRepository(RepositoryType repositoryType, string storageName)
        {
            RepositoryType = repositoryType;

            StorageName = storageName;
        }

        #endregion

        #region Abstract methods

        public abstract void Add(T value, HashAlgorithm hashAlgorithm = null);

        public abstract bool Add(List<T> values, HashAlgorithm hashAlgorithm = null);

        public abstract List<EventConsistency> Get(int workPackage);

        #endregion

        #region Private fields

        protected RepositoryType RepositoryType { get; }

        protected string StorageName { get; }

        #endregion

        #region Methods

        BinaryFormatter BinaryFormatter = new BinaryFormatter();
        public string CalculateConsistency(T value, HashAlgorithm hashAlgorithm = null)
        {
            var json = value.ToJson();

            if (hashAlgorithm == null)
                hashAlgorithm = MD5.Create();

            var memoryStream = new MemoryStream();
            BinaryFormatter.Serialize(memoryStream, json);

            byte[] hash = hashAlgorithm.ComputeHash(memoryStream.GetBuffer());

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public void ClearConsistencies()
        {
            StorageService.ClearConsistency(RepositoryType, StorageName);
        }

        #endregion
    }
}
