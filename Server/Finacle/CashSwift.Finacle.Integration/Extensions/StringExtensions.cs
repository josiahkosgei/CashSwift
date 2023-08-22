using CashSwift.Library.Standard.Utilities;
using System.Text;

namespace CashSwift.Finacle.Integration.Extensions
{
    public static class StringExtensions
	{
		public static byte[] FromBase64String(this string s)
		{
			return Convert.FromBase64String(s);
		}

		public static Uri ToURI(this string s)
		{
			return new Uri(s);
		}

		public static Stream ToStream(this string s)
		{
			return s.ToStream(Encoding.UTF8);
		}

		public static Stream ToStream(this string s, Encoding encoding)
		{
			return new MemoryStream(encoding.GetBytes(s ?? ""));
		}

		public static bool isEmail(this string s)
		{
			RegexUtilities regexUtilities = new RegexUtilities();
			return regexUtilities.IsValidEmail(s);
		}

		public static string Left(this string s, int count)
		{
			if (s != null && s.Count() > 0)
			{
				return new string(s.Take(Math.Max(0, count)).ToArray());
			}
			return null;
		}

		public static string Shuffle(this string s)
		{
			List<char> list = s.ToList();
			list.Shuffle();
			return new string(list.ToArray());
		}
	}

}
