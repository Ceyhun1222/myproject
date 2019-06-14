using System.Collections;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Temporality.Common.Entity.Enum;

namespace Aran.Temporality.Common.Entity.Util
{
    public class AccessRightUtil
    {
        public virtual int MyGroupId { get; set; } //can not be null
        public virtual int MyStorageId { get; set; } //can not be null
        public virtual int MyWorkPackage { get; set; } //-1 means any, 0 means default
        
        public virtual int MyFeatureTypeId { get; set; } //-1 means any
        public virtual int MyOperationFlag { get; set; }

        #region Zip/Unzip data

        public static List<AccessRightUtil> DecodeRights(AccessRightZipped zipped)
        {
            var accessData = new BitArray(zipped.ZippedData);
            var result = new List<AccessRightUtil>();

            if (zipped.OperationFlag > 0)
            {
                result.Add(new AccessRightUtil
                               {
                                   MyGroupId = zipped.UserGroupId,
                                   MyStorageId = zipped.StorageId,
                                   MyWorkPackage = zipped.WorkPackage,
                                   MyFeatureTypeId = -1,
                                   MyOperationFlag = zipped.OperationFlag
                               });
            }


            foreach (var feature in Features)
            {
                var index = GetIndexByType(feature);

                var read = accessData.Get(index * 2);
                var full = accessData.Get(index * 2 + 1);

                var accessFlag = 0;
                if (read) accessFlag |= (int)DataOperation.ReadData;
                if (full) accessFlag |= (int)DataOperation.WriteData;

                result.Add(new AccessRightUtil
                               {
                                   MyGroupId = zipped.UserGroupId,
                                   MyStorageId = zipped.StorageId,
                                   MyWorkPackage = zipped.WorkPackage,
                                   MyFeatureTypeId = (int)feature,
                                   MyOperationFlag = accessFlag
                               });
            }


            return result;
        }

        public static void SetAccess(AccessRightZipped zipped, List<AccessRightUtil> list)
        {
            var accessData = new BitArray(Features.Count * 2);

            foreach (var accessRight in list)
            {
                var accessFlag = accessRight.MyOperationFlag;
                var read = (accessFlag & (int)DataOperation.ReadData) != 0;
                var full = (accessFlag & (int)DataOperation.WriteData) != 0;
                var index = GetIndexByType((FeatureType)accessRight.MyFeatureTypeId);

                accessData.Set(index * 2, read);
                accessData.Set(index * 2 + 1, full);
            }


            var count = accessData.Count;
            if (count % 8 != 0)
            {
                count = count / 8;//from bit to byte
                count++;
            }
            else
            {
                count = count / 8;//from bit to byte
            }

            if (count == 0)
            {
                zipped.ZippedData = null;
                return;
            }

            zipped.ZippedData = new byte[count];
            accessData.CopyTo(zipped.ZippedData, 0);
        }

        public static bool IsDataEqual(AccessRightZipped zipped1, AccessRightZipped zipped2)
        {
            if (zipped1 == null && zipped2 == null) return true;
            if (zipped1 != null && zipped2 == null) return false;
            if (zipped1 == null) return false;

            var accessData1 = new BitArray(zipped1.ZippedData);
            var accessData2 = new BitArray(zipped2.ZippedData);
            
            foreach (var feature in Features)
            {
                  var index = GetIndexByType(feature);
                  if (accessData1.Get(index * 2) != accessData2.Get(index * 2))
                      return false;
                  if (accessData1.Get(index * 2+1) != accessData2.Get(index * 2+1))
                      return false;
            }

            return true;
        }

        #endregion

        #region Feature Type to Index mapping

        private static List<FeatureType> _features;
        private static List<FeatureType> Features
        {
            get
            {
                InitFeatures();
                return _features;
            }
        }

        private static void InitFeatures()
        {
            if (_features == null)
            {
                _features = new List<FeatureType>((FeatureType[])System.Enum.GetValues(typeof(FeatureType)));
                _features.Sort();
            }
        }

        private static int GetIndexByType(FeatureType type)
        {
            return Features.IndexOf(type);
        }

        #endregion

        public static void SetZippedData(AccessRightZipped target, AccessRightZipped source)
        {
            target.ZippedData = new byte[source.ZippedData.Length];
            for (int i = 0; i < source.ZippedData.Length; i++)
            {
                target.ZippedData[i] = source.ZippedData[i];
            }
        }
    }
}
