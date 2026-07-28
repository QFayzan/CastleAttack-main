using UnityEngine;
using UnityEngine.UI;

public class WeaponAttributes : MonoBehaviour
{
    public string fighterID;
    public string weaponID;

    public bool canAttack = true;
    public bool aimShootWeapon;
    public bool physicsProjectileWeapon;


    public AudioClip walkSound;
    public float walkSoundVolume;
    public AudioClip runSound;
    public float runSoundVolume;


    public float attackInterval = 1;
    public bool canCompleteAttack = true;

    public float camShakeDuration = .2f;
    public float camShakeStrength = .1f;

    public Transform weaponSelectBtn;


    [HideInInspector]
    public float playerShootRecordedTime;

    public void SelectWeapon()
    {
      
        WeaponManager.ins.activeWeapon = this;

        for (int i = 0; i < WeaponManager.ins.weaponSelectBtns.childCount; i++)
        {
            WeaponManager.ins.weaponSelectBtns.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.white;
        }

        weaponSelectBtn.GetChild(0).GetComponent<Image>().color = Color.green;


        if (weaponID == "Heli") 
        {
            TPSController.ins.fighter.isFlying = true;
        }
        else
        {
            TPSController.ins.fighter.isFlying = false;
        }
        if (weaponID == "NoWeapon") { TPSController.ins.fighter.activeFighterModel.DeselectWeapon();
       
        TPSController.ins.fighter.activeFighterModel.fighterAnims.PlayAnim("Idle");
        return; }

        TPSController.ins.SelectWeapon(fighterID,weaponID);


        return;


        if (weaponID == "Tank" || weaponID == "Mortar")
        {
            TPSController.ins.fighter.characterController.radius = 1;
        }
        else 
        {
            TPSController.ins.fighter.characterController.radius = .5f;
        }
       


        CameraController.ins.SetCamTarget(CameraController.ins.camDefaultTarget);

        /*
        string animTypePlaying = TPSController.ins.anims.activeAnimType;


        WeaponManager.ins.attackBtn.SetActive(canAttack);
        WeaponManager.ins.weaponAim.SetActive(aimShootWeapon);

        

        PlayerAttrsManager.ins.ApplyPlayerAttrsBasedOnAnim(weaponID, animTypePlaying);

        //we wanna switch animation on weapon switch only when character is idle, walking or running 
        //if weapon is switched while jumping then when character lands JumpCoroutine() in TPSController plays the right weapon animation
        //if weapon is switched while shooting then Attack() of WeaponManager plays the right weapon animation afetr attacked is stopped

        if (animTypePlaying == "Attack")
        {
            if (!canAttack)
            {
                WeaponManager.ins.StopAttack();
                TPSController.ins.Idle();
            }
        }
        else 
        {
            //if (TPSController.ins.anims.IsLoopAnimPlaying()) { TPSController.ins.anims.PlayActiveAnim(); }

            TPSController.ins.anims.PlayActiveAnim();
        }*/

        
    }
}
