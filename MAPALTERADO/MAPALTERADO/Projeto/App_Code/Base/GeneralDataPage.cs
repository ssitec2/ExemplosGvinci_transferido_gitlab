using System;
using System.Collections;
using System.Collections.Specialized;
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
using COMPONENTS;
using COMPONENTS.Data;
using COMPONENTS.Configuration;
using COMPONENTS.Security;
using System.Resources;
using PROJETO.DataProviders;
using System.IO.Compression;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using Telerik.Web.UI;

namespace PROJETO.DataPages
{
	public enum FormStateEnum 
	{ 
		Navigation, 
		Edit, 
		Insert 
	};

	public abstract class GeneralDataPage : Page, IGeneralDataProvider
	{

		private GeneralDataProvider _DataProvider;
		public GeneralDataProvider DataProvider
		{
			get { return _DataProvider; }
			set { _DataProvider = value; }
		}

		protected ResourceManager RM;
		protected FormSupportedOperations PageOperations;
		protected NameValueCollection PageErrors;
		public virtual bool ParentTryLogin { get { return true; } }

		private GeneralDataProviderItem _PageItem;
		public GeneralDataProviderItem PageItem
		{
			get
			{
				return _PageItem;
			}
			set
			{
				_PageItem = value;
			}
		}
		
		private DataAccessObject _dao;
		public DataAccessObject Dao
		{
			get
			{
				if (_dao == null)
					_dao = Settings.GetDataAccessObject(((Databases)HttpContext.Current.Application["Databases"])[DatabaseName]);

				return _dao;
			}

		}

		private RadAjaxPanel _AjaxPanel;
		public RadAjaxPanel AjaxPanel
		{
			get
			{
				return _AjaxPanel;
			}
			set
			{
				_AjaxPanel = value;
				_AjaxPanel.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(AjaxRequest);
			}
		}

		private Label _ErrorLabel;
        public Label ErrorLabel
        {
            get
            {
                return _ErrorLabel;
            }
            set
            {
                _ErrorLabel = value;
            }
        }
        
		private Hashtable _OldParameters;
		public Hashtable OldParameters
		{
			get
			{
				return _OldParameters;
			}
			set
			{
				_OldParameters = value;
			}
		}

		/// <summary>
		/// Guarda variavel de sessao da pagina do GRID.
		/// </summary>
		private string _GridSessionPageIndex = "";
		public string GridSessionPageIndex
		{
			get
			{
				return _GridSessionPageIndex;
			}
			set
			{
				_GridSessionPageIndex = value;
			}
		}

		/// <summary>
		/// Filtro da pagina
		/// </summary>
		public string sWhere
		{
			get
			{
				if (ViewState["sWhere"] == null)
					return "";
				else
					return (string)ViewState["sWhere"];
			}
			set 
			{
				ViewState["sWhere"] = value;
			}
		}

		/// <summary>
		/// Numero do registro em que o usuario esta
		/// </summary>
		public int PageNumber
		{
			get 
			{
				string txtPageNumber = Request.Form["__PAGENUMBER"];
				if (txtPageNumber == null)
				{
					return 0;
				}
				else
				{
					return Convert.ToInt32(txtPageNumber);
				}
			}
		}
		
		/// <summary>
		/// Numero de paginas do banco de dados
		/// </summary>
		public int PageCount
        {
            get
            {
                string txtPageNumber = Request.Form["__PAGECOUNT"];
                if (txtPageNumber == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(txtPageNumber);
                }
            }
        }
			
		public abstract string FormID { get; }
		public abstract string TableName { get; }
		public abstract string DatabaseName { get; }
		public abstract string PageName { get; }
		public abstract string ProjectID{ get; }
		public abstract void CreateProvider();

		public virtual bool AuthenticationRequired { get { return true; } }
		public virtual bool TryLoginOnParent{ get { return true; } }
		public virtual HtmlGenericControl PageHolder { get { return null; } }
		public virtual string TableParameters { get { return ""; } }
		public virtual bool PageInsert { get { return false; } }
		public virtual bool CanView { get { return true; } }
		public virtual bool CanInsert { get { return true; } }
		public virtual bool CanEdit { get { return true; } }
		public virtual bool CanDelete { get { return true; } }
		public virtual bool OpenInEditMode { get { return false; } }		
		
		public virtual void OnCommiting() { }
		public virtual void OnRollbacking() { }

		public void ApplyMasks(WebControl Txt)
		{
			if (Txt.Attributes["Mask"] != null)
			{
				if (!IsPostBack)
				{
					ClientScript.RegisterStartupScript(this.GetType(), "Mask" + Txt.ClientID, "<script>ApplyElementMask(document.getElementById('" + Txt.ClientID + "'));</script>");
				}
				else
				{
					AjaxPanel.ResponseScripts.Add("ApplyElementMask(document.getElementById('" + Txt.ClientID + "'));");
				}
			}
		}
		
