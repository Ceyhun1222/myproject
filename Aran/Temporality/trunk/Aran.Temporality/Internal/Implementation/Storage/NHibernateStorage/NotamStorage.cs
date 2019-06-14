using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Exceptions;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class NotamStorage : CrudStorageTemplate<Notam>, INotamStorage
    {
        public int SaveNotam(Notam notam)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var notams = session.QueryOver<Notam>()
                    .Where(t => t.Type == notam.Type
                                && t.Series == notam.Series && t.Number == notam.Number && t.Year == notam.Year)
                    .List<Notam>();

                if (notams != null && notams.Count > 0)
                    throw new OperationException("Unique constraints violated");
            }

            return CreateEntity(notam);
        }

        public bool UpdateNotam(Notam notam)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var entity = session.Get<Notam>(notam.Id);
                if (entity != null)
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        entity.Code23 = notam.Code23;
                        entity.Code45 = notam.Code45;
                        entity.Coordinates = notam.Coordinates;
                        entity.CreatedOn = notam.CreatedOn;
                        entity.EndValidity = notam.EndValidity;
                        entity.EndValidityEst = notam.EndValidityEst;
                        entity.FIR = notam.FIR;
                        entity.Format = notam.Format;
                        entity.ICAO = notam.ICAO;
                        entity.ItemE = notam.ItemE;
                        entity.ItemF = notam.ItemF;
                        entity.ItemG = notam.ItemG;
                        entity.Lower = notam.Lower;
                        entity.Number = notam.Number;
                        entity.Purpose = notam.Purpose;
                        entity.Radius = notam.Radius;
                        entity.RefNotam = notam.RefNotam;
                        entity.Schedule = notam.Schedule;
                        entity.Scope = notam.Scope;
                        entity.Series = notam.Series;
                        entity.StartValidity = notam.StartValidity;
                        entity.Text = notam.Text;
                        entity.Traffic = notam.Traffic;
                        entity.Type = notam.Type;
                        entity.Upper = notam.Upper;
                        entity.UserName = notam.UserName;
                        entity.Year = notam.Year;
                        session.Update(entity);
                        transaction.Commit();
                        return true;
                    }
                }
                return false;
            }
        }
    }
}