<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Status.aspx.cs" Inherits="PROJETO._Status" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
	<title>Adapta</title>
	<link rel="stylesheet" type="text/css" href="../css/Style.css" />

	<script type="text/javascript" language="javascript">
		var alertflag = false;
		function btnAlerAdapter() 
		{
			var resp = window.confirm('A execução desse script irá modificar a estrutura do seu banco de dados de forma irreversível.\r\n\r\n Deseja continuar?');
			if (resp) 
			{
				SetVisible();
				__doPostBack('btn_exec_Click', 'ClickAdap');
			}
			else 
			{
				return false;
			}
		}

		function AlertDropDatabase() 
		{
			var resp = window.confirm('Adaptação executada com sucesso!\r\n\r\nDeseja deletar o banco temporário?');
			if (resp) 
			{
				__doPostBack('exec_ClickAdap', 'DropDb');
			}
			else 
			{
				return false;
			}
		}

		function SetVisible() 
		{
			document.getElementById("PgImage").className = "ProgressBarVisible";
		}
	</script>
	<style type="text/css">
		.style1
		{
			width: 704px;
		}
		.style2
		{
			width: 704px;
			height: 17px;
		}
	</style>
</head>
<body>
	<div align="center">
		<form id="Form1" runat="server" style="width: 722px; height: 485px">
		<div>
			<asp:ScriptManager ID="MainScriptManager" runat="server"/>
			<telerik:RadAjaxPanel ID="ajaxpanel" runat="server">
				<table class="Controls">
					<tr>
						<td align="center" class="style1" colspan="2">
							<asp:Label ID="LbDataBase" runat="server" Style="font-size: medium; color:#3d84cc" Text=""/>
						</td>
					</tr>
					<tr>
						<td align="center" class="style1" colspan="2">
							<asp:Label ID="Label1" runat="server" Text="Alertas"></asp:Label>
						</td>
					</tr>
					<tr>
						<td class="style1" colspan="2">
							<asp:TextBox ID="TxtWarning" runat="server" Width="705px" Height="70px" Style="background-color: Silver"
								ReadOnly="true" TextMode="MultiLine" ForeColor="Red"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="center" class="style1" colspan="2">
							<asp:Label ID="LblDiffs" runat="server" Text="Diferenças Encontradas"></asp:Label>
						</td>
					</tr>
					<tr>
						<td class="style1" colspan="2">
							<asp:TextBox ID="txtDiffs" runat="server" Width="705px" Height="70px" Style="background-color: Silver"
								ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="center" class="style2" colspan="2">
							<asp:Label ID="Label2" runat="server" Text="Script de Adaptação"></asp:Label>
						</td>
					</tr>
					<tr>
						<td class="style1" colspan="2">
							<asp:TextBox ID="txtScript" runat="server" Width="705px" Height="280px" TextMode="MultiLine"></asp:TextBox>
						</td>
					</tr>
					<tr align="left">
						<th style="width:50%">
							<img id="PgImage" alt="Title" class="ProgressBarInvisible" src="../Images/loading1.gif" />
						</th>
						<td align="center">
							<asp:Button ID="btnBackup" runat="server" OnClick="btnBackup_Click" OnClientClick="SetVisible();"
							Text="Backup" Width="80px" />
							<asp:Button ID="btn_exec" runat="server" OnClientClick="btnAlerAdapter()" Style="margin-left: 10px;margin-right: 10px"
								Text="Adaptar" Width="80px" />
							<asp:Button ID="btnRestore" runat="server" OnClick="btnRestore_Click" OnClientClick="SetVisible();"
								 Text="Restaurar" Width="80px" />
						</td>
					</tr>
				</table>
				<asp:HiddenField ID="HiddenCanDeleteTemp" runat="server" />
			</telerik:RadAjaxPanel>
		</div>
		</form>
	</div>

	<script type="text/javascript" language="javascript">
		if (document.getElementById("HiddenCanDeleteTemp").value == "true") 
		{
			AlertDropDatabase();
			document.getElementById("HiddenCanDeleteTemp").value = "false";
		}
	</script>

</body>
</html>
