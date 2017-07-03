using System;
using System.Web;
using COMPONENTS;
using System.Web.UI;
using COMPONENTS.Configuration;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace PROJETO
{
	public static class ServerValidation
	{
		/// <summary>
		/// Valida uma String usando como base a validação de CNPJ
		/// </summary>
		/// <param name="St">String que será validada</param>
		/// <returns>true caso o string de entrada for um CNPJ válido</returns>
	
		public static bool CheckCNPJ(object CNPJ)
		{
			string _CNPJ = (string)CNPJ.ToString();
			bool Digits_Equal = true;
			string Numbers, Digits;

			_CNPJ = GetOnlyNumbers(_CNPJ);

			if (_CNPJ.Length != 14)
				return false;

			Digits = _CNPJ.Substring(0, 1); ;
			for (int i = 0; i < _CNPJ.Length; i++)
			{
				if (Digits != _CNPJ.Substring(i, 1))
				{
					Digits_Equal = false;
					break;
				}
			}

			if (!Digits_Equal)
			{
				Numbers = _CNPJ.Substring(0, 12);
				Digits = _CNPJ.Substring(12, 2);

				if (GetCNPJDigit(Numbers).ToString() == Digits.Substring(0, 1) && GetCNPJDigit(Numbers + Digits.Substring(0, 1)).ToString() == Digits.Substring(1, 1))
				{
					return true;
				}
				else
				{
					return false;
				}

			}

			return false;

		}
		private static long GetCNPJDigit(string CNPJ)
		{
			int Weight = CNPJ.Length - 7;
			int Sum = 0;

			for (int i = 0; i <= CNPJ.Length - 1; i++)
			{
				Sum += Weight * Convert.ToInt32(CNPJ[i].ToString());
				Weight = (Weight == 2) ? 9 : Weight - 1;
			}

			return ((Sum % 11) < 2 ? 0 : (11 - (Sum % 11)));
		}

		/// <summary>
		/// Valida uma String usando como base a validação de CPF
		/// </summary>
		/// <param name="St">String que será validada</param>
		/// <returns>true caso o string de entrada for um CPF válido</returns>
		public static bool CheckCPF(object CPF)
		{
			string _CPF = (string)CPF.ToString();
			bool Digits_Equal = true;
			string Numbers, Digits;

			_CPF = GetOnlyNumbers(_CPF);

			if (_CPF.Length != 11)
				return false;

			Digits = _CPF.Substring(0, 1); ;
			for (int i = 0; i < _CPF.Length; i++)
			{
				if (Digits != _CPF.Substring(i, 1))
				{
					Digits_Equal = false;
					break;
				}
			}

			if (!Digits_Equal)
			{
				Numbers = _CPF.Substring(0, 9);
				Digits = _CPF.Substring(9, 2);
				if (GetCPFDigit(Numbers).ToString() == Digits.Substring(0, 1) && GetCPFDigit(Numbers + Digits.Substring(0, 1)).ToString() == Digits.Substring(1, 1))
				{
					return true;
				}
				else
				{
					return false;
				}

			}
			return false;
		}

		private static long GetCPFDigit(string CPF)
		{
			int Weight = 2;
			int Sum = 0;
			for (int i = CPF.Length - 1; i >= 0; i--)
			{
				Sum += Weight * Convert.ToInt32(CPF[i].ToString());
				Weight++;
			}
			return ((11 - (Sum % 11)) > 9 ? 0 : (11 - (Sum % 11)));
		}

		/// <summary>
		/// Retorna uma string com somente os numeros da string de entrada
		/// </summary>
		/// <param name="St">String que será parseada</param>
		/// <returns>Retorna a string com os numeros presentes na string de entrada</returns>
		private static string GetOnlyNumbers(string str)
		{
			try
			{
				Regex reg = new Regex("[^0-9]");
				str = reg.Replace(str, "");
				return str;
			}
			catch (Exception ex)
			{
				return "";
			}
		}
		
		public static bool CheckDayMonth(object str)
		{ 
			string[] Splited = (str.ToString()).Split(new char[]{'/','-'});
			if (Splited.Length == 2)
			{
				int Day = 0;
				int Month = 0;
				try
				{
					Day = Convert.ToInt16(Splited[0]);
					Month = Convert.ToInt16(Splited[1]);
				}
				catch (Exception)
				{
					return false;
				}

				if ((Day >= 1 && Day <= 31) && (Month >= 1 && Month <= 12)) //verifica se os numeros sao validos
				{
					if ((Day == 29 && Month == 2)){return true;}
					//verifica os monthes de 30 days
					if ((Day <= 30) && (Month == 4 || Month == 6 || Month == 9 || Month == 11)){return true;}
					//verifica os monthes de 31 days
					if ((Day <= 31) && (Month == 1 || Month == 3 || Month == 5 || Month == 7 || Month == 8 || Month == 10 || Month == 12)){return true;}
				}
			}
			return false;
		}
		
		public static bool CheckMonthYear(object str)
		{
			string[] Splited = (str.ToString()).Split(new char[] { '/', '-' });
			if (Splited.Length == 2)
			{
				int Month = 0;
				int Year = 0;
				try
				{
					Month = Convert.ToInt16(Splited[0]);
					Year = Convert.ToInt16(Splited[1]);
				}
				catch (Exception)
				{
					return false;
				}

				if ((Month >= 1 && Month <= 12) && (Year >= 1900 && Year <= 2100)) { return true; }
			}
			return false;
		}
		
		public static bool IsWholeNumber(object strNumber)
		{
			Regex objNotWholePattern = new Regex("[^0-9]");

			return !objNotWholePattern.IsMatch(strNumber.ToString());
		}

		public static bool CheckNotEmpty(object str)
		{
			if (str is string)
			{
				return str.ToString().Length != 0;
			}
			else if(str is short || str is int || str is long || str is double)
			{
				return Convert.ToInt64(str) != 0;
			}
			else if (str is DateTime)
			{
				return !str.Equals(default(DateTime)) && !str.Equals(new DateTime(599266080000000000));
			}
			else
			{
				return (str != null);
			}
		}

		public static bool CheckNumber(object strNumber)
		{
			Regex objNotNaturalPattern = new Regex("[^0-9]");
			Regex objNaturalPattern = new Regex("0*[1-9][0-9]*");
			return !objNotNaturalPattern.IsMatch(strNumber.ToString()) && objNaturalPattern.IsMatch(strNumber.ToString());
		}

		public static bool CheckCharOnly(object ObjValidation)
		{
			string strToCheck = ObjValidation.ToString() ;
			Regex objAlphaPattern = new Regex("[^a-zA-Z]");
			return !objAlphaPattern.IsMatch(strToCheck);
		}

		public static bool CheckNoSpecialChar(object strToCheck)
		{
			Regex objAlphaNumericPattern = new Regex("[^a-zA-Z0-9]");
			return !objAlphaNumericPattern.IsMatch(strToCheck.ToString());
		}

		public static bool CheckEmail(object strToCheck)
		{

			string patternStrict = @"^(([^<>()[\]\\.,;:\s@\""]+"
									+ @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
									+ @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
									+ @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
									+ @"[a-zA-Z]{2,}))$";

			Regex objEmailPattern = new Regex(patternStrict);
			return objEmailPattern.IsMatch(strToCheck.ToString());
		}

		public static bool CheckCreditCard(object strToCheck)
		{
			int[] DELTAS = new int[] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
			int checksum = 0;
			char[] chars = strToCheck.ToString().ToCharArray();
			for (int i = chars.Length - 1; i > -1; i--)
			{
				int j = ((int)chars[i]) - 48;
				checksum += j;
				if (((i - chars.Length) % 2) == 0)
					checksum += DELTAS[j];
			}

			return ((checksum % 10) == 0);
		}

		public static bool CheckDate(object sDate)
		{
			DateTime dt;
			bool isDate = true;

			try
			{
				dt = DateTime.Parse(sDate.ToString());
			}
			catch
			{
				isDate = false;
			}
			return isDate;
		}
	}
}
