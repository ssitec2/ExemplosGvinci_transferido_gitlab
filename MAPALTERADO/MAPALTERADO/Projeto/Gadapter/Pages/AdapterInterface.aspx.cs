using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using COMPONENTS;
using System.Data;
using GAdapter;      

public partial class AdapterInferface : System.Web.UI.Page
{
	public List<FieldStruct> FieldStructs
	{
		get
		{
			return (List<FieldStruct>)ViewState["OldFieldsList"];
		}
		set
		{
			ViewState["OldFieldsList"] = value;
		}
	}

	private ReadyAdaptation Adap;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{

			this.LbDataBase.Text = HttpContext.Current.Session["DatabaseName"].ToString();
			Adap = new ReadyAdaptation();
			switch (Adap.AdapterStep)
			{
				case "OnFieldTableChangedEvent":
					break;
				case "OnTableChangedEvent":
					LoadFieldsGrid(Adap.AvaibleTables[0]);
					break;
				case "OnTableCreatedOrRenamedEvent":
					ListBoxTable.Items.Clear();
					ListBoxTable.Items.Add(new ListItem("<Nenhuma Tabela>", ""));
					foreach (TableStruct Item in Adap.AvaibleTables)
					{
						ListItem LItem = new ListItem(Item.TableName);
						ListBoxTable.Items.Add(LItem);
					}
					break;
				default:
					break;
			}
			DefineInformationText(Adap.AdapterStep);
		}
	}
	
	protected void ListBoxTable_SelectedIndexChanged(object sender, EventArgs e)
	{
		Adap = new ReadyAdaptation();
		if(ListBoxTable.SelectedIndex == 0)
		{
			LoadFieldsGrid(null);
			return;
		}
		TableStruct SelectedTable = (from t in Adap.AvaibleTables where t.TableName == ListBoxTable.SelectedValue select t).SingleOrDefault();

		switch (Adap.AdapterStep)
		{
			case "OnFieldTableChangedEvent":
				txtInformation.Text = " TESTE  OnFieldTableChangedEvent";
				break;
			case "OnTableChangedEvent":
				LoadFieldsGrid(Adap.AvaibleTables[0]);	
				break;
			case "OnTableCreatedOrRenamedEvent":
				if (SelectedTable != null)
				{
					LoadFieldsGrid(SelectedTable);
				}
				break;
			default:
				break;
		}
	}
	
	private void DefineInformationText(string EventName)
	{
		switch (EventName)
		{
			case "OnTableChangedEvent":
				txtInformation.Text = "A tabela " + Adap.AdaptingTable + " sofreu alteração nos campos.";
				break;
			case "OnTableCreatedOrRenamedEvent":
				txtInformation.Text = "Caso alguma tabela abaixo tenha sido renomeada para " + Adap.AdaptingTable.TableName
							  + " \r\nselecione a tabela correspondente.";
				break;
			default:
				txtInformation.Text = "Não Implementado: OnFieldTableChangedEvent";
				break;
		}
		GrdFields.DataSource = null;
	}

	public void LoadFieldsGrid(TableStruct OldTable)
	{
		if (OldTable == null)
		{
			GrdFields.DataSource = null;
			GrdFields.DataBind();
			return;
		}
		FieldStructs = OldTable.Fields;
		DataTable dt = new DataTable(Adap.AdaptingTable.TableName);

		dt.Columns.Add("NewField");
		dt.Columns.Add("OldField");

		for (int i = 0; i < Adap.AdaptingTable.Fields.Count; i++)
		{

			FieldStruct NewField = Adap.AdaptingTable.Fields[i];

			DataRow dr = dt.NewRow();
			dr["NewField"] = NewField.FieldName;
			dr["OldField"] = "";

			FieldStruct FieldProperty = (from f
										 in OldTable.Fields
										 where f.FieldName == NewField.FieldName
										 select f).FirstOrDefault();

			if (FieldProperty != null)
			{
				dr["OldField"] = FieldProperty.FieldName;
			}

			dt.Rows.Add(dr);
		}

		GrdFields.DataSource = dt;
		GrdFields.DataBind();
	}

	protected void Cbo_PreRender(object sender, EventArgs e)
	{
		DropDownList lst = sender as DropDownList;
		lst.Items.Clear();
		lst.Items.Add(new ListItem("<Nenhum Campo>", ""));
		foreach (FieldStruct Item in FieldStructs)
		{
			lst.Items.Add(Item.FieldName);
		}

	}
	protected void btn_Next_Click(object sender, EventArgs e)
	{
		string Error = "";
		ReadyAdaptation Adap = new ReadyAdaptation();
		Adap.Init();
		switch (Adap.AdapterStep)
		{
			case "OnTableChangedEvent":
			case "OnTableCreatedOrRenamedEvent":
				if (Adap.AdapterStep == "OnTableChangedEvent")
				{
					Adap.NewTable();
				}
				else
				{
					Adap.ChangeTable(ListBoxTable.SelectedValue);
				}

				foreach (GridViewRow Row in GrdFields.Rows)
				{
					Adap.ChangeField(Server.HtmlDecode(Row.Cells[0].Text), Server.HtmlDecode(((DropDownList)Row.Cells[1].FindControl("OldFieldsCboList")).Text));
				}
				//Adap.ChangeField(ListBoxTable.SelectedValue);
				break;
			default:
				txtInformation.Text = "Não Implementado: OnFieldTableChangedEvent";
				break;
		}
		Adap.Run(ref Error);
	}

}
