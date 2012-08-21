using System;
using System.Web.Mvc;
using databaseviewengine.Data;
using databaseviewengine.Entities;
using databaseviewengine.Models;

namespace databaseviewengine.Controllers
{
    public class PageController : Controller
    {
        private readonly PageRepository repository;

        public PageController()
        {
            repository = new PageRepository();
        }

        public ActionResult Init()
        {
            if (repository.Count() != 0)
                return View("Index", repository.GetAll());

            repository.Save(new Page { Id = Guid.NewGuid(), Domain = "localhost", Title = "Test 1", Body = "@model databaseviewengine.Models.TestViewModel \r\n @{ ViewBag.Title = \"Test 1\";}<h2>Test 1</h2><p>@Model.Forename</p><p>@Model.Surname</p><p>localhost</p>", ViewName = "Page/Test1", LastModifiedDateTime = DateTime.Now.AddDays(-1) });
            repository.Save(new Page { Id = Guid.NewGuid(), Domain = "default", Title = "Test 1", Body = "@model databaseviewengine.Models.TestViewModel \r\n @{ ViewBag.Title = \"Test 1\";}<h2>Test 1</h2><p>@Model.Forename</p><p>@Model.Surname</p><p>default</p>", ViewName = "Page/Test1", LastModifiedDateTime = DateTime.Now.AddDays(-1) });
            repository.Save(new Page { Id = Guid.NewGuid(), Domain = "default", Title = "Test 2", Body = "@model databaseviewengine.Models.Test2ViewModel \r\n @{ ViewBag.Title = \"Test 2\";}<h2>Test 2</h2><p>Today: @Model.Today</p><p>default</p>", ViewName = "Page/Test2", LastModifiedDateTime = DateTime.Now.AddDays(-1) });
            repository.Save(new Page { Id = Guid.NewGuid(), Domain = "localhost", Title = "Test 3", Body = "@model databaseviewengine.Models.Test3ViewModel \r\n @{ ViewBag.Title = \"Test 3\";}<h2>Test 3</h2><p>Number: @Model.Number</p><p>localhost</p>", ViewName = "Page/Test3", LastModifiedDateTime = DateTime.Now.AddDays(-1) });
            //repository.Save(new Page { Id = Guid.NewGuid(), Domain = "default", Title = "Precompiled", Body = "@model databaseviewengine.Models.Test3ViewModel \r\n @{ ViewBag.Title = \"Test 3\";}<h2>Test 3</h2><p>Number: @Model.Number</p><p>localhost</p>", ViewName = "Page/Precompiled", LastModifiedDateTime = DateTime.Now.AddDays(-1) });

            return View("Index", repository.GetAll());
        }

        public ActionResult Index()
        {
            return View(repository.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Page page)
        {
            page.Id = Guid.NewGuid();
            page.LastModifiedDateTime = DateTime.Now;

            repository.Save(page);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            return View(repository.GetById(Guid.Parse(id)));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Page page)
        {
            page.LastModifiedDateTime = DateTime.Now;

            repository.Update(page);

            return RedirectToAction("Index");
        }

        public ActionResult Standard()
        {
            return View();
        }

        public ActionResult Precompiled()
        {
            return View();
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
    }
}
