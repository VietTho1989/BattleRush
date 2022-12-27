using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDictRemove<T> : Change<T>
{

    public List<T> values = new List<T>();

    public override Type getType()
    {
        return Type.DictRemove;
    }

}