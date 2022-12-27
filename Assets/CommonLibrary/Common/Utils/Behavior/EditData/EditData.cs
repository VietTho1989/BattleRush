using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Compare co le nen them vao compare with other type
 * */
public class EditData<K> : Data, ValueChangeCallBack where K : Data
{

    public VO<ReferenceData<K>> origin;

    public VO<ReferenceData<K>> show;

    public VO<ReferenceData<K>> compare;

    public VO<ReferenceData<Data>> compareOtherType;

    public VO<bool> canEdit;

    public VO<EditType> editType;

    #region Constructor

    public enum Property
    {
        origin,
        show,
        compare,
        compareOtherType,
        canEdit,
        editType
    }

    public EditData() : base()
    {
        this.origin = new VO<ReferenceData<K>>(this, (byte)Property.origin, ReferenceData<K>.Null);
        this.show = new VO<ReferenceData<K>>(this, (byte)Property.show, ReferenceData<K>.Null);
        this.compare = new VO<ReferenceData<K>>(this, (byte)Property.compare, ReferenceData<K>.Null);
        this.compareOtherType = new VO<ReferenceData<Data>>(this, (byte)Property.compareOtherType, ReferenceData<Data>.Null);
        this.canEdit = new VO<bool>(this, (byte)Property.canEdit, false);
        this.editType = new VO<EditType>(this, (byte)Property.editType, EditType.Immediate);
        // add callBacks
        this.addCallBack(this);
    }

    #endregion

    private bool dirty = true;
    /** co cai haveOriginChange the nay thi do phai reset khi doi origin*/
    private bool haveOriginChange = true;

    /**
	 * Copy nhung cai co the edit
	 * */
    public List<byte> allowNames = null;

    public void update()
    {
        if (dirty)
        {
            dirty = false;
            if (this.origin.v.data != null)
            {
                if (this.editType.v == EditType.Immediate)
                {
                    this.show.v = new ReferenceData<K>(this.origin.v.data);
                }
                else
                {
                    // need create complete new
                    if (this.show.v.data == null || this.show.v.data == this.origin.v.data || this.show.v.data.GetType() != this.origin.v.data.GetType())
                    {
                        this.show.v = new ReferenceData<K>((K)DataUtils.cloneData(this.origin.v.data));
                    }
                    else
                    {
                        // update when origin change
                        if (haveOriginChange)
                        {
                            DataUtils.copyData(this.show.v.data, this.origin.v.data, allowNames);
                        }
                    }
                }
            }
            else
            {
                this.show.v = ReferenceData<K>.Null;
            }
            haveOriginChange = false;
        }
    }

    #region implement callBacks

    public void onAddCallBack<T>(T data) where T : Data
    {
        if (data is EditData<K>)
        {
            return;
        }
        Logger.LogError("Don't process: " + data + "; " + this);
    }

    public void onRemoveCallBack<T>(T data, bool isHide) where T : Data
    {
        if (data is EditData<K>)
        {
            return;
        }
        Logger.LogError("Don't process: " + data + "; " + this);
    }

    public void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
    {
        if (WrapProperty.checkError(wrapProperty))
        {
            return;
        }
        if (wrapProperty.p is EditData<K>)
        {
            switch ((EditData<K>.Property)wrapProperty.n)
            {
                case Property.origin:
                    {
                        haveOriginChange = true;
                        dirty = true;
                    }
                    break;
                case Property.show:
                    dirty = true;
                    break;
                case Property.compare:
                    break;
                case Property.compareOtherType:
                    break;
                case Property.canEdit:
                    break;
                case Property.editType:
                    dirty = true;
                    break;
                default:
                    Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                    break;
            }
            return;
        }
        Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
    }

    #endregion

}