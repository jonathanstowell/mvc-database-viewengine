using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MvcThemable.Data.Abstract;

namespace MvcThemable.Core.DatabaseView
{
    public class MongoDatabaseViewRepository : IDatabaseViewRepository
    {
        protected MongoDatabase Database 
        { 
            get { return MongoServer.Create("mongodb://localhost/").GetDatabase("databaseviewengine"); }
        }

        protected MongoCollection<Entities.Concrete.DatabaseView> Collection
        {
            get { return Database.GetCollection<Entities.Concrete.DatabaseView>("pages"); }
        }

        public IEnumerable<Entities.Concrete.DatabaseView> GetAll()
        {
            return Collection.FindAll();
        }

        public Entities.Concrete.DatabaseView GetById(Guid id)
        {
            return Collection.AsQueryable().SingleOrDefault(x => x.Id == id);
        }

        public Entities.Concrete.DatabaseView GetByViewKey(string key)
        {
            return Collection.AsQueryable().SingleOrDefault(x => x.ViewKey == key);
        }

        public void Save(Entities.Concrete.DatabaseView databaseView)
        {
            Collection.Insert(databaseView);
        }

        public void Update(Entities.Concrete.DatabaseView databaseView)
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