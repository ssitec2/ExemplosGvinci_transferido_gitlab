using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PROJETO;
using COMPONENTS.Data;
using System.Data;
using COMPONENTS.Configuration;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Telerik.Web.UI;

namespace PROJETO
{
	public partial class ConstructorView : System.Web.UI.Page
	{
		private DatabaseConfig Dc;
		ViewSettings Vs = new ViewSettings();

		public string OldName
		{
			get
			{
				if (ViewState["OldName"] != null)
				{
					return ViewState["OldName"].ToString();
				}
				return null;
			}
			set
			{
				ViewState["OldName"] = value;
			}
		}

		public void FillComboBoxes()
		{
			if (cboJoinOperator.Items.Count == 0)
			{
				cboJoinOperator.Items.Add(new RadComboBoxItem("=", "="));
				cboJoinOperator.Items.Add(new RadComboBoxItem("!=", "!="));
				cboJoinOperator.Items.Add(new RadComboBoxItem("LIKE", "LIKE"));
				cboJoinOperator.Items.Add(new RadComboBoxItem(">", ">"));
				cboJoinOperator.Items.Add(new RadComboBoxItem("<", "<"));
			}
			if (cboJoinType.Items.Count == 0)
			{
				cboJoinType.Items.Add(new RadComboBoxItem("Inner", "Inner"));
				cboJoinType.Items.Add(new RadComboBoxItem("Left", "Left"));
				cboJoinType.Items.Add(new RadComboBoxItem("Right", "Right"));
				cboJoinType.Items.Add(new RadComboBoxItem("Full", "Full"));
			}
			if (cboOperator.Items.Count == 0)
			{
				cboOperator.Items.Add(new RadComboBoxItem("=", "="));
				cboOperator.Items.Add(new RadComboBoxItem("!=", "!="));
				cboOperator.Items.Add(new RadComboBoxItem("LIKE", "LIKE"));
				cboOperator.Items.Add(new RadComboBoxItem(">", ">"));
				cboOperator.Items.Add(new RadComboBoxItem("<", "<"));
			}
			if (cboJoinOperator.Items.Count == 0)
			{
				cboJoinOperator.Items.Add(new RadComboBoxItem("Contar", "Contar"));
				cboJoinOperator.Items.Add(new RadComboBoxItem("Soma", "Soma"));
				cboJoinOperator.Items.Add(new RadComboBoxItem("Media", "Media"));
				cboJoinOperator.Items.Add(new RadComboBoxItem("Maior", "Maior"));
				cboJoinOperator.Items.Add(new RadComboBoxItem("Menor", "Menor"));
				cboJoinOperator.Items.Add(new RadComboBoxItem("Dia", "Dia"));
				cboJoinOperator.Items.Add(new RadComboBoxItem("Mes", "Mes"));
				cboJoinOperator.Items.Add(new RadComboBoxItem("Ano", "Ano"));
				cboJoinOperator.Items.Add(new RadComboBoxItem("MesAno", "MesAno"));
			}
		}
		private void cboDbs_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
		{
			Vs = new ViewSettings();
			Vs.DataBase = cboDbs.SelectedItem.Text;
			Database db = Dc.GetDatabaseByName(Vs.DataBase);
			lstTables.Items.Clear();
			cboJoinTable.Items.Clear();
			if (db != null)
			{
				foreach (PROJETO.Table tb in db.Tables)
				{
					lstTables.Items.Add(new ListItem(tb.Title, "Table_" + tb.Name));
					cboJoinTable.Items.Add(new RadComboBoxItem(tb.Title, "Table_" + tb.Name));
				}
				if (cboJoinTable.Items.Count > 0)
				{
					cboJoinTable.SelectedIndex = 0;
				}
			}
			FillComboBoxes();
			FillFieldsList();
		}

		private void cboJoinTable_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
		{
			string DB =  cboDbs.SelectedItem.Text ;
			PROJETO.Table JoinTb = Dc.getTableByName(cboJoinTable.SelectedValue.Remove(0, 6), DB);
			if(JoinTb != null)
			{
				cboJoinField.SelectedIndex = -1;
				cboJoinField.Items.Clear();
				foreach (Field f in JoinTb.Fields)
				{
					cboJoinField.Items.Add(new RadComboBoxItem("[" + cboJoinTable.Text + "].[" + f.Title + "]", "[" + cboJoinTable.SelectedValue + "].[" + f.Name + "]"));
				}
			}
		}

