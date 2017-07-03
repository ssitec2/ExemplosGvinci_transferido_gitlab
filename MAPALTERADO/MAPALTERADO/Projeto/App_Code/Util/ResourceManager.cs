using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Globalization;
using System.Resources;

namespace PROJETO
{

	/// <summary>
	/// Resource manager da pagina
	/// </summary>
	public class gResourceManager : ResourceManager
	{

		/// <summary>
		/// Construtor
		/// </summary>
		/// <param name="baseName">Nome da pagina</param>
		/// <param name="assembly">Local do webconfig com as informações</param>
		public gResourceManager(string baseName, System.Reflection.Assembly assembly) : base(baseName, assembly)
		{ }

		/// <summary>
		/// Pega a string de acordo com o nome
		/// </summary>
		/// <param name="name">Nome</param>
		/// <returns>String</returns>
		public override string GetString(string name)
		{
			string val = null;
			val = base.GetString(name);
			if (val == null) val = name;
			return val;
		}

		/// <summary>
		/// Pega a string de acordo com o nome e cultura
		/// </summary>
		/// <param name="name">Nome</param>
		/// <param name="culture">Informações de cultura</param>
		/// <returns>String</returns>
		public override string GetString(string name, CultureInfo culture)
		{
			string val = null;
			val = base.GetString(name, culture);
			if (val == null) val = name;
			return val;
		}

	}

}
