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
using PROJETO;
using PROJETO.AccessControl;
using PROJETO.DataProviders;
using COMPONENTS;
using COMPONENTS.Security;
using COMPONENTS.Configuration;
using COMPONENTS.Data;
using System.Text;
using System.Xml;

namespace PROJETO
{

	public partial class Access : System.Web.UI.Page
	{

		private GMembershipProvider vgMembership = ((GMembershipProvider)Membership.Providers["GMembershipProvider"]);
		private Site SiteConfig = new Site();
		private Hashtable _ErrorList;
		private string vgGrupoLogado;
		public LoginGroupItem CurrentGroup;
		public string ProjectID { get { return "30D047A8"; } }
		protected override void OnLoadComplete(EventArgs e)
		{
			base.OnLoadComplete(e);
		}

		/// <summary>
		/// Lista de erros que devem ser mostrados
		/// </summary>
		public Hashtable ErrorList
		{
			get
			{
				return _ErrorList;
			}
			set
			{
				_ErrorList = value;
			}
		}
		protected override void OnInit(EventArgs e)
		{
			if (!IsPostBack)
			{
			}
			base.OnInit(e);
		}
		
		protected override void InitializeCulture()
		{
			Utility.SetThreadCulture();
		}		

		/// <summary>
		/// DeclaraÃ§ao de variaveis e implementação de estilo na pagina
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			ErrorList = new Hashtable();
			ShowError();
			Utility.CheckAuthentication(this);
			if (!IsPostBack)
			{
				ClearGroupChecks();
				FillGroupsListBox();
				FillGridAccess();
				txtActualPassword.Focus();

				EnableGroupButtons(false, false, true, true);
				EnableUserButtons(false, false, true, true);
				EnablePasswordButtons(true, true);
			}
			vgGrupoLogado = HttpContext.Current.Session["vgGroupID"].ToString();
			CurrentGroup = vgMembership.GetGroupByID(vgGrupoLogado);
			if (CurrentGroup != null)
			{
				if (!(bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
					ListBox5.Enabled = false;
					ListBox6.Enabled = false;
					TextBox7.Enabled = false;
					TextBox11.Enabled = false;
					TextBox8.Enabled = false;
					TextBox9.Enabled = false;
					TextBox10.Enabled = false;
					EnableUserButtons(false, false, false, false);

					ListBox3.Enabled = false;
					TextBox6.Enabled = false;
					CheckBox1.Enabled = false;
					ListBox4.Enabled = false;
					CheckBox5.Enabled = false;
					CheckBox3.Enabled = false;
					CheckBox2.Enabled = false;
					CheckBox4.Enabled = false;

					EnableGroupButtons(false, false, false, false);
				}
			}
			AjaxPanel1.AjaxRequest += new Telerik.Web.UI.RadAjaxControl.AjaxRequestDelegate(AjaxPanel1_AjaxRequest);
		}
		

		/// <summary>
		/// Coloca os textbox checados ou nao de acordo com o grupo e pagina passado no parametro
		/// </summary>
		/// <param name="vgGrupoId">Id do grupo que esta selecionado</param>
		/// <param name="vgPaginaNome">Nome da pagina selecionada</param>
		public void SetGroupChecks(string vgGrupoId, string vgPaginaNome)
		{
			CheckBox2.Checked = vgMembership.TestIfCanSee(ProjectID, vgGrupoId, vgPaginaNome);
			CheckBox3.Checked = vgMembership.TestIfCanEdit(ProjectID, vgGrupoId, vgPaginaNome);
			CheckBox5.Checked = vgMembership.TestIfCanAdd(ProjectID, vgGrupoId, vgPaginaNome);
			CheckBox4.Checked = vgMembership.TestIfCanRemove(ProjectID, vgGrupoId, vgPaginaNome);
		}

