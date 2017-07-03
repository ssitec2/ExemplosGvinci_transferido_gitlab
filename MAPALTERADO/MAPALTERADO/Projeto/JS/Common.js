var isIE = (navigator.appName.indexOf("Microsoft") != -1 || navigator.userAgent.indexOf("Trident") != -1);
var isChrome = !isIE && (navigator.userAgent.indexOf("Chrome") != -1);
var isSafari = !isIE && !isChrome && (navigator.vendor.indexOf("Apple") != -1);
var isNN = !isIE && !isChrome && !isSafari && (navigator.appName.indexOf("Netscape") != -1);

var BrowserVersion = GetBrowserVersion();
var BrowserType = GetBrowserType();

var IsAjaxProcessing = false;

var vgErrorLabel;

function autoTab(e) {
	var input = e.currentTarget;
	var ind = 0;
	var keyCode = (isNN || isChrome || isSafari) ? e.which : e.keyCode;
	var nKeyCode = e.keyCode;
	var hasTabIndex = input.tabIndex != 0 ? true : false;
	var controlTabIndex = input.tabIndex;
	var higherTabIndex = getHigherTabIndex();
	var isMultiLine = input.rows != undefined ? true : false;
	if (keyCode == 13 && !isMultiLine) {
		var $j = jQuery.noConflict();
		$j(input).off('keydown', autoTab);
		//if (!isNN || !isChrome || !isSafari) { window.event.keyCode = 0; } // evitar o beep  
		ind = getIndex(input);
		if (input.form[ind].type == 'textarea') {
			return;
		}
		while (ind <= input.form.length) {
			try {
				ind++;
				if (controlTabIndex == higherTabIndex) {
					hasTabIndex = false;
					ind = 0;
					controlTabIndex = input.form[ind].tabIndex;
				}
				if ($j(input.form[ind]).is(':visible') && !$j(input.form[ind]).is(':hidden') && $j(input.form[ind]).css("visibility") != "hidden" && !hasTabIndex) {
					if (input.form[ind].tabIndex == 0) {
						input.form[ind].focus();
						if (input.form[ind] != undefined) {
							if (input.form[ind].type == 'text') {
								input.form[ind].select();
							}
						}
						e.preventDefault();
						break;
					}
				}
				else if (hasTabIndex) {
					var x = 2;
					input = GetByTabIndex(controlTabIndex + 1);
					while (input == null || input.localName != "input") {
						input = GetByTabIndex(controlTabIndex + x);
						while (input == null) {
							x++;
							input = GetByTabIndex(controlTabIndex + x);
						}
						x++;
					}
					input.focus();
					if (input.type == 'text') {
						input.select();
					}
					e.preventDefault();
					break;
				}
				else if(ind > input.form.length){
					input = GetByTabIndex(1);
					input.focus();
					if (input.type == 'text') {
						input.select();
					}
					e.preventDefault();
					break
				}
				
			}
			catch (ex) {
				break;
			}
		}
	}

	function getHigherTabIndex() {
		var higherTabIndex = 0;
		if (input.form) {
			for (i = 0; i < input.form.length; i++) {
				if (input.form[i].tabIndex > higherTabIndex) {
					higherTabIndex = input.form[i].tabIndex;
				}
			}
		}
		return higherTabIndex;
	}

	function GetByTabIndex(index) {
		var obj = $j("[TabIndex='" + index + "']");
		if (obj.length > 0) {
			obj = obj[0];
		}
		else {
			obj = null;
		}
		return obj;
	}

	function getIndex(input) {
		var index = -1, i = 0, found = false;
		if (input.form) {
			while (i < input.form.length && index == -1)
				if (input.form[i] == input) {
					index = i;
					if (i < (input.form.length - 1)) {
						if (input.form[i + 1].type == 'hidden') {
							index++;
						}
						if (input.form[i + 1].type == 'button' && input.form[i + 1].id == 'tabstopfalse') {
							index++;
						}
					}
				}
				else
					i++;
		}
		return index;
	}
}

