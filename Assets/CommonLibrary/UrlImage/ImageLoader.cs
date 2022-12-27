using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Images
{
    public class ImageLoader : Data
    {

        #region instance

        private static ImageLoader instance;

        static ImageLoader()
        {
            instance = new ImageLoader();
        }

        public static ImageLoader get()
        {
            return instance;
        }

        #endregion

        #region imageCaches

        public LD<ImageCache> imageCaches;

        public ImageCache findImageCache(string url)
        {
            return this.imageCaches.vs.Find((ImageCache obj) => obj.url.v == url);
        }

        #endregion

        #region imageSet

        public LD<ImageSet> imageSets;

        public void addImageSet(UrlUI urlUI, string url)
        {
            // check url legal
            bool isUrlLegal = true;
            {
                // null or empty
                if (isUrlLegal)
                {
                    if (string.IsNullOrEmpty(url))
                    {
                        isUrlLegal = false;
                    }
                }
            }
            // process
            if (isUrlLegal)
            {
                // find imageSet
                ImageSet imageSet = this.imageSets.vs.Find((ImageSet obj) => obj.image.v == urlUI);
                bool needAdd = false;
                {
                    if (imageSet == null)
                    {
                        imageSet = new ImageSet();
                        {
                            imageSet.uid = this.imageSets.makeId();
                        }
                        needAdd = true;
                    }
                }
                // Update
                {
                    imageSet.image.v = urlUI;
                    imageSet.url.v = url;
                }
                // add
                if (needAdd)
                    this.imageSets.add(imageSet);
            }
            else
            {
                // Debug.LogError("url not legal: " + url);
            }
        }

        public void removeImageSet(UrlUI urlUI)
        {
            ImageSet imageSet = this.imageSets.vs.Find((ImageSet obj) => obj.image.v == urlUI);
            this.imageSets.remove(imageSet);
        }

        #endregion

        public VO<int> changeIndex;

        #region Constructor

        public enum Property
        {
            imageCaches,
            imageSets,
            changeIndex
        }

        public ImageLoader() : base()
        {
            this.imageCaches = new LD<ImageCache>(this, (byte)Property.imageCaches);
            this.imageSets = new LD<ImageSet>(this, (byte)Property.imageSets);
            this.changeIndex = new VO<int>(this, (byte)Property.changeIndex, 0);
        }

        #endregion

    }
}