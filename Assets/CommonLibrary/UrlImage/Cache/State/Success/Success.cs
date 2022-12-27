using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Images.Cache
{
    public class Success : ImageCache.State
    {

        public VO<Texture2D> texture;

        #region Constructor

        public enum Property
        {
            texture
        }

        public Success() : base()
        {
            this.texture = new VO<Texture2D>(this, (byte)Property.texture, null);
        }

        #endregion

        public override Type getType()
        {
            return Type.Success;
        }

        #region get common sprite

        private Sprite oldSprite = null;
        private Texture oldTexture = null;

        public Sprite getCommonSprite()
        {
            Sprite ret = null;
            {
                // get old
                {
                    if (this.texture.v == oldTexture)
                    {
                        ret = oldSprite;
                    }
                    else
                    {
                        oldTexture = null;
                        oldSprite = null;
                    }
                }
                // make new
                if (ret == null)
                {
                    // Logger.Log("need make new sprite");
                    ret = Sprite.Create(this.texture.v, new Rect(0, 0, this.texture.v.width, this.texture.v.height), new Vector2(0.5f, 0.5f), 100);
                    oldSprite = ret;
                    oldTexture = this.texture.v;
                }
                else
                {
                    // Logger.Log("have old sprite");
                }
            }
            return ret;
        }

        #endregion

    }
}