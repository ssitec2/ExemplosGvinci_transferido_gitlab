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
	public partial class AboutPage:Page
	{
		

		protected override void OnLoad(EventArgs e)
		{
			try
			{
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
		}

		private void InitializePageContent()
		{
			ProjectVersion.Text = EnvironmentVariable.ProjectVersion;
			CompanyName.Text = EnvironmentVariable.CompanyName;
			DeveloperName.Text = EnvironmentVariable.DeveloperName;
			ProjectCopyright.Text = EnvironmentVariable.ProjectCopyright;
			ShowFormulas();
		}

		protected override void InitializeCulture()
		{
			Utility.SetThreadCulture();  
		}


	}

}
