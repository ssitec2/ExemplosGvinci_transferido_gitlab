using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Collections.Specialized;
using PROJETO;
using COMPONENTS;
using COMPONENTS.Data;
using COMPONENTS.Configuration;
using COMPONENTS.Security;
using Telerik.Web.UI;


namespace PROJETO
{
	public partial class ConfigDB : System.Web.UI.Page
	{
		public bool AutenticacaoWindowsField
		{
			get
			{
				return (bool)chkAutent.Checked;
			}
		}
		
		string ConnectionStringServer = "";
		NameValueCollection erros = new NameValueCollection();
		public int DatabaseCount
		{
			get
			{
				if (ViewState["DatabaseCount"] != null)
					return (int)ViewState["DatabaseCount"];

				return 0;
			}
			set
			{
				ViewState["DatabaseCount"] = value;
			}
		}
		protected override void OnInit(EventArgs e)
		{
			if (!IsPostBack)
			{
				chkAutent_OnCheckedChanged(null, null);
			}
			base.OnInit(e);
		}
		
		protected override void InitializeCulture()
		{
			Utility.SetThreadCulture();
		}
		
		public string DatabaseName
		{
			get
			{
				if (ViewState["DatabaseName"] != null)
					return ViewState["DatabaseName"].ToString();

				return "";
			}
			set
			{
				ViewState["DatabaseName"] = value;
			}
		}

		public string DatabaseAlias
		{
			get
			{
				if (ViewState["DatabaseAlias"] != null)
					return ViewState["DatabaseAlias"].ToString();

				return "";
			}
			set
			{
				ViewState["DatabaseAlias"] = value;
			}
		}
		
		/// <summary>	
		/// Page load,
		/// fica tudo o que deve ser feito ao inciar a aplicaçao
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				int i = 0;
				foreach (DatabaseInfo vgDbi in ((Databases)Application["Databases"]).DataBaseList.Values)
				{
					i++;
					if ((vgDbi.CheckDatabase == null || vgDbi.CheckDatabase == true)  || (vgDbi.StringConnection == null || vgDbi.StringConnection == ""))
					{
						DatabaseName = vgDbi.Name;
						DatabaseAlias = vgDbi.DataBaseAlias;
						txtUser.Text = vgDbi.User;
                        txtPassword.Attributes.Add("DefaultPassword", vgDbi.Password);
                        txtServer.Text = vgDbi.ServerName;
                        chkAutent.Checked = vgDbi.WinAut;
						DatabaseCount = i;
						RadComboBoxItem gcboType = new RadComboBoxItem(vgDbi.Type);
						cboDataType.Items.Add(gcboType);
						break;
					}
				}
			
				RadComboBoxItem gcboODBC = new RadComboBoxItem("ODBC", "ODBC");
				cboDataType.Items.Add(gcboODBC);
				RadComboBoxItem gcboOleDb = new RadComboBoxItem("OleDb", "OleDb");
				cboDataType.Items.Add(gcboOleDb);
				RadComboBoxItem gcboLocaldb = new RadComboBoxItem("Localdb", "Localdb");
				cboDataType.Items.Add(gcboLocaldb);

