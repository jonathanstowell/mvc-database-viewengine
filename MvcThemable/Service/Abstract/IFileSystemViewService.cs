namespace MvcThemable.Service.Abstract
{
    using MvcThemable.Entities.Concrete;

    public interface IFileSystemViewService
    {
        FileView GetContent(string domain, string controller, string action);
    }
}
