var Numbers = "-0123456789"; 	 //set de caracteres numÃ©ricos
var Mask = "";
var MaskedControl = null;
var MaxSize = 0;
var OldValue;
var newPos = null;
var NumericMask = false;
var ApplyMaskOnlyOnBlur = false;
var MaskSetted = false;
var isGrid = false;

function setCursorPosition(iCaretPos) {
	if (document.selection) {

		// Set focus on the element
		MaskedControl.focus();

		// Create empty selection range
		var oSel = document.selection.createRange();
		oSel.collapse(true);

		oSel.moveEnd('character', -MaskedControl.value.length);
		// Move selection start and end to desired position
		oSel.moveEnd('character', iCaretPos);
		oSel.moveStart('character', iCaretPos);
		oSel.select();
	}

	// Firefox support
	else if (MaskedControl.selectionStart || MaskedControl.selectionStart == '0') {
		MaskedControl.selectionStart = iCaretPos;
		MaskedControl.selectionEnd = iCaretPos;
		//MaskedControl.focus();
	}

}

function OnMaskBlur() {
	var $j = jQuery.noConflict();
	$j(MaskedControl).off('propertychange', OnPropertyChange);
	$j(MaskedControl).off('keypress', OnKeyPress);
	$j(MaskedControl).off('beforepaste', OnBeforePaste);
	$j(MaskedControl).off('paste', OnPaste);
	$j(MaskedControl).off('keydown', OnKeyDown);
	if (NumericMask) {
		FixNumericDecimals();
	}
	else {
		try {
			var i = 0;
			var compMask = "";
			while (i < Mask.length) {
				compMask += "9";
				i++;
			}
			if (Mask == compMask) {
				var MaskLength = Mask.length;
				var currentVal = MaskedControl.value.length;
				var RetVal = MaskedControl.value;
				while (currentVal < MaskLength) {
					RetVal = "0" + RetVal;
					currentVal++;
				}
				MaskedControl.value = RetVal;
			}
		}
		catch (Ex) { }
	}
	if (ApplyMaskOnlyOnBlur) {
		ApplyMask();
	}
}

function SetMask($sender, setEvents) {
	if (!$sender.jquery) {
		var $j = jQuery.noConflict();
		$sender = $j($sender);
	}
	MaskedControl = $sender[0];
	if (setEvents) {
		var $j = jQuery.noConflict();
		$sender.off('change', OnPropertyChange);
		$sender.off('keypress', OnKeyPress);
		$sender.off('beforepaste', OnBeforePaste);
		$sender.off('paste', OnPaste);
		$sender.off('keydown', OnKeyDown);
		if (!ApplyMaskOnlyOnBlur) {
			$sender.on('change', OnPropertyChange);
			$sender.on('keypress', OnKeyPress);
			$sender.on('beforepaste', OnBeforePaste);
			$sender.on('paste', OnPaste);
		}
		$sender.on('keydown', OnKeyDown);
	}
	Mask = $sender.attr("Mask");
	NumericMask = $sender.attr("IsNumeric") == "True";

	while (Mask.toLowerCase().indexOf("d") != -1 || Mask.toLowerCase().indexOf("m") != -1 || Mask.toLowerCase().indexOf("y") != -1 || Mask.toLowerCase().indexOf("h") != -1 || Mask.toLowerCase().indexOf("s") != -1) {
		Mask = Mask.toLowerCase().replace("d", "9").replace("m", "9").replace("y", "9").replace("s", "9").replace("h", "9");
	}
	try {
		MaxSize = $sender.attr("MaxSize");
	}
	catch (ex) {
		MaxSize = 0;
	}
	OldValue = $sender.val();
}

function getCursorPosition() {
	var txt = MaskedControl.value;
	var len = txt.length;
	var pos = -1;
	if (typeof document.selection != "undefined") // FOR MSIE
	{
		range_sel = document.selection.createRange();
		range_obj = MaskedControl.createTextRange();
		range_obj.moveToBookmark(range_sel.getBookmark());
		range_obj.moveEnd('character', MaskedControl.value.length);
		pos = len - range_obj.text.length;
	}
	else if (typeof MaskedControl.selectionStart != "undefined") // FOR MOZILLA
	{
		pos = MaskedControl.selectionStart;
	}
	return pos;
}
function OnPaste() {
	if (isChrome) {
		OnBeforePaste(event);
	}

	noBubble(event);
	return false;
}

