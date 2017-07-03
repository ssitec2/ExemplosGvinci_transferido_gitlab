var ComboRequestStack = new Array();

function PrepareAndRequestItems(NewComboRequests) //NewComboRequests deve ser um array ou string com o nome de comboboxes
{
	if (typeof (jQuery) != "undefined" && jQuery)
	{
		if (typeof (NewComboRequests) != "undefined" && NewComboRequests && typeof (NewComboRequests.concat) != "undefined" && NewComboRequests.concat)
		{
			ComboRequestStack = ComboRequestStack.concat(NewComboRequests);
		}
		if (ComboRequestStack.length > 0)
		{
			for (var i = 0; i < ComboRequestStack.length; i++)
			{
				jQuery("#" + ComboRequestStack[i]).attr("IsRefresh", "True");
				jQuery("#" + ComboRequestStack[i])[0].control.requestItems("", false);
			}
		}		
	}
}

function CheckComboItems(sender, args)
{
	var IsRefresh = jQuery(sender._element).attr("IsRefresh");
	var Item = sender.findItemByValue(sender.get_value());
	if (IsRefresh == "True" && (typeof(Item) == "undefined" || !Item))
	{
		sender.set_text("");
		sender.set_value("");
		jQuery(sender._element).attr("IsRefresh", "False");
	}
	if (ComboRequestStack.length > 0 && ComboRequestStack[0] == sender._element.id) 
	{
		ComboRequestStack.shift();
		if (ComboRequestStack.length > 0) PrepareAndRequestItems();
	}
}

function ValidateCombo(sender, args)
{
	jQuery("form").validationEngine("validateField", "#" + sender._element.id);
}

function Combo_OnClientItemsRequesting(sender, eventArgs)
{
	if (jQuery) 
	{
		var context = eventArgs.get_context();
		if (typeof(GetAditionalFields) != "undefined" && GetAditionalFields) context["ClientFields"] = GetAditionalFields(sender._element.id);
		context["AllowFilter"] = jQuery(sender._element).attr("AllowFilter");
		context["ItemsCount"] = sender.get_items().get_count();
		context["IsRefresh"] = jQuery(sender._element).attr("IsRefresh");
	}
}

function Combo_HandleKeyPress(sender, eventArgs)
{
	if (jQuery) 
	{
		jQuery(sender._element).attr("AllowFilter", "True");
	}
}