		/// <summary>
		/// Limpa todos os campos para fazer um insert ou algo do tipo
		/// </summary>
		public void ClearGroupChecks()
		{
			CheckBox2.Checked = false;
			CheckBox3.Checked = false;
			CheckBox5.Checked = false;
			CheckBox4.Checked = false;

			CheckBox2.Enabled = false;
			CheckBox3.Enabled = false;
			CheckBox5.Enabled = false;
			CheckBox4.Enabled = false;

			ListBox4.SelectedIndex = -1;
		}

		/// <summary>
		/// Mostra os erros ocorridos na pagina em relaÃ§ao a banco de dados
		/// </summary>
		public void ShowError()
		{
			string vgText = "";
			foreach (string vgKey in ErrorList.Keys)
			{
				vgText += ErrorList[vgKey].ToString() + "<br />";
			}
			labError.Visible = true;
			labError.Text = vgText;
		}

		/// <summary>
		/// Enche a lista de paginas com todas as paginas do xml
		/// </summary>
		private void FillGridAccess()
		{
			ListBox4.Items.Clear();
			XmlDocument doc = new XmlDocument();
			doc.Load(Server.MapPath("../Xmls/pages.xml"));
			foreach (XmlNode mPage in doc.FirstChild.NextSibling.ChildNodes)
			{
				ListBox4.Items.Add(mPage.Attributes["Name"].Value);
			}
		}

		/// <summary>
		/// Enche a lista de usuarios de acordo com o grupo escolhido
		/// </summary>
		private void FillUserListBox()
		{
			ListBox6.Items.Clear();
			DataTable dt = vgMembership.GetAllUsersByGroup(ListBox5.SelectedItem.Value);
			
			foreach (DataRow dr in dt.Rows)
			{
				ListItem li = new ListItem(Crypt.Decripta(dr[GMembershipProvider.Default.UserLoginField].ToString()), dr[GMembershipProvider.Default.UserLoginField].ToString());
				li.Value = dr["LOGIN_USER_LOGIN"].ToString();
				ListBox6.Items.Add(li);
			}
		}

		/// <summary>
		/// Enche a list de grupos com os grupos que estao no banco
		/// </summary>
		private void FillGroupsListBox()
		{
			FillGroupsListBox(false);
		}

		/// <summary>
		/// Enche a list de grupos com os grupos que estao no banco
		/// </summary>
		private void FillGroupsListBox(bool JustUser)
		{
			if (!JustUser) ListBox3.Items.Clear();
			ListBox5.Items.Clear();
			DataTable dt = vgMembership.GetAllGroups();
			foreach (DataRow dr in dt.Rows)
			{
				ListItem li = new ListItem(Crypt.Decripta(dr[GMembershipProvider.Default.GroupNameField].ToString()), dr[GMembershipProvider.Default.GroupIdField].ToString());
				if (!JustUser) ListBox3.Items.Add(li);
				ListBox5.Items.Add(li);
			}
		}

		/// <summary>
		/// Decripta o registo que veio do banco de acordo com o nome do campo
		/// </summary>
		/// <param name="Ds">Data set com os registros</param>
		/// <param name="FieldName"></param>
		private void DecryptDataSetField(ref DataSet Ds, string FieldName)
		{
			foreach (DataRow Dr in Ds.Tables[0].Rows)
			{
				Dr[FieldName] = Crypt.Decripta(Dr[FieldName].ToString());
			}
		}

		/// <summary>
		/// ao mudar de grupo selecionado ele coloca as permissÃµes e arruma a toolbar de acordo com o grupo escolhido
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ChangedGroup()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
					LoginGroupItem item = vgMembership.GetGroupByID(ListBox3.SelectedItem.Value);
			
