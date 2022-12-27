using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommonLibrary;

public class SortData : Data
{

	public VO<string> filter;

	#region SortType

	public enum SortType
	{
		None,
		Name,
		Kind,
		Date
	}

	public VO<SortType> sortType;

    private static readonly TxtLanguage txtNone = new TxtLanguage("None");
    private static readonly TxtLanguage txtName = new TxtLanguage("Name");
    private static readonly TxtLanguage txtKind = new TxtLanguage("Kind");
    private static readonly TxtLanguage txtDate = new TxtLanguage("Time");

    static SortData()
    {
        txtNone.add(CommonLibrary.Language.Type.vi, "Không");
        txtName.add(CommonLibrary.Language.Type.vi, "Tên");
        txtKind.add(CommonLibrary.Language.Type.vi, "Loại");
        txtDate.add(CommonLibrary.Language.Type.vi, "Giờ");
    }

    public static List<string> getSortTypeList()
    {
        List<string> ret = new List<string>();
        {
            ret.Add(txtNone.get());
            ret.Add(txtName.get());
            ret.Add(txtKind.get());
            ret.Add(txtDate.get());
        }
        return ret;
    }

    #endregion

    #region Constructor

    public enum Property
	{
		filter,
		sortType
	}

	public SortData() : base()
	{
		this.filter = new VO<string> (this, (byte)Property.filter, "");
		this.sortType = new VO<SortType> (this, (byte)Property.sortType, SortType.None);
	}

	#endregion

	public void reset()
	{
		this.filter.v = "";
		this.sortType.v = SortType.None;
	}

}