				txtDataBase.Text = DatabaseName;
			}
		}

		public void CreateDB()
		{
			if (txtProvider.Text == string.Empty && txtProvider.Visible == true  && cboDataType.Text.ToUpper() == "OLEDB")
				erros.Add("Provider Vazio", "Preencher campo \"Provider\"");
			if (txtServer.Text == string.Empty && txtServer.Visible == true)
				erros.Add("Server name Vazio", "Preencher campo \"Server name\"");
			if (chkAutent.Checked == false)
			{
				if (txtUser.Text == string.Empty && txtUser.Visible == true)
					erros.Add("User Vazio", "Preencher campo \"User\"");
				if (txtPassword.Text == string.Empty && txtPassword.Visible == true)
					erros.Add("Password Vazio", "Preencher campo \"Password\"");
			}
			if (txtDataBase.Text == string.Empty && txtDataBase.Visible == true)
				erros.Add("Nome do banco Vazio", "Preencher campo \"Nome do banco\"");
		
			if (cboDataType.Text == string.Empty && cboDataType.Visible == true)
				erros.Add("Provider Vazio", "Selecione o Driver");
		
			if (erros.Count > 0)
			{
	            ShowErrors();
				return;
			}
			switch (cboDataType.Text.ToUpper())
			{
				case "SQL":
					isSqlServer();
					break;
				case "LOCALDB":
					isLocaldbServer();
					break;
				case "ODBC":
					break;
				case "OLEDB":
					break;
			}
			if (erros.Count == 0)
			{
				int i = 0;
				foreach (DatabaseInfo vgDbi in ((Databases)Application["Databases"]).DataBaseList.Values)
				{
					i++;
					if (vgDbi.StringConnection == null || vgDbi.StringConnection == "" && vgDbi.Name != DatabaseName)
					{
						DatabaseName = vgDbi.Name;
						DatabaseAlias = vgDbi.DataBaseAlias;
						DatabaseCount = i;
						txtDataBase.Text = DatabaseName;
						txtUser.Text = "";
						txtPassword.Text = "";
						txtServer.Text = "";
						txtProvider.Text = "";
						cboDataType.Items.Clear();
						RadComboBoxItem gcboType = new RadComboBoxItem(vgDbi.Type);
						cboDataType.Items.Add(gcboType);
						RadComboBoxItem gcboODBC = new RadComboBoxItem("ODBC", "ODBC");
						cboDataType.Items.Add(gcboODBC);
						RadComboBoxItem gcboOleDb = new RadComboBoxItem("OleDb", "OleDb");
						cboDataType.Items.Add(gcboOleDb);
						RadComboBoxItem gcboLocaldb = new RadComboBoxItem("gcboLocaldb", "gcboLocaldb");
						cboDataType.Items.Add(gcboLocaldb);
						cboDataType.SelectedIndex = 0;

						break;
					}
				}
				Session.Abandon();
				System.Web.HttpContext.Current.Response.Redirect("StartPage.aspx");
			}
		}
		

		
		/// <summary>
		/// Se for sql ele entra aqui para criar o banco  e as tabelas quando preciso
		/// </summary>
		public void isLocaldbServer()
		{
			bool InTransaction = false;
			//OleDbDao access = new OleDbDao(new ConnectionString());

			SqlDao access = new SqlDao(new ConnectionString());

			try
			{
				if (chkAutent.Checked == true)
				{
					ConnectionStringServer = "Data Source=" + txtServer.Text +";Integrated Security=True";
				}
				else
				{
					ConnectionStringServer = "Data Source=" + txtServer.Text + ";User Id=" + txtUser.Text + ";Password=" + txtPassword.Text;
				}
				try
				{
					access.ConnectionString = ConnectionStringServer;
					access.OpenConnection();
				}
				catch (Exception e)
				{
					throw new Exception("Dados de servidor invalidos. " + e.Message);
				}

				string vgStr = access.ExecuteScript(Server.MapPath("../Databases/" + DatabaseAlias + ".sql"), txtDataBase.Text, Server.MapPath("../App_Data/"));

				Utility.SetAppConfig(DatabaseAlias.ToString(), "StringConnection", ConnectionStringServer + String.Format(";AttachDbFilename=|DataDirectory|\\{0}.mdf;DataBase={0};",DatabaseName));
 
				Utility.SetAppConfig(DatabaseAlias.ToString(),"Type", cboDataType.Text);
				if (!access.DatabaseAlreadyExisted) { Utility.SetAppConfig(DatabaseAlias.ToString(), "RunAdapter", "false"); }
				WriteDatabaseInfo();
				if (InTransaction) access.CommitTrans();
			}
			catch (Exception ex)
			{
				if (InTransaction) access.RollBack();
				erros.Add("Erro ao iniciar a aplicação", ex.Message);
				ShowErrors();
			}
			finally
			{
				Utility.SetAppConfig(DatabaseAlias.ToString(), "CheckDatabase", "false");
				access.CloseConnection();
			}
		}

		/// <summary>
		/// Se for sql ele entra aqui para criar o banco  e as tabelas quando preciso
		/// </summary>
		public void isSqlServer()
		{
			bool InTransaction = false;
			OleDbDao access = new OleDbDao(new ConnectionString());
			try
			{       
				string vgProv = "";
				if (cboDataType.Text.ToUpper() != "OLEDB")
				{
 				vgProv = "SQLOLEDB.1";
				}
				else
				{
					vgProv = txtProvider.Text;
				}
				if (chkAutent.Checked == true)
				{
					ConnectionStringServer = "Provider=" + vgProv + ";Server=" + txtServer.Text + ";DataBase=;Integrated Security=SSPI";
				}
				else
				{
					ConnectionStringServer = "Provider=" + vgProv + ";Server=" + txtServer.Text + ";DataBase=;User ID=" + txtUser.Text + ";Password= " + txtPassword.Text;
				}
				try
				{
					access.ConnectionString = ConnectionStringServer;
					access.OpenConnection();
				}
				catch (Exception e)
				{
					throw new Exception("Dados de servidor invalidos. " + e.Message);
				}
				
                if (access.Transaction == null)
                {
                    access.BeginTrans();
                    InTransaction = true;
                }	
				string vgStr = access.ExecuteScript(Server.MapPath("../Databases/" + DatabaseAlias + ".sql"), txtDataBase.Text);
				if (cboDataType.Text.ToUpper() != "OLEDB")
				{
					if (chkAutent.Checked == true)
					{
						Utility.SetAppConfig(DatabaseAlias.ToString(),"StringConnection", vgStr.Substring(vgStr.IndexOf(";") + 1).Replace("Persist Security Info=False;Initial Catalog", "Database"));
					}
					else
					{
						Utility.SetAppConfig(DatabaseAlias.ToString(),"StringConnection", vgStr.Substring(vgStr.IndexOf(";") + 1).Replace("Persist Security Info=False;Initial Catalog", "Database") + "Password=" + txtPassword.Text + ";Trusted_Connection=false");
					}
				}
				else
				{
					Utility.SetAppConfig(DatabaseAlias.ToString(),"StringConnection", vgStr + "Password=" + txtPassword.Text + ";");
				}

				Utility.SetAppConfig(DatabaseAlias.ToString(),"Type", cboDataType.Text);
				if (!access.DatabaseAlreadyExisted) { Utility.SetAppConfig(DatabaseAlias.ToString(), "RunAdapter", "false"); }
				WriteDatabaseInfo();
				if (InTransaction) access.CommitTrans();
			}
			catch (Exception ex)
			{
				if (InTransaction)  access.RollBack();
				erros.Add("Erro ao iniciar a aplicação",ex.Message);
	            ShowErrors();
			}
			finally
			{	
				Utility.SetAppConfig(DatabaseAlias.ToString(), "CheckDatabase", "false");		
				access.CloseConnection();
			}
		}

		/// <summary>
		/// Mostra os erros que aconteceram durante a criaçao do banco ou ao tentar colocar a string de conexao no webconfig
		/// </summary>
		protected void ShowErrors()
		{
			string DefaultMessage = "";
			for (int i = 0; i < erros.Count; i++)
				switch (erros.AllKeys[i])
				{
					default:
						if (DefaultMessage != "") DefaultMessage += "<br>";
						DefaultMessage += erros[i];
						break;
				}
			labError.Visible = true;
			labError.ForeColor = System.Drawing.Color.Red;
			labError.Text = DefaultMessage;
		}

		protected void WriteDatabaseInfo()
		{
			Utility.SetAppConfig(DatabaseAlias.ToString(), "User", txtUser.Text);
			Utility.SetAppConfig(DatabaseAlias.ToString(), "Password", txtPassword.Text);
			Utility.SetAppConfig(DatabaseAlias.ToString(), "ServerName", txtServer.Text);
			Utility.SetAppConfig(DatabaseAlias.ToString(), "WinAut", chkAutent.Checked.ToString());
			Utility.SetAppConfig(DatabaseAlias.ToString(), "DataBaseName", txtDataBase.Text.ToString());

		}

		/// <summary>
		/// se for oledp ele abilita o campo da provider
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void cboDataType_SelectedIndexChanged(object sender, RadComboBoxItemsRequestedEventArgs e)
		{
			if (cboDataType.Text.ToUpper() == "OLEDB")
			{
				txtProvider.Visible = true;
			}
		}

		protected void chkAutent_OnCheckedChanged(object sender, EventArgs e)
		{
			if (chkAutent.Checked == true)
			{
				txtUser.Enabled = false;
				txtPassword.Enabled = false;
				labPassword.Enabled = false;
				labUser.Enabled = false;
			}
			else
			{
				txtUser.Enabled = true;
				txtPassword.Enabled = true;
				labPassword.Enabled = true;
				labUser.Enabled = true;
			}
		}

		protected void ___chkAutent_OnCheckedChanged(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				chkAutent_OnCheckedChanged(sender, e);
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butCreate_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				CreateDB();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

	}
}
