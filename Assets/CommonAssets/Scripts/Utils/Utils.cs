using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
    public static void InvokeDelayedAction(float delay, Action action)
    {
        GameSession.ins.StartCoroutine(DelayedActionCoroutine(delay, action));
    }

    static IEnumerator DelayedActionCoroutine(float delay, Action action)
    {
        if (delay == 0) { yield return new WaitForEndOfFrame(); }
        if (delay > 0)  { yield return new WaitForSeconds(delay); }
        action?.Invoke(); 
    }

    public static void FrameDelayedAction(Action action, int frames = 1)
    {
        GameSession.ins.StartCoroutine(FrameDelayedActionCoroutine(action, frames));
    }

    static IEnumerator FrameDelayedActionCoroutine(Action action, int frames = 1)
    {
        while (frames > 0)
        {
            frames -= 1;
            yield return new WaitForEndOfFrame();
        }
        action?.Invoke(); 
    }
	
	public static T ParseEnum<T>(string value) { return (T)Enum.Parse(typeof(T), value, true); }

    private static float hapticRecordedTime = 0;
    public static void ContinuousHaptic(float delay)
    {
        Taptic.tapticOn = true;
        if (Time.realtimeSinceStartup - hapticRecordedTime < delay) { return; }
        hapticRecordedTime = Time.realtimeSinceStartup;
        Debug.Log("ContinuousHaptic");
        Taptic.Medium();
    }

    static Coroutine hapticCoroutine = null;
    public static void StartHaptic(float duration, float delay = .25f) { hapticCoroutine = GameSession.ins.StartCoroutine(HapticCoroutine(duration, delay)); }
    static IEnumerator HapticCoroutine(float duration, float delay)
    {
        Taptic.tapticOn = true;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            ContinuousHaptic(delay);
            yield return null;
        }
    }
    public static void StopHaptic() { GameSession.ins.StopCoroutine(hapticCoroutine); }




    public static void MoveTo(Transform subject, Transform target, Vector3 targetOffset, float duration, float height, Action onComplete = null)
    {
        GameSession.ins.StartCoroutine(MoveToRoutine(subject, target, targetOffset, duration, height, onComplete));
    }


    private static IEnumerator MoveToRoutine(Transform subject, Transform target, Vector3 targetOffset, float duration, float height, Action onComplete)
    {
        Vector3 start = subject.position;
        float time = 0f;
        
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            Vector3 end = target.position + targetOffset;

            // Control point (this creates the arc)
            Vector3 control = (start + end) * 0.5f + Vector3.up * height;

            // Quadratic Bezier formula
            Vector3 pos =
                Mathf.Pow(1 - t, 2) * start +
                2 * (1 - t) * t * control +
                Mathf.Pow(t, 2) * end;

            subject.position = pos;

            yield return null;
        }

        subject.position = target.position + targetOffset;
        onComplete?.Invoke();
    }

}
