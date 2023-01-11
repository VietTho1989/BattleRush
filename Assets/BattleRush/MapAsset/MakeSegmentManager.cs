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

        public VO<int> assetIndex;

        public VO<int> repeatIndex;

        public VO<float> currentLength;

        public void next()
        {
            this.repeatIndex.v++;
            if(this.assetIndex.v>=0 && this.assetIndex.v < this.mapAsset.v.segments.Count)
            {
                SegmentAsset mapAsset = this.mapAsset.v.segments[this.assetIndex.v];
                if(this.repeatIndex.v >= Math.Max(mapAsset.repeat, 1) )
                {
                    this.assetIndex.v++;
                    this.repeatIndex.v = 0;
                }
                else
                {
                    // still in repeat
                }
            }
        }

        #endregion

        #region index

        public VO<int> currentNextSegmentIndex;

        #endregion

        #region Constructor

        public enum Property
        {
            mapAsset,
            assetIndex,
            repeatIndex,

            currentNextSegmentIndex,
            currentLength
        }

        public MakeSegmentManager() : base()
        {
            this.mapAsset = new VO<MapAsset>(this, (byte)Property.currentNextSegmentIndex, null);
            this.assetIndex = new VO<int>(this, (byte)Property.assetIndex, 0);
            this.repeatIndex = new VO<int>(this, (byte)Property.repeatIndex, -1);

            this.currentNextSegmentIndex = new VO<int>(this, (byte)Property.currentNextSegmentIndex, 0);
            this.currentLength = new VO<float>(this, (byte)Property.currentLength, -1);
        }

        #endregion

        public void reset()
        {
            this.mapAsset.v = null;
            this.assetIndex.v = 0;
            this.repeatIndex.v = -1;

            this.currentNextSegmentIndex.v = 0;
        }

    }
}