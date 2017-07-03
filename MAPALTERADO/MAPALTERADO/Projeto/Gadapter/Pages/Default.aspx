<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" EnableEventValidation="false" CodeFile="Default.aspx.cs" Inherits="PROJETO._Default" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Configurações</title>
	<link rel="stylesheet" type="text/css" href="../css/Style.css" />
	<style type="text/css">
		.style1
		{
			height: 71px;
		}
	</style>
</head>
<body>
	<script type="text/javascript" language="javascript">
		function ShowTempServer(sender) 
		{
			if (sender.checked == false) 
			{
				RowPassword.style.display = "none";
				RowLogin.style.display = "none";
				RowServer.style.display = "none";
				RowAutTmp.style.display = "none";
			}
			else 
			{
			    RowPassword.style.display = "table-row"
                RowLogin.style.display = "table-row";
				RowServer.style.display = "table-row";
				RowAutTmp.style.display = "table-row";
			}
		}

		function WinAut() 
		{
			if (document.getElementById("cbxWinAut").checked) 
			{	
				document.getElementById("txtUser").disabled = true;
				document.getElementById("txtPassword").disabled = true;
			}
			else 
			{
				document.getElementById("txtUser").disabled = false;
				document.getElementById("txtPassword").disabled = false;
			}
		}

		function WinAutTMP() 
		{
			if (document.getElementById("cbxWinAutTmp").checked) 
			{
				document.getElementById("txtUserServerTemp").disabled = true;
				document.getElementById("txtPasswordServerTemp").disabled = true;
			}
			else 
			{
				document.getElementById("txtUserServerTemp").disabled = false;
				document.getElementById("txtPasswordServerTemp").disabled = false;
			}
		}

		function SetVisible() 
		{
			document.getElementById("PgImage").className = "ProgressBarVisible";
		}
	</script>
	<form id="Form1" runat="server">
		<div align="center">
			<asp:ScriptManager ID="MainScriptManager" runat="server"/>
			<telerik:RadAjaxPanel ID="ajaxpanel" runat="server" OnAjaxRequest="AjaxRequest">
				<table style="border-collapse: collapse" width="450px">
					<tr class="Controls">
						<td colspan="2" class="style1">
							<img alt="Title" src="../Images/adapter.jpg" style="width: 100%; height: 64px; margin-bottom: 0px;" />
						</td>
					</tr>
					<tr class="Controls">
						<td colspan="2" align="center">
							<asp:TextBox ID="txtInformation" runat="server" Height="45px" Width="98%" BorderWidth="0px"
								BackColor="Transparent" ForeColor="Red" ReadOnly="true" Style="cursor: default;
								text-align: center; font: 12" TextMode="MultiLine" />
						</td>
					</tr>
					<tr class="Controls">
						<th style="width:40%">
							Servidor:
						</th>
						<td align="left">
							<asp:RadioButtonList id="radiolist1" runat="server" RepeatDirection="Horizontal" >
							   <asp:ListItem id="RbSQL" runat="server" selected="true">SQL</asp:ListItem>
							   <asp:ListItem id="RbMySQL"  runat="server">MySQL</asp:ListItem>
							</asp:RadioButtonList>
						</td>
					</tr>
					<tr class="Controls">
						<th style="width:40%">
							Servidor:
						</th>
						<td>
							<asp:TextBox ID="txtServer" runat="server" Width="96%"></asp:TextBox>
						</td>
					</tr>
					<tr class="Controls">
						<th>
							Usar autenticação do Windows:
						</th>
						<td align="left">
							<asp:CheckBox ID="cbxWinAut" runat="server" Width="96%" onclick="WinAut()"/>
						</td>
					</tr>
					<tr class="Controls">
						<th>
							Login:
						</th>
						<td>
							<asp:TextBox ID="txtUser" runat="server" Width="96%"></asp:TextBox>
						</td>
					</tr>
								
					<tr class="Controls">
						<th>
							Senha:
						</th>
						<td>
							<asp:TextBox ID="txtPassword" runat="server" Width="96%" TextMode="Password"></asp:TextBox>
						</td>
					</tr>
					<tr class="Controls">
						<th>
							Database:
						</th>
						<td>
							<asp:TextBox onkeydown="RefreshCombo = true;" ID="txtDatabase" runat="server" Width="96%"></asp:TextBox>
						</td>
					</tr>
					<tr class="Controls">
						<th>
							SQL FileName:
						</th>
						<td>
							<asp:TextBox onkeydown="RefreshCombo = true;" ID="txtFileName" runat="server" Width="96%"></asp:TextBox>
						</td>
					</tr>
					<tr class="Controls">
						<th>
							Usar banco temporário:
						</th>
						<td align="left">
							<asp:CheckBox ID="cbxTempDB" runat="server" Width="96%" onclick="ShowTempServer(this)" />
						</td>
					</tr>
					<tr id="RowServer" class="Controls" style="display: none">
						<th>
							Servidor Temporário:
						</th>
						<td>
							<asp:TextBox ID="txtServerTemp" runat="server" Width="96%"></asp:TextBox>
						</td>
					</tr>
					<tr id="RowAutTmp" class="Controls" style="display: none">
						<th>
							Usar autenticação do Windows:
						</th>
						<td align="left">
							<asp:CheckBox ID="cbxWinAutTmp" runat="server" Width="96%" onclick="WinAutTMP()"/>
						</td>
					</tr>
					<tr id="RowLogin" class="Controls" style="display: none">
						<th>
							Login:
						</th>
						<td>
							<asp:TextBox ID="txtUserServerTemp" runat="server" Width="96%"></asp:TextBox>
						</td>
					</tr>
					<tr id="RowPassword" class="Controls" style="display: none">
						<th>
							Senha:
						</th>
						<td>
							<asp:TextBox ID="txtPasswordServerTemp" runat="server" TextMode="Password" Width="96%"></asp:TextBox>
						</td>
					</tr>
					<tr class="Controls">
					<th>
						<img  class="ProgressBarInvisible" id="PgImage" alt="Title" src="../Images/loading1.gif" style="width: 96px; height: 21px; margin-bottom: 0px ;vertical-align:middle" />
					</th>
					<td align="right">
				
								<asp:Button ID="btn_RunAdapter" runat="server" Text="Adaptar" OnClientClick="SetVisible();" OnClick="btn_RunAdapter_Click"
								ValidationGroup="ValidateScript"/>
						</td>
					</tr>
				</table>
			</telerik:RadAjaxPanel>
		</div>
	</form>
</body>
	<script type="text/javascript">

		ShowClientFormulas();
		function ShowClientFormulas()
		{
		}
	</script>
</html>
