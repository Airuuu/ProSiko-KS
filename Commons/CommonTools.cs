using System.Runtime.Intrinsics.Arm;

namespace Commons
{
    public static class CommonTools
    {
        public static string Trush(int n)
        {
            char[] chars = new char[n];
            int m = 'Z' - 'A' + 1;
            for (int i = 0; i < n; i++)
            {
                chars[i] = (char)('A' + i % m);
            }
            return new string(chars);
        }

        public static string SubstringMax(this string str, int len)
        {
            return str.Substring(0, Math.Min(len, str.Length));
        }

        public static int GetAnswerLength(string command)
        {
            var separator = " ";
            var i = command.IndexOf(separator);
            var j = command.IndexOf(separator, i + 1);
            return int.Parse(command.Substring(i + 1, j - i - 1));
        }
    }
}
