using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using PROJETO;
using COMPONENTS;
using COMPONENTS.Data;
using COMPONENTS.Configuration;
using COMPONENTS.Security;

namespace PROJETO
{
	/// <summary>
	/// Code behind da pagina de Visualização de consulta
	/// </summary>
	public partial class VisualizerView : System.Web.UI.Page
	{
		
		protected override void InitializeCulture()
		{
			Utility.SetThreadCulture();
		}

		/// <summary>
		/// Page load, apenas faz a autenticaçao para saber se esta logado
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			Utility.CheckAuthentication(this,true);

			if (!Page.IsPostBack)
			{
			    XmlDocument doc = new XmlDocument();
				if(!File.Exists(Server.MapPath("../Xmls/ViewsList.xml")))
				{
					XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
					doc.InsertBefore(xmlDeclaration, doc.DocumentElement);
					XmlNode ViewsNode = doc.CreateNode(XmlNodeType.Element, "Views", "");
					doc.AppendChild(ViewsNode);
				}
				else
				{
					doc.Load(Server.MapPath("../Xmls/ViewsList.xml"));
				}
				foreach (XmlNode Node in doc.FirstChild.NextSibling.ChildNodes)
				{
					lstConsults.Items.Add(Node.Attributes["Name"].Value);
				}
			}

		}
		public void RemoveView()
		{
			if(lstConsults.SelectedItem != null)
			{
				DeleteQuery(lstConsults.SelectedItem.Text);
			}
		}
		public static void Serialize(object source, string file, Type type)
		{
			using (XmlTextWriter writer = new XmlTextWriter(file, System.Text.Encoding.UTF8))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				MemoryStream Ms = new MemoryStream();
	
				xmlSerializer.Serialize(writer, source);
				xmlSerializer.Serialize(Ms, source);

				writer.Flush();
			}
		}
		public static object Deserialize(object source, string file)
		{
			if (!File.Exists(file))
			{
				return null;
			}
	
			using (StreamReader reader = File.OpenText(file))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
				return xmlSerializer.Deserialize(reader);
			}
		}

		override protected void OnInit(EventArgs e)
		{
			if (!IsPostBack)
			{
			}
			base.OnInit(e);
			
		}

		public void DeleteQuery(string Name)
		{
			XmlDocument vgXml = new XmlDocument();
			vgXml.Load(Server.MapPath("../Xmls/ViewsList.xml"));
			File.Delete(Server.MapPath("../Views/" + Name));
			foreach (XmlNode vgNode in vgXml.FirstChild.NextSibling.ChildNodes)
			{
				if (vgNode.Attributes["Name"].Value == Name)
				{
					vgXml.FirstChild.NextSibling.RemoveChild(vgNode);
					lstConsults.Items.Remove(Name);
					break;
				}
			}
			vgXml.Save(Server.MapPath("../Xmls/ViewsList.xml"));
		}
		protected void ___butDel_OnClick(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				RemoveView();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

	}
	
	
}
