using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS.TroopAttackS;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS
{
    public class TroopAttackUpdate : UpdateBehavior<TroopAttack>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {

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
            if(data is TroopAttack)
            {
                TroopAttack troopAttack = data as TroopAttack;
                // Child
                {
                    troopAttack.state.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is TroopAttack.State)
            {
                TroopAttack.State state = data as TroopAttack.State;
                // Update
                {
                    switch (state.getType())
                    {
                        case TroopAttack.State.Type.Normal:
                            {
                                Normal normal = state as Normal;
                                UpdateUtils.makeUpdate<NormalUpdate, Normal>(normal, this.transform);
                            }
                            break;
                        case TroopAttack.State.Type.Animation:
                            {
                                Anim anim = state as Anim;
                                UpdateUtils.makeUpdate<AnimUpdate, Anim>(anim, this.transform);
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + state.getType());
                            break;
                    }
                }
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is TroopAttack)
            {
                TroopAttack troopAttack = data as TroopAttack;
                // Child
                {
                    troopAttack.state.allRemoveCallBack(this);
                }
                this.setDataNull(troopAttack);
                return;
            }
            // Child
            if (data is TroopAttack.State)
            {
                TroopAttack.State state = data as TroopAttack.State;
                // Update
                {
                    switch (state.getType())
                    {
                        case TroopAttack.State.Type.Normal:
                            {
                                Normal normal = state as Normal;
                                normal.removeCallBackAndDestroy(typeof(NormalUpdate));
                            }
                            break;
                        case TroopAttack.State.Type.Animation:
                            {
                                Anim anim = state as Anim;
                                anim.removeCallBackAndDestroy(typeof(AnimUpdate));
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + state.getType());
                            break;
                    }
                }
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
            if (wrapProperty.p is TroopAttack)
            {
                switch ((TroopAttack.Property)wrapProperty.n)
                {
                    case TroopAttack.Property.state:
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
            if (wrapProperty.p is TroopAttack.State)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}