using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyzer
{
	class SyntacticAnalyzer
	{
		public Dictionary<string, string> _initializedVariables = new Dictionary<string, string>();
		public List<string> operationsAssignments = new List<string>();
		public List<string> expression = new List<string>();

		private Form1 _form;
		private List<string> _identificators;
		private List<string> _numbers;
		
		private List<string> operations =  new List<string> { "NE", "EQ", "LT", "LE", "GT", "GE", "plus", "min", "or", "mult", "div", "and", "~" };
		public SyntacticAnalyzer(Form1 form, List<string> identificators, List<string> numbers)
		{
			_form = form;
			_identificators = identificators;
			_numbers = numbers;
		}

        public void CheckProgram(string programStr)
        {
            if (!BracketCheck(programStr))
            {
                _form.CatchError($"Нарушена парность скобок");
            }

            string[] programStructure = ReferenceStrings.Program.Split(' ');
            string[] programStrArr = programStr.Split(' ');
            int p = 0;
            for (int i = 0; i < programStructure.Length; i++)
            {
                if (programStructure[i] == programStrArr[p])
                {
                    p++;
                    continue;
                }
                if (programStructure[i] == "{description}")
                {
                    p = Description(programStrArr, p);
                    if (p == -1)
                    {
                        _form.CatchError($"Неверное описание переменных в программе");
                        break;
                    }
					continue;
                }

                if (programStructure[i] == "{body}")
                {
                    int endCheck = Body(programStrArr, p);
					if (endCheck == -1)
					{
                        _form.CatchError($"Неверный синтаксис оператора.");
                        break;
					}
					else if (endCheck==1)
					{
						break;
					}
					
                }
				if (programStructure[i]=="end." && programStrArr[p] == "end" && programStrArr[p+1] == ".")
				{
					break;
				}
				if(programStrArr[programStrArr.Length - 1]!="."&& programStrArr[programStrArr.Length-2] != "end")
				{
					_form.CatchError($"Программа должна заканчиваться на end.");
				}
				else
				{
					_form.CatchError($"Ошибка в структуре программы, не хватает program, var или begin.");
				}

            }
        }


		/// <summary>
		/// Инициализация переменных.
		/// </summary>
		public int Description(string[] str, int p)
		{
			string[] descriptionStructure = ReferenceStrings.Description.Split(' ');
			List<string> tempInd = new List<string>();
			string type = "";
			for (int i = 0; i < descriptionStructure.Length; i++)
			{
				
				if (descriptionStructure[i] == "{type}")
				{
					if (str[p] == "%" || str[p] == "!" || str[p] == "$")
					{
						type = str[p];
						p++;
						continue;
					}
					else
					{
						return -1;
					}
				}
				else if (descriptionStructure[i] == "{identifier}" && _identificators.Contains(str[p]))
				{
					tempInd.Add(str[p]);
					p++;
				}
				else if (descriptionStructure[i] == ":" && str[p]==":")
				{
					p++;
					continue;
				}
				else if (descriptionStructure[i] == "{,}")
				{
					if (str[p] == "," && _identificators.Contains(str[p + 1]))
					{
						p++;
						i -= 2;
						continue;
					}
				}
				else if (str[p] == ";")
				{
					
					while (tempInd.Count > 0)
					{
						_initializedVariables.Add(tempInd[tempInd.Count - 1], type);
						tempInd.RemoveAt(tempInd.Count - 1);
					}
					if (!_identificators.Contains(str[p + 1]))

					{
						p++;
						return p;
					}
					else
					{
						i = -1;
						p++;
						continue;
					}
				}

				else
				{
					return -1;
				}

				//if (str[p] == "%" || str[p] == "!" || str[p] == "$")
				//{
				//	p = Description(str, p);
				//}
			}

			while (tempInd.Count > 0)
			{
				_initializedVariables.Add(tempInd[tempInd.Count - 1], type);
				tempInd.RemoveAt(tempInd.Count - 1);
			}

			return p;
		}


        /// <summary>
        /// Разбор тела
        /// </summary>
		public int Body(string[] str, int p)
		{
			int pn = p;
            string[] bodyStructure = ReferenceStrings.Body.Split(' ');
            for(int i=0; i < bodyStructure.Length; i++)
			{
				while ((str[p] == "(" && str[p + 1] == "*") || str[p] == "*" || (str[p] == ")" && str[p - 1] == "*")) // если мы встретили скобки комментария
				{
					if (p == str.Length - 1 && str[p] != "end")
					{
						return -1;
					}
					p++;
					pn++;
				}
				if (str[p] == bodyStructure[0])
				{
					p++;
					pn = p;
				}
				if (CheckOperator(str, ref pn))
				{
					p = pn;
					if (str[p] == ";")
					{
						p++;
						pn = p;
						if (CheckOperator(str, ref pn))
						{
							i = 0;
							pn = p;
							continue;

						}
						else { return 0; }
						
					}
				}
				if (str[p] == "end" && str[p + 1] == ".")
				{
					return 1;
				}
				else
				{
					return -1; 
				}

            }
			return p;
        }
   //     public int Body(string[] str, int p)
   //     {
   //         string[] bodyStructure = ReferenceStrings.Body.Split(' ');
			//if (str[p] == bodyStructure[0])
			//{
			//	p++;
			//	int pn = p;
			//	while (str[p] != "end")
			//	{
			//		while ((str[p] == "(" && str[p + 1] == "*") || str[p] == "*" || (str[p]==")" && str[p-1]=="*")) // если мы встретили скобки комментария
   //                 {
   //                     if (p == str.Length - 1 && str[p] != "end")
   //                     {
			//				return -1;
   //                     }
   //                     p++;
   //                     pn++;
   //                 }

			//		if (str[p] == ";")
			//		{
			//			pn++;
			//			p++;
			//		}

   //                 if (CheckOperator(str, ref pn))
   //                 {
   //                     p = pn;
   //                 }
   //                 else
   //                 {
   //                     _form.CatchError($"Неверный синтаксис оператора {str[pn]}");
   //                     pn++;
   //                     p = pn;
   //                 }
   //                 if (str[p] == "end" && str[p + 1] == ".")
   //                 {
			//			return p;
   //                 }
                    
   //             }
   //         }
   //         else
   //         {
			//	return 0;
   //         }
			//return 0;
   //     }


        public bool CheckOperator(string[] str, ref int p)
		{
			return isAssignment(str, ref p) || isFor(str, ref p) || isIf(str, ref p) || isWhile(str, ref p) || isWrite(str, ref p) || isReadLn(str, ref p);
		}

		public bool isIf(string[] str, ref int p)
		{
			if (str[p] == "if")
			{
				p++;
				if (isExpression(str, ref p, true))
				{
					if (str[p] == "then")
					{
						p++;
						if (CheckOperator( str, ref p))
						{
							if (str[p] == "else")
							{
								p++;
								return CheckOperator(str, ref p);
							}
							else
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		private bool isReadLn(string[] str, ref int p)
		{
			if (str[p] == "read")
			{
				p++;
				if (str[p] == "(")
				{
					p++;
					while (str[p] != ")")
					{
						if (_identificators.Contains(str[p]) || str[p] == ",")
						{
							p++;
						}
						else
						{
							return false;
						}
					}

					if (str[p] == ")")
					{
						p++;
						return true;
					}
				}
			}
			return false;
		}

		private bool isWrite(string[] str, ref int p)
		{
			if (str[p] == "write")
			{
				p++;
				if (str[p] == "(")
				{
					p++;
					while (str[p] != ")")
					{
						if (isExpression(str, ref p, true))
						{
							if (str[p] == ")")
							{
								p++;
								return true;
							}
							p++;
						}
						else
						{
							return false;
						}
					}
				}
			}
			return false;
		}

		private bool isWhile(string[] str, ref int p)
		{
			if (str[p] == "while")
			{
				p++;
				if (isExpression(str, ref p, true))
				{
					if (str[p] == "do")
					{
						p++;
						return (CheckOperator(str, ref p));
					}
				}
			}
			return false;
		}

		private bool isFor(string[] str, ref int p)
		{
			if (str[p] == "for")
			{
				p++;
				if (isAssignment(str, ref p))
				{
					if (str[p] == "to")
					{
						p++;
						if (isExpression(str, ref p, true))
						{
							if (str[p] == "do")
							{
								p++;
								return CheckOperator(str, ref p);
							}
						}
					}
				}
			}
			return false;		
		}

		/// <summary>
		/// проверка оператора присваивания
		/// </summary>
		/// <returns></returns>
		public bool isAssignment(string[] str, ref int p)
		{
			if (_identificators.Contains(str[p]))
			{
				int startIndex = p;
				p++;
				if (str[p] == "as")
				{
					p++;
					if (isExpression(str, ref p))
					{
						operationsAssignments.Add(string.Join(" ", str, startIndex, p - startIndex));
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			return false;
		}

		/// <summary>
		/// Проверка выражения.
		/// </summary>
		public bool isExpression(string[] str, ref int p, bool addExpression = false)
		{
			if (_identificators.Contains(str[p]) || _numbers.Contains(str[p]) || str[p] == "true" || str[p] == "false")
			{
				int startIndex = p;
				p++;
				bool operation = false;
				while (str[p] != ";" && str[p] != ")" && str[p] != "do" && str[p] != "then" && str[p] != "to" && str[p] != "else" && str[p] != "," && str[p]!="end")
				{
					if ((_identificators.Contains(str[p]) || _numbers.Contains(str[p])) && operation)
					{
						operation = false;
						p++;
					}
					else if (operations.Contains(str[p]) && !operation)
					{
						operation = true;
						p++;
					}
					else
					{
						return false;
					}
				}
				if (addExpression)
				{
					expression.Add(string.Join(" ", str, startIndex, p - startIndex));
				}

				return true;
			}
			else
			{
				return false;
			}
			
		}

        /// <summary>
        /// Проверка на парность скобок.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static bool BracketCheck(string s)
        {
            string t = "[{(]})";
            string comment = "/*";
            Stack<char> st = new Stack<char>();
            List<char> stcomment = new List<char>();
            foreach (var x in s)
            {
                int f = t.IndexOf(x);
                int com = comment.IndexOf(x);

                if (f >= 0 && f <= 2)
                    st.Push(x);

                if (com > -1 && !stcomment.Contains(x))
                    stcomment.Add(x);
                else if (stcomment.Contains(x))
                    stcomment.Remove(x);

                if (f > 2)
                {
                    if (st.Count == 0 || st.Pop() != t[f - 3])
                        return false;
                }
            }

            if (st.Count != 0 || stcomment.Count != 0)
                return false;

            return true;
        }
    }

    public static class ReferenceStrings
	{
		public static string Program = "program var {description} begin {body} end.";
        public static string Description = "{identifier} {,} : {type} ;";
        public static string Body = "; {operator}";
	}
}
