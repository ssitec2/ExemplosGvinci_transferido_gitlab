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
	public partial class ErrorPage:Page
	{
		
		string ErrorCode;
		string ErrorMessage;

		protected override void OnLoad(EventArgs e)
		{
			try
			{
				if(Session["ErrorCode"] != null)
					ErrorCode = Session["errorCode"].ToString();
				if (Session["errorMessage"] != null)
					ErrorMessage = Session["errorMessage"].ToString();

				AjaxPanel1.ResponseScripts.Add("setTimeout(\"resizeIframe();\",100);");

				Utility.CheckAuthentication(this,true);
				InitializePageContent();
				Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this));
			}
			catch (Exception ex)
			{
			}
		}


		public void ShowFormulas()
		{
			Label4.Text = Label4.Text.Replace("<", "&lt;");
			Label4.Text = Label4.Text.Replace(">", "&gt;");
			Label2.Text = Label2.Text.Replace("<", "&lt;");
			Label2.Text = Label2.Text.Replace(">", "&gt;");
			Label3.Text = Label3.Text.Replace("<", "&lt;");
			Label3.Text = Label3.Text.Replace(">", "&gt;");
			labHttpErrorCode.Text = ErrorCode;
			labHttpErrorMessage.Text = ErrorMessage;
			Label1.Text = Label1.Text.Replace("<", "&lt;");
			Label1.Text = Label1.Text.Replace(">", "&gt;");
		}

		private void InitializePageContent()
		{
			ShowFormulas();
		}

		protected override void InitializeCulture()
		{
			Utility.SetThreadCulture();  
		}


	}

}
