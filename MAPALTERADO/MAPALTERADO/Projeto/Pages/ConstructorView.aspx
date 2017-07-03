<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="True" CodeFile="ConstructorView.aspx.cs" Inherits="PROJETO.ConstructorView" Culture="auto" UICulture="auto"%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=PROJETO.Utility.CurrentSiteLanguage%>">
<head runat="server">
	<title><asp:Literal runat="server" Text="<%$ Resources: Form1 %>" /></title>
	<link rel="stylesheet" href="../Styles/ConstructorView.css" type="text/css" media="screen" title="no title" charset="utf-8" />
</head>
<body onload="InitializeClient();" id="Form1_body" style="margin-left:auto;margin-right:auto;">
	<script language="JavaScript" type="text/javascript" src="../JS/jquery.js" ></script>
	<script language="JavaScript" type="text/javascript" src="../JS/Common.js"></script>
	<script language="JavaScript" type="text/javascript" src="../JS/Functions.js"></script>
	<script language="JavaScript" src='../JS/Mask.js' type="text/javascript"></script>
	<script language="JavaScript" type="text/javascript" src="../JS/RadComboBoxHelper.js"></script>

<script type="text/javascript" src="../JS/ConstructorView_USER.js" language="JavaScript"></script>
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
	</script>

    <script language="JavaScript" src='../js/Page.js' type="text/javascript"></script>

    <script type="text/javascript">
        //cmbQueryName
        function txtQueryChanged() 
		{
            if (document.getElementById(ControlsPanel).disabled == false) 
			{
                Default.Gasconfirm("Ao editar a query manualmente o editor será desabilitado. Deseja continuar?", CallBack);
                function CallBack(retval) 
				{
                    if (retval) 
					{
                        ExecuteCommandRequest('SetCustomView');
                    }
                    else 
					{
                        ExecuteCommandRequest('SetManagedView');
                    }
                }
            }
        }
        
    </script>
		
		<form id="Form1" runat="server">
			<asp:ScriptManager ID="MainScriptManager" runat="server"/>
			<div id="__MainDiv" class="c_MainDiv" FitToContent="True" MarginToContent="0">
				<div id="Div1" runat="server" AutoExpandToContent="True" AutoExpandToContentMargin="10">
					<div id="Div5" runat="server" AutoExpandToContent="False" AutoExpandToContentMargin="10">
						<telerik:RadLabel id="Label24" runat="server" Text="<%$ Resources: Label24 %>" />
					</div>
					<telerik:RadAjaxPanel id="ControlsAjaxPanel" runat="server" LoadingPanelID="___ControlsAjaxPanel_AjaxLoading">
						<telerik:RadLabel id="Label1" runat="server" Text="<%$ Resources: Label1 %>" />
						<telerik:RadTextBox id="txtViews" runat="server" AutoPostBack="False" EnableSingleInputRendering="True" MaxLength="0" ReadOnly="False"
							RenderMode="Classic" TabIndex="1" TextMode="SingleLine" WrapperCssClass="c_txtViews_wrapper" />
						<telerik:RadComboBox id="cboDbs" runat="server" AllowCustomText="False" AutoPostBack="False" CollapseAnimation-Duration="300"
							CollapseAnimation-Type="None" EnableEmbeddedSkins="True" EnableVirtualScrolling="True" ExpandAnimation-Duration="300"
							ExpandAnimation-Type="None" LoadingMessage="<%$ Resources: cboDbs %>" MarkFirstMatch="true" MaxHeight="100"
							OnClientItemsRequesting="Combo_OnClientItemsRequesting" OnClientKeyPressing="Combo_HandleKeyPress" RenderMode="Classic" TabIndex="2" />
						<telerik:RadLabel id="Label2" runat="server" Text="<%$ Resources: Label2 %>" />
						<telerik:RadLabel id="Label23" runat="server" />
						<telerik:RadTextBox id="txtQuery" runat="server" AutoPostBack="False" EnableSingleInputRendering="True" ForeColor="#000000" MaxLength="0"
							ReadOnly="False" RenderMode="Classic" TabIndex="44" TextMode="MultiLine" WrapperCssClass="c_txtQuery_wrapper" />
						<asp:Panel id="ControlsPanel" runat="server" BorderStyle="None" BorderWidth="0" ScrollBars="Auto">
							<div style="position:absolute !important;left:0px;top:0px;width:650px;height:344px">
								<telerik:RadTabStrip id="TabControl1" runat="server" Align="Left" AutoPostBack="True" MultiPageID="TabControl1MultiPage"
									PerTabScrolling="False" RenderMode="Classic" ScrollButtonsPosition="Middle" ScrollChildren="True">
									<Tabs>
										<telerik:RadTab id="TabItem1" runat="server" Selected="true" Text="<%$ Resources: TabPage1 %>">
										</telerik:RadTab>
										<telerik:RadTab id="TabItem2" runat="server" Text="<%$ Resources: TabPage2 %>">
										</telerik:RadTab>
										<telerik:RadTab id="TabItem3" runat="server" Text="<%$ Resources: TabPage3 %>">
										</telerik:RadTab>
									</Tabs>
								</telerik:RadTabStrip>
								<telerik:RadMultiPage runat="server" ID="TabControl1MultiPage" BorderColor="#000000" BorderWidth="1" BorderStyle="Solid" Width="100%" Height="318px" SelectedIndex="0">
									<telerik:RadPageView id="TabPage1" runat="server" BackColor="#FFFFFF">
										<asp:ListBox id="lstTables" runat="server" SelectionMode="Single" TabIndex="3" />
										<telerik:RadLabel id="Label3" runat="server" Text="<%$ Resources: Label3 %>" />
										<telerik:RadLabel id="Label4" runat="server" Text="<%$ Resources: Label4 %>" />
										<asp:ListBox id="lstFields" runat="server" SelectionMode="Single" TabIndex="5" />
										<asp:Button id="butAddField" runat="server" CommandName="butAddField" OnClick="___butAddField_OnClick"
											OnClientClick="this.disabled = true;" TabIndex="6" />
										<asp:Button id="butDelField" runat="server" CommandName="butDelField" OnClick="___butDelField_OnClick"
											OnClientClick="this.disabled = true;" TabIndex="7" />
										<telerik:RadLabel id="Label22" runat="server" Text="<%$ Resources: Label22 %>" />
										<asp:ListBox id="lstSelectedField" runat="server" SelectionMode="Single" TabIndex="8" />
										<asp:Button id="butUpField" runat="server" CommandName="butUpField" OnClick="___butUpField_OnClick" OnClientClick="this.disabled = true;"
											TabIndex="9" />
										<asp:Button id="butDownField" runat="server" CommandName="butDownField" OnClick="___butDownField_OnClick"
											OnClientClick="this.disabled = true;" TabIndex="10" />
										<telerik:RadLabel id="Label5" runat="server" Text="<%$ Resources: Label5 %>" />
										<telerik:RadLabel id="Label6" runat="server" Text="<%$ Resources: Label6 %>" />
										<telerik:RadLabel id="Label7" runat="server" Text="<%$ Resources: Label7 %>" />
										<telerik:RadComboBox id="cboColumnContent" runat="server" AllowCustomText="False" AutoPostBack="False" CollapseAnimation-Duration="300"
											CollapseAnimation-Type="None" EnableEmbeddedSkins="True" EnableVirtualScrolling="True" ExpandAnimation-Duration="300"
											ExpandAnimation-Type="None" LoadingMessage="<%$ Resources: cboColumnContent %>" MarkFirstMatch="true" MaxHeight="100"
											OnClientItemsRequesting="Combo_OnClientItemsRequesting" OnClientKeyPressing="Combo_HandleKeyPress" RenderMode="Classic" TabIndex="12" />
										<telerik:RadComboBox id="cboColumnFunc" runat="server" AllowCustomText="False" AutoPostBack="False" CollapseAnimation-Duration="300"
											CollapseAnimation-Type="None" EnableEmbeddedSkins="True" EnableVirtualScrolling="True" ExpandAnimation-Duration="300"
											ExpandAnimation-Type="None" LoadingMessage="<%$ Resources: cboColumnFunc %>" MarkFirstMatch="true" MaxHeight="100"
											OnClientItemsRequesting="Combo_OnClientItemsRequesting" OnClientKeyPressing="Combo_HandleKeyPress" RenderMode="Classic" TabIndex="11" />
										<telerik:RadTextBox id="txtColumnTitle" runat="server" AutoPostBack="False" EnableSingleInputRendering="True" ForeColor="#000000"
											MaxLength="0" ReadOnly="False" RenderMode="Classic" TabIndex="13" TextMode="SingleLine" WrapperCssClass="c_txtColumnTitle_wrapper" />
										<asp:Button id="butNewCol" runat="server" CommandName="butNewCol" OnClick="___butNewCol_OnClick" TabIndex="14"
											Text="<%$ Resources: butNewCol %>" />
										<asp:Button id="butDelJoin" runat="server" CommandName="butDelJoin" OnClick="___butDelJoin_OnClick" OnClientClick="this.disabled = true;"
											TabIndex="4" />
									</telerik:RadPageView>
									<telerik:RadPageView id="TabPage2" runat="server" BackColor="#FFFFFF">
										<asp:ListBox id="lstGroup" runat="server" SelectionMode="Single" TabIndex="15" />
										<telerik:RadLabel id="Label8" runat="server" Text="<%$ Resources: Label8 %>" />
										<telerik:RadLabel id="Label9" runat="server" Text="<%$ Resources: Label9 %>" />
										<asp:ListBox id="lstOrder" runat="server" SelectionMode="Single" TabIndex="21" />
										<asp:ListBox id="lstSelectedGroupBy" runat="server" SelectionMode="Single" TabIndex="18" />
										<telerik:RadLabel id="Label10" runat="server" Text="<%$ Resources: Label10 %>" />
										<telerik:RadLabel id="Label11" runat="server" Text="<%$ Resources: Label11 %>" />
										<asp:ListBox id="lstSelectedOrderBy" runat="server" SelectionMode="Single" TabIndex="24" />
										<asp:RadioButton id="rdbAsc" runat="server" AutoPostBack="false" BorderStyle="None" BorderWidth="0" Checked="False" class="rdbAsc"
											GroupName="TabPage2Group" TabIndex="27" Text="<%$ Resources: rdbAsc %>" />
										<asp:Button id="butUpOrderBy" runat="server" CommandName="butUpOrderBy" OnClick="___butUpOrderBy_OnClick"
											OnClientClick="this.disabled = true;" TabIndex="25" />
										<asp:Button id="butDownGroupBy" runat="server" CommandName="butDownGroupBy" OnClick="___butDownGroupBy_OnClick"
											OnClientClick="this.disabled = true;" TabIndex="20" />
										<asp:Button id="butUpGroupBy" runat="server" CommandName="butUpGroupBy" OnClick="___butUpGroupBy_OnClick"
											OnClientClick="this.disabled = true;" TabIndex="19" />
										<asp:Button id="butDownOrderBy" runat="server" CommandName="butDownOrderBy" OnClick="___butDownOrderBy_OnClick"
											OnClientClick="this.disabled = true;" TabIndex="26" />
										<asp:RadioButton id="rdbDesc" runat="server" AutoPostBack="false" BorderStyle="None" BorderWidth="0" Checked="False" class="rdbDesc"
											GroupName="TabPage2Group" TabIndex="28" Text="<%$ Resources: rdbDesc %>" />
										<asp:Button id="butDelGroupBy" runat="server" CommandName="butDelGroupBy" OnClick="___butDelGroupBy_OnClick"
											OnClientClick="this.disabled = true;" TabIndex="17" />
										<asp:Button id="butDelOrderBy" runat="server" CommandName="butDelOrderBy" OnClick="___butDelOrderBy_OnClick"
											OnClientClick="this.disabled = true;" TabIndex="23" />
										<asp:Button id="butAddGroup" runat="server" CommandName="butAddGroup" OnClick="___butAddGroup_OnClick"
											OnClientClick="this.disabled = true;" TabIndex="16" />
										<asp:Button id="butAddOrder" runat="server" CommandName="butAddOrder" OnClick="___butAddOrder_OnClick"
											OnClientClick="this.disabled = true;" TabIndex="22" />
									</telerik:RadPageView>
									<telerik:RadPageView id="TabPage3" runat="server" BackColor="#FFFFFF">
										<telerik:RadTextBox id="txtTopRegisters" runat="server" AutoPostBack="False" EnableSingleInputRendering="True" MaxLength="0"
											ReadOnly="False" RenderMode="Classic" TabIndex="29" TextMode="SingleLine" WrapperCssClass="c_txtTopRegisters_wrapper" />
										<telerik:RadLabel id="Label12" runat="server" Text="<%$ Resources: Label12 %>" />
										<telerik:RadComboBox id="cboJoinType" runat="server" AllowCustomText="False" AutoPostBack="False" CollapseAnimation-Duration="300"
											CollapseAnimation-Type="None" EnableEmbeddedSkins="True" EnableVirtualScrolling="True" ExpandAnimation-Duration="300"
											ExpandAnimation-Type="None" LoadingMessage="<%$ Resources: cboJoinType %>" MarkFirstMatch="true" MaxHeight="100"
											OnClientItemsRequesting="Combo_OnClientItemsRequesting" OnClientKeyPressing="Combo_HandleKeyPress" RenderMode="Classic" TabIndex="30" />
										<telerik:RadLabel id="Label14" runat="server" Text="<%$ Resources: Label14 %>" />
										<telerik:RadComboBox id="cboJoinTable" runat="server" AllowCustomText="False" AutoPostBack="False" CollapseAnimation-Duration="300"
											CollapseAnimation-Type="None" EnableEmbeddedSkins="True" EnableVirtualScrolling="True" ExpandAnimation-Duration="300"
											ExpandAnimation-Type="None" LoadingMessage="<%$ Resources: cboJoinTable %>" MarkFirstMatch="true" MaxHeight="100"
											OnClientItemsRequesting="Combo_OnClientItemsRequesting" OnClientKeyPressing="Combo_HandleKeyPress" RenderMode="Classic" TabIndex="33" />
										<telerik:RadComboBox id="cboJoinField" runat="server" AllowCustomText="False" AutoPostBack="False" CollapseAnimation-Duration="300"
											CollapseAnimation-Type="None" EnableEmbeddedSkins="True" EnableVirtualScrolling="True" ExpandAnimation-Duration="300"
											ExpandAnimation-Type="None" LoadingMessage="<%$ Resources: cboJoinField %>" MarkFirstMatch="true" MaxHeight="100"
											OnClientItemsRequesting="Combo_OnClientItemsRequesting" OnClientKeyPressing="Combo_HandleKeyPress" RenderMode="Classic" TabIndex="31" />
										<telerik:RadComboBox id="cboJoinOperator" runat="server" AllowCustomText="False" AutoPostBack="False" CollapseAnimation-Duration="300"
											CollapseAnimation-Type="None" EnableEmbeddedSkins="True" EnableVirtualScrolling="True" ExpandAnimation-Duration="300"
											ExpandAnimation-Type="None" LoadingMessage="<%$ Resources: cboJoinOperator %>" MarkFirstMatch="true" MaxHeight="100"
											OnClientItemsRequesting="Combo_OnClientItemsRequesting" OnClientKeyPressing="Combo_HandleKeyPress" RenderMode="Classic" TabIndex="34" />
										<telerik:RadComboBox id="cboJoinBaseField" runat="server" AllowCustomText="False" AutoPostBack="False" CollapseAnimation-Duration="300"
											CollapseAnimation-Type="None" EnableEmbeddedSkins="True" EnableVirtualScrolling="True" ExpandAnimation-Duration="300"
											ExpandAnimation-Type="None" LoadingMessage="<%$ Resources: cboJoinBaseField %>" MarkFirstMatch="true" MaxHeight="100"
											OnClientItemsRequesting="Combo_OnClientItemsRequesting" OnClientKeyPressing="Combo_HandleKeyPress" RenderMode="Classic" TabIndex="32" />
										<telerik:RadLabel id="Label15" runat="server" Text="<%$ Resources: Label15 %>" />
										<telerik:RadLabel id="Label16" runat="server" Text="<%$ Resources: Label16 %>" />
										<telerik:RadLabel id="Label17" runat="server" Text="<%$ Resources: Label17 %>" />
										<telerik:RadLabel id="Label13" runat="server" Text="<%$ Resources: Label13 %>" />
										<asp:Button id="butAddJoin" runat="server" CommandName="butAddJoin" OnClick="___butAddJoin_OnClick" OnClientClick="this.disabled = true;"
											TabIndex="36" />
										<telerik:RadLabel id="Label18" runat="server" Text="<%$ Resources: Label18 %>" />
										<telerik:RadTextBox id="txtJoinAlias" runat="server" AutoPostBack="False" EnableSingleInputRendering="True" MaxLength="0" ReadOnly="False"
											RenderMode="Classic" TabIndex="35" TextMode="SingleLine" WrapperCssClass="c_txtJoinAlias_wrapper" />
										<telerik:RadLabel id="Label26" runat="server" Text="<%$ Resources: Label26 %>" />
										<telerik:RadComboBox id="cboField" runat="server" AllowCustomText="False" AutoPostBack="False" CollapseAnimation-Duration="300"
											CollapseAnimation-Type="None" EnableEmbeddedSkins="True" EnableVirtualScrolling="True" ExpandAnimation-Duration="300"
											ExpandAnimation-Type="None" LoadingMessage="<%$ Resources: cboField %>" MarkFirstMatch="true" MaxHeight="100"
											OnClientItemsRequesting="Combo_OnClientItemsRequesting" OnClientKeyPressing="Combo_HandleKeyPress" RenderMode="Classic" TabIndex="37" />
										<telerik:RadComboBox id="cboOperator" runat="server" AllowCustomText="False" AutoPostBack="False" CollapseAnimation-Duration="300"
											CollapseAnimation-Type="None" EnableEmbeddedSkins="True" EnableVirtualScrolling="True" ExpandAnimation-Duration="300"
											ExpandAnimation-Type="None" LoadingMessage="<%$ Resources: cboOperator %>" MarkFirstMatch="true" MaxHeight="100"
											OnClientItemsRequesting="Combo_OnClientItemsRequesting" OnClientKeyPressing="Combo_HandleKeyPress" RenderMode="Classic" TabIndex="38" />
										<telerik:RadLabel id="Label20" runat="server" Text="<%$ Resources: Label20 %>" />
										<telerik:RadComboBox id="cboValue" runat="server" AllowCustomText="True" AutoPostBack="False" CollapseAnimation-Duration="300"
											CollapseAnimation-Type="None" EnableEmbeddedSkins="True" EnableVirtualScrolling="True" ExpandAnimation-Duration="300"
											ExpandAnimation-Type="None" LoadingMessage="<%$ Resources: cboValue %>" MarkFirstMatch="true" MaxHeight="100"
											OnClientItemsRequesting="Combo_OnClientItemsRequesting" OnClientKeyPressing="Combo_HandleKeyPress" RenderMode="Classic" TabIndex="39" />
										<telerik:RadLabel id="Label21" runat="server" Text="<%$ Resources: Label21 %>" />
										<asp:RadioButton id="rdbAnd" runat="server" AutoPostBack="false" BorderStyle="None" BorderWidth="0" Checked="False" class="rdbAnd"
											GroupName="TabPage3Group" TabIndex="40" Text="<%$ Resources: rdbAnd %>" />
										<asp:RadioButton id="rdbOr" runat="server" AutoPostBack="false" BorderStyle="None" BorderWidth="0" Checked="False" class="rdbOr"
											GroupName="TabPage3Group" TabIndex="41" Text="<%$ Resources: rdbOr %>" />
										<asp:Button id="butAddFilter" runat="server" CommandName="butAddFilter" OnClick="___butAddFilter_OnClick"
											OnClientClick="this.disabled = true;" TabIndex="42" />
										<asp:Button id="butClearFilter" runat="server" CommandName="butClearFilter" OnClick="___butClearFilter_OnClick"
											OnClientClick="this.disabled = true;" TabIndex="43" />
										<telerik:RadLabel id="Label19" runat="server" Text="<%$ Resources: Label19 %>" />
										<telerik:RadLabel id="Label25" runat="server" Text="<%$ Resources: Label25 %>" />
										<Div id="Line1">
										</Div>
									</telerik:RadPageView>
								</telerik:RadMultiPage>
							</div>
						</asp:Panel>
						<asp:Button id="butSave" runat="server" CommandName="butSave" OnClick="___butSave_OnClick" TabIndex="46" Text="<%$ Resources: butSave %>" />
						<asp:Button id="butTest" runat="server" CommandName="butTest" OnClick="___butTest_OnClick" TabIndex="45" Text="<%$ Resources: butTest %>" />
						<telerik:RadLabel id="ResultLabel" runat="server" />
						<telerik:RadAjaxLoadingPanel ID="___ControlsAjaxPanel_AjaxLoading" runat="server">
						</telerik:RadAjaxLoadingPanel>
					</telerik:RadAjaxPanel>
				</div>
			</div>
		</form>
</body>
	<script type="text/javascript">
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
	</script>

</html>
