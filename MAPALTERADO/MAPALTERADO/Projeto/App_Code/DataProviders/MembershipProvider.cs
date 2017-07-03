using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Collections;
using System.Collections.Specialized;
using COMPONENTS;
using COMPONENTS.Data;
using COMPONENTS.Configuration;
using PROJETO.DataProviders;
using System.Collections.Generic;

namespace PROJETO
{

	public class MembershipUserProvider : IGeneralDataProvider
	{

		public LoginUserItem CurrentItem;

		#region IGeneralDataProvider Members

		public string FormID
		{
			get { return "1.1"; }
		}

		public string TableName
		{
			get { return GMembershipProvider.Default.UserTableName; }
		}

		public string DatabaseName
		{
			get { return GMembershipProvider.Default.DatabaseName; }
		}

		private LoginUserDataProvider LoginUserProvider;
		public GeneralDataProvider DataProvider
		{
			get { return LoginUserProvider; }
			set { LoginUserProvider = value as LoginUserDataProvider; }
		}

		public GeneralDataProviderItem LoadItemFromControl(bool EnableValidation)
		{
			throw new NotImplementedException();
		}
		
		public GeneralDataProviderItem LoadItemFromGridControl(bool EnableValidation, string GridId)
		{
			throw new NotImplementedException();
		}

		public GeneralDataProviderItem LoadItemFromSchedulerControl(bool EnableValidation, string Grid)
		{
			throw new NotImplementedException();
		}
		
		public GeneralDataProviderItem LoadItemFromImageGalleryControl(bool EnableValidation, string ImageGallery)
        {
            throw new NotImplementedException();
        }

		public GeneralDataProviderItem LoadItemFromGanttControl(bool EnableValidation, string Gantt)
		{
			throw new NotImplementedException();
		}

		public GeneralDataProviderItem LoadItemFromDependenciesGanttControl(bool EnableValidation, string Gantt)
		{
			throw new NotImplementedException();
		}

		public void DeleteChildItens()
		{
			throw new NotImplementedException();
		}

		public GeneralDataProviderItem GetDataProviderItem(GeneralDataProvider Provider)
		{
			return new LoginUserItem();
		}
		
		public void OnSelectedItem(GeneralDataProvider Provider, GeneralDataProviderItem Item, bool UpdateFromUI)
		{
		}
		
		public GeneralDataProviderItem GetCurrentItem(FormPositioningEnum Positioning, bool UpdateFromUI)
		{
			return null;
		}
		
		public void SetParametersValues(GeneralDataProvider Provider)
		{
			if(CurrentItem != null)
			{
				DataProvider.Parameters["Login"].Parameter.SetValue(CurrentItem.Fields["Login"].Value);
			}
		}

		// cria lista de parâmetros vazia
		public void CreateEmptyParameters(GeneralDataProvider Provider)
		{
			if(DataProvider==Provider)
			{
				Provider.Parameters.Clear();
				Provider.CreateParameters();
			}	
		}

		public void GetParameters(bool KeepCurrentRecord,GeneralDataProvider Provider)
		{
			CreateEmptyParameters(Provider);
			SetParametersValues(Provider);
		}
		
		public void SetOldParameters(GeneralDataProviderItem Item)
        {
		
        }
		
		#endregion

		public MembershipUserProvider()
		{
			LoginUserProvider = new LoginUserDataProvider(this, TableName, DatabaseName,"", "");
		}

		private bool CheckUser(string UserName, string Password)
		{
			CurrentItem = new LoginUserItem();
			CurrentItem.Fields["Login"].SetValue(Crypt.Encripta(UserName));
			CurrentItem = DataProvider.SelectItem(0, FormPositioningEnum.First) as LoginUserItem;
			if (CurrentItem != null)
			{
				return CurrentItem.Fields["Password"].Value.Equals(Crypt.Encripta(Password));
			}
			return false;
		}

		public bool ChangePW(string UserName, string Password)
		{
			CurrentItem = new LoginUserItem();
			CurrentItem.Fields["Login"].SetValue(Crypt.Encripta(UserName));
			CurrentItem = DataProvider.SelectItem(0, FormPositioningEnum.First) as LoginUserItem;
			CurrentItem.Fields["Password"].SetValue(Crypt.Encripta(Password));
			return DataProvider.UpdateItem(CurrentItem) > 0;
		}

