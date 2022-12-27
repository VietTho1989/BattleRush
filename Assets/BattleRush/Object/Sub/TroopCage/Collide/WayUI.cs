using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using BattleRushS.HeroS.RunS;
using BattleRushS.StateS;
using UnityEngine;

namespace BattleRushS.ObjectS.TroopCageS
{
    public class WayUI : UIBehavior<WayUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public LO<Collider> colliders;

            #region Constructor

            public enum Property
            {
                colliders
            }

            public UIData() : base()
            {
                this.colliders = new LO<Collider>(this, (byte)Property.colliders);
            }

            #endregion

        }

        #endregion

        #region Refresh

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    TroopCageUI.UIData troopCageUIData = this.data.findDataInParent<TroopCageUI.UIData>();
                    if (troopCageUIData != null)
                    {
                        TroopCage troopCage = troopCageUIData.troopCage.v.data;
                        if (troopCage != null)
                        {
                            // find state allow shoot
                            bool stateAllowShoot = false;
                            {
                                BattleRush battleRush = troopCage.findDataInParent<BattleRush>();
                                if (battleRush != null)
                                {
                                    switch (battleRush.state.v.getType())
                                    {
                                        case BattleRush.State.Type.Load:
                                            break;
                                        case BattleRush.State.Type.Start:
                                            break;
                                        case BattleRush.State.Type.Play:
                                            {
                                                Play play = battleRush.state.v as Play;
                                                switch (play.state.v)
                                                {
                                                    case Play.State.Normal:
                                                        {
                                                            // check hero is running
                                                            if(battleRush.hero.v.heroMove.v.sub.v!=null && battleRush.hero.v.heroMove.v.sub.v.getType() == HeroMove.Sub.Type.Run)
                                                            {
                                                                stateAllowShoot = true;
                                                            }
                                                        }
                                                        break;
                                                    case Play.State.Pause:
                                                        break;
                                                    default:
                                                        Logger.LogError("unknown state: "+play.state.v);
                                                        break;
                                                }
                                            }
                                            break;
                                        case BattleRush.State.Type.End:
                                            break;
                                        default:
                                            Logger.LogError("unknown state: " + battleRush.state.v.getType());
                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("battleRush null");
                                }
                            }
                            // process
                            if (stateAllowShoot)
                            {
                                switch (troopCage.state.v.getType())
                                {
                                    case TroopCage.State.Type.Live:
                                        {
                                            Live live = troopCage.state.v as Live;
                                            if (live.hitpoint.v - live.projectiles.vs.Count > 0)
                                            {
                                                foreach (Collider collider in this.data.colliders.vs)
                                                {
                                                    HeroUI heroUI = collider.GetComponent<HeroUI>();
                                                    if (heroUI != null)
                                                    {
                                                        HeroUI.UIData heroUIData = heroUI.data;
                                                        if (heroUIData != null)
                                                        {
                                                            Hero hero = heroUIData.hero.v.data;
                                                            if (hero != null)
                                                            {
                                                                if (hero.heroMove.v.sub.v != null)
                                                                {
                                                                    switch (hero.heroMove.v.sub.v.getType())
                                                                    {
                                                                        case HeroS.HeroMove.Sub.Type.Run:
                                                                            {
                                                                                HeroMoveRun heroMoveRun = hero.heroMove.v.sub.v as HeroMoveRun;
                                                                                // make shoot project tile
                                                                                switch (heroMoveRun.heroRunShoot.v.state.v.getType())
                                                                                {
                                                                                    case HeroS.RunS.HeroRunShoot.State.Type.Normal:
                                                                                        {
                                                                                            TroopCageShootProjectile troopCageShootProjectile = new TroopCageShootProjectile();
                                                                                            {
                                                                                                troopCageShootProjectile.uid = live.projectiles.makeId();
                                                                                                troopCageShootProjectile.startPosition.v = heroUI.transform.position;
                                                                                                // duration
                                                                                                {
                                                                                                    TroopCageShootProjectile.State.Move move = troopCageShootProjectile.state.newOrOld<TroopCageShootProjectile.State.Move>();
                                                                                                    {
                                                                                                        TroopCageUI troopCageUI = troopCageUIData.findCallBack<TroopCageUI>();
                                                                                                        if (troopCageUI != null)
                                                                                                        {
                                                                                                            float distance = Vector3.Distance(heroUI.transform.position, troopCageUI.transform.position);
                                                                                                            Logger.Log("distance to troop cage: " + distance);
                                                                                                            move.duration.v = distance / 20;
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            Logger.LogError("troopCageUI null");
                                                                                                        }
                                                                                                    }
                                                                                                    troopCageShootProjectile.state.v = move;
                                                                                                }
                                                                                            }
                                                                                            live.projectiles.add(troopCageShootProjectile);
                                                                                            // change hero to cooldown
                                                                                            {
                                                                                                HeroRunShoot.State.CoolDown coolDown = heroMoveRun.heroRunShoot.v.state.newOrOld<HeroRunShoot.State.CoolDown>();
                                                                                                {

                                                                                                }
                                                                                                heroMoveRun.heroRunShoot.v.state.v = coolDown;
                                                                                            }
                                                                                        }
                                                                                        break;
                                                                                    case HeroS.RunS.HeroRunShoot.State.Type.CoolDown:
                                                                                        {
                                                                                            // hero in cooldown, cannot shoot
                                                                                        }
                                                                                        break;
                                                                                }
                                                                            }
                                                                            break;
                                                                        case HeroS.HeroMove.Sub.Type.Arena:
                                                                            break;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Logger.LogError("sub null");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("hero null");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Logger.LogError("heroUIData null");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Logger.LogError("heroUI null");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Logger.Log("already shoot enough");
                                            }
                                        }
                                        break;
                                    case TroopCage.State.Type.ReleaseTroop:
                                        break;
                                    case TroopCage.State.Type.Destroyed:
                                        break;
                                }
                            }
                            else
                            {
                                Logger.Log("stateAllowShoot");
                            }
                        }
                        else
                        {
                            Logger.LogError("troopCage null");
                        }                       
                    }
                    else
                    {
                        Logger.LogError("troopCageUIData null");
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

        private TroopCageUI.UIData troopCageUIData = null;
        private BattleRush battleRush = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Parent
                {
                    DataUtils.addParentCallBack(uiData, this, ref this.troopCageUIData);
                }
                dirty = true;
                return;
            }
            // Parent
            {
                if(data is TroopCageUI.UIData)
                {
                    TroopCageUI.UIData troopCageUIData = data as TroopCageUI.UIData;
                    // Child
                    {
                        troopCageUIData.troopCage.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                {
                    if(data is TroopCage)
                    {
                        TroopCage troopCage = data as TroopCage;
                        // Parent
                        {
                            DataUtils.addParentCallBack(troopCage, this, ref this.battleRush);
                        }
                        // Child
                        {
                            troopCage.state.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Parent
                    {
                        if (data is BattleRush)
                        {
                            BattleRush battleRush = data as BattleRush;
                            // Child
                            {
                                battleRush.hero.allAddCallBack(this);
                                battleRush.state.allAddCallBack(this);
                            }
                            dirty = true;
                            return;
                        }
                        // Child
                        {
                            // Hero
                            {
                                if (data is Hero)
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
                                {
                                    if (data is HeroMove)
                                    {
                                        HeroMove heroMove = data as HeroMove;
                                        // Child
                                        {
                                            heroMove.sub.allAddCallBack(this);
                                        }
                                        dirty = true;
                                        return;
                                    }
                                    // Child
                                    {
                                        if (data is HeroMove.Sub)
                                        {
                                            HeroMove.Sub sub = data as HeroMove.Sub;
                                            // Child
                                            {
                                                switch (sub.getType())
                                                {
                                                    case HeroMove.Sub.Type.Run:
                                                        {
                                                            HeroMoveRun heroMoveRun = sub as HeroMoveRun;
                                                            heroMoveRun.heroRunShoot.allAddCallBack(this);
                                                        }
                                                        break;
                                                    case HeroMove.Sub.Type.Arena:
                                                        break;
                                                }
                                            }
                                            dirty = true;
                                            return;
                                        }
                                        // Child
                                        if (data is HeroRunShoot)
                                        {
                                            dirty = true;
                                            return;
                                        }
                                    }
                                }
                            }
                            // state
                            if(data is BattleRush.State)
                            {
                                dirty = true;
                                return;
                            }
                        }
                    }                    
                    // Child
                    if(data is TroopCage.State)
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
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Parent
                {
                    DataUtils.removeParentCallBack(uiData, this, ref this.troopCageUIData);
                }
                this.setDataNull(uiData);
                return;
            }
            // Parent
            {
                if (data is TroopCageUI.UIData)
                {
                    TroopCageUI.UIData troopCageUIData = data as TroopCageUI.UIData;
                    // Child
                    {
                        troopCageUIData.troopCage.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                {
                    if (data is TroopCage)
                    {
                        TroopCage troopCage = data as TroopCage;
                        // Parent
                        {
                            DataUtils.removeParentCallBack(troopCage, this, ref this.battleRush);
                        }
                        // Child
                        {
                            troopCage.state.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Parent
                    {
                        if (data is BattleRush)
                        {
                            BattleRush battleRush = data as BattleRush;
                            // Child
                            {
                                battleRush.hero.allRemoveCallBack(this);
                                battleRush.state.allRemoveCallBack(this);
                            }
                            return;
                        }
                        // Child
                        {
                            // hero
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
                                {
                                    if (data is HeroMove)
                                    {
                                        HeroMove heroMove = data as HeroMove;
                                        // Child
                                        {
                                            heroMove.sub.allRemoveCallBack(this);
                                        }
                                        return;
                                    }
                                    // Child
                                    {
                                        if (data is HeroMove.Sub)
                                        {
                                            HeroMove.Sub sub = data as HeroMove.Sub;
                                            // Child
                                            {
                                                switch (sub.getType())
                                                {
                                                    case HeroMove.Sub.Type.Run:
                                                        {
                                                            HeroMoveRun heroMoveRun = sub as HeroMoveRun;
                                                            heroMoveRun.heroRunShoot.allRemoveCallBack(this);
                                                        }
                                                        break;
                                                    case HeroMove.Sub.Type.Arena:
                                                        break;
                                                }
                                            }
                                            return;
                                        }
                                        // Child
                                        if (data is HeroRunShoot)
                                        {
                                            return;
                                        }
                                    }
                                }
                            }
                            // state
                            if(data is BattleRush.State)
                            {
                                return;
                            }
                        }
                    }
                    // Child
                    if (data is TroopCage.State)
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
            if (wrapProperty.p is UIData)
            {
                switch ((UIData.Property)wrapProperty.n)
                {
                    case UIData.Property.colliders:
                        dirty = true;
                        break;
                    default:
                        break;
                }
                return;
            }
            // Parent
            {
                if (wrapProperty.p is TroopCageUI.UIData)
                {
                    switch ((TroopCageUI.UIData.Property)wrapProperty.n)
                    {
                        case TroopCageUI.UIData.Property.troopCage:
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
                    if (wrapProperty.p is TroopCage)
                    {
                        switch ((TroopCage.Property)wrapProperty.n)
                        {
                            case TroopCage.Property.state:
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
                    // Parent
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
                                case BattleRush.Property.state:
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
                            // Hero
                            {
                                if (wrapProperty.p is Hero)
                                {
                                    switch ((Hero.Property)wrapProperty.n)
                                    {
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
                                {
                                    if (wrapProperty.p is HeroMove)
                                    {
                                        switch ((HeroMove.Property)wrapProperty.n)
                                        {
                                            case HeroMove.Property.sub:
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
                                        if (wrapProperty.p is HeroMove.Sub)
                                        {
                                            HeroMove.Sub sub = wrapProperty.p as HeroMove.Sub;
                                            // Child
                                            {
                                                switch (sub.getType())
                                                {
                                                    case HeroMove.Sub.Type.Run:
                                                        {
                                                            switch ((HeroMoveRun.Property)wrapProperty.n)
                                                            {
                                                                case HeroMoveRun.Property.heroRunShoot:
                                                                    {
                                                                        ValueChangeUtils.replaceCallBack(this, syncs);
                                                                        dirty = true;
                                                                    }
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case HeroMove.Sub.Type.Arena:
                                                        break;
                                                }
                                            }
                                            return;
                                        }
                                        // Child
                                        if (wrapProperty.p is HeroRunShoot)
                                        {
                                            switch ((HeroRunShoot.Property)wrapProperty.n)
                                            {
                                                case HeroRunShoot.Property.state:
                                                    dirty = true;
                                                    break;
                                                default:
                                                    break;
                                            }
                                            return;
                                        }
                                    }
                                }
                            }
                            // state
                            if(wrapProperty.p is BattleRush.State)
                            {
                                BattleRush.State state = wrapProperty.p as BattleRush.State;
                                switch (state.getType())
                                {
                                    case BattleRush.State.Type.Load:
                                        break;
                                    case BattleRush.State.Type.Start:
                                        break;
                                    case BattleRush.State.Type.Play:
                                        {
                                            switch ((Play.Property)wrapProperty.n)
                                            {
                                                case Play.Property.state:
                                                    dirty = true;
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        break;
                                    case BattleRush.State.Type.End:
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + state.getType());
                                        break;
                                }
                                return;
                            }
                        }
                    }
                    // Child
                    if (wrapProperty.p is TroopCage.State)
                    {
                        TroopCage.State state = wrapProperty.p as TroopCage.State;
                        switch (state.getType())
                        {
                            case TroopCage.State.Type.Live:
                                {
                                    switch ((Live.Property)wrapProperty.n)
                                    {
                                        case Live.Property.hitpoint:
                                            dirty = true;
                                            break;
                                        case Live.Property.projectiles:
                                            dirty = true;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case TroopCage.State.Type.ReleaseTroop:
                                break;
                            case TroopCage.State.Type.Destroyed:
                                break;
                        }
                        return;
                    }
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

        private void OnTriggerEnter(Collider collider)
        {
            Logger.Log("TroopCageUI: WayUI: onTriggerEnter: " + collider + ", " + this.gameObject);
            if (this.data != null)
            {
                // chi lay hero
                bool isHero = collider.GetComponent<HeroUI>() !=null;
                // add
                if (isHero)
                {
                    Logger.Log("add hero collider");
                    this.data.colliders.add(collider);
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            Logger.Log("TroopCageUI: WayUI: onTriggerEnter: " + collider + ", " + this.gameObject);
            if (this.data != null)
            {
                this.data.colliders.remove(collider);
            }
            else
            {
                Logger.LogError("data null");
            }
        }

    }
}
