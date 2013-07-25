using System;
using System.Collections.Generic;

namespace MvcThemable.Views.Models.Abstract
{
    public interface IProvideViewModels
    {
        IDictionary<string, string> GetModels(params string[] namespaces);
        IDictionary<string, string> GetModelProperties(string fullName);
    }
}
