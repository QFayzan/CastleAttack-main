using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFade : MonoBehaviour
{
    public float time = 1;
    public float defaultVal;
    public float fadeVal = 1;
    private Image image;
    private Text text;

    void OnEnable()
    {
        if (image == null) { image = GetComponent<Image>(); }
        if (text == null) { text = GetComponent<Text>(); }

        if (image != null)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, defaultVal);
            image.DOFade(fadeVal, time);
        }

        if (text != null)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, defaultVal);
            text.DOFade(fadeVal, time);
        }
    }

    public void FadeIn()
    {
        if (image != null)
        {
            image.DOFade(0, 1);
        }

        if (text != null)
        {
            text.DOFade(0, 1);
        }
    }

    public void FadeOut()
    {
        if (image != null)
        {
            image.DOFade(1, 1);
        }

        if (text != null)
        {
            text.DOFade(1, 1);
        }
    }

}
