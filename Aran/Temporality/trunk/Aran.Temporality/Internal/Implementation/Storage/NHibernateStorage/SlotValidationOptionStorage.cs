using Aran.Temporality.Common.Entity;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class  SlotValidationOptionStorage : CrudStorageTemplate<SlotValidationOption>, ISlotValidationOptionStorage
    {
        public SlotValidationOption GetOptionBySlotId(int slotId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    return session.CreateCriteria(typeof(SlotValidationOption)).
                    Add(Restrictions.Eq("PrivateSlot", new PrivateSlot{ Id = slotId })).
                    UniqueResult<SlotValidationOption>();
                }
            }
        }

        public bool UpdateSlotValidationOption(int slotId, SlotValidationOption newOption)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var old = session.CreateCriteria(typeof(SlotValidationOption)).
                            Add(Restrictions.Eq("PrivateSlot", new PrivateSlot { Id = slotId })).
                            UniqueResult<SlotValidationOption>();

                        if (old == null)
                        {
                            session.Save(newOption);
                        }
                        else
                        {
                            old.Flag = newOption.Flag;
                            old.MoreOptions = newOption.MoreOptions;
                            session.Update(old);
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }

            return false;
        }
    }
}
