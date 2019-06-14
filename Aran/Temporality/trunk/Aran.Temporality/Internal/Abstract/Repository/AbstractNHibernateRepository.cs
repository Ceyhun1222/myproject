using System;
using NHibernate;

namespace Aran.Temporality.Internal.Abstract.Repository
{
    abstract class AbstractNHibernateRepository<T>: AbstractDataRepository<T, int> where T : class
    {
        protected abstract ISessionFactory CreateSessionFactory(bool rewrite);

        public ISessionFactory SessionFactory;


        #region Overrides of AbstractDataRepository<T>

        public override void Open(bool rewrite = false)
        {
            SessionFactory = CreateSessionFactory(rewrite);
        }

        public override void Close()
        {
            SessionFactory.Close();
            SessionFactory.Dispose();
        }

        public override void RemoveAll()
        {
            Close();
            Open(true);//rewrite all
        }


        public override void Remove(T item)
        {
            throw new NotImplementedException();
        }


        public override int Add(T item)
        {
            if (item == null) throw new Exception("adding nulls is not supported");

            using (var session = SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(item);
                    transaction.Commit();
                }
            }

            return 0;
        }

        public override void Dispose()
        {
            Close();
        }

        #endregion
    }
}
