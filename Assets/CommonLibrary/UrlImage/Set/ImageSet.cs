using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Images
{
    public class ImageSet : Data
    {

        public VO<UrlUI> image;

        public VO<string> url;

        #region Constructor

        public enum Property
        {
            image,
            url
        }

        public ImageSet() : base()
        {
            this.image = new VO<UrlUI>(this, (byte)Property.image, null);
            this.url = new VO<string>(this, (byte)Property.url, "");
        }

        #endregion

    }
}