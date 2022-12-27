using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Locale : Data
{

	public VO<CommonLibrary.Language.Type> type;

	public VO<string> txt;

	#region Constructor

	public enum Property
	{
		type,
		txt
	}

	public Locale() : base()
	{
		this.type = new VO<CommonLibrary.Language.Type> (this, (byte)Property.type, CommonLibrary.Language.Type.en);
		this.txt = new VO<string> (this, (byte)Property.txt, "");
	}

	#endregion

}