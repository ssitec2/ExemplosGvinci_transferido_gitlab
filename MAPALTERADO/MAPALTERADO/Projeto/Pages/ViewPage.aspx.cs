using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using COMPONENTS.Data;
using COMPONENTS;
using System.Data;
using COMPONENTS.Configuration;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace PROJETO
{
	public partial class ViewPage : System.Web.UI.Page
	{
		protected override void InitializeCulture()
		{
			Utility.SetThreadCulture();
		}
		protected override void OnInit(EventArgs e)
		{
			Utility.CheckAuthentication(this,true);
			if (!IsPostBack)
			{
			}
			base.OnInit(e);
		}
		
		protected void grdView_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
		{
			int TotalRecords = 0;

			grdView.DataSource = GetDataGrid(grdView.CurrentPageIndex, grdView.PageSize, out TotalRecords);
			grdView.VirtualItemCount = TotalRecords;
		}

		private DataSet GetDataGrid(int CurrentPageIndex, int PageSize, out int TotalRecords)
		{
			string ViewName = HttpContext.Current.Request["page"].ToString();
			ViewSettings Sql = (ViewSettings)Deserialize(Server.MapPath("../Views/" + ViewName));
			DataAccessObject Dao = Settings.GetDataAccessObject(((COMPONENTS.Databases)HttpContext.Current.Application["Databases"])[Sql.DataBase]);

			DataCommand Select = new TableCommand(Sql.GenerateSqlQuery(), new string[0], Dao);
			DataCommand Count = new TableCommand("SELECT COUNT(*) FROM (" + Sql.GenerateSqlQuery() + ") t", new string[] { }, Dao);
			TotalRecords = (int)Count.ExecuteScalar();

			if (CurrentPageIndex == -1)
			{
				CurrentPageIndex = (int)Math.Ceiling((decimal)TotalRecords / (decimal)PageSize) - 1;
			}

			if (TotalRecords > 0)
			{
				return Select.Execute(CurrentPageIndex * PageSize, PageSize);
			}
			return Select.Execute();
		}
		
		public static object Deserialize(string file)
		{
			if (!File.Exists(file)) return null;
			using (StreamReader reader = File.OpenText(file))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ViewSettings));
				return xmlSerializer.Deserialize(reader);
			}
		}
		protected override void OnLoadComplete(EventArgs e)
		{
			base.OnLoadComplete(e);
		}


	}
	
}