		protected override void InitializeCulture()
		{
			Utility.SetThreadCulture();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			ResultLabel.Text = "";
			Dc = new DatabaseConfig(Server.MapPath("../Databases/"));
			Dc.LoadXmlFile();
			Vs = new ViewSettings();
			FillComboBoxes();
			
			rdbDesc.CheckedChanged += new EventHandler(rdbDesc_CheckedChanged);
			rdbDesc.AutoPostBack = true;
			rdbAsc.CheckedChanged += new EventHandler(rdbAsc_CheckedChanged);
			rdbAsc.AutoPostBack = true;
			lstTables.SelectedIndexChanged += new EventHandler(lstTables_SelectedIndexChanged);
			lstTables.AutoPostBack = true;
			lstSelectedOrderBy.SelectedIndexChanged += new EventHandler(lstSelectedOrderBy_SelectedIndexChanged);
			lstSelectedOrderBy.AutoPostBack = true;
			txtTopRegisters.TextChanged += new EventHandler(txtTopRegisters_TextChanged);
			txtTopRegisters.AutoPostBack = true;
			cboDbs.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboDbs_SelectedIndexChanged);
			cboDbs.AutoPostBack = true;
			cboJoinTable.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboJoinTable_SelectedIndexChanged);
			cboJoinTable.AutoPostBack = true;
			if (!IsPostBack)
			{
				cboDbs.Items.Clear();
				foreach (Database db in Dc.DataBases)
				{
					cboDbs.Items.Add(new RadComboBoxItem(db.Name, db.Name));
				}
				if (cboDbs.Items.Count > 0)
				{
					cboDbs.SelectedIndex = 0;
					cboDbs_SelectedIndexChanged(null, null);
				}
				rdbAsc.Enabled = false;
				rdbDesc.Enabled = false;
				if (HttpContext.Current.Request["page"] != null)
				{
					Vs = (ViewSettings)Deserialize(Vs, Server.MapPath("../Views/" + HttpContext.Current.Request["page"].ToString()));
					OldName = Vs.ViewName;
					foreach (RadComboBoxItem item in cboDbs.Items)
					{
						if (item.Text == Vs.DataBase)
						{
							item.Selected = true;
							cboDbs.Text = Vs.DataBase;
						}
						else
						{
							item.Selected = false;
						}
					}
					txtViews.Text = Vs.ViewName;
					if (Vs.CustomText == null)
					{
						Database db = Dc.GetDatabaseByName(Vs.DataBase);
						foreach (PROJETO.Table tb in db.Tables)
						{
							lstTables.Items.Add(new ListItem(tb.Title, "Table_" + tb.Name));
							cboJoinTable.Items.Add(new RadComboBoxItem(tb.Name));
						}
						foreach (InnerJoin Ij in Vs.InnerJoins)
						{
							lstTables.Items.Add(new ListItem(Ij.Alias));
						}
						FillFieldsList();
						SetAllFieldsLocation();
						foreach (FieldItem fd in Vs.SelectedFields)
						{
							if (fd.IsNewColumn)
							{
								lstSelectedField.Items.Add(new ListItem(fd.ToString()));
							}
							else
							{
								lstSelectedField.Items.Add(new ListItem(fd.ToString(), "Field_" + fd.ToString()));
							}
						}
						foreach (FieldItem fd in Vs.GroupBy)
						{
							lstGroup.Items.Remove(lstGroup.Items.FindByText(fd.ToString()));
							lstSelectedGroupBy.Items.Add(new ListItem(fd.ToString()));
						}
						foreach (OrderBy ob in Vs.OrderBy)
						{
							lstOrder.Items.Remove(lstOrder.Items.FindByText(ob.Field.ToString()));
							lstSelectedOrderBy.Items.Add(new ListItem(ob.Field.ToString()));
						}
						txtTopRegisters.Text = Vs.Top;
					}
					else	
					{
						ControlsPanel.Enabled = false;
						txtQuery.Text = Vs.CustomText;
					}
				}
			}
			else
			{
				if (ViewState["ViewSettings"] != null)
				{
					Vs = (ViewSettings)ViewState["ViewSettings"];
				}
			}
		}
		void rdbAsc_CheckedChanged(object sender, EventArgs e)
		{
			if (rdbAsc.Checked == true)
			{
				SetOrderByAsc();
			}
		}
		void rdbDesc_CheckedChanged(object sender, EventArgs e)
		{
			if (rdbDesc.Checked == true)
			{
				SetOrderByDesc();
			}
		}

		protected override void OnPreRenderComplete(EventArgs e)
		{
			if (Vs.CustomText == null || Vs.CustomText == "")
			{
				txtQuery.Text = Vs.GenerateSqlQuery();
			}
			base.OnPreRenderComplete(e);
		}

		protected override void OnInit(EventArgs e)
		{
			Utility.CheckAuthentication(this,true);
			if (!IsPostBack)
			{
			}
			base.OnInit(e);

		}

		protected void lstTables_SelectedIndexChanged(object sender, EventArgs e)
		{
			FillFieldsList();
			if (lstTables.SelectedItem.Value.IndexOf("Table") == 0)
			{
				butDelJoin.Enabled = false;
			}
			else
			{
				butDelJoin.Enabled = true;
			}
		}

		public void SetAllFieldsLocation()
		{
			Dictionary<string,string> lst= new Dictionary<string,string>();
			string DB =  cboDbs.SelectedItem.Text ;
			foreach (string tbName in Vs.SelectedTables)
			{
				PROJETO.Table tb = Dc.getTableByName(tbName, DB);
				if(tb != null)
				{
					foreach (Field fd in tb.Fields)
					{
						lst.Add("[" + tb.Name  + "].[" + fd.Name+ "]", "[" + tb.Title + "].[" + fd.Title+ "]");
					}
				}
			}

			foreach (InnerJoin inner in Vs.InnerJoins)
			{
				if (Vs.SelectedTables.IndexOf(inner.Table.Name) == -1)
				{
					foreach (Field fd in inner.Table.Fields)
					{
						lst.Add("[" + inner.Alias + "].[" + fd.Name + "]","[" + inner.Alias + "].[" + fd.Name + "]");
					}
				}
			}
			cboJoinBaseField.Items.Clear();
			cboJoinBaseField.Text = "";
			cboField.Items.Clear();
			cboField.Text = "";
			cboValue.Items.Clear();
			cboValue.Text = "";
            lstOrder.Items.Clear();
            lstGroup.Items.Clear();
			foreach (string fdName in lst.Keys)
			{
				if(!lstSelectedGroupBy.Items.Contains(new ListItem(lst[fdName], fdName)))
                lstGroup.Items.Add(new ListItem(lst[fdName], fdName));
				if(!lstSelectedOrderBy.Items.Contains(new ListItem(lst[fdName], fdName)))
                lstOrder.Items.Add(new ListItem(lst[fdName], fdName));
                cboJoinBaseField.Items.Add(new RadComboBoxItem(lst[fdName], fdName));
                cboField.Items.Add(new RadComboBoxItem(lst[fdName], fdName));
                cboValue.Items.Add(new RadComboBoxItem(lst[fdName], fdName));
			}
            cboField.SelectedIndex = -1;
            cboValue.SelectedIndex = -1;
		}

		protected void lstFields_SelectedIndexChanged(object sender, EventArgs e)
		{
			AddFieldsOnList();
		}

		public void AddNewColumn()
		{
			if (lstTables.SelectedItem == null || cboColumnContent.SelectedItem == null)
			{
				return;
			}
			if (Vs.SelectedTables.IndexOf(lstTables.SelectedItem.Value.Remove(0, 6)) == -1 && lstTables.SelectedValue.IndexOf("Join__") != 0)
			{
				Vs.SelectedTables.Add(lstTables.SelectedItem.Value.Remove(0, 6));
				SetAllFieldsLocation();
			}
			NewColumn NewColumn = new NewColumn();
			NewColumn.Fucntion =  cboColumnFunc.Text;
			FieldItem fi = new FieldItem();
			if (cboColumnContent.SelectedIndex > -1)
			{
				fi.TableName = cboColumnContent.SelectedItem.Value.Substring(1, cboColumnContent.SelectedItem.Value.IndexOf("].[") - 1);
				fi.FieldName = cboColumnContent.SelectedItem.Value.Substring(cboColumnContent.SelectedItem.Value.IndexOf("].[") + 3).TrimEnd(']');
			}
			else
			{
				fi.CustomValue = cboColumnContent.Text;
			}
			NewColumn.Content = fi;
			if (txtColumnTitle.Text != "")
			{
				NewColumn.Title =  txtColumnTitle.Text ;
			}
			else
			{
				NewColumn.HasTitle = false;
				NewColumn.Title = NewColumn.Fucntion + "(" + NewColumn.Content.FullName + ")";
			}
			Vs.NewColumn.Add(NewColumn);
			ListItem li = new ListItem(NewColumn.Title, NewColumn.Title);
			lstSelectedField.Items.Add(li);
			li.Selected = true;
			FieldItem fi2 = new FieldItem();
			fi2.CustomValue = NewColumn.Title;
			fi2.TableName = NewColumn.Content.TableName;
			fi2.IsNewColumn = true;
			Vs.SelectedFields.Add(fi2);
		}

		protected void lstOrder_SelectedIndexChanged(object sender, EventArgs e)
		{
			AddIndexLst();
		}

		protected void lstGroup_SelectedIndexChanged(object sender, EventArgs e)
		{
			AddGroupByLst();
		}

		public void SetOrderByAsc()
		{
			if (lstSelectedOrderBy.SelectedItem != null)
			{
	            foreach (OrderBy ob in Vs.OrderBy)
				{
					if (ob.Field.FullName == lstSelectedOrderBy.SelectedItem.Value)
					{
						if (rdbAsc.Checked == true)
						{
							ob.Type = "Asc";
							return;
                        }
					}
				}
			}
		}

		public void SetOrderByDesc()
		{
			if (lstSelectedOrderBy.SelectedItem != null)
			{
				foreach (OrderBy ob in Vs.OrderBy)
				{
					if (ob.Field.FullName == lstSelectedOrderBy.SelectedItem.Value)
					{
						if (rdbDesc.Checked == true)
						{
							ob.Type = "Desc";
							return;
                        }
					}
				}
			}
		}

		protected void txtTopRegisters_TextChanged(object sender, EventArgs e)
		{
			int result = 0;
			if (int.TryParse(txtTopRegisters.Text, out result))
			{
				Vs.Top = txtTopRegisters.Text;
			}
			else
			{
				txtTopRegisters.Text = "";
				Vs.Top = txtTopRegisters.Text;
			}
		}

		public void AddJoin()
		{

			if (cboJoinTable.Text != "" && cboJoinField.Text != "" && cboJoinBaseField.Text != "" && txtJoinAlias.Text != "")
			{
				string DB =  cboDbs.SelectedItem.Text;
				InnerJoin inner = new InnerJoin();
	            inner.Table = Dc.getTableByName(cboJoinTable.SelectedValue.Remove(0,6), DB);

				FieldItem fi = new FieldItem();
				if (cboJoinField.SelectedIndex >= 0)
				{
					fi.TableName = txtJoinAlias.Text ;
					fi.FieldName = cboJoinField.SelectedItem.Value.Substring(cboJoinField.SelectedItem.Value.IndexOf("].[") + 3).TrimEnd(']');
					fi.Title = cboJoinField.SelectedItem.Text.Substring(cboJoinField.SelectedItem.Text.IndexOf("].[") + 3).TrimEnd(']');
				}
				else
				{
					fi.CustomValue = cboJoinField.Text;
				}
				inner.InnerJoinField = fi;

				FieldItem fi2 = new FieldItem();
				if (cboJoinBaseField.SelectedIndex >= 0)
				{
					fi2.TableName = cboJoinBaseField.SelectedItem.Value.Substring(1, cboJoinBaseField.SelectedItem.Value.IndexOf("].[") - 1);
					fi2.FieldName = cboJoinBaseField.SelectedItem.Value.Substring(cboJoinBaseField.SelectedItem.Value.IndexOf("].[") + 3).TrimEnd(']');
				}
				else
				{
					fi2.CustomValue = cboJoinBaseField.SelectedValue;
				}
				inner.BaseField = fi2;

				switch (cboJoinOperator.Text)
				{
					case "=":
						inner.Operator = Operators.Equals;
						break;
					case "LIKE":
						inner.Operator = Operators.Like;
						break;
					case "!=":
						inner.Operator = Operators.Diferent;
						break;
					case ">":
						inner.Operator = Operators.BiggerThan;
						break;
					case "<":
						inner.Operator = Operators.LowerThan;
						break;
					default:
						inner.Operator = Operators.Equals;
						break;
				}
                inner.Alias = txtJoinAlias.Text;
                inner.JoinType = cboJoinType.Text;
				Vs.InnerJoins.Add(inner);
                lstTables.Items.Add(new ListItem(inner.Alias, "Join__" + inner.Alias));
				SetAllFieldsLocation();
			}
		}

		public void AddFilter()
		{
			if (cboField.Text != ""  && cboOperator.Text != ""  && cboValue.Text != "")
			{
				Filter f = new Filter();
				FieldItem fi = new FieldItem();
				if (cboField.SelectedIndex >= 0)
				{
					fi.TableName = cboField.SelectedItem.Value.Substring(1, cboField.SelectedItem.Value.IndexOf("].[") - 1);
					fi.FieldName = cboField.SelectedItem.Value.Substring(cboField.SelectedItem.Value.IndexOf("].[") + 3).TrimEnd(']');
				}
				else
				{
					fi.CustomValue = cboField.SelectedValue;
				}
				f.FirstField = fi;
				switch (cboOperator.Text)
				{
					case "=":
						f.Operator = Operators.Equals;
						break;
					case "LIKE":
						f.Operator = Operators.Like;
						break;
					case "!=":
						f.Operator = Operators.Diferent;
						break;
					case ">":
						f.Operator = Operators.BiggerThan;
						break;
					case "<":
						f.Operator = Operators.LowerThan;
						break;
					default:
						f.Operator = Operators.Equals;
						break;
				}
				FieldItem fi2 = new FieldItem();
				if (cboValue.SelectedIndex >= 0 && cboValue.Text == cboValue.SelectedItem.Text)
				{
					fi2.TableName = cboValue.SelectedItem.Value.Substring(1, cboValue.SelectedItem.Value.IndexOf("].[") - 1);
					fi2.FieldName = cboValue.SelectedItem.Value.Substring(cboValue.SelectedItem.Value.IndexOf("].[") + 3).TrimEnd(']');
				}
				else
				{
					if (cboValue.Text.IndexOf("].[") > 0 && cboValue.Text == cboValue.SelectedItem.Text)
					{
						fi2.CustomValue = cboValue.SelectedValue;
					}
					else
					{
						fi2.CustomValue = cboValue.Text;
					}
					
				}
				f.SeccondField = fi2;
				if (rdbOr.Checked == true)
				{
					f.LogicalOperator = "Or";
				}
				else
				{
						f.LogicalOperator = "And";
                }
				Vs.Filters.Add(f);
			}
		}

		public void ClearFilter()
		{
			Vs.Filters.Clear();
		}

		public void MoveFieldUp()
		{
			if (lstSelectedField.SelectedIndex > 0)
			{
				ListItem MovingLi = lstSelectedField.SelectedItem;
				string MovingText = MovingLi.Text;
				string MovingValue = MovingLi.Value;
				int OldIndex = lstSelectedField.SelectedIndex;
				ListItem ReplacingLi = lstSelectedField.Items[lstSelectedField.SelectedIndex - 1];
				MovingLi.Text = ReplacingLi.Text;
				MovingLi.Value = ReplacingLi.Value;
				ReplacingLi.Text = MovingText;
				ReplacingLi.Value = MovingValue;
				FieldItem ReplaceItem = Vs.SelectedFields[OldIndex];
				Vs.SelectedFields[OldIndex] = Vs.SelectedFields[OldIndex - 1];
				Vs.SelectedFields[OldIndex - 1] = ReplaceItem;
				MovingLi.Selected = false;
				ReplacingLi.Selected = true;
			}
		}

		public void MoveFieldDown()
		{
			if (lstSelectedField.SelectedIndex != -1 && lstSelectedField.SelectedIndex < lstSelectedField.Items.Count - 1)
			{
				ListItem MovingLi = lstSelectedField.SelectedItem;
				string MovingText = MovingLi.Text;
				int OldIndex = lstSelectedField.SelectedIndex;
				ListItem ReplacingLi = lstSelectedField.Items[lstSelectedField.SelectedIndex + 1];
				MovingLi.Text = ReplacingLi.Text;
				ReplacingLi.Text = MovingText;
				FieldItem ReplaceItem = Vs.SelectedFields[OldIndex];
				Vs.SelectedFields[OldIndex] = Vs.SelectedFields[OldIndex + 1];
				Vs.SelectedFields[OldIndex + 1] = ReplaceItem;
				MovingLi.Selected = false;
				ReplacingLi.Selected = true;
			}
		}

		public void FillFieldsList()
		{
			cboColumnContent.Items.Clear();
			cboColumnContent.Text = "";
            lstFields.Items.Clear();
			string DB =  cboDbs.SelectedItem.Text ;
			if (lstTables.SelectedIndex != -1)
			{
				if (lstTables.SelectedItem.Value.IndexOf("Table") == 0)
				{
					PROJETO.Table tb = Dc.getTableByName(lstTables.SelectedItem.Value.Remove(0,6), DB);
					foreach (Field fd in tb.Fields)
					{
						bool IsSelected = false;
						foreach (ListItem li in lstSelectedField.Items)
						{
							if (li.Text == "[" + tb.Title + "].[" + fd.Title + "]")
							{
								IsSelected = true;
								break;
							}
						}
						if (!IsSelected)
						{
							lstFields.Items.Add(new ListItem(fd.Title, "Field_[" + tb.Name + "].[" + fd.Name + "]"));
						}
                        cboColumnContent.Items.Add(new RadComboBoxItem("[" + tb.Title + "].[" + fd.Title + "]", "[" + tb.Name + "].[" + fd.Name + "]"));
					}
				}
				else
				{
					PROJETO.Table tb = Vs.GetTableByJoinAlias(lstTables.SelectedItem.Text);
					foreach (Field fd in tb.Fields)
					{
						bool IsSelected = false;
						foreach (ListItem li in lstSelectedField.Items)
						{
							if (li.Text == "[" + lstTables.SelectedItem.Text + "].[" + fd.Name + "]")
							{
								IsSelected = true;
								break;
							}
						}
						if (!IsSelected)
						{
                            lstFields.Items.Add(new ListItem(fd.Title, "Join__[" + lstTables.SelectedItem.Text + "].[" + fd.Name + "]"));
						}
                        cboColumnContent.Items.Add(new RadComboBoxItem("[" + lstTables.SelectedItem.Text + "].[" + fd.Title + "]", "[" + lstTables.SelectedItem.Value + "].[" + fd.Name + "]"));
					}
				}
			}
			if (cboJoinTable.SelectedIndex > -1)
			{
				PROJETO.Table tb = Dc.getTableByName(cboJoinTable.SelectedItem.Value.Remove(0, 6), DB);
				cboJoinField.Items.Clear();
				foreach (Field fd in tb.Fields)
				{
					cboJoinField.Items.Add(new RadComboBoxItem("[" + tb.Title + "].[" + fd.Title + "]", "[" + tb.Name + "].[" + fd.Name + "]"));
				}
			}
		}

		public void DeleteFieldView()
		{
			string DeletedTable = "";
			if (lstSelectedField.SelectedIndex != -1 && lstSelectedField.SelectedValue.IndexOf("Join") != 0 && lstSelectedField.SelectedValue.IndexOf("Field") == 0)
			{
				DeletedTable = lstSelectedField.SelectedItem.Value.Remove(0,6).Substring(0, lstSelectedField.SelectedItem.Value.Remove(0,6).IndexOf(".") ==-1 ? 0:lstSelectedField.SelectedItem.Value.Remove(0,6).IndexOf(".")).TrimStart('[').TrimEnd(']');
				Vs.SelectedFields.Remove(Vs.GetFieldByName(lstSelectedField.SelectedItem.Value.Remove(0,6)));
				
				lstSelectedField.Items.Remove(lstSelectedField.SelectedItem);
				
				bool HasOtherField = false;
				foreach (ListItem li in lstSelectedField.Items)
				{
					if (li.Value.IndexOf("Field") == 0)
					{
						if (li.Value.Remove(0, 6).TrimStart('[').IndexOf(DeletedTable) == 0)
						{
							HasOtherField = true;
							break;
						}
					}
					else
					{
						NewColumn nc = Vs.getNewColumnByName(li.Text);
						if (nc != null && nc.Content.TableName == DeletedTable)
						{
							HasOtherField = true;
							break;
						}
					}
				}
				if (HasOtherField == false)
				{
					if (lstTables.Items.FindByValue("Table_"+DeletedTable.Substring(0, DeletedTable.Length)).Value.IndexOf("Table_") == 0)
					
					{
						DeleteAllFieldsWithTable(DeletedTable);
						Vs.SelectedTables.Remove(DeletedTable);
                    }
				}
			}
			else if (lstSelectedField.SelectedValue.IndexOf("Join") == 0)
			{
				lstSelectedField.Items.Remove(lstSelectedField.SelectedItem);
				Vs.SelectedFields.Remove(Vs.GetFieldByName(lstSelectedField.SelectedValue));
			}
			else if (lstSelectedField.SelectedIndex != -1)
			{
				if (Vs.getNewColumnByName(lstSelectedField.SelectedItem.Text).Content.TableName != "")
				{
					bool HasOtherField = false;
					DeletedTable = Vs.getNewColumnByName(lstSelectedField.SelectedItem.Text).Content.TableName;
					foreach (ListItem li in lstSelectedField.Items)
					{
						if (li.Value.IndexOf(DeletedTable) == 7 || (li.Text != lstSelectedField.SelectedItem.Text && Vs.getNewColumnByName(li.Text) != null && Vs.getNewColumnByName(li.Text).Content.TableName == DeletedTable))
						{
							HasOtherField = true;
							break;
						}
					}
					if (HasOtherField == false)
					{
						if (lstTables.Items.FindByValue("Table_" + DeletedTable) != null)
						{
							DeleteAllFieldsWithTable(DeletedTable);
							Vs.SelectedTables.Remove(DeletedTable);
						}
					}
				}
				foreach (NewColumn nc in Vs.NewColumn)
				{
					if (nc.Title == lstSelectedField.SelectedValue)
					{
						Vs.NewColumn.Remove(nc);
						break;
					}
				}
				Vs.SelectedFields.Remove(Vs.GetFieldByName(lstSelectedField.SelectedValue));
				lstSelectedField.Items.Remove(lstSelectedField.SelectedItem);
			}
			SetAllFieldsLocation();
			FillFieldsList();
		}

		public void DeleteInnerJoin()
		{
			if (lstTables.SelectedItem != null)
			{
				if (lstTables.SelectedItem.Value.IndexOf("Join") == 0)
				{
					Vs.InnerJoins.Remove(Vs.getJoinByAlias(lstTables.SelectedItem.Text));
					string Name = lstTables.SelectedItem.Text;
					lstTables.Items.Remove(lstTables.SelectedItem);
					DeleteAllFieldsWithTable(Name);
					lstTables.SelectedIndex = 0;
					FillFieldsList();
					if (lstTables.SelectedItem.Value.IndexOf("Table") == 0)
					{
						butDelJoin.Enabled = false;
					}
					else
					{
						butDelJoin.Enabled = true;
					}
				}
			}
		}

		private void DeleteAllFieldsWithTable(string TableName)
		{
			List<Filter> DeletedFilterList = new List<Filter>();
			foreach (Filter f in Vs.Filters)
			{
				if (f.FirstField.TableName == TableName || f.SeccondField.TableName == TableName)
				{
					DeletedFilterList.Add(f);
				}
			}
			foreach (Filter df in DeletedFilterList)
			{
				Vs.Filters.Remove(df);
			}
			List<InnerJoin> DeletedInnerJoinList = new List<InnerJoin>();
			foreach (InnerJoin I in Vs.InnerJoins)
			{
				if (I.BaseField.TableName == TableName || I.InnerJoinField.TableName == TableName)
				{
					DeletedInnerJoinList.Add(I);
				}
			}
			foreach (InnerJoin df in DeletedInnerJoinList)
			{
				Vs.InnerJoins.Remove(df);
                lstTables.Items.Remove(lstTables.Items.FindByText(df.Alias));
			}

			List<FieldItem> DeletedGroupByList = new List<FieldItem>();
			foreach (FieldItem g in Vs.GroupBy)
			{
				if (g.TableName == TableName)
				{
					DeletedGroupByList.Add(g);
				}
			}
			foreach (FieldItem df in DeletedGroupByList)
			{
				Vs.GroupBy.Remove(df);
                lstSelectedGroupBy.Items.Remove(lstSelectedGroupBy.Items.FindByText(df.FullName));
			}

			List<OrderBy> DeletedOrderByList = new List<OrderBy>();
			foreach (OrderBy g in Vs.OrderBy)
			{
				if (g.Field.TableName == TableName)
				{
					DeletedOrderByList.Add(g);
				}
			}
			foreach (OrderBy df in DeletedOrderByList)
			{
				Vs.OrderBy.Remove(df);
                lstSelectedOrderBy.Items.Remove(lstSelectedOrderBy.Items.FindByText(df.Field.FullName));
			}

			List<FieldItem> DeletedFieldList = new List<FieldItem>();
			foreach (FieldItem field in Vs.SelectedFields)
			{
				if (field.TableName == TableName)
				{
					DeletedFieldList.Add(field);
				}
			}
			foreach (FieldItem df in DeletedFieldList)
			{
				Vs.SelectedFields.Remove(df);
                lstSelectedField.Items.Remove(lstSelectedField.Items.FindByText(df.FullName));
			}

			List<NewColumn> DeletedNewColumnList = new List<NewColumn>();
			foreach (NewColumn c in Vs.NewColumn)
			{
				if (c.Content.TableName == TableName)
				{
					DeletedNewColumnList.Add(c);
				}
			}
			foreach (NewColumn df in DeletedNewColumnList)
			{
				Vs.NewColumn.Remove(df);
                lstSelectedField.Items.Remove(lstSelectedField.Items.FindByText(df.Title));
			}
			SetAllFieldsLocation();
			FillFieldsList();
		}

		public void MoveGroupByUp()
		{
			if (lstSelectedGroupBy.SelectedIndex > 0)
			{
				ListItem MovingLi = lstSelectedGroupBy.SelectedItem;
				string MovingText = MovingLi.Text;
				string MovingValue = MovingLi.Value;
				int OldIndex = lstSelectedGroupBy.SelectedIndex;
				ListItem ReplacingLi = lstSelectedGroupBy.Items[lstSelectedGroupBy.SelectedIndex - 1];
				MovingLi.Text = ReplacingLi.Text;
				MovingLi.Value = ReplacingLi.Value;
				ReplacingLi.Text = MovingText;
				ReplacingLi.Value = MovingValue;
				FieldItem ReplaceItem = Vs.GroupBy[OldIndex];
				Vs.GroupBy[OldIndex] = Vs.GroupBy[OldIndex - 1];
				Vs.GroupBy[OldIndex - 1] = ReplaceItem;
				MovingLi.Selected = false;
				ReplacingLi.Selected = true;
			}
		}

		public void MoveGroupByDown()
		{
			if (lstSelectedGroupBy.SelectedIndex != -1 && lstSelectedGroupBy.SelectedIndex < lstSelectedGroupBy.Items.Count - 1)
			{
				ListItem MovingLi = lstSelectedGroupBy.SelectedItem;
				string MovingText = MovingLi.Text;
				string MovingValue = MovingLi.Value;
				int OldIndex = lstSelectedGroupBy.SelectedIndex;
				ListItem ReplacingLi = lstSelectedGroupBy.Items[lstSelectedGroupBy.SelectedIndex + 1];
				MovingLi.Text = ReplacingLi.Text;
				MovingLi.Value = ReplacingLi.Value;
				ReplacingLi.Text = MovingText;
				ReplacingLi.Value = MovingValue;
				FieldItem ReplaceItem = Vs.GroupBy[OldIndex];
				Vs.GroupBy[OldIndex] = Vs.GroupBy[OldIndex + 1];
				Vs.GroupBy[OldIndex + 1] = ReplaceItem;
				MovingLi.Selected = false;
				ReplacingLi.Selected = true;
			}
		}

		public void DeleteGroupBy()
		{
			if (lstSelectedGroupBy.SelectedIndex != -1)
			{
				foreach (FieldItem fi in Vs.GroupBy)
				{
					if (fi.FullName == lstSelectedGroupBy.SelectedItem.Value)
					{
						Vs.GroupBy.Remove(fi);
						break;
					}
				}
				lstSelectedGroupBy.Items.Remove(lstSelectedGroupBy.SelectedItem);
			}
			SetAllFieldsLocation();
			FillFieldsList();
		}

		public void MoveOrderByUp()
		{
			if (lstSelectedOrderBy.SelectedIndex > 0)
			{
				ListItem MovingLi = lstSelectedOrderBy.SelectedItem;
				string MovingText = MovingLi.Text;
				string MovingValue = MovingLi.Value;
				int OldIndex = lstSelectedOrderBy.SelectedIndex;
				ListItem ReplacingLi = lstSelectedOrderBy.Items[lstSelectedOrderBy.SelectedIndex - 1];
				MovingLi.Text = ReplacingLi.Text;
				MovingLi.Value = ReplacingLi.Value;
				ReplacingLi.Text = MovingText;
				ReplacingLi.Value = MovingValue;
				OrderBy ReplaceItem = Vs.OrderBy[OldIndex];
				Vs.OrderBy[OldIndex] = Vs.OrderBy[OldIndex - 1];
				Vs.OrderBy[OldIndex - 1] = ReplaceItem;
				MovingLi.Selected = false;
				ReplacingLi.Selected = true;
			}
		}

		private void AddFieldsOnList()
		{
			if (lstFields.SelectedItem != null)
			{
				lstSelectedField.SelectedIndex = -1;
				lstSelectedField.Items.Add(new ListItem("[" + lstTables.SelectedItem.Text + "].[" + lstFields.SelectedItem.Text + "]",
				lstFields.SelectedItem.Value));
				FieldItem fi = new FieldItem();
				fi.TableName = lstFields.SelectedItem.Value.Remove(0, 6).Substring(1, lstFields.SelectedItem.Value.Remove(0, 6).IndexOf("].[") - 1);
				fi.FieldName = lstFields.SelectedItem.Value.Substring(lstFields.SelectedItem.Value.IndexOf("].[") + 3).TrimEnd(']');
				fi.Title = lstFields.SelectedItem.Text;
				lstFields.Items.Remove(lstFields.SelectedItem);
				Vs.SelectedFields.Add(fi);
				if (lstTables.SelectedItem.Value.Length > 6)
				{
					if (Vs.SelectedTables.IndexOf(lstTables.SelectedItem.Value.Remove(0, 6)) == -1 && lstTables.SelectedValue.IndexOf("Join__") != 0)
					{
						Vs.SelectedTables.Add(lstTables.SelectedItem.Value.Remove(0, 6));
						SetAllFieldsLocation();
					}
				}
			}
		}

		private void AddIndexLst()
		{
			if (lstOrder.SelectedItem != null)
			{
				OrderBy ob = new OrderBy();
				FieldItem fi = new FieldItem();
				fi.TableName = lstOrder.SelectedItem.Value.Substring(1, lstOrder.SelectedItem.Value.IndexOf("].[") - 1);
				fi.FieldName = lstOrder.SelectedItem.Value.Substring(lstOrder.SelectedItem.Value.IndexOf("].[") + 3).TrimEnd(']');
				ob.Field = fi;
				lstSelectedOrderBy.SelectedIndex = -1;
				lstSelectedOrderBy.Items.Add(lstOrder.SelectedItem);
				lstOrder.Items.Remove(lstOrder.SelectedItem);
				ob.Type = "Asc";
				rdbAsc.Checked = true;
				rdbAsc.Enabled = true;
				rdbDesc.Checked = false;
				rdbDesc.Enabled = true;
				Vs.OrderBy.Add(ob);
			}
		}

		private void AddGroupByLst()
		{
			if (lstGroup.SelectedItem != null)
			{
				FieldItem fi = new FieldItem();
				fi.TableName = lstGroup.SelectedItem.Value.Substring(1, lstGroup.SelectedItem.Value.IndexOf("].[") - 1);
				fi.FieldName = lstGroup.SelectedItem.Value.Substring(lstGroup.SelectedItem.Value.IndexOf("].[") + 3).TrimEnd(']');
				Vs.GroupBy.Add(fi);
				lstSelectedGroupBy.SelectedIndex = -1;
				lstSelectedGroupBy.Items.Add(lstGroup.SelectedItem);
				lstGroup.Items.Remove(lstGroup.SelectedItem);
			}
		}

		public void MoveOrderByDown()
		{
			if (lstSelectedOrderBy.SelectedIndex != -1 && lstSelectedOrderBy.SelectedIndex < lstSelectedOrderBy.Items.Count - 1)
			{
				ListItem MovingLi = lstSelectedOrderBy.SelectedItem;
				string MovingText = MovingLi.Text;
				string MovingValue = MovingLi.Value;
				int OldIndex = lstSelectedOrderBy.SelectedIndex;
				ListItem ReplacingLi = lstSelectedOrderBy.Items[lstSelectedOrderBy.SelectedIndex + 1];
				MovingLi.Text = ReplacingLi.Text;
				MovingLi.Value = ReplacingLi.Value;
				ReplacingLi.Text = MovingText;
				ReplacingLi.Value = MovingValue;
				OrderBy ReplaceItem = Vs.OrderBy[OldIndex];
				Vs.OrderBy[OldIndex] = Vs.OrderBy[OldIndex + 1];
				Vs.OrderBy[OldIndex + 1] = ReplaceItem;
				MovingLi.Selected = false;
				ReplacingLi.Selected = true;
			}
		}

		public void RemoveOrderBy()
		{
			if (lstSelectedOrderBy.SelectedIndex != -1)
			{
				foreach (OrderBy ob in Vs.OrderBy)
				{
					if (ob.Field.FullName == lstSelectedOrderBy.SelectedItem.Value)
					{
						Vs.OrderBy.Remove(ob);
						break;
					}
				}
				lstSelectedOrderBy.Items.Remove(lstSelectedOrderBy.SelectedItem);
			}
			SetAllFieldsLocation();
			FillFieldsList();
		}

		protected void txtViews_TextChanged(object sender, EventArgs e)
		{
			Vs.ViewName = txtViews.Text;
		}

		protected void lstSelectedOrderBy_SelectedIndexChanged(object sender, EventArgs e)
		{
            rdbAsc.Enabled = true;
            rdbDesc.Enabled = true;
			foreach (OrderBy ob in Vs.OrderBy)
			{
				if (ob.Field.ToString() == lstSelectedOrderBy.SelectedItem.Value)
				{
					if (ob.Type == "Desc")
					{
                        rdbDesc.Checked = true;
                        rdbAsc.Checked = false;
					}
					else
					{
                        rdbDesc.Checked = false;
                        rdbAsc.Checked = true;
					}
				}
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			ViewState["ViewSettings"] = Vs;
			base.OnPreRender(e);
		}
		
		private void TestDB()
		{
			if (Vs.DataBase != null && Vs.DataBase != "")
			{
				DataAccessObject dao = Settings.GetDataAccessObject(((COMPONENTS.Databases)HttpContext.Current.Application["Databases"])[Vs.DataBase]);
				try
				{
					DataSet ds = dao.RunSql(txtQuery.Text);
		           ResultLabel.Text = "Sql executado com sucesso!";
		           ResultLabel.ForeColor = Color.Green;
				}
				catch
				{
		             ResultLabel.Text = "Erro no Sql!";
		             ResultLabel.ForeColor = Color.Red;
				}
			}
		}

		protected void cboValue_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
		{
			SetAllFieldsLocation();
		}

		protected void ControlsAjaxPanel_AjaxRequest(object source, Telerik.Web.UI.AjaxRequestEventArgs e)
		{
			if (e.Argument == "ExecuteCommand:SetCustomView")
			{
				ViewSettings New = new ViewSettings();
				New.ViewName = Vs.ViewName;
				New.DataBase = Vs.DataBase;
				Vs = New;
				Vs.CustomText = txtQuery.Text;
				SetAllFieldsLocation();
				FillFieldsList();
				if (txtQuery.Text == "")
				{
					ControlsPanel.Enabled = true;
				}
				else
				{
					ControlsPanel.Enabled = false;
				}
			}
			else if (e.Argument == "ExecuteComand:SetManagedView")
			{
				txtQuery.Text = Vs.GenerateSqlQuery();
			}
		}
		
		///<summary>
		///Serializa o objeto para ser colocado no xml
		///</summary>
		///<param name="source">O que vai ser serializado</param>
		///<param name="file">Onde vai ser gravado o objeto serializado</param>
		///<param name="type">Tipo do objeto serializado</param>
		public static void Serialize(object source, string file, Type type)
		{
			using (XmlTextWriter writer = new XmlTextWriter(file, System.Text.Encoding.UTF8))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				MemoryStream Ms = new MemoryStream();

				xmlSerializer.Serialize(writer, source);
				xmlSerializer.Serialize(Ms, source);

				writer.Flush();
			}
		}

		public static object Deserialize(object source, string file)
		{
			if (!File.Exists(file))
			{
				return null;
			}

			using (StreamReader reader = File.OpenText(file))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
				return xmlSerializer.Deserialize(reader);
			}
		}

		void SaveView()
		{
			if (ControlsPanel.Enabled == false)
			{
				Vs.CustomText = txtQuery.Text;
            }
			Vs.ViewName = txtViews.Text;
			if (Vs.ViewName == "")
			{
				return;
			}
			if (OldName == null)
			{
				XmlDocument doc = new XmlDocument();
				if(!File.Exists(Server.MapPath("../Xmls/ViewsList.xml")))
				{
					XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
					doc.InsertBefore(xmlDeclaration, doc.DocumentElement);
					XmlNode ViewsNode = doc.CreateNode(XmlNodeType.Element, "Views", "");
					doc.AppendChild(ViewsNode);
				}
				else
				{
					doc.Load(Server.MapPath("../Xmls/ViewsList.xml"));
				}
				XmlNode newView = doc.CreateNode(XmlNodeType.Element, "View", "");
				XmlAttribute NameAt = doc.CreateAttribute("Name");
				NameAt.Value = Vs.ViewName;
				newView.Attributes.Append(NameAt);
				doc.FirstChild.NextSibling.AppendChild(newView);
				doc.Save(Server.MapPath("../Xmls/ViewsList.xml"));
			}
			else if (OldName != Vs.ViewName)
			{
				if (File.Exists(Server.MapPath("../Views/" + OldName)))
				{
					File.Delete(Server.MapPath("../Views/" + OldName));
				}
				XmlDocument doc = new XmlDocument();
				doc.Load(Server.MapPath("../Xmls/ViewsList.xml"));
				foreach (XmlNode nd in doc.FirstChild.NextSibling.ChildNodes)
				{
					if (nd.Attributes["Name"].Value == OldName)
					{
						nd.Attributes["Name"].Value = Vs.ViewName;
					}
				}
				doc.Save(Server.MapPath("../Xmls/ViewsList.xml"));
			}
			Serialize(Vs, Server.MapPath("../Views/" + Vs.ViewName), typeof(ViewSettings));
			OldName = Vs.ViewName;  
            ResultLabel.Text = "Salvo com sucesso!";
            ResultLabel.ForeColor = Color.Green;
		}
		
		protected void ___butNewCol_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				AddNewColumn();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butUpField_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				MoveFieldUp();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butDownField_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				MoveFieldDown();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butDelJoin_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				DeleteInnerJoin();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butAddField_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				AddFieldsOnList();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butDelField_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				DeleteFieldView();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butUpOrderBy_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				MoveOrderByUp();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butDownGroupBy_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				MoveGroupByDown();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butUpGroupBy_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				MoveGroupByUp();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butDownOrderBy_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				MoveOrderByDown();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butDelGroupBy_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				DeleteGroupBy();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butDelOrderBy_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				RemoveOrderBy();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butAddGroup_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				AddGroupByLst();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butAddOrder_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				AddIndexLst();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butAddJoin_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				AddJoin();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butAddFilter_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				AddFilter();
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

		protected void ___butSave_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				SaveView();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		protected void ___butTest_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				TestDB();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

	}
}
