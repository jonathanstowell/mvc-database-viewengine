using System;
using MvcThemable.Entities.Concrete;

namespace MvcThemable.Cache.Abstract
{
    public interface IDatabaseViewCacheManager
    {
        DatabaseView Get(string key, Func<DatabaseView> retrieve = null);
        void Remove(string key);
    }
}