		internal bool ValidateUser(string UserName, string Password)
		{
			bool RetVal = CheckUser(UserName, Password);
			if (CurrentItem == null)
			{
				GMembershipProvider.Default.CheckDefaultUserGroup();
				RetVal = CheckUser(UserName, Password);
			}
			return RetVal;
		}

		internal LoginUserItem GetDefaultUser()
		{
			CurrentItem = new LoginUserItem();
			CurrentItem.SetDefaultUser();
			CurrentItem = DataProvider.SelectItem(0, FormPositioningEnum.First) as LoginUserItem;
			return CurrentItem;
		}

		private void CreateDefaultUser()
		{
			CurrentItem = new LoginUserItem();
			CurrentItem.SetDefaultUser();
			DataProvider.InsertItem(CurrentItem);
		}

		internal void CheckDefaultUser()
		{
			if (DataProvider.GetExistRecords() == 0)
			{
				if (GetDefaultUser() == null)
				{
					CreateDefaultUser();
				}
			}
		}

		public bool SelectUser(string Login)
		{
			CurrentItem = new LoginUserItem();
			CurrentItem.Fields["Login"].SetValue(Login);
			CurrentItem = DataProvider.SelectItem(0, FormPositioningEnum.First) as LoginUserItem;
			return (CurrentItem != null);
		}

		internal DataTable GetAllUsersByGroup(string GroupID)
		{
			if (GroupID.Length > 0)
			{
				return LoginUserProvider.GetAllUsersByGroup("'" + GroupID + "'", GMembershipProvider.Default.UserGroupIdField);
			}
			else
			{
				return LoginUserProvider.GetAllUsersByGroup("", GMembershipProvider.Default.UserGroupIdField);
			}
		}

		public void OnCommiting()
		{
		}

		public void OnRollbacking()
		{
		}
	}
	public class MembershipRuleProvider : IGeneralDataProvider
	{

		public LoginRuleItem CurrentItem;

		#region IGeneralDataProvider Members

		public string FormID
		{
			get { return "1.2"; }
		}

		public string TableName
		{
			get { return GMembershipProvider.Default.RuleTableName; }
		}

		public string DatabaseName
		{
			get { return GMembershipProvider.Default.DatabaseName; }
		}

		private LoginRuleDataProvider LoginRuleProvider;
		public GeneralDataProvider DataProvider
		{
			get { return LoginRuleProvider; }
			set { LoginRuleProvider = value as LoginRuleDataProvider; }
		}

		public GeneralDataProviderItem LoadItemFromControl(bool EnableValidation)
		{
			throw new NotImplementedException();
		}

		public GeneralDataProviderItem LoadItemFromGridControl(bool EnableValidation, string GridId)
		{
			throw new NotImplementedException();
		}

		public GeneralDataProviderItem LoadItemFromSchedulerControl(bool EnableValidation, string Grid)
		{
			throw new NotImplementedException();
		}
		
		public GeneralDataProviderItem LoadItemFromImageGalleryControl(bool EnableValidation, string ImageGallery)
        {
            throw new NotImplementedException();
        }

		public GeneralDataProviderItem LoadItemFromGanttControl(bool EnableValidation, string Gantt)
		{
			throw new NotImplementedException();
		}

		public GeneralDataProviderItem LoadItemFromDependenciesGanttControl(bool EnableValidation, string Gantt)
		{
			throw new NotImplementedException();
		}

		public void DeleteChildItens()
		{
			throw new NotImplementedException();
		}

		public GeneralDataProviderItem GetDataProviderItem(GeneralDataProvider Provider)
		{
			return new LoginRuleItem();
		}

		public void OnSelectedItem(GeneralDataProvider Provider, GeneralDataProviderItem Item, bool UpdateFromUI)
		{
		}
		
		public GeneralDataProviderItem GetCurrentItem(FormPositioningEnum Positioning, bool UpdateFromUI)
		{
			return null;
		}
		
		public void SetParametersValues(GeneralDataProvider Provider)
		{
			DataProvider.Parameters["Group"].Parameter.SetValue(CurrentItem.Fields["Group"].Value);
			DataProvider.Parameters["Project"].Parameter.SetValue(CurrentItem.Fields["Project"].Value);
			DataProvider.Parameters["Object"].Parameter.SetValue(CurrentItem.Fields["Object"].Value);
		}

