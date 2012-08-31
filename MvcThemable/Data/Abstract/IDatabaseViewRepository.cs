using MvcThemable.Entities.Concrete;

namespace MvcThemable.Data.Abstract
{
    public interface IDatabaseViewRepository
    {
        DatabaseView GetByViewKey(string key);
    }
}
