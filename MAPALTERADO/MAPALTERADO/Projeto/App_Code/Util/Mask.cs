using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PROJETO
{
	/// <summary>
	/// Summary description for Mask
	/// </summary>
	public static class Mask
	{
		//Size = 0 para memos. Para tamanho de 32000
		/// <summary>
		/// Atribui mascara aos campos
		/// </summary>
		/// <param name="vgTextBox">Textbox que vai receber a mascara</param>
		/// <param name="vgMask">A mascara que vai ser recebida</param>
		/// <param name="vgSize">Tamanho de caracteres da string</param>
		/// <param name="vgType">Tipo de dados que serao usados no campo</param>
		/// Exemplo:
		///  podemos atribuir direto no ASPX ou aqui ....
		///  vgTextBox.Items[0].Attributes.Add("Mask", "'" + vgMask + "'~'" + vgType + "'~'" + vgSize + "'");
		public static void SetMask(WebControl vgTextBox, string vgMask, int vgSize, bool isNumeric, bool OnlyOnBlur)
		{
			Utility.SetControlOnFocus(vgTextBox);
            //vgTextBox.Attributes.Add("onFocus", "ControlOnFocus(event)");
			vgTextBox.Attributes.Add("Mask", vgMask);
            vgTextBox.Attributes.Add("IsNumeric", isNumeric.ToString());
			if (vgSize == 0)
			{
				vgTextBox.Attributes.Remove("MaxSize");
			}
			else
			{
				vgTextBox.Attributes.Add("MaxSize", vgSize.ToString());
			}
            vgTextBox.Attributes.Add("OnlyOnBlur", OnlyOnBlur.ToString());
        }

		public static void SetMask(WebControl vgTextBox, string vgMask, int vgSize, bool isNumeric)
        {
            SetMask(vgTextBox, vgMask, vgSize, isNumeric, false);
        }

		public static void SetMask(WebControl vgTextBox, string vgMask, int vgSize)
        {
            SetMask(vgTextBox, vgMask, vgSize, false,false);
        }

		public static void SetMask(WebControl vgTextBox, string vgMask)
        {
            int vgSize = 0;
            if (vgMask.StartsWith("@"))
            {
                vgSize = 32665;
            }
            else
            {
                vgSize = vgMask.Length;
            }
            SetMask(vgTextBox, vgMask, vgSize);
        }

		public static string ApplyMask(string Text, string Mask, bool NumericMask)
		{
			string FieldType = "NUMBER";
			if (!NumericMask)
			{
				FieldType = "TEXT";
			}

			return ApplyMask(Text, Mask, FieldType);
		}

		public static string ApplyMask(string Text, string Mask, string FieldType)
		{
			string Numbers = "0123456789";
			string RetVal = "";
			try
			{
				if (Mask.Length > 0)
				{
					int previousTextLength = Text.Length;
					string NewValue = "";
					int ValCount = 0;
					if (FieldType == "NUMBER")
					{
						string currentValue = Text;
						string RunningValue = "";
						int DecimalsCount = 0;
						bool IsNegative = false;
                        if (Convert.ToDouble(currentValue) < 0) IsNegative = true;
						if (Mask.IndexOf(",") != -1)
						{
							DecimalsCount = Mask.Substring(Mask.IndexOf(",")).Length;
						}
						for (var ii = 0; ii < currentValue.Length; ii++)
						{
							if (RunningValue.IndexOf(",") != -1 && RunningValue.Substring(RunningValue.IndexOf(",")).Length >= DecimalsCount)
							{
								break;
							}
							if (DecimalsCount > 0 && (ii == Mask.Length - DecimalsCount) && RunningValue.IndexOf(",") == -1 && currentValue.IndexOf(",") == -1)
							{
								RunningValue += ",";
							}
							if (Numbers.IndexOf(currentValue.Substring(ii, 1)) != -1 || currentValue.Substring(ii, 1) == "." || currentValue.Substring(ii, 1) == ",")
							{
								RunningValue += currentValue.Substring(ii, 1);
							}
						}

						if (Mask.IndexOf(".") != -1 || Mask.IndexOf(",") != -1)
						{
							var AllDots = 0;
							while (RunningValue.IndexOf(".") != -1)
							{
								RunningValue = RunningValue.Replace(".", "");
								AllDots++;
							}
							var NoneDecimal = RunningValue;
							if (RunningValue.IndexOf(",") != -1)
							{
								NoneDecimal = RunningValue.Substring(0, RunningValue.IndexOf(","));
							}
							var AddedDots = 0;
							if (NoneDecimal.Length >= 4)
							{
								var DotCount = NoneDecimal.Length - 3;
								while (DotCount > 0)
								{
									NoneDecimal = NoneDecimal.Substring(0, DotCount) + "." + NoneDecimal.Substring(DotCount);
									DotCount -= 3;
									AddedDots++;
								}
							}
							RetVal = NoneDecimal + (RunningValue.IndexOf(",") != -1 ? RunningValue.Substring(RunningValue.IndexOf(",")) : "");
							if (RetVal.IndexOf(",") == -1 && DecimalsCount > 0)
							{
								RetVal += ",";
							}
							if (DecimalsCount > RetVal.Substring(RetVal.IndexOf(",")).Length)
							{
								int zerosToAdd = RetVal.Substring(RetVal.IndexOf(",")).Length;
								for (int ct = 0; ct < DecimalsCount - zerosToAdd; ct++)
								{
									RetVal += "0";
								}
							}
						}
						else
						{
							RetVal = RunningValue;
						}
						if (IsNegative) RetVal = "-" + RetVal;
					}
					else if (FieldType == "TEXT")
					{
						if (Mask.Substring(0, 1) == "@")
						{
							bool AllowSpecialCharacters = false;
							bool AllUpperCase = false;
							bool AllLowerCase = false;
							bool CapitalCharacters = false;
							bool AllowCharacters = false;
							bool AllowNumbers = false;
							bool AllowSpaces = false;
							if (Mask.IndexOf("!") != -1)
							{
								AllowSpaces = false;
								AllowNumbers = true;
								CapitalCharacters = true;
								AllowCharacters = true;
								AllowSpecialCharacters = true;
							}
							if (Mask.IndexOf("a") != -1)
							{
								AllLowerCase = true;
								AllowSpaces = true;
								AllowNumbers = false;
								AllowCharacters = true;
								AllowSpecialCharacters = false;
							}
							if (Mask.IndexOf("A") != -1)
							{
								AllUpperCase = true;
								AllowSpaces = true;
								AllowNumbers = false;
								AllowCharacters = true;
								AllowSpecialCharacters = false;
							}
							if (Mask.IndexOf("N") != -1)
							{
								AllowNumbers = true;
								AllowCharacters = true;
								AllowSpecialCharacters = false;
								AllowSpaces = false;
							}
							if (Mask.IndexOf("9") != -1)
							{
								AllowNumbers = true;
								AllowSpaces = false;
								AllowCharacters = false;
								CapitalCharacters = false;
								AllowSpecialCharacters = false;
							}
							if (Mask.IndexOf("#") != -1)
							{
								AllowNumbers = true;
								AllowSpaces = true;
								AllowCharacters = false;
								CapitalCharacters = false;
								AllowSpecialCharacters = false;
							}
							if (Mask.IndexOf("X") != -1)
							{
								AllowSpecialCharacters = true;
								CapitalCharacters = false;
								AllowCharacters = true;
								AllowNumbers = true;
								AllowSpaces = true;
							}
							for (var i = 0; i < Text.Length; i++)
							{
								if (Numbers.IndexOf(Text.Substring(i, 1)) != -1 && AllowNumbers)
								{
									NewValue += Text.Substring(i, 1);
								}
								if (Numbers.IndexOf(Text.Substring(i, 1)) == -1)
								{
									System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
									int valASC = Convert.ToInt32(encoding.GetBytes(Text.Substring(i, 1))[0]);
									//Asc(Text.Substring(i, 1));
									bool isSpecial = false;
									//	not (A - Z || a - z)
									if (!((valASC >= 65 && valASC <= 90) || (valASC >= 97 && valASC <= 122)))
									{
										isSpecial = true;
									}
									if (isSpecial && AllowSpecialCharacters)
									{
										NewValue += (CapitalCharacters ? Text.Substring(i, 1).ToUpper() : Text.Substring(i, 1));

									}
									else
									{
										if (AllowCharacters)
										{
											NewValue += (CapitalCharacters ? Text.Substring(i, 1).ToUpper() : Text.Substring(i, 1));
										}
									}
								}
							}
							if (AllUpperCase)
							{
								NewValue = NewValue.ToUpper();
							}

							if (AllLowerCase)
							{
								NewValue = NewValue.ToLower();
							}
						}
						else
						{
							if (Mask != "")
							{
								for (int i = 0; i < Mask.Length; i++)
								{
									if (ValCount >= Text.Length)
									{
										break;
									}

									if (Mask.Substring(i, 1) == "9")
									{
										if (Numbers.IndexOf(Text.Substring(ValCount, 1)) != -1)
										{
											NewValue += Text.Substring(ValCount, 1);
										}
										else
										{
											i--;
										}
										ValCount++;
									}
									else
									{
										if (Mask.Substring(i, 1) == "A")
										{
											if (Numbers.IndexOf(Text.Substring(ValCount, 1)) == -1)
											{
												NewValue += Text.Substring(ValCount, 1);
											}
											else
											{
												i--;
											}
											ValCount++;
										}
										else
										{
											if (Mask.Substring(i, 1) == "N")
											{
												int StrAsc = Convert.ToByte(Convert.ToUInt32((Text.Substring(ValCount, 1))));
												if (Numbers.IndexOf(Text.Substring(ValCount, 1)) != -1 || ((StrAsc >= 97 && StrAsc <= 122) || (StrAsc >= 65 && StrAsc <= 90)))
												{
													NewValue += Text.Substring(ValCount, 1);
												}
												else
												{
													i--;
												}
												ValCount++;
											}
											else
											{
												if (Mask.Substring(i, 1) == "!")
												{
													NewValue += Text.Substring(ValCount, 1).ToUpper();
													ValCount++;
												}
												else
												{
													if (Mask.Substring(i, 1) == "#")
													{
														if (Numbers.IndexOf(Text.Substring(ValCount, 1)) != -1 || Text.Substring(ValCount, 1) == " ")
														{
															NewValue += Text.Substring(ValCount, 1);
															ValCount++;
														}
													}
													else
													{
														if (Mask.Substring(i, 1) == Text.Substring(ValCount, 1))
														{
															NewValue += Text.Substring(ValCount, 1);
															ValCount++;
														}
														else
														{
															if (Mask.Substring(i, 1) != ".")
															{
																NewValue += Mask.Substring(i, 1);
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}

						RetVal = NewValue;
					}
					else if (FieldType == "DATE" || FieldType == "DATETIME")
					{
						try 
						{	        
							RetVal =Convert.ToDateTime(Text).ToString(Mask);
						}
						catch (Exception)
						{
							RetVal = Text;
						}
					}
				}
				else
				{
					RetVal = Text;
				}

			}
			catch (Exception ex)
			{
			}
			return RetVal;
		}
	}
}