function ControlOnFocus(e, isGrid) {
	var sender = e;
	this.isGrid = isGrid;
	if (e.srcElement) {
		sender = e.srcElement;
	}
	else {
		sender = e.target;
	}
	var $j = jQuery.noConflict();

	var $sender = $j(sender);

	var Mask = $sender.attr("Mask");
	if (typeof (Mask) != "undefined" && Mask != null && Mask.length > 0) {
		SetMask($sender, true);
	}
	$sender.off('keydown', autoTab);
	$sender.keydown(autoTab);
}

function SetControlOnFocus(input) {
	var $j = jQuery.noConflict();
	$j(document).ready(function () {
		$j("#" + input).on("focusin", ControlOnFocus);
	});
}

function HideDatePickerValidation(sender, args) 
{
	try 
	{
		var $ = jQuery.noConflict();
		var senderID = sender.get_id();
		var ID = senderID.substr(0, senderID.indexOf("_calendar"));
		var InputElement = $($("#" + ID)[0].control._dateInput._element);
		InputElement.validationEngine('hide');
	}
	catch (ex) { }
}

function setDatePickerFocus(sender) {
	sender.get_dateInput().focus();
}


var ScrollBarHeight = 0;
var ScrollBarWidth = 0;

function getScrollBarWidth() {
	var inner = document.createElement('p');
	inner.style.width = "100%";
	inner.style.height = "200px";

	var outer = document.createElement('div');
	outer.style.position = "absolute";
	outer.style.top = "0px";
	outer.style.left = "0px";
	outer.style.visibility = "hidden";
	outer.style.width = "200px";
	outer.style.height = "150px";
	outer.style.overflow = "hidden";
	outer.appendChild(inner);

	document.body.appendChild(outer);
	var w1 = inner.offsetWidth;
	outer.style.overflow = 'scroll';
	var w2 = inner.offsetWidth;
	if (w1 == w2) w2 = outer.clientWidth;

	document.body.removeChild(outer);

	return (w1 - w2);
};

function getScrollBarHeight() {
	var inner = document.createElement('p');
	inner.style.width = "200px";
	inner.style.height = "100%";

	var outer = document.createElement('div');
	outer.style.position = "absolute";
	outer.style.top = "0px";
	outer.style.left = "0px";
	outer.style.visibility = "hidden";
	outer.style.width = "150px";
	outer.style.height = "200px";
	outer.style.overflow = "hidden";
	outer.appendChild(inner);

	document.body.appendChild(outer);
	var h1 = inner.offsetHeight;
	outer.style.overflow = 'scroll';
	var h2 = inner.offsetHeight;
	if (h1 == h2) h2 = outer.clientHeight;

	document.body.removeChild(outer);

	return (h1 - h2);
};

function GetScrollSizes() {
    if (ScrollBarHeight == 0 || ScrollBarWidth == 0) {
    	ScrollBarWidth = getScrollBarWidth();
    	ScrollBarHeight = getScrollBarHeight();
    }
}

function InitiateEditAuto() {

		try {
			var Doc = document;
			if (Doc.getElementById("__TABLENAME") != null) {
				var PageStateHidden = Doc.getElementById("__PAGESTATE");
				if (PageStateHidden.value == "Navigation") {
					PageStateHidden.value = "Edit";
					EnableButtons();
					if (getParentPage() != "undefined") {
						getParentPage().EnableButtons();
					}
				}
			}
		}
		catch (exc) {

		}

	}

function InitializeClient() 
{
	if (typeof (LoadEditAuto) != "undefined" && LoadEditAuto) LoadEditAuto();
    GetScrollSizes();
    UpdateDivAutoExpandToContent();
    UpdateDivAutoExpandWidth();
    FixRepeaterRegionsWidths();
	resizeIframe();
	if (typeof (jQuery) != "undefined" && jQuery && typeof (HasValidation) != "undefined" && HasValidation) 
	{
		var $j = jQuery.noConflict();
		$j(document).ready(function () 
		{
			$j("form").validationEngine();
		});
	}
	if (typeof (EnableButtons) != "undefined" && EnableButtons) EnableButtons();
	try 
	{
		if (getParentPage() != self) 
		{
			getParentPage().EnableButtons();
		}
	}
	catch (e) { }
}

