using UnityEngine;
using System.Collections;

public abstract class Change<T>
{

	public enum Type
	{
		Set,
		Add,
		Remove,

        DictAdd,
        DictRemove,
        DictSet
	}

	public abstract Type getType();

}

