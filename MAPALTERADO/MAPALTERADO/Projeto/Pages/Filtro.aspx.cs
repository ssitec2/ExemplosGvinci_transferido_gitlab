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
using System.Xml;
using Telerik.Web.UI;

namespace PROJETO
{
	/// <summary>
	/// Code behind do web control de filtro
	/// </summary>
	public partial class Filtro: System.Web.UI.Page
	{
		string TableName = "";
		string DatabaseName = "";
		string PageName = "";
		protected string FieldDataCalendName;
		protected string FieldDataCalendDateControl;
		public string cboCampoField
		{
			get
			{
				return cboField.SelectedValue;
			}
		}
		
		public string cboOperadorField
		{
			get
			{
				return cboOperator.SelectedValue;
			}
		}
		
		/// <summary>
		/// Expressão de filtro que será passada para a página que requisitou o filtro
		/// </summary>
		public string FinalFilter
		{
			get
			{
				if (ViewState["FiltroFinal"] == null)
				{
					return "";
				}
				else
				{
					return (string)ViewState["FiltroFinal"];
				}
			}
			set
			{
				ViewState["FiltroFinal"] = value;
			}
		}

		/// <summary>
		/// Primeira função a ser executada
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			if (!IsPostBack)
			{
			}
		}
		
		protected override void InitializeCulture()
		{
			Utility.SetThreadCulture();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			TableName = HttpContext.Current.Request["TableName"];
			DatabaseName = HttpContext.Current.Request["DatabaseName"];
			PageName = HttpContext.Current.Request["PageName"];
			ClientScript.RegisterHiddenField("__PAGENAME", PageName);
			Utility.CheckAuthentication(this,true);

			if (!IsPostBack)
			{
				rdAnd.Checked = true;
				FillOperatorsCombo();//Preenchemos o combo com os devidos operadores
				FillFieldsCombo();//Preenchemos a combo com os campos
				if(string.IsNullOrEmpty(PageName))
					ClientScript.RegisterStartupScript(this.GetType(), "SetExpressionField", "<script>Load();</script>");
			}
			cboField.AutoPostBack = true;
			cboField.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboField_SelectedIndexChanged);
			FilterFieldsOperators(cboField.SelectedValue);
			ClientScript.RegisterClientScriptBlock(this.GetType(), "SetExpressionField", "<script>var ExpressionFieldName = \"" + txtExpression.ClientID + "\";</script>");
		}
		
		/// <summary>
		/// Seta a visibilidade dos botões
		/// </summary>
		/// <param name="e"></param>
		public void SetButtonVisibility(bool vgVisible)
		{
			butExecuteFilter.Visible = vgVisible;
			butClearFilter.Visible = vgVisible;
		}
		
		/// <summary>
		/// Carrega todos os devidos operadores na combo
		/// </summary>
		public void FillOperatorsCombo()
		{
			cboOperator.Items.Clear(); 
			string[] Operators = new string[] {"=", "<>" , "<" , ">" , "<=" , ">=" , "LIKE"};
			foreach (string Operator in Operators)
			{
				RadComboBoxItem vglist = new RadComboBoxItem(Operator, Operator);
				cboOperator.Items.Add(vglist);
			}
		}
	  
		/// <summary>
		/// Preenche a combo de campos com os devidos campos da tabela que será filtrada
		/// </summary>
		public void FillFieldsCombo()
		{
			RadComboBoxItem vgList;
			XmlDocument vgDoc = new XmlDocument();
			vgDoc.Load(Server.MapPath("../Databases/" + DatabaseName + ".xml"));

			XmlNode vgCampo = FindFieldByName(vgDoc);
			foreach (XmlNode vgNode in vgCampo.ChildNodes)
			{
				if (vgNode.Name == "FIELD" && vgNode.Attributes.GetNamedItem("TYPE").Value.ToLower() != "image")
				{
					vgList = new RadComboBoxItem(vgNode.Attributes.GetNamedItem("TITLE").Value);
					vgList.Value = vgNode.Attributes.GetNamedItem("NAME").Value+"@"+vgNode.Attributes.GetNamedItem("TYPE").Value;
					
					if (vgNode.Attributes.GetNamedItem("MASK") != null)
					{
						vgList.Value += "|" + vgNode.Attributes.GetNamedItem("MASK").Value;
					}
					else if (vgNode.Attributes.GetNamedItem("SIZE") != null)
					{
						vgList.Value += "-" + vgNode.Attributes.GetNamedItem("SIZE").Value;
					}
					cboField.Items.Add(vgList);
				}
			}
			if(cboField.Items.Count > 0)
				cboField.SelectedIndex = 0;
		}
	  
		/// <summary>
		/// Encontra o campo de acordo do nome da tabela
		/// </summary>
		/// <param name="vgDoc"></param>
		/// <returns></returns>
		public XmlNode FindFieldByName(XmlDocument vgDoc)
		{
			foreach (XmlNode xmlNode in vgDoc.FirstChild.NextSibling.FirstChild.ChildNodes)
			{
				if (xmlNode.Attributes.GetNamedItem("NAME").Value.ToLower() == TableName.ToLower())
				{
					return xmlNode;
				}
			}
			return null;
		}	
		
		/// <summary>
		/// Executa a função quando houver mudança do campo a ser filtrado
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void cboField_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
		{
			txtValue.Style.Add("text-align", "left");
			txtValue.Text = "";
			FilterFieldsOperators(cboField.SelectedValue);
		}

		/// <summary>
		/// Testa se os operadores são necessarios na combo, caso
		/// não seja, este será removido
		/// Exemplo: Caso estejamos filtrando um campo do tipo lógico ( boolean ), não será preciso operadores como por 
		//  exemplo 'LIKE', ou '<', logo, estes serão removidos
		/// </summary>
		/// <param name="vgTipo"></param>
		public void FilterFieldsOperators(string vgTipo)
		{
			FillOperatorsCombo();
			string[] vgFieldAttributes = vgTipo.Split(new char[]{'@'},2);
			string vgMascara = "@X";
			int vgSize = 100;
			if (vgFieldAttributes[1].IndexOf("|") != -1)
			{
				vgMascara = vgFieldAttributes[1].Substring(vgFieldAttributes[1].IndexOf("|") + 1);
			}
			else if (vgFieldAttributes[1].IndexOf("-") != -1)
			{
				vgSize = Convert.ToInt32(vgFieldAttributes[1].Substring(vgFieldAttributes[1].IndexOf("-") + 1));
			}
			txtValue.Visible = true;
			FieldDate.Style["Display"] = "none";
																	
			if (vgFieldAttributes[1].IndexOf("|") != -1)
			{
				vgFieldAttributes[1] = vgFieldAttributes[1].Substring(0, vgFieldAttributes[1].IndexOf("|"));
			}
			else if (vgFieldAttributes[1].IndexOf("-") != -1)
			{
				vgFieldAttributes[1] = vgFieldAttributes[1].Substring(0, vgFieldAttributes[1].IndexOf("-"));
			}
			switch (vgFieldAttributes[1].ToLower())
			{
				case "boolean":
					RadComboBoxItem vgList;
					Mask.SetMask(txtValue, "9", 1, false);
					for (int i = 0; i < cboOperator.Items.Count; i++)
					{
						vgList = cboOperator.Items[i];
						if (vgList.Text == ">" || vgList.Text == ">=" || vgList.Text == "<" || vgList.Text == "<=" || vgList.Text == "LIKE")
						{
							cboOperator.Items.Remove(vgList);
							i--;
						}
					}
				break;
				case "date":
				case "datetime":
					FieldDate.Style["Display"] = "inline";
					txtValue.Visible = false;
			if (vgMascara.StartsWith("@"))
				Mask.SetMask(FieldDate.DateInput, vgMascara, 32665, false);
			else
				Mask.SetMask(FieldDate.DateInput, vgMascara, vgMascara.Length, false);
				break;
				case "number":
					if (vgMascara.StartsWith("@"))				
						Mask.SetMask(txtValue, vgMascara, 32665, true);
					else
						Mask.SetMask(txtValue, vgMascara, vgMascara.Length, true);
					txtValue.Style.Add("text-align", "right");
				break;
				default:
					if (vgMascara.StartsWith("@"))				
						Mask.SetMask(txtValue, vgMascara, vgSize, false);
					else
						Mask.SetMask(txtValue, vgMascara, vgMascara.Length, false);
					cboOperator.Items.Clear();
					FillOperatorsCombo();
				break;
			}
		}
	
		private DataAccessObject _Dao;
		public DataAccessObject Dao
		{
			get
			{
				if (_Dao == null) _Dao = Settings.GetDataAccessObject(((Databases)HttpContext.Current.Application["Databases"])[DatabaseName]);
				return _Dao;
			}
			set
			{
				_Dao = value;
			}
		}
	
		protected void ExecFilter()
		{
			bool isOk = true;
			if (!string.IsNullOrEmpty(txtExpression.Text.TrimStart()))
			{
				try
				{
					Dao.RunSql("select * from [" + TableName + "] where " + txtExpression.Text.TrimStart());
				}
				catch (Exception e)
				{
					isOk = false;
				}
			}
			if (isOk)
			{
				Random Rnd = new Random();
				string sessionFilter = "ActualFilter" + Rnd.Next(10000).ToString();
				Session[sessionFilter] = txtExpression.Text.TrimStart();
				//ClientScript.RegisterHiddenField("__SESSIONFILTER", sessionFilter);
				ajxMainAjaxPanel.Controls.Add(new Literal() { Text = string.Format("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", "__SESSIONFILTER", sessionFilter), ID = "__SESSIONFILTER" });
			ajxMainAjaxPanel.ResponseScripts.Add("ExecFilter();");  
			}
			else
			{
			ajxMainAjaxPanel.ResponseScripts.Add("alert('Expressão de filtro está errada.');");
			}
		}
	
		/// <summary>
		/// Executa o filtro
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Concatenate()
		{		
			if (cboField.SelectedValue.ToLower().IndexOf("data") == -1)
			{
				txtValue.Text = txtValue.Text;
			}
			if ((txtValue.Text == "" && FieldDate.SelectedDate != null && FieldDate.SelectedDate.HasValue && FieldDate.SelectedDate.Value.ToString() == "")|| cboField.Text == "" ||  cboOperator.Text == "")
			{
				return;
			}
			if (txtExpression.Text.Trim() == "")
			{
				txtExpression.Text = "(" + GetFilter(false) + ")";
				FinalFilter = "(" + GetFilter(true) + ")";
			}
			else
			{
				if (rdAnd.Checked == true)
				{
					txtExpression.Text += " AND (" + GetFilter(false) + ")";
					FinalFilter += " AND (" + GetFilter(true) + ")";
				}
				if (rdOr.Checked == true)
				{
					txtExpression.Text = txtExpression.Text.Substring(0, txtExpression.Text.Length - 1) + " OR " + GetFilter(false) + ")";
					FinalFilter = FinalFilter + " OR (" + GetFilter(true) + ")";
				}
			}		
			txtValue.Text = "";
			cboOperator.Text = "";
			cboField.Text = "";
			FieldDate.SelectedDate = null;
		}
		
		/// <summary>
		/// Pega a data do filtro
		/// </summary>
		/// <returns></returns>
		public string GetDate()
		{
			string s="";
			s = cboField.Text;
			s += " " + cboOperator.Text;
			s += " format('" + txtValue.Text + "', '" + cboField.SelectedValue.Substring(cboField.SelectedValue.IndexOf("|") + 1) + "'";
			return s;
		}

		/// <summary>
		/// Pega o filtro que ja tinha sido feito antes
		/// </summary>
		/// <returns>Filtro que ja estava pronto</returns>
		public string GetFilter(bool ToSqlExpression)
		{
			string s="";
			string retVal = "";
			if (ToSqlExpression && cboField.SelectedValue.ToLower().IndexOf("data") != -1)
			{
				s = cboField.Text;
				s += " " + cboOperator.Text;
			}
			else
			{
				s = "[" + cboField.SelectedValue.Split('@')[0].ToString() + "]";
				s += " " + cboOperator.Text;
				string delimit = "";
				retVal = txtValue.Text;
				if (cboField.SelectedValue.ToLower().IndexOf("text") != -1) delimit = "'";
				else if (cboField.SelectedValue.ToLower().IndexOf("number") != -1)
				{
					delimit = "";
					retVal = retVal.Replace(".", "").Replace(",", ".");
				}
				else if (cboField.SelectedValue.ToLower().IndexOf("datetime") != -1 || cboField.SelectedValue.ToLower().IndexOf("date") != -1)
				{
					delimit = "'";
					if (FieldDate.SelectedDate != null && FieldDate.SelectedDate.HasValue)
					{
						retVal = FieldDate.SelectedDate.Value.ToString("yyyy-MM-dd");
					}
					else
					{
						retVal = "";
					}
				}
				else if (cboField.SelectedValue.ToLower().IndexOf("memo") != -1) delimit = "'";
				s += " " + delimit + retVal.ToString() + delimit;
			}
			return s;
		}
	
		/// <summary>
		/// Zera o valor de todos os campos quando apertar no botão de limpar filtro
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ClearFilter()
		{
			FinalFilter = "";
			txtValue.Text = "";
			txtExpression.Text = "";
			cboOperator.Text = "";
			cboField.Text = "";
		}
	
		protected void ___butExecuteFilter_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				ExecFilter();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butClearFilter_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				ClearFilter();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butConcatenate_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				Concatenate();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

	
	}

}
	
