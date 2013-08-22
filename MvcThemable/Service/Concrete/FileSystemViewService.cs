namespace MvcThemable.Service.Concrete
{
    using System;
    using System.IO;

    using MvcThemable.Entities.Concrete;
    using MvcThemable.Service.Abstract;
    using MvcThemable.ViewPipeline.ViewKeyProcessors.Abstract;

    public class FileSystemViewService : IFileSystemViewService
    {
        private readonly IViewKeyProcessor viewKeyProcessor;

        public FileSystemViewService(IViewKeyProcessor viewKeyProcessor)
        {
            this.viewKeyProcessor = viewKeyProcessor;
        }

        public FileView GetContent(string domain, string controller, string action)
        {
            var ret = new FileView
            {
                Id = Guid.NewGuid(),
                ViewKey = viewKeyProcessor.Generate(domain, controller, action),
                Body = File.ReadAllText(@"C:\repos\databaseviewengine\MvcThemable.WebUI\Views\localhost\Page\standard.cshtml"),
                Action = action,
                Controller = controller,
                Layout = string.Empty
            };

            return ret;
        }
    }
}