using System;
using System.IO;
using System.IO.Compression;
using System.Web.UI;
using PROJETO.DataPages;

namespace PROJETO
{

	/// <summary>
	/// Classe com funções para controle de compactação de viewstate para páginas
	/// </summary>
	public static class ViewStateManager
	{

		/// <summary>
		/// Descompacta viewstate de uma página caso necessário
		/// </summary>
		/// <param name="BasePage">Referência da página que contém a viewstate</param>
		/// <returns>Retorna viewstate já desserializada</returns>
		public static object DecompressViewState(Page BasePage)
		{
			// tenta buscar viewstate compactada no form
			string viewState = BasePage.Request.Form["__NVIEWSTATE"];

			// se tem encontrou viewstate compactada no form, descompacta
			if (viewState != null && viewState.Substring(0, 2) == "1'")
			{
				// retira os bytes de controle e retira o base64
				byte[] bytes = Convert.FromBase64String(viewState.Remove(0, 2));

				// descompacta o conteúdo da viewstate    
				MemoryStream msZippedViewState = new MemoryStream();
				msZippedViewState.Write(bytes, 0, bytes.Length);
				msZippedViewState.Position = 0;
				GZipStream Zip = new GZipStream(msZippedViewState, CompressionMode.Decompress, true);
				MemoryStream msViewState = new MemoryStream();
				byte[] Buffer = new byte[128];
				int ReadBytes = Zip.Read(Buffer, 0, Buffer.Length);
				while (ReadBytes > 0)
				{
					msViewState.Write(Buffer, 0, ReadBytes);
					ReadBytes = Zip.Read(Buffer, 0, Buffer.Length);
				}
				Zip.Close();
				bytes = msViewState.ToArray();
				msViewState.Close();
				msZippedViewState.Close();

				// converte novo conteúdo, já descompactado, para base 64
				viewState = Convert.ToBase64String(bytes);
			}

			// formatador para desserializar a viewstate
			LosFormatter formatter = new LosFormatter();

			// retorna viewstate desserializada
			if(viewState != null)
				return formatter.Deserialize(viewState);
			return null;
		}

		/// <summary>
		/// Compacta viewstate para página caso necessário
		/// </summary>
		/// <param name="BasePage">Referência da página que contém a viewstate</param>
		/// <param name="viewState">Valor da viewstate recebido por SavePageStateToPersistenceMedium</param>
		public static void CompressViewState(Page BasePage, object viewState)
		{
			// formatador para serializar a viewstate
			LosFormatter formatter = new LosFormatter();

			// responsável por receber os dados serializados da viewstate
			StringWriter swViewState = new StringWriter();

			// serializa viewstate
			formatter.Serialize(swViewState, viewState);

			// guarda valor original da viewstate
			string OriginalViewState = swViewState.ToString();

			// retira o base64
			byte[] bytes = Convert.FromBase64String(OriginalViewState);

			// compacta o conteúdo da viewstate
			MemoryStream msViewState = new MemoryStream();
			GZipStream Zip = new GZipStream(msViewState, CompressionMode.Compress, true);
			Zip.Write(bytes, 0, bytes.Length);
			Zip.Close();
			bytes = msViewState.ToArray();
			msViewState.Close();

			// converte novo conteúdo, já compactado, para base64
			string NewViewState = Convert.ToBase64String(bytes);

			if (BasePage is GeneralDataPage)
			{
				CompressViewStateWithTelerik((GeneralDataPage)BasePage, OriginalViewState, NewViewState);
			}
			else
			{
				// verifica se a compactação diminui o tamanho da viewstate
				// em pelo menos 10%, se não diminuir usa viewstate original
				if (NewViewState.Length > OriginalViewState.Length * 0.9)
				{
					BasePage.ClientScript.RegisterHiddenField("__NVIEWSTATE", OriginalViewState);
				}
				else
				{
					BasePage.ClientScript.RegisterHiddenField("__NVIEWSTATE", "1'" + NewViewState);
				}
			}			
		}

		private static void CompressViewStateWithTelerik(GeneralDataPage BasePage, string OriginalViewState, string NewViewState)
		{
			// verifica se a compactação diminui o tamanho da viewstate
			// em pelo menos 10%, se não diminuir usa viewstate original
			if (NewViewState.Length > OriginalViewState.Length * 0.9)
			{
				BasePage.RegisterTelerikHiddenField("__NVIEWSTATE", OriginalViewState);
			}
			else
			{
				BasePage.RegisterTelerikHiddenField("__NVIEWSTATE", "1'" + NewViewState);
			}
		}
	}
}
