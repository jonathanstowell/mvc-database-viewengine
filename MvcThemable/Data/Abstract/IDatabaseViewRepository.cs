using MvcThemable.Entities.Abstract;

namespace MvcThemable.Data.Abstract
{
    public interface IDatabaseViewRepository
    {
        IDatabaseView GetByViewKey(string key);
    }
}
