using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleRushS.StateS.LoadS
{
    public class LoadLevelByScriptableObjectUpdate : UpdateBehavior<LoadLevelByScriptableObject>
    {

        #region update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                    if (battleRush != null)
                    {
                        // find
                        ItemMap itemMap = null;
                        {
                            BattleRushUI battleRushUI = battleRush.findCallBack<BattleRushUI>();
                            if (battleRushUI != null)
                            {
                                if (battleRushUI.itemMaps.Count > 0)
                                {
                                    LoadLevel loadLevel = this.data.findDataInParent<LoadLevel>();
                                    if (loadLevel != null)
                                    {
                                        int index = Mathf.Clamp(loadLevel.level.v, 0, battleRushUI.itemMaps.Count - 1);
                                        itemMap = battleRushUI.itemMaps[index];
                                    }
                                    else
                                    {
                                        Logger.LogError("loadLevel null");
                                    }
                                }
                            }
                            else
                            {
                                Logger.LogError("battleRushUI null");
                            }
                        }
                        // process
                        if (itemMap != null)
                        {
                            // make map data
                            MapData mapData = new MapData();
                            {

                            }
                            // change state
                            {

                            }
                        }
                        else
                        {
                            Logger.LogError("itemMap null");
                        }
                    }
                    else
                    {
                        Logger.LogError("battleRush null");
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
            if(data is LoadLevelByScriptableObject)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is LoadLevelByScriptableObject)
            {
                LoadLevelByScriptableObject loadLevelByScriptableObject = data as LoadLevelByScriptableObject;
                this.setDataNull(loadLevelByScriptableObject);
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
            if(wrapProperty.p is LoadLevelByScriptableObject)
            {
                switch ((LoadLevelByScriptableObject.Property)wrapProperty.n)
                {
                    default:
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}