using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler , IPointerUpHandler
{
    public static Joystick ins;
    void Awake() { ins = this; }

    public float innerRadius = 132.5f;
    public float outerRadius = 150;
    public Transform joystick;
    public Image innerBg;
    public Image outerBg;

    public Action onJoystickHalfDown;
    public Action onJoystickFullDown;
    public Action<Vector3,bool> onJoystick;
    public Action onJoystickUp;


    private Transform joystickHelper;
    private bool joystickDown;

    [HideInInspector]
    public bool joystickOnFullIntensity;

    void Start()
    {
        joystickHelper = new GameObject().transform;
        joystickHelper.parent = transform;
        joystickHelper.localPosition = Vector3.zero;
    }

    void Update()
    {
        if (joystickDown) 
        {

            float dragLength = Vector3.Distance(Vector3.zero, joystickHelper.localPosition);
            if (!joystickOnFullIntensity && dragLength > innerRadius) 
            {
                joystickOnFullIntensity = true;
                outerBg.color = new Color(outerBg.color.r, outerBg.color.g, outerBg.color.b, 1);
                onJoystickFullDown?.Invoke();
            }

            if (joystickOnFullIntensity && dragLength < innerRadius)
            {
                joystickOnFullIntensity = false;
                outerBg.color = new Color(outerBg.color.r, outerBg.color.g, outerBg.color.b, .25f);
                onJoystickHalfDown?.Invoke();
            }

            Vector3 normalizedVal = (joystick.localPosition / outerRadius).normalized;
            Vector3 dir = new Vector3(-normalizedVal.y, 0, normalizedVal.x);
            onJoystick?.Invoke(dir, joystickOnFullIntensity); 
        }
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        innerBg.color = new Color(innerBg.color.r, innerBg.color.g, innerBg.color.b, 1);
        onJoystickHalfDown?.Invoke();

        joystickOnFullIntensity = false;
        joystickDown = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        joystickHelper.Translate(eventData.delta);

        Vector3 recPos = joystick.localPosition;
        joystick.localPosition = joystickHelper.localPosition;

        if (Vector3.Distance(Vector3.zero, joystickHelper.localPosition) > outerRadius)
        { 
            joystick.localPosition = Vector3.Normalize(joystickHelper.localPosition)* outerRadius; 
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.localPosition = Vector3.zero;
        joystickHelper.localPosition = Vector3.zero;

        innerBg.color = new Color(innerBg.color.r, innerBg.color.g, innerBg.color.b, .25f);
        outerBg.color = new Color(outerBg.color.r, outerBg.color.g, outerBg.color.b, .25f);
        onJoystickUp?.Invoke();

        joystickDown = false;
    }


}