		public virtual void DefineStartScripts()
		{
		}

		public virtual Hashtable DefineGridInsertValues(string GridID, Hashtable newValues)
		{
			return newValues;
		}

		public virtual void GridItemCreatedFinished(object sender, GridItemEventArgs e)
		{
		}

		public virtual void ExecuteServerCommandRequest(string CommandName, string TargetName, string[] Parameters)
		{
		}

		public virtual void ExecuteLocalCommandRequest(string CommandName, string TargetName, string[] Parameters)
		{
		}
		
		public virtual void DeleteChildItens()
		{
		}

		/// <summary>
		/// Coloca m??scara nos campos da p??gina
		/// </summary>
		public virtual void DefineMask()
		{
		}
		public virtual void DisableEnableContros(bool Action)
		{
		}
	
		/// <summary>
		/// Mostra os erros que aconteceram no acesso ao banco ou pelo menos em tentativas de acesso
		/// </summary>
		
		public virtual void ShowErrors()
		{
			string DefaultMessage = "";
			for (int i = 0; i < PageErrors.Count; i++)
			{
				switch (PageErrors.AllKeys[i])
				{
					default:
						if (DefaultMessage != "") DefaultMessage += "<br>";
						DefaultMessage += PageErrors[i];
						break;
				}
			}
			if (ErrorLabel != null)
			{
				ErrorLabel.Visible = true;
				ErrorLabel.Text = DefaultMessage;
			}
			else
			{
				if(!string.IsNullOrEmpty(DefaultMessage) && DefaultMessage != "\"\"")
				{
					AjaxPanel.ResponseScripts.Add(String.Format("alert('{0}');", DefaultMessage.Replace("'", "").Replace("\r\n", "\\n")));
				}
			}

		}

		/// <summary>
		/// Limpa os erros que aconteceram no acesso ao banco ou pelo menos em tentativas de acesso
		/// </summary>
		protected virtual void ClearErrors()
		{
			PageErrors.Clear();
			if(ErrorLabel != null)
				ErrorLabel.Text = "";
		}

		/// <summary>
		/// Limpa todos os campos para inserir outro ou algo do genero
		/// </summary>
		public void ClearFields()
		{
			ClearFields(true);
		}

        public virtual void ClearFields(bool ShouldClearFields)
		{
		}

		/// <summary>
		/// Limpa todos os campos para inserir outro ou algo do genero
		/// </summary>
		public virtual void DefinePageContent(GeneralDataProviderItem Item)
		{
			PageItem = Item;
		}

		public virtual void InitializeAlias(GeneralDataProviderItem Item)
		{
		}

		/// <summary>
		/// Carrega em um item os campos que foram escrito na tela
		/// </summary>
		/// <param name="item">item a ser preenchido</param>
		/// <param name="EnableValidation">se aceita valida??ao ou nao</param>
		public abstract GeneralDataProviderItem LoadItemFromControl(bool EnableValidation);

		public virtual GeneralDataProviderItem LoadItemFromGridControl(bool EnableValidation, string GridId)
		{
			return null;
		}

		public virtual GeneralDataProviderItem LoadItemFromSchedulerControl(bool EnableValidation, string SchedulerId)
		{
			return null;
		}

		public virtual GeneralDataProviderItem LoadItemFromImageGalleryControl(bool EnableValidation, string ImageGallery)
		{
			return null;
		}
		public virtual GeneralDataProviderItem LoadItemFromGanttControl(bool EnableValidation, string Gantt)
		{
			return null;
		}

		public virtual GeneralDataProviderItem LoadItemFromDependenciesGanttControl(bool EnableValidation, string Gantt)
		{
			return null;
		}

		public virtual void UpdateItemFromControl(GeneralDataProviderItem  Item)
		{
		}

		public virtual GeneralDataProviderItem GetDataProviderItem(GeneralDataProvider Provider)
		{
			if (DataProvider != null && DataProvider.PageProvider != null)
			{
				return DataProvider.PageProvider.GetDataProviderItem(Provider);
			}
			return null;
		}

		public void FillAuxiliarTables()
		{
			if (DataProvider != null && DataProvider.PageProvider != null)
			{
				DataProvider.PageProvider.FillAuxiliarTables();
			}
		}
		
		public void SetStateGrid()
		{
			StateGrid(PageState);
		}
		
