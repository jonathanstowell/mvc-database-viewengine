using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MvcThemable.Data.Abstract;
using MvcThemable.Entities.Concrete;

namespace MvcThemable.Data.Concrete
{
    public class DatabaseViewRepository : IDatabaseViewRepository
    {
        protected MongoDatabase Database 
        { 
            get { return MongoServer.Create("mongodb://localhost/").GetDatabase("databaseviewengine"); }
        }

        protected MongoCollection<DatabaseView> Collection
        {
            get { return Database.GetCollection<DatabaseView>("pages"); }
        }

        public IEnumerable<DatabaseView> GetAll()
        {
            return Collection.FindAll();
        }

        public DatabaseView GetById(Guid id)
        {
            return Collection.AsQueryable().SingleOrDefault(x => x.Id == id);
        }

        public DatabaseView GetByViewKey(string key)
        {
            return Collection.AsQueryable().SingleOrDefault(x => x.ViewKey == key);
        }

        public void Save(DatabaseView databaseView)
        {
            Collection.Insert(databaseView);
        }

        public void Update(DatabaseView databaseView)
        {
            Collection.Save(databaseView);
        }

        public void Delete(Guid id)
        {
            Collection.Remove(Query.EQ("_id", id));
        }

        public long Count()
        {
            return Collection.Count();
        }
    }
}