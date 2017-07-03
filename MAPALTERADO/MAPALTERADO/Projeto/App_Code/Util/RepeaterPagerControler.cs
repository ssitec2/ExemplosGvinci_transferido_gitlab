using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using PROJETO.DataProviders;
using System.Data;

/// <summary>
/// Summary description for RepeaterPagerControler
/// </summary>
public static class RepeaterPagerControler
{

	public static int GetRepaterPageNumber(GeneralDataProvider Provider, string FilterPosition, int PageSize, out DataRow Row)
	{
		int ret = 0;
		int Count = Provider.PageCount;
		System.Data.DataTable Data = Provider.SelectItems(0, 0, out Count);
		if (Data.Rows.Count > 0)
		{
			System.Data.DataRow[] drs = Data.Select(FilterPosition.Replace("N'", "'"));
			if (drs.Length > 0)
			{
				ret = Data.Rows.IndexOf(drs[0]) / PageSize;
				Row = drs[0];
			}
			else
			{
				Row = null;
			}
		}
		else
		{
			Row = null;
		}
		return ret;
	}

    public static System.Data.DataTable ControlPagesButtons(Panel Panel, int ButtonsCount, int PageSize, ref int CurrentPage, bool HasLastFirst, bool HasNextPreviews, GeneralDataProvider Provider)
    {
        int Count = Provider.PageCount;
        System.Data.DataTable Data = Provider.SelectItems(CurrentPage, PageSize, out Count);
        Count = (int)Math.Ceiling((decimal)Count / (decimal)PageSize);
        if (CurrentPage == -1)
        {
            CurrentPage = Count - 1;
        }
        else if (CurrentPage >= Count && Count > 0)
        {
            CurrentPage = Count - 1;
			Data = Provider.SelectItems(CurrentPage, PageSize, out Count);
			Count = (int)Math.Ceiling((decimal)Count / (decimal)PageSize);
        }

        if (Panel != null)
        {
            if (Count == 1)
            {
                Panel.Visible = false;
                return Data;
            }
            else if (Count >= 2)
            {
                Panel.Visible = true;
            }

            int startNum = 0;
            int startBtn = 1;
            int BtnPosCounter = 1;
            if ((int)((ButtonsCount + 1) / 2) < CurrentPage + 1)
            {
                if ((ButtonsCount & 1) == 1)
                {
                    startNum = (CurrentPage) - ((ButtonsCount) / 2);
                }
                else
                {
                    startNum = (CurrentPage + 1) - ((ButtonsCount) / 2);
                }
            }
            if (startNum > 0 && startNum + ButtonsCount > Count)
            {
                startNum = Count - ButtonsCount;
            }

            if (ButtonsCount >= Count)
            {
                startNum = 0;
            }

            if (Count < ButtonsCount)
            {
                startBtn = ButtonsCount - Count + startBtn;
            }
            BtnPosCounter = startBtn;
            int VisCount = BtnPosCounter;

            if (HasLastFirst)
            {
                ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).Text = "<<";
                ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).CommandArgument = "F";
                ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).Visible = true;
                BtnPosCounter++;
            }
            if (HasNextPreviews)
            {
                ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).Text = "<";
                ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).CommandArgument = "P";
                ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).Visible = true;
                BtnPosCounter++;
            }

            if (Count >= ButtonsCount)
            {
                for (int i = 0; i < ButtonsCount; i++)
                {
                    ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).Text = (startNum + 1).ToString();
                    ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).CommandArgument = startNum.ToString();
                    ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).Visible = true;
                    BtnPosCounter++;
                    startNum++;
                }
            }
            else
            {
                for (int i = 0; i < Count; i++)
                {
                    ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).Text = (startNum + 1).ToString();
                    ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).CommandArgument = startNum.ToString();
                    ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).Visible = true;
                    BtnPosCounter++;
                    startNum++;
                }
            }

            if (HasNextPreviews)
            {
                ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).Text = ">";
                ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).CommandArgument = "N";
                ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).Visible = true;
                BtnPosCounter++;
            }
            if (HasLastFirst)
            {
                ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).Text = ">>";
                ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).CommandArgument = "L";
                ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnPosCounter.ToString())).Visible = true;
            }

            for (int BtnN = VisCount - 1; BtnN >= 1; BtnN--)
            {
                ((Button)Panel.FindControl("__" + Panel.ID + "__Button" + BtnN.ToString())).Visible = false;
            }
        }
        return Data;
    }
}
