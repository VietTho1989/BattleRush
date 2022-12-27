using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleRushS.HeroS
{
    public class HeroMove : Data
    {
        
        /**
         * current segment hero in
         * */
        public VO<Segment> currentSegment;

        #region sub

        public abstract class Sub : Data
        {

            public enum Type
            {
                Run,
                Arena
            }

            public abstract Type getType();

        }

        public VD<Sub> sub;

        #endregion

        #region Constructor

        public enum Property
        {
            currentSegment,
            sub
        }

        public HeroMove() : base()
        {
            this.currentSegment = new VO<Segment>(this, (byte)Property.currentSegment, null);
            this.sub = new VD<Sub>(this, (byte)Property.sub, null);
        }

        #endregion

        public void reset()
        {
            this.sub.v = null;
        }

    }
}
