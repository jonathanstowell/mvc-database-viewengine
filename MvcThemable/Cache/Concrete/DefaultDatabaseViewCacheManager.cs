using System;
using System.Collections.Generic;
using MvcThemable.Cache.Abstract;
using MvcThemable.Entities.Concrete;

namespace MvcThemable.Cache.Concrete
{
    public class DefaultDatabaseViewCacheManager : IDatabaseViewCacheManager
    {
        private static readonly IDictionary<string, DatabaseView> Cache = new Dictionary<string, DatabaseView>();

        public DatabaseView Get(string key, Func<DatabaseView> retrieve = null)
        {
            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }

            if (retrieve != null)
            {
                var view = retrieve();
                
                if (view != null)
                {
                    Cache.Add(key, view);
                    return view;
                }
            }

            return null;
        }

        public void Remove(string key)
        {
            if (Cache.ContainsKey(key))
            {
                Cache.Remove(key);
            }
        }
    }
}
