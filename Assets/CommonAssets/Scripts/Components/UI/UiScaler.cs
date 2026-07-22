using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiScaler : MonoBehaviour
{
    public bool basedOnHeight = true;
    public Vector2 heightRange = new Vector2(1920,2360);
    public Vector2 heightScale = new Vector2(1,1.25f);

    public bool basedOnWidth;
    public Vector2 widthRange = new Vector2(1080, 2360);
    public Vector2 widthScale = new Vector2(1, 1.25f);



    void Start()
    {
        SetScale();
    }

    //This method will calculate and apply the scale on the self transform based on the defined properties 
    void SetScale()
    {
        if (basedOnHeight) 
        {
            float val = Mathf.Lerp(heightScale.x, heightScale.y, Mathf.InverseLerp(heightRange.x, heightRange.y, Screen.height));
            transform.localScale = new Vector3(val, val, 1);
        }


        if (basedOnWidth)
        {
            float val = Mathf.Lerp(widthScale.x, widthScale.y, Mathf.InverseLerp(widthRange.x, widthRange.y, Screen.width));
            transform.localScale = new Vector3(val, val, 1);
        }

        
    }
}
