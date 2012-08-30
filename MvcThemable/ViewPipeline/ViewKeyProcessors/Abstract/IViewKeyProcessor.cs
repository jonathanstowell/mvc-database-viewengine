namespace MvcThemable.ViewPipeline.ViewKeyProcessors.Abstract
{
    public interface IViewKeyProcessor
    {
        string Generate(string domain, string controller, string action);
        string Retrieve(string virtualPath);
    }
}
