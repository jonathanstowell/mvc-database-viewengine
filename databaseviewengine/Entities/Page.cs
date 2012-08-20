using System;

namespace databaseviewengine.Entities
{
    public class Page
    {
        public virtual Guid Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Body { get; set; }
        public virtual string ViewName { get; set; }
    }
}