using System;
using System.Web.Mvc;

using MvcThemable.Entities.Concrete;
using MvcThemable.Extensions;
using MvcThemable.Request.Abstract;
using MvcThemable.Service.Abstract;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Abstract;
using MvcThemable.Views.Models.Abstract;
using MvcThemable.WebUI.Models;

namespace MvcThemable.WebUI.Controllers
{
    public class PageController : Controller
    {
        private readonly IDatabaseViewService databaseViewService;
        private readonly IFileSystemViewService fileViewService;
        private readonly IProvideViewModels provideViewModels;
        private readonly IViewKeyProcessor viewKeyProcessor;
        private readonly IProvideCurrentRequestContext currentRequestContext;

        public PageController(IDatabaseViewService databaseViewService, IProvideViewModels provideViewModels, IViewKeyProcessor viewKeyProcessor, IProvideCurrentRequestContext currentRequestContext, IFileSystemViewService fileViewService)
        {
            this.databaseViewService = databaseViewService;
            this.provideViewModels = provideViewModels;
            this.viewKeyProcessor = viewKeyProcessor;
            this.currentRequestContext = currentRequestContext;
            this.fileViewService = fileViewService;
        }

        public ActionResult Init()
        {
            if (this.databaseViewService.Count() != 0)
                return View("Index", this.databaseViewService.GetAll());

            this.databaseViewService.Save(new DatabaseView { Id = Guid.NewGuid(), ViewKey = "localhost.Page.Test1", ViewModel = new ViewModel { FullName = "MvcThemable.WebUI.Models.TestViewModel", AssemblyQualifiedName = typeof(TestViewModel).AssemblyQualifiedName }, Layout = "~/Views/Shared/_Layout.cshtml", Domain = "localhost", Controller = "Page", Action = "Test1", Title = "Test 1", Body = "<h2>Test 1</h2><p>@Model.Forename</p><p>@Model.Surname</p><p>localhost</p>", LastModifiedDateTime = DateTime.Now.AddDays(-1) });
            this.databaseViewService.Save(new DatabaseView { Id = Guid.NewGuid(), ViewKey = "default.Page.Test1", ViewModel = new ViewModel { FullName = "MvcThemable.WebUI.Models.TestViewModel", AssemblyQualifiedName = typeof(TestViewModel).AssemblyQualifiedName }, Layout = "~/Views/Shared/_Layout.cshtml", Domain = "default", Controller = "Page", Action = "Test1", Title = "Test 1", Body = "<h2>Test 1</h2><p>@Model.Forename</p><p>@Model.Surname</p><p>default</p>", LastModifiedDateTime = DateTime.Now.AddDays(-1) });
            this.databaseViewService.Save(new DatabaseView { Id = Guid.NewGuid(), ViewKey = "default.Page.Test2", ViewModel = new ViewModel { FullName = "MvcThemable.WebUI.Models.Test2ViewModel", AssemblyQualifiedName = typeof(TestViewModel).AssemblyQualifiedName }, Layout = "~/Views/Shared/_Layout.cshtml", Domain = "default", Controller = "Page", Action = "Test2", Title = "Test 2", Body = "<h2>Test 2</h2><p>Today: @Model.Today</p><p>default</p>", LastModifiedDateTime = DateTime.Now.AddDays(-1) });
            this.databaseViewService.Save(new DatabaseView { Id = Guid.NewGuid(), ViewKey = "localhost.Page.Test3", ViewModel = new ViewModel { FullName = "MvcThemable.WebUI.Models.Test3ViewModel", AssemblyQualifiedName = typeof(TestViewModel).AssemblyQualifiedName }, Layout = "~/Views/Shared/_Layout.cshtml", Domain = "localhost", Controller = "Page", Action = "Test3", Title = "Test 3", Body = "<h2>Test 3</h2><p>Number: @Model.Number</p><p>localhost</p>", LastModifiedDateTime = DateTime.Now.AddDays(-1) });
            this.databaseViewService.Save(new DatabaseView { Id = Guid.NewGuid(), ViewKey = "default.Page.Precompiled", ViewModel = new ViewModel { FullName = "MvcThemable.WebUI.Models.Test3ViewModel", AssemblyQualifiedName = typeof(TestViewModel).AssemblyQualifiedName }, Layout = "~/Views/Shared/_Layout.cshtml", Domain = "default", Controller = "Page", Action = "Precompiled", Title = "Precompiled", Body = "<h2>Database Precompiled</h2><p>Number: @Model.Number</p><p>localhost</p>", LastModifiedDateTime = DateTime.Now.AddDays(-1) });

            return View("Index", this.databaseViewService.GetAll());
        }

        public ActionResult Index()
        {
            return View(this.databaseViewService.GetAll());
        }

        public ActionResult Create()
        {
            var model = new DatabaseViewCreationViewModel();

            model.DatabaseView = new DatabaseView();
            model.ViewModels = provideViewModels.GetModels("MvcThemable.WebUI.Models").ToSelectList();

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(DatabaseViewCreationViewModel model)
        {
            model.DatabaseView.Id = Guid.NewGuid();
            model.DatabaseView.LastModifiedDateTime = DateTime.Now;

            this.databaseViewService.Save(model.DatabaseView);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            return View(this.databaseViewService.GetById(Guid.Parse(id)));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(DatabaseView databaseView)
        {
            databaseView.LastModifiedDateTime = DateTime.Now;

            this.databaseViewService.Update(databaseView);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(string viewKey, string fullName, string assemblyQualifiedName, string layout, string body)
        {
            var view = this.databaseViewService.GetByViewKey(viewKey);

            view.ViewModel = string.IsNullOrWhiteSpace(assemblyQualifiedName) || string.IsNullOrWhiteSpace(fullName) ? null : new ViewModel { AssemblyQualifiedName = assemblyQualifiedName, FullName = fullName };
            view.Layout = layout;
            view.Body = body;

            this.databaseViewService.Update(view);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            this.databaseViewService.Delete(Guid.Parse(id));

            return RedirectToAction("Index");
        }

        public ActionResult Standard()
        {
            return View();
        }

        public ActionResult Precompiled()
        {
            return View(new Test3ViewModel { Number = 7 });
        }

        public ActionResult Test1()
        {
            return View(new TestViewModel { Forename = "Jonathan", Surname = "Stowell" });
        }

        public ActionResult Test2()
        {
            return View(new Test2ViewModel());
        }

        public ActionResult Test3()
        {
            return View(new Test3ViewModel { Number = 7 });
        }

        public ActionResult Test4()
        {
            return PartialView();
        }

        public JsonResult Get(string controller, string controllerAction)
        {
            var ret = this.databaseViewService.GetByViewKey(viewKeyProcessor.Generate(currentRequestContext.CurrentHost, controller, controllerAction));

            if (ret == null)
                ret = this.databaseViewService.GetByViewKey(viewKeyProcessor.Generate("default", controller, controllerAction));

            if (ret == null)
            {
                var fileView = fileViewService.GetContent(currentRequestContext.CurrentHost, controller, controllerAction);

                if (fileView != null)
                {
                    ret = new DatabaseView
                    {
                        ViewKey = fileView.ViewKey,
                        Layout = fileView.Layout,
                        Body = fileView.Body,
                        ViewModel = fileView.ViewModel
                    };
                }
            }

            return Json(new { View = ret, ViewModels = provideViewModels.GetModels("MvcThemable.WebUI.Models").ToSelectList() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetViewModelProperties(string fullClassName)
        {
            return Json(new { Properties = provideViewModels.GetModelProperties(fullClassName).ToSelectList() }, JsonRequestBehavior.AllowGet);
        }
    }
}
