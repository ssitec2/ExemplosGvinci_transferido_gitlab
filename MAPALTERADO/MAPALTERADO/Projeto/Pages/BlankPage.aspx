<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" CodeFile="BlankPage.aspx.cs" Inherits="PROJETO.Pages.BlankPage" Culture="auto" UICulture="auto"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <title>BlankPage</title>
</head>
	<body>
	    <script language="JavaScript" type="text/javascript" src="../JS/Common.js"></script>
		<script language="JavaScript" type="text/javascript"> 
			function TryLogin(PageToRedirect, RefreshControlsID)
			{
				var isWindow = false;
				if (frameElement != null && frameElement.name.indexOf('gWinClass') != -1) 
				{
					isWindow = true;
				}
				TryParentLogin(PageToRedirect, RefreshControlsID, isWindow);
				try 
				{
					if (isWindow) 
					{
						var oWin = getParentPage().GetgWinsettings();
						var oArray = oWin.Windows.concat([]);
						for (i = 0; i < oArray.length; i++) 
						{
	                        if (oArray[i].Name == frameElement.name) 
	                        {
								oArray[i].Close();
								break;
							}
						}
					}
				}
				catch (ex)
				{
	
		        }
				TryParentLogin(PageToRedirect, RefreshControlsID);
			}
		</script>
		<form id="Form1" runat="server"/>
	</body>
	<script type="text/javascript">
		var Parameters = document.location.search;
		if (Parameters.length > 0)
		{
			var PageRedir = Parameters.substr(Parameters.indexOf("RequestingPage=") + 15);
			TryLogin(PageRedir, "");
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
		}
	</script>

</html>