		public void OnSelectedItem(GeneralDataProvider Provider, GeneralDataProviderItem Item, bool UpdateFromUI)
		{
			if (Provider == DataProvider)
			{
				if (UpdateFromUI) UpdateItemFromControl(Item);
				InitializeAlias(Item);
				FillAuxiliarTables();
				if (UpdateFromUI)
				{
					if (PageState == FormStateEnum.Insert)
					{
						ShowInitialValues();
					}
					ShowFormulas();
				}
			}
		}
		
		public Hashtable GetRegisterHashtable()
		{
			Hashtable gComponents = null;
			if (HttpContext.Current != null && HttpContext.Current.Items.Contains("gComponentsScripts"))
			{
				gComponents = (Hashtable)HttpContext.Current.Items["gComponentsScripts"];
			}
			else
			{
				gComponents = new Hashtable();
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Items["gComponentsScripts"] = gComponents;
				}
			}
			return gComponents;
		}

		public void RegisterClientScriptBlock(string scriptKey)
		{
			Hashtable gComponents = GetRegisterHashtable();
			gComponents.Add(scriptKey, true);
		}

		public virtual void SetParametersValues(GeneralDataProvider Provider)
		{
		}

		public virtual void ShowFormulas()
		{
		}
		
		public virtual void ShowInitialValues()
		{
		}
		
		public virtual void StateGrid(FormStateEnum State )
		{
		}

		public FormStateEnum PageState;

		public string Permission 
		{
			get
			{
				if(PageOperations != null)
				{
					return "View:" + PageOperations.AllowRead+";Edit:" + PageOperations.AllowUpdate+";Insert:"+PageOperations.AllowInsert+";Remove:"+PageOperations.AllowDelete+";";
				}
				return "";
			}
		}

		public GeneralDataPage()
		{
			_OldParameters = new Hashtable();
			PageErrors = new NameValueCollection();
		}

		// cria lista de par??metros vazia
		public void CreateEmptyParameters(GeneralDataProvider Provider)
		{
			if (Provider == DataProvider)
			{
				Provider.Parameters.Clear();
				Provider.CreateParameters();
			}
		}

		public void RegisterTelerikHiddenField(string name, string value)
		{
			if (AjaxPanel != null)
			{
				if (AjaxPanel.FindControl(name) == null)
				{
					AjaxPanel.Controls.Add(new Literal() { Text = string.Format("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", name, value), ID = name });
				}
				else
				{
					(AjaxPanel.FindControl(name) as Literal).Text = string.Format("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", name, value);
				}
			}
			else
			{
				ClientScript.RegisterHiddenField(name, value);
			}
		}

		/// <summary>
		/// Cria campos ocultos com informa????es sobre essa datapage usados para controle em javascript
		/// </summary>
		public void CreateHiddenFields()
		{
			RegisterTelerikHiddenField("__PAGENUMBER", DataProvider.PageNumber.ToString());
			RegisterTelerikHiddenField("__PAGECOUNT", DataProvider.PageCount.ToString());
			RegisterTelerikHiddenField("__TABLENAME", TableName);
			RegisterTelerikHiddenField("__DATABASENAME", DatabaseName);
			RegisterTelerikHiddenField("__PAGESTATE", PageState.ToString());
			RegisterTelerikHiddenField("__PAGENAME", PageName);
			RegisterTelerikHiddenField("__PERMISSION", Permission);
			RegisterTelerikHiddenField("__TABLEPARAMETER", TableParameters.ToString().ToLower());
			RegisterTelerikHiddenField("Targets", "");
		}

		/// <summary>
		/// L?? valores dos campos ocultos para vari??veis internas
		/// </summary>
		private void ReadHiddenFieldsValues()
		{
			PageState = GetPageState();
			DataProvider.PageNumber = PageNumber;
            DataProvider.PageCount = PageCount;
		}

		/// <summary>
		/// item antigo usado para fazer update depois dos campos serem modificados
		/// ?? preciso para poder identificar de qual registro se trata
		/// </summary>
		/// <param name="item">item a ser guardado como item antigo</param>
		public void SetOldParameters(GeneralDataProviderItem Item)
		{
			_OldParameters.Clear();
			if (Item != null)
			{
				foreach (string ParamKey in DataProvider.Parameters.Keys)
				{
					_OldParameters.Add(ParamKey, Item.Fields[ParamKey].Value);
					DataProvider.Parameters[ParamKey].Parameter.SetValue(Item.Fields[ParamKey].Value);
				}
			}
			ViewState["OldParameters"] = _OldParameters;
		}

		/// <summary>
		/// Estado da pagina
		/// </summary>
		private FormStateEnum GetPageState()
		{
			string txtPageState = Request.Form["__PAGESTATE"];
			if (txtPageState == null)
			{
				return FormStateEnum.Navigation;
			}
			else
			{
				return (FormStateEnum) System.Enum.Parse(typeof(FormStateEnum), txtPageState);
			}
		}

