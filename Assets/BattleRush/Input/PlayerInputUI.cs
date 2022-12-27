using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using BattleRushS.StateS;
using Dreamteck.Forever;
using BattleRushS.HeroS;

namespace BattleRushS
{
    public class PlayerInputUI : UIBehavior<PlayerInputUI.UIData>, IPointerDownHandler, IPointerUpHandler
    {

        #region UIData

        public class UIData : Data
        {

            public VO<ReferenceData<BattleRush>> game;

            #region Constructor

            public enum Property
            {
                game
            }

            public UIData() : base()
            {
                this.game = new VO<ReferenceData<BattleRush>>(this, (byte)Property.game, ReferenceData<BattleRush>.Null);
            }

            #endregion

        }

        /*public override int getStartAllocate()
        {
            return 1;
        }*/

        #endregion

        #region Refresh

        private const float Ratio = 160;

        private bool needResetState = false;

        public new void Update()
        {
            // MyOnPointerDown();
            refresh();
            processUpdate();
            // MyOnPointerUp();
        }

        public LaneRunner laneRunner;

        private void processUpdate()
        {
            if (this.data != null)
            {
                BattleRush game = this.data.game.v.data;
                if (game != null)
                {
                    Hero hero = game.hero.v;
                    if (hero != null)
                    {
                        if (hero.heroMove.v.sub.v != null)
                        {
                            switch (hero.heroMove.v.sub.v.getType())
                            {
                                case HeroS.HeroMove.Sub.Type.Run:
                                    {
                                        HeroMoveRun heroMoveRun = hero.heroMove.v.sub.v as HeroMoveRun;
                                        // laneRunner
                                        {
                                            // get
                                            if (laneRunner == null)
                                            {
                                                HeroUI heroUI = hero.findCallBack<HeroUI>();
                                                if (heroUI != null)
                                                {
                                                    laneRunner = heroUI.GetComponent<LaneRunner>();
                                                }
                                                else
                                                {
                                                    Logger.LogError("heroUI null");
                                                }
                                            }
                                            // process
                                            if (laneRunner != null)
                                            {
                                                // find
                                                bool canRun = false;
                                                {
                                                    switch (game.state.v.getType())
                                                    {
                                                        case BattleRush.State.Type.Load:
                                                            break;
                                                        case BattleRush.State.Type.Start:
                                                            break;
                                                        case BattleRush.State.Type.Play:
                                                            {
                                                                Play play = game.state.v as Play;
                                                                switch (play.state.v)
                                                                {
                                                                    case Play.State.Normal:
                                                                        canRun = true;
                                                                        break;
                                                                    case Play.State.Pause:
                                                                        break;
                                                                }
                                                            }
                                                            break;
                                                        case BattleRush.State.Type.End:
                                                            break;
                                                    }
                                                }
                                                // process
                                                laneRunner.enabled = canRun;
                                            }
                                            else
                                            {
                                                Logger.LogError("laneRunner null");
                                            }
                                        }
                                        // Debug.LogError("touchCount: " + Input.touchCount);
                                        // change game state : pause
                                        {
                                            if (Settings.get().canPause.v)
                                            {
                                                BattleRush.State gameState = game.state.v;
                                                if (gameState != null)
                                                {
                                                    switch (gameState.getType())
                                                    {
                                                        case BattleRush.State.Type.Load:
                                                            break;
                                                        case BattleRush.State.Type.Start:
                                                            break;
                                                        case BattleRush.State.Type.Play:
                                                            {
                                                                Play play = gameState as Play;
                                                                // pause or not
                                                                bool isTouching = false;
                                                                {
                                                                    HeroMoveRun.State uiState = heroMoveRun.state.v;
                                                                    if (uiState != null)
                                                                    {
                                                                        switch (uiState.getType())
                                                                        {
                                                                            case HeroMoveRun.State.Type.None:
                                                                                break;
                                                                            case HeroMoveRun.State.Type.CanPlay:
                                                                                {
                                                                                    HeroMoveRun.StateCanPlay stateCanPlay = uiState as HeroMoveRun.StateCanPlay;
                                                                                    if (stateCanPlay.sub.v.getType() == HeroMoveRun.StateCanPlay.Sub.Type.Touch)
                                                                                    {
                                                                                        isTouching = true;
                                                                                    }
                                                                                }
                                                                                break;
                                                                            default:
                                                                                Logger.LogError("unknown type: " + uiState.getType());
                                                                                break;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Logger.LogError("uiState null");
                                                                    }
                                                                }
                                                                //Debug.Log("playerInput: " + isTouching);
                                                                play.state.v = isTouching ? Play.State.Normal : Play.State.Pause;
                                                            }
                                                            break;
                                                        case BattleRush.State.Type.End:
                                                            break;
                                                        default:
                                                            Logger.LogError("unknown type: " + gameState.getType());
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    Logger.LogError("gameState null");
                                                }
                                            }
                                        }
                                        // state
                                        HeroMoveRun.State state = heroMoveRun.state.v;
                                        if (state != null)
                                        {
                                            switch (state.getType())
                                            {
                                                case HeroMoveRun.State.Type.None:
                                                    break;
                                                case HeroMoveRun.State.Type.CanPlay:
                                                    {
                                                        // make move
                                                        HeroMoveRun.StateCanPlay stateCanPlay = state as HeroMoveRun.StateCanPlay;
                                                        switch (stateCanPlay.sub.v.getType())
                                                        {
                                                            case HeroMoveRun.StateCanPlay.Sub.Type.NotTouch:
                                                                break;
                                                            case HeroMoveRun.StateCanPlay.Sub.Type.Touch:
                                                                {
                                                                    HeroMoveRun.StateCanPlay.Touch stateTouch = stateCanPlay.sub.v as HeroMoveRun.StateCanPlay.Touch;
                                                                    // set current
                                                                    {
                                                                        Vector2 mousePosition = Input.mousePosition;
                                                                        {
                                                                            // touch 0
                                                                            {
                                                                                if (Input.touches.Length > 0)
                                                                                {
                                                                                    // find touch
                                                                                    Touch touch = Input.GetTouch(0);
                                                                                    {
                                                                                        bool isFound = false;
                                                                                        foreach (Touch check in Input.touches)
                                                                                        {
                                                                                            if (check.fingerId == stateTouch.fingerId.v)
                                                                                            {
                                                                                                isFound = true;
                                                                                                touch = check;
                                                                                                Logger.LogError("find correct touch: " + touch.fingerId);
                                                                                                break;
                                                                                            }
                                                                                        }
                                                                                        if (!isFound)
                                                                                        {
                                                                                            Logger.LogError("why not found touch: " + stateTouch.fingerId);
                                                                                        }
                                                                                    }
                                                                                    // process
                                                                                    mousePosition = touch.position;
                                                                                }
                                                                                else
                                                                                {
                                                                                    // Logger.LogError("Don't have any touchs");
                                                                                }
                                                                            }
                                                                        }
                                                                        Vector2 localPos = transform.InverseTransformPoint(mousePosition);
                                                                        stateTouch.current.v = localPos.x;
                                                                        // Debug.LogError("current input mouse position: " + stateTouch.current.v + ", " + stateTouch.from.v);
                                                                    }
                                                                    // set player input
                                                                    {
                                                                        bool canMove = true;
                                                                        {
                                                                            // isPlay
                                                                            if (canMove)
                                                                            {
                                                                                BattleRush.State gameState = game.state.v;
                                                                                if (gameState != null)
                                                                                {
                                                                                    switch (gameState.getType())
                                                                                    {
                                                                                        case BattleRush.State.Type.Load:
                                                                                            canMove = false;
                                                                                            break;
                                                                                        case BattleRush.State.Type.Start:
                                                                                            canMove = true;
                                                                                            break;
                                                                                        case BattleRush.State.Type.Play:
                                                                                            {
                                                                                                Play play = gameState as Play;
                                                                                                if (play.state.v != Play.State.Normal)
                                                                                                {
                                                                                                    canMove = false;
                                                                                                }
                                                                                            }
                                                                                            break;
                                                                                        case BattleRush.State.Type.End:
                                                                                            canMove = false;
                                                                                            break;
                                                                                        default:
                                                                                            Logger.LogError("unknown type: " + gameState.getType());
                                                                                            break;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    Logger.LogError("gameState null");
                                                                                }
                                                                            }
                                                                        }
                                                                        if (canMove)
                                                                        {
                                                                            float newPos = stateTouch.startInput.v + (stateTouch.current.v - stateTouch.from.v) * (70.0f / 480) * 0.03f;// (80.0f / 480) * 0.03f;
                                                                            float newMove = Mathf.Lerp(heroMoveRun.move.v, newPos, Time.deltaTime * 8.0f);// 10.0f
                                                                            heroMoveRun.move.v = Mathf.Clamp(newMove, -0.5f, 0.5f);
                                                                            heroMoveRun.newPos.v = newPos;

                                                                            // update laneRunner position
                                                                            {
                                                                                if (laneRunner != null)
                                                                                {
                                                                                    int newLane = Mathf.RoundToInt((heroMoveRun.newPos.v + 0.5f) * laneRunner.laneCount);
                                                                                    if (laneRunner.lane < newLane)
                                                                                    {
                                                                                        laneRunner.lane++;
                                                                                    }
                                                                                    else if (laneRunner.lane > newLane)
                                                                                    {
                                                                                        laneRunner.lane--;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    Logger.LogError("laneRunner null");
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                break;
                                                            default:
                                                                Logger.LogError("unknown type: " + stateCanPlay.sub.v.getType());
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    Logger.LogError("unknown type: " + state.getType());
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("state null");
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
                    Logger.LogError("game null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    BattleRush battleRush = this.data.game.v.data;
                    if (battleRush != null)
                    {
                        Hero hero = battleRush.hero.v;
                        if (hero != null)
                        {
                            if (hero.heroMove.v.sub.v != null)
                            {
                                switch (hero.heroMove.v.sub.v.getType())
                                {
                                    case HeroMove.Sub.Type.Run:
                                        {
                                            HeroMoveRun heroMoveRun = hero.heroMove.v.sub.v as HeroMoveRun;
                                            bool canPlay = false;
                                            {
                                                BattleRush.State state = battleRush.state.v;
                                                if (state != null)
                                                {
                                                    switch (battleRush.state.v.getType())
                                                    {
                                                        case BattleRush.State.Type.Load:
                                                            break;
                                                        case BattleRush.State.Type.Start:
                                                            {
                                                                canPlay = true;
                                                            }
                                                            break;
                                                        case BattleRush.State.Type.Play:
                                                            {
                                                                canPlay = true;
                                                            }
                                                            break;
                                                        case BattleRush.State.Type.End:
                                                            break;
                                                        default:
                                                            Logger.LogError("unknown type: " + battleRush.state.v.getType());
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    Logger.LogError("state null");
                                                }
                                            }
                                            // Debug.LogError("canPlay: " + canPlay);
                                            if (canPlay)
                                            {
                                                HeroMoveRun.StateCanPlay stateCanPlay = heroMoveRun.state.newOrOld<HeroMoveRun.StateCanPlay>();
                                                {

                                                }
                                                heroMoveRun.state.v = stateCanPlay;
                                            }
                                            else
                                            {
                                                HeroMoveRun.StateNone none = heroMoveRun.state.newOrOld<HeroMoveRun.StateNone>();
                                                {

                                                }
                                                heroMoveRun.state.v = none;
                                            }
                                            // reset state
                                            {
                                                if (needResetState)
                                                {
                                                    needResetState = false;
                                                    switch (heroMoveRun.state.v.getType())
                                                    {
                                                        case HeroMoveRun.State.Type.CanPlay:
                                                            {
                                                                HeroMoveRun.StateCanPlay stateCanPlay = heroMoveRun.state.v as HeroMoveRun.StateCanPlay;
                                                                // make none
                                                                {
                                                                    switch (stateCanPlay.sub.v.getType())
                                                                    {
                                                                        case HeroMoveRun.StateCanPlay.Sub.Type.NotTouch:
                                                                            break;
                                                                        case HeroMoveRun.StateCanPlay.Sub.Type.Touch:
                                                                            {
                                                                                HeroMoveRun.StateCanPlay.Touch touch = stateCanPlay.sub.v as HeroMoveRun.StateCanPlay.Touch;
                                                                                // change start input
                                                                                touch.startInput.v = 0;
                                                                                // change from
                                                                                {
                                                                                    Vector2 localPos = transform.InverseTransformPoint(Input.mousePosition);// transform.InverseTransformPoint(eventData.position);
                                                                                    touch.from.v = localPos.x;
                                                                                    touch.last.v = localPos.x;
                                                                                }
                                                                            }
                                                                            break;
                                                                        default:
                                                                            Logger.LogError("unknown type: " + stateCanPlay.sub.v.getType());
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case HeroMove.Sub.Type.Arena:
                                        break;
                                    default:
                                        Logger.LogError("unknown sub: " + hero.heroMove.v.sub.v.getType());
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
                        // Debug.LogError("player null");
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
            return false;
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
                    uiData.game.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is BattleRush)
                {
                    BattleRush game = data as BattleRush;
                    // Child
                    {
                        game.state.allAddCallBack(this);
                        game.hero.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                {
                    if (data is BattleRush.State)
                    {
                        dirty = true;
                        return;
                    }
                    // hero
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
                            if(data is HeroMove)
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
                                if(data is HeroMove.Sub)
                                {
                                    HeroMove.Sub sub = data as HeroMove.Sub;
                                    // Child
                                    {
                                        switch (sub.getType())
                                        {
                                            case HeroMove.Sub.Type.Run:
                                                {
                                                    HeroMoveRun heroMoveRun = sub as HeroMoveRun;
                                                    heroMoveRun.state.allAddCallBack(this);
                                                }
                                                break;
                                            case HeroMove.Sub.Type.Arena:
                                                break;
                                            default:
                                                Logger.LogError("unknown type: " + sub.getType());
                                                break;
                                        }
                                    }
                                    dirty = true;
                                    return;
                                }
                                // Child
                                if(data is HeroMoveRun.State)
                                {
                                    dirty = true;
                                    return;
                                }
                            }
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
                    uiData.game.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is BattleRush)
                {
                    BattleRush game = data as BattleRush;
                    // Child
                    {
                        game.state.allRemoveCallBack(this);
                        game.hero.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                {
                    if (data is BattleRush.State)
                    {
                        return;
                    }
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
                                                    heroMoveRun.state.allRemoveCallBack(this);
                                                }
                                                break;
                                            case HeroMove.Sub.Type.Arena:
                                                break;
                                            default:
                                                Logger.LogError("unknown type: " + sub.getType());
                                                break;
                                        }
                                    }
                                    return;
                                }
                                // Child
                                if (data is HeroMoveRun.State)
                                {
                                    return;
                                }
                            }
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
                    case UIData.Property.game:
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
                        case BattleRush.Property.hero:
                            {
                                ValueChangeUtils.replaceCallBack(this, syncs);
                                dirty = true;
                            }
                            break;
                        default:
                            // Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
                // Child
                {
                    if (wrapProperty.p is BattleRush.State)
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
                                            Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                                            break;
                                    }
                                }
                                break;
                            case BattleRush.State.Type.End:
                                break;
                            default:
                                break;
                        }
                        return;
                    }
                    // hero
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
                                                        case HeroMoveRun.Property.state:
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
                                            default:
                                                Logger.LogError("unknown type: " + sub.getType());
                                                break;
                                        }
                                    }
                                    return;
                                }
                                // Child
                                if (wrapProperty.p is HeroMoveRun.State)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

        #region myOnPointer

        private struct MyTouch
        {
            public int fingerId;
            public float x;
        }

        private List<MyTouch> myTouchDowns = new List<MyTouch>();

        public static bool IsLastInHaveRight = true;

        public void MyOnPointerDown()
        {
            Debug.Log("MyOnPinterDown");
            refresh();
            if (this.data != null)
            {
                BattleRush game = this.data.game.v.data;
                if (game != null)
                {
                    Hero hero = game.hero.v;
                    if (hero != null)
                    {
                        if (hero.heroMove.v.sub.v != null)
                        {
                            switch (hero.heroMove.v.sub.v.getType())
                            {
                                case HeroMove.Sub.Type.Run:
                                    {
                                        HeroMoveRun heroMoveRun = hero.heroMove.v.sub.v as HeroMoveRun;
                                        // get myTouchDowns
                                        myTouchDowns.Clear();
                                        {
                                            // click mouse
                                            if (Input.GetMouseButtonDown(0))
                                            {
                                                MyTouch myTouch = new MyTouch();
                                                {
                                                    myTouch.fingerId = -1;
                                                    myTouch.x = transform.InverseTransformPoint(Input.mousePosition).x;
                                                }
                                                myTouchDowns.Add(myTouch);
                                            }
                                            // touch
                                            foreach (Touch touch in Input.touches)
                                            {
                                                if (touch.phase == TouchPhase.Began)
                                                {
                                                    MyTouch myTouch = new MyTouch();
                                                    {
                                                        myTouch.fingerId = touch.fingerId;
                                                        myTouch.x = transform.InverseTransformPoint(touch.position).x;
                                                    }
                                                    myTouchDowns.Add(myTouch);
                                                }
                                            }
                                        }
                                        // process
                                        foreach (MyTouch myTouch in myTouchDowns)
                                        {
                                            // touch
                                            HeroMoveRun.State state = heroMoveRun.state.v;
                                            if (state != null)
                                            {
                                                // Logger.LogError("onPointerDown: " + state);
                                                switch (state.getType())
                                                {
                                                    case HeroMoveRun.State.Type.None:
                                                        break;
                                                    case HeroMoveRun.State.Type.CanPlay:
                                                        {
                                                            HeroMoveRun.StateCanPlay stateCanPlay = state as HeroMoveRun.StateCanPlay;
                                                            // find need new touch
                                                            bool isNeedNewTouch = false;
                                                            {
                                                                switch (stateCanPlay.sub.v.getType())
                                                                {
                                                                    case HeroMoveRun.StateCanPlay.Sub.Type.NotTouch:
                                                                        isNeedNewTouch = true;
                                                                        break;
                                                                    case HeroMoveRun.StateCanPlay.Sub.Type.Touch:
                                                                        {
                                                                            if (IsLastInHaveRight)
                                                                            {
                                                                                isNeedNewTouch = true;
                                                                            }
                                                                        }
                                                                        break;
                                                                    default:
                                                                        Logger.LogError("unknown sub type: " + stateCanPlay.sub.v.getType());
                                                                        break;
                                                                }
                                                            }
                                                            // process
                                                            if (isNeedNewTouch)
                                                            {
                                                                HeroMoveRun.StateCanPlay.Touch stateTouch = stateCanPlay.sub.newOrOld<HeroMoveRun.StateCanPlay.Touch>();
                                                                {
                                                                    // startInput
                                                                    stateTouch.startInput.v = heroMoveRun.move.v;
                                                                    // fingerId
                                                                    stateTouch.fingerId.v = myTouch.fingerId;
                                                                    // from
                                                                    {
                                                                        stateTouch.from.v = myTouch.x;
                                                                        stateTouch.last.v = myTouch.x;
                                                                    }
                                                                }
                                                                stateCanPlay.sub.v = stateTouch;
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        Logger.LogError("unknown type: " + state.getType());
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                Logger.LogError("state null");
                                            }
                                        }
                                        // start game
                                        {
                                            BattleRush.State gameState = game.state.v;
                                            if (gameState != null)
                                            {
                                                if (gameState is Start)
                                                {
                                                    Play play = game.state.newOrOld<Play>();
                                                    {

                                                    }
                                                    game.state.v = play;
                                                }
                                            }
                                            else
                                            {
                                                Logger.LogError("gameState null: " + this);
                                            }
                                        }
                                    }
                                    break;
                                case HeroMove.Sub.Type.Arena:
                                    break;
                                default:
                                    Logger.LogError("unknown sub: " + hero.heroMove.v.sub.v.getType());
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
                    Logger.LogError("player null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        private List<MyTouch> myTouchUps = new List<MyTouch>();

        public void MyOnPointerUp()
        {
            // Debug.LogError("onPointerUp: " + eventData);
            refresh();
            if (this.data != null)
            {
                BattleRush game = this.data.game.v.data;
                if (game != null)
                {
                    Hero hero = game.hero.v;
                    if (hero != null)
                    {
                        if (hero.heroMove.v.sub.v != null)
                        {
                            switch (hero.heroMove.v.sub.v.getType())
                            {
                                case HeroMove.Sub.Type.Run:
                                    {
                                        HeroMoveRun heroMoveRun = hero.heroMove.v.sub.v as HeroMoveRun;
                                        // get
                                        myTouchUps.Clear();
                                        {
                                            // click mouse
                                            if (Input.GetMouseButtonUp(0))
                                            {
                                                MyTouch myTouch = new MyTouch();
                                                {
                                                    myTouch.fingerId = -1;
                                                    myTouch.x = transform.InverseTransformPoint(Input.mousePosition).x;
                                                }
                                                myTouchUps.Add(myTouch);
                                            }
                                            // touch
                                            foreach (Touch touch in Input.touches)
                                            {
                                                if (touch.phase == TouchPhase.Ended)
                                                {
                                                    MyTouch myTouch = new MyTouch();
                                                    {
                                                        myTouch.fingerId = touch.fingerId;
                                                        myTouch.x = transform.InverseTransformPoint(touch.position).x;
                                                    }
                                                    myTouchUps.Add(myTouch);
                                                }
                                            }
                                        }
                                        // process
                                        {
                                            if (myTouchUps.Count > 0)
                                            {
                                                HeroMoveRun.State state = heroMoveRun.state.v;
                                                if (state != null)
                                                {
                                                    switch (state.getType())
                                                    {
                                                        case HeroMoveRun.State.Type.None:
                                                            break;
                                                        case HeroMoveRun.State.Type.CanPlay:
                                                            {
                                                                HeroMoveRun.StateCanPlay stateCanPlay = state as HeroMoveRun.StateCanPlay;
                                                                switch (stateCanPlay.sub.v.getType())
                                                                {
                                                                    case HeroMoveRun.StateCanPlay.Sub.Type.NotTouch:
                                                                        break;
                                                                    case HeroMoveRun.StateCanPlay.Sub.Type.Touch:
                                                                        {
                                                                            HeroMoveRun.StateCanPlay.Touch stateTouch = stateCanPlay.sub.v as HeroMoveRun.StateCanPlay.Touch;
                                                                            // find remain touch
                                                                            MyTouch remainingTouch = new MyTouch();
                                                                            bool isHaveRemainingTouch = false;
                                                                            {
                                                                                // find in touch
                                                                                if (!isHaveRemainingTouch)
                                                                                {
                                                                                    if (IsLastInHaveRight)
                                                                                    {
                                                                                        for (int i = Input.touches.Length - 1; i >= 0; i--)
                                                                                        {
                                                                                            bool isRemain = true;
                                                                                            {
                                                                                                foreach (MyTouch check in myTouchUps)
                                                                                                {
                                                                                                    if (check.fingerId == Input.touches[i].fingerId)
                                                                                                    {
                                                                                                        isRemain = false;
                                                                                                        break;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            if (isRemain)
                                                                                            {
                                                                                                remainingTouch.fingerId = Input.touches[i].fingerId;
                                                                                                remainingTouch.x = transform.InverseTransformPoint(Input.touches[i].position).x;
                                                                                                isHaveRemainingTouch = true;
                                                                                                break;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        // phai giong voi cai tren
                                                                                        for (int i = 0; i < Input.touches.Length; i++)
                                                                                        {
                                                                                            bool isRemain = true;
                                                                                            {
                                                                                                foreach (MyTouch check in myTouchUps)
                                                                                                {
                                                                                                    if (check.fingerId == Input.touches[i].fingerId)
                                                                                                    {
                                                                                                        isRemain = false;
                                                                                                        break;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            if (isRemain)
                                                                                            {
                                                                                                remainingTouch.fingerId = Input.touches[i].fingerId;
                                                                                                remainingTouch.x = transform.InverseTransformPoint(Input.touches[i].position).x;
                                                                                                isHaveRemainingTouch = true;
                                                                                                break;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                // find in mouse
                                                                                if (!isHaveRemainingTouch)
                                                                                {
                                                                                    if (Input.GetMouseButton(0))
                                                                                    {
                                                                                        bool isRemain = true;
                                                                                        {
                                                                                            foreach (MyTouch check in myTouchUps)
                                                                                            {
                                                                                                if (check.fingerId == -1)
                                                                                                {
                                                                                                    isRemain = false;
                                                                                                    break;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        if (isRemain)
                                                                                        {
                                                                                            remainingTouch.fingerId = -1;
                                                                                            remainingTouch.x = transform.InverseTransformPoint(Input.mousePosition).x;
                                                                                            isHaveRemainingTouch = true;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            // process
                                                                            if (isHaveRemainingTouch)
                                                                            {
                                                                                // startInput
                                                                                stateTouch.startInput.v = heroMoveRun.move.v;
                                                                                // fingerId
                                                                                stateTouch.fingerId.v = remainingTouch.fingerId;
                                                                                // from
                                                                                {
                                                                                    stateTouch.from.v = remainingTouch.x;
                                                                                    stateTouch.last.v = remainingTouch.x;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                HeroMoveRun.StateCanPlay.NotTouch notTouch = new HeroMoveRun.StateCanPlay.NotTouch();
                                                                                {
                                                                                    notTouch.uid = stateCanPlay.sub.makeId();
                                                                                }
                                                                                stateCanPlay.sub.v = notTouch;
                                                                            }
                                                                        }
                                                                        break;
                                                                    default:
                                                                        Logger.LogError("unknown sub type: " + stateCanPlay.sub.v.getType());
                                                                        break;
                                                                }
                                                            }
                                                            break;
                                                        default:
                                                            Logger.LogError("unknown type: " + state.getType());
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    Logger.LogError("state null");
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case HeroMove.Sub.Type.Arena:
                                    break;
                                default:
                                    Logger.LogError("unknown type: " + hero.heroMove.v.sub.v.getType());
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
                    Logger.LogError("player null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            Debug.LogError("onPointerDown");
            MyOnPointerDown();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            Debug.LogError("onPointerUp");
            MyOnPointerUp();
        }

        #endregion

    }
}
// TODO Vuot cam giac qua tay