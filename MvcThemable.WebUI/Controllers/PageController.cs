﻿using System;
using System.Web.Mvc;
using MvcThemable.Data.Abstract;
using MvcThemable.Entities.Concrete;
using MvcThemable.Extensions;
using MvcThemable.Request.Abstract;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Abstract;
using MvcThemable.Views.Models.Abstract;
using MvcThemable.WebUI.Models;

namespace MvcThemable.WebUI.Controllers
{
    public class PageController : Controller
    {
        private readonly IDatabaseViewRepository repository;
        private readonly IProvideViewModels provideViewModels;
        private readonly IViewKeyProcessor viewKeyProcessor;
        private readonly IProvideCurrentRequestContext currentRequestContext;

        public PageController(IDatabaseViewRepository repository, IProvideViewModels provideViewModels, IViewKeyProcessor viewKeyProcessor, IProvideCurrentRequestContext currentRequestContext)
        {
            this.repository = repository;
            this.provideViewModels = provideViewModels;
            this.viewKeyProcessor = viewKeyProcessor;
            this.currentRequestContext = currentRequestContext;
        }

        public ActionResult Init()
        {
            if (repository.Count() != 0)
                return View("Index", repository.GetAll());

            repository.Save(new DatabaseView { Id = Guid.NewGuid(), ViewKey = "localhost.Page.Test1", ViewModel = "MvcThemable.WebUI.Models.TestViewModel", Domain = "localhost", Controller = "Page", Action = "Test1", Title = "Test 1", Body = "@model MvcThemable.WebUI.Models.TestViewModel \r\n @{  Layout = \"~/Views/Shared/_Layout.cshtml\"; ViewBag.Title = \"Test 1\";}<h2>Test 1</h2><p>@Model.Forename</p><p>@Model.Surname</p><p>localhost</p>", LastModifiedDateTime = DateTime.Now.AddDays(-1) });
            repository.Save(new DatabaseView { Id = Guid.NewGuid(), ViewKey = "default.Page.Test1", ViewModel = "MvcThemable.WebUI.Models.TestViewModel", Domain = "default", Controller = "Page", Action = "Test1", Title = "Test 1", Body = "@model MvcThemable.WebUI.Models.TestViewModel \r\n @{  Layout = \"~/Views/Shared/_Layout.cshtml\"; ViewBag.Title = \"Test 1\";}<h2>Test 1</h2><p>@Model.Forename</p><p>@Model.Surname</p><p>default</p>", LastModifiedDateTime = DateTime.Now.AddDays(-1) });
            repository.Save(new DatabaseView { Id = Guid.NewGuid(), ViewKey = "default.Page.Test2", ViewModel = "MvcThemable.WebUI.Models.Test2ViewModel", Domain = "default", Controller = "Page", Action = "Test2", Title = "Test 2", Body = "@model MvcThemable.WebUI.Models.Test2ViewModel \r\n @{  Layout = \"~/Views/Shared/_Layout.cshtml\"; ViewBag.Title = \"Test 2\";}<h2>Test 2</h2><p>Today: @Model.Today</p><p>default</p>", LastModifiedDateTime = DateTime.Now.AddDays(-1) });
            repository.Save(new DatabaseView { Id = Guid.NewGuid(), ViewKey = "localhost.Page.Test3", ViewModel = "MvcThemable.WebUI.Models.Test3ViewModel", Domain = "localhost", Controller = "Page", Action = "Test3", Title = "Test 3", Body = "@model MvcThemable.WebUI.Models.Test3ViewModel \r\n @{  Layout = \"~/Views/Shared/_Layout.cshtml\"; ViewBag.Title = \"Test 3\";}<h2>Test 3</h2><p>Number: @Model.Number</p><p>localhost</p>", LastModifiedDateTime = DateTime.Now.AddDays(-1) });
            repository.Save(new DatabaseView { Id = Guid.NewGuid(), ViewKey = "default.Page.Precompiled", ViewModel = "MvcThemable.WebUI.Models.Test3ViewModel", Domain = "default", Controller = "Page", Action = "Precompiled", Title = "Precompiled", Body = "@model MvcThemable.WebUI.Models.Test3ViewModel \r\n @{ Layout = \"~/Views/Shared/_Layout.cshtml\"; ViewBag.Title = \"Database Precompiled\";}<h2>Database Precompiled</h2><p>Number: @Model.Number</p><p>localhost</p>", LastModifiedDateTime = DateTime.Now.AddDays(-1) });

            return View("Index", repository.GetAll());
        }

        public ActionResult Index()
        {
            return View(repository.GetAll());
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

            repository.Save(model.DatabaseView);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            return View(repository.GetById(Guid.Parse(id)));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(DatabaseView databaseView)
        {
            databaseView.LastModifiedDateTime = DateTime.Now;

            repository.Update(databaseView);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(string viewKey, string body)
        {
            var view = repository.GetByViewKey(viewKey);

            view.Body = body;

            repository.Update(view);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            repository.Delete(Guid.Parse(id));

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

        public JsonResult Get(string controller, string controllerAction)
        {
            var view = repository.GetByViewKey(viewKeyProcessor.Generate(currentRequestContext.CurrentHost, controller, controllerAction));

            if (view == null)
                view = repository.GetByViewKey(viewKeyProcessor.Generate("default", controller, controllerAction));

            return Json(view, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetViewModelProperties(string fullClassName)
        {
            return Json(provideViewModels.GetModelProperties(fullClassName), JsonRequestBehavior.AllowGet);
        }
    }
}