		public void GetParameters(bool KeepCurrentRecord, GeneralDataProvider Provider)
		{
			try
			{
				CreateEmptyParameters(Provider);
				if (KeepCurrentRecord)
				{
					if (ViewState["OldParameters"] != null && ((Hashtable)ViewState["OldParameters"]).Count > 0)
					{
						OldParameters = (Hashtable)ViewState["OldParameters"];
                        foreach (string ParamKey in Provider.Parameters.Keys)
						{
                            Provider.Parameters[ParamKey].Parameter.SetValue(OldParameters[ParamKey].ToString());
						}
					}
				}
				if (Provider.TableName == this.TableName)
					Provider.FiltroAtual = sWhere;
                Provider.InitialFilter = Provider.StartFilter;
			}
			catch (Exception e)
			{
				PageErrors.Add("Parameters", "Form parameters: " + e.Message);
			}
		}

		protected override object LoadPageStateFromPersistenceMedium()
		{
			return ViewStateManager.DecompressViewState(this);
		}

		protected override void SavePageStateToPersistenceMedium(object viewState)
		{
			ViewStateManager.CompressViewState(this, viewState);
		}

		/// <summary>
		/// primeira fun??ao a executar, usada na declara??ao de variaveis e outras coisas
		/// </summary>
		/// <param name="e">argumentos da OnInit</param>
		override protected void OnInit(EventArgs e)
		{
			RM = (System.Resources.ResourceManager)Application["rm"];

			//Vamos explicitar todos os eventos porque estamos usando a diretiva/propriedade AutoEventWireup=false
			this.Load += new System.EventHandler(this.PageLoad);
			base.OnInit(e);
			bool vgVisualiza = CanView;
			bool vgModifica = CanEdit;
			bool vgInclui = CanInsert;
			bool vgExclui = CanDelete;	

			PageCheckAuthentication();
            if (this.AuthenticationRequired)
            {
				vgVisualiza &= Utility.CheckAllowView(this);
				vgModifica &= Utility.CheckAllowEdit(this);
				vgInclui &= Utility.CheckAllowAdd(this);
				vgExclui &= Utility.CheckAllowRemove(this);
				if (Utility.CheckAllowView(this) == false)
				{
					throw new Exception("Acesso negado!");
				}
			}
			PageOperations = new FormSupportedOperations(false, vgVisualiza, vgInclui, vgModifica, vgExclui);
			
			CreateProvider();
			
			this.PreRender += new EventHandler(GeneralDataPage_PreRender);
		}
		
		void GeneralDataPage_PreRender(object sender, EventArgs e)
		{
			if (PageErrors.Count > 0)
			{
				ShowErrors();
			}
		}

		protected override void InitializeCulture()
		{
			Utility.SetThreadCulture();
		}
        
		public virtual void setGridPerm()
		{
		}
		public virtual GeneralGridProvider GetGridProvider(RadGrid Grid)
        {
            return null;
        }
        public virtual void SetStartFilter()
        {
        }

		public virtual bool InsertValidation()
		{
			return true;
		}

		public virtual bool UpdateValidation()
		{
			return true;
		}

		public virtual bool DeleteValidation()
		{
			return true;
		}

		public virtual string GetStartFilterPosition()
        {
			return "";
        }
        
		/// <summary>
		/// Page load da pagina, aqui se faz tudo o que deve ser feito quando a pagina carrega ou quando ocorre um post
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PageLoad(object sender, System.EventArgs e)
		{
			DefineMask();          // faz defini????o de m??scara 
			
			DefineStartScripts();   // faz defini????o de scripts gerais para a p??gina
		
			ClearErrors();
			if(AjaxPanel != null)
			{ 			
				Page.ClientScript.RegisterStartupScript(GetType(), "Ajax", String.Format("<script>SetAjaxPanel('{0}');</script>", AjaxPanel.ClientID));
			}	
			if (ErrorLabel != null)
			{
				Page.ClientScript.RegisterStartupScript(GetType(), "ErrorLabel", String.Format("<script>SetErrorLabel('{0}');</script>", ErrorLabel.ClientID));
			}
			InitializeAlias(DataProvider.Item);
			FillAuxiliarTables();
			SetStartFilter();
			if (!this.IsPostBack)
			{
				string session = HttpContext.Current.Request["sessionfilter"];
				if (!string.IsNullOrEmpty(session))
				{
					sWhere = Session[session].ToString();
					Session[session] = "";
				}
				if (!OpenInEditMode)
				{
					PageState = FormStateEnum.Navigation;
					PageShow(FormPositioningEnum.First);
				}
				else
				{
					PageEdit();
				}
			}
			else
			{
				ReadHiddenFieldsValues();

				if (this.Request["__EVENTTARGET"] != AjaxPanel.ClientID)
				{
					if (PageState != FormStateEnum.Insert)
					{
				    		DataProvider.SelectItem(PageNumber, FormPositioningEnum.None);
					
						if (DataProvider.PageNumber == 0)
						{
							InitializeAlias(DataProvider.Item);
							FillAuxiliarTables();
							ShowFormulas();
						}
					}
					else
					{
					    DataProvider.Item = LoadItemFromControl(false);
					    InitializeAlias(DataProvider.Item);
					    FillAuxiliarTables();
						ShowFormulas();
					}
				}
				CreateHiddenFields();
			}

		}
		
