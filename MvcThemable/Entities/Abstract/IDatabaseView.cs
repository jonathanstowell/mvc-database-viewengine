using System;

namespace MvcThemable.Entities.Abstract
{
    public interface IDatabaseView
    {
        string ViewKey { get; set; }
        IViewModel ViewModel { get; set; }
        string Layout { get; set; }
        string Body { get; set; }
        DateTime LastModifiedDateTime { get; set; }
    }

    public interface IViewModel
    {
        string FullName { get; set; }
        string AssemblyQualifiedName { get; set; }
    }
}
