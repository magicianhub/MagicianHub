using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using MagicianHub.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MagicianHub.Tests.Converters
{
    [TestClass]
    public class VisibilityToBoolConverterTests
    {
        private VisibilityToBoolConverter _visibilityToBoolConverterTests;

        [TestInitialize]
        public void Initialize()
        {
            _visibilityToBoolConverterTests = new VisibilityToBoolConverter();
        }

        [TestMethod]
        public void FalseFromVisibilityVisibleWithTrueParameter()
        {
            bool actualValue = (bool)_visibilityToBoolConverterTests.Convert(
                Visibility.Visible,
                typeof(string),
                "true",
                "null"
            );
            Assert.AreEqual(false, actualValue);
        }

        [TestMethod]
        public void FalseFromVisibilityCollapsedWithNullParameter()
        {
            bool actualValue = (bool)_visibilityToBoolConverterTests.Convert(
                Visibility.Collapsed,
                typeof(string),
                "",
                "null"
            );
            Assert.AreEqual(false, actualValue);
        }

        [TestMethod]
        public void FalseFromVisibilityCollapsedWithFalseParameter()
        {
            bool actualValue = (bool)_visibilityToBoolConverterTests.Convert(
                Visibility.Collapsed,
                typeof(string),
                false,
                "null"
            );
            Assert.AreEqual(false, actualValue);
        }

        [TestMethod]
        public void TrueFromVisibilityCollapsedWithTrueParameter()
        {
            bool actualValue = (bool)_visibilityToBoolConverterTests.Convert(
                Visibility.Collapsed,
                typeof(string),
                true,
                "null"
            );
            Assert.AreEqual(true, actualValue);
        }

        [TestMethod]
        public void TrueFromVisibilityVisibleWithNullParameter()
        {
            bool actualValue = (bool)_visibilityToBoolConverterTests.Convert(
                Visibility.Visible,
                typeof(string),
                "",
                "null"
            );
            Assert.AreEqual(true, actualValue);
        }

        [TestMethod]
        public void TrueFromVisibilityVisibleWithFalseParameter()
        {
            bool actualValue = (bool)_visibilityToBoolConverterTests.Convert(
                Visibility.Visible,
                typeof(string),
                false,
                "null"
            );
            Assert.AreEqual(true, actualValue);
        }
    }
}