function PrintFunction(Content)
{
       
        var printIframe = document.createElement("IFRAME");
        document.body.appendChild(printIframe);
        var printDocument = printIframe.contentWindow.document;
        printDocument.designMode = "on";
        printDocument.open();
        var currentLocation = document.location.href;
        currentLocation = currentLocation.substring(0, currentLocation.lastIndexOf("/") + 1);
        printDocument.write("<html><head></head><body>" + Content + "</body></html>");
        printDocument.close();
        try {
            if (document.all) {
                var oLink = printDocument.createElement("link");
                oLink.setAttribute("href", currentLocation + "PrintWithStyles.css", 0);
                oLink.setAttribute("type", "text/css");
                oLink.setAttribute("rel", "stylesheet", 0);
                printDocument.getElementsByTagName("head")[0].appendChild(oLink);
                printDocument.execCommand("Print");
            }
            else {
                printDocument.body.innerHTML = "<link rel='stylesheet' type='text/css' href='" + currentLocation + "PrintWithStyles.css'></link>" + printDocument.body.innerHTML;
                printIframe.contentWindow.print();
            }
        }
        catch (ex) {
        }
        document.body.removeChild(printIframe);
      
};

function HasValue(obj) 
{
	return typeof (obj) != "undefined" && obj != null && obj;
}

function FindIdInObject(obj) 
{
	if (!HasValue(obj)) return "";
	var objID = "";
	if (HasValue(obj.get_id)) objID = obj.get_id();
	if (typeof (objID) == "undefined" || !objID) 
	{
		if (HasValue(obj.srcElement)) objID = obj.srcElement.id;
		if ((typeof (objID) == "undefined" || !objID) && HasValue(obj.id)) objID = obj.id;
	}
	return objID;
}

function SetErrorLabel(ErrorLabel) 
{
    vgErrorLabel = ErrorLabel;
}

function ReplaceAll(string, token, newtoken) {
	while (string.indexOf(token) != -1) {
		string = string.replace(token, newtoken);
	}
	return string;
}

function SetCurrentFilter(FiltroVal)
{
    getParentPage().FilteredPage.Iframe.contentWindow.window.CurrentFilter = FiltroVal;
    getParentPage().FilteredPage.Iframe.contentWindow.window.ExecuteCommandRequest("setfilter", "", false, FiltroVal);
}

function setLookUpValue(Target, Value, ControlName)
{
	var obj = this;
	if (Target != "") obj = FindObjectByID(Target);
    obj.window.setLookUpFormValue(Value, ControlName);
}

function UpdateDivSize(div, margin) {
    if (div.attr("AutoExpandToContent") == "True") {
        if (margin == null || typeof (margin) == "undefined") {
            margin = div.attr("AutoExpandToContentMargin");
            if (margin == null || typeof (margin) == "undefined") {
                margin = 0;
            }
        }
        var OldOverflow = div.css('overflow');
        div.css('overflow', 'hidden');
        div.height(0);
        var newHeight = div.prop("scrollHeight") + parseInt(margin);
        if (div[0].scrollWidth > div[0].clientWidth && (div.css('right') != 'auto' && div.css('left') != 'auto') && !(div.css('right') == '0px' && div.css('left') == '0px')) newHeight += ScrollBarHeight;
        div.height(newHeight);
        div.css('overflow', OldOverflow);
    }
    if (div.attr("AutoExpandWidth") == "True") 
    {
        var OldOverflow = div.css('overflow');
        div.css('overflow', 'hidden');
        var OldWidth =  div.width();
		if (OldWidth > 0) 
		{
			div.width(0);
			var newWidth = div.prop("scrollWidth");

			if (OldWidth > newWidth) 
			{
				div.width(OldWidth);
			}
			else 
			{
				div.width(newWidth);
			}
		}
		div.css('overflow', OldOverflow);
    }
}

function UpdateDivAutoExpandToContent() {
	GetScrollSizes();
    if (jQuery != null && jQuery != undefined) {

        var $ = jQuery.noConflict();
        $(document).ready(function () {
            var objs = [];
            $('div[AutoExpandToContent="True"]').each(function (index, obj) {
                objs.push({ index: $(obj).parents().length, value: $(obj) });
            });
            objs.sort(function (obj1, obj2) {
                return obj2.index - obj1.index;
            });
            $.each(objs, function (index, obj) {
                UpdateDivSize($(obj)[0].value);
            });
        });
    }
}

