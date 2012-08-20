using System.Web.Mvc;

namespace databaseviewengine.ViewEngines
{
    public class MongoViewEngine : RazorViewEngine
    {
        public MongoViewEngine() : this(null)
        {
        }

        public MongoViewEngine(IViewPageActivator viewPageActivator) : base(viewPageActivator)
        {
            AreaViewLocationFormats = new[]
                                    {
                                        "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                        "~/Areas/{2}/Views/{1}/{0}",
                                        "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                        "~/Areas/{2}/Views/Shared/{0}"
                                    };

            AreaMasterLocationFormats = new[]
                                        {
                                            "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                            "~/Areas/{2}/Views/{1}/{0}",
                                            "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                            "~/Areas/{2}/Views/Shared/{0}"
                                        };

            AreaPartialViewLocationFormats = new[]
                                            {
                                                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                "~/Areas/{2}/Views/{1}/{0}",
                                                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                                "~/Areas/{2}/Views/Shared/{0}"
                                            };
            ViewLocationFormats = new[]
                                {
                                    "~/Views/{1}/{0}.cshtml",
                                    "~/Views/{1}/{0}",
                                    "~/Views/Shared/{0}.cshtml",
                                    "~/Views/Shared/{0}"
                                };

            MasterLocationFormats = new[]
                                    {
                                        "~/Views/{1}/{0}.cshtml",
                                        "~/Views/{1}/{0}",
                                        "~/Views/Shared/{0}.cshtml",
                                        "~/Views/Shared/{0}"
                                    };

            PartialViewLocationFormats = new[]
                                        {
                                            "~/Views/{1}/{0}.cshtml",
                                            "~/Views/{1}/{0}",
                                            "~/Views/Shared/{0}.cshtml",
                                            "~/Views/Shared/{0}"
                                        };

            FileExtensions = new [] { "cshtml" };
        }
    }
}