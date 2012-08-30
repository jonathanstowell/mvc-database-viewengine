using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcThemable.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                {
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    // handle duplicate key issue here
                }
            }
        }

        public static IEnumerable<SelectListItem> ToSelectList<K, V>(this IDictionary<K, V> source)
        {
            return source.Select(x => new SelectListItem { Value = x.Key.ToString(), Text = x.Value.ToString() });
        }
    }
}
