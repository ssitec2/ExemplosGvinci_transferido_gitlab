using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using PROJETO;

namespace PROJETO
{

	public static class Crypt
	{

		/// <summary>
		/// Função para encriptação de string
		/// </summary>
		/// <param name="Exp">String a ser codificada</param>
		/// <param name="Chv">Chave para ser usada na codificação</param>
		/// <returns>Retorna a string encriptada</returns>
		public static string S001(string Exp, string Chv)
		{
			string RetVal, ExpChv, pi, pp;
			int i, j, Cont, t, ContChv;
			string[] Dados;
			byte bit;

			t = Exp.Length;
			Dados = new string[t];

			ExpChv = "";

			//vamos montar a chave para encriptação
			for (i = 0; i < Chv.Length; i++)
			{
				for (j = 7; j >= 0; j--)
				{
					bit = Convert.ToByte((Utility.Asc(Chv.Substring(i, 1)) & (long)Math.Pow(2, j)) != 0);
					ExpChv += Convert.ToString(bit);
				}
			}

			//vamos misturar os bits da string
			ContChv = 0;
			for (Cont = 0; Cont < t; Cont++)
			{
				pp = "";
				pi = "";
				for (j = 7; j >= 0; j--)
				{
					// se ímpar...
					if ((j & 1) != 0)
					{
						//usa o primeiro char
						bit = Convert.ToByte(((Utility.Asc(Exp.Substring(Cont, 1)) & (long)Math.Pow(2, j)) != 0) ^
																	(Convert.ToInt32(ExpChv.Substring(ContChv, 1)) != 0));
						pi += Convert.ToString(bit);
					}
					// par
					else
					{
						// usa o último char
						bit = Convert.ToByte(((Utility.Asc(Exp.Substring(t - Cont - 1, 1)) & (long)Math.Pow(2, j)) != 0) ^
																	(Convert.ToInt32(ExpChv.Substring(ContChv, 1)) != 0));
						pp += Convert.ToString(bit);
					}
					ContChv += 1;
					if (ContChv == ExpChv.Length) ContChv = 0;
				}
				Dados[Cont] = (pp + pi);
			}

			//agora vamos montar a string já encriptada para retorno
			RetVal = "";
			for (i = 0; i < t; i++)
			{
				RetVal += Utility.Chr(RetAsc(Dados[i]));
			}

			return RetVal;

		}

		/// <summary>
		/// Função para desencriptação de string
		/// </summary>
		/// <param name="Exp">String a ser codificada</param>
		/// <param name="Chv">Chave para ser usada na codificação</param>
		/// <returns>retorna a string decriptada</returns>
		static public string S002(string Exp, string Chv)
		{
			string RetVal, z, ExpChv;
			string[] Dados;
			int j, i, Cont, t, ContChv, k, ContIni;
			byte bit;

			t = Exp.Length;
			Dados = new string[t];
			ExpChv = "";

			//vamos montar a chave para encriptação
			for (i = 0; i < Chv.Length; i++)
			{
				for (j = 7; j >= 0; j--)
				{
					bit = Convert.ToByte((Utility.Asc(Chv.Substring(i, 1)) & (long)Math.Pow(2, j)) != 0);
					ExpChv += Convert.ToString(bit);
				}
			}

			//desfaz o xor com a chave  (a posição de chave que deverá ser utilizada para desencriptar cada byte deverá ser: 13570246)
			ContChv = 0;
			for (i = 0; i < Exp.Length; i++)
			{
				z = "";
				ContIni = ContChv;
				ContChv += 1;
				if (ContChv == ExpChv.Length) ContChv = 0;
				for (j = 0; j <= 7; j++)
				{
					bit = Convert.ToByte(Convert.ToInt32((Utility.Asc(Exp.Substring(i, 1)) & (long)Math.Pow(2, j)) != 0) ^
																Convert.ToInt32(ExpChv.Substring(ContChv, 1)));
					z = z + Convert.ToString(bit);
					if (j == 3)
						ContChv = ContIni;
					else
					{
						for (k = 1; k <= 2; k++)
						{
							ContChv += 1;
							if (ContChv == ExpChv.Length) ContChv = 0;
						}
					}
				}
				Utility.ChangeStr(ref Exp, i, Utility.Chr(RetAsc(z)));
			}

			//agora vamos corrigir a posição de cada bit
			for (Cont = 0; Cont < t; Cont++)
			{
				for (j = 7; j >= 0; j--)
				{
					// se ímpar...
					if ((j & 1) != 0)
					{

						// calcula bit original
						i = (7 - Convert.ToInt32(j / 2));

						// usa primeiro char
						bit = Convert.ToByte(Convert.ToInt32(Utility.Asc(Exp.Substring(Cont, 1)) & (long)Math.Pow(2, i)) != 0);

					}
					// par
					else
					{
						// calcula bit original...
						i = 3 - (j / 2);

						// usa último char
						bit = Convert.ToByte(Convert.ToInt32(Utility.Asc(Exp.Substring(t - Cont - 1, 1)) & (long)Math.Pow(2, i)) != 0);
					}
					Dados[Cont] = Convert.ToString(bit) + Dados[Cont];
				}
			}

			//agora vamos montar a string já desencriptada para retorno
			RetVal = "";
			for (i = 0; i < t; i++)
			{
				RetVal += Utility.Chr(RetAsc(Dados[i]));
			}

			return RetVal;
		}

