using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDictSet<T> : Change<T>
{

    public List<T> addValues = new List<T>();
    public List<T> removeValues = new List<T>();

    public override Type getType()
    {
        return Type.DictSet;
    }

}