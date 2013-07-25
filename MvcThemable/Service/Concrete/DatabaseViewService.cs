using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcThemable.Cache.Abstract;
using MvcThemable.Data.Abstract;
using MvcThemable.Entities.Concrete;
using MvcThemable.Service.Abstract;

namespace MvcThemable.Service.Concrete
{
    public class DatabaseViewService : IDatabaseViewService
    {
        private readonly IDatabaseViewRepository repository;
        private readonly IDatabaseViewCacheManager cache;

        public DatabaseViewService()
        {
            repository = DependencyResolver.Current.GetService<IDatabaseViewRepository>();
            cache = DependencyResolver.Current.GetService<IDatabaseViewCacheManager>();
        }

        public IEnumerable<DatabaseView> GetAll()
        {
            return repository.GetAll();
        }

        public DatabaseView GetById(Guid id)
        {
            return cache.Get(id.ToString(), () => repository.GetById(id));
        }

        public DatabaseView GetByViewKey(string key)
        {
            return cache.Get(key, () => repository.GetByViewKey(key));
        }

        public void Save(DatabaseView databaseView)
        {
            repository.Save(databaseView);

            cache.Remove(databaseView.ViewKey);
            cache.Remove(databaseView.Id.ToString());
        }

        public void Update(DatabaseView databaseView)
        {
            repository.Update(databaseView);

            cache.Remove(databaseView.ViewKey);
            cache.Remove(databaseView.Id.ToString());
        }

        public void Delete(Guid id)
        {
            var view = GetById(id);

            repository.Delete(id);

            cache.Remove(view.ViewKey);
            cache.Remove(view.Id.ToString());
        }

        public long Count()
        {
            return repository.Count();
        }
    }
}
