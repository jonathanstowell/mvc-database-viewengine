using System;
using System.Web.Mvc;
using Bootstrap.Extensions.StartupTasks;

namespace MvcThemable.WebUI.Installers
{
    public class RegisterFilters : IStartupTask
    {
        public void Run()
        {
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}