function FixRepeaterRegionsWidths() {
    if (jQuery != null && jQuery != undefined) {

        var $ = jQuery.noConflict();
        $(document).ready(function () {
            var objs = [];
            $('div[EquateRepeaterRegionsWidth="True"]').each(function (index, obj) {
                objs.push({ index: $(obj).parents().length, value: $(obj) });
            });
            objs.sort(function (obj1, obj2) {
                return obj2.index - obj1.index;
            });
            $.each(objs, function (index, obj) {
                UpdateRepeaterDivWidths($(obj)[0].value);
            });
        });
    }
}

function UpdateDivAutoExpandWidth() {
    if (jQuery != null && jQuery != undefined) {

        var $ = jQuery.noConflict();
        $(document).ready(function () {
            var objs = [];
            $('div[AutoExpandWidth="True"]').each(function (index, obj) {
                objs.push({ index: $(obj).parents().length, value: $(obj) });
            });
            objs.sort(function (obj1, obj2) {
                return obj2.index - obj1.index;
            });
            $.each(objs, function (index, obj) {
                UpdateDivSize($(obj)[0].value);
            });
        });
    }
}

function UpdateRepeaterDivWidths(repeater) {
    if (jQuery != null && jQuery != undefined) {
        var $ = jQuery.noConflict();
        $(document).ready(function () {
            var objs = [];
            var Widest = 0;
            $(repeater).children('div[AutoExpandWidth="True"]').each(function (index, obj) {
                if ($(obj).width() > Widest) Widest = $(obj).width();
                objs.push($(obj));
            });
            $.each(objs, function (index, obj) {
                if (Widest > 0) {
					$(obj).width(Widest);
				}
            });
        });
    }
}

function UpdateRepeaterRegionSize(divID, margin)
{
	try {
        UpdateDivSize(document.getElementById(divID), margin);
	}
	catch (ex)
	{
	}
}

function FindObjectByID(objectName)
{
    var obj = null;
    var TargetsNames= objectName.split("|");
    var obj = this;
    for (t = 0; t < TargetsNames.length; t++) 
	{
        for (var i = 0; i < obj.frames.length; i++)
		{
            var att = obj.frames[i].frameElement.id;
            if (att != null && att == TargetsNames[t])
			{
                obj = obj.frames[i];
                break;
            }
        }
    }
    
    if (obj != null) {
        return obj;
    }
    else {
        var ParentPage = getParentPage();
        if (ParentPage != null && typeof (ParentPage) != "undefined" && ParentPage != self) {
            return ParentPage.FindObjectByID(objectName);
        }
    }
    return null;
}

function GetCurrentFilter()
{
    if (getParentPage().FilteredPage.Iframe.contentWindow.window.CurrentFilter == null) getParentPage().FilteredPage.Iframe.contentWindow.window.CurrentFilter = "";
    return getParentPage().FilteredPage.Iframe.contentWindow.window.CurrentFilter;
}

function BrowserHeight() {
	var winH = 0;
	if (document.body && document.body.offsetWidth) {
		winH = document.body.offsetHeight;
	}
	if (document.compatMode == 'CSS1Compat' && document.documentElement && document.documentElement.offsetWidth) {
		winH = document.documentElement.offsetHeight;
	}
	if (window.innerWidth && window.innerHeight) {
		winH = window.innerHeight;
	}
	return winH;
}

