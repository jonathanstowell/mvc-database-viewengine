using System;
using System.Collections.Generic;
using System.Linq;
using MvcThemable.Views.Models.Abstract;
using MvcThemable.Views.Models.Concrete;
using NUnit.Framework;

namespace MvcThemable.Tests.Views.ModelLocations
{
    [TestFixture]
    public class ProvideViewModelsTests
    {
        [Test]
        public void GetModelsTest()
        {
            IProvideViewModels provideViewModels = new ProvideViewModels();

            IDictionary<string, string> result = provideViewModels.GetModels("MvcThemable.Tests.Views.ModelLocations.Models");

            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GetModelsPropertiesTest()
        {
            IProvideViewModels provideViewModels = new ProvideViewModels();

            IDictionary<string, Type> result = provideViewModels.GetModelProperties("MvcThemable.Tests.Views.ModelLocations.Models.TestViewModel, MvcThemable.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            Assert.That(result.Count(), Is.EqualTo(2));
        }
    }
}