		// cria lista de parâmetros vazia
		public void CreateEmptyParameters(GeneralDataProvider Provider)
		{
			if(DataProvider==Provider)
			{
				Provider.Parameters.Clear();
				Provider.CreateParameters();
			}	
		}

		public void GetParameters(bool KeepCurrentRecord,GeneralDataProvider Provider)
		{
			CreateEmptyParameters(Provider);
			SetParametersValues(Provider);
		}
		
		public void SetOldParameters(GeneralDataProviderItem Item)
        {
        
        }

		#endregion

		public MembershipRuleProvider()
		{
			LoginRuleProvider = new LoginRuleDataProvider(this, TableName, DatabaseName,"", "");
		}

		internal bool SelectRule(string ProjectName, string vgGrupoId, string vgPaginaNome)
		{
			CurrentItem = new LoginRuleItem();
			CurrentItem.Fields["Project"].SetValue(ProjectName);
			CurrentItem.Fields["Group"].SetValue(vgGrupoId);
			CurrentItem.Fields["Object"].SetValue(vgPaginaNome);
			CurrentItem = DataProvider.SelectItem(0, FormPositioningEnum.First) as LoginRuleItem;
			return (CurrentItem != null);
		}

		public void OnCommiting()
		{
		}

		public void OnRollbacking()
		{
		}
	}

	public class MembershipGroupProvider : IGeneralDataProvider
	{

		public LoginGroupItem CurrentItem;

		#region IGeneralDataProvider Members

		public string FormID
		{
			get { return "1.3"; }
		}

		public string TableName
		{
			get { return GMembershipProvider.Default.GroupTableName; }
		}

		public string DatabaseName
		{
			get { return GMembershipProvider.Default.DatabaseName; }
		}

		private LoginGroupDataProvider LoginGroupProvider;
		public GeneralDataProvider DataProvider
		{
			get { return LoginGroupProvider; }
			set { LoginGroupProvider = value as LoginGroupDataProvider; }
		}

		public GeneralDataProviderItem LoadItemFromControl(bool EnableValidation)
		{
			throw new NotImplementedException();
		}

		public GeneralDataProviderItem LoadItemFromGridControl(bool EnableValidation, string GridId)
		{
			throw new NotImplementedException();
		}

		public GeneralDataProviderItem LoadItemFromSchedulerControl(bool EnableValidation, string Grid)
		{
			throw new NotImplementedException();
		}
		
		public GeneralDataProviderItem LoadItemFromImageGalleryControl(bool EnableValidation, string ImageGallery)
        {
            throw new NotImplementedException();
        }
		
		public GeneralDataProviderItem LoadItemFromGanttControl(bool EnableValidation, string Gantt)
		{
			throw new NotImplementedException();
		}

		public GeneralDataProviderItem LoadItemFromDependenciesGanttControl(bool EnableValidation, string Gantt)
		{
			throw new NotImplementedException();
		}

		public void DeleteChildItens()
		{
			throw new NotImplementedException();
		}

		public GeneralDataProviderItem GetDataProviderItem(GeneralDataProvider Provider)
		{
			return new LoginGroupItem();
		}
		
		public GeneralDataProviderItem GetCurrentItem(FormPositioningEnum Positioning, bool UpdateFromUI)
		{
			return null;
		}

		public void OnSelectedItem(GeneralDataProvider Provider, GeneralDataProviderItem Item, bool UpdateFromUI)
		{
		}
		
		public void SetParametersValues(GeneralDataProvider Provider)
		{
			DataProvider.Parameters["Id"].Parameter.SetValue(CurrentItem.Fields["Id"].Value);
		}

		// cria lista de parâmetros vazia
		public void CreateEmptyParameters(GeneralDataProvider Provider)
		{
			if(DataProvider==Provider)
			{
				Provider.Parameters.Clear();
				Provider.CreateParameters();
			}	
		}

		public void GetParameters(bool KeepCurrentRecord,GeneralDataProvider Provider)
		{
			CreateEmptyParameters(Provider);
			SetParametersValues(Provider);
		}
		
		public void SetOldParameters(GeneralDataProviderItem Item)
        {
        
        }

		#endregion

