using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using UnityEngine;

namespace BattleRushS.StateS
{
    class PlayUpdate : UpdateBehavior<Play>
    {

        #region update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // check game end
                    {
                        BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            Hero hero = battleRush.hero.v;
                            if (hero != null)
                            {
                                if (hero.heroMove.v.sub.v != null)
                                {
                                    switch (hero.heroMove.v.sub.v.getType())
                                    {
                                        case HeroS.HeroMove.Sub.Type.Run:
                                            {
                                                if (hero.hitPoint.v <= 0)
                                                {
                                                    // hit point 0, hero die, game change to end
                                                    End end = battleRush.state.newOrOld<End>();
                                                    {

                                                    }
                                                    battleRush.state.v = end;
                                                }
                                            }
                                            break;
                                        case HeroS.HeroMove.Sub.Type.Arena:
                                            break;
                                        default:
                                            Logger.LogError("unknown type: " + hero.heroMove.v.sub.v.getType());
                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("heroMove null");
                                }
                            }
                            else
                            {
                                Logger.LogError("hero null");
                            }
                        }
                        else
                        {
                            Logger.LogError("battleRush null");
                        }
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

        private BattleRush battleRush = null;

        public override void onAddCallBack<T>(T data)
        {
            if (data is Play)
            {
                Play play = data as Play;
                // Parent
                {
                    DataUtils.addParentCallBack(play, this, ref this.battleRush);
                }
                dirty = true;
                return;
            }
            //  Parent
            {
                if(data is BattleRush)
                {
                    BattleRush battleRush = data as BattleRush;
                    // Child
                    {
                        battleRush.hero.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                {
                    if(data is Hero)
                    {
                        Hero hero = data as Hero;
                        // Child
                        {
                            hero.heroMove.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    if(data is HeroMove)
                    {
                        dirty = true;
                        return;
                    }
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is Play)
            {
                Play play = data as Play;
                // Parent
                {
                    DataUtils.removeParentCallBack(play, this, ref this.battleRush);
                }
                this.setDataNull(play);
                return;
            }
            //  Parent
            {
                if (data is BattleRush)
                {
                    BattleRush battleRush = data as BattleRush;
                    // Child
                    {
                        battleRush.hero.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                {
                    if (data is Hero)
                    {
                        Hero hero = data as Hero;
                        // Child
                        {
                            hero.heroMove.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Child
                    if (data is HeroMove)
                    {
                        return;
                    }
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            if (wrapProperty.p is Play)
            {
                switch ((Play.Property)wrapProperty.n)
                {
                    case Play.Property.state:
                        dirty = true;
                        break;
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            //  Parent
            {
                if (wrapProperty.p is BattleRush)
                {
                    switch ((BattleRush.Property)wrapProperty.n)
                    {
                        case BattleRush.Property.hero:
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
                {
                    if (wrapProperty.p is Hero)
                    {
                        switch ((Hero.Property)wrapProperty.n)
                        {
                            case Hero.Property.hitPoint:
                                dirty = true;
                                break;
                            case Hero.Property.heroMove:
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
                    if (wrapProperty.p is HeroMove)
                    {
                        switch ((HeroMove.Property)wrapProperty.n)
                        {
                            case HeroMove.Property.sub:
                                dirty = true;
                                break;
                            default:
                                break;
                        }
                        return;
                    }
                }
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
