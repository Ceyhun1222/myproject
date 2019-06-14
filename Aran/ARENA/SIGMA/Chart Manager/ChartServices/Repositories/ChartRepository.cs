using ChartServices.DataContract;
using ChartServices.Helpers;
using ChartServices.Repositories;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartManagerServices.Repositories
{
    public class ChartRepository : Repository<Chart>
    {
        private static readonly ProjectionList _chartProjections;

        static ChartRepository()
        {
            _chartProjections = Projections.ProjectionList()
                .Add(Projections.Property(nameof(Chart.Airport)), nameof(Chart.Airport))
                .Add(Projections.Property(nameof(Chart.BeginEffectiveDate)), nameof(Chart.BeginEffectiveDate))
                .Add(Projections.Property(nameof(Chart.CreatedAt)), nameof(Chart.CreatedAt))
                .Add(Projections.Property(nameof(Chart.CreatedBy)), nameof(Chart.CreatedBy))
                .Add(Projections.Property(nameof(Chart.EndEffectiveDate)), nameof(Chart.EndEffectiveDate))
                //.Add(Projections.Property(nameof(Chart.HasUpdate)), nameof(Chart.HasUpdate))
                .Add(Projections.Property(nameof(Chart.Id)), nameof(Chart.Id))
                .Add(Projections.Property(nameof(Chart.Identifier)), nameof(Chart.Identifier))
                .Add(Projections.Property(nameof(Chart.IsLocked)), nameof(Chart.IsLocked))
                .Add(Projections.Property(nameof(Chart.LockedBy)), nameof(Chart.LockedBy))
                .Add(Projections.Property(nameof(Chart.Name)), nameof(Chart.Name))
                .Add(Projections.Property(nameof(Chart.Note)), nameof(Chart.Note))
                .Add(Projections.Property(nameof(Chart.Organization)), nameof(Chart.Organization))
                .Add(Projections.Property(nameof(Chart.PublicationDate)), nameof(Chart.PublicationDate))
                .Add(Projections.Property(nameof(Chart.RunwayDirection)), nameof(Chart.RunwayDirection))
                .Add(Projections.Property(nameof(Chart.Type)), nameof(Chart.Type))
                .Add(Projections.Property(nameof(Chart.Version)), nameof(Chart.Version))
                //.Add(Projections.Property(nameof(Chart.UpdatedBasedOn)))
                .Add(Projections.Property(nameof(Chart.Airport)), nameof(Chart.Airport));
        }
        public ChartRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override IQueryable<Chart> GetAll()
        {
            UnitOfWork.BeginTransaction();
            var res = _session.CreateCriteria<ChartWithReference>()
                .SetProjection(_chartProjections)
                    .SetResultTransformer(Transformers.AliasToBean<Chart>()).List<Chart>();
            UnitOfWork.Commit();
            return res.AsQueryable<Chart>();
        }

        public override Chart GetById(long id)
        {
            UnitOfWork.BeginTransaction();
            var res = _session.CreateCriteria<ChartWithReference>()
                .Add(Restrictions.Eq(nameof(Chart.Id), id))
                .SetProjection(_chartProjections)
                .SetResultTransformer(Transformers.AliasToBean<Chart>()).List<Chart>().FirstOrDefault();
            UnitOfWork.Commit();
            return res;
        }

        //public override void Delete(long id)
        //{
        //    UnitOfWork.BeginTransaction();
        //    _session.Delete(_session.Load<ChartWithReference>(id));
        //    //var queryString = string.Format("delete {0} where id = :id", typeof(ChartWithReference));
        //    //_session.CreateQuery(queryString)
        //    //       .SetParameter("id", id)
        //    //       .ExecuteUpdate();
        //    UnitOfWork.Commit();

        //}

        //public override Chart Update(Chart entity)
        //{
        //    ChartWithReference chartWithReference = entity as ChartWithReference; ;

        //    UnitOfWork.BeginTransaction();
        //    _session.Update(nameof(ChartWithReference), entity, entity.Id);
        //    UnitOfWork.Commit();
        //    return entity;
        //}

        public override Chart Create(Chart entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Chart entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(Chart entity)
        {
            throw new NotImplementedException();
        }
    }
}