function resizeIframe() {
	try {
		if (self == parent) {
			if (document.getElementById("__MainDiv").attributes["FitToContent"] && document.getElementById("__MainDiv").attributes["FitToContent"].value == "True") {
				//if (isNN) {
					var OldOverflow = document.getElementById("__MainDiv").style["overflow"];
					document.getElementById("__MainDiv").style["overflow"] = "hidden";
				//}
	            document.getElementById("__MainDiv").style.height = "0px";
	            var FramePageHeight = document.getElementById("__MainDiv").scrollHeight + parseInt(document.getElementById("__MainDiv").attributes["MarginToContent"].value);
				if (BrowserHeight() > FramePageHeight)
					FramePageHeight = BrowserHeight();
				document.getElementById("__MainDiv").style.height = (FramePageHeight) + 'px';
				document.getElementsByTagName('body')[0].style.height = (FramePageHeight) + 'px';
				//if (isNN) document.getElementById("__MainDiv").style["overflow"] = OldOverflow;
				document.getElementById("__MainDiv").style["overflow"] = OldOverflow;
				var p = document.getElementById("__MainDiv").parentElement;
				if (typeof (p) != "undefined" && p != null && p.id == "MainAjaxPanel") {
					p.style.height = (FramePageHeight) + 'px';
				}
			}
			return false;
		}
		else if (document.getElementById) {
			if (self.frameElement && (self.frameElement.radWindow || (self.frameElement.attributes["AutoExpandToContent"] && self.frameElement.attributes["AutoExpandToContent"].value == "True"))) {
				var FramePageHeight = 0;
				if (document.getElementById("__MainDiv").attributes["FitToContent"] && document.getElementById("__MainDiv").attributes["FitToContent"].value == "True") {
				    document.body.style.height = "0px";
					var OldOverflow = document.getElementById("__MainDiv").style["overflow"];
					document.getElementById("__MainDiv").style["overflow"] = "hidden";
		            document.getElementById("__MainDiv").style.height = "0px";
		            FramePageHeight = document.getElementById("__MainDiv").scrollHeight + parseInt(document.getElementById("__MainDiv").attributes["MarginToContent"].value) + 10;
					document.getElementById("__MainDiv").style.height = (FramePageHeight) + 'px'; 
					document.getElementsByTagName('body')[0].style.height = (FramePageHeight) + 'px';
					document.getElementById("__MainDiv").style["overflow"] = OldOverflow;
					var p = document.getElementById("__MainDiv").parentElement;
					if (typeof (p) != "undefined" && p != null && p.id == "MainAjaxPanel") {
						p.style.height = (FramePageHeight) + 'px';
					}
				}
				else {
					var OldOverflow = document.getElementById("__MainDiv").style["overflow"];
					document.getElementById("__MainDiv").style["overflow"] = "hidden";
					if (document.getElementById("__MainDiv").clientHeight && document.getElementById("__MainDiv").clientHeight != "" && document.getElementById("__MainDiv").clientHeight > 0) {
						FramePageHeight = document.getElementById("__MainDiv").clientHeight + 10;
					}
					else {
						FramePageHeight = parseInt(self.frameElement.attributes["DefaultHeight"].value);
					}
					document.getElementById("__MainDiv").style["overflow"] = OldOverflow;
				}
				parent.document.getElementById("ParentDiv_" + self.frameElement.id).style.height = FramePageHeight + 'px';
				if (parent.document.getElementById("StackParentDiv_" + self.frameElement.id)) {
					parent.document.getElementById("StackParentDiv_" + self.frameElement.id).style.height = FramePageHeight + 'px';
				}
				if (parent.resizeIframe && parent != self) {
					parent.resizeIframe();
				}
			}
		}
	}
	catch (ex) { }
}
 
function NavigateBrowser(Url) 
{
	try
	{
		var parentPage = getParentPage();
		if (typeof(parentPage) != "undefined" && parentPage != null && parentPage != self) 
		{
			parentPage.NavigateBrowser(Url);
		}
		else 
		{
			location.href = Url;
		}
	}
	catch(ex)
	{
	
	}
}

function RefreshBrowser()
{
	try
	{
		var BrowserWindow = self;
		var parentPage = BrowserWindow.getParentPage();
		while (typeof (parentPage) != "undefined" && parentPage != null && parentPage != self && BrowserWindow != parentPage)
		{
			BrowserWindow = parentPage;
			parentPage = parentPage.getParentPage();
		}
		BrowserWindow.NavigateBrowser(BrowserWindow.window.location);
	}
	catch (ex)
	{
	}
}

function EnableControl(controlName) 
{
	try
	{
		document.getElementById(controlName).disabled = false;
	}
	catch(ex)
	{
	}
}

function DisableControl(controlName) 
{
	try
	{
		document.getElementById(controlName).disabled = true;
	}
	catch(ex)
	{
	}
}

function HideControl(controlName) 
{
	try
	{
		document.getElementById(controlName).style.display = "none";
	}
	catch(ex)
	{
	}
}