		public MembershipGroupProvider()
		{
			LoginGroupProvider = new LoginGroupDataProvider(this, TableName, DatabaseName,"LOGIN_GROUP_PK", "");
		}

		internal LoginGroupItem GetDefaultGroup()
		{
			CurrentItem = new LoginGroupItem();
			CurrentItem.SetDefaultGroup();
			CurrentItem = DataProvider.SelectItem(0, FormPositioningEnum.First) as LoginGroupItem;
			return CurrentItem;
		}

		public bool SelectGroup(string GroupId)
		{
			CurrentItem = new LoginGroupItem();
			CurrentItem.Fields["Id"].SetValue(GroupId);
			CurrentItem = DataProvider.SelectItem(0, FormPositioningEnum.First) as LoginGroupItem;
			return (CurrentItem != null);
		}

		private void CreateDefaultGroup()
		{
			CurrentItem = new LoginGroupItem();
			CurrentItem.SetDefaultGroup();
			DataProvider.InsertItem(CurrentItem);
		}

		internal void CheckDefaultGroup()
		{
			if (DataProvider.GetExistRecords() == 0)
			{
				if (GetDefaultGroup() == null)
				{
					CreateDefaultGroup();
				}
			}
		}

		internal DataTable GetAllGroups()
		{
			return LoginGroupProvider.GetAllGroups();
		}

		public void OnCommiting()
		{
		}

		public void OnRollbacking()
		{
		}
	}

	/// <summary>
	/// Provider de manutenção dos usuários no banco de dados
	/// </summary>
	public class GMembershipProvider : MembershipProvider
	{

		public static GMembershipProvider Default = (GMembershipProvider)Membership.Providers["GMembershipProvider"];
		
		private MembershipGroupProvider _GroupProvider;
		public MembershipGroupProvider GroupProvider
		{
			get
			{
				if (_GroupProvider == null)
				{
					_GroupProvider = new MembershipGroupProvider();
				}
				return _GroupProvider;
			}
		}

		private MembershipRuleProvider _RuleProvider;
		public MembershipRuleProvider RuleProvider
		{
			get
			{
				if (_RuleProvider == null)
				{
					_RuleProvider = new MembershipRuleProvider();
				}
				return _RuleProvider;
			}
		}
		
		private MembershipUserProvider _UserProvider;
		public MembershipUserProvider UserProvider
		{
			get
			{
				if (_UserProvider == null)
				{
					_UserProvider = new MembershipUserProvider();
				}
				return _UserProvider;
			}
		}
		
		private string _DatabaseName;
		
		private string _UserTableName;
		private string _UserIdField;
		private string _UserNameField;
		private string _UserLoginField;
		private string _UserPasswordField;
		private string _UserObsField;
		private string _UserGroupIdField;
		
		private string _GroupTableName;
		private string _GroupIdField;
		private string _GroupNameField;
		private string _GroupIsAdminField;

		private string _RuleTableName;
		private string _RuleProjectField;
		private string _RuleGroupIdField;
		private string _RuleObjectField;
		private string _RulePermissionsField;

		private string _UserIdSessionVariable;
		private string _UserLoginSessionVariable;
		private string _UserNameSessionVariable;
		private string _UserObsSessionVariable;
		private string _GroupIdSessionVariable;
		private string _GroupNameSessionVariable;
		private string _GroupIsAdminSessionVariable;

		public string DatabaseName { get { return _DatabaseName; } set { _DatabaseName = value; } }
		public string UserTableName { get { return _UserTableName; } set { _UserTableName = value; } }
		public string UserIdField { get { return _UserIdField; } set { _UserIdField = value; } }
		public string UserNameField { get { return _UserNameField; } set { _UserNameField = value; } }
		public string UserLoginField { get { return _UserLoginField; } set { _UserLoginField = value; } }
		public string UserPasswordField { get { return _UserPasswordField; } set { _UserPasswordField = value; } }
		public string UserObsField { get { return _UserObsField; } set { _UserObsField = value; } }
		public string UserGroupIdField { get { return _UserGroupIdField; } set { _UserGroupIdField = value; } }
		public string GroupTableName { get { return _GroupTableName; } set { _GroupTableName = value; } }
		public string GroupIdField { get { return _GroupIdField; } set { _GroupIdField = value; } }
		public string GroupNameField { get { return _GroupNameField; } set { _GroupNameField = value; } }
		public string GroupIsAdminField { get { return _GroupIsAdminField; } set { _GroupIsAdminField = value; } }
		
