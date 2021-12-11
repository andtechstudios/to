﻿using System.Linq;
using System.Text.RegularExpressions;

namespace Andtech.To
{
	public struct Query
	{
		public string RawKeywords { get; private set; }
		public string[] Keywords { get; set; }
		public string Path { get; set; }
		public string Search { get; set; }

		public static Query Parse(string input)
		{
			var hotspotMatch = Regex.Match(input, @"^(?<value>[^/?]+)");
			var suffixMatch = Regex.Match(input, @"/(?<value>.+)");
			var searchMatch = Regex.Match(input, @"\?(?<value>.+)");

			var rawKeywords = hotspotMatch.Groups["value"].Value;
			var queryKeywords = Regex.Split(rawKeywords, @"[\s]+")
				.Select(x => x.Trim())
				.ToArray();

			return new Query
			{
				RawKeywords = rawKeywords,
				Keywords = queryKeywords,
				Path = suffixMatch.Success ? suffixMatch.Groups["value"].Value.Trim() : null,
				Search = searchMatch.Success ? searchMatch.Groups["value"].Value.Trim() : null,
			};
		}
	}
}