function ShowControl(controlName) 
{
	try
	{
		document.getElementById(controlName).style.display = "block";
	}
	catch(ex)
	{
	}
}

function AlternateControlVisibility(controlName) 
{
	try
	{
		if (document.getElementById(controlName).style.display != "" && document.getElementById(controlName).style.display != "block" && document.getElementById(controlName).style.display != "inline-block")
		{
			document.getElementById(controlName).style.display = "block";
		}
		else 
		{
			document.getElementById(controlName).style.display = "none";
		}
	}
	catch(ex)
	{
	
	}
}

function AlternateControlEnabled(controlName) 
{
	try 
	{
		if (document.getElementById(controlName).disabled != "" && document.getElementById(controlName).disabled)
		{
			document.getElementById(controlName).disabled = false;
		}
		else 
		{
			document.getElementById(controlName).disabled = true;
		}
	}
	catch (ex) 
	{
	}
}

function getParentPage()
{
	if (!isNN || typeof(this.window) != "undefined")
	{
		return this.window.parent;
	}
	else
	{
		return this.contentWindow.parent;
	}
}

function NavigateTargetByReference(Url, Reference, Options) {
    try {
        var obj = FindObjectByReference(Reference);
        var SourceUrl = this.location.href;
        var TargetUrl = null;
        if (SourceUrl.substr(SourceUrl.lenght - 1, 1) == '/') {
            TargetUrl = SourceUrl + Url;
        }
        else {

            var LastFolderIndexParameter = SourceUrl.lastIndexOf("?");
            var LastFolderIndex;
            if (parseInt(LastFolderIndexParameter) > 0) {
                LastFolderIndex = SourceUrl.lastIndexOf("/", LastFolderIndexParameter);
            }
            else {

                LastFolderIndex = SourceUrl.lastIndexOf("/");
            }
            TargetUrl = SourceUrl.substr(0, LastFolderIndex + 1) + Url;
        }
        if (typeof (obj) != "undefined" && obj != null) {
            if (obj.tagName == "IFRAME") {
            	obj.contentWindow.location.href = TargetUrl;
            	WaitForIFrameNavigation(obj);
            }
            else {
                var wnd = obj.Open(TargetUrl);
                wnd.Caller = this;

				var $ = jQuery.noConflict();
                $(wnd._iframe).load(function() { SetWinCaller(wnd._iframe);});

                if (HasValue(Options) && Options.Modal) {
                    wnd.SetModal(true);
                    if (Options.Center) setTimeout(function () { CentralizeWindow(wnd.Id) }, 100);
                }
            }
        }
        else {
            document.location.href = TargetUrl;
        }
    }
    catch (ex) {
    }
}

function WaitForIFrameNavigation(iframe) {
	if (iframe.readyState != "complete") {
		setTimeout(function invoke() { WaitForIFrameNavigation(iframe); }, 200);
	} else {
		iframe.contentWindow.document.Caller = this;
	}
}

function SetWinCaller(iframe) 
{
    try 
    {
        iframe.contentWindow.document.Caller = this;
    } catch (e) 
    {

    }
}

function FindObjectByReference(objectName)
{
	var obj = null;
	var iframes = document.getElementsByTagName("iframe");
	for (var i = 0; i < iframes.length; i++)
	{
		var att = iframes[i].attributes["NavigationReference"];
		if (att != null && att.value == objectName)
		{
			obj = iframes[i];
			break;
		}
	}
	try
	{
		var wndSettings = GetgWinsettings();
		var att = document.getElementById(wndSettings.Id).attributes["NavigationReference"];
		if (att != null && att.value == objectName)
		{
			obj = wndSettings;
		}
	}
	catch(ex)
	{
		try
		{
			var wndSettings = GetRadWindowManager();
			var att = document.getElementById(wndSettings._name).attributes["NavigationReference"];
			if (att != null && att.value == objectName)
			{
				obj = wndSettings;
			}
		}
		catch(ex)
		{
		}
	}
	if (obj != null)
	{
		return obj;
	}
	else
	{
		var ParentPage = getParentPage();
		if (ParentPage != null && typeof (ParentPage) != "undefined" && ParentPage != self)
		{
			return ParentPage.FindObjectByReference(objectName);
		}
	}
	return null;
}

