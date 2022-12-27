using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ArrowAnimation : MonoBehaviour
{

    private float time = 0;
    private float duration = 1.0f;

    public Sprite[] arrows;
    public Image imgArrow;

    private void Update()
    {
        if (imgArrow != null)
        {
            if (arrows != null)
            {
                // correct time
                {
                    if (time < 0)
                    {
                        time = 0;
                    }
                    else if (time >= duration)
                    {
                        time -= duration;
                    }
                }
                // set sprite
                {
                    int index = (int)(time / (duration / arrows.Length));
                    if(index>=0 && index < arrows.Length)
                    {
                        imgArrow.sprite = arrows[index];
                    }
                    else
                    {
                        Logger.LogError("index error: " + index);
                    }
                }
                // change time
                time += Time.deltaTime;
            }
            else
            {
                Logger.LogError("arrows null");
            }
        }
        else
        {
            Logger.LogError("imgArrow null");
        }
    }

    private void OnEnable()
    {
        time = 0;
    }

}
