using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using PROJETO;

public static class MediaHandlerHelper
{
	public static void ParseFileContent(byte[] FullContent, out int ContentStart, out int ContentLenght, out string FileName)
	{
		FileName = "";
		int Split = 0;

		try
		{
			for (int y = FullContent.Length - 1; y > 0; y--)
			{
				if (FullContent[y].CompareTo((byte)'|') == 0)
				{
					Split = y;
					break;
				}
				else
				{
					char c = (char)FullContent[y];
					if ((int)c < 32 || (int)c > 253)
					{
						Split = 0;
						FileName = "";
						break;
					}
					FileName = c.ToString() + FileName;
				}
				if (FullContent.Length - y > 255)
				{
					FileName = "";
					break;
				}
			}
			if (Split == 0)
			{
				int i = FullContent[0];
				for (ContentStart = 1; ContentStart <= i; ContentStart++)
				{
					if (FullContent[ContentStart] > 200)
					{
						break;
					}
					char c = (char)FullContent[ContentStart];
					FileName += c.ToString();
				}
				FileName = FileName.Trim('\0');
				ContentLenght = FullContent.Length - ContentStart;
			}
			else
			{
				ContentLenght = Split;
				ContentStart = 0;
			}
		}
		catch
		{
			throw;
		}
		finally
		{
			FullContent = null;
		}
	}

	public static void PrepareMediaHandler(Object value, string SessionHandlerObject, bool SaveAsFile, bool EncryptedFile)
	{
		string FileName = "";
		byte[] FullContent = null;

		HttpContext.Current.Session.Remove(SessionHandlerObject);
		HttpContext.Current.Session.Remove(SessionHandlerObject + "FileName");

		try
		{
			if (SaveAsFile)
			{
				if (value == null) return;

                string FilePath = "";
                try
                {
                    FilePath = HttpContext.Current.Server.MapPath(value.ToString());
                }
                catch
                {
                    if (value.ToString().StartsWith("/"))
                    {
                        FilePath = HttpContext.Current.Server.MapPath("~" + value.ToString());
                    }
                }

				//busca arquivo
				if (!File.Exists(FilePath)) return;

				using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					FullContent = new byte[fs.Length];
					fs.Read(FullContent, 0, (int)fs.Length);

					if (EncryptedFile)
						FullContent = Crypt.Decript_AES(FullContent);

					FileName = Path.GetFileName(value.ToString());

					HttpContext.Current.Session[SessionHandlerObject] = Convert.ToBase64String(FullContent);
					HttpContext.Current.Session[SessionHandlerObject + "FileName"] = value.ToString();  // FileName;
				}
			}
			else
			{
				int ContentStart = 0;
				int ContentLenght = 0;

				FullContent = (byte[])value;
				if (FullContent == null) return;

				if (EncryptedFile)
					FullContent = Crypt.Decript_AES(FullContent);

				MediaHandlerHelper.ParseFileContent(FullContent, out ContentStart, out ContentLenght, out FileName);
				HttpContext.Current.Session[SessionHandlerObject] = Convert.ToBase64String(FullContent);
				HttpContext.Current.Session[SessionHandlerObject + "FileName"] = FileName;
			}
		}
		catch
		{
			throw;
		}
		finally
		{
			value = null;
			FullContent = null;
		}
	}
}