function OnBeforePaste(e) {
	Pending = true;
	var CopiedData = window.clipboardData.getData("Text");
	var Selection = GetSelection();
	var CursorPos = getCursorPosition();
	if (MaskedControl.value.length + CopiedData.length - Selection.length > MaxSize && MaxSize > 0) {
		CopiedData = CopiedData.substr(0, MaxSize - (MaskedControl.value.length - Selection.length));
	}
	var FinalText = MaskedControl.value;
	if (CopiedData.length > 0) {
		FinalText = MaskedControl.value.substr(0, CursorPos) + CopiedData + MaskedControl.value.substr(CursorPos + Selection.length);
	}
	var newCursorPos = (MaskedControl.value.substr(0, CursorPos) + CopiedData).length;
	if (FinalText != MaskedControl.value) {
		MaskedControl.value = FinalText;
	}
	Pending = false;
	setCursorPosition(newCursorPos);
	noBubble(e);
	event.returnValue = false;
	return false;
}
function GetSelection() {
	var selectedText;
	// IE version
	if (typeof (document.selection) != "undefined") {
		var sel = document.selection.createRange();
		selectedText = sel.text;
	}
	// Mozilla version
	else if (MaskedControl.selectionStart != undefined) {
		var startPos = MaskedControl.selectionStart;
		var endPos = MaskedControl.selectionEnd;
		selectedText = MaskedControl.value.substring(startPos, endPos)
	}
	return selectedText;
}
var Pending = false;
function OnKeyDown(e) {
	if (MaskedControl.readOnly == true) return;
	if (isNN && (e.keyCode == "37" || e.keyCode == "39")) {
		return;
	}
	if (!ApplyMaskOnlyOnBlur && (e.keyCode == "46" || e.keyCode == "8")) {
		Pending = false;
		var selection = GetSelection();
		newPos = getCursorPosition();

		//DELETE
		if (e.keyCode == "46") {
			if (NumericMask) {
				if (Mask.indexOf(".") != -1) {
					if (MaskedControl.value.substr(newPos, 1) == "." || MaskedControl.value.substr(newPos, 1) == ",") {
						setCursorPosition(newPos + 1);
						newPos++;
					}
				}
			}
			if (selection.length == 0) {
				MaskedControl.value = MaskedControl.value.substr(0, newPos) + MaskedControl.value.substr(newPos + 1 + selection.length);
			}
			else {
				MaskedControl.value = MaskedControl.value.substr(0, newPos) + MaskedControl.value.substr(newPos + selection.length);
			}

		}

		//BACKSPACE
		if (e.keyCode == "8") {
			if (NumericMask) {
				if (Mask.indexOf(".") != -1) {
					if (MaskedControl.value.substr(newPos - 1, 1) == ".") {
						setCursorPosition(newPos - 1);
						newPos--;
					}
				}
			}
			if (selection.length == 0) {
				MaskedControl.value = MaskedControl.value.substr(0, newPos - 1) + MaskedControl.value.substr(newPos + selection.length);
				newPos--;
			}
			else {
				MaskedControl.value = MaskedControl.value.substr(0, newPos) + MaskedControl.value.substr(newPos + selection.length);
			}
		}
		ApplyMask();
		setCursorPosition(newPos);
		noBubble(e);
		return false;
	}

	try {
		if (!MaskedControl.attributes["isGrid"]) onTextChanged(e);
	}
	catch (ex) { }
}

function noBubble(e) {
	e = e || event;
	if (e) {
		e.returnValue = false;
		e.cancelBubble = true;
		try
		{
			e.stopPropagation();
			e.preventDefault();
		} catch (e) { }
	}
}

