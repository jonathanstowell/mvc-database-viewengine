using System;
using System.Collections.Generic;
using MvcThemable.Entities.Concrete;

namespace MvcThemable.Service.Abstract
{
    public interface IDatabaseViewService
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
