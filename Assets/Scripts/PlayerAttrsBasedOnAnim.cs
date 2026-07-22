using UnityEngine;

public class PlayerAttrsBasedOnAnim : MonoBehaviour
{
    public string weaponID;
    public string animType;

    public Transform camTarget;
    public bool lookForwardOnPlay;
    public bool lookForwardLock;

    public GameObject[] objsToActivate;
    public GameObject[] objsToDeactivate;
}
