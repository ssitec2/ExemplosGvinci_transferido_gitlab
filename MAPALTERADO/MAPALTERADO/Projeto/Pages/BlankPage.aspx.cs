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
using PROJETO;

namespace PROJETO.Pages
{

	public partial class BlankPage : Page
	{
		protected override void OnLoadComplete(EventArgs e)
		{
			if (Request["errorCode"] != null && Request["errorMessage"] != null)
			{
				Session["errorCode"] = Request["errorCode"];
				Session["errorMessage"] = Request["errorMessage"];
				Response.Redirect("../Pages/ErrorPage.aspx");
			}
			base.OnLoadComplete(e);
			
		}
	}
}
