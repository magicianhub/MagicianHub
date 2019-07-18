using MagicianHub.Extensions;
using Xunit;

namespace MagicianHub.Tests.Extensions
{
    public class StringHexCoderExtensionTests
    {
        [Fact]
        public void StringFromHex()
        {
            var actual =
                "303436306e7032647177367361643062393971306136766263746c61323330713636"
                .FromHex();
            var expected = "0460np2dqw6sad0b99q0a6vbctla230q66";
            Assert.Equal(expected, actual);
        }
    }
}
