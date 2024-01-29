using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyzer
{
	public static class DataTable
	{
		public static List<string> GetServiceWords()
		{
			return new List<string>()
			{
				"program",
				"var",
				"begin",
				"end",
				"if",
				"then",
				"else",
				"for",
				"to",
				"do",
				"while",
				"read",
				"write", 
				"true", 
				"false",
				"!",
				"%",
				"$"
			};

		}
		public static List<string> GetSeparators()
		{
			return new List<string>()
			{
				"NE",
				"EQ",
				"LT",
				"LE",
				"GT",
				"GE",
				"plus",
				"min",
				"or",
				"mult",
				"div",
				"and",
				"~",
				";",
				".",
				",",
				":",
				"/n",
				"(",
				")",
				"as",
				"*"
			};

		}
	}
}
