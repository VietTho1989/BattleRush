using System.Collections;
using System.Collections.Generic;
using BattleRushS.StateS.LoadS;
using UnityEngine;

namespace BattleRushS.StateS
{
    class LoadUpdate : UpdateBehavior<Load>
    {

        #region update

        // const string TestData = "[{\"I\":\"okg_coin\",\"P\":\"-2,6.22,2.5\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-2,6.22,8.5\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-2,6.22,12.5\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"troop_cage_1\",\"P\":\"0.3,0,52.9\",\"R\":\"0,1,0,0\"},{\"I\":\"troop_cage_1\",\"P\":\"-12.5,0,126.4\",\"R\":\"0,0.9659258,0,0.2588191\"},{\"I\":\"okg_coin\",\"P\":\"-0.6900001,0.5,66.9\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-2.26,0.5,68.57\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-0.6900001,0.5,69.9\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-2,6.22,67.5\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-16.5,0,130.3\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-18,0.5,132.3\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-19,0.5,133.9\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-14.5,0.5,131.2\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-15.8,0.5,133.1\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-16.8,0.5,134.8\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-20.2,0.5,136.2\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-21,0.5,138.2\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-22,0.5,140.2\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-23,0.5,142.2\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-24,0.5,144.2\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-22.5,0.5,144.6\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-18.6,0.5,137.2\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-19.5,0.5,138.9\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-20.3,0.5,141\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"okg_coin\",\"P\":\"-21.4,0.5,142.9\",\"R\":\"0,0.9998302,0,-0.01843051\"},{\"I\":\"energy_orb_normal\",\"P\":\"-1.3,0.5,21.8\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-1.3,0.5,22.8\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-1.3,0.5,23.8\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-1.3,0.5,24.8\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-1.3,0.5,25.8\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-1.3,0.5,26.8\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0.01,0.5,75.75\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0.01,0.5,77.75\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-2,0.5,79.75\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-2,0.5,80\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-2,0.5,82\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-2,0.5,84\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,0.5,80\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,0.5,82\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,0.5,84\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"1.47,2,79.14\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"1.47,2,81.14\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"1.47,2,83.14\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,2,85\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,2,87\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,2,89\",\"R\":\"0,0,0,1\"}]";
        public const string TestData = "[{\"I\":\"okg_coin\",\"P\":\"60.04,0,67\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"62.04,0,67\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"58.04,0,67\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"64.03999,0,67\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"56.04,0,67\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"56,0,68.99999\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"64,0,68.99999\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"58,0,68.99999\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"62,0,68.99999\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"60,0,68.99999\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"58,0,65.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"62,0,65.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"60,0,65.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"60,0,70.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"62,0,70.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"58,0,70.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"48.4,0,69\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"50.4,0,69\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"52.4,0,69\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"52.4,0,67.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"50.4,0,67.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"48.4,0,67.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"45.54,0,67\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"43.54,0,67\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"41.54,0,67\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"41.6,0,68.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"43.6,0,68.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"45.6,0,68.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"45.8,0,70\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"43.8,0,70\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"41.8,0,70\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"36,0,67.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"38,0,67.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"40,0,67.5\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"40,0,68.99999\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"38,0,68.99999\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"36,0,68.99999\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"2,0,2\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"2,0,4\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"2,0,6\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"2,0,8\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"2,0,10\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"2,0,12\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,0,2\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,0,4\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,0,6\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,0,8\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,0,10\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,0,12\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-2,0,2\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-2,0,4\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-2,0,6\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-2,0,8\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-2,0,10\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-2,0,12\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-1,0,50\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-1,0,52\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-1,0,54\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-1.155696,0,60.78828\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-1.78731,0,58.43108\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"-2,0,56\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,0,56\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0,0,58.43108\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0.7236893,0,60.10424\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"2.807614,0,65.19239\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"1.351025,0,63.45649\",\"R\":\"0,0,0,1\"},{\"I\":\"energy_orb_normal\",\"P\":\"0.2179996,0,61.49404\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"-1.5,0,38\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"-1.5,0,40\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"-1.5,0,42\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"0,0,42.06\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"0,0,40.06\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"0,0,38.06\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"1.5,0,38.06\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"1.5,0,40.06\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"1.5,0,42.06\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"3,0,40\",\"R\":\"0,0,0,1\"},{\"I\":\"okg_coin\",\"P\":\"-3,0,40\",\"R\":\"0,0,0,1\"}]";

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
            if(data is Load)
            {
                Load load = data as Load;
                // Child
                {
                    load.sub.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is Load.Sub)
                {
                    Load.Sub sub = data as Load.Sub;
                    // Update
                    {
                        switch (sub.getType())
                        {
                            case Load.Sub.Type.Reset:
                                {
                                    Reset reset = sub as Reset;
                                    UpdateUtils.makeUpdate<ResetUpdate, Reset>(reset, this.transform);
                                }
                                break;
                            case Load.Sub.Type.ChooseLevel:
                                {
                                    ChooseLevel chooseLevel = sub as ChooseLevel;
                                    UpdateUtils.makeUpdate<ChooseLevelUpdate, ChooseLevel>(chooseLevel, this.transform);
                                }
                                break;
                            case Load.Sub.Type.LoadLevel:
                                {
                                    LoadLevel loadLevel = sub as LoadLevel;
                                    UpdateUtils.makeUpdate<LoadLevelUpdate, LoadLevel>(loadLevel, this.transform);
                                }
                                break;
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
            if (data is Load)
            {
                Load load = data as Load;
                // Child
                {
                    load.sub.allRemoveCallBack(this);
                }
                this.setDataNull(load);
                return;
            }
            // Child
            {
                if (data is Load.Sub)
                {
                    Load.Sub sub = data as Load.Sub;
                    // Update
                    {
                        switch (sub.getType())
                        {
                            case Load.Sub.Type.Reset:
                                {
                                    Reset reset = sub as Reset;
                                    reset.removeCallBackAndDestroy(typeof(ResetUpdate));
                                }
                                break;
                            case Load.Sub.Type.ChooseLevel:
                                {
                                    ChooseLevel chooseLevel = sub as ChooseLevel;
                                    chooseLevel.removeCallBackAndDestroy(typeof(ChooseLevelUpdate));
                                }
                                break;
                            case Load.Sub.Type.LoadLevel:
                                {
                                    LoadLevel loadLevel = sub as LoadLevel;
                                    loadLevel.removeCallBackAndDestroy(typeof(LoadLevelUpdate));
                                }
                                break;
                        }
                    }
                    dirty = true;
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
            if(wrapProperty.p is Load)
            {
                switch ((Load.Property)wrapProperty.n)
                {
                    case Load.Property.sub:
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
                if (wrapProperty.p is Load.Sub)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
