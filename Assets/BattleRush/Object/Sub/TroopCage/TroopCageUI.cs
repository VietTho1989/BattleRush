using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using BattleRushS.ObjectS.TroopCageS;
using Dreamteck.Forever;
using UnityEngine;
using UnityEngine.UI;

namespace BattleRushS.ObjectS
{
    public class TroopCageUI : UIBehavior<TroopCageUI.UIData>, BattleRushUI.UIData.ObjectInPathUIInterface
    {

        public void setObjectInPathUIData(Data data)
        {
            if (data is UIData)
            {
                this.setData((UIData)data);
            }
        }

        public GameObject getMyGameObject()
        {
            return this.gameObject;
        }

        public ObjectInPath.Type getType()
        {
            return ObjectInPath.Type.TroopCage;
        }

        #region UIData

        public class UIData : BattleRushUI.UIData.ObjectInPathUI
        {

            public VO<ReferenceData<TroopCage>> troopCage;

            public VD<WayUI.UIData> way;

            public LD<TroopCageShootProjectileUI.UIData> projectiles;

            public VD<PrisonAnimationUI.UIData> prisonAnimation;

            public LD<TroopFollowUI.UIData> troops;

            public VD<ObstructionUI.UIData> obstruction;

            #region Constructor

            public enum Property
            {
                troopCage,
                way,
                projectiles,
                prisonAnimation,
                troops,
                obstruction
            }

            public UIData() : base()
            {
                this.troopCage = new VO<ReferenceData<TroopCage>>(this, (byte)Property.troopCage, new ReferenceData<TroopCage>(null));
                this.way = new VD<WayUI.UIData>(this, (byte)Property.way, new WayUI.UIData());
                this.projectiles = new LD<TroopCageShootProjectileUI.UIData>(this, (byte)Property.projectiles);
                this.prisonAnimation = new VD<PrisonAnimationUI.UIData>(this, (byte)Property.prisonAnimation, new PrisonAnimationUI.UIData());
                this.troops = new LD<TroopFollowUI.UIData>(this, (byte)Property.troops);
                this.obstruction = new VD<ObstructionUI.UIData>(this, (byte)Property.obstruction, new ObstructionUI.UIData());
            }

            #endregion

            public override ObjectInPath.Type getType()
            {
                return ObjectInPath.Type.TroopCage;
            }

            public override ObjectInPath getObjectInPath()
            {
                return this.troopCage.v.data;
            }

        }

        #endregion

        /**
         * position where troop in cage
         * */
        public Transform troopPoint;

        #region Refresh

