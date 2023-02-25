
namespace CryptoApp.Module.Extension
{
    public static class StringExtension
    {
        public static string Replace(this string str, char[] old, string _new)
        {
            foreach (var c in old) str = str.Replace(c.ToString(), _new);
            return str;
        }
    }
}