		/// <summary>
		/// retorna string de acordo com a sua representação binária
		/// </summary>
		/// <param name="Bin">Representação binaria da string</param>
		/// <returns>string de acordo com a sua representação binária</returns>
		static int RetAsc(string Bin)
		{
			int i, RetVal;
			RetVal = 0;
			for (i = 0; i <= 7; i++)
			{
				RetVal += (int)(Convert.ToInt32(Bin.Substring(i, 1)) * Math.Pow(2, i));
			}
			return RetVal;
		}
		/// <summary>
		/// gera lixo para embaralhar os dados
		/// </summary>
		/// <param name="Lenght">Tamanho da string</param>
		/// <param name="NoNumber">Se tem numero ou nao</param>
		/// <returns>String com lixo embaralhado</returns>
		public static string MakeTrash(int Lenght, Boolean NoNumber)
		{
			long i, c;
			string RetVal;
			Random SysRand = new System.Random();
			RetVal = "";
			for (i = 1; i <= Lenght; i++)
			{
				c = SysRand.Next(32, 255);
				if (NoNumber = false | (c < 48 | c > 57))
					RetVal = RetVal + Utility.Chr((int)c);
				else
					i = i - 1;
			}
			return RetVal;
		}
		/// <summary>
		/// gera lixo para embaralhar os dados
		/// </summary>
		/// <param name="Lenght">Tamanho da string</param>
		/// <returns>String com lixo embaralhado</returns>
		public static string MakeTrash(int Lenght)
		{
			return MakeTrash(Lenght, false);
		}

		/// <summary>
		/// Encripta a string 
		/// </summary>
		/// <param name="St">string a ser encriptada</param>
		/// <param name="Pw">Chave de encriptação</param>
		/// <returns>String encriptada</returns>
		private static string Cript(string St, string Pw)
		{
			string x = "";
			int i;
			int n = 0;
			int p;
			int j = 0;
			p = 0;
			for (i = 0; i < St.Length; i++)
			{

				if (p >= Pw.Length)
				{
					p = 0;
				}

				j = Utility.Asc(Pw.Substring(p, 1)) | 128;

				n = Utility.Asc(St.Substring(i));
				p = p + 1;
			DeNovo:
				n = n ^ j;
				if (n < 31)
				{
					n = (128 + n);
					goto DeNovo;
				}
				else if (n > 127 && n < 159)
				{
					n = n - 128;
					goto DeNovo;
				}
				x = x + Utility.Chr(n);
			}

			return x;
		}

		/// <summary>
		/// Encripta a string
		/// </summary>
		/// <param name="vgSt">Sting a ser encriptada</param>
		/// <returns>string encriptada</returns>
		public static string Encripta(string vgSt)
		{
			string vgRetVal;
			vgRetVal = Base64.Encode(S001(vgSt, "PROJETO_CHAVE"));
			
			return vgRetVal;
		}

		/// <summary>
		/// Decripta a string
		/// </summary>
		/// <param name="vgSt">String a ser decripitada</param>
		/// <returns>String decriptada</returns>
		public static string Decripta(string vgSt)
		{
			string vgRetVal;
			vgRetVal = S002(Base64.Decode(vgSt), "PROJETO_CHAVE");
			return vgRetVal;
		}

        /// <summary>
        /// Encripta um Byte[]
        /// </summary>
        /// <param name="data">byte[] a ser criptografado</param>
        /// <returns>byte[] criptografado</returns>
        public static byte[] Encript_AES(byte[] data)
        {
            try
            {
                byte[] data2;

                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    byte[] pass = Encoding.Default.GetBytes("PROJETO_CHAVE");

                    Rfc2898DeriveBytes GeradorChave = new Rfc2898DeriveBytes("PROJETO_CHAVE", pass);

                    byte[] bKey = GeradorChave.GetBytes(16);
                    byte[] bIV = GeradorChave.GetBytes(16);

                    aes.Key = bKey;
                    aes.IV = bIV;

                    ICryptoTransform cryptTransf = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoStr = new MemoryStream())
                    {
                        using (CryptoStream cryptStream = new CryptoStream(memoStr, cryptTransf, CryptoStreamMode.Write))
                        {
                            cryptStream.Write(data, 0, data.Length);
                            cryptStream.FlushFinalBlock();
                        }

                        data2 = memoStr.ToArray();
                    }
                }

                return data2;
            }
            catch (Exception)
            {
            }
			return new byte[0];
        }

        /// <summary>
        /// Decripta um Byte[] OBS: Deve ser encriptado pela função: "Encript_AES(byte[] data)"
        /// </summary>
        /// <param name="data">byte[] a ser decripitado</param>
        /// <returns>byte[] decriptado</returns>
        public static byte[] Decript_AES(byte[] data)
        {
            try
            {
                byte[] data2;

                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    byte[] pass = Encoding.Default.GetBytes("PROJETO_CHAVE");

                    Rfc2898DeriveBytes GeradorChave = new Rfc2898DeriveBytes("PROJETO_CHAVE", pass);

                    byte[] bKey = GeradorChave.GetBytes(16);
                    byte[] bIV = GeradorChave.GetBytes(16);

                    aes.Key = bKey;
                    aes.IV = bIV;

                    ICryptoTransform deCryptTransf = aes.CreateDecryptor(aes.Key, aes.IV);                   

                    using (MemoryStream memoStr = new MemoryStream(data))
                    {
                        using (CryptoStream cryptStream = new CryptoStream(memoStr, deCryptTransf, CryptoStreamMode.Read))
                        {
							using (BinaryReader Reader = new BinaryReader(cryptStream))
							{
								data2 = Reader.ReadBytes(data.Length);
							}
                        }
                    }
                }
                return data2;
            }
            catch (Exception)
            {
            }
			return new byte[0];
        }

	}

}
