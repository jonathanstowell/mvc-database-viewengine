using System;
using System.Collections.Generic;
using System.Linq;
using MvcThemable.Data.Abstract;
using Raven.Client;

namespace MvcThemable.Core.DatabaseViewRepositories
{
    public class RavenDatabaseViewRepository : IDatabaseViewRepository
    {
        private readonly IDocumentStore store;

        public RavenDatabaseViewRepository(IDocumentStore store)
        {
            this.store = store;
        }

        public IEnumerable<Entities.Concrete.DatabaseView> GetAll()
        {
            using (var session = store.OpenSession())
            {
                return session.Query<Entities.Concrete.DatabaseView>().ToList();
            }
        }

        public Entities.Concrete.DatabaseView GetById(Guid id)
        {
            using (var session = store.OpenSession())
            {
                return session.Load<Entities.Concrete.DatabaseView>(id);
            }
        }

        public Entities.Concrete.DatabaseView GetByViewKey(string key)
        {
            using (var session = store.OpenSession())
            {
                return session.Query<Entities.Concrete.DatabaseView>().SingleOrDefault(x => x.ViewKey == key);
            }
        }

        public void Save(Entities.Concrete.DatabaseView databaseView)
        {
            using (var session = store.OpenSession())
            {
                session.Store(databaseView);
                session.SaveChanges();
            }
        }

        public void Update(Entities.Concrete.DatabaseView databaseView)
        {
            using (var session = store.OpenSession())
            {
                var view = session.Load<Entities.Concrete.DatabaseView>(databaseView.Id);

                view.Action = databaseView.Action;
                view.Body = databaseView.Body;
                view.Controller = databaseView.Controller;
                view.Domain = databaseView.Domain;
                view.LastModifiedDateTime = DateTime.Now;
                view.Title = databaseView.Title;
                view.ViewKey = databaseView.ViewKey;
                view.ViewModel = databaseView.ViewModel;
       
                session.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            using (var session = store.OpenSession())
            {
                session.Delete(session.Load<Entities.Concrete.DatabaseView>(id));
            }
        }

        public long Count()
        {
            using (var session = store.OpenSession())
            {
                return session.Query<Entities.Concrete.DatabaseView>().Count();
            }
        }
    }
}