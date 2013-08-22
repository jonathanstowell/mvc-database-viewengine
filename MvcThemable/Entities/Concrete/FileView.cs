namespace MvcThemable.Entities.Concrete
{
    using System;

    using MvcThemable.Entities.Abstract;

    public class FileView
    {
        public virtual Guid Id { get; set; }
        public virtual string Body { get; set; }
        public virtual string Layout { get; set; }
        public virtual string Controller { get; set; }
        public virtual string Action { get; set; }
        public virtual IViewModel ViewModel { get; set; }
        public virtual string ViewKey { get; set; }
    }
}