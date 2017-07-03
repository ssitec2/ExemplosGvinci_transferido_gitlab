using System.Text;
using System;

namespace PROJETO
{

	/// <summary>
	/// Decodificador e codificador de string em base 64
	/// </summary>
	public static class Base64
	{

		// caracteres base64 padrão
		private const string Base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

		/// <summary>
		/// Codificador MIME
		/// </summary>
		/// <param name="intIn">String a ser codificada</param>
		/// <returns>String codificada</returns>

		private static string MimeEncode(int intIn)
		{
			if (intIn >= 0)
			{
				return Base64Chars.Substring(intIn, 1);
			}
			else
			{
				return "";
			}
		}

		/// <summary>
		/// Codifica para Base64
		/// </summary>
		/// <param name="strIn">Sting a ser codificada</param>
		/// <returns>string codificada</returns>
		public static string Encode(string strIn)
		{
			int c1, c2, c3, w1, w2, w3, w4, n;
			string strOut = "";
			for (n = 0; n < strIn.Length; n += 3)
			{
				c1 = 0; c2 = 0; c3 = 0;
				w1 = -1; w2 = -1; w3 = -1; w4 = -1;
				c1 = Utility.Asc(strIn.Substring(n, 1));
				if (n + 1 < strIn.Length)
					c2 = Utility.Asc(strIn.Substring(n + 1, 1));
				if (n + 2 < strIn.Length)
					c3 = Utility.Asc(strIn.Substring(n + 2, 1));
				w1 = Convert.ToInt32(c1 / 4);
				w2 = (c1 & 3) * 16 + Convert.ToInt32(c2 / 16);
				if (strIn.Length >= n + 2) w3 = (c2 & 15) * 4 + Convert.ToInt32(c3 / 64);
				if (strIn.Length >= n + 3) w4 = (c3 & 63);
				strOut += MimeEncode(w1) + MimeEncode(w2) + MimeEncode(w3) + MimeEncode(w4);
			}
			return strOut + "==";
		}

		/// <summary>
		/// Decodifica Base64
		/// </summary>
		/// <param name="strIn">String a ser decodificada</param>
		/// <returns>String decodificada</returns>
		public static string Decode(string strIn)
		{
			int w1, w2, w3, w4, n;
			string strOut = "";
			string x = "";
			for (n = 0; n < strIn.Length; n = n + 4)
			{
				w1 = -1; w2 = -1; w3 = -1; w4 = -1;
				w1 = MimeDecode(strIn.Substring(n, 1));
				if (n + 1 < strIn.Length)
					w2 = MimeDecode(strIn.Substring(n + 1, 1));
				if (n + 2 < strIn.Length)
					w3 = MimeDecode(strIn.Substring(n + 2, 1));
				if (n + 3 < strIn.Length)
					w4 = MimeDecode(strIn.Substring(n + 3, 1));
				if (w2 >= 0) { strOut += Utility.Chr(((w1 * 4 + Convert.ToInt32(w2 / 16)) & 255)); x += Convert.ToString(((w1 * 4 + Convert.ToInt32(w2 / 16)) & 255)) + " - "; }
				if (w3 >= 0) { strOut += Utility.Chr(((w2 * 16 + Convert.ToInt32(w3 / 4)) & 255)); x += Convert.ToString(((w2 * 16 + Convert.ToInt32(w3 / 4)) & 255)) + " - "; }
				if (w4 >= 0) { strOut += Utility.Chr(((w3 * 64 + w4) & 255)); x += Convert.ToString(((w3 * 64 + w4) & 255)) + " - "; }
			}
			return strOut;
		}

		/// <summary>
		/// Decodificador para MIME
		/// </summary>
		/// <param name="strIn">Sting a ser decodificada</param>
		/// <returns>String decodificada</returns>
		private static int MimeDecode(string strIn)
		{
			if (strIn.Length > 0)
			{
				return Base64Chars.IndexOf(strIn);
			}
			else
			{
				return -1;
			}
		}

	}

}
