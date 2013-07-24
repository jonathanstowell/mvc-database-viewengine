using System;
using System.Collections.Generic;
using MvcThemable.Entities.Concrete;

namespace MvcThemable.Data.Abstract
{
    public interface IDatabaseViewRepository
    {
        IEnumerable<DatabaseView> GetAll();
        DatabaseView GetById(Guid id);
        DatabaseView GetByViewKey(string key);
        void Save(DatabaseView databaseView);
        void Update(DatabaseView databaseView);
        void Delete(Guid id);
        long Count();
    }
}
