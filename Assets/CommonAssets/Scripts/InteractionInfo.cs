using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionInfo : MonoBehaviour
{
    public UnityEvent onCollisionEnterEv, onCollisionExitEv, onTriggerEnterEv, onTriggerExitEv;    



    public Action<Collider> onTriggerEnter;
    public Action<Collider> onTriggerExit;

    public Action<Collision> onCollisionEnter;
    public Action<Collision> onCollisionExit;

    private void OnTriggerEnter(Collider collider) { onTriggerEnterEv.Invoke(); onTriggerEnter?.Invoke(collider); }
    private void OnTriggerExit(Collider collider) { onTriggerExitEv.Invoke(); onTriggerExit?.Invoke(collider); }
    private void OnCollisionEnter(Collision collision) { onCollisionEnter?.Invoke(collision); }
    private void OnCollisionExit(Collision collision) { onCollisionExit?.Invoke(collision); }
}
