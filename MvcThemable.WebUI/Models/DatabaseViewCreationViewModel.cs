using System.Collections.Generic;
using System.Web.Mvc;
using MvcThemable.Entities.Concrete;

namespace MvcThemable.WebUI.Models
{
    public class DatabaseViewCreationViewModel
    {
        public IEnumerable<SelectListItem> ViewModels { get; set; }

        public DatabaseView DatabaseView { get; set; }
    }
}