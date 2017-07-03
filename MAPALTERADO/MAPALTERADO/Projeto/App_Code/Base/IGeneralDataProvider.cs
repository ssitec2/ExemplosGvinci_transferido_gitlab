using System;
using System.Collections.Generic;
using System.Web;

namespace PROJETO.DataProviders
{
	public enum FormPositioningEnum
	{
		None,
		Current,
		First,
		Previous,
		Next,
		Last
	}

	/// <summary>
	/// Este Enum Define se o lancamento sera de Insert, Update ou Delete
	/// </summary>
	public enum EntryCommand
	{
		Insert,
		Update,
		Delete
	}

	/// <summary>
	/// Summary description for GeneralDataProviderInterface
	/// </summary>
	public interface IGeneralDataProvider
	{
		GeneralDataProvider DataProvider { get; set; }
		string FormID { get; }
		string TableName { get; }
		string DatabaseName { get; }

		GeneralDataProviderItem LoadItemFromControl(bool EnableValidation);
		GeneralDataProviderItem LoadItemFromGridControl(bool EnableValidation, string Grid);
		GeneralDataProviderItem LoadItemFromSchedulerControl(bool EnableValidation, string Scheduler);
		GeneralDataProviderItem LoadItemFromImageGalleryControl(bool EnableValidation, string ImageGallery);
		GeneralDataProviderItem LoadItemFromGanttControl(bool EnableValidation, string Gantt);
		GeneralDataProviderItem LoadItemFromDependenciesGanttControl(bool EnableValidation, string Gantt);
		GeneralDataProviderItem GetDataProviderItem(GeneralDataProvider Provider);
		GeneralDataProviderItem GetCurrentItem(FormPositioningEnum Positioning, bool UpdateFromUI);
		void OnSelectedItem(GeneralDataProvider Provider, GeneralDataProviderItem Item, bool UpdateFromUI);
		void GetParameters(bool KeepCurrentRecord, GeneralDataProvider Provider);
		void SetParametersValues(GeneralDataProvider Provider);
		void SetOldParameters(GeneralDataProviderItem Item);
		void DeleteChildItens();
		void OnCommiting();
		void OnRollbacking();
	}
}
