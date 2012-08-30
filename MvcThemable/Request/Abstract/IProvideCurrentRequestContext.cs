namespace MvcThemable.Request.Abstract
{
    public interface IProvideCurrentRequestContext
    {
        string CurrentHost { get; }
    }
}