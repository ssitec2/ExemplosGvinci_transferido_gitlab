using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using PROJETO;
using COMPONENTS;
using Telerik.Web.UI;

namespace PROJETO
{
	public partial class InfDB : System.Web.UI.Page
	{
		bool vgChecado = false; //testa se é ou nao é pra aparecer as integridades

		public DatabaseConfig DBConfig
		{
			get
			{
				if (ViewState["DbConfig"] == null)
				{
					DatabaseConfig dc = new DatabaseConfig(Server.MapPath("../Databases/"));
					dc.LoadXmlFile();
					ViewState["DbConfig"] = dc;
				}
				return (DatabaseConfig)ViewState["DbConfig"];
			}
		}
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			
		}

		protected override void InitializeCulture()
		{
			Utility.SetThreadCulture();
		}

		/// <summary>
		/// Metodo usado para carregar a pagina com estilo e preencher a treview
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e) // load da pagina
		{
			if (!IsPostBack) // testa c nao eh postback
			{
				FillTreeView(DBConfig); // chama a funçao para encher a treeview com o nome de xml q esta nas configs.
			}
			Utility.CheckAuthentication(this,true);

		}

		/// <summary>
		/// Metodo usado para correr o xml do banco e encher a treeview com todas as informações que seram mostradas 
		/// </summary>
		/// <param name="vgXML"></param>
		private void FillTreeView(string vgXML) // funçao para encher a treeview
		{
			GxmlEstruct vgAuxiliar = new GxmlEstruct(); // vgAuxiliar passa a ser a classe de auxiliares
			DataSet vgDataSet = vgAuxiliar.ConvertXmlToDataTable(vgXML); // passa um arquivo xml pra dataset
			DataTable vgBancos = vgAuxiliar.getXmlBancos(vgDataSet); // pega todos os bancos q tiverem no data set
			foreach (DataRow vgBanco in vgBancos.Rows) // corre todos os bancos
			{
				RadTreeNode vgTvBanco = new RadTreeNode(vgBanco["NOME"].ToString()); // coloca o nome do banco na treeview
				vgTvBanco.Expanded = true; // diz q a treeview ta expandida
				DataTable vgTabelas = vgAuxiliar.getXmlTabelas(vgDataSet, vgBanco["NOME"].ToString()); // pega todas as tabelas do banco em q esta passando
				RadTreeNode vgTodasTabelas = new RadTreeNode("Tabelas"); // cria um RadTreeNode apenas filho do banco para separar as integridades das tabelas quando nescessario
				trvDataBase.Nodes.Add(vgTvBanco); // diz q o banco eh filho da treeview
				vgTvBanco.Nodes.Add(vgTodasTabelas); // diz q o node "tabelas" eh filho de banco
				vgTodasTabelas.Expanded = true; // diz q tabelas vai tar expandida
				foreach (DataRow vgTabela in vgTabelas.Rows) // corre todas as tabelas
				{
					RadTreeNode vgTvTabela = new RadTreeNode(vgTabela["NOME"].ToString()); // coloca o nome de cada tabela em um RadTreeNode
					DataTable vgCampos = vgAuxiliar.getXmlCampos(vgDataSet, vgTabela["NOME"].ToString(), vgBanco["NOME"].ToString()); // pega todos os campos da tabela e banco alvo
					DataTable vgIndices = vgAuxiliar.getXmlIndexs(vgDataSet, vgTabela["NOME"].ToString(), vgBanco["NOME"].ToString()); // pega todos os indices da tabela e banco alvo
					RadTreeNode vgTodosCampos = new RadTreeNode("Campos"); // cria um RadTreeNode com o nome campos para separar dos indices
					RadTreeNode vgTodosIndices = new RadTreeNode("Indices"); // cria um RadTreeNode com o nome Indices para separar dos Campos
					vgTodasTabelas.Nodes.Add(vgTvTabela); // colocas o nome da tabela como filho da RadTreeNode "tabela"
					vgTvTabela.Expanded = false; // coloca pra nao ficar expandido o nome da tabela
					vgTvTabela.Nodes.Add(vgTodosCampos); // coloca todos os campos como filho da tabela
					vgTodosCampos.Expanded = false; // coloca dizendo q os campos vai ficar escondido
					vgTvTabela.Nodes.Add(vgTodosIndices);// coloca todos os indices com filho da tabela
					vgTodosIndices.Expanded = false; // diz q todos os indices nao vai tar expandido
					foreach (DataRow vgCampo in vgCampos.Rows) // corre todos os campos da tabela alvo
					{
						RadTreeNode vgTvCampo = new RadTreeNode(vgCampo["NOME"].ToString() + " - " + vgCampo["TIPO"].ToString() + " (" + vgCampo["TAMANHO"].ToString() + ")"); // coloca o nome do campo na treeview
						vgTodosCampos.Nodes.Add(vgTvCampo); // coloca os campos filho de "campos" que é filho de tabela
					}
					foreach (DataRow vgIndice in vgIndices.Rows) // corre todos os indices
					{
						RadTreeNode vgTvIndice = new RadTreeNode(vgIndice["NOME"].ToString()); // coloca o nomde do indice na treeview
						DataTable vgCamposIndice = vgAuxiliar.getXmlCampoIndexs(vgDataSet, vgIndice["NOME"].ToString(), vgTabela["NOME"].ToString(), vgBanco["NOME"].ToString()); // pega todos os campos do indice alvo
						vgTodosIndices.Nodes.Add(vgTvIndice);//coloca os indices no node indices q eh filho de tabela
						vgTvIndice.Expanded = false; // diz que nao vai tar expandido
						foreach (DataRow vgCampoIndice in vgCamposIndice.Rows) // corre todos os campos do indice alvo
						{
							RadTreeNode vgTvCampoIndice = new RadTreeNode(vgCampoIndice["NOME"].ToString()); // coloca o nome do campo como filho do indice
							vgTvIndice.Nodes.Add(vgTvCampoIndice); // coloca os campos como filho de indices
						}
					}
				}
				if (vgChecado == true) // testa se o check box foi checado e tem q mostrar as integridades
				{
					DataTable vgIntegridades = vgAuxiliar.getXmlIntegridades(vgDataSet, vgBanco["NOME"].ToString()); // pega todas as integridades da tabela alvo
					RadTreeNode vgTodasIntegridades = new RadTreeNode("Integridades"); // cria um node chamado integridades para separar de tabelas
					vgTodasIntegridades.Expanded = true;// diz que vai ter expandido
					vgTvBanco.Nodes.Add(vgTodasIntegridades); // coloca como filho do banco
					foreach (DataRow vgTvIntegridade in vgIntegridades.Rows) // corre todas as integridades
					{
						RadTreeNode vgIntegridade = new RadTreeNode(vgTvIntegridade["TRIGGER"].ToString() == "" ? vgTvIntegridade["BASE"].ToString() + " => " + vgTvIntegridade["ESTRANGEIRA"].ToString() : vgTvIntegridade["BASE"].ToString() + " => " + vgTvIntegridade["ESTRANGEIRA"].ToString() + " (TRIGGER)"); // coloca na treeview o nome do campo base + o nome do campo estrangeiro
						vgTodasIntegridades.Nodes.Add(vgIntegridade); // colocas o RadTreeNode na treeview
						DataTable vgCamposIntegridades = vgAuxiliar.getXmlCamposIntegridade(vgDataSet, vgTvIntegridade["INTEGRIDADE_Id"].ToString(), vgBanco["NOME"].ToString());// pega todos os campos da integridade x
						foreach (DataRow vgCampoIntegridade in vgCamposIntegridades.Rows)// corre todos os campos da integridade
						{
							RadTreeNode vgTvCampoInt = new RadTreeNode(vgCampoIntegridade["BASE"].ToString() + " = " + vgCampoIntegridade["ESTRANGEIRO"].ToString()); // coloca no node os nomes dos campos iguais
							vgIntegridade.Nodes.Add(vgTvCampoInt);// coloca na treeview os campos
						}
						if (vgTvIntegridade["ATUALIZA"].ToString() == "1") // testa se atualiza em cascata
						{
							RadTreeNode vgAtt = new RadTreeNode("ATUALIZA EM CASCATA"); // coloca como filho da integridade atualiza em cascata
							vgIntegridade.Nodes.Add(vgAtt);// coloca o att em cascata na treeview
						}
						if (vgTvIntegridade["EXCLUI"].ToString() == "1") // testa se exclui em cascata
						{
							RadTreeNode vgExc = new RadTreeNode("EXCLUI EM CASCATA"); // coloca como filho da integridade exclui em cascata
							vgIntegridade.Nodes.Add(vgExc); // coloca o exc em cascata na treeview
						}
						vgIntegridade.Expanded = false; // coloca para nao ficar expandido

					}
				}
			}
		}
		
		/// <summary>
		/// Metodo usado para correr o DatabaseConfig do banco e encher a treeview com todas as informações que seram mostradas 
		/// </summary>
		/// <param name="vgXML"></param>
		private void FillTreeView(DatabaseConfig dc) // funçao para encher a treeview
		{
			foreach (Database Db in dc.DataBases) // corre todos os bancos
			{
				RadTreeNode vgTvBanco = new RadTreeNode(Db.Title); // coloca o nome do banco na treeview
				vgTvBanco.Expanded = true; // diz q a treeview ta expandida
				RadTreeNode vgTodasTabelas = new RadTreeNode("Tabelas"); // cria um RadTreeNode apenas filho do banco para separar as integridades das tabelas quando nescessario
				trvDataBase.Nodes.Add(vgTvBanco); // diz q o banco eh filho da treeview
				vgTvBanco.Nodes.Add(vgTodasTabelas); // diz q o node "tabelas" eh filho de banco
				vgTodasTabelas.Expanded = true; // diz q tabelas vai tar expandida
				RadTreeNode vgTodasIntegridades = null;
				if (vgChecado == true) // testa se o check box foi checado e tem q mostrar as integridades
				{
					vgTodasIntegridades = new RadTreeNode("Integridades"); // cria um node chamado integridades para separar de tabelas
					vgTodasIntegridades.Expanded = true;// diz que vai ter expandido
					vgTvBanco.Nodes.Add(vgTodasIntegridades); // coloca como filho do banco
				}
				foreach (PROJETO.Table tb in Db.Tables) // corre todas as tabelas
				{
					RadTreeNode vgTvTabela = new RadTreeNode(tb.Title); // coloca o nome de cada tabela em um RadTreeNode
					RadTreeNode vgTodosCampos = new RadTreeNode("Campos"); // cria um RadTreeNode com o nome campos para separar dos indices
					RadTreeNode vgTodosIndices = new RadTreeNode("Indices"); // cria um RadTreeNode com o nome Indices para separar dos Campos
					RadTreeNode vgTodosRelacionamentos = new RadTreeNode("Relacionamentos"); // cria um RadTreeNode com o nome Relacionamentos para separar dos Campos e Indices
					vgTodasTabelas.Nodes.Add(vgTvTabela); // coloca o nome da tabela como filho da RadTreeNode "tabela"
					vgTvTabela.Expanded = false; // coloca pra nao ficar expandido o nome da tabela
					vgTvTabela.Nodes.Add(vgTodosCampos); // coloca todos os campos como filho da tabela
					vgTodosCampos.Expanded = false; // coloca dizendo q os campos vao ficar escondidos
					vgTvTabela.Nodes.Add(vgTodosIndices);// coloca todos os indices como filho da tabela
					vgTodosIndices.Expanded = false; // diz q todos os indices nao vao estar expandidos
					vgTvTabela.Nodes.Add(vgTodosRelacionamentos);// coloca todos os relacionamentos como filho da tabela
					vgTodosRelacionamentos.Expanded = false; // diz q todos os relacionamentos nao vao estar expandido
					foreach (Field fd in tb.Fields) // corre todos os campos da tabela alvo
					{
						RadTreeNode vgTvCampo = new RadTreeNode(fd.Title + " - " + fd.Type + " (" +fd.Size + ")"); // coloca o nome do campo na treeview
						vgTodosCampos.Nodes.Add(vgTvCampo); // coloca os campos filho de "campos" que é filho de tabela
					}
					foreach (Index Ix in tb.Index) // corre todos os indices
					{
						RadTreeNode vgTvIndice = new RadTreeNode(Ix.Title); // coloca o nomde do indice na treeview
						vgTodosIndices.Nodes.Add(vgTvIndice);//coloca os indices no node indices q eh filho de tabela
						vgTvIndice.Expanded = false; // diz que nao vai tar expandido
						foreach (IndexField vgCampoIndice in Ix.Fields) // corre todos os campos do indice alvo
						{
							RadTreeNode vgTvCampoIndice = new RadTreeNode(vgCampoIndice.Title); // coloca o nome do campo como filho do indice
							vgTvIndice.Nodes.Add(vgTvCampoIndice); // coloca os campos como filho de indices
						}
					}
					foreach (Integrity It in tb.Integrities) // corre todas as integridades
					{
						RadTreeNode vgIntegridade = new RadTreeNode(It.Base + " => " + It.Foreign + (It.Trigger != "" ? " (TRIGGER)" : "")); // coloca na treeview o nome do campo base + o nome do campo estrangeiro
						vgTodosRelacionamentos.Nodes.Add(vgIntegridade); // colocas o RadTreeNode na treeview
						foreach (IntegrityField ItFd in It.Fields)// corre todos os campos da integridade
						{
							RadTreeNode vgTvCampoInt = new RadTreeNode(ItFd.Base + " = " + ItFd.Foreign); // coloca no node os nomes dos campos iguais
							vgIntegridade.Nodes.Add(vgTvCampoInt);// coloca na treeview os campos
						}
					}
					if (vgChecado == true) // testa se o check box foi checado e tem q mostrar as integridades
					{
						//+RadTreeNode vgTodasIntegridades = new RadTreeNode("Integridades"); // cria um node chamado integridades para separar de tabelas
						//+vgTodasIntegridades.Expanded = true;// diz que vai ter expandido
						//+vgTvBanco.Nodes.Add(vgTodasIntegridades); // coloca como filho do banco
						foreach (Integrity It in tb.Integrities) // corre todas as integridades
						{
							if (vgTodasIntegridades == null)
							{
								vgTodasIntegridades = new RadTreeNode("Integridades"); // cria um node chamado integridades para separar de tabelas
								vgTodasIntegridades.Expanded = true;// diz que vai estar expandido
								vgTvBanco.Nodes.Add(vgTodasIntegridades); // coloca como filho do banco
							}
							RadTreeNode vgIntegridade = new RadTreeNode(It.Base + " => " + It.Foreign + (It.Trigger != "" ?" (TRIGGER)":"")); // coloca na treeview o nome do campo base + o nome do campo estrangeiro
							vgTodasIntegridades.Nodes.Add(vgIntegridade); // colocas o RadTreeNode na treeview
							foreach (IntegrityField ItFd in It.Fields)// corre todos os campos da integridade
							{
								RadTreeNode vgTvCampoInt = new RadTreeNode(ItFd.Base + " = " + ItFd.Foreign); // coloca no node os nomes dos campos iguais
								vgIntegridade.Nodes.Add(vgTvCampoInt);// coloca na treeview os campos
							}
							if (It.Refresh == "1") // testa se atualiza em cascata
							{
								RadTreeNode vgAtt = new RadTreeNode("ATUALIZA EM CASCATA"); // coloca como filho da integridade atualiza em cascata
								vgIntegridade.Nodes.Add(vgAtt);// coloca o att em cascata na treeview
							}
							if (It.Exclude == "1") // testa se exclui em cascata
							{
								RadTreeNode vgExc = new RadTreeNode("EXCLUI EM CASCATA"); // coloca como filho da integridade exclui em cascata
								vgIntegridade.Nodes.Add(vgExc); // coloca o exc em cascata na treeview
							}
							vgIntegridade.Expanded = false; // coloca para nao ficar expandido

						}
					}
				}
			}
		}
		public void ShowRelations(bool Visible)
		{
			trvDataBase.Nodes.Clear(); // limpa a treeview
			vgChecado = Visible;
			FillTreeView(DBConfig); // chama a funçao para preencher a treeview
		}

		protected void SepairIntegrity()
		{
			ShowRelations((bool)chkIntegrity.Checked);
		}
		protected void ___chkIntegrity_OnCheckedChanged(object sender, EventArgs e)
		{
			bool ActionSucceeded_1 = true;
			try
			{
				SepairIntegrity();
			}
			catch (Exception ex)
			{
				ActionSucceeded_1 = false;
			}
		}

		
	}
}