function GetTargetWindow(elem)
{
	return elem.contentWindow;
}

function GetWindow(Obj)
{
	return Obj.window;
}

function NavigatePopup(Url) 
{
	window.open(Url);
}
function Navigate(NavigateUrl, OpenAsWindow, TargetControl, Options)
{
	try
			{
 		if (typeof(TargetControl) == "undefined")
 		{
 			document.location.href = NavigateUrl;
 		}
 		else
 		{
 			document.getElementById(TargetControl).contentWindow.location.href = NavigateUrl;
 		}
	}
	catch (ex)
	{
	}
}

function GetBrowserVersion() {
    var ua = navigator.userAgent, tem,
    M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*([\d\.]+)/i) || [];
    if (/trident/i.test(M[1])) {
        tem = /\brv[ :]+(\d+(\.\d+)?)/g.exec(ua) || [];
        return (tem[1]);
    }
    M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
    if ((tem = ua.match(/version\/([\.\d]+)/i)) != null) M[2] = tem[1];
    return M[1];
};

function GetBrowserType() {
    var ua = navigator.userAgent, tem,
    M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*([\d\.]+)/i) || [];
    if (/trident/i.test(M[1])) {
        tem = /\brv[ :]+(\d+(\.\d+)?)/g.exec(ua) || [];
        return 'IE';
    }
    M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
    if ((tem = ua.match(/version\/([\.\d]+)/i)) != null) M[2] = tem[1];
    return  M[0];
};

function TryParentLogin(PageToRedirect, RefreshControlsID, isWindow)
{
	if (isWindow) 
    {
        RefreshControlsID = 'GASWindowSettings';
    }
    else
    {
		if (this.frameElement != null)
		{
			RefreshControlsID = frameElement.id + (RefreshControlsID != "" && typeof(RefreshControlsID) != "undefined" ? "|" : "") + (typeof(RefreshControlsID) != "undefined" ? RefreshControlsID:"")
		}
	}
	var parentPage = getParentPage();
	if (parentPage != null && parentPage != self)
	{
		try
		{
			parentPage.TryLogin(PageToRedirect, RefreshControlsID);
		}
		catch (ex)
		{

		}
	}
	else if(parentPage != null)
	{
		document.location.href = "StartPage.aspx?RefreshControlID=" + RefreshControlsID + "&url=" + PageToRedirect;
	}
}

function RefreshNavigationControl(RefreshControlsID,Url)
{
	if (RefreshControlsID != "")
	{
		if (RefreshControlsID.indexOf("|") != -1)
		{
			var ControlToRefresh = RefreshControlsID.substr(0, RefreshControlsID.indexOf("|"));
			try
			{
				document.getElementById(ControlToRefresh).contentWindow.RefreshNavigationControl(RefreshControlsID.substr(RefreshControlsID.indexOf("|") + 1), Url);
			}
			catch(ex)
			{
			}
		}
		else
		{
			if (Url != "")
			{
				if (document.getElementById(RefreshControlsID) != null)
				{
					document.getElementById(RefreshControlsID).contentWindow.location.href = Url;
				}
				else if (RefreshControlsID == 'GASWindowSettings') 
			    {
			        Navigate(Url, true);
			    }
				try
				{
					OnLoginSucceded();
				}
				catch (ex)
				{
				}
				return false;
			}
			else
			{
				document.getElementById(RefreshControlsID).contentWindow.location.Reload();
			}
		}
	}
	else
	{
		if(Url != "")
		{
			document.location.href = Url;
		}
	}
}

	function RestaurazIndex(sender,e)
	{
		sender.OriginalZIndex = 0;
	}

	function NewzIndex(sender, e)
	{
		sender.OriginalZIndex = 500000;
	} 

	function EnableDisableMenuItem(Menu, ItemId, Enable)
	{
		var MenuObj = $find(Menu.id);
		var Item;
		var menus = MenuObj._getAllItems();
		for (var i = 0; i < menus.length; i++)
		{
		    if (menus[i].get_value() == ItemId)
			{
			    Item = menus[i];
				break;
			}
		}
		if (Enable)
		{
			Item.enable();
		}
		else
		{
			Item.disable();
		}
	}

    function EnableDisableToolbarButtons(ToolBarID, Argument, isEnabled) 
    {
        var toolBar = $find(ToolBarID);
        if (toolBar) 
	{
        	var button = toolBar.findButtonByCommandName(Argument);
        	if (button) 
	{
        		if (isEnabled) 
		{
        			button.enable();
		}
		else
		{
        			button.disable();
        		}
        	}
		}
	}

