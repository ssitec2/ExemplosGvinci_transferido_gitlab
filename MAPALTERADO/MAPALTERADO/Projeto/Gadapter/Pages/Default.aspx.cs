using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using COMPONENTS;
using GAdapter;
using System.IO;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Xml;
using PROJETO;
using Telerik.Web.UI;

namespace PROJETO
{
	public partial class _Default : System.Web.UI.Page
	{
	
		public List<string> _ErroList = new List<string>();
		private string NewDatabase 
		{
			get
			{
				return Session["DataBaseAdapterName"].ToString();	
			}
		}
        GAdapter.Util.DatabaseType DatabaseType = GAdapter.Util.DatabaseType.SQL;
		public TableStruct AdaptingTable;
		private bool SilentMode { get; set; }
		private bool _WinAut { get; set; }
        private GAdapter.Util.DbConnectionInfo ConnectionStringDB;
        private GAdapter.Util.DbConnectionInfo ConnectionStringDBTemp;
        private string FileName = "";
	
		protected void Page_Load(object sender, EventArgs e)
		{
			HtmlLink link = new HtmlLink();
			link.Attributes.Add("rel", "stylesheet");
			link.Attributes.Add("type", "text/css");
			link.Attributes.Add("href", "Styles/Gadapta.css");
			this.Page.Header.Controls.Add(link);
	
			SilentMode = (Request.QueryString["SilentMode"] != null && Request.QueryString["SilentMode"].ToString().ToUpper() == "TRUE");
			Session["SilentMode"] = SilentMode;
			LoadConfig(SilentMode);
		}
	
		private void LoadConfig(bool SilentMode)
		{
			if (SilentMode)
			{
				bool Erro = false;
				foreach (DatabaseInfo Conf in ((Databases)Application["Databases"]).DataBaseList.Values)
				{
					if (Conf.RunAdapter)
					{
						Session["DataBase"] = Conf.DataBaseAlias;
						Session["DataBaseAdapterName"] = Conf.DataBaseAdapterName;
						_WinAut = Conf.WinAut;
						FileName = Conf.DataBaseAlias;
						HttpContext.Current.Session["DatabaseName"] = Conf.DataBaseAlias;
						DatabaseType = (Conf.Type.ToUpper() == "SQL" || Conf.Type.ToUpper() == "LOCALDB") ? GAdapter.Util.DatabaseType.SQL : GAdapter.Util.DatabaseType.MYSQL;
						MakeConnString(Conf.ServerName, Conf.User, Conf.Password, false, Conf.WinAut, Conf.Name,  DatabaseType);
						Erro = RunSilentAdapter();
					}
				}
				if (!Erro)
				{
					Session.Abandon();
					Response.Redirect(@"../../Pages/StartPage.aspx");
				}
	
			}
			else
			{
                if(RbMySQL.Selected)
                {
                    DatabaseType = GAdapter.Util.DatabaseType.MYSQL;
                }
				HttpContext.Current.Session["DatabaseName"] = txtDatabase.Text;
				HttpContext.Current.Session["DataBaseAdapterName"] = txtDatabase.Text + "_TEMP";
                MakeConnString(txtServer.Text, txtUser.Text, txtPassword.Text, false, cbxWinAut.Checked, txtDatabase.Text, DatabaseType);
				FileName = txtFileName.Text;
				if (cbxTempDB.Checked)
				{
                    MakeConnString(txtServerTemp.Text, txtUserServerTemp.Text, txtPasswordServerTemp.Text, true, cbxWinAutTmp.Checked, NewDatabase, DatabaseType);
				}
			}
		}

        private void MakeConnString(string Server, string User, string Pwd, bool IsTempServer, bool WinAut, string DbName, GAdapter.Util.DatabaseType ServerType)
		{

            if (IsTempServer)
            {
                ConnectionStringDBTemp.Server = Server;
                ConnectionStringDBTemp.UserName = User;
                ConnectionStringDBTemp.UserPassword = Pwd;
                ConnectionStringDBTemp.TrustedConnection = WinAut;
                ConnectionStringDBTemp.DbName = NewDatabase;
                ConnectionStringDBTemp.ServerType = ServerType;
                
            }
            else
            {
                ConnectionStringDB.Server = Server;
                ConnectionStringDB.UserName = User;
                ConnectionStringDB.UserPassword = Pwd;
                ConnectionStringDB.TrustedConnection = WinAut;
                ConnectionStringDB.DbName = DbName;
                ConnectionStringDB.ServerType = ServerType;
            }
		}

		protected void AjaxRequest(object source, Telerik.Web.UI.AjaxRequestEventArgs e)
		{
			string Arguments = Request.Params.Get("__EVENTARGUMENT");
		}
	
		protected bool RunSilentAdapter()
		{
	
			string Error = "";
            string ScriptFile = Server.MapPath(@"..\..\Databases\" + FileName + ".sql");
			
			if (!cbxTempDB.Checked)
			{
				ConnectionStringDBTemp = ConnectionStringDB;
			}
			
			if (System.IO.File.Exists(ScriptFile))
			{
                ConnectionStringDBTemp.DbName = NewDatabase;
                ReadyAdaptation ReadyAdap = new ReadyAdaptation(ConnectionStringDB, ConnectionStringDBTemp);
				List<string> SQLScript = ReadyAdap.LoadScriptSQL(ScriptFile);
	
				ReadyAdap.Init();
				ReadyAdap.DBCreateByScript(GAdapter.Util.GetConnectionString(ConnectionStringDBTemp), SQLScript);
	
				if (!ReadyAdap.ExecErro && !ReadyAdap.Run(ref Error)) // Cria o Script e roda a adaptação
				{
					if (SilentMode)
					{
                        ReadyAdap.CleanDatabase(GAdapter.Util.GetConnectionString(ConnectionStringDBTemp, true));
                        Utility.SetAppConfig(Session["DataBase"].ToString(), "RunAdapter", "false");
						Response.Redirect(@"Default.aspx?SilentMode=true");
					}
					else
					{
						txtInformation.Text = "Não existe diferenças entre os bancos.";
						txtInformation.ForeColor = System.Drawing.Color.Blue;
                        ReadyAdap.CleanDatabase(GAdapter.Util.GetConnectionString(ConnectionStringDBTemp, true));
					}
				}
				else
				{
					txtInformation.Text = Error + ReadyAdap.GetErro();
					txtInformation.ForeColor = System.Drawing.Color.Red;
				}
				return ReadyAdap.ExecErro;
	
			}
			else
			{
				txtInformation.Text = "Script do banco de dados não foi encontrado em \r\n" + ScriptFile;
				txtInformation.ForeColor = System.Drawing.Color.Red;
			}
			return true;
		}
	
		protected void btn_RunAdapter_Click(object sender, EventArgs e)
		{
			RunSilentAdapter();
		}
	}
}
