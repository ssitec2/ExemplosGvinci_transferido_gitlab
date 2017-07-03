using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GAdapter;
using COMPONENTS;
using PROJETO;

namespace PROJETO
{

	public partial class _Status : System.Web.UI.Page
	{
        public bool SilentMode
		{
			get
			{
				return (Session["SilentMode"] != null ? Session["SilentMode"].ToString().ToLower() : "false") == "true";
			}
			set
			{
				HttpContext.Current.Session["SilentMode"] = value;
			}
		}
		public bool HasErro
		{
			get
			{
				return (Session["HasErro"] != null ? Session["HasErro"].ToString().ToLower() : "false") == "true";
			}
			set
			{
				HttpContext.Current.Session["HasErro"] = value;
			}
		}
	
		public bool Finish
		{
			get
			{
				return (Session["Finish"] != null ? Session["Finish"].ToString().ToLower() : "false") == "true";
			}
			set
			{
				HttpContext.Current.Session["Finish"] = value;
			}
		}
	
		public bool FinishAdp
		{
			get
			{
				return (Session["FinishAdp"] != null ? Session["FinishAdp"].ToString().ToLower() : "false") == "true";
			}
			set
			{
				HttpContext.Current.Session["FinishAdp"] = value;
			}
		}

        public GAdapter.Util.DbConnectionInfo ConnectionInfoDB
        {
            get
            {
                return (GAdapter.Util.DbConnectionInfo)HttpContext.Current.Session["ConnectionStringDB"];
            }
        }

        public GAdapter.Util.DbConnectionInfo ConnectionInfoDBtemp
        {
            get
            {
                return (GAdapter.Util.DbConnectionInfo)HttpContext.Current.Session["ConnectionInfoDBtemp"];
            }
        }

		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				if (ConnectionInfoDB.ServerType != Util.DatabaseType.SQL)
				{
					btnBackup.Visible = false;
				}

				string Arguments = Request.Params.Get("__EVENTARGUMENT");
				if (!IsPostBack)
				{
	
					Session.Remove("Diffs");
					Session.Remove("script");
					Session.Remove("Warning");
	
					this.txtDiffs.ForeColor = System.Drawing.Color.Blue;
					this.LblDiffs.Text = "Status";
					 this.LbDataBase.Text = HttpContext.Current.Session["DatabaseName"].ToString();
	
					if (!FinishAdp)
					{
						HttpContext.Current.Session["FinishAdp"] = true;
					}
					else if (!HasErro)
					{
						if (SilentMode)
						{
							Utility.SetAppConfig(Session["DataBase"].ToString(), "RunAdapter", "false");
							Response.Redirect("Default.aspx?SilentMode=true");
						}
						else
						{
							this.txtDiffs.Text = "Execução do Script.... Sucesso \r\nVerificação do banco.... Sucesso \r\nNão existe diferença entre o banco e o script.";
							Session.Abandon();
							Response.Redirect(@"..\..\Pages\StartPage.aspx");
						}
						if (!Finish)
						{
							HiddenCanDeleteTemp.Value = "true";
						}
					}
					else
					{
						this.txtDiffs.ForeColor = System.Drawing.Color.Red;
						this.LblDiffs.Text = "Erro ao Executar o Script";
					}
				}
	
				if (Arguments == "ClickAdap")
				{
					CheckAdapter();
				}
	
				if (Arguments == "DropDb")
				{
					ReadyAdaptation ReadyAdap = new ReadyAdaptation();
					ReadyAdap.DropDbTmp();
					this.btn_exec.Enabled = false;
					Finish = true;
					HiddenCanDeleteTemp.Value = "false";
				}
			}
			catch
			{
			}
		}
	
		protected override void OnInit(EventArgs e)
		{
			txtDiffs.Text = (Session["Diffs"] != null ? Session["Diffs"].ToString() : "");
			txtScript.Text = (Session["script"] != null ? Session["script"].ToString() : "");
			TxtWarning.Text = (Session["Warning"] != null ? Session["Warning"].ToString() : "");
		}
	
		private void CheckAdapter()
		{
			ReadyAdaptation ReadyAdap = new ReadyAdaptation();
            ReadyAdap.CheckAdaptation(txtScript.Text.ToString(), GAdapter.Util.GetConnectionString(ConnectionInfoDB, true), GAdapter.Util.GetConnectionString(ConnectionInfoDBtemp, true));
		}
	
		protected void btnBackup_Click(object sender, EventArgs e)
		{
			ReadyAdaptation ReadyAdap = new ReadyAdaptation();
			ReadyAdap.Init();
	
			if (ReadyAdap.Createbackup())
			{
				this.txtDiffs.ForeColor = System.Drawing.Color.Red;
				this.txtDiffs.Text = ReadyAdap.GetErro();
				this.LblDiffs.Text = "Erro ao criação do backup";
			}
			else
			{
				this.txtDiffs.ForeColor = System.Drawing.Color.Blue;
				this.txtDiffs.Text = "Backup criado com sucesso";
			}
		}
	
		protected void btnRestore_Click(object sender, EventArgs e)
		{
			ReadyAdaptation ReadyAdap = new ReadyAdaptation();
			ReadyAdap.Init();
	
			if (ReadyAdap.Restorebackup())
			{
				this.txtDiffs.ForeColor = System.Drawing.Color.Red;
				this.txtDiffs.Text = ReadyAdap.GetErro();
				this.LblDiffs.Text = "Erro ao restaurar o backup";
			}
			else
			{
				this.txtDiffs.ForeColor = System.Drawing.Color.Blue;
				this.txtDiffs.Text = "Backup restaurado com sucesso";
			}
		}
	}
}