		public string RuleTableName { get { return _RuleTableName; } set { _RuleTableName = value; } }
		public string RuleProjectField { get { return _RuleProjectField; } set { _RuleProjectField = value; } }
		public string RuleGroupIdField { get { return _RuleGroupIdField; } set { _RuleGroupIdField = value; } }
		public string RuleObjectField { get { return _RuleObjectField; } set { _RuleObjectField = value; } }
		public string RulePermissionsField { get { return _RulePermissionsField; } set { _RulePermissionsField = value; } }
		
		public string UserIdSessionVariable { get { return _UserIdSessionVariable; } set { _UserIdSessionVariable = value; } }
		public string UserLoginSessionVariable { get { return _UserLoginSessionVariable; } set { _UserLoginSessionVariable = value; } }
		public string UserNameSessionVariable { get { return _UserNameSessionVariable; } set { _UserNameSessionVariable = value; } }
		public string UserObsSessionVariable { get { return _UserObsSessionVariable; } set { _UserObsSessionVariable = value; } }
		public string GroupIdSessionVariable { get { return _GroupIdSessionVariable; } set { _GroupIdSessionVariable = value; } }
		public string GroupNameSessionVariable { get { return _GroupNameSessionVariable; } set { _GroupNameSessionVariable = value; } }
		public string GroupIsAdminSessionVariable { get { return _GroupIsAdminSessionVariable; } set { _GroupIsAdminSessionVariable = value; } }

		/// <summary>
		/// Nome da aplicação
		/// </summary>
		public override string ApplicationName
		{
			get
			{
				return ConfigurationManager.AppSettings["AppName"].ToString();
			}
			set
			{
			}
		}

		/// <summary>
		/// Não permite reset de password
		/// </summary>
		public override bool EnablePasswordReset
		{
			get { return false; }
		}

		/// <summary>
		/// Não permite password request
		/// </summary>
		public override bool EnablePasswordRetrieval
		{
			get { return false; }
		}

		/// <summary>
		/// Inicia todas as variáveis de acordo com o web config
		/// </summary>
		/// <param name="name"></param>
		/// <param name="config">Todos os campos sao preenchidos com os nomes que estao no webconfig</param>
		public override void Initialize(string Name, System.Collections.Specialized.NameValueCollection Config)
		{
			DatabaseName = Config["DatabaseName"];
			
			UserTableName = Config["UserTableName"];
			UserIdField = Config["UserIdField"];
			UserNameField = Config["UserNameField"];
			UserLoginField = Config["UserLoginField"];
			UserPasswordField = Config["UserPasswordField"];
			UserObsField = Config["UserObsField"];
			UserGroupIdField = Config["UserGroupIdField"];
			
			GroupTableName = Config["GroupTableName"];
			GroupIdField = Config["GroupIdField"];
			GroupNameField = Config["GroupNameField"];
			GroupIsAdminField = Config["GroupIsAdminField"];
			
			RuleTableName = Config["RuleTableName"];
			RuleProjectField = Config["RuleProjectIdField"];
			RuleGroupIdField = Config["RuleGroupIdField"];
			RuleObjectField = Config["RuleObjectField"];
			RulePermissionsField = Config["RulePermissionsField"];
			
			UserIdSessionVariable = Config["UserIdSessionVariable"];
			UserLoginSessionVariable = Config["UserLoginSessionVariable"];
			UserNameSessionVariable = Config["UserNameSessionVariable"];
			UserObsSessionVariable = Config["UserObsSessionVariable"];
			GroupIdSessionVariable = Config["GroupIdSessionVariable"];
			GroupNameSessionVariable = Config["GroupNameSessionVariable"];
			GroupIsAdminSessionVariable = Config["GroupIsAdminSessionVariable"];
			base.Initialize(Name, Config);
		}

