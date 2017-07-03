using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Summary description for ComboBoxDataItem
/// </summary>
public class RadComboBoxDataItem
{
	public string Text { get; set; }
	public string Value { get; set; }

	public RadComboBoxDataItem() : this("", "")
	{
	}

	public RadComboBoxDataItem(string Text, string Value)
	{
		this.Text = Text;
		this.Value = Value;
	}

	public Telerik.Web.UI.RadComboBoxItem ToRadComboBoxItem()
	{
		return new Telerik.Web.UI.RadComboBoxItem(Text, Value);
	}

	public DataRow ToDataRow()
	{
		DataTable t = new DataTable();
		t.Columns.Add("DISPLAY_FIELD");
		t.Columns.Add("Value");
		DataRow r = t.NewRow();
		r["DISPLAY_FIELD"] = Text;
		r["Value"] = Value;
		return r;
	}
}
