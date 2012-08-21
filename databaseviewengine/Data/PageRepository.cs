using System;
using System.Collections.Generic;
using System.Linq;
using FluentMongo.Linq;
using MongoDB.Driver;
using databaseviewengine.Entities;

namespace databaseviewengine.Data
{
    public class PageRepository
    {
        protected MongoDatabase Database 
        { 
            get { return MongoServer.Create("mongodb://localhost/").GetDatabase("databaseviewengine"); }
        }

        protected MongoCollection<Page> Collection
        {
            get { return Database.GetCollection<Page>("pages"); }
        }

        public IEnumerable<Page> GetAll()
        {
            return Collection.FindAll();
        }

        public Page GetById(Guid id)
        {
            return Collection.AsQueryable().SingleOrDefault(x => x.Id == id);
        }

        public Page GetByViewName(string viewName)
        {
            return Collection.AsQueryable().SingleOrDefault(x => x.ViewName == viewName);
        }

        public Page GetByDomainAndViewName(string domain, string viewName)
        {
            return Collection.AsQueryable().SingleOrDefault(x => x.Domain == domain && x.ViewName == viewName);
        }

        public void Save(Page page)
        {
            Collection.Insert(page);
        }

        public void Update(Page page)
        {
            Collection.Save(page);
        }

        public long Count()
        {
            return Collection.Count();
        }
    }
}