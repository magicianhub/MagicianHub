using MagicianHub.Converters;
using Windows.UI.Xaml;
using Xunit;

namespace MagicianHub.Tests.Converters
{
    public class VisibilityToBoolConverterTests
    {
        private readonly VisibilityToBoolConverter _visibilityToBoolConverterTests 
            = new VisibilityToBoolConverter();

        [Fact]
        public void FalseFromVisibilityVisibleWithTrueParameter()
        {
            bool actualValue = (bool)_visibilityToBoolConverterTests.Convert(
                Visibility.Visible,
                typeof(string),
                "true",
                "null"
            );
            Assert.False(actualValue);
        }

        [Fact]
        public void FalseFromVisibilityCollapsedWithNullParameter()
        {
            bool actualValue = (bool)_visibilityToBoolConverterTests.Convert(
                Visibility.Collapsed,
                typeof(string),
                "",
                "null"
            );
            Assert.False(actualValue);
        }

        [Fact]
        public void FalseFromVisibilityCollapsedWithFalseParameter()
        {
            bool actualValue = (bool)_visibilityToBoolConverterTests.Convert(
                Visibility.Collapsed,
                typeof(string),
                false,
                "null"
            );
            Assert.False(actualValue);
        }

        [Fact]
        public void TrueFromVisibilityCollapsedWithTrueParameter()
        {
            bool actualValue = (bool)_visibilityToBoolConverterTests.Convert(
                Visibility.Collapsed,
                typeof(string),
                true,
                "null"
            );
            Assert.True(actualValue);
        }

        [Fact]
        public void TrueFromVisibilityVisibleWithNullParameter()
        {
            bool actualValue = (bool)_visibilityToBoolConverterTests.Convert(
                Visibility.Visible,
                typeof(string),
                "",
                "null"
            );
            Assert.True(actualValue);
        }

        [Fact]
        public void TrueFromVisibilityVisibleWithFalseParameter()
        {
            bool actualValue = (bool)_visibilityToBoolConverterTests.Convert(
                Visibility.Visible,
                typeof(string),
                false,
                "null"
            );
            Assert.True(actualValue);
        }
    }
}
