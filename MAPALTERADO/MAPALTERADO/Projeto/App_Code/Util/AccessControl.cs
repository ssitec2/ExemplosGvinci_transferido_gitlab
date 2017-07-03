using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Xml;
using COMPONENTS;

namespace PROJETO.AccessControl
{
	/// <summary>
	/// Uma unica pagina do site
	/// </summary>
	public class SitePage
	{
		string _pagename;
		string _pagedescription;
		/// <summary>
		/// Nome da pagina
		/// </summary>
		public string PageName
		{
			get { return _pagename; }
			set { _pagename = value; }
		}
		/// <summary>
		/// Descrição da pagina
		/// </summary>
		public string PageDescription
		{
			get { return _pagedescription; }
			set { _pagedescription = value; }
		}
	}
	/// <summary>
	/// coleçao de Paginas do site
	/// </summary>
	public class SitePageCollection : CollectionBase
	{
		/// <summary>
		/// Adiciona paginas a colection
		/// </summary>
		/// <param name="NewItem">Nova pagina</param>
		public virtual void Add(SitePage NewItem)
		{
			this.List.Add(NewItem);
		}
		/// <summary>
		/// Coleção de pages
		/// </summary>
		/// <param name="Index">Numero da pagina</param>
		/// <returns>a pagina que foi escolida pelo index</returns>
		public virtual SitePage this[int Index]
		{
			get
			{
				if (Index > 0)
				{
					return (SitePage)this.List[Index];
				}
				else
				{
					return null;
				}
			}
			set
			{
				this.List[Index] = value;
			}
		}

	}
	/// <summary>
	/// um item do menu principal
	/// </summary>
	public class SiteMenu
	{
		string _menuname;
		string _menutext;

		public SiteMenuCollection MenuItems = new SiteMenuCollection();
		/// <summary>
		/// Nome do item do menu
		/// </summary>
		public string MenuName
		{
			get { return _menuname; }
			set { _menuname = value; }
		}
		/// <summary>
		/// Texto do item do menu
		/// </summary>
		public string MenuText
		{
			get { return _menutext; }
			set { _menutext = value; }
		}
	}
	/// <summary>
	/// Coleçao de itens do menu
	/// </summary>
	public class SiteMenuCollection : CollectionBase
	{
		/// <summary>
		/// Adiciona um item do menu a collection
		/// </summary>
		/// <param name="NewMenu">Novo menu item</param>
		public virtual void Add(SiteMenu NewMenu)
		{
			this.List.Add(NewMenu);
		}
		/// <summary>
		/// Pega o menu de acordo com o index
		/// </summary>
		/// <param name="Index">Index do menu</param>
		/// <returns>O menu escolhido pelo index</returns>
		public virtual SiteMenu this[int Index]
		{
			get
			{
				return (SiteMenu)this.List[Index];
			}
			set
			{
				this.List[Index] = value;
			}
		}
	}
	/// <summary>
	/// Pagina onde esta o meny
	/// </summary>
	public class Site
	{
		public SitePageCollection Pages;
		public SiteMenu Menu = new SiteMenu();
		/// <summary>
		/// Construtor
		/// </summary>
		public Site()
		{
		}
		/// <summary>
		/// abre as paginas que estao no xml
		/// </summary>
		/// <param name="FilePath">nome do xml de paginas</param>
		public void LoadPages(string FilePath)
		{
			Pages = new SitePageCollection();

			XmlDocument PagesDoc = new XmlDocument();

			//PagesDoc.Load(FilePath);
			PagesDoc.Load(FilePath);
			XmlNode PagesNode = PagesDoc.FirstChild.NextSibling;

			for (int i = 0; i < PagesNode.ChildNodes.Count; i++)
			{
				SitePage Pg = new SitePage();
				Pg.PageName = PagesNode.ChildNodes[i].Attributes.GetNamedItem("nome").Value;
				Pages.Add(Pg);
			}

		}
	}

}
