using UnityEngine;

public class PlayerAttrsManager : MonoBehaviour
{
    public static PlayerAttrsManager ins;
    void Awake() { ins = this; }

    public Transform playerAttrsParent;
    public Transform animsParent;

    public GameObject[] objsToActivate;
    public GameObject[] objsToDeativate;




    public PlayerAttrsBasedOnAnim GetPlayerAttrsBasedOnAnim(string weaponID, string animType)
    {
        for (int i = 0; i < playerAttrsParent.childCount; i++)
        {
            PlayerAttrsBasedOnAnim playerAttrs = playerAttrsParent.GetChild(i).GetComponent<PlayerAttrsBasedOnAnim>();
            if (playerAttrs.weaponID == weaponID && playerAttrs.animType == animType)
            {
                return playerAttrs;
            }
        }

        return null;
    }

    public PlayerAttrsBasedOnAnim GetPlayerAttrsBasedOnActiveWeaponAnim(string animType)
    {
        for (int i = 0; i < playerAttrsParent.childCount; i++)
        {
            PlayerAttrsBasedOnAnim playerAttrs = playerAttrsParent.GetChild(i).GetComponent<PlayerAttrsBasedOnAnim>();
            if (playerAttrs.weaponID == WeaponManager.ins.activeWeapon.weaponID && playerAttrs.animType == animType)
            {
                return playerAttrs;
            }
        }

        return null;
    }


    public void ApplyPlayerAttrsBasedOnAnim(string weaponID, string animType)
    {
        for (int i = 0; i < objsToActivate.Length; i++)  { objsToActivate[i].SetActive(true); }
        for (int i = 0; i < objsToDeativate.Length; i++) { objsToDeativate[i].SetActive(false); }

        PlayerAttrsBasedOnAnim playerAttrs = GetPlayerAttrsBasedOnAnim(weaponID, animType);

        if (playerAttrs == null) { return; }

        for (int i = 0; i < playerAttrs.objsToActivate.Length; i++)   { playerAttrs.objsToActivate[i].SetActive(true); }
        for (int i = 0; i < playerAttrs.objsToDeactivate.Length; i++) { playerAttrs.objsToDeactivate[i].SetActive(false); }

        if (playerAttrs.camTarget != null) { CameraController.ins.SetCamTarget(playerAttrs.camTarget); }

        if (playerAttrs.lookForwardOnPlay) { animsParent.localEulerAngles = Vector3.zero; }
    }



}
