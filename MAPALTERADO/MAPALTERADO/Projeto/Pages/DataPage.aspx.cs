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
using System.Configuration;
using System.Linq;
using PROJETO;
using COMPONENTS;
using COMPONENTS.Data;
using COMPONENTS.Configuration;
using COMPONENTS.Security;
using PROJETO.DataProviders;
using PROJETO.DataPages;
using Telerik.Web.UI;

namespace PROJETO.DataPages
{
	public partial class DataPage : GeneralDataPage
	{
		protected TB_LOGIN_GROUPPageProvider PageProvider;
	
		public string LOGIN_GROUP_NAMEField = "";
		public bool LOGIN_GROUP_IS_ADMINField = false;
		
		public override string FormID { get { return "29219"; } }
		public override string TableName { get { return "TB_LOGIN_GROUP"; } }
		public override string DatabaseName { get { return "22326MAKOTO"; } }
		public override string PageName { get { return "DataPage.aspx"; } }
		public override string ProjectID { get { return "30D047A8"; } }
		public override string TableParameters { get { return "false"; } }
		public override bool PageInsert { get { return false;}}
		public override bool CanEdit { get { return true && UpdateValidation(); }}
		public override bool CanInsert  { get { return true && InsertValidation(); } }
		public override bool CanDelete { get { return true && DeleteValidation(); } }
		public override bool CanView { get { return true; } }
		public override bool OpenInEditMode { get { return false; } }
		



		
		public override void CreateProvider()
		{
			PageProvider = new TB_LOGIN_GROUPPageProvider(this);
		}
		
		private void InitializePageContent()
		{
		}

		private void ShowMaps()
		{
			string ScriptMap1 = "";
			string Map1Address = "Brazil" + ", " + "DF" + ", " + "Aguas claras" + ", " + "QS 08 Conj 430B Casa 05" + ", " + "71975-185";
			ScriptMap1 += String.Format("setTimeout(\"codeAddress('Map1', google.maps.MapTypeId.HYBRID, '{0}', 17);\",100);", Map1Address);
			if (IsPostBack)
			{
				AjaxPanel.ResponseScripts.Add(ScriptMap1);
			}
			else
			{
				ClientScript.RegisterStartupScript(this.GetType(), "GoogleMapsInitialization", ScriptMap1, true);
			}
		}

		/// <summary>
        /// onInit Vamos Carregar o Painel de Ajax e Label de erros da página
        /// </summary>
		protected override void OnInit(EventArgs e)
		{
			AjaxPanel = MainAjaxPanel;
			if (IsPostBack)
			{
				AjaxPanel.ResponseScripts.Add("setTimeout(\"InitializeClient();\",100);");
			}
			if (!PageInsert )
				DisableEnableContros(false);

			base.OnInit(e);
		}
		

		/// <summary>
		/// Carrega os objetos de Item de acordo com os controles
		/// </summary>
		public override void UpdateItemFromControl(GeneralDataProviderItem  Item)
		{
			// só vamos permitir a carga dos itens de acordo com os controles de tela caso esteja ocorrendo
			// um postback pois em caso contrário a página está sendo aberta em modo de inclusão/edição
			// e dessa forma não teve alteração de usuário nos dados do formulário
			if (PageState != FormStateEnum.Navigation && this.IsPostBack)
			{
			}
			InitializeAlias(Item);
		}

		/// <summary>
		/// Carrega os objetos de tela para o Item Provider da página
		/// </summary>

		public override GeneralDataProviderItem LoadItemFromControl(bool EnableValidation)
		{
			GeneralDataProviderItem Item = PageProvider.GetDataProviderItem(DataProvider);
			if (PageState != FormStateEnum.Navigation)
			{
			}
			else
			{
				Item = PageProvider.MainProvider.DataProvider.SelectItem(PageNumber, FormPositioningEnum.Current);
			}
			if (EnableValidation)
			{
				InitializeAlias(Item);
				if (PageState == FormStateEnum.Insert)
				{
					FillAuxiliarTables();
				}
				PageProvider.Validate(Item); 
			}
			if (Item!=null) PageErrors.Add(Item.Errors);
			return Item;
		}
		

		/// <summary>
		/// Define a Máscara para cada campo na tela
		/// </summary>
		public override void DefineMask()
		{
		}

		public override void DefineStartScripts()
		{
		}
		
		public override void DisableEnableContros(bool Action)
		{
		}

		/// <summary>
		/// Limpa Campos na tela
		/// </summary>
		public override void ClearFields(bool ShouldClearFields)
		{
			if (ShouldClearFields)
			{
			}
			if (!PageInsert && PageState == FormStateEnum.Navigation)
				DisableEnableContros(false);				
			else
				DisableEnableContros(true);				
		}		

		public override void ShowInitialValues()
		{
		}

		public override void PageEdit()
		{
			DisableEnableContros(true); 
			base.PageEdit(); 
		}

		public override void ShowFormulas()
		{
		}
		
		/// <summary>
		/// Define conteudo dos objetos de Tela
		/// </summary>
		public override void DefinePageContent(GeneralDataProviderItem Item)
		{
			ShowMaps();
			InitializePageContent();
			base.DefinePageContent(Item);
		}
		
		/// <summary>
		/// Define apelidos da Página
		/// </summary>
		public override void InitializeAlias(GeneralDataProviderItem Item)
        {
			PageProvider.AliasVariables = new Dictionary<string, object>();
			PageProvider.AliasVariables.Clear();
			
			try
			{
				LOGIN_GROUP_NAMEField = Item["LOGIN_GROUP_NAME"].GetFormattedValue();
			}
			catch
			{
				LOGIN_GROUP_NAMEField = "";
			}
			try
			{
				LOGIN_GROUP_IS_ADMINField = Utility.StringConverterToBool(Item["LOGIN_GROUP_IS_ADMIN"].GetFormattedValue());
			}
			catch
			{
				LOGIN_GROUP_IS_ADMINField = false;
			}
			PageProvider.AliasVariables.Add("LOGIN_GROUP_NAMEField", LOGIN_GROUP_NAMEField);
			PageProvider.AliasVariables.Add("LOGIN_GROUP_IS_ADMINField", LOGIN_GROUP_IS_ADMINField);
			PageProvider.AliasVariables.Add("BasePage", this);
        }





		public override void ExecuteServerCommandRequest(string CommandName, string TargetName, string[] Parameters)
		{
			ExecuteLocalCommandRequest(CommandName, TargetName, Parameters);
		}		





	}
}