		/// <summary>
		/// Executa o comando solicitado
		/// </summary>
		/// <param name="CommandName">Identificador do comando a executar</param>
		public void ExecuteCommandRequest(string CommandName, string TargetName, string[] Parameters)
		{
			switch (CommandName.ToUpper())
			{
				case "FILTER":
                    PageFilter();
                    break;
				case "NAVIGATE":
					PageNavigate(Parameters);
					break;
				case "NEW":
					PagePrepareInsert();
					break;
				case "NEW_COPY":
					PagePrepareInsertCopy();
					break;
				case "SAVE":
					PageSave();
					break;
				case "VALIDATE":
					PageValidate();
					break;
				case "REMOVE":
					if (PageState == FormStateEnum.Insert)
						PageCancel();
					else
						PageDelete();
					break;
				case "CANCEL":
					PageCancel();
					break;
				case "REFRESH":
                    PageShow(FormPositioningEnum.Current);
                    break;
				case "SETFILTER":
					sWhere = Parameters.Length > 0 ? Parameters[0]: "";
					ClearFields();
					ShowInitialValues();
					ShowFormulas(); 
					PageShow(FormPositioningEnum.First);
					break;
				case "FIND":
					break;
				case "EDIT":
					PageEdit();
					break;
				case "FIRST":
					PageMoveFirst();
					break;
				case "PREVIOUS":
					PageMovePrevious();
					break;
				case "NEXT":
					PageMoveNext();
					break;
				case "LAST":
					PageMoveLast();
					break;
				case "DBINFO":
					break;
				case "SHOWFORMULAS":
					PageShowFormulas();
					break;
				case "LOGOFF":
					SessionClose();
					break;
				case "REMOVELISTITEM":
                    DeleteListItem(TargetName);
					break;
				case "REFRESHGRID":
                    RefreshGrid(TargetName, Parameters);
                    break;
				default:
					if (PageState != FormStateEnum.Insert)
						DataProvider.SelectItem(PageNumber, FormPositioningEnum.None);
					else
					{
						DataProvider.Item = LoadItemFromControl(false);
						InitializeAlias(DataProvider.Item);
						FillAuxiliarTables();
					}
					ExecuteServerCommandRequest(CommandName, TargetName, Parameters);
					break;
				}
		}

