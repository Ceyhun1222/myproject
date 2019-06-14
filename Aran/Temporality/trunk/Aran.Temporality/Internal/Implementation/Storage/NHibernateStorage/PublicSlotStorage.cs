using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class PublicSlotStorage : CrudStorageTemplate<PublicSlot>, IPublicSlotStorage
    {

        public bool UpdatePublicSlot(PublicSlot publicSlot)
        {
            if (publicSlot == null) return false;
            if (publicSlot.Id == 0) return false;

            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var slot=session.Get<PublicSlot>(publicSlot.Id);

                    if (slot != null)
                    {
                        //do update
                        slot.Name = publicSlot.Name;
                        slot.PlannedCommitDate = publicSlot.PlannedCommitDate;
                        //if (slot.Status != publicSlot.Status)
                        //{
                        //    slot.StatusChangedDate = DateTime.Now;
                        //}
                        slot.Status = publicSlot.Status;
                        //slot.EffectiveDate can not be updated

                        slot.SlotType = publicSlot.SlotType;

                        session.Update(slot);
                        transaction.Commit();
                    }
                }
            }

            return true;
        }

        public void ResetSlotStatus()
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var slots = session.CreateCriteria(typeof(PublicSlot)).
                    Add(Restrictions.Eq("Status", SlotStatus.Checking)).List<PublicSlot>();
                if (slots != null && slots.Count > 0)
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        foreach (var slot in slots)
                        {
                            slot.Status = SlotStatus.ToBeChecked;
                            session.Update(slot);
                        }
                        transaction.Commit();
                    }
                }
            }
        }

        public PublicSlot GetFirstAndSetStatus(SlotStatus initialStatus, SlotStatus nextStatus)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var slots = session.CreateCriteria(typeof(PublicSlot)).
                    Add(Restrictions.Eq("Status", initialStatus)).SetMaxResults(1).List<PublicSlot>();

                if (slots != null && slots.Count > 0)
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var slot = slots.First();
                        slot.Status = nextStatus;
                        session.Update(slot);
                        transaction.Commit();
                        return slot;
                    }
                }
            }

            return null;
        }
    }
}
