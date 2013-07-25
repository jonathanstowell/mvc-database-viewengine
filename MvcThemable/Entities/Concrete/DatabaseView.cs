using System;
using MvcThemable.Entities.Abstract;

namespace MvcThemable.Entities.Concrete
{
    public class DatabaseView : IDatabaseView
    {
        public virtual Guid Id { get; set; }
        public virtual string ViewKey { get; set; }
        public virtual string ViewModel { get; set; }
        public virtual string Layout { get; set; }
        public virtual string Domain { get; set; }
        public virtual string Controller { get; set; }
        public virtual string Action { get; set; }
        public virtual string Title { get; set; }
        public virtual string Body { get; set; }
        public virtual DateTime LastModifiedDateTime { get; set; }
    }
}