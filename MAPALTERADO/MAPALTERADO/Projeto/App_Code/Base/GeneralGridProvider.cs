using System;
using COMPONENTS;
using COMPONENTS.Data;
using COMPONENTS.Configuration;
using System.Collections.Specialized;
using System.Collections;
using PROJETO.DataPages;

namespace PROJETO.DataProviders
{
	public abstract class GeneralGridProvider : GeneralProvider, IGeneralDataProvider
	{
		public NameValueCollection PageErrors;
		public Hashtable GridData { get; set; }
		public Hashtable GridDataParameters { get; set; }

		public delegate void SetRelationParametersEventHandler(object sender);
		public event SetRelationParametersEventHandler SetRelationParameters;
		public void RaiseSetRelationParametersEvent(object sender)
		{
			if (SetRelationParameters != null)
			{
				SetRelationParameters(sender);
			}
		}

		public delegate void SetRelationFieldsEventHandler(object sender, GeneralDataProviderItem Item);
		public event SetRelationFieldsEventHandler SetRelationFields;
		public void RaiseSetRelationFields(object sender, GeneralDataProviderItem Item)
		{
			if (SetRelationFields != null)
			{
				SetRelationFields(sender, Item);
			}
		}

		public new virtual GeneralDataProviderItem GetDataProviderItem(GeneralDataProvider Provider)
		{
			return null;
		}

		public virtual void RefreshParentProvider(GeneralProvider ParentProvider)
		{
		}

		public virtual void SetParametersValues(GeneralDataProvider Provider)
		{
		}

		public virtual void InitializeAlias(GeneralDataProviderItem Item)
		{
		}

		public virtual bool Validate(GeneralDataProviderItem ProviderItem)
		{
			return true;
		}

		public virtual string GetMediaQuery(GeneralDataProviderItem Item, string Column)
		{
			return "";
		}

		public void InsertItem(IGeneralDataProvider BaseInterface, string GridId, Hashtable GridData)
		{
			GeneralDataProviderItem Item = null;
			try
			{
				this.GridData = GridData;
				Item = BaseInterface.LoadItemFromGridControl(true, GridId);
				if (Item.Errors.Count > 0)
				{
					PageErrors = new NameValueCollection();
                    PageErrors.Add(Item.Errors);
				}
				else
                {
                    DataProvider.InsertItem(Item);
                }
			}
			catch (Exception ex)
			{
				Item.Errors.Add("grid", ex.Message);
				if (PageErrors == null)
                {
                    PageErrors = new NameValueCollection();
                }
                PageErrors.Add("Error", ex.Message);
			}
		}

		public void UpdateItem(IGeneralDataProvider BaseInterface, string GridId, Hashtable GridData, Hashtable GridDataParameters)
		{
			GeneralDataProviderItem Item = null;
			try
			{
				this.GridData = GridData;
				this.GridDataParameters = GridDataParameters;
				DataProvider.SelectItem(false, 0, FormPositioningEnum.Current);
				Item = BaseInterface.LoadItemFromGridControl(true, GridId);
                if (Item.Errors.Count > 0)
                {
                    PageErrors = new NameValueCollection();
                    PageErrors.Add(Item.Errors);
                }
                else
                {
                    DataProvider.UpdateItem(Item);
                }
			}
			catch (Exception ex)
			{
			    if (PageErrors == null)
                {
                    PageErrors = new NameValueCollection();
                }
                PageErrors.Add("Error",ex.Message);
			}
		}

		public void DeleteItem(IGeneralDataProvider BaseInterface, string GridId, Hashtable GridData, Hashtable GridDataParameters)
		{
			GeneralDataProviderItem Item = null;
			try
			{
				this.GridData = GridData;
				this.GridDataParameters = GridDataParameters;
				Item = BaseInterface.LoadItemFromGridControl(false, GridId);
				if (Item.Errors.Count > 0)
				{
					((GeneralDataPage)BaseInterface).ShowErrors();
					return;
				}
				DataProvider.DeleteItem(Item);
			}
			catch (Exception ex)
			{
				if (PageErrors == null)
                {
                    PageErrors = new NameValueCollection();
                }
                PageErrors.Add("Error", ex.Message);
			}
		}

		public void LocateRecord(IGeneralDataProvider BaseInterface, string GridId, Hashtable GridData)
		{
			GeneralDataProviderItem Item = null;
			try
			{
				this.GridData = GridData;
				Item = BaseInterface.LoadItemFromGridControl(false, GridId);
				if (Item.Errors.Count > 0)
				{
					((GeneralDataPage)BaseInterface).ShowErrors();
					return;
				}
				DataProvider.LocateRecord(Item);
			}
			catch (Exception ex)
			{
			}
		}

		public void SelectItem(IGeneralDataProvider BaseInterface, string GridId, Hashtable GridData)
		{
			try
			{
				this.GridData = GridData;
				this.GridDataParameters = GridData;
				DataProvider.SelectItem(0, FormPositioningEnum.Current);
			}
			catch (Exception ex)
			{
			}
		}

		public void CreateEmptyParameters(GeneralDataProvider Provider)
		{
			if (DataProvider == Provider)
			{
				Provider.Parameters.Clear();
				Provider.CreateParameters();
			}
		}

		#region IGeneralDataProvider Members

		public abstract GeneralDataProvider DataProvider { get; set; }
		public abstract string TableName { get; }
		public abstract string DatabaseName { get; }
		public abstract string FormID { get; }

		public void GetParameters(bool KeepCurrentRecord, GeneralDataProvider Provider)
		{
			CreateEmptyParameters(Provider);
			if (KeepCurrentRecord)
			{
				if (GridData != null)
				{
					foreach (string ParamKey in Provider.Parameters.Keys)
					{
						Provider.Parameters[ParamKey].Parameter.SetValue(GridDataParameters[ParamKey]);
					}
				}
			}
			else
			{
				RaiseSetRelationParametersEvent(this);
			}
		}

		public void OnSelectedItem(GeneralDataProvider Provider, GeneralDataProviderItem Item, bool UpdateFromUI)
		{
			if (Provider == DataProvider)
			{
				InitializeAlias(Item);
				FillAuxiliarTables();
				ShowFormulas();
				SetLinks();
			}
		}

		public GeneralDataProviderItem LoadItemFromControl(bool EnableValidation)
		{
			return null;
		}

		public GeneralDataProviderItem LoadItemFromGridControl(bool EnableValidation, string Grid)
		{
			return null;
		}

		public GeneralDataProviderItem LoadItemFromSchedulerControl(bool EnableValidation, string Scheduler)
		{
			return null;
		}

		public GeneralDataProviderItem LoadItemFromImageGalleryControl(bool EnableValidation, string ImageGallery)
		{
			return null;
		}

		public GeneralDataProviderItem LoadItemFromGanttControl(bool EnableValidation, string Gantt)
		{
			return null;
		}

		public GeneralDataProviderItem LoadItemFromDependenciesGanttControl(bool EnableValidation, string Gantt)
		{
			return null;
		}

		public abstract void SetOldParameters(GeneralDataProviderItem Item);

		public virtual void ShowFormulas()
		{
		}

		public void DeleteChildItens()
		{
		}

		public virtual void SetLinks()
		{
		}

		public void OnCommiting()
		{
		}

		public void OnRollbacking()
		{
		}

		public GeneralDataProviderItem GetCurrentItem(FormPositioningEnum Positioning, bool UpdateFromUI)
		{
			return DataProvider.SelectItem(true, MainProvider.DataProvider.PageNumber, Positioning, UpdateFromUI);
		}
		#endregion
	}
}
