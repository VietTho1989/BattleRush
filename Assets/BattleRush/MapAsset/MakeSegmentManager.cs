using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS
{
    public class MakeSegmentManager : Data
    {

        #region map asset

        public VO<MapAsset> mapAsset;

        public class AssetIndex : Data
        {

            public VO<int> index;

            public VO<int> repeat;

            public VO<bool> alreadyMakePath;

            public VO<bool> alreadyMakePrefab;

            #region Constructor

            public enum Property
            {
                index,
                repeat,
                alreadyMakePath,
                alreadyMakePrefab
            }

            #endregion

        }

        public VO<int> assetIndex;

        public VO<int> repeatIndex;

        public VO<float> currentLength;

        public void next()
        {
            this.repeatIndex.v++;
            if (this.assetIndex.v >= 0 && this.assetIndex.v < this.mapAsset.v.segments.Count)
            {
                SegmentAsset mapAsset = this.mapAsset.v.segments[this.assetIndex.v];
                if (this.repeatIndex.v >= Math.Max(mapAsset.repeat, 1))
                {
                    this.assetIndex.v++;
                    this.repeatIndex.v = 0;
                }
                else
                {
                    // still in repeat
                }
            }
            Logger.Log("makeSegmentManager: " + this.assetIndex.v);
        }

        #endregion


        #region Constructor

        public enum Property
        {
            mapAsset,
            assetIndex,
            repeatIndex,
            currentLength
        }

        public MakeSegmentManager() : base()
        {
            this.mapAsset = new VO<MapAsset>(this, (byte)Property.mapAsset, null);
            this.assetIndex = new VO<int>(this, (byte)Property.assetIndex, 0);
            this.repeatIndex = new VO<int>(this, (byte)Property.repeatIndex, -1);
            this.currentLength = new VO<float>(this, (byte)Property.currentLength, 0);
        }

        #endregion

        public void reset()
        {
            this.mapAsset.v = null;
            this.assetIndex.v = 0;
            this.repeatIndex.v = -1;
            this.currentLength.v = 0;
        }

    }
}