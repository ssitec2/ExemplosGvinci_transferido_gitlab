<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="True" ValidateRequest="false" CodeFile="StartPage.aspx.cs" Inherits="PROJETO.StartPage" Culture="auto" UICulture="auto"%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=PROJETO.Utility.CurrentSiteLanguage%>">
<head runat="server">
	  <title><asp:Literal runat="server" Text="<%$ Resources: Form1 %>" /></title>
	<link rel="stylesheet" href="../Styles/validationEngine.jquery.css" type="text/css" media="screen" title="no title" charset="utf-8" />
	<link rel="stylesheet" href="../Styles/StartPage.css" type="text/css" media="screen" title="no title" charset="utf-8" />
</head>
<body onload="InitializeClient();" id="Form1_body" style="margin-left:auto;margin-right:auto;">
	<script language="JavaScript" type="text/javascript" src="../JS/jquery.js" ></script>
	<script language="JavaScript" type="text/javascript" src="../JS/Common.js"></script>
	<script language="JavaScript" type="text/javascript" src="../JS/Functions.js"></script>
	<script language="JavaScript" src='../JS/Mask.js' type="text/javascript"></script>
		
		<script type="text/javascript" src="../JS/StartPage_USER.js" language="JavaScript"></script>
	<script type="text/javascript">
		function OnLoginSucceded()
		{
			document.location.href='Default.aspx';return false;
		}
		function TryLogin(PageToRedirect, RefreshControlsID)
		{
			document.forms[0].RefreshControlsIDHidden.value = RefreshControlsID;
			document.forms[0].PageToRedirectHidden.value = PageToRedirect;
		}
	</script>
		<form id="Form1" runat="server">
		<input type="hidden" name="PageToRedirectHidden" value="" />
		<input type="hidden" name="RefreshControlsIDHidden" value="" />
			<asp:ScriptManager ID="MainScriptManager" runat="server"/>
			<div id="__MainDiv" class="c_MainDiv" FitToContent="True" MarginToContent="0">
				<telerik:RadAjaxPanel id="ajxMainAjaxPanel" runat="server" LoadingPanelID="___ajxMainAjaxPanel_AjaxLoading">
					<div id="Div1" runat="server" AutoExpandToContent="False" AutoExpandToContentMargin="10">
						<telerik:RadTextBox id="txtLoginUser" runat="server" AutoPostBack="False" EnableSingleInputRendering="True" MaxLength="0" ReadOnly="False"
							RenderMode="Classic" TabIndex="1" TextMode="SingleLine" WrapperCssClass="c_txtLoginUser_wrapper" />
						<telerik:RadLabel id="Label2" runat="server" Text="<%$ Resources: Label2 %>" />
						<asp:Button id="butDoLogin" runat="server" CommandName="butDoLogin" OnClick="___butDoLogin_OnClick" TabIndex="3"
							Text="<%$ Resources: butDoLogin %>" />
						<telerik:RadLabel id="Label1" runat="server" Text="<%$ Resources: Label1 %>" />
						<telerik:RadTextBox id="txtLoginPassword" runat="server" AutoPostBack="False" EnableSingleInputRendering="True" MaxLength="0" ReadOnly="False"
							RenderMode="Classic" TabIndex="2" TextMode="Password" WrapperCssClass="c_txtLoginPassword_wrapper" />
						<asp:Label id="labError" runat="server" class="Error" />
						<div id="Div2" runat="server" AutoExpandToContent="False" AutoExpandToContentMargin="10">
							<asp:Label id="labProjectTitle" runat="server" Text="Título do projeto" />
						</div>
					</div>
					<telerik:RadAjaxLoadingPanel ID="___ajxMainAjaxPanel_AjaxLoading" runat="server">
					</telerik:RadAjaxLoadingPanel>
				</telerik:RadAjaxPanel>
			</div>
		</form>
		
</body>
<script type="text/javascript">
	ShowClientFormulas();

	function ShowClientFormulas()
	{
	}
	var $j = jQuery.noConflict();
	$j(document).ready(SetFocusFirstField());
	function SetFocusFirstField()
	{
		try
		{
			{
				window.focus();
				setTimeout("var $j = jQuery.noConflict();$j('#txtLoginUser').first().focus();", 200);
			}
		}
		catch (e)
		{
		}
	}

</script>

</html>
