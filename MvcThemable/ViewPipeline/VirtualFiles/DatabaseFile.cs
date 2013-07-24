using System;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace MvcThemable.ViewPipeline.VirtualFiles
{
    public class DatabaseFile : VirtualFile
    {
        private readonly string content;

        public DatabaseFile(string virtualPath, string content)
            : base(virtualPath)
        {
            this.content = content;
        }

        public override Stream Open()
        {
            return new MemoryStream(new ASCIIEncoding().GetBytes(content + "<button class=\"btn btn-primary\" onclick=\"jQuery(document).trigger('editDatabaseComponent', [ '@Model.GetType().AssemblyQualifiedName', '@ViewContext.RouteData.GetRequiredString(\"controller\")', '@ViewContext.RouteData.GetRequiredString(\"action\")' ])\">Edit</button>"), false);
        }
    }

    public class DatabaseFileCacheDependency : System.Web.Caching.CacheDependency
    {
        public DatabaseFileCacheDependency(DateTime lastModified)
        {
            base.SetUtcLastModified(lastModified);
        }
        public DatabaseFileCacheDependency()
        {
            base.SetUtcLastModified(DateTime.UtcNow);
        }
    }
}