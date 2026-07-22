using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TakeCoverMode : MonoBehaviour
{
    public Vector2 camRotateLeftLimits;
    public Vector2 camRotateRightLimits;

    public Transform referenceLeft, referenceCenter, referenceRight;

    public bool leftAttack, rightAttack;

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            StartCoroutine("CheckIfCharacterIsFacingFront", other.GetComponent<Fighter>());
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") 
        {
            StopCoroutine("CheckIfCharacterIsFacingFront");
            
            GameplayScreen.ins.takeCoverBtn.gameObject.SetActive(false);
            GameplayScreen.ins.onTakeCoverClicked = null;
        }
    }


    IEnumerator CheckIfCharacterIsFacingFront(Fighter fighter)
    {
        while (true)
        {
            if (Mathf.Abs(Mathf.DeltaAngle(CameraController.ins.cam.eulerAngles.y, referenceCenter.eulerAngles.y)) <= 70)
            {
                GameplayScreen.ins.attackFromLeftBtn.gameObject.SetActive(false);
                GameplayScreen.ins.attackFromRightBtn.gameObject.SetActive(false);
                GameplayScreen.ins.takeCoverBtn.gameObject.SetActive(true);
                GameplayScreen.ins.takeCoverBtn.interactable = true;

                GameplayScreen.ins.onTakeCoverClicked = PlayerEnterTakeCoverMode;
            }
            else 
            {
                GameplayScreen.ins.takeCoverBtn.gameObject.SetActive(false);
                GameplayScreen.ins.onTakeCoverClicked = null;
            }
            yield return null;
        }
    }




    public void PlayerEnterTakeCoverMode()
    {
        StopCoroutine("CheckIfCharacterIsFacingFront");

        for (int i = 0; i < GameplayScreen.ins.weaponBtns.childCount; i++) 
        {
            WeaponBtn weaponBtn = GameplayScreen.ins.weaponBtns.GetChild(i).GetComponent<WeaponBtn>();
            if (weaponBtn.weaponID != "Pistol" && weaponBtn.weaponID != "Rifle") { weaponBtn.btn.interactable = false; }
        }
        
        
        TPSController.ins.fighter.takenCover = true;

        TPSController.ins.onJoystickDown += PlayerExitTakeCoverMode;
        TPSController.ins.onJumpBtnPressed += PlayerExitTakeCoverMode;

        GameplayScreen.ins.attackFromLeftBtn.gameObject.SetActive(true);
        GameplayScreen.ins.attackFromRightBtn.gameObject.SetActive(true);
        GameplayScreen.ins.takeCoverBtn.gameObject.SetActive(true);

        GameplayScreen.ins.onAttackFromLeftClicked = PlayerPeekFromLeft;
        GameplayScreen.ins.onTakeCoverClicked = PlayerStopPeeking;
        GameplayScreen.ins.onAttackFromRightClicked = PlayerPeekFromRight;

        GameplayScreen.ins.pistolBtn.SelectWeapon();

        /*if (TPSController.ins.fighter.activeFighterModel.fighterID == "Stubby")
        {
            if (!TPSController.ins.fighter.GetActiveWeapon().takeCoverModeEligible) { GameplayScreen.ins.pistolBtn.SelectWeapon(); }
        }
        else { GameplayScreen.ins.pistolBtn.SelectWeapon(); }*/

        TPSController.ins.fighter.activeFighterModel.StopLayerAnimations(1);
        


        TPSController.ins.fighter.characterController.enabled = false;

        
        CameraController.ins.cameraParent.transform.localEulerAngles = new Vector3(-10, 0, 0);



        PlayerStopPeeking();

        StartCoroutine("CamRotLimits");

    }

    public void PlayerExitTakeCoverMode()
    {
        for (int i = 0; i < GameplayScreen.ins.weaponBtns.childCount; i++)
        {
            WeaponBtn weaponBtn = GameplayScreen.ins.weaponBtns.GetChild(i).GetComponent<WeaponBtn>();
            weaponBtn.btn.interactable = true; 
        }

        TPSController.ins.fighter.takenCover = false;

        TPSController.ins.fighter.transform.parent = null;

        TPSController.ins.onJoystickDown -= PlayerExitTakeCoverMode;
        TPSController.ins.onJumpBtnPressed -= PlayerExitTakeCoverMode;

        GameplayScreen.ins.onAttackFromLeftClicked = null;
        GameplayScreen.ins.onTakeCoverClicked = null;
        GameplayScreen.ins.onAttackFromRightClicked = null;

        GameplayScreen.ins.attackFromLeftBtn.gameObject.SetActive(false);
        GameplayScreen.ins.attackFromRightBtn.gameObject.SetActive(false);
        GameplayScreen.ins.takeCoverBtn.gameObject.SetActive(false);

        TPSController.ins.fighter.characterController.enabled = true;


        GameplayScreen.ins.weaponAim.transform.localPosition = new Vector3(235, GameplayScreen.ins.weaponAim.transform.localPosition.y, 0);
        CameraController.ins.cameraParent.transform.localPosition = new Vector3(0, CameraController.ins.cameraParent.localPosition.y, 0);
        CameraController.ins.colliderImage.gameObject.SetActive(true);

        TPSController.ins.fighter.fighterModels.localScale = new Vector3(1, 1, 1);

        CameraController.ins.cameraParent.transform.localEulerAngles = new Vector3(-10, 0, 0);
        GameplayScreen.ins.attackBtn.SetActive(true);

        TPSController.ins.SelectWeapon(TPSController.ins.fighter.activeFighterModel.fighterID, TPSController.ins.fighter.activeFighterModel.activeWeapon.weaponID);

        StopCoroutine("CamRotLimits");

    }

    public void PlayerPeekFromLeft()
    {

        GameplayScreen.ins.attackBtn.SetActive(true);

        GameplayScreen.ins.attackFromLeftBtn.interactable = (false);
        GameplayScreen.ins.takeCoverBtn.interactable = (true);
        GameplayScreen.ins.attackFromRightBtn.interactable = (rightAttack);

        SnapToReference(TPSController.ins.fighter.transform, referenceLeft);

        if (TPSController.ins.fighter.GetActiveWeapon().shootingWeapon.aimShooting)
        {
            GameplayScreen.ins.weaponAim.SetActive(true);
            GameplayScreen.ins.weaponAim.transform.localPosition = new Vector3(-235, GameplayScreen.ins.weaponAim.transform.localPosition.y, 0);
        }

        CameraController.ins.cameraParent.localPosition = new Vector3(-1.5f, CameraController.ins.cameraParent.localPosition.y, 0);

        CameraController.ins.colliderImage.gameObject.SetActive(true);

        TPSController.ins.fighter.fighterModels.localEulerAngles = new Vector3(0, -25, 0);

        TPSController.ins.fighter.fighterModels.localScale = new Vector3(-1,1,1);

        TPSController.ins.fighter.activeFighterModel.fighterAnims.PlayAnim("Peek");
    }

    public void PlayerPeekFromRight()
    {
        GameplayScreen.ins.attackBtn.SetActive(true);

        GameplayScreen.ins.attackFromLeftBtn.interactable = (leftAttack);
        GameplayScreen.ins.takeCoverBtn.interactable = (true);
        GameplayScreen.ins.attackFromRightBtn.interactable = (false);

        SnapToReference(TPSController.ins.fighter.transform, referenceRight);

        if (TPSController.ins.fighter.GetActiveWeapon().shootingWeapon.aimShooting) 
        {
            GameplayScreen.ins.weaponAim.SetActive(true);
            GameplayScreen.ins.weaponAim.transform.localPosition = new Vector3(235, GameplayScreen.ins.weaponAim.transform.localPosition.y, 0);
        }

        CameraController.ins.cameraParent.transform.localPosition = new Vector3(1, CameraController.ins.cameraParent.localPosition.y, 0);

        CameraController.ins.colliderImage.gameObject.SetActive(true);


        TPSController.ins.fighter.fighterModels.localEulerAngles = new Vector3(0,20,0);

        TPSController.ins.fighter.fighterModels.localScale = new Vector3(1, 1, 1);

        TPSController.ins.fighter.activeFighterModel.fighterAnims.PlayAnim("Peek");
    }

    public void PlayerStopPeeking()
    {
        GameplayScreen.ins.attackBtn.SetActive(false);

        SnapToReference(TPSController.ins.fighter.transform, referenceCenter);


        GameplayScreen.ins.attackFromLeftBtn.interactable = (leftAttack);
        GameplayScreen.ins.takeCoverBtn.interactable = (false);
        GameplayScreen.ins.attackFromRightBtn.interactable = (rightAttack);

        GameplayScreen.ins.weaponAim.SetActive(false);

        CameraController.ins.cameraParent.localPosition = new Vector3(0, CameraController.ins.cameraParent.localPosition.y, 0);

        TPSController.ins.fighter.fighterModels.localEulerAngles = Vector3.zero;

        CameraController.ins.colliderImage.gameObject.SetActive(false);

        TPSController.ins.fighter.activeFighterModel.PlayIdleAnim();
    }



    public IEnumerator CamRotLimits()
    {
        while (true)
        {
            Transform player = TPSController.ins.fighter.transform;
            Vector3 ang = player.localEulerAngles;
            ang.y = NormalizeAngle(player.localEulerAngles.y);
            //Debug.Log(ang);

            if (referenceLeft.childCount > 0) 
            {
                if (ang.y < camRotateLeftLimits.x) { player.localEulerAngles = new Vector3(ang.x, camRotateLeftLimits.x, 0); }
                if (ang.y > camRotateLeftLimits.y) { player.localEulerAngles = new Vector3(ang.x, camRotateLeftLimits.y, 0); }
            }


            if (referenceRight.childCount > 0)
            {
                if (ang.y < camRotateRightLimits.x) { player.localEulerAngles = new Vector3(ang.x, camRotateRightLimits.x, 0); }
                if (ang.y > camRotateRightLimits.y) { player.localEulerAngles = new Vector3(ang.x, camRotateRightLimits.y, 0); }
            }

            yield return null;
        }

        
    }

    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f)
            angle -= 360f;
        return angle;
    }

    void SnapToReference(Transform fighter, Transform reference)
    {
        fighter.parent = reference;
        fighter.localPosition = new Vector3(0, fighter.localPosition.y, 0) ;
        fighter.localEulerAngles = Vector3.zero;
    }
}