function OnRowSelected(sender, eventArgs) {
	ExecuteCommandRequest("RefreshGrid", sender.ClientID, false, "ROWID:" + eventArgs.get_itemIndexHierarchical());
}

function OnRequestStart(sender, arguments)
{
		var vgDoc = null;
	try
	{
		if (arguments.EventTargetElement != null && HasValue(arguments.EventTargetElement.ownerDocument)) vgDoc = arguments.EventTargetElement.ownerDocument;
		else if (HasValue(sender._element.document)) vgDoc = vgDoc = sender._element.document;
		else vgDoc = document;

		var Loadings = vgDoc.getElementsByClassName("GasLoadingAjaxInvisible");
		if (Loadings.length > 0)
		{
		    var LoadingsList = [];
		    for (var i = 0; i < Loadings.length; i++) {
                if (typeof(vgAjax) != "undefined" && sender._element.id == vgAjax) {
		            if (Loadings[i].id == "MainAjaxPanelLoading") {
		                LoadingsList.push(Loadings[i]);
		            }
                }
                else {
                    if (Loadings[i].getAttribute("ControlID") == sender._element.id) {
                        LoadingsList.push(Loadings[i]);
                    }
                }
		    }
		    for (var i = 0; i < LoadingsList.length; i++) {
		        LoadingsList[i].className = "GasLoadingAjaxVisible";
		    }
			vgDoc.body.style.cursor = "wait";
		}

		if (typeof(vgErrorLabel) != "undefined")
		{
			HideControl(vgErrorLabel);
		}
		if (arguments.EventTarget.indexOf("$ExportTo") != -1 || (arguments.EventArgument.indexOf("ExportTo") != -1 && arguments.EventArgument.startsWith("FireCommand"))) {
		    arguments.EnableAjax = false;
		}
	}
	catch (ex)
	{
	}

	try
	{
		window.IsAjaxProcessing = true;
		window.EnableButtons();
	}
	catch (e)
	{
	}
}

function OnResponseEnd(sender, arguments) 
{
	var vgDoc;
	if (typeof (arguments.EventTargetElement) != "undefined" && arguments.EventTargetElement != null)
		vgDoc = arguments.EventTargetElement.ownerDocument;
	else
		vgDoc = document;

    if (isIE)
        var vgWindow = vgDoc.parentWindow.window;
    else
        var vgWindow = vgDoc.defaultView.window;

    vgDoc.body.style.cursor = "default";
      
    var x = arguments["EventArgument"];
      
    if (typeof(x) != "undefined" && x.indexOf("ExecuteCommand:") != -1)
    {
		var CommandName = "";
        var TargetName = "";
		var Parameters = x.split('|');
		
		for (i = 0; i < Parameters.length; i++)
        {
			var Args = Parameters[i].split(':');
            switch (Args[0])
            {
				case "ExecuteCommand":
					CommandName = Args[1];
					break;
				case "TargetControl":
					TargetName = Args[1];
					break;
            }
		}

        try
        {
            if (CommandName == "new" || CommandName == "edit")
            {
				vgWindow.SetFocusFirstField();
            }
            else if (CommandName == "graph")
            {
                vgWindow.parent.OpenGraph('BarVertCluster')
            }
        }
        catch (e)
		{
		}
			
		try 
		{
			vgWindow.OnPageResponseEnd(CommandName);
		}
		catch (e) 
		{
		}
    }

	try 
	{
		vgWindow.IsAjaxProcessing = false;
		vgWindow.EnableButtons();
	}
	catch (e)
	{			
	}
			
	try
	{
		var ParentPage = vgWindow.getParentPage();
		if (ParentPage != "undefined")
		{
			GetWindow(ParentPage).EnableButtons(CommandName);
		}
	}
	catch (e)
	{
	}
}

function Logoff()
{
	__doPostBack('LOGOFF');
	return false;
}
