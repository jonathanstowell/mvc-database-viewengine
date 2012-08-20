using System.IO;
using System.Text;
using System.Web.Hosting;

namespace databaseviewengine.VirtualFiles
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
            return new MemoryStream(new ASCIIEncoding().GetBytes(content), false);
        }
    }
}