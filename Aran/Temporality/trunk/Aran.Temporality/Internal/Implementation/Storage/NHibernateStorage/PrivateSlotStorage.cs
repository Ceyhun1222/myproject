using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class PrivateSlotStorage : CrudStorageTemplate<PrivateSlot>, IPrivateSlotStorage
    {
        #region Implementation of IPrivateSloteStorage

        public IList<PrivateSlot> GetPrivateSlots(int publicSlotId, int userId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    //TODO: add restrictions
                    return session.CreateCriteria(typeof(PrivateSlot)).
                    Add(Restrictions.Eq("PublicSlot", new PublicSlot{Id= publicSlotId})).
                    List<PrivateSlot>();
                }
            }
        }

        #endregion

        public bool UpdatePrivateSlot(PrivateSlot privateSlot)
        {
            if (privateSlot == null) return false;
            if (privateSlot.Id == 0) return false;

            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var slot = session.Get<PrivateSlot>(privateSlot.Id);

                    if (slot != null)
                    {
                        //do update
                        slot.Name = privateSlot.Name;
                        slot.Reason = privateSlot.Reason;

                        slot.Status = privateSlot.Status;

                        //if (slot.Status != privateSlot.Status)
                        {
                            slot.StatusChangeDate = DateTime.Now;
                        }
                  
                        session.Update(slot);
                        transaction.Commit();
                    }
                }
            }

            return true;
        }

        public PrivateSlot GetFirstAndSetStatus(SlotStatus initialStatus, SlotStatus nextStatus)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var slots = session.CreateCriteria(typeof(PrivateSlot)).
                    Add(Restrictions.Eq("Status", initialStatus)).SetMaxResults(1).List<PrivateSlot>();

                if (slots != null && slots.Count > 0)
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var slot = slots.First();
                        slot.Status = nextStatus;
                        slot.StatusChangeDate = DateTime.Now;
                        session.Update(slot);
                        transaction.Commit();
                        return slot;
                    }
                }
            }

            return null;
        }

        public void ResetSlotStatus()
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var slots = session.CreateCriteria(typeof(PrivateSlot)).
                    Add(Restrictions.Eq("Status", SlotStatus.Checking)).List<PrivateSlot>();

                if (slots != null && slots.Count > 0)
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        foreach (var slot in slots)
                        {
                            slot.Status = SlotStatus.CheckCancelled;
                            //slot.Status = SlotStatus.ToBeChecked;
                            session.Update(slot);
                        }
                        transaction.Commit();
                    }
                }
            }
        }

        public bool DeleteById(int id)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var privateSlot = session.Get<PrivateSlot>(id);
                        if (privateSlot != null)
                        {
                            var users = session.CreateCriteria(typeof(User)).Add(Restrictions.Eq("ActivePrivateSlot", privateSlot)).List<User>();
                            foreach (var user in users)
                            {
                                user.ActivePrivateSlot = null;
                                session.Update(user);
                            }

                            var reports = session.CreateCriteria(typeof(ProblemReport)).Add(Restrictions.Eq("PrivateSlotId", id)).List<ProblemReport>();
                            foreach (var report in reports)
                            {
                                session.Delete(report);
                            }

                            var options = session.CreateCriteria(typeof(SlotValidationOption)).Add(Restrictions.Eq("PrivateSlot", privateSlot)).List<SlotValidationOption>();
                            foreach (var option in options)
                            {
                                session.Delete(option);
                            }



                            session.Delete(privateSlot);
                            transaction.Commit();
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                    return false;
                }
            }
        }

        public void PreStart()
        {
           
        }
    }
}
