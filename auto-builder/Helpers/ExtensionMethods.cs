using System.Text;

namespace AutoBuilder.Helpers
{
    public static class ExtensionMethods
    {
        public static string ToPascalCasing(this string original)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == '-')
                {
                    i++;
                    if (i < original.Length)
                        stringBuilder.Append(char.ToUpper(original[i]));
                }
                else
                {
                    stringBuilder.Append(original[i]);
                }
            }

            stringBuilder[0] = char.ToUpper(stringBuilder[0]);
            return stringBuilder.ToString();
        }

        public static string ToKebabCasing(this string original)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < original.Length; i++)
            {
                if (char.IsUpper(original[i]) && i != 0)
                {
                    stringBuilder.Append('-');
                    stringBuilder.Append(char.ToLower(original[i]));
                }
                else
                {
                    stringBuilder.Append(char.ToLower(original[i]));
                }
            }

            return stringBuilder.ToString();
        }
    }
}
