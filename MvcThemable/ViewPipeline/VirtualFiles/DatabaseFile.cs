using System;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace MvcThemable.ViewPipeline.VirtualFiles
{
    public class DatabaseFile : VirtualFile
    {
        private readonly string viewModel;
        private readonly string layout;
        private readonly string content;

        public DatabaseFile(string virtualPath, string content, string viewModel = null, string layout = null)
            : base(virtualPath)
        {
            this.content = content;
            this.viewModel = viewModel;
            this.layout = layout;
        }

        public override Stream Open()
        {
            return new MemoryStream(
                new ASCIIEncoding().GetBytes(
                    string.Format("{0}{1}{2}",
                        this.viewModel != null ? "@model " + viewModel + Environment.NewLine : string.Empty,
                        layout != null ? "@{ Layout = \"" + layout + "\"; }" + Environment.NewLine : string.Empty,
                        content + "<button class=\"btn btn-primary\" onclick=\"jQuery(document).trigger('editDatabaseComponent', [ '@if (Model != null) { @Model.GetType().AssemblyQualifiedName; }', '@ViewContext.RouteData.GetRequiredString(\"controller\")', '@ViewContext.RouteData.GetRequiredString(\"action\")' ])\">Edit</button>")
                ), false);
        }
    }
}