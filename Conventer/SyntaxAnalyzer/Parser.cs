using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyzer
{
	public static class Parser
	{
		public static string StartParser(string programStr)
		{
			while (programStr.Contains("\n")) { programStr = programStr.Replace("\n", " "); }
			while (programStr.Contains("\r")) { programStr = programStr.Replace("\r", " "); }
			for (int i = 0; i < programStr.Length; i++)
			{
				if (programStr[i] == ',' || (programStr[i] == '(' && programStr[i+1] != '*') || (programStr[i] == ')' && programStr[i-1] != '*') || programStr[i] == ':' || programStr[i] == ';')
				{
					programStr = programStr.Insert(i, " ");
					programStr = programStr.Insert(i+2, " ");
					i +=2;
				}
				else if (programStr[i] == '=')
				{
					programStr = programStr.Insert(i + 1, " ");
				}
			}
			while (programStr.Contains("  ")) { programStr = programStr.Replace("  ", " "); }
			while (programStr.Contains("end.")) { programStr = programStr.Replace("end.", "end ."); }

			string tempStr = "";
			bool comment = false;
			for (int i = 0; i < programStr.Count(); i++)
			{

				if (programStr[i] == '(' && programStr[i+1] == '*')
				{
					comment = true;
					i++;
				}
				else if(programStr[i] == '*' && programStr[i+1]==')')
				{
					i++;
					comment = false;
				}
				else if (!comment)
				{
					tempStr += programStr[i];
				}
			}
			while (tempStr.Contains("  ")) { tempStr = tempStr.Replace("  ", " "); }

			return tempStr;
		}
	}
}
