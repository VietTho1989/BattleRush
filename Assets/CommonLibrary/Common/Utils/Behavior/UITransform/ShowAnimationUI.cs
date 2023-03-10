using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowAnimationUI : UIBehavior<ShowAnimationUI.UIData>
{

    #region state

    public abstract class State : Data
    {

        public enum Type
        {
            Show,
            Normal,
            Hide
        }

        public abstract Type getType();

    }

    public const float ShowDuration = 0.3f;

    public class Show : State
    {

        public VO<float> time;

        public VO<float> duration;

        #region Constructor

        public enum Property
        {
            time,
            duration
        }

        public Show() : base()
        {
            this.time = new VO<float>(this, (byte)Property.time, 0f);
            this.duration = new VO<float>(this, (byte)Property.duration, ShowDuration);
        }

        #endregion

        public override Type getType()
        {
            return Type.Show;
        }

    }

    public class Normal : State
    {

        #region Constructor

        public enum Property
        {

        }

        public Normal() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Normal;
        }

    }

    public class Hide : State
    {

        public VO<float> time;

        public VO<float> duration;

        #region Constructor

        public enum Property
        {
            time,
            duration
        }

        public Hide() : base()
        {
            this.time = new VO<float>(this, (byte)Property.time, 0);
            this.duration = new VO<float>(this, (byte)Property.duration, ShowDuration);
        }

        #endregion

        public override Type getType()
        {
            return Type.Hide;
        }

    }

    #endregion

    #region UIData

    public class UIData : Data
    {

        public VD<State> state;

        #region hide event

        public delegate void OnHide();

        public VO<OnHide> onHide;

        #endregion

        #region Constructor

        public enum Property
        {
            state,
            onHide
        }

        public UIData() : base()
        {
            this.state = new VD<State>(this, (byte)Property.state, new Normal());
            this.onHide = new VO<OnHide>(this, (byte)Property.onHide, null);
        }

        #endregion

        public void show()
        {
            if(!(this.state.v is Show))
            {
                ShowAnimationUI.Show show = new Show();
                {
                    show.uid = this.state.makeId();
                }
                this.state.v = show;
                // set hide position
                {
                    ShowAnimationUI showAnimationUI = this.findCallBack<ShowAnimationUI>();
                    if (showAnimationUI != null)
                    {
                        showAnimationUI.setHidePosition();
                    }
                    else
                    {
                        Logger.LogError("showAnimationUI null");
                    }
                }
            }
        }

        public void hide()
        {
            if ((this.state.v is ShowAnimationUI.Normal))
            {
                ShowAnimationUI.Hide hide = new ShowAnimationUI.Hide();
                {
                    hide.uid = this.state.makeId();
                }
                this.state.v = hide;
            }
            else
            {
                Logger.LogError("state error: " + this.state.v);
            }
        }

    }

    #endregion

    #region Refresh

    private float LastTime = float.MinValue;

    public override void refresh()
    {
        if (dirty)
        {
            dirty = false;
            if (this.data != null)
            {
                State state = this.data.state.v;
                if (state != null)
                {
                    switch (state.getType())
                    {
                        case State.Type.Show:
                            {
                                Show show = state as Show;
                                // time
                                if (show.time.v <= show.duration.v)
                                {
                                    if (Time.time == LastTime)
                                    {
                                        // Debug.LogError("the same last time: " + LastTime);
                                        dirty = true;
                                    }
                                    else
                                    {
                                        show.time.v += Time.fixedDeltaTime;
                                    }
                                }
                                else
                                {
                                    // chuyen sang normal
                                    Normal normal = new Normal();
                                    {
                                        normal.uid = this.data.state.makeId();
                                    }
                                    this.data.state.v = normal;
                                }
                                // UI
                                {
                                    RectTransform rectTransform = (RectTransform)this.transform;
                                    if (rectTransform != null)
                                    {
                                        if (rectTransform.rect.height > 0)
                                        {
                                            if (show.duration.v > 0)
                                            {
                                                UIRectTransform.SetPosY(rectTransform, Mathf.Clamp(
                                                    ((show.duration.v-show.time.v)/show.duration.v)*(-rectTransform.rect.height),
                                                     -rectTransform.rect.height, 0));
                                            }
                                            else
                                            {
                                                Logger.LogError("show duration < 0: " + show.duration.v);
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("rectTransform rect height 0");
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogError("rectTransform null");
                                    }
                                }
                            }
                            break;
                        case State.Type.Normal:
                            {
                                UIRectTransform.SetPosY((RectTransform)this.transform, 0);
                            }
                            break;
                        case State.Type.Hide:
                            {
                                Hide hide = state as Hide;
                                if (hide.time.v <= hide.duration.v)
                                {
                                    if (Time.time == LastTime)
                                    {
                                        // Debug.LogError("the same last time: " + LastTime);
                                        dirty = true;
                                    }
                                    else
                                    {
                                        hide.time.v += Time.fixedDeltaTime;
                                    }
                                }
                                else
                                {
                                    // chuyen sang normal
                                    Normal normal = new Normal();
                                    {
                                        normal.uid = this.data.state.makeId();
                                    }
                                    this.data.state.v = normal;
                                    // event
                                    if (this.data.onHide.v != null)
                                    {
                                        this.data.onHide.v();
                                    }
                                    else
                                    {
                                        Logger.LogError("onHide null");
                                    }
                                }
                                // UI
                                {
                                    RectTransform rectTransform = (RectTransform)this.transform;
                                    if (rectTransform != null)
                                    {
                                        if (rectTransform.rect.height > 0)
                                        {
                                            if (hide.duration.v > 0)
                                            {
                                                UIRectTransform.SetPosY(rectTransform, Mathf.Clamp(
                                                    (hide.time.v / hide.duration.v) * (-rectTransform.rect.height),
                                                     -rectTransform.rect.height, 0));
                                            }
                                            else
                                            {
                                                Logger.LogError("show duration < 0: " + hide.duration.v);
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("rectTransform rect height 0");
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogError("rectTransform null");
                                    }
                                }
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + state.getType());
                            break;
                    }
                    LastTime = Time.time;
                }
                else
                {
                    Logger.LogError("state null");
                    Normal normal = new Normal();
                    {
                        normal.uid = this.data.state.makeId();
                    }
                    this.data.state.v = normal;
                }
            }
            else
            {
                // Debug.LogError("data null");
            }
        }
    }

    public override bool isShouldDisableUpdate()
    {
        return true;
    }

    #endregion

    #region implement callBacks

    public override void onAddCallBack<T>(T data)
    {
        if(data is UIData)
        {
            UIData uiData = data as UIData;
            // Child
            {
                uiData.state.allAddCallBack(this);
            }
            dirty = true;
            return;
        }
        // Child
        if(data is State)
        {
            dirty = true;
            return;
        }
        Logger.LogError("Don't process: " + data + "; " + this);
    }

    public override void onRemoveCallBack<T>(T data, bool isHide)
    {
        if (data is UIData)
        {
            UIData uiData = data as UIData;
            // Child
            {
                uiData.state.allRemoveCallBack(this);
            }
            this.setDataNull(uiData);
            return;
        }
        // Child
        if (data is State)
        {
            return;
        }
        Logger.LogError("Don't process: " + data + "; " + this);
    }

    public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
    {
        if (WrapProperty.checkError(wrapProperty))
        {
            return;
        }
        if (wrapProperty.p is UIData)
        {
            switch ((UIData.Property)wrapProperty.n)
            {
                case UIData.Property.state:
                    {
                        ValueChangeUtils.replaceCallBack(this, syncs);
                        dirty = true;
                    }
                    break;
                default:
                    Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                    break;
            }
            return;
        }
        // Child
        if (wrapProperty.p is State)
        {
            dirty = true;
            return;
        }
        Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
    }

    #endregion

    private void setHidePosition()
    {
        UIRectTransform.SetPosY((RectTransform)this.transform, float.MinValue);
    }

}