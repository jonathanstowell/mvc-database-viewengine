namespace MvcThemable.ViewPipeline.ViewLocations.Abstract
{
    public interface IProvideViewLocations
    {
        string[] ViewLocations(params string[] extensions);
        string[] ParitalViewLocations(params string[] extensions);
    }
}
