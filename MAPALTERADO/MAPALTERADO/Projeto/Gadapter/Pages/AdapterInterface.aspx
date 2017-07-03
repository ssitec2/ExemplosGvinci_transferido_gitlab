<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdapterInterface.aspx.cs" Inherits="AdapterInferface" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Adapta</title>
	<link rel="stylesheet" type="text/css" href="../css/Style.css" />
	
</head>
<body>
	<script type="text/javascript" language="javascript">
		function SetVisible() 
		{
			document.getElementById("PgImage").className = "ProgressBarVisible";
		}
	</script>
	<div align="center">
		<form id="Form1" runat="server" style="width: 600px; height: 400px;">
			<asp:ScriptManager ID="MainScriptManager" runat="server"/>
			<div style="vertical-align:middle;border:1px solid #3d84cc">
				<telerik:RadAjaxPanel ID="ajaxpanel" runat="server" Width="100%">
					<table style="border-collapse: collapse" width="100%">
						<tr>
							<td align="center" colspan="2" class="style1">
								<asp:Label ID="LbDataBase" runat="server" Style="font-size: medium; color:#3d84cc" Text=""/>
							</td>
						</tr>
						<tr class="Controls">
							<td align="center" colspan="2">
								<asp:TextBox ID="txtInformation" runat="server" BackColor="Transparent" BorderColor="Silver"
									BorderWidth="1px" Height="45px" ReadOnly="true" Style="cursor: default; text-align: left"
									TextMode="MultiLine" Width="98%" />
							</td>
						</tr>
						<tr class="Controls">
							<td style="width: 35%">
								<asp:ListBox ID="ListBoxTable" runat="server" AutoPostBack="true" Height="298px"
									OnSelectedIndexChanged="ListBoxTable_SelectedIndexChanged" Width="98%"></asp:ListBox>
							</td>
							<td style="width: 65%">
								<asp:Panel ID="Panel1" runat="server" Height="280px" ScrollBars="Vertical">
									<asp:GridView ID="GrdFields" runat="server" AutoGenerateColumns="False" Width="95%">
										<Columns>
											<asp:BoundField ControlStyle-Width="40%" DataField="NewField" HeaderText="Campo novo"
												InsertVisible="False" ItemStyle-Width="40%" ReadOnly="True" />
											<asp:TemplateField ControlStyle-Width="100%" HeaderText="Campo antigo" ItemStyle-Width="200%">
												<ItemTemplate>
													<asp:DropDownList ID="OldFieldsCboList" runat="server" OnInit="Cbo_PreRender" DataTextField="OldField"
														SelectedValue='<%# Bind("OldField") %>'>
													</asp:DropDownList>
												</ItemTemplate>
											</asp:TemplateField>
										</Columns>
										<SelectedRowStyle BackColor="Yellow" />
									</asp:GridView>
								</asp:Panel>
							</td>
						</tr>
					</table>
				</telerik:RadAjaxPanel>
				<table width="100%">
				<tr>
					<th align="left">
						<img class="ProgressBarInvisible" id="PgImage" alt="Title" src="../Images/loading2.gif"
							style="width: 98px; height: 21px; margin-bottom: 0px; vertical-align: middle" />
					</th>
					<td align="right">
						<asp:Button ID="btnNext" runat="server" OnClick="btn_Next_Click" OnClientClick="SetVisible();" Text="Avançar" />
					</td>
				</tr>
			</table>
			</div>
		</form>
	</div>
</body>
</html>
