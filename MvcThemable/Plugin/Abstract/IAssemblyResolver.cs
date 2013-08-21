using System.Collections.Generic;
using System.Reflection;

namespace MvcThemable.Plugin.Abstract
{
    public interface IAssemblyResolver
    {
        IDictionary<string, Assembly> PluginAssembliesByFullName { get; }
        Assembly GetAssembly(string name);
    }
}
