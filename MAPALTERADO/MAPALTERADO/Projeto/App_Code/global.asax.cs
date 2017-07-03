using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Web.UI;
using COMPONENTS;
namespace PROJETO
{

	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(Object sender, EventArgs e)
		{
			LoadApplicationSettings();			
		}

		protected string GetFileHash(string fileName)
		{
			FileStream file = new FileStream(fileName, FileMode.Open);
			System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] retVal = md5.ComputeHash(file);
			file.Close();

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0; i < retVal.Length; i++)
			{
				sb.Append(retVal[i].ToString("x2"));
			}
			return sb.ToString();
		}

		private void LoadApplicationSettings()
		{
			string ConfigFile = Server.MapPath("~/App_Data/App.Config");
			string CurrentHash = GetFileHash(ConfigFile);
			
			// não vamos recarregar as configurações...
			if (Application["ConfigFileHash"] != null && Application["ConfigFileHash"].Equals(CurrentHash)) return;

			Application["Databases"] = new Databases(ConfigFile);
			Application["_locales"] = System.Configuration.ConfigurationManager.GetSection("locales");
			HttpContext.Current.Cache.Insert("__InvalidateAllPages", DateTime.Now, null,
											System.DateTime.MaxValue, System.TimeSpan.Zero,
											System.Web.Caching.CacheItemPriority.NotRemovable,
											null);
			Application["culture"] = Utility.siteLanguage;
			bool NeedToCreateDB = false;
			bool NeedToAdapter = false;
			foreach (DatabaseInfo vgDbi in ((Databases)Application["Databases"]).DataBaseList.Values)
			{
				if ((vgDbi.CheckDatabase == null || vgDbi.CheckDatabase == true) || (vgDbi.StringConnection == null || vgDbi.StringConnection == ""))
				{
					NeedToCreateDB = true;
				}
				else
				{
					if (vgDbi.RunAdapter)
					{
						NeedToAdapter = true;						
					}
				}
			}
			if (NeedToCreateDB)
			{
				Application["PageStart"] = "1";
			}
			else if (NeedToAdapter)
			{
				Application["PageStart"] = "2";
			}
			else
			{
				Application.Remove("PageStart");
			}
		}
		
		protected void Session_Start(Object sender, EventArgs e)
		{
			LoadApplicationSettings();
			if (Application["PageStart"] != null)
			{
				if (Application["PageStart"] == "1")
				{
					System.Web.HttpContext.Current.Response.Redirect("~/Pages/ConfigDB.aspx");
				}
				else if (Application["PageStart"] == "2")
				{
					System.Web.HttpContext.Current.Response.Redirect("~/Gadapter/Pages/Default.aspx?SilentMode=true");
				}
				Application.Remove("PageStart");
			}
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			if (Application[Request.PhysicalPath] != null)
				Request.ContentEncoding = System.Text.Encoding.GetEncoding(Application[Request.PhysicalPath].ToString());
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{
		}

		protected void Session_End(Object sender, EventArgs e)
		{
		}

		protected void Application_End(Object sender, EventArgs e)
		{
		}

		void Application_Error(object sender, EventArgs e)
		{
			if (Context != null)
			{
			    HttpException CurrentException = Server.GetLastError() as HttpException;
			    if (CurrentException != null)
			    {
                    int ErrorCode = 0;
                    string ErrorMessage = "";
                    if ((CurrentException).InnerException != null)
                    {
                        ErrorMessage = (CurrentException).InnerException.Message;
                    }
                    else 
                    {
                        ErrorCode = CurrentException.GetHttpCode();
                        ErrorMessage = CurrentException.Message;
                    }
					Server.ClearError();
					if(!Response.IsRequestBeingRedirected)
						Response.Redirect("~/Pages/BlankPage.aspx?errorCode=" + ErrorCode + "&errorMessage=" + ErrorMessage);
			    }
			}
		}

	}

}
