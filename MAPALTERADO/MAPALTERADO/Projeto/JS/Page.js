var vgWin;
var CurrentFilter = "";
var Default = this.parent;
var vgAjax;
var isError = false;
var isIE = (navigator.appName.indexOf("Microsoft") != -1 || navigator.userAgent.indexOf("Trident") != -1);
var isChrome = !isIE && (navigator.userAgent.indexOf("Chrome") != -1);
var isSafari = !isIE && !isChrome && (navigator.vendor.indexOf("Apple") != -1);
var isNN = !isIE && !isChrome && !isSafari && (navigator.appName.indexOf("Netscape") != -1);

var IsFromRelat = false;
function SetFilter(FilterText) 
{
	if (!IsFromRelat) 
	{
		SetCurrentFilter(FilterText);
	}
	else 
	{
		IsFromRelat = false;
		SetCurrentFilter(FilterText);
	}
}

function showFilter(AppPath) 
{
	var Path = AppPath + "Pages/Filtro.aspx?TableName=" + document.getElementById("__TABLENAME").value + "&DatabaseName=" + document.getElementById("__DATABASENAME").value + "&PageName=" + document.getElementById("__PAGENAME").value;
	if (typeof (vgWin) != "undefined" && vgWin)
	{
		vgWin.SetModal(true);
		getParentPage().FilteredPage = vgWin;
		vgWin.IframeDocument.location.href = Path;
	}
	else 
	{
		Navigate(Path);
	}
}

function DisableGrid(gridCtrl) {
	gridCtrl.get_element().disabled = "disabled";
	gridCtrl.ClientSettings.Resizing.AllowColumnResize = false;
	gridCtrl.ClientSettings.Resizing.AllowRowResize = false;
	gridCtrl.ClientSettings.AllowColumnsReorder = false;
	gridCtrl.ClientSettings.AllowDragToGroup = false;
	var links = gridCtrl.get_element().getElementsByTagName("a");
	var images = gridCtrl.get_element().getElementsByTagName("img");
	var inputs = gridCtrl.get_element().getElementsByTagName("input");
	var sortButtons = gridCtrl.get_element().getElementsByTagName("span");
	for (var i = 0; i < links.length; i++) {
		if (links[i].parentElement.id != gridCtrl.ClientID + "_ctl00NPPH") {
			links[i].href = "";
			links[i].onclick = function () {
				return false;
			}
		}
	}
	for (var i = 0; i < images.length; i++) {
		images[i].onclick = function () {
			return false;
		}
	}
	for (var i = 0; i < sortButtons.length; i++) {
		sortButtons[i].onclick = function () {
			return false;
		}
	}
	for (var i = 0; i < inputs.length; i++) {
		if (!inputs[i].className.startsWith("rgPage") && !inputs[i].className.startsWith("rgSort") && !inputs[i].className.startsWith("rgRefresh")) {
			switch (inputs[i].type) {
				case "button":
					inputs[i].onclick = function () {
						return false;
					}
					inputs[i].style.display = "none";
					break;
				case "checkbox":
					inputs[i].disabled = "disabled";
					break;
				case "radio":
					inputs[i].disabled = "disabled";
					break;
				case "text":
					inputs[i].disabled = "disabled";
					break;
				case "password":
					inputs[i].disabled = "disabled";
					break;
				case "image":
					inputs[i].onclick = function () {
						return false;
					}
					inputs[i].style.display = "none";
					break;
				case "file":
					inputs[i].disabled = "disabled";
					break;
				default:
					break;
			}
		}
	}
	var scrollArea = $find(gridCtrl.ClientID).GridDataDiv;
	if (scrollArea) {
		scrollArea.disabled = "disabled";
	}
}

function rowDblClick(sender, eventArgs) 
{
	if (!sender.get_element().disabled)
		sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
}

function SetGasWindow(oWin)
{
	vgWin = oWin;
}

function SetAjaxPanel(AjaxPanel) 
{
	vgAjax = AjaxPanel;
}

function New(sender)
{
	if(sender && sender.id)
		ExecuteCommandRequest("new", sender.id);
	else
		ExecuteCommandRequest("new");
}

function NewCopy(sender)
{
	if(sender && sender.id)
		ExecuteCommandRequest("new_copy", sender.id);
	else
		ExecuteCommandRequest("new_copy");
}

function Validate()
{
	if (jQuery('form').validationEngine && !jQuery('form').validationEngine('validate')) return false;
	ExecuteCommandRequest("validate");
}

function Save(sender)
{
	if (jQuery('form').validationEngine && !jQuery('form').validationEngine('validate')) return false;
	if(sender && sender.id)
		ExecuteCommandRequest("save", sender.id);
	else
		ExecuteCommandRequest("save");
}

function Cancel(sender)
{
	if(jQuery('form').validationEngine) jQuery('form').validationEngine('hide');
	if(sender && sender.id)
		ExecuteCommandRequest("cancel", sender.id);
	else
		ExecuteCommandRequest("cancel");
}

function Refresh(sender)
{
	if(sender && sender.id)
		ExecuteCommandRequest("refresh", sender.id);
	else
		ExecuteCommandRequest("refresh");
}

function Remove(Item, Confirm) {
     if (Item.attributes != undefined && Item.attributes["HasDatalistParent"] != null) {
        ExecuteCommandRequest("RemoveListItem", Item.name, Confirm);
    }
    else {
        ExecuteCommandRequest("remove", Item.id, Confirm == true);
    }
}

