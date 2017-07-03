using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Web;
using System.IO;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Globalization;
using COMPONENTS;

namespace PROJETO
{
	public partial class StartPage:Page
	{
		

		protected override void OnLoad(EventArgs e)
		{
			try
			{
				ajxMainAjaxPanel.ResponseScripts.Add("setTimeout(\"resizeIframe();\",100);");

				InitializePageContent();
				Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this));
			}
			catch (Exception ex)
			{
			}
		}


		public void ShowFormulas()
		{
			Label2.Text = Label2.Text.Replace("<", "&lt;");
			Label2.Text = Label2.Text.Replace(">", "&gt;");
			Label1.Text = Label1.Text.Replace("<", "&lt;");
			Label1.Text = Label1.Text.Replace(">", "&gt;");
		}

		private void InitializePageContent()
		{
			ShowFormulas();
		}


		public bool DoLogin()
		{
			string LoginError = "";
			bool RetVal = Utility.DoLogin(txtLoginUser.Text , txtLoginPassword.Text , this, ref LoginError, ajxMainAjaxPanel);
			if(!RetVal)
			{
				labError.Text = LoginError;
			}
			else
			{
				labError.Text = "";
			}
			InitializePageContent();
			return RetVal;
		}
			protected void ___butDoLogin_OnClick(object sender, EventArgs e)
			{
				bool ActionSucceeded_1 = true;
				try
				{
					ActionSucceeded_1 = DoLogin();
				}
				catch (Exception ex)
				{
					ActionSucceeded_1 = false;
				}
			}

		protected override void InitializeCulture()
		{
			Utility.SetThreadCulture();  
		}


	}

}