        public Text tvHP;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    TroopCage troopCage = this.data.troopCage.v.data;
                    if (troopCage != null)
                    {
                        // obstruction
                        {
                            // need or not
                            {
                                // find
                                bool isNeed = true;
                                {
                                    switch (troopCage.state.v.getType())
                                    {
                                        case TroopCage.State.Type.Live:
                                            break;
                                        case TroopCage.State.Type.ReleaseTroop:
                                            isNeed = false;
                                            break;
                                        case TroopCage.State.Type.Destroyed:
                                            isNeed = false;
                                            break;
                                    }
                                }
                                // process
                                if (isNeed)
                                {
                                    ObstructionUI.UIData obstructionUIData = this.data.obstruction.newOrOld<ObstructionUI.UIData>();
                                    {

                                    }
                                    this.data.obstruction.v = obstructionUIData;
                                }
                                else
                                {
                                    this.data.obstruction.v = null;
                                }
                            }
                            // update
                            {
                                ObstructionUI.UIData obstructionUIData = this.data.obstruction.v;
                                if (obstructionUIData != null)
                                {
                                    obstructionUIData.obstruction.v = new ReferenceData<Obstruction>(troopCage.obstruction.v);
                                }
                                else
                                {
                                    Logger.LogError("obstructionUIData null");
                                }
                            }                            
                        }
                        // tvHP
                        {
                            if (tvHP != null)
                            {
                                // find
                                int hp = 0;
                                {
                                    switch (troopCage.state.v.getType())
                                    {
                                        case TroopCage.State.Type.Live:
                                            {
                                                Live live = troopCage.state.v as Live;
                                                hp = live.hitpoint.v;
                                            }
                                            break;
                                        case TroopCage.State.Type.ReleaseTroop:
                                            hp = 0;
                                            break;
                                        case TroopCage.State.Type.Destroyed:
                                            hp = 0;
                                            break;
                                    }
                                }
                                // set
                                tvHP.text = hp.ToString();
                            }
                            else
                            {
                                Logger.LogError("tvHP null");
                            }
                        }
                        // projectiles
                        {
                            // find
                            List<TroopCageShootProjectile> projectiles = new List<TroopCageShootProjectile>();
                            {
                                switch (troopCage.state.v.getType())
                                {
                                    case TroopCage.State.Type.Live:
                                        {
                                            Live live = troopCage.state.v as Live;
                                            projectiles.AddRange(live.projectiles.vs);
                                        }
                                        break;
                                    case TroopCage.State.Type.ReleaseTroop:
                                        break;
                                    case TroopCage.State.Type.Destroyed:
                                        break;
                                }
                            }
                            // process
                            {
                                // get old
                                List<TroopCageShootProjectileUI.UIData> olds = new List<TroopCageShootProjectileUI.UIData>();
                                {
                                    olds.AddRange(this.data.projectiles.vs);
                                }
                                // Update
                                {
                                    foreach(TroopCageShootProjectile troopCageShootProjectile in projectiles)
                                    {
                                        // get UI
                                        TroopCageShootProjectileUI.UIData troopCageShootProjectileUIData = null;
                                        bool needAdd = false;
                                        {
                                            // find old
                                            foreach(TroopCageShootProjectileUI.UIData check in olds)
                                            {
                                                if (check.projectile.v.data == troopCageShootProjectile)
                                                {
                                                    troopCageShootProjectileUIData = check;
                                                    break;
                                                }
                                            }
                                            // make new
                                            if (troopCageShootProjectileUIData == null)
                                            {
                                                troopCageShootProjectileUIData = new TroopCageShootProjectileUI.UIData();
                                                {
                                                    troopCageShootProjectileUIData.uid = this.data.projectiles.makeId();
                                                }
                                                needAdd = true;
                                            }
                                            else
                                            {
                                                olds.Remove(troopCageShootProjectileUIData);
                                            }
                                        }
                                        // update
                                        {
                                            troopCageShootProjectileUIData.projectile.v = new ReferenceData<TroopCageShootProjectile>(troopCageShootProjectile);
                                        }
                                        // add
                                        if (needAdd)
                                        {
                                            this.data.projectiles.add(troopCageShootProjectileUIData);
                                        }
                                    }
                                }
                                // remove old
                                foreach(TroopCageShootProjectileUI.UIData old in olds)
                                {
                                    this.data.projectiles.remove(old);
                                }
                            }
                        }
                        // troops
                        {
                            // find
                            List<TroopFollow> troops = new List<TroopFollow>();
                            {
                                troops.AddRange(troopCage.troops.vs);
                            }
                            // process
                            {
                                // get old
                                List<TroopFollowUI.UIData> olds = new List<TroopFollowUI.UIData>();
                                {
                                    olds.AddRange(this.data.troops.vs);
                                }
                                // Update
                                {
                                    foreach (TroopFollow troop in troops)
                                    {
                                        // get UI
                                        TroopFollowUI.UIData troopFollowUIData = null;
                                        bool needAdd = false;
                                        {
                                            // find old
                                            foreach (TroopFollowUI.UIData check in olds)
                                            {
                                                if (check.troopFollow.v.data == troop)
                                                {
                                                    troopFollowUIData = check;
                                                    break;
                                                }
                                            }
                                            // make new
                                            if (troopFollowUIData == null)
                                            {
                                                troopFollowUIData = new TroopFollowUI.UIData();
                                                {
                                                    troopFollowUIData.uid = this.data.troops.makeId();
                                                }
                                                needAdd = true;
                                            }
                                            else
                                            {
                                                olds.Remove(troopFollowUIData);
                                            }
                                        }
                                        // update
                                        {
                                            troopFollowUIData.troopFollow.v = new ReferenceData<TroopFollow>(troop);
                                        }
                                        // add
                                        if (needAdd)
                                        {
                                            this.data.troops.add(troopFollowUIData);
                                        }
                                    }
                                }
                                // remove old
                                foreach (TroopFollowUI.UIData old in olds)
                                {
                                    this.data.troops.remove(old);
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("troopCage null");
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

        public override void OnDestroy()
        {
            base.OnDestroy();
            // remove: auto delete
            {
                if (this.data != null)
                {
                    TroopCage troopCage = this.data.troopCage.v.data;
                    if (troopCage != null)
                    {
                        BattleRush battleRush = troopCage.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            battleRush.laneObjects.remove(troopCage);
                        }
                    }
                    else
                    {
                        Logger.LogError("troopCage null");
                    }
                }
                else
                {
                    Logger.LogError("data null");
                }
            }
        }

        #region implement callBacks

        private BattleRush battleRush = null;

        public WayUI wayUI;
        public PrisonAnimationUI prisonAnimation;

        public TroopCageShootProjectileUI projectilePrefab;
        public TroopFollowUI troopFollowPrefab;

        public ObstructionUI obstructionUI;

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.troopCage.allAddCallBack(this);
                    uiData.way.allAddCallBack(this);
                    uiData.projectiles.allAddCallBack(this);
                    uiData.prisonAnimation.allAddCallBack(this);
                    uiData.troops.allAddCallBack(this);
                    uiData.obstruction.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                // troopCage
                {
                    if (data is TroopCage)
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
                    if (data is BattleRush)
                    {
                        dirty = true;
                        return;
                    }
                    // Child
                    if (data is TroopCage.State)
                    {
                        dirty = true;
                        return;
                    }
                }
                if(data is WayUI.UIData)
                {
                    WayUI.UIData wayUIData = data as WayUI.UIData;
                    // UI
                    {
                        if (wayUI != null)
                        {
                            wayUI.setData(wayUIData);
                        }
                        else
                        {
                            Logger.LogError("wayUI null");
                        }
                    }
                    dirty = true;
                    return;
                }
                if(data is TroopCageShootProjectileUI.UIData)
                {
                    TroopCageShootProjectileUI.UIData troopCageShootProjectileUIData = data as TroopCageShootProjectileUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(troopCageShootProjectileUIData, projectilePrefab, this.transform);
                    }
                    dirty = true;
                    return;
                }
                if(data is PrisonAnimationUI.UIData)
                {
                    PrisonAnimationUI.UIData prisonAnimationUIData = data as PrisonAnimationUI.UIData;
                    // UI
                    {
                        if (prisonAnimation != null)
                        {
                            prisonAnimation.setData(prisonAnimationUIData);
                        }
                        else
                        {
                            Logger.LogError("prisonAnimation null");
                        }
                    }
                    dirty = true;
                    return;
                }
                if(data is TroopFollowUI.UIData)
                {
                    TroopFollowUI.UIData troopFollowUIData = data as TroopFollowUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(troopFollowUIData, troopFollowPrefab, troopPoint);
                    }
                    dirty = true;
                    return;
                }
                if(data is ObstructionUI.UIData)
                {
                    ObstructionUI.UIData obstructionUIData = data as ObstructionUI.UIData;
                    // UI
                    {
                        if (obstructionUI != null)
                        {
                            obstructionUI.setData(obstructionUIData);
                        }
                        else
                        {
                            Logger.LogError("obstructionUI null");
                        }
                    }
                    dirty = true;
                    return;
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
                    uiData.troopCage.allRemoveCallBack(this);
                    uiData.way.allRemoveCallBack(this);
                    uiData.projectiles.allRemoveCallBack(this);
                    uiData.prisonAnimation.allRemoveCallBack(this);
                    uiData.troops.allRemoveCallBack(this);
                    uiData.obstruction.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                // troopCage
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
                    if (data is BattleRush)
                    {
                        return;
                    }
                    // Child
                    if (data is TroopCage.State)
                    {
                        return;
                    }
                }
                if (data is WayUI.UIData)
                {
                    WayUI.UIData wayUIData = data as WayUI.UIData;
                    // UI
                    {
                        if (wayUI != null)
                        {
                            wayUI.setDataNull(wayUIData);
                        }
                        else
                        {
                            Logger.LogError("wayUI null");
                        }
                    }
                    return;
                }
                if (data is TroopCageShootProjectileUI.UIData)
                {
                    TroopCageShootProjectileUI.UIData troopCageShootProjectileUIData = data as TroopCageShootProjectileUI.UIData;
                    // UI
                    {
                        troopCageShootProjectileUIData.removeCallBackAndDestroy(typeof(TroopCageShootProjectileUI));
                    }
                    return;
                }
                if (data is PrisonAnimationUI.UIData)
                {
                    PrisonAnimationUI.UIData prisonAnimationUIData = data as PrisonAnimationUI.UIData;
                    // UI
                    {
                        if (prisonAnimation != null)
                        {
                            prisonAnimation.setDataNull(prisonAnimationUIData);
                        }
                        else
                        {
                            Logger.LogError("prisonAnimation null");
                        }
                    }
                    return;
                }
                if (data is TroopFollowUI.UIData)
                {
                    TroopFollowUI.UIData troopFollowUIData = data as TroopFollowUI.UIData;
                    // UI
                    {
                        troopFollowUIData.removeCallBackAndDestroy(typeof(TroopFollowUI));
                    }
                    return;
                }
                if (data is ObstructionUI.UIData)
                {
                    ObstructionUI.UIData obstructionUIData = data as ObstructionUI.UIData;
                    // UI
                    {
                        if (obstructionUI != null)
                        {
                            obstructionUI.setDataNull(obstructionUIData);
                        }
                        else
                        {
                            Logger.LogError("obstructionUI null");
                        }
                    }
                    return;
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
                    case UIData.Property.troopCage:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.way:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.projectiles:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.prisonAnimation:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.troops:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.obstruction:
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
                // troopCage
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
                            case TroopCage.Property.position:
                                dirty = true;
                                break;
                            case TroopCage.Property.obstruction:
                                dirty = true;
                                break;
                            default:
                                Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                    // Parent
                    if (wrapProperty.p is BattleRush)
                    {
                        switch ((BattleRush.Property)wrapProperty.n)
                        {
                            case BattleRush.Property.segmentIndex:
                                dirty = true;
                                break;
                            default:
                                break;
                        }
                        return;
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
                if (wrapProperty.p is WayUI.UIData)
                {
                    return;
                }
                if (wrapProperty.p is PrisonAnimationUI.UIData)
                {
                    return;
                }
                if (wrapProperty.p is TroopFollowUI.UIData)
                {
                    return;
                }
                if (wrapProperty.p is ObstructionUI.UIData)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