function Filter()
{
	ExecuteCommandRequest("filter");
}
function ApplyFilter() 
{
    ExecuteCommandRequest("setfilter");
}
function First(sender)
{
	if(sender && sender.id)
		ExecuteCommandRequest("first", sender.id);
	else
		ExecuteCommandRequest("first");
}
function Last(sender)
{
	if(sender && sender.id)
		ExecuteCommandRequest("last", sender.id);
	else
		ExecuteCommandRequest("last");
}
function Next(sender)
{
	if(sender && sender.id)
		ExecuteCommandRequest("next", sender.id);
	else
		ExecuteCommandRequest("next");
}
function Previous(sender)
{
	if(sender && sender.id)
		ExecuteCommandRequest("previous", sender.id);
	else
		ExecuteCommandRequest("previous");
}
function Close()
{
	ExecuteCommandRequest("close");
}
function OpenPage(PageName)
{
	ExecuteCommandRequest("open", PageName);
}
function InfDb()
{
	ExecuteCommandRequest("dbinfo");
}
function Edit(sender)
{
	if(sender && sender.id)
		ExecuteCommandRequest("edit", sender.id);
	else
		ExecuteCommandRequest("edit");
}
function ComboShowFormulas(a, b, c)
{
	ShowFormulas();
}
function ShowFormulas()
{
	ExecuteCommandRequest("showformulas");
}
function Logoff() 
{
	ExecuteCommandRequest("logoff");
}

function onCboChanged(a, b, c, e, oWin)
{
	onTextChanged(e);
}
function InitiateEdit()
{
	if (!MaskedControl)
		return;
	if (MaskedControl.attributes["EnableEdit"] && MaskedControl.attributes["EnableEdit"].value == "True") {
		try {
			var Doc = document;
			if (Doc.getElementById("__TABLENAME") != null) {
				var PageStateHidden = Doc.getElementById("__PAGESTATE");
				if (PageStateHidden.value == "Navigation") {
					PageStateHidden.value = "Edit";
				}
			}

		}
		catch (ex) {
		}
		try {
			EnableButtons();
		}
		catch (exc) {
		}
		try {
			if (getParentPage() != "undefined") {
				getParentPage().EnableButtons();
			}
		}
		catch (exc) {

		}
	}
}

function onTextChanged(e)
{
	var win = window;
	
	var isIE = (navigator.appName.indexOf("Microsoft") != -1);
	
	if (isIE)
	{
		var keycode = e.keyCode;
	}
	else
	{
		InitiateEdit();
		return;
	}

	if (e.altKey) return;
	if (e.ctrlKey) return;
	if (e.shiftKey) return;
	if (keycode == 9) return;

	var digit = (keycode >= 48 && keycode <= 57);
	var plus = (keycode == 43);
	var dash = (keycode == 45);
	var space = (keycode == 32);
	var back = (keycode == 8);
	var uletter = (keycode >= 65 && keycode <= 90);
	var lletter = (keycode >= 97 && keycode <= 122);

	if (digit || plus || dash || space || uletter || lletter || back || e != "undefined")
	{
		InitiateEdit();
	}
}

function ExecuteCommandRequest(CommandName, TargetControl, Confirm, Parameters)
{
	var CommandParameters = new Array();
	var vgAjax = window[window.vgAjax];
	// se Parameters existe
	if (Parameters)
	{
		// se Parameters não for Array
		if (!(typeof (Parameters) == 'object' && typeof (Parameters.length) == 'number' && !(Parameters.propertyIsEnumerable('length')) && typeof (Parameters.splice) == 'function'))
		{
			for (i = 3; i < ExecuteCommandRequest.arguments.length; i++)
			{
				CommandParameters[i - 3] = ExecuteCommandRequest.arguments[i];
			}
		}
		else
		{
			CommandParameters = Parameters;
		}
	}
	if (CommandName.substr(0, 7).toUpperCase() == "SERVER:")
	{
		CommandName = CommandName.substr(7);
	}
	if (typeof (Confirm) != "undefined" && Confirm)
	{
		if (CommandName.toLowerCase() == "remove" || CommandName.toLowerCase() ==  "removelistitem")
		{
			if (!confirm("Deseja remover o registro?"))
			{
				return false;
			}
		
		}
	}

	switch (CommandName.toString().toLowerCase())
	{
		case "close":
			if (typeof (vgWin) != "undefined")
			{
				Gasconfirm("Deseja Finalizar", EndCallBack, 200, 150);
				function EndCallBack(retval)
				{
					if (retval)
					{
						var oWin = GetgWinsettings();
						oWin.CloseAll();
						window.close();
					}
				}
			}
			return;
		default:
			if (typeof (vgAjax) != "undefined")	
			{
				vgAjaxControlID = vgAjax._uniqueID;
			}
			else
			{
				return;
			}
			var Command = "ExecuteCommand:" + CommandName;
			if (typeof(TargetControl) != "undefined" && TargetControl.length > 0)
			{
				Command = Command + "|" + "TargetControl:" + TargetControl
			}
			if (CommandParameters.length > 0)
			{
				Command = Command + "|" + "Parameters:"
				for (i = 0; i < CommandParameters.length; i++)
				{
					if (i > 0) Command = Command + "#";
					Command = Command + CommandParameters[i];
				}
			}
			vgAjax.ajaxRequestWithTarget(vgAjaxControlID, Command);
	}

}
