using System.Linq;
using System.Text;

namespace MagicianHub.Extensions
{
    public static class ArrayExtension
    {
        public static string Normalize<T>(this T[] array)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");

            for (var i = 0; i < array.Length; i++)
            {
                var item = array[i];
                if (i == array.Length - 1)
                {
                    stringBuilder.Append($"\"{item}\"");
                    stringBuilder.Append("]");
                }
                else
                {
                    stringBuilder.Append($"\"{item}\",");
                }
            }

            return stringBuilder.ToString();
        }

        public static T[] RemoveByIndex<T>(this T[] array, int index)
        {
            var list = array.ToList();
            list.RemoveAt(index);
            var newArray = list.ToArray();
            return newArray;
        }
    }
}
