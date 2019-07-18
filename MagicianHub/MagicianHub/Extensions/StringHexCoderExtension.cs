using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace MagicianHub.Extensions
{
    public static class StringHexCoderExtension
    {
        public static string FromHex(this string str)
        {
            IBuffer buffer = CryptographicBuffer.DecodeFromHexString(str);
            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffer);
        }
    }
}