		public string FullyQualifiedApplicationPath
        {
            get
            {     
                string appPath = null;
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    appPath = string.Format("{0}://{1}{2}{3}", context.Request.Url.Scheme, context.Request.Url.Host, context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port, context.Request.ApplicationPath);
                }
                if (!appPath.EndsWith("/"))
                    appPath += "/";
                return appPath;
            }
        } 

		public void PageFilter()
		{
            AjaxPanel.ResponseScripts.Add(String.Format("showFilter('{0}');", FullyQualifiedApplicationPath));
		}
		
		public void PageNavigate(string[] Parameters)
		{
			string Url = "";
			if (Parameters.Length > 0)
			{
				Url = Parameters[0];
				if (Parameters.Length > 1)
				{
					string WindowSetting = Parameters[1];
					if (WindowSetting == "true")
					{
						AjaxPanel.ResponseScripts.Add("Gasopen(\"" + Url + "\");");
						return;
					}
				}
				HttpContext.Current.Response.Redirect(Url, false);
			}
		}
		
		/// <summary>
		/// prepara para um futuro insert.
		/// coloca o FormStateEnum como Insert e limpa os campos
		/// </summary>
		public virtual void PagePrepareInsert()
		{
			if (PageState == FormStateEnum.Navigation)
			{
				PageState = FormStateEnum.Insert;
				PageShow(FormPositioningEnum.None);
			}
		}

		public virtual void PagePrepareInsertCopy()
		{
			if (PageState == FormStateEnum.Navigation)
			{
				PageState = FormStateEnum.Insert;
				PageShow(FormPositioningEnum.None, false);
			}
		}
		
		public virtual void PageEdit()
		{
			if (PageState == FormStateEnum.Navigation)
			{
				PageState = FormStateEnum.Edit;
				PageShow(FormPositioningEnum.None);
			}
		}
		
		public virtual void RefreshParentGrids()
		{

		}

		public virtual void RefreshOnNavigation()
		{

		}

				/// <summary>
		/// Move para o ultimo registro da tabela de acordo com o order by
		/// </summary>
		private void PageMoveLast()
		{
			if (PageState == FormStateEnum.Navigation)
			{
				RefreshOnNavigation();
				RefreshParentGrids();
				PageShow(FormPositioningEnum.Last);
			}
		}

		/// <summary>
		/// Vai para o proximo registro da tabela de acordo com o order by
		/// </summary>
		private void PageMoveNext()
		{
			if (PageState == FormStateEnum.Navigation)
			{
				RefreshOnNavigation();
				RefreshParentGrids();
				PageShow(FormPositioningEnum.Next);
			}
		} 

		/// <summary>
		/// Move um registro para tras da tabela de acordo com o order by
		/// </summary>
		private void PageMovePrevious()
		{
			if (PageState == FormStateEnum.Navigation)
			{
				RefreshOnNavigation();
				RefreshParentGrids();
				PageShow(FormPositioningEnum.Previous);
			}
		}

		/// <summary>
		/// Move para o primeiro registro da tabela de acordo com o order by
		/// </summary>
		private void PageMoveFirst()
		{
			if (PageState == FormStateEnum.Navigation)
			{
				RefreshOnNavigation();
				RefreshParentGrids();
				PageShow(FormPositioningEnum.First);
			}
		}
		
		/// <summary>
		/// Destroi a Sessao do usuario
		/// </summary>
		private void SessionClose()
		{
			FormsAuthentication.SignOut();
			Session.Clear();
			Response.Redirect(Utility.StartPageName);
		}

		/// <summary>
		/// Carrega a pagina com os dados necess??rios
		/// </summary>
		protected void PageShow(FormPositioningEnum Positioning)
		{
			PageShow(Positioning, true);
		}
		
		protected void PageShow(FormPositioningEnum Positioning, bool ShouldClearFields)
		{
			PageShow(Positioning, ShouldClearFields, true);
		}

		public GeneralDataProviderItem GetCurrentItem(FormPositioningEnum Positioning, bool UpdateFromUI)
		{
			GeneralDataProviderItem Item;
			if (!IsPostBack && GetStartFilterPosition() != "")
			{
				DataProvider.FiltroAtual = DataProvider.StartFilter;
				DataSet ds = DataProvider.SelectAllItems();
				DataRow[] drs = ds.Tables[0].Select(GetStartFilterPosition().Replace("N'","'"));
				if (drs.Length > 0)
				{
					DataProvider.PageNumber = ds.Tables[0].Rows.IndexOf(drs[0]) + 1;
					Item = DataProvider.SelectItem(true, DataProvider.PageNumber, FormPositioningEnum.None, UpdateFromUI);
				}
				else
				{
					Item = DataProvider.SelectItem(true, PageNumber, Positioning, UpdateFromUI);
				}
			}
			else
			{
				Item = DataProvider.SelectItem(true, PageNumber, Positioning, UpdateFromUI);
			}
			return Item;
		}

		protected void PageShow(FormPositioningEnum Positioning, bool ShouldClearFields, bool RebindGrid)
		{
			PageShow(Positioning, ShouldClearFields,RebindGrid, true);
		}
		
		protected void PageShow(FormPositioningEnum Positioning, bool ShouldClearFields, bool RebindGrid, bool UpdateFromUI)
		{
			if (PageState == FormStateEnum.Navigation && CanView)
			{
				DisableEnableContros(false); // Habilita Enable ou Disable dos controles da tela
			}
			if (PageOperations.None)
			{
				if (PageHolder != null)
				{
					PageHolder.Visible = false;
				}
				DefinePageContent(null);
				InitializeAlias(null);
				FillAuxiliarTables();
				ShowFormulas();
				return;
			}
			bool IsInsertig = (PageState == FormStateEnum.Insert);

			GeneralDataProviderItem Item;
			if (!IsInsertig)
			{
				Item = GetCurrentItem(Positioning, UpdateFromUI);

				if(Item == null && OpenInEditMode && !IsPostBack && PageState == FormStateEnum.Edit && CanView)
				{
					PageState = FormStateEnum.Navigation;
					PageShow(FormPositioningEnum.First);
					return;
				}
				
				// Para tabelas que sejam do tipo "Par??metros" vamos verificar se existe um registro.
				// Caso n??o exista nenhum registro, iremos inserir um com valores default.
				if (TableParameters.ToString().ToLower().Equals("true") && Item == null)
				{
					Item = GetDataProviderItem(DataProvider);
					foreach (FieldBase Field in Item.Fields.Values)
					{
						if (Field is IntegerField || Field is LongField || Field is DecimalField)
						{
							Field.Value = 0;
						}
						if (Field is TextField || Field is DateField || Field is MemoField)
						{
							Field.Value = " ";
						}
						if (Field is BooleanField)
						{
							Field.Value = false;
						}
						if (Field is DateField)
						{
							Field.Value = new DateTime(1901,01,01);
						}
					}
					DataProvider.InsertItem(Item);
					Item = DataProvider.SelectItem(PageNumber, Positioning);
				}
				
				SetOldParameters(Item);
				
				DefinePageContent(Item);

				if (Item != null && PageOperations.AllowRead)
				{
					if(!CanView) 
					{
						ClearFields();
						ShowInitialValues();
						ShowFormulas();
					}
				}
				else
				{
					ClearFields();
					InitializeAlias(Item);
					FillAuxiliarTables();
					ShowInitialValues(); 
					ShowFormulas();
				}
			}

			if (IsInsertig || (DataProvider.PageCount == 0 && sWhere == "" && PageInsert) || (!CanView && PageInsert))
			{
				PageState = FormStateEnum.Insert;
				DataProvider.PageNumber = PageNumber;				// guarda o n??mero do registro para, caso cancelar a inclus??o, recuperar depois
				Item = GetDataProviderItem(DataProvider);
				ClearFields(ShouldClearFields);
				 
				//vamos reposicionar o item para atualizar formulas, etc etc etc etc etc ...
				DataProvider.LocateRecord(Item); 

			}

			if (DataProvider.PageCount > 0 || PageState == FormStateEnum.Insert)
			{
				SetStateGrid();  
			}

			CreateHiddenFields();
			
			if (RebindGrid) GridRebind();

			if (Positioning != FormPositioningEnum.Current && Positioning != FormPositioningEnum.None) 
			{
				if (IsPostBack)
				{
					ResetDataList();
					ResetRepeaterList();
				}
			}
		}
		
		public virtual void ResetDataList()
		{

		}
		
		public virtual void ResetRepeaterList()
		{

		}

		public virtual void GridRebind()
		{

		}

		/// <summary>
		/// Executa valida????o no item que est?? na sendo mostrado
		/// </summary>
		public virtual bool PageValidate()
		{
			bool IsInsertig = (PageState == FormStateEnum.Insert);
			GeneralDataProviderItem Item = DataProvider.Item;
			if (IsInsertig)
			{
				Item = LoadItemFromControl(true);
			}
			else
			{
				Item = DataProvider.SelectItem(PageNumber, FormPositioningEnum.Current);
				DataProvider.PageProvider.Validate(Item);
				PageErrors.Add(Item.Errors);
			}
			
			// vamos definir o PageItem corrente para ser utilizado em rotinas como PageSave
			PageItem = Item;

			if (PageErrors.Count == 0)
			{
				ValidationSucceeded(Item);
			}
			else
			{
				ValidationFailed(Item);
			}
			return (PageErrors.Count == 0);
		}

		/// <summary>
		/// Executa update no item que esta na sendo mostrado
		/// </summary>
		public virtual GeneralDataProviderItem PageSave()
		{
			if (PageState == FormStateEnum.Navigation) return PageItem;

			bool IsInsertig = (PageState == FormStateEnum.Insert);

			// faz a valida????o da p??gina
			PageValidate();
			
			if ((IsInsertig && PageOperations.AllowInsert) || (!IsInsertig && PageOperations.AllowUpdate))
			{

				// enche item com dados do form fazendo valida????o
				PageItem.IsNew = IsInsertig;
		
				if (PageErrors.Count == 0)
				{
					try
					{
						FormPositioningEnum NewPosition;
						if (IsInsertig)
						{
							// executa inclus??o do registro
							DataProvider.InsertItem(PageItem);

							if (PageErrors.Count == 0)
							{
								DataProvider.PageProvider.GetTableIdentity();
							}
							
							// vamos sempre deixar PageNumber no ??ltimo registro, ap??s inclus??o
							NewPosition = FormPositioningEnum.Last;
						}
						else
						{
							// executa update do registro
							DataProvider.UpdateItem(PageItem);

							// vamos sempre manter o valor de PageNumber, ap??s altera????o
							NewPosition = FormPositioningEnum.Current;
						}

						PageItem = DataProvider.Item;

						// seta chave para reposicionar o registro
						SetOldParameters(PageItem);

						InitializeAlias(PageItem);

						// dispara eventos de sucesso ou falha
						if(PageErrors.Count == 0)
						{
							SaveSucceeded(PageItem);
						}
						else
						{
							SaveFailed(PageItem);
						}

						// atualiza o form
						PageState = FormStateEnum.Navigation;
						PageShow(NewPosition);

					}
					catch (Exception ex)
					{
						PageErrors.Add("DataProvider", ex.Message);
						SaveFailed(PageItem);
					}
				}
			}
			return PageItem;
		}

		public virtual void SaveFailed(GeneralDataProviderItem Item)
		{
		}

		public virtual void SaveSucceeded(GeneralDataProviderItem Item)
		{
		}

		public virtual void ValidationSucceeded(GeneralDataProviderItem Item)
		{
		}

		public virtual void ValidationFailed(GeneralDataProviderItem Item)
		{
		}

		/// <summary>
		/// Executa Delete no item que esta na sendo mostrado
		/// </summary>
		protected virtual int PageDelete()
		{
			if (PageOperations.AllowDelete)
			{
				int RetVal = 0;
				FormStateEnum OldState = PageState;
				// enche item com dados do form sem fazer valida????o
				GeneralDataProviderItem Item = GetCurrentItem(FormPositioningEnum.Current, false);
				Item.IsNew = false;
				Item.IsDeleted = true;

				if (PageErrors.Count == 0)
				{
					try
					{
						OldState = PageState;
						PageState = FormStateEnum.Navigation;
						DeleteChildItens();  
						if (PageErrors.Count != 0)
						{
							return RetVal;
						}					
						RetVal = DataProvider.DeleteItem(Item);
						PageShow(FormPositioningEnum.None);
						if (RetVal > 0)
						{
							OnRemoveSucceeded(Item);
						}
						else
						{
							PageState = OldState;
						}
					}
					catch (Exception ex)
					{
						PageState = OldState;
						PageErrors.Add("DataProvider", ex.Message);
					}
				}
				return RetVal;
			}
			return 0;
		}

		public virtual void OnRemoveSucceeded(GeneralDataProviderItem Item)
		{
		
		}

		public virtual void DeleteListItem(string ControlName)
        {

        }

		/// <summary>
		/// Cancela a execu??ao de um update ou insert
		/// </summary>
		public virtual void PageCancel()
		{
			PageState = FormStateEnum.Navigation;
			PageShow(FormPositioningEnum.Current);
		}

		public virtual void RefreshGrid(string TargetName, string[] Parameters)
        {

        }

		public virtual void PageShowFormulas()
		{
			if (PageState == FormStateEnum.Navigation)
			{
				LoadItemFromControl(false);
			}
			else
			{
				DataProvider.LocateRecord(LoadItemFromControl(false));  
			}
		}

		/// <summary>
		/// testa se esta logado e se nao estiver manda para a pagina de login
		/// </summary>
		private void PageCheckAuthentication()
		{
			if (this.AuthenticationRequired)
			{
				Utility.CheckAuthentication(this,TryLoginOnParent);
			}
		}

		/// <summary>
		/// Direciona o click da toolbar ou do filtro de acordo com o que foi passado no event argument
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e">argumentos do evento</param>
		public string CommandName = "";
		public string SenderControlAlias = "";
		protected void AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
		{
			string Arguments = Request.Params.Get("__EVENTARGUMENT");

			if (Arguments.IndexOf("ExecuteCommand:") != -1)
			{

				string[] Parameters = Arguments.Split('|');
				string[] CommandParameters = new string[] { };

				for (int i = 0; i < Parameters.Length; i++)
				{
					string[] Args = new string[2];
					string ParamName = "";
					string Param = "";
					int Div = Parameters[i].IndexOf(':');
					if (Div >= 0)
					{
						ParamName = Parameters[i].Substring(0, Div);
						Param = Parameters[i].Substring(Div + 1);
					}
					switch (ParamName)
					{
						case "ExecuteCommand":
							CommandName = Param;
							break;
						case "TargetControl":
							SenderControlAlias = Param;
							break;
						case "Parameters":
							CommandParameters = Param.Split('#');
							break;
					}
				}
				ExecuteCommandRequest(CommandName, SenderControlAlias, CommandParameters);
				// n??o vamos deixar continuar se houver algum erro
				if (PageErrors.Count == 0)
				{
					foreach (string Str in CommandParameters)
					{
						if (Str.StartsWith("continue:"))
						{
							string function = Str.Substring(9);
							if (!function.EndsWith(")"))
                            {
                                function += "()";
                            }
							AjaxPanel.ResponseScripts.Add("setTimeout(\"" + function + "\",1);");
							break;
						}
					}
				}
			}
		}
	}

}
