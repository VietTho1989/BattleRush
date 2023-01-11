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

        public VO<int> index;

        public VO<int> repeat;

        public VO<bool> alreadyMakePath;

        public VO<bool> alreadyMakePrefab;

        public SegmentAsset nextMakePath()
        {
            this.alreadyMakePath.v = true;
            return next();
        }

        public SegmentAsset nextMakePrefab()
        {
            this.alreadyMakePrefab.v = true;
            return next();
        }

        private SegmentAsset next()
        {
            SegmentAsset ret = null;
            {
                if (this.alreadyMakePath.v && this.alreadyMakePrefab.v)
                {
                    this.repeat.v++;
                    if (this.index.v >= 0 && this.index.v < this.mapAsset.v.segments.Count)
                    {
                        SegmentAsset mapAsset = this.mapAsset.v.segments[this.index.v];
                        ret = mapAsset;
                        if (this.repeat.v >= Math.Max(mapAsset.repeat, 1) - 1)
                        {
                            this.index.v++;
                            this.repeat.v = -1;
                            this.alreadyMakePath.v = false;
                            this.alreadyMakePrefab.v = false;
                        }
                        else
                        {
                            // still in repeat
                        }
                    }
                }
            }
            return ret;
        }

        #endregion

        public VO<float> currentLength;


        #region Constructor

        public enum Property
        {
            mapAsset,

            index,
            repeat,
            alreadyMakePath,
            alreadyMakePrefab,

            currentLength
        }

        public MakeSegmentManager() : base()
        {
            this.mapAsset = new VO<MapAsset>(this, (byte)Property.mapAsset, null);
            // asset index
            {
                this.index = new VO<int>(this, (byte)Property.index, 0);
                this.repeat = new VO<int>(this, (byte)Property.repeat, -1);
                this.alreadyMakePath = new VO<bool>(this, (byte)Property.alreadyMakePath, false);
                this.alreadyMakePrefab = new VO<bool>(this, (byte)Property.alreadyMakePrefab, false);
            }
            this.currentLength = new VO<float>(this, (byte)Property.currentLength, 0);
        }

        #endregion

        public void reset()
        {
            this.mapAsset.v = null;
            // asset index
            {
                this.index.v = 0;
                this.repeat.v = -1;
                this.alreadyMakePath.v = false;
                this.alreadyMakePrefab.v = false;
            }            
            this.currentLength.v = 0;
        }

    }
}