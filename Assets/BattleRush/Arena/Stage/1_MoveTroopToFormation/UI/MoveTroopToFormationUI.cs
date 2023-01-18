using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.StateS
{
    public class MoveTroopToFormationUI : UIBehavior<MoveTroopToFormationUI.UIData>
    {

        #region UIData

        public class UIData : ArenaUI.UIData.StageUI
        {

            public VO<ReferenceData<MoveTroopToFormation>> moveTroopToFormation;

            #region Constructor

            public enum Property
            {
                moveTroopToFormation
            }

            public UIData() : base()
            {
                this.moveTroopToFormation = new VO<ReferenceData<MoveTroopToFormation>>(this, (byte)Property.moveTroopToFormation, new ReferenceData<MoveTroopToFormation>(null));
            }

            #endregion

            public override Arena.Stage.Type getType()
            {
                return Arena.Stage.Type.MoveTroopToFormation;
            }

        }

        #endregion

        #region Refresh

        public GameObject btnFight;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    MoveTroopToFormation moveTroopToFormation = this.data.moveTroopToFormation.v.data;
                    if (moveTroopToFormation != null)
                    {
                        // btnFight
                        if (btnFight != null)
                        {
                            switch (moveTroopToFormation.state.v)
                            {
                                case MoveTroopToFormation.State.Start:
                                case MoveTroopToFormation.State.Move:
                                case MoveTroopToFormation.State.Came:
                                    btnFight.SetActive(false);
                                    break;
                                case MoveTroopToFormation.State.Ready:
                                    btnFight.SetActive(true);
                                    break;
                                default:
                                    Logger.LogError("unknown state: " + moveTroopToFormation.state.v);
                                    break;
                            }
                        }
                        else
                        {
                            Logger.LogError("btnFight null");
                        }
                    }
                    else
                    {
                        Logger.LogError("moveTroopToFormation null");
                    }                    
                }
                else
                {
                    Logger.LogError("data null");
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
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.moveTroopToFormation.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if (data is MoveTroopToFormation)
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
                    uiData.moveTroopToFormation.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            if (data is MoveTroopToFormation)
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
                    case UIData.Property.moveTroopToFormation:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    default:
                        break;
                }
                return;
            }
            // Child
            if (wrapProperty.p is MoveTroopToFormation)
            {
                switch ((MoveTroopToFormation.Property)wrapProperty.n)
                {
                    case MoveTroopToFormation.Property.state:
                        dirty = true;
                        break;
                    default:
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

        public void onClickBtnFight()
        {
            if (this.data != null)
            {
                MoveTroopToFormation moveTroopToFormation = this.data.moveTroopToFormation.v.data;
                if (moveTroopToFormation != null)
                {
                    // change to auto fight
                    Arena arena = moveTroopToFormation.findDataInParent<Arena>();
                    if (arena != null)
                    {
                        AutoFight autoFight = arena.stage.newOrOld<AutoFight>();
                        {

                        }
                        arena.stage.v = autoFight;
                    }
                    else
                    {
                        Logger.LogError("arena null");
                    }
                }
                else
                {
                    Logger.LogError("moveTroopToFormation null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

    }
}