var NetscapeRaising = false;
var CursorToEnd = 0;
function OnKeyPress(e) {
	if (MaskedControl.readOnly == true) return;
	if (!ApplyMaskOnlyOnBlur) {
		try {
			var keyCode = (isIE ? e.keyCode : e.charCode);
			if (keyCode != 0) {
				MaskSetted = false;
				Pending = true;
				newPos = null;
				var Str = Chr(keyCode); //gets the value to be inserted

				if (Str != "") {
					var CursorPosition = getCursorPosition();
					if (CursorPosition != MaskedControl.value.length) {
						newPos = CursorPosition + 1;
					}
					var SelectionCount = GetSelection().length; //checks selection size
					if ((MaskedControl.value + Str).length - SelectionCount > MaxSize && MaxSize > 0) //if value's length will be bigger than mask's length
					{
						// cancel the key press
						noBubble(e);
						return false;
					}
					if (!NetscapeRaising) {
						if (NumericMask) {
							if (MaskedControl.value.indexOf(",") != -1 && Mask.indexOf(",") != -1) {
								//Is typing decimals, and there are no empty spots for decimals or
								//Is typing integers and there are no empty spots for integers
								if (MaskedControl.value.indexOf(",") != -1 && ((MaskedControl.value.substr(MaskedControl.value.indexOf(",")).length - SelectionCount == Mask.substr(Mask.indexOf(",")).length && CursorPosition > MaskedControl.value.indexOf(",")) ||
						(CursorPosition <= MaskedControl.value.indexOf(",") && MaskedControl.value.substr(0, MaskedControl.value.indexOf(",")).length - SelectionCount == Mask.substr(0, Mask.indexOf(",")).length))) {
									noBubble(e);
									return false;
								}
							}
						}
						if (Mask.substr(0, 1) == "@")//if mask starts with @
						{
							MaskSetted = true;
							var valASC = Asc(Str);
							if (Mask.indexOf("!") != -1) {
								var lowerStr = Str;
								Str = Str.toUpperCase();
								if (Str != lowerStr) {
									if (isIE || isChrome || isSafari) {
										e.keyCode = Asc(Str);
									}
									else {
										//		NETSCAPE EVENT PROPERY e.charCode IS READONLY								
										//		RERAISING KEYPRESS EVENT WITH CUSTOM EVENT PROPERTIES
										NetscapeRaising = true;
										var newEvent = document.createEvent("KeyEvents")
										newEvent.initKeyEvent("keypress", true, true, document.defaultView,
										e.ctrlKey, e.altKey, e.shiftKey,
										e.metaKey, 0, Asc(Str));
										e.preventDefault();
										e.target.dispatchEvent(newEvent);
										noBubble(e);
										e.charCode = Asc(Str);
									}
								}
								else {
									return true;
								}
							}
							if (Mask.indexOf("a") != -1) {
								var lowerStr = Str;
								Str = Str.toLowerCase();
								if (Str != lowerStr) {
									if (isIE || isChrome || isSafari) {
										e.keyCode = Asc(Str);
									}
									else {
										NetscapeRaising = true;
										var newEvent = document.createEvent("KeyEvents")
										newEvent.initKeyEvent("keypress", true, true, document.defaultView, e.ctrlKey, e.altKey, e.shiftKey, e.metaKey, 0, Asc(Str));
										e.preventDefault();
										e.target.dispatchEvent(newEvent);
										noBubble(e);
										return false;
									}
								}
								else {
									return true;
								}
							}
							if (Mask.indexOf("A") != -1) {
								if (!((valASC >= 65 && valASC <= 90) || (valASC >= 97 && valASC <= 122) || valASC == 32)) {
									if (isIE || isChrome || isSafari) {
										e.keyCode = 0;
									}
									noBubble(e);
									return false;
								}
							}
							if (Mask.indexOf("N") != -1) {
								if (!((valASC >= 65 && valASC <= 90) || (valASC >= 97 && valASC <= 122) || (Numbers.indexOf(Str) != -1))) {
									if (isIE || isChrome || isSafari) {
										e.keyCode = 0;
									}
									noBubble(e);
									return false;
								}
							}
							if (Mask.indexOf("9") != -1) {
								if (Numbers.indexOf(Str) == -1) {
									if (isIE || isChrome || isSafari) {
										e.keyCode = 0;
									}
									noBubble(e);
									return false;
								}
							}
							if (Mask.indexOf("#") != -1) {
								if (!(Numbers.indexOf(Str) != -1 || Str == " ")) {
									if (isIE || isChrome || isSafari) {
										e.keyCode = 0;
									}
									noBubble(e);
									return false;
								}
							}
						}
						else//if mask doesn't start with @
						{
							//while carret isn't at a valid typing position
							var Continue = true;
							if (NumericMask) {
								if (Mask.indexOf(",") != -1) {
									if (Str == "," && MaskedControl.value.indexOf(",") == -1) {
										Continue = false;
									}
								}
							}
							if (Continue) {
								var SkipAdding = false;
								while (Mask.substr(CursorPosition, 1) != "undefined" && Mask.substr(CursorPosition, 1) != "9" && Mask.substr(CursorPosition, 1) != "A" && Mask.substr(CursorPosition, 1) != Str && Mask.substr(CursorPosition, 1) != "!" &&
										Mask.substr(CursorPosition, 1) != "N" && Mask.substr(CursorPosition, 1) != "X" && Mask.substr(CursorPosition, 1) != "#" && Mask.substr(CursorPosition, 1) != "@") {
									//this will make sure that none character will be inserted wherever it cannot be
									CursorPosition++;
									SkipAdding = true;
									if (CursorPosition >= MaskedControl.value.length)
										break;
								}
								if (!SkipAdding) {
									if (CursorPosition != MaskedControl.value.length) {
										newPos = CursorPosition + 1;
									}
								}
								if (Mask.substr(CursorPosition, 1) == "!") {
									//accepts any capital character
									if (isIE || isChrome || isSafari) {
										e.keyCode = Asc(Str.toUpperCase());
									}
									else {
										var newEvent = document.createEvent("KeyEvents")
										newEvent.initKeyEvent("keypress", true, true, document.defaultView,
								e.ctrlKey, e.altKey, e.shiftKey,
								e.metaKey, 0, Str.toUpperCase());
										e.preventDefault();
										e.target.dispatchEvent(newEvent);
									}
								}
								else {
									if (Mask.substr(CursorPosition, 1) == "N") {
										//accepts numbers and letters, but none special characters
										var StrAsc = Asc(Str);
										var Ret = !(Numbers.indexOf(Str) != -1 || ((StrAsc >= 97 && StrAsc <= 122) || (StrAsc >= 65 && StrAsc <= 90)));
										if (Ret) {
											noBubble(e);
											return false;
										}
									}
									else {
										if (Mask.substr(CursorPosition, 1) == "#") {
											//accepts numbers and space, none characters
											if (!(Numbers.indexOf(Str) != -1 || Str == " ")) {
												noBubble(e);
												return false;
											}
										}
										if (Mask.substr(CursorPosition, 1) != "X")// X - allows everything
										{
											//A - accepts any character
											//9 - accepts only number
											if ((Mask.substr(CursorPosition, 1) == "9" && Numbers.indexOf(Str) == -1) || (Mask.substr(CursorPosition, 1) == "A" && Numbers.indexOf(Str) != -1) || MaskedControl.value.length - SelectionCount >= Mask.length) {
												noBubble(e);
												return false;
											}
										}
									}
								}
							}
						}
					}
					else {
						NetscapeRaising = false;
						return true;
					}
				}
			}
		}
		catch (ex) {
		}
		if (keyCode != 0) {
			noBubble(e);
			var LastPosition = (getCursorPosition() == MaskedControl.value.length)
			MaskedControl.value = MaskedControl.value.substr(0, CursorPosition) + Str + MaskedControl.value.substr(CursorPosition + SelectionCount);
			ApplyMask();
			CursorPosition = LastPosition ? MaskedControl.value.length : CursorPosition + 1;
			setCursorPosition(CursorPosition);
			return false;
		}
	}
}

