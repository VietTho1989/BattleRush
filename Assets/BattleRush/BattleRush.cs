using System.Collections;
using System.Collections.Generic;
using BattleRushS.StateS;
using UnityEngine;

namespace BattleRushS
{
    public class BattleRush : Data
    {
        #region state

        public abstract class State : Data
        {

            public enum Type
            {
                Load,
                Start,
                Play,
                End
            }

            public abstract Type getType();

        }

        public VD<State> state;

        #endregion

        public VD<MapData> mapData;

        public VD<Hero> hero;

        public LD<ObjectInPath> laneObjects;

        /**
         * index of last segment in LevelGenerator
         */
        public VO<int> segmentIndex;

        public LD<Arena> arenas;

        public enum Property
        {
            state,
            mapData, 
            hero,
            laneObjects,
            segmentIndex,
            arenas
        }

        public BattleRush() : base()
        {
            this.state = new VD<State>(this, (byte)Property.state, new Load());
            this.mapData = new VD<MapData>(this, (byte)Property.mapData, new MapData());
            this.hero = new VD<Hero>(this, (byte)Property.hero, new Hero());
            this.laneObjects = new LD<ObjectInPath>(this, (byte)Property.laneObjects);
            this.segmentIndex = new VO<int>(this, (byte)Property.segmentIndex, -1);
            this.arenas = new LD<Arena>(this, (byte)Property.arenas);
        }
    }

}