					if ((bool)item.Fields["IsAdmin"].Value)
					{
						ListBox4.Enabled = false;
						CheckBox5.Enabled = false;
						CheckBox3.Enabled = false;
						CheckBox4.Enabled = false;
						CheckBox2.Enabled = false;
						CheckBox5.Checked = false;
						CheckBox3.Checked = false;
						CheckBox4.Checked = false;
						CheckBox2.Checked = false;
						EnableGroupButtons(false, false, false, true);
					
						CheckBox1.Checked = true;
						TextBox6.Text = Crypt.Decripta(item.Fields["Name"].GetFormattedValue());
						TextBox6.Focus();
					}
					else
					{
						ListBox4.Enabled = true;
						CheckBox5.Enabled = false;
						CheckBox3.Enabled = false;
						CheckBox4.Enabled = false;
						CheckBox2.Enabled = false;
						EnableGroupButtons(true, true, true, true);
						TextBox6.Text = Crypt.Decripta(item.Fields["Name"].GetFormattedValue());
						CheckBox1.Checked = (bool)item.Fields["IsAdmin"].Value;
						TextBox6.Focus();
						ChangedPagesPermission();
					}
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// Troca a senha do usuário que está logado
		/// </summary>
		public void ChangeUserPassword()
		{
			if (vgMembership.ValidateUser(Crypt.Decripta(HttpContext.Current.Session["vgUserLogin"].ToString()), txtActualPassword.Text.ToUpper()) == true)
			{
				if (txtNewPassword.Text != "")
				{
					if (txtNewPasswordConfirm.Text != "")
					{
						if (txtNewPassword.Text.ToUpper() == txtNewPasswordConfirm.Text.ToUpper())
						{
							vgMembership.ChangePassword(Crypt.Decripta(HttpContext.Current.Session["vgUserLogin"].ToString()), "", txtNewPassword.Text.ToUpper());
							OpenAlert("Salvo com sucesso!");
						}
						else
						{
							ErrorList.Add("ConfirmTrcaSenhaErrada", "Confirmação de senha invalida!");
						}
					}
					else
					{
						ErrorList.Add("SenhaAtualErrada", "Confirmação de senha tem que ser preenchida!");
					}
				}
				else
				{
					ErrorList.Add("SenhaAtualErrada", "Senha nova tem que ser preenchida!");
				}
			}
			else
			{
				if (txtActualPassword.Text != "")
				{
					ErrorList.Add("SenhaAtualErrada", "Senha atual errada!");
				}
				else
				{
					ErrorList.Add("SenhaAtualErrada", "Senha atual tem que ser preenchida!");
				}
			}
			ShowError();
		}

		/// <summary>
		/// Abre uma janela com uma mensagem
		/// </summary>
		/// <param name="text">mensagem a ser escrita no alert</param>
		public void OpenAlert(string text)
		{
			AjaxPanel1.ResponseScripts.Add("alert('" + text + "');");
		}

		/// <summary>
		/// Cancela a mudanÃ§a de senha
		/// </summary>
		private void CancelPasswordChanges()
		{
			txtActualPassword.Text = "";
			txtNewPassword.Text = "";
			txtNewPasswordConfirm.Text = "";
		}

		/// <summary>
		/// Efetiva a nova senha
		/// </summary>
		private void SavePasswordChanges()
		{
			ChangeUserPassword();
		}

		public void UpdateGroup()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
					LoginGroupItem item = new LoginGroupItem();
					item.Fields["Name"].SetValue(Crypt.Encripta(TextBox6.Text));
					item.Fields["Id"].SetValue(ListBox3.SelectedValue);

					item.Fields["IsAdmin"].SetValue(CheckBox1.Checked);
			
					ListBox3.Items[ListBox3.SelectedIndex].Text = TextBox6.Text;