		public override string GetPassword(string username, string answer)
		{
			throw new Exception("Método não implementado.");
		}

		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new Exception("Método não implementado.");
		}

		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			throw new Exception("Método não implementado.");
		}

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			throw new Exception("Método não implementado.");
		}

		public override string GetUserNameByEmail(string email)
		{
			throw new Exception("Método não implementado.");
		}

		public override int MaxInvalidPasswordAttempts
		{
			get { throw new Exception("Método não implementado."); }
		}

		public override int MinRequiredNonAlphanumericCharacters
		{
			get { throw new Exception("Método não implementado."); }
		}

		public override int MinRequiredPasswordLength
		{
			get { throw new Exception("Método não implementado."); }
		}

		public override int PasswordAttemptWindow
		{
			get { throw new Exception("Método não implementado."); }
		}

		public override MembershipPasswordFormat PasswordFormat
		{
			get { throw new Exception("Método não implementado."); }
		}

		public override string PasswordStrengthRegularExpression
		{
			get { throw new Exception("Método não implementado."); }
		}

		public override bool RequiresQuestionAndAnswer
		{
			get { throw new Exception("Método não implementado."); }
		}

		public override bool RequiresUniqueEmail
		{
			get { throw new Exception("Método não implementado."); }
		}

		public override string ResetPassword(string username, string answer)
		{
			throw new Exception("Método não implementado.");
		}

		public override bool UnlockUser(string userName)
		{
			throw new Exception("Método não implementado.");
		}

		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			return UserProvider.ChangePW(username, newPassword);
		}

		public DataTable GetAllUsersByGroup(string GroupID)
		{
			return UserProvider.GetAllUsersByGroup(GroupID);
		}

		public DataTable GetAllGroups()
		{
			return GroupProvider.GetAllGroups();
		}

		public LoginGroupItem GetGroupByID(string ID)
		{
			GroupProvider.SelectGroup(ID);
			return GroupProvider.CurrentItem;
		}

		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			throw new NotImplementedException();
		}

		public int CreateUser(LoginUserItem NewUser)
		{
			return UserProvider.DataProvider.InsertItem(NewUser);
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			throw new NotImplementedException();
		}

		public int DeleteUser(string UserID)
		{
			UserProvider.SelectUser(UserID);
			return UserProvider.DataProvider.DeleteItem(UserProvider.CurrentItem);
		}

		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		public override int GetNumberOfUsersOnline()
		{
			throw new NotImplementedException();
		}

		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			throw new NotImplementedException();
		}

		public LoginUserItem GetUser(string UserID)
		{
			UserProvider.SelectUser(UserID);
			return UserProvider.CurrentItem;
		}

		public override void UpdateUser(MembershipUser user)
		{
			throw new NotImplementedException();
		}

		public void UpdateUser(LoginUserItem user)
		{
			UserProvider.DataProvider.UpdateItem(user);
		}

		public override bool ValidateUser(string username, string password)
		{
			bool IsValid = UserProvider.ValidateUser(username, password);
			if (IsValid)
			{
				HttpContext.Current.Session[UserIdSessionVariable] = UserProvider.CurrentItem["Id"].Value;
				HttpContext.Current.Session[UserLoginSessionVariable] = UserProvider.CurrentItem["Login"].Value;
				HttpContext.Current.Session[UserNameSessionVariable] = UserProvider.CurrentItem["Name"].Value;
				HttpContext.Current.Session[UserObsSessionVariable] = UserProvider.CurrentItem["Obs"].Value;
				if (GroupProvider.SelectGroup(UserProvider.CurrentItem["Group"].Value.ToString()))
				{
					HttpContext.Current.Session[GroupIdSessionVariable] = GroupProvider.CurrentItem["Id"].Value;
					HttpContext.Current.Session[GroupNameSessionVariable] = GroupProvider.CurrentItem["Name"].Value;
					HttpContext.Current.Session[GroupIsAdminSessionVariable] = GroupProvider.CurrentItem["IsAdmin"].Value;
				}
			}
			return IsValid;
		}
		internal LoginGroupItem GetDefaultGroup()
		{
			return GroupProvider.GetDefaultGroup();
		}
		
		internal void CheckDefaultUserGroup()
		{
			GroupProvider.CheckDefaultGroup();
			UserProvider.CheckDefaultUser();
		}

		public int DeleteGroup(string GroupID)
		{
			GroupProvider.SelectGroup(GroupID);
			return GroupProvider.DataProvider.DeleteItem(GroupProvider.CurrentItem);
		}

		public int UpdateGroup(LoginGroupItem item)
		{
			GroupProvider.CurrentItem = item;
			return GroupProvider.DataProvider.UpdateItem(item);
		}
		public bool TestIfCanSee(string ProjectName, string vgGrupoId, string vgPaginaNome)
		{
			RuleProvider.SelectRule(ProjectName, vgGrupoId, vgPaginaNome);
			if (RuleProvider.CurrentItem == null)
			{
				return true;
			}
			string Perm = RuleProvider.CurrentItem.Fields["Permissions"].GetFormattedValue();
			return (Perm[1] == '1');
		}

		public bool TestIfCanEdit(string ProjectName, string vgGrupoId, string vgPaginaNome)
		{
			RuleProvider.SelectRule(ProjectName, vgGrupoId, vgPaginaNome);
			if (RuleProvider.CurrentItem == null)
			{
				return true;
			}
			string Perm = RuleProvider.CurrentItem.Fields["Permissions"].GetFormattedValue();
			return (Perm[2] == '1');
		}

		public bool TestIfCanAdd(string ProjectName, string vgGrupoId, string vgPaginaNome)
		{
			RuleProvider.SelectRule(ProjectName, vgGrupoId, vgPaginaNome);
			if (RuleProvider.CurrentItem == null)
			{
				return true;
			}
			string Perm = RuleProvider.CurrentItem.Fields["Permissions"].GetFormattedValue();
			return (Perm[3] == '1');
		}

		public bool TestIfCanRemove(string ProjectName, string vgGrupoId, string vgPaginaNome)
		{
			RuleProvider.SelectRule(ProjectName, vgGrupoId, vgPaginaNome);
			if (RuleProvider.CurrentItem == null)
			{
				return true;
			}
			string Perm = RuleProvider.CurrentItem.Fields["Permissions"].GetFormattedValue();
			return (Perm[4] == '1');
		}

		public void InsertPerm(string vgGrupo, string vgPagina, string ViewChecked, string EditChecked, string AddChecked, string RemoveChecked, string ProjectName)
		{
			RuleProvider.SelectRule(ProjectName, vgGrupo, vgPagina);
			if (RuleProvider.CurrentItem == null)
			{
				LoginRuleItem item = new LoginRuleItem();
				item.Fields["Project"].SetValue(ProjectName);
				item.Fields["Group"].SetValue(vgGrupo);
				item.Fields["Object"].SetValue(vgPagina);
				item.Fields["Permissions"].SetValue("P" + ViewChecked + EditChecked + AddChecked + RemoveChecked);
				RuleProvider.DataProvider.InsertItem(item);
			}
			else
			{
				RuleProvider.CurrentItem.Fields["Permissions"].SetValue("P" + ViewChecked + EditChecked + AddChecked + RemoveChecked);
				RuleProvider.DataProvider.UpdateItem(RuleProvider.CurrentItem);
			}
		}

		public void ClearPermMenus(string Group, string Project)
		{
			LoginRuleItem item = new LoginRuleItem();
			item.Fields["Group"].SetValue(Group);
			item.Fields["Project"].SetValue(Project);
			RuleProvider.DataProvider.DeleteItem(item);
		}

		public void SaveMenuItem(string Group, string Object, bool Perm, string ProjectName)
		{
			RuleProvider.SelectRule(ProjectName, Group, Object);
			if (RuleProvider.CurrentItem == null)
			{
				LoginRuleItem item = new LoginRuleItem();
				item.Fields["Project"].SetValue(ProjectName);
				item.Fields["Group"].SetValue(Group);
				item.Fields["Object"].SetValue(Object);
				item.Fields["Permissions"].SetValue("M" + (Perm? "1":"0"));
				RuleProvider.DataProvider.InsertItem(item);
			}
			else
			{
				RuleProvider.CurrentItem.Fields["Permissions"].SetValue("M" + (Perm ? "1" : "0"));
				RuleProvider.DataProvider.UpdateItem(RuleProvider.CurrentItem);
			}
		}

		public bool checkMenuPerm(string Object, string Group, string ProjectName)
		{
			RuleProvider.SelectRule(ProjectName, Group, Object);
			if (RuleProvider.CurrentItem == null)
			{
				return true;
			}
			string Perm = RuleProvider.CurrentItem.Fields["Permissions"].GetFormattedValue();
			return (Perm[1] == '1');
		}
	}
} 
