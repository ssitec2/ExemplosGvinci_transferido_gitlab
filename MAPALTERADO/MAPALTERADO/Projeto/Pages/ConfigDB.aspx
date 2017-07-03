<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="True" CodeFile="ConfigDB.aspx.cs" Inherits="PROJETO.ConfigDB" Culture="auto" UICulture="auto"%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=PROJETO.Utility.CurrentSiteLanguage%>">
<head runat="server">
	<title><asp:Literal runat="server" Text="<%$ Resources: Form1 %>" /></title>
	<script type="text/javascript">
		function OnLoginSucceded()
		{
			if(getParentPage() != self && getParentPage() != null)
			{
				getParentPage().OnLoginSucceded();
			}
		}
		function TryLogin(PageToRedirect, RefreshControlsID)
		{
			TryParentLogin(PageToRedirect, RefreshControlsID);
		}

      function GetWindow()   
      {   
        var oWindow = null;   
        if (window.gWin) oWindow = window.gWin;
        else if (window.frameElement && window.frameElement.gWin) oWindow = window.frameElement.gWin;   
        return oWindow;   
      }    
      function CloseWin()   
      {
      	var oWindow = GetWindow();
      	if (oWindow != null) {
      		this.parent.Reload();
      		oWindow.Close();
      	} else {
      		document.location.href = 'StartPage.aspx';
      	}
      }   
      function openDefault()
      {
      }
</script>
	<link rel="stylesheet" href="../Styles/ConfigDB.css" type="text/css" media="screen" title="no title" charset="utf-8" />
</head>
<script type="text/javascript" src="../JS/ConfigDB_USER.js" language="JavaScript"></script>
<body onload="InitializeClient();" id="Form1_body" style="margin-left:auto;margin-right:auto;">
	<script language="JavaScript" type="text/javascript" src="../JS/jquery.js" ></script>
	<script language="JavaScript" type="text/javascript" src="../JS/ConfigDB_USER.js"></script>
	<script language="JavaScript" type="text/javascript" src="../JS/Functions.js"></script>
	<script language="JavaScript" type="text/javascript" src="../JS/Common.js"></script>
	<script language="JavaScript" src='../JS/Mask.js' type="text/javascript"></script>
	<script language="JavaScript" type="text/javascript" src="../JS/RadComboBoxHelper.js"></script>
		
		<form id="Form1" runat="server">
			<asp:ScriptManager ID="MainScriptManager" runat="server"/>
			<div id="__MainDiv" class="c_MainDiv" FitToContent="True" MarginToContent="0">
				<div id="Div1" runat="server" AutoExpandToContent="False" AutoExpandToContentMargin="10">
					<div id="Div3" runat="server" AutoExpandToContent="False" AutoExpandToContentMargin="10">
						<asp:Label id="labModuleTitle" runat="server" Text="<%$ Resources: Form1 %>" />
					</div>
					<telerik:RadAjaxPanel id="ajxAjaxPanel" runat="server" LoadingPanelID="___ajxAjaxPanel_AjaxLoading">
						<telerik:RadTextBox id="txtServer" runat="server" AutoPostBack="False" EnableSingleInputRendering="True" MaxLength="0" ReadOnly="False"
							RenderMode="Classic" TabIndex="1" TextMode="SingleLine" WrapperCssClass="c_txtServer_wrapper" />
						<telerik:RadTextBox id="txtDataBase" runat="server" AutoPostBack="False" EnableSingleInputRendering="True" MaxLength="0" ReadOnly="False"
							RenderMode="Classic" TabIndex="2" TextMode="SingleLine" WrapperCssClass="c_txtDataBase_wrapper" />
						<telerik:RadCheckBox id="chkAutent" runat="server" AutoPostBack="True" Checked="False" CssClass="chkAutent"
							OnCheckedChanged="___chkAutent_OnCheckedChanged" RenderMode="Classic" TabIndex="3" Text="<%$ Resources: chkAutent %>" />
						<telerik:RadTextBox id="txtUser" runat="server" AutoPostBack="False" EnableSingleInputRendering="True" MaxLength="0" ReadOnly="False"
							RenderMode="Classic" TabIndex="4" TextMode="SingleLine" WrapperCssClass="c_txtUser_wrapper" />
						<telerik:RadTextBox id="txtPassword" runat="server" AutoPostBack="False" EnableSingleInputRendering="True" MaxLength="0" ReadOnly="False"
							RenderMode="Classic" TabIndex="5" TextMode="Password" WrapperCssClass="c_txtPassword_wrapper" />
						<telerik:RadComboBox id="cboDataType" runat="server" AllowCustomText="False" AutoPostBack="False" CollapseAnimation-Duration="300"
							CollapseAnimation-Type="None" EnableEmbeddedSkins="True" EnableVirtualScrolling="True" ExpandAnimation-Duration="300"
							ExpandAnimation-Type="None" LoadingMessage="<%$ Resources: cboDataType %>" MarkFirstMatch="true" MaxHeight="100"
							OnClientItemsRequesting="Combo_OnClientItemsRequesting" OnClientKeyPressing="Combo_HandleKeyPress" RenderMode="Classic" TabIndex="6" />
						<telerik:RadTextBox id="txtProvider" runat="server" AutoPostBack="False" EnableSingleInputRendering="True" MaxLength="0" ReadOnly="False"
							RenderMode="Classic" TabIndex="7" TextMode="SingleLine" WrapperCssClass="c_txtProvider_wrapper" />
						<telerik:RadLabel id="Label2" runat="server" Text="<%$ Resources: Label2 %>" />
						<telerik:RadLabel id="lbDbName" runat="server" Text="<%$ Resources: lbDbName %>" />
						<telerik:RadLabel id="labPassword" runat="server" Text="<%$ Resources: labPassword %>" />
						<telerik:RadLabel id="labUser" runat="server" Text="<%$ Resources: labUser %>" />
						<telerik:RadLabel id="Label7" runat="server" Text="<%$ Resources: Label7 %>" />
						<telerik:RadLabel id="Label9" runat="server" Text="<%$ Resources: Label9 %>" />
						<telerik:RadAjaxLoadingPanel ID="___ajxAjaxPanel_AjaxLoading" runat="server">
						</telerik:RadAjaxLoadingPanel>
					</telerik:RadAjaxPanel>
					<asp:Button id="butCreate" runat="server" CommandName="butCreate" OnClick="___butCreate_OnClick" TabIndex="8" Text="<%$ Resources: butCreate %>"
						/>
					<asp:Label id="labError" runat="server" class="Error" />
				</div>
			</div>
		</form>
</body>
	<script type="text/javascript">
		var $j = jQuery.noConflict();
		$j(document).ready(SetFocusFirstField());
		function SetFocusFirstField()
		{
			try
			{
				{
					window.focus();
					setTimeout("var $j = jQuery.noConflict();$j('#txtServer').first().focus();", 200);
				}
			}
			catch (e)
			{
			}
		}
		function AutenticacaoWindows() { return document.getElementById("chkAutent").value; }
		try
		{
			if(getParentPage() != self)
			{
				getParentPage().EnableButtons();
			}
		}
		catch (e)
		{
		}
		document.getElementById('txtPassword').value = document.getElementById('txtPassword').getAttribute('DefaultPassword');
	</script>
</html>
