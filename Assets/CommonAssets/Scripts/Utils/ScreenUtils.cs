using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class ScreenUtils
{
    private static Coroutine activateScreenCoroutine;

    public static void ActivateScreen(GameObject screenToDeactivate, GameObject screenToActivate, Action onScreenDeactivate = null, Action onScreenActivate = null)
    {
        if (activateScreenCoroutine != null) { GameSession.ins.StopCoroutine(activateScreenCoroutine); }
        activateScreenCoroutine = GameSession.ins.StartCoroutine(ActivateScreenCoroutine(screenToDeactivate, screenToActivate, onScreenDeactivate, onScreenActivate));
    }

    public static void ActivateScreen(GameObject screenToActivate, Action onScreenActivate = null)
    {
        if (activateScreenCoroutine != null) { GameSession.ins.StopCoroutine(activateScreenCoroutine); }
        activateScreenCoroutine = GameSession.ins.StartCoroutine(ActivateScreenCoroutine(null, screenToActivate, null, onScreenActivate));
    }

    static IEnumerator ActivateScreenCoroutine(GameObject screenToDeactivate, GameObject screenToActivate, Action onScreenDeactivate, Action onScreenActivate)
    {
        if (screenToDeactivate != null)
        {
            TopMenu.ins.Hide();
            
            Animation animation = screenToDeactivate.GetComponent<Animation>();
            if (animation != null)
            {
                float val = animation.clip.length;
                animation.Play();
                while (val > 0)
                {
                    val -= Time.deltaTime;
                    if (val < 0) { val = 0; }
                    animation[animation.clip.name].time = val;
                    yield return null;
                }
            }

            screenToDeactivate.SetActive(false);
            if (onScreenDeactivate != null) { onScreenDeactivate.Invoke(); }
        }


        if (screenToActivate != null)
        {
            TopMenu.ins.Appear();

            screenToActivate.SetActive(true);
            if (onScreenActivate != null) { onScreenActivate.Invoke(); }
            Animation animation = screenToActivate.GetComponent<Animation>();
            if (animation != null)
            {
                animation.Play();
                yield return new WaitForSeconds(animation.clip.length);
            }
        }
    }
}