					ListBox3.Items[ListBox3.SelectedIndex].Value = Crypt.Encripta(TextBox6.Text);
					FillGroupsListBox(true);
					vgMembership.UpdateGroup(item);
				}
			}
			catch
			{
			
			}
		}
		private void SaveGroupChanges()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
					if (ViewState["Insert"] == null || (bool)ViewState["Insert"] == false)
					{
						UpdateGroup();
						if (ListBox4.SelectedItem != null)
						{
							LoginGroupItem item = new LoginGroupItem();
							FillGroupsListBox();
							InsertPageAccess(ListBox4.SelectedItem.Text, Crypt.Encripta(TextBox6.Text));
							SetGroupChecks(ListBox3.SelectedValue, ListBox4.SelectedItem.Text);
						}
					}
					else
					{
						CreateGroup();
						ClearGroupChecks();
						ViewState["Insert"] = false;
					}
					ViewState["SalvaPaginas"] = true;
					ShowError();
				}
			}
			catch
			{
			}
		}

		private void RemoveGroup()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
					if (ListBox3.SelectedValue != "")
					{
						DeleteGroup();
						ClearGroupChecks();
					}
				}
			}
			catch
			{
			
			}
		}

		private void InsertGroup()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
					ViewState["Insert"] = true;
					TextBox6.Enabled = true;
					EnableGroupButtons(true, true, false, false);
					ListBox3.Enabled = false;
					TextBox6.Focus();
					TextBox6.Text = "";
					CheckBox1.Checked = false;
				}
			}
			catch
			{
			}
		}

		private void CancelGroupChanges()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
					if (ListBox4.SelectedItem != null)
					{
						SetGroupChecks(ListBox3.SelectedValue, ListBox4.SelectedItem.Text);
					}
					ViewState["SalvaPaginas"] = true;
					ViewState["Insert"] = false;
					EnableGroupButtons(false, false, true, true);
					ListBox3.Enabled = true;
				}
			}
			catch
			{
			
			}
		}

		private void SaveUserChanges()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
					if (ViewState["IncluiUsuario"] != null && (bool)ViewState["IncluiUsuario"] == true)
					{
						CreateUser();
					}
					else if (ListBox6.SelectedIndex >= 0)
					{
						UpdateUser();
					}
					ShowError();
				}
			}
			catch
			{
			}
		}

		private void RemoveUser()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
					if (ListBox6.SelectedValue != "")
					{
						DeleteUser();
					}
					ViewState["IncluiUsuario"] = false;
				}
			}
			catch
			{
			}
		}

		private void InsertUser()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
					TextBox7.Focus();
					EnableUserButtons(true, true, false, false);
					ListBox6.Enabled = false;
					ListBox5.Enabled = false;
					TextBox7.Text = "";
					TextBox9.Text = "";
					TextBox8.Text = "";
					TextBox10.Text = "";
					TextBox11.Text = "";
					ListBox6.SelectedIndex = -1;
					ViewState["IncluiUsuario"] = true;
					TextBox9.Enabled = true;
					TextBox7.Enabled = true;
					TextBox8.Enabled = true;
					TextBox10.Enabled = true;
				}
			}
			catch
			{
			}
		}
		
		private void CancelUserChanges()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
					if (ViewState["IncluiUsuario"] != null && (bool)ViewState["IncluiUsuario"] == true)
					{
						TextBox7.Text = "";
						TextBox11.Text = "";
						TextBox9.Text = "";
						TextBox8.Text = "";
						TextBox10.Text = "";
						ViewState["IncluiUsuario"] = false;
					}
					else if (ListBox6.SelectedIndex != -1 && ListBox6.SelectedValue != null)
					{
						LoginUserItem vgUser = vgMembership.GetUser(Crypt.Encripta(ListBox6.SelectedItem.Text));
						TextBox7.Text = Crypt.Decripta(vgUser.Fields["Login"].GetFormattedValue());
						TextBox11.Text = Crypt.Decripta(vgUser.Fields["Name"].GetFormattedValue());

						TextBox10.Text = Crypt.Decripta(vgUser.Fields["Obs"].GetFormattedValue());
					}
					EnableUserButtons(true, true, true, true);
					ListBox6.Enabled = true;
					ListBox5.Enabled = true;
					TextBox9.Enabled = true;
					TextBox7.Enabled = true;
					TextBox8.Enabled = true;
					TextBox10.Enabled = true;
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// Update um registo de usuario
		/// </summary>
		private void UpdateUser()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
					string vgPass = "";
					if (TextBox8.Text != "")
					{
						if (TextBox8.Text == TextBox9.Text)
						{
							vgPass = TextBox8.Text.ToUpper();
						}
						else
						{
							ErrorList.Add("SenhaErrada", "Confirmação de senha invalida");
							return;
						}
					}
					try
					{
						LoginUserItem item = new LoginUserItem();
						
						item.Fields["Group"].SetValue(ListBox5.SelectedValue);

						item.Fields["Login"].SetValue(Crypt.Encripta(TextBox7.Text));

						item.Fields["Id"].SetValue(ListBox6.SelectedValue);
						if (vgPass != "")
						{
							item.Fields["Password"].SetValue(Crypt.Encripta(vgPass));
						}
						else
						{
							LoginUserItem OldItem = vgMembership.GetUser(ListBox6.SelectedValue);
							item.Fields["Password"].SetValue(OldItem.Fields["Password"].GetFormattedValue());
						}
						item.Fields["Obs"].SetValue(Crypt.Encripta(TextBox10.Text));
						item.Fields["Name"].SetValue(Crypt.Encripta(TextBox11.Text.ToUpper()));
						vgMembership.UpdateUser(item);
						ListBox6.Items[ListBox6.SelectedIndex].Text = TextBox7.Text;
						ListBox6.Enabled = true;
						ListBox5.Enabled = true;
						EnableUserButtons(true, true, true, true);
					}
					catch (Exception e)
					{
						ErrorList.Add("UpdateUserError", e.Message);
					}
				}
			}
			catch
			{
			}
		}
		/// <summary>
		/// Metodo usado para inserir um grupo no banco de dados
		/// </summary>
		private void CreateGroup()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{		
					if (TextBox6.Text == "")
					{
						ErrorList.Add("GrupoInvalido", "Nome do grupo tem que ser preenchido");
						return;
					}
					try
					{
						LoginGroupItem item = new LoginGroupItem();
						item.Fields["Id"].SetValue(TextBox6.Text);
						item.Fields["Name"].SetValue(Crypt.Encripta(TextBox6.Text));
						item.Fields["IsAdmin"].SetValue(CheckBox1.Checked);
						vgMembership.GroupProvider.DataProvider.InsertItem(item);
						FillGroupsListBox();
						ListBox3.SelectedIndex = ListBox3.Items.Count - 1;
						FillUserListBox();
						ViewState["Insert"] = false;
						EnableGroupButtons(true, true, true, true);
						ListBox3.Enabled = true;
					}
					catch (Exception e)
					{
						ErrorList.Add("dadosError", e.Message);
					}
				}
			}
			catch
			{
			
			}
			
		}

		/// <summary>
		/// Exclui um grupo de usuarios e enche os lists a treview de menus de acordo com os menus q sobraram
		/// O grupo de admininstrador nao pode ser excluido
		/// </summary>
		private void DeleteGroup()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
			
					if (ListBox3.SelectedIndex == 0)
					{
						ErrorList.Add("GrupoAdminExcluido", "O grupo Administrador nÃ£o pode ser excluido");
						return;
					}
					vgMembership.DeleteGroup(ListBox3.SelectedValue);
					TextBox6.Text = "";
					FillGroupsListBox();
					FillUserListBox();
				}
			}
			catch
			{

			}
		}
		/// <summary>
		/// Exclui usuario do banco de dados
		/// </summary>
		private void DeleteUser()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{		
					if (ListBox5.SelectedIndex == 0 && ListBox6.Items.Count == 1)
					{
						ErrorList.Add("ExcluirUltimoAdmin", "Ultimo administrador nÃ£o pode ser excluido!");
					}
					else	
					{
						vgMembership.DeleteUser(Crypt.Encripta(ListBox6.SelectedItem.Text));
						FillUserListBox();
						TextBox10.Text = "";
						TextBox7.Text = "";
					}
				}
			}
			catch
			{
			}
		}
		/// <summary>
		/// Insere um usuario no banco de dados
		/// </summary>
		private void CreateUser()
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{		
					if (ListBox5.SelectedIndex == -1)
					{
						ErrorList.Add("UsuarioGrupoInvalido", "Escolha um grupo");
					}
					else
					{
					if (TextBox7.Text == "")
					{
						ErrorList.Add("UsuarioNomeInvalido", "Nome do Login de usuário tem que ser preenchido");
					}

					if (TextBox11.Text == "")
					{
						ErrorList.Add("UsuarioNomeInvalido", "Nome Completo do usuário tem que ser preenchido");
					}

					if (TextBox8.Text == "")
					{
						ErrorList.Add("UsuarioSenhaInvalido", "Senha do usuário tem que ser preenchido");
					}
					if (TextBox9.Text == "")
					{
						ErrorList.Add("UsuarioCsenhaInvalido", "Confirmação de senha tem que ser preenchido");
					}
					}
					if (ErrorList.Count > 0)
					{
						return;
					}
					if (TextBox8.Text != TextBox9.Text)
					{
						ErrorList.Add("SenhaEconfirmacaoDif", "Confirmação de senha invalida");
					}
					if (ErrorList.Count > 0)
					{
						return;
					}
					try
					{
						LoginUserItem item = new LoginUserItem();
						item.Fields["Login"].SetValue(Crypt.Encripta(TextBox7.Text.ToUpper()));
						item.Fields["Id"].SetValue(ListBox6.SelectedValue);

						item.Fields["Password"].SetValue(Crypt.Encripta(TextBox8.Text.ToUpper()));
						item.Fields["Group"].SetValue(ListBox5.SelectedValue);

						item.Fields["Obs"].SetValue(Crypt.Encripta(TextBox10.Text));
						item.Fields["Name"].SetValue(Crypt.Encripta(TextBox11.Text));
						if (vgMembership.CreateUser(item) == 0)
						{
							throw new Exception("Erro na criação do usuario");
						}
						FillUserListBox();
						ListBox6.Enabled = true;
						ListBox5.Enabled = true;
						ListBox6.SelectedIndex = ListBox6.Items.Count - 1;
						EnableUserButtons(false, false, true, true);
					}
					catch (Exception ex)
					{
						ErrorList.Add("ErroDeCriacao", ex.Message);
					}
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// Habilita ou desabilita os botÃµes de grupos
		/// </summary>
		private void EnableGroupButtons(bool SaveButton, bool CancelButton, bool RemoveButton, bool AddButton)
		{
			butGroupNew2.Enabled = AddButton;
			butGroupSave2.Enabled = SaveButton;
			butGroupCancel2.Enabled = CancelButton;
			butGroupRemove2.Enabled = RemoveButton;
		}

		/// <summary>
		/// Habilita ou desabilita os botÃµes de usuarios
		/// </summary>
		private void EnableUserButtons(bool SaveButton, bool CancelButton, bool RemoveButton, bool AddButton)
		{
			Button1.Enabled = AddButton;
			Button3.Enabled = SaveButton;
			Button4.Enabled = CancelButton;
			Button2.Enabled = RemoveButton;
		}

		/// <summary>
		/// Habilita ou desabilita os botÃµes de senha
		/// </summary>
		private void EnablePasswordButtons(bool SaveButton, bool CancelButton)
		{
			butPWSave.Enabled = SaveButton;
		}
		/// <summary>
		/// Coloca permissÃ£o para os grupos de acordo com a pagina 
		/// </summary>
		/// <param name="vgPagina"></param>
		/// <param name="vgGrupo"></param>
		private void InsertPageAccess(string vgPagina, string vgGrupo)
		{
			try
			{
				if (CurrentGroup != null && (bool)CurrentGroup.Fields["IsAdmin"].Value)
				{
					string ViewChecked = "1";
					string EditChecked = "1";
					string AddChecked = "1";
					string RemoveChecked = "1";
					if (CheckBox2.Checked == false)
						ViewChecked = "0";
					if (CheckBox3.Checked == false)
						EditChecked = "0";
					if (CheckBox5.Checked == false)
						AddChecked = "0";
					if (CheckBox4.Checked == false)
						RemoveChecked = "0";
					vgMembership.InsertPerm(vgGrupo, vgPagina, ViewChecked, EditChecked, AddChecked, RemoveChecked, ProjectID);
				}
			}
			catch
			{
			}
			
		}
		/// <summary>
		/// Quando mudar de grupo na pagina de inserção de usuarios mostrar os novos usuarios na list
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ChangedGroupUser()
		{
			FillUserListBox();
			EnableUserButtons(false, false, true, true);
			TextBox9.Enabled = false;
			TextBox7.Enabled = false;
			TextBox8.Enabled = false;
			TextBox10.Enabled = false;

			TextBox9.Text = "";
			TextBox7.Text = "";
			TextBox8.Text = "";
			TextBox10.Text = "";
			TextBox11.Text = "";
		}

		/// <summary>
		/// Metodo que mostra o registro do usuario selecionado
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ChangedUser()
		{
			if(ListBox6.SelectedItem != null)
			{
				LoginUserItem vgUser = vgMembership.GetUser(Crypt.Encripta(ListBox6.SelectedItem.Text));
				TextBox7.Text = Crypt.Decripta(vgUser.Fields["Login"].GetFormattedValue());
				TextBox11.Text = Crypt.Decripta(vgUser.Fields["Name"].GetFormattedValue());
				TextBox10.Text = Crypt.Decripta(vgUser.Fields["Obs"].GetFormattedValue());
				TextBox9.Enabled = true;
				TextBox9.Text = "";
				TextBox10.Enabled = true;
				TextBox8.Text = "";
				TextBox7.Enabled = true;
				TextBox8.Enabled = true;

				ViewState["IncluiUsuario"] = false;
				EnableUserButtons(true, true, true, true);
			}
		}

		/// <summary>
		/// Metodo que coloca as permições de acordo com a página que foi clicada
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ChangedPagesPermission()
		{
			CheckBox2.Enabled = true;
			CheckBox3.Enabled = true;
			CheckBox5.Enabled = true;
			CheckBox4.Enabled = true;
			if (ViewState["UltimaPagina"] != null && ViewState["UltimaPagina"].ToString() != "" && ViewState["UltimaPagina"].ToString() != ListBox4.SelectedItem.Text)
			{
				InsertPageAccess(ViewState["UltimaPagina"].ToString(), ListBox3.SelectedValue);
			}
			ViewState["UltimaPagina"] = ListBox4.SelectedItem.Text;
			SetGroupChecks(ListBox3.SelectedValue, ListBox4.SelectedItem.Text);
		}

		void AjaxPanel1_AjaxRequest(object source, Telerik.Web.UI.AjaxRequestEventArgs e)
		{
			if (!e.Argument.StartsWith("ExecuteCommand"))
			{
				switch (e.Argument)
				{
					case "butPWSave":
						___butPWSave_OnClick(source, e);
						break;
					case "butGroupCancel2":
						___butGroupCancel2_OnClick(source, e);
						break;
					case "butGroupSave2":
						___butGroupSave2_OnClick(source, e);
						break;
					case "butGroupNew2":
						___butGroupNew2_OnClick(source, e);
						break;
					case "butGroupRemove2":
						___butGroupRemove2_OnClick(source, e);
						break;
					case "Button1":
						___Button1_OnClick(source, e);
						break;
					case "Button3":
						___Button3_OnClick(source, e);
						break;
					case "Button4":
						___Button4_OnClick(source, e);
						break;
					case "Button2":
						___Button2_OnClick(source, e);
						break;
				}
			}
		}
		protected void ___butPWSave_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				SavePasswordChanges();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___ListBox3_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				ChangedGroup();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___ListBox4_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				ChangedPagesPermission();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butGroupCancel2_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				CancelGroupChanges();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butGroupSave2_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				SaveGroupChanges();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butGroupNew2_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				InsertGroup();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butGroupRemove2_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				RemoveGroup();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___ListBox5_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				ChangedGroupUser();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___ListBox6_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				ChangedUser();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___Button1_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				InsertUser();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___Button3_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				SaveUserChanges();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___Button4_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				CancelUserChanges();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___Button2_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				RemoveUser();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

	}
}
