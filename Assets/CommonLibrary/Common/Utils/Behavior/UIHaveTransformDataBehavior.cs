using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class UIHaveTransformDataBehavior<K> : UIBehavior<K>, HaveTransformData where K : Data
{

    #region TransformData

    public TransformData transformData = new TransformData();

    private void updateTransformData()
    {
        this.transformData.update(this.transform);
    }

    public TransformData getTransformData()
    {
        return this.transformData;
    }

    public Data getDataHaveTransformData()
    {
        return this.data;
    }

    public Transform getUITransform()
    {
        return this.transform;
    }

    #endregion

    public void setDirtyForTransformData()
    {
        this.dirty = true;
    }

    #region lifeCycle

    /*public GlobalUICheckChange checkChange = null;

    public override void Awake()
    {
        base.Awake();
        // checkChange
        {
            checkChange = new GlobalUICheckChange(this);
            checkChange.setData(Global.get());
        }
    }*/

    public override void onDespawn()
    {
        base.onDespawn();
        // transformData
        // Debug.LogError("onDespawn");
        transformDataClear();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        // checkChange
        /*if (checkChange != null)
        {
            checkChange.setData(null);
        }
        else
        {
            Debug.LogError("checkChange null");
        }*/
        // transformData
        transformDataClear();
    }

    private void transformDataClear()
    {
        // Debug.LogError("transformData clear: " + this);
        transformData.callBacks.Clear();
        transformData.reset();
    }

    #endregion

    #region refresh

    // TODO Tam bo
    /*public override void FixedUpdate()
    {
        base.FixedUpdate();
        updateTransformData();
    }*/
    void FixedUpdate()
    {
        updateTransformData();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        updateTransformData();
    }

    void Start()
    {
        updateTransformData();
    }

    void OnGUI()
    {
        updateTransformData();
    }

    /*public override void onStart()
    {
        base.onStart();
        updateTransformData();
    }

    public override void OnGUI()
    {
        base.OnGUI();
        updateTransformData();
    }*/

    /*public override void OnPreCull()
    {
        base.OnPreCull();
        updateTransformData();
    }*/

    public override void onAfterSetData()
    {
        base.onAfterSetData();
        updateTransformData();
    }

    #endregion

}