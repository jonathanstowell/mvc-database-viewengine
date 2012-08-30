using System;

namespace MvcThemable.Entities.Abstract
{
    public interface IDatabaseView
    {
        string ViewKey { get; set; }
        string Body { get; set; }
        DateTime LastModifiedDateTime { get; set; }
    }
}
