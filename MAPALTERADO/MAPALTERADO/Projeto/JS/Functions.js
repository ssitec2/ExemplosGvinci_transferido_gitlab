function replaceAll(vgString, vgOldString, vgNewString)
{
	while (vgString.indexOf(vgOldString) != -1)
	{
 		vgString = vgString.replace(vgOldString, vgNewString);
	}
	return vgString;
}

function Tally(vgString,vgQ)
{
    var vgX = 0;
    var RetVal = 0;
    while(vgX != vgString.length)
    {
        if(vgString.substr(vgX, 1) == vgQ)
        {
            RetVal = RetVal + 1;
        }
        vgX = vgX + 1;
    }
    return RetVal;
}

function InsertAt(vgString,vgQ,vgPos)
{
    vgString = vgString.substr(0,vgPos) + vgQ + vgString.substr(vgPos);
    return vgString;
}

function LTrim(string)
{                                                                    
  while(string.substr(0,1) == " ")
  {
    string = string.substr(1);
  }
  return string;
}

function trim(str, chars)
{
    return ltrim(rtrim(str, chars), chars);
}

function ltrim(str, chars)
{
    chars = chars || "\\s";
    return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
}

function rtrim(str, chars)
{
    chars = chars || "\\s";
    return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
}

function Left(string, QtdadeEsquerda)
{
  if(QtdadeEsquerda > 0)
  {
    return string.substr(0, QtdadeEsquerda);
  }
  else
  {
    return "";  
  }
}

function Mid()
{                                                             
  if(arguments.length == 2)
  {
    return arguments[0].substr(arguments[1]);
  }
  else if(arguments.length == 3)
  {
    return arguments[0].substr(arguments[1], arguments[2]);
  }
  return "";
}

function Asc(String)
{
 return String.charCodeAt(0);
}

function Chr(n)
{
  return String.fromCharCode(n)
}

