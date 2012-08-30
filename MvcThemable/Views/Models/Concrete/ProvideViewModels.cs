using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvcThemable.Extensions;
using MvcThemable.Views.Models.Abstract;

namespace MvcThemable.Views.Models.Concrete
{
    public class ProvideViewModels : IProvideViewModels
    {
        public IDictionary<string, string> GetModels(params string[] namespaces)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var models = new Dictionary<string, string>();

            foreach (var assembly in assemblies)
            {
                foreach (var ns in namespaces)
                {
                    models.AddRange(assembly.GetTypes().Where(a => string.Equals(a.Namespace, ns, StringComparison.Ordinal)).Select(a => new { a.FullName, a.Name }).ToDictionary(a => a.FullName, a => a.Name));
                }
            }

            return models;
        }

        public IDictionary<string, Type> GetModelProperties(string fullName)
        {
            var type = Type.GetType(fullName);

            if (type != null)
                return type.GetProperties().Select(p => new { p.Name, p.PropertyType }).ToDictionary(p => p.Name, p => p.PropertyType);

            return null;
        }
    }
}
