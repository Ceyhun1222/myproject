#region

using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.MetaData.Offset;

#endregion

namespace Aran.Temporality.Internal.MetaData.Util
{
    internal class StateMetaCache<TDataType, TOffsetType> : MetaCache<StateMetaSet<TDataType, TOffsetType>>
        where TDataType : OffsetStateMetaData<TOffsetType>
    {
        public IList<TDataType> GetByFeatureTypeNameBefore(IFeatureId meta, DateTime dateTime, bool equalDateIsOk)
        {
            IList<StateMetaSet<TDataType, TOffsetType>> containers = GetByFeatureTypeName(meta);
            return containers.Select(container => container.GetLastItemBefore(dateTime, equalDateIsOk)).ToList();
        }

        public IList<TDataType> GetAllValid()
        {
            var result = new List<TDataType>();
            foreach (var pair in MetaByWorkPackages.Values)
            {
                foreach (var pair2 in pair.Values)
                {
                    foreach (var stateMetaSet in pair2.Values)
                    {
                        result.AddRange(stateMetaSet.GetValid());
                    }
                }
            }
            return result;
        }

        public TDataType PokeAny(int workpackage)
        {
            SortedList<int, SortedList<Guid, StateMetaSet<TDataType, TOffsetType>>> value;
            if (MetaByWorkPackages.TryGetValue(workpackage, out value))
            {
                foreach (var pair2 in value.Values)
                {
                    foreach (var stateMetaSet in pair2.Values)
                    {
                        var item = stateMetaSet.PokeAny();
                        if (item != null)
                        {
                            return item;
                        }
                    }
                }
            }
            return null;
        }

        public TDataType PokeInvalid()
        {
            {
                foreach (var pair in MetaByWorkPackages.Values)
                {
                    foreach (var pair2 in pair.Values)
                    {
                        foreach (var stateMetaSet in pair2.Values)
                        {
                            TDataType item = stateMetaSet.PokeInvalid();
                            if (item != null)
                            {
                                return item;
                            }
                        }
                    }
                }
            }
            

            return null;
        }

        public void PrepareForDeleteWorkPackage(int workPackage)
        {
              SortedList<int, SortedList<Guid, StateMetaSet<TDataType, TOffsetType>>> value;
              if (MetaByWorkPackages.TryGetValue(workPackage, out value))
              {
                  foreach (SortedList<Guid, StateMetaSet<TDataType, TOffsetType>> pair in value.Values)
                  {
                      foreach (StateMetaSet<TDataType, TOffsetType> stateMetaSet in pair.Values)
                      {
                          stateMetaSet.RemoveAllCSA();
                      }
                  }
              }
        }
    }
}