function ApplyElementMask(TextBox) {
	if (TextBox != null && TextBox != "undefined") {
		var PreviousElement = MaskedControl;
		SetMask(TextBox, false);
		ApplyMask();
		if (NumericMask) {
			FixNumericDecimals();
		}
		if (PreviousElement != null) {
			SetMask(PreviousElement, false);
		}
	}
}

function FixNumericDecimals() {
	try {
		if (MaskedControl.value != "") {
			if (Mask.indexOf(",") != -1) {
				if (MaskedControl.value.indexOf(",") == 0) {
					MaskedControl.value = "0" + MaskedControl.value;
				}
				if (MaskedControl.value.length > 0 && MaskedControl.value.indexOf(",") == -1) {
					MaskedControl.value += ",";
				}
				while (MaskedControl.value.substr(MaskedControl.value.indexOf(",")).length <= Mask.substr(Mask.indexOf(",")).length && MaskedControl.value.substr(MaskedControl.value.indexOf(",")).length != Mask.substr(Mask.indexOf(",")).length) {
					MaskedControl.value += "0";
				}
			}
		}
	}
	catch (ex) {
	}
}

function ApplyMask() {
	var NewValue = "";
	var AddingValue = "";
	var ValCount = 0;
	if (NumericMask) {
		var currentValue = MaskedControl.value;
		var RunningValue = "";
		var DecimalsCount = Mask.substr(Mask.indexOf(",")).length;
		if (Mask.indexOf(",") == -1) {
			DecimalsCount = 0;
		}

		for (var ii = 0; ii < currentValue.length; ii++) {
			if (ii == Mask.length - DecimalsCount && RunningValue.indexOf(",") == -1 && currentValue.indexOf(",") == -1) {
				RunningValue += ",";
			}
			if (Numbers.indexOf(currentValue.substr(ii, 1)) != -1 || currentValue.substr(ii, 1) == "." || currentValue.substr(ii, 1) == ",") {
				RunningValue += currentValue.substr(ii, 1);
			}
		}

		if (Mask.indexOf(".") != -1) {
			var AllDots = 0;
			while (RunningValue.indexOf(".") != -1) {
				RunningValue = RunningValue.replace(".", "");
				AllDots++;
			}
			var NoneDecimal = RunningValue;
			if (RunningValue.indexOf(",") != -1) {
				NoneDecimal = RunningValue.substr(0, RunningValue.indexOf(","));
			}
			var AddedDots = 0;
            var isNegative = NoneDecimal[0] == "-";
			while (NoneDecimal.indexOf("-") != -1) {
			    NoneDecimal = NoneDecimal.replace("-", "");
			}
            if (NoneDecimal.length >= 4) {
			    var DotCount = NoneDecimal.length - 3;
			    while (DotCount > 0) {
			            NoneDecimal = NoneDecimal.substr(0, DotCount) + "." + NoneDecimal.substr(DotCount);
			            DotCount -= 3;
			            AddedDots++;
				}
			}
			if (isNegative) NoneDecimal = "-" + NoneDecimal;
			MaskedControl.value = NoneDecimal + (RunningValue.indexOf(",") != -1 ? RunningValue.substr(RunningValue.indexOf(",")) : "");
			if (newPos != null) {
				newPos += AddedDots - AllDots;
			}
		}
		else {
			MaskedControl.value = RunningValue;
		}
	}
	else {
		if (!MaskSetted) {
			if (Mask.substr(0, 1) == "@") {
				var AllowSpecialCharacters = false;
				var CapitalCharacters = false;
				var AllowCharacters = false;
				var AllowNumbers = false;
				var AllowSpaces = false;
				if (Mask.indexOf("a") != -1) {
					AllowSpaces = true;
					AllowNumbers = true;
					CapitalCharacters = false;
					AllowCharacters = true;
					AllowSpecialCharacters = true;
				}
				if (Mask.indexOf("!") != -1) {
					AllowSpaces = true;
					AllowNumbers = true;
					CapitalCharacters = true;
					AllowCharacters = true;
					AllowSpecialCharacters = true;
				}
				if (Mask.indexOf("A") != -1) {
					AllowSpaces = true;
					AllowNumbers = false;
					AllowCharacters = true;
					AllowSpecialCharacters = false;
				}
				if (Mask.indexOf("N") != -1) {
					AllowNumbers = true;
					AllowCharacters = true;
					AllowSpecialCharacters = false;
					AllowSpaces = false;
				}
				if (Mask.indexOf("9") != -1) {
					AllowNumbers = true;
					AllowSpaces = false;
					AllowCharacters = false;
					CapitalCharacters = false;
					AllowSpecialCharacters = false;
				}
				if (Mask.indexOf("#") != -1) {
					AllowNumbers = true;
					AllowSpaces = true;
					AllowCharacters = false;
					CapitalCharacters = false;
					AllowSpecialCharacters = false;
				}
				if (Mask.indexOf("X") != -1) {
					AllowSpecialCharacters = true;
					CapitalCharacters = false;
					AllowCharacters = true;
					AllowNumbers = true;
					AllowSpaces = true;
				}
				for (var i = 0; i < MaskedControl.value.length; i++) {
					if (i > MaxSize && MaxSize > 0) {
						return;
					}
					if (Numbers.indexOf(MaskedControl.value.substr(i, 1)) != -1 && AllowNumbers) {
						NewValue += MaskedControl.value.substr(i, 1);
					}
					if (Numbers.indexOf(MaskedControl.value.substr(i, 1)) == -1) {
						var valASC = Asc(MaskedControl.value.substr(i, 1));
						var isSpecial = false;
						//	not (A - Z || a - z)
						if (!((valASC >= 65 && valASC <= 90) || (valASC >= 97 && valASC <= 122))) {
							isSpecial = true;
						}
						if (isSpecial && AllowSpecialCharacters) {
							NewValue += (CapitalCharacters ? MaskedControl.value.substr(i, 1).toUpperCase() : MaskedControl.value.substr(i, 1));
						}
						else {
							if (AllowCharacters) {
								NewValue += (CapitalCharacters ? MaskedControl.value.substr(i, 1).toUpperCase() : MaskedControl.value.substr(i, 1));
							}
						}
					}
				}
			}
			else {
				if (Mask != "") {
					for (var i = 0; i < Mask.length; i++) {
						if (ValCount >= MaskedControl.value.length) {
							break;
						}

						if (Mask.substr(i, 1) == "9") {
							if (Numbers.indexOf(MaskedControl.value.substr(ValCount, 1)) != -1 && (!NumericMask && MaskedControl.value.substr(ValCount, 1) != "-")) {
								NewValue += MaskedControl.value.substr(ValCount, 1);
							}
							else {
								i--;
							}
							ValCount++;
						}
						else {
							if (Mask.substr(i, 1) == "A") {
								if (Numbers.indexOf(MaskedControl.value.substr(ValCount, 1)) == -1) {
									NewValue += MaskedControl.value.substr(ValCount, 1);
								}
								else {
									i--;
								}
								ValCount++;
							}
							else {
								if (Mask.substr(i, 1) == "N") {
									var StrAsc = Asc(MaskedControl.value.substr(ValCount, 1));
									if (Numbers.indexOf(MaskedControl.value.substr(ValCount, 1)) != -1 || ((StrAsc >= 97 && StrAsc <= 122) || (StrAsc >= 65 && StrAsc <= 90))) {
										NewValue += MaskedControl.value.substr(ValCount, 1);
									}
									else {
										i--;
									}
									ValCount++;
								}
								else {
									if (Mask.substr(i, 1) == "!") {
										NewValue += MaskedControl.value.substr(ValCount, 1).toUpperCase();
										ValCount++;
									}
									else {
										if (Mask.substr(i, 1) == "#") {
											if (Numbers.indexOf(MaskedControl.value.substr(ValCount, 1)) != -1 || MaskedControl.value.substr(ValCount, 1) == " ") {
												NewValue += MaskedControl.value.substr(ValCount, 1);
												ValCount++;
											}
										}
										else {
											if (Mask.substr(i, 1) == MaskedControl.value.substr(ValCount, 1)) {
												NewValue += MaskedControl.value.substr(ValCount, 1);
												ValCount++;
											}
											else {
												if (!NumericMask || Mask.substr(i, 1) != "." || Mask.substr(i, 1) != "-") {
													NewValue += Mask.substr(i, 1);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			MaskedControl.value = NewValue;
		}
	}

}
function OnPropertyChange(e) {
	if (!ApplyMaskOnlyOnBlur) {
		if (e.propertyName == "value") {
			if (Pending) {
				Pending = false;
				if (!MaskSetted) {
					ApplyMask();
				}
				MaskSetted = false;
				if (newPos != null) {
					setCursorPosition(newPos);
					newPos = null;
				}
			}
		}
	}
}
