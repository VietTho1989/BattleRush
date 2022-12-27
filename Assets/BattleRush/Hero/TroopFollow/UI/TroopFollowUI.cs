using System.Collections;
using System.Collections.Generic;
using BattleRushS.ObjectS;
using BattleRushS.StateS;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class TroopFollowUI : UIBehavior<TroopFollowUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public VO<ReferenceData<TroopFollow>> troopFollow;

            #region Constructor

            public enum Property
            {
                troopFollow
            }

            public UIData() : base()
            {
                this.troopFollow = new VO<ReferenceData<TroopFollow>>(this, (byte)Property.troopFollow, new ReferenceData<TroopFollow>(null));
            }

            #endregion

        }

        #endregion

        #region Refresh

        private TroopInformation currentTroopTypeModel;
        public List<TroopInformation> troopPrefabs;

        public Follow follow;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    TroopFollow troopFollow = this.data.troopFollow.v.data;
                    if (troopFollow != null)
                    {
                        // troopTypeModel
                        {
                            // find
                            bool isNeedMakeNew = true;
                            {
                                if (currentTroopTypeModel != null)
                                {
                                    if (currentTroopTypeModel.troopType == troopFollow.troopType.v)
                                    {
                                        isNeedMakeNew = false;
                                    }
                                }
                            }
                            // make new
                            if (isNeedMakeNew)
                            {
                                // find prefab
                                TroopInformation prefab = null;
                                {
                                    foreach(TroopInformation check in troopPrefabs)
                                    {
                                        if (check != null)
                                        {
                                            if (check.troopType == troopFollow.troopType.v)
                                            {
                                                prefab = check;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("why prefab check null");
                                        }
                                    }
                                }
                                // process
                                if (prefab != null)
                                {
                                    currentTroopTypeModel = Instantiate(prefab, this.transform);
                                    currentTroopTypeModel.transform.localPosition = Vector3.zero;
                                }
                                else
                                {
                                    Logger.LogError("prefab null");
                                }
                            }
                        }
                        const int MaxNumberPerRow = 3;
                        // in cage
                        {
                            TroopCage troopCage = troopFollow.findDataInParent<TroopCage>();
                            if (troopCage != null)
                            {
                                // position
                                {
                                    // disable follow hero
                                    {
                                        if (follow != null)
                                        {
                                            follow.enabled = false;
                                        }
                                        else
                                        {
                                            Logger.LogError("follow null");
                                        }
                                    }
                                    int positionIndex = troopCage.troops.vs.IndexOf(troopFollow);
                                    if (positionIndex >= 0 && positionIndex < troopCage.troops.vs.Count)
                                    {
                                        TroopCageUI troopCageUI = troopCage.findCallBack<TroopCageUI>();
                                        if (troopCageUI != null)
                                        {
                                            if (troopCageUI.troopPoint != null)
                                            {
                                                int troopCountInRow = Mathf.Min(troopCage.troops.vs.Count, MaxNumberPerRow);
                                                int nummberOfRow = troopCage.troops.vs.Count / MaxNumberPerRow;
                                                int row = positionIndex / troopCountInRow;
                                                int col = positionIndex % troopCountInRow;
                                                // x
                                                float x = 0;
                                                {
                                                    const float DistanceBetweenRow = 1.25f;
                                                    x += ((col + 1) / 2.0f - troopCountInRow / 2.0f) * DistanceBetweenRow;
                                                    Logger.Log("troopCage: ");
                                                }
                                                // z
                                                float z = 0;
                                                {
                                                    const float DistanceBetweenCol = 2.0f;
                                                    z += ((row + 1) / 2.0f - nummberOfRow / 2.0f) * DistanceBetweenCol;
                                                }
                                                this.transform.localPosition = new Vector3(x, 0, z);
                                            }
                                            else
                                            {
                                                Logger.LogError("troopPoint null");
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("troopCageUI null");
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogError("positionIndex: " + positionIndex);
                                    }
                                }
                                // animation
                                if(currentTroopTypeModel!=null && currentTroopTypeModel.troopAnimator != null)
                                {
                                    switch (troopCage.state.v.getType())
                                    {
                                        case TroopCage.State.Type.Live:
                                            {
                                                currentTroopTypeModel.gameObject.SetActive(true);
                                                if (currentTroopTypeModel.animationIdle != null)
                                                {
                                                    currentTroopTypeModel.troopAnimator.Play("Idle");// (currentTroopTypeModel.animationIdle.name);
                                                }
                                                else
                                                {
                                                    Logger.LogError("animationIdle null");
                                                }
                                            }
                                            break;
                                        case TroopCage.State.Type.ReleaseTroop:
                                            {
                                                currentTroopTypeModel.gameObject.SetActive(false);
                                            }
                                            break;
                                        case TroopCage.State.Type.Destroyed:
                                            {
                                                currentTroopTypeModel.gameObject.SetActive(false);
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("currentTroopTypeModel null");
                                }                                
                            }
                        }
                        // follow hero
                        {
                            Hero hero = troopFollow.findDataInParent<Hero>();
                            if (hero != null)
                            {
                                // position
                                {
                                    // find offset compare to hero
                                    Vector3 offset = Vector3.zero;
                                    {
                                        int positionIndex = hero.troopFollows.vs.IndexOf(troopFollow);
                                        if (positionIndex >= 0 && positionIndex < hero.troopFollows.vs.Count)
                                        {
                                            HeroUI heroUI = hero.findCallBack<HeroUI>();
                                            if (heroUI != null)
                                            {
                                                if (heroUI.troopPoint != null)
                                                {
                                                    int troopCountInRow = Mathf.Min(hero.troopFollows.vs.Count, MaxNumberPerRow);
                                                    int nummberOfRow = hero.troopFollows.vs.Count / MaxNumberPerRow;
                                                    int row = positionIndex / troopCountInRow;
                                                    int col = positionIndex % troopCountInRow;
                                                    // x
                                                    float x = 0;
                                                    {
                                                        const float DistanceBetweenRow = 1.5f;
                                                        x += ((col + 1) / 2.0f - troopCountInRow / 2.0f) * DistanceBetweenRow;
                                                        Logger.Log("troopCage: ");
                                                    }
                                                    // z
                                                    float z = 0;
                                                    {
                                                        const float DistanceBetweenCol = 1.0f;
                                                        z -= row * DistanceBetweenCol;
                                                    }
                                                    offset = new Vector3(x, 0, z);
                                                }
                                                else
                                                {
                                                    Logger.LogError("troopPoint null");
                                                }
                                            }
                                            else
                                            {
                                                Logger.LogError("heroUI null");
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("postionIndex error: " + positionIndex + ", " + hero.troopFollows.vs.Count);
                                        }
                                    }
                                    // process
                                    if (offset != Vector3.zero)
                                    {
                                        if (follow != null)
                                        {
                                            follow.enabled = true;
                                            follow.target = this.transform.parent;
                                            follow.XOffset = offset.x;
                                            follow.YOffset = offset.y;
                                            follow.ZOffset = offset.z;
                                        }
                                        else
                                        {
                                            Logger.LogError("follow null");
                                        }
                                    }
                                }
                                // animation
                                {
                                    if(currentTroopTypeModel!=null && currentTroopTypeModel.troopAnimator != null)
                                    {
                                        // find run or pause
                                        bool isRun = true;
                                        {
                                            BattleRush battleRush = troopFollow.findDataInParent<BattleRush>();
                                            if (battleRush != null)
                                            {
                                                switch (battleRush.state.v.getType())
                                                {
                                                    case BattleRush.State.Type.Load:
                                                        isRun = false;
                                                        break;
                                                    case BattleRush.State.Type.Start:
                                                        isRun = false;
                                                        break;
                                                    case BattleRush.State.Type.Play:
                                                        {
                                                            Play play = battleRush.state.v as Play;
                                                            switch (play.state.v)
                                                            {
                                                                case Play.State.Normal:
                                                                    {
                                                                        HeroMove.Sub sub = battleRush.hero.v.heroMove.v.sub.v;
                                                                        if (sub != null)
                                                                        {
                                                                            switch (sub.getType())
                                                                            {
                                                                                case HeroMove.Sub.Type.Run:
                                                                                    isRun = true;
                                                                                    break;
                                                                                case HeroMove.Sub.Type.Arena:
                                                                                    isRun = false;
                                                                                    break;
                                                                                default:
                                                                                    Logger.LogError("unknown type: " + sub.getType());
                                                                                    break;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            Logger.LogError("sub null");
                                                                        }                                                                        
                                                                    }                                                                    
                                                                    break;
                                                                case Play.State.Pause:
                                                                    isRun = false;
                                                                    break;
                                                                default:
                                                                    Logger.LogError("unknown state: " + play.state.v);
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case BattleRush.State.Type.End:
                                                        isRun = false;
                                                        break;
                                                    default:
                                                        Logger.LogError("unknown type: "+battleRush.state.v.getType());
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                Logger.Log("battleRush null");
                                            }
                                        }
                                        // process
                                        {
                                            if (isRun)
                                            {
                                                if (currentTroopTypeModel.animationMove != null)
                                                {
                                                    Logger.Log("TroopFollow animation move: " + currentTroopTypeModel.animationMove.isLooping + ", " + currentTroopTypeModel.animationMove.name);
                                                    currentTroopTypeModel.troopAnimator.Play("Move");// (currentTroopTypeModel.animationMove.name);                                                    
                                                }
                                                else
                                                {
                                                    Logger.LogError("animationMove null");
                                                }
                                            }
                                            else
                                            {
                                                if (currentTroopTypeModel.animationIdle != null)
                                                {
                                                    Logger.Log("TroopFollow animation idle");
                                                    currentTroopTypeModel.troopAnimator.Play("Idle");// (currentTroopTypeModel.animationIdle.name);
                                                }
                                                else
                                                {
                                                    Logger.LogError("animationIdle null");
                                                }
                                            }
                                        }                                       
                                    }
                                    else
                                    {
                                        Logger.LogError("animator null");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("troopFollow null");
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

        private Hero hero = null;
        private TroopCage troopCage = null;

        private BattleRush battleRush = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.troopFollow.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is TroopFollow)
                {
                    TroopFollow troopFollow = data as TroopFollow;
                    // Parent
                    {
                        DataUtils.addParentCallBack(troopFollow, this, ref this.hero);
                        DataUtils.addParentCallBack(troopFollow, this, ref this.troopCage);
                        DataUtils.addParentCallBack(troopFollow, this, ref this.battleRush);
                    }
                    dirty = true;
                    return;
                }
                // Parent
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
                        if(data is HeroMove)
                        {
                            dirty = true;
                            return;
                        }
                    }                    
                    if(data is TroopCage)
                    {
                        dirty = true;
                        return;
                    }
                    // battleRush
                    {
                        if(data is BattleRush)
                        {
                            BattleRush battleRush = data as BattleRush;
                            // Child
                            {
                                battleRush.state.allAddCallBack(this);
                            }
                            dirty = true;
                            return;
                        }
                        // Child
                        if(data is BattleRush.State)
                        {
                            dirty = true;
                            return;
                        }
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
                // Child
                {
                    uiData.troopFollow.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is TroopFollow)
                {
                    TroopFollow troopFollow = data as TroopFollow;
                    // Parent
                    {
                        DataUtils.removeParentCallBack(troopFollow, this, ref this.hero);
                        DataUtils.removeParentCallBack(troopFollow, this, ref this.troopCage);
                        DataUtils.removeParentCallBack(troopFollow, this, ref this.battleRush);
                    }
                    return;
                }
                // Parent
                {
                    // Hero
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
                        if(data is HeroMove)
                        {
                            return;
                        }
                    }                    
                    if (data is TroopCage)
                    {
                        return;
                    }
                    // battleRush
                    {
                        if (data is BattleRush)
                        {
                            BattleRush battleRush = data as BattleRush;
                            // Child
                            {
                                battleRush.state.allRemoveCallBack(this);
                            }
                            return;
                        }
                        // Child
                        if (data is BattleRush.State)
                        {
                            return;
                        }
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
                    case UIData.Property.troopFollow:
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
                if (wrapProperty.p is TroopFollow)
                {
                    switch ((TroopFollow.Property)wrapProperty.n)
                    {
                        case TroopFollow.Property.hitPoint:
                            dirty = true;
                            break;
                        case TroopFollow.Property.troopType:
                            dirty = true;
                            break;
                        default:
                            break;
                    }
                    return;
                }
                // Parent
                {
                    // Hero
                    {
                        if (wrapProperty.p is Hero)
                        {
                            switch ((Hero.Property)wrapProperty.n)
                            {
                                case Hero.Property.troopFollows:
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
                        if(wrapProperty.p is HeroMove)
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
                    if (wrapProperty.p is TroopCage)
                    {
                        switch ((TroopCage.Property)wrapProperty.n)
                        {
                            case TroopCage.Property.state:
                                dirty = true;
                                break;
                            case TroopCage.Property.troops:
                                dirty = true;
                                break;
                            default:
                                break;
                        }
                        return;
                    }
                    // battleRush
                    {
                        if (wrapProperty.p is BattleRush)
                        {
                            switch ((BattleRush.Property)wrapProperty.n)
                            {
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
                        if (wrapProperty.p is BattleRush.State)
                        {
                            BattleRush.State state = wrapProperty.p as BattleRush.State;
                            switch (state.getType())
                            {
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
                                default:
                                    Logger.LogError("unknown type: " + state.getType());
                                    break;
                            }
                            return;
                        }
                    }
                }
            }            
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}