using System;
using MvcThemable.Cache.Abstract;
using MvcThemable.Entities.Concrete;

namespace MvcThemable.Cache.Concrete
{
    public class NullDatabaseViewCacheManager : IDatabaseViewCacheManager
    {
        public DatabaseView Get(string key, Func<DatabaseView> retrieve = null)
        {
            return retrieve == null ? null : retrieve();
        }

        public void Remove(string key)
        {
            
        }
